using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.Security;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS;



/// <summary>
/// Represents the main control of the CMS component, which is included in the desired page on the web site where component should be integrated.
/// The control inherits most of its functionality from <see cref="Melon.Components.CMS.ComponentEngine.InnerBaseCMSControl"/> and is a critical part of the CMS component engine.
/// In short this control manages all the logic flow and by using a system of events decides how to proceed on every step. This is deeply connected with the usage of dynamic control load, which is implemented here.
/// </summary>
/// <remarks>
///		<para>
///			Basic workflow of the CMS component engine:
///			<list type="bullet">
///				<listheader>
///					As a general rule in order events and data to be maintained between page recycling the state of the controls (dynamic or not) should be preserved. While in general ViewState helps with the static controls, for dynamic a little additional effort is needed – they must be recreated at proper time so the asp.net engine can match their ViewState from the previous page iteration.
///					In the suggested algorithm the following system was chosen:
///				</listheader>
///				<item>Before load of the ViewState of this a special refresh function is called which recreates the controls.</item>
///				<item>Since the refresh function should somehow know which controls to recreate 
///                 and the ViewState is obviously not the place for that information, the control state is used instead. 
///                 This is simply because the control state is loaded before the viewstate.</item>
///				<item>Moreover a special structure is needed to store the info about the controls in the control state. 
///             That structure contains the control file name (in case of web control) or the control class (in case of custom control). 
///             Because these child controls hold some data which depends on the other controls, a special init blocks are needed so the control will match its state from the previous page request. 
///             These init blocks are implemented as instance methods Initializer of the user controls. 
///             Finally Initializer method has arguments with real values. These values are the secind part of this <see cref="Melon.Components.CMS.ComponentEngine.ControlInitInfo">ControlInitInfo</see> class.</item>
///				<item>The above structure is stored in the control state and being read between cycles by the <see cref="Melon.Components.CMS.ComponentEngine.InnerBaseCMSControl.Refresh">Refresh</see> function. 
///				This function on the other hand calls the function <see cref="Melon.Components.CMS.ComponentEngine.InnerBaseCMSControl.LoadCustomControl(Melon.Components.CMS.ComponentEngine.ControlInitInfo[])">LoadCustomControl</see> which actually cares to load the controls and call the init blocks with their argument values.</item>
///				<item>The main control(CMS) inherits <see cref="Melon.Components.CMS.ComponentEngine.BaseCMSControl"/> which defines a set of delegates, events and methods which provide the communication system between the controls. 
///				Every event describes a specific action within the component. This action can happen in each of the child controls. 
///				Moreover every event has a public fire method, which allows the chibld controls to initiate the execution of attached event handler(s). 
///				All the handling of the events is done in this via handling its base’s events.</item>
///				<item>This parent class <see cref="Melon.Components.CMS.ComponentEngine.BaseCMSControl">BaseCMSCOntrol</see> inherits <see cref="Melon.Components.CMS.ComponentEngine.InnerBaseCMSControl">InnerBaseCMSControl</see> class where all the control state functionality is implemented.</item>
///				<item>On the other hand all child controls inherit <see cref="Melon.Components.CMS.ComponentEngine.CMSControl"/> class, which defines the following set of features:
///					<list type="bullet">
///						<item>Property defining the parent control, which because of the above and the run-time creation of web controls should be the BaseNewsControl (this explains why this control handles its base events).</item>
///						<item>Implementation of IsControlPostBack feature, indicating that the control was loaded before and if the postback occurred by it.</item>
///					</list>
///				</item>
///			</list>
///		</para>
/// </remarks>
/// <seealso cref="Melon.Components.CMS.ComponentEngine.BaseCMSControl"/>
/// <seealso cref="Melon.Components.CMS.ComponentEngine.InnerBaseCMSControl"/>
/// <seealso cref="Melon.Components.CMS.ComponentEngine.CMSControl"/>
/// <seealso cref="Melon.Components.CMS.ComponentEngine.ControlInitInfo"/>
public partial class CMS:BaseCMSControl
{

    #region Fields & Properties

    /// <summary>
    /// <para>
    ///     Flag for order of CMS panel controls.
    /// </para>
    /// <para>
    ///     The CMS panels are one or two in given moment.
    ///     When they are two it depends on flag IsVertical whether they will be displayed horizontal on one row,
    ///     or vertical on two rows. 
    /// </para>
    /// </summary>
    public bool IsVertical
    {
        get 
        { 
            if (ViewState["__mc_cms_IsVertical"] != null)
            {
                return Convert.ToBoolean(ViewState["__mc_cms_IsVertical"]);
            }
            else
            {
                return true;
            }
        }
        set 
        {
           ViewState["__mc_cms_IsVertical"] = value; 
        }
    }

    #endregion

    /// <summary>
    /// Attach event handlers to the controls' events and CMS events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/02/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.lbtnOpenExplorer.Click += new EventHandler(lbtnOpenExplorer_Click);
        this.lbtnOpenTemplates.Click += new EventHandler(lbtnOpenTemplates_Click);
        this.lbtnOpenUsers.Click += new EventHandler(lbtnOpenUsers_Click);
        this.lbtnReturnFromPreview.Command += new System.Web.UI.WebControls.CommandEventHandler(lbtnReturnFromPreview_Command);

        this.LoadWorkAreaEvent += new LoadWorkAreaEventHandler(LoadWorkArea);
        this.LoadPagePreviewEvent += new LoadPagePreviewEventHandler(LoadPagePreview);
        this.LoadTemplateListEvent += new LoadTemplateListEventHandler(LoadTemplateList);
        this.LoadTemplateAddEditEvent += new LoadTemplateAddEditEventHandler(LoadTemplateAddEdit);
        this.LoadAccessDeniedEvent += new LoadAccessDeniedEventHandler(LoadAccessDenied);
        this.RemoveCMSControlEvent += new RemoveCMSControlEventHandler(RemoveCMSControl);
        this.DisplayErrorMessageEvent += new DisplayErrorMessageEventHandler(DisplayErrorMessage);

        this.LoadUsersListEvent+=new LoadUsersListEventHandler(LoadUsersList);

        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.   
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Check the current logged user. If there is no such user then redirect to login page.
    ///         If there is logged user but it is not CMS user "Access is denied" is displayed.
    ///     </para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/02/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {        
        lblErrorMessage.Text = "";

        if (!Page.IsPostBack)
        {
            imgTransparent.DataBind();
            CurrentUser = User.Load();

            if (CurrentUser == null)
            {
                RedirectToLoginPage();
            }
            else if (CurrentUser.IsCMSUser())
            {
                //The currently logged user is CMS user. Display screen CMS Explorer.
                lbtnOpenExplorer_Click(sender, e);

                if (CurrentUser.IsInCMSRole(CMSRole.Writer))
                {
                    //For users from role Writer menu nodes Templates and Users are not visible. 
                    tdOpenTemplates.Visible = false;
                    tdOpenUsers.Visible = false;
                }
                else if (CurrentUser.IsInCMSRole(CMSRole.Administrator))
                {
                    //For users from role Administrator menu node Templates is not visible. 
                    tdOpenTemplates.Visible = false;
                }
            }
            else
            {
                //The currently logged user is not CMS user. Display screen Access Denied and hide menu.

                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole=true;
                args.UserRole= CMSRole.None;
                LoadAccessDenied(sender,args);
            }
        }
        else
        {
            //Check whether there is currently logged user because CMS component is accessible only for logged user.
            if (CurrentUser == null)
            {
                RedirectToLoginPage();
            }
        }
    }


    /// <summary>
    ///Event handler for event Click on button lbtnOpenExplorer.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         It selects menu item "CMS Explorer" in the CMS menu and calls 
    ///         method <see cref="Melon.Components.CMS.ComponentEngine.BaseCMSControl.OnLoadWorkAreaEvent"/>.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/02/2008</date>
    protected void lbtnOpenExplorer_Click(object sender, EventArgs e)
    {
        if (this.CurrentUser != null)
        {
            IsVertical = false;

            lbtnReturnFromPreview.Visible = false;

            //Select in menu, menu item: "CMS Explorer".
            tdOpenExplorer.Attributes.Add("class", "mc_cms_menu_item_explorer_selected");
            tdOpenTemplates.Attributes.Add("class", "mc_cms_menu_item_template");
            tdOpenUsers.Attributes.Add("class", "mc_cms_menu_item_users");

            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.RefreshExplorer = true;
            this.OnLoadWorkAreaEvent(sender, args);
        }
        else
        {
            RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Event handler for event Click on button lbtnOpenTemplates.
    /// </summary>
    /// <remarks>
    ///     <para>It selects menu item "Templates" in the CMS menu and calls 
    ///         method <see cref="Melon.Components.CMS.ComponentEngine.BaseCMSControl.OnLoadTemplateListEvent"/>.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/02/2008</date>
    protected void lbtnOpenTemplates_Click(object sender, EventArgs e)
    {
        LoadTemplateList(sender, new LoadTemplateListEventArgs());
    }

    /// <summary>
    /// Event handler for event Click on button lbtnOpenUsers.
    /// </summary>
    /// <remarks>
    ///     <para>It selects menu item "Users" in the CMS menu and loads user control UserList.ascx.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/02/2008</date>
    protected void lbtnOpenUsers_Click(object sender, EventArgs e)
    {
        LoadUsersList(sender, e);
    }

    /// <summary>
    /// Event handler for event Command on button lbtnReturnFromPreview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>27/05/2008</date>
    protected void lbtnReturnFromPreview_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        if (this.CurrentUser != null)
        {
            IsVertical = false;

            tdOpenExplorer.Visible = true;
            tdOpenTemplates.Visible = true;
            tdOpenUsers.Visible = true;
            lbtnReturnFromPreview.Visible = false;

            //Select in menu, menu item: "CMS Explorer".
            tdOpenExplorer.Attributes.Add("class", "mc_cms_menu_item_explorer_selected");
            tdOpenTemplates.Attributes.Add("class", "mc_cms_menu_item_template");
            tdOpenUsers.Attributes.Add("class", "mc_cms_menu_item_users");

            string[] command_args = e.CommandArgument.ToString().Split(';');
            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.NodeId = Convert.ToInt32(command_args[0]); //page id
            args.SelectedTab = (WorkAreaTabs)Convert.ToInt32(command_args[1]);//selected tab before the preview

            LoadWorkArea(sender, args);
        }
        else
        {
            RedirectToLoginPage();
        }
    }


    /// <summary>
    /// Loads user controls Explorer.ascx and WorkArea.ascx.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected void LoadWorkArea(object sender, LoadWorkAreaArgs e)
    {
        int? selectedNode = (e.NodeId == null)? e.NodeParentId : e.NodeId ; 
        ControlInitInfo cntrlExplorerInitInfo = new ControlInitInfo("Explorer.ascx", e.RefreshExplorer, new object[] { selectedNode });

        ControlInitInfo cntrlWorkAreaInitInfo = new ControlInitInfo("WorkArea.ascx", true, new object[] { e.ExplorerNodeType, e.NodeId, e.NodeParentId, e.NodeLocation, e.SelectedTab });
        LoadCustomControl(new ControlInitInfo[] { cntrlExplorerInitInfo, cntrlWorkAreaInitInfo });
    }

    /// <summary>
    /// Loads user control PagePreview.ascx. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>26/05/2008</date>
    protected void LoadPagePreview(object sender, LoadPagePreviewArgs e)
    {
        IsVertical = true;

        tdOpenExplorer.Visible = false;
        tdOpenTemplates.Visible = false;
        tdOpenUsers.Visible = false;
        lbtnReturnFromPreview.Visible = true;

        //The command argument is semi-column separated list of page id, page code, selected tab (before the preview).
        lbtnReturnFromPreview.CommandArgument = e.PageId.ToString() + ";" + Convert.ToString(Convert.ToInt32(e.SelectedTab));

        ControlInitInfo cntrlPagePreview = new ControlInitInfo("PagePreview.ascx", false, new object[] {e.Version,e.PageId,e.Url });
        LoadCustomControl(new ControlInitInfo[] { cntrlPagePreview });
    }

    /// <summary>
    /// Loads user control TemplateList.ascx.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void LoadTemplateList(object sender, LoadTemplateListEventArgs e)
    {
        if (this.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator)
            {
                IsVertical = true;

                lbtnReturnFromPreview.Visible = false;

                //Select menu item "Templates".
                tdOpenExplorer.Attributes.Add("class", "mc_cms_menu_item_explorer");
                tdOpenTemplates.Attributes.Add("class", "mc_cms_menu_item_template_selected");
                tdOpenUsers.Attributes.Add("class", "mc_cms_menu_item_users");

                ControlInitInfo cntrlTemplateList = new ControlInitInfo("TemplateList.ascx", true, null);
                LoadCustomControl(new ControlInitInfo[] { cntrlTemplateList });
            }
            else
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole = false;
                args.UserRole = currentUserRole;
                LoadAccessDenied(sender, args);
            }
        }
        else
        {
            RedirectToLoginPage();
        }          
    }

    protected void LoadUsersList(object sender, EventArgs e)
    {
        if (this.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator || currentUserRole == CMSRole.Administrator)
            {
                IsVertical = true;

                lbtnReturnFromPreview.Visible = false;

                //Select menu item "Users".
                tdOpenExplorer.Attributes.Add("class", "mc_cms_menu_item_explorer");
                tdOpenTemplates.Attributes.Add("class", "mc_cms_menu_item_template");
                tdOpenUsers.Attributes.Add("class", "mc_cms_menu_item_users_selected");

                ControlInitInfo cntrlUserList = new ControlInitInfo("UserList.ascx", false, null);
                LoadCustomControl(new ControlInitInfo[] { cntrlUserList });
            }
            else
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole = false;
                args.UserRole = currentUserRole;
                LoadAccessDenied(sender, args);
            }
        }
        else
        {
            RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Loads user controls TemplateList.ascx and TemplateAddEdit.ascx.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void LoadTemplateAddEdit(object sender, LoadTemplateAddEditEventArgs e)
    {
        ControlInitInfo cntrlTemplateListInitInfo = new ControlInitInfo("TemplateList.ascx", false, null);

        ControlInitInfo cntrlTemplateAddEditInitInfo = new ControlInitInfo("TemplateAddEdit.ascx", true, new object[] { e.TemplateId });
        LoadCustomControl(new ControlInitInfo[] { cntrlTemplateListInitInfo, cntrlTemplateAddEditInitInfo });
    }

    /// <summary>
    /// Loads user control AccessDenied.ascx.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>01/07/2008</date>
    protected void LoadAccessDenied(object sender, LoadAccessDeniedEventArgs e)
    {
        divNavigation.Visible = false;

        ControlInitInfo cntrlCMSAccessDenied = new ControlInitInfo("AccessDenied.ascx", false, new object[] {e.UserRole,e.IsUserLoggedRole });
        LoadCustomControl(new ControlInitInfo[] { cntrlCMSAccessDenied });
    }

    /// <summary>
    /// Removes user control from the CMS panel where it is loaded.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void RemoveCMSControl(object sender, RemoveCMSControlEventArgs e)
    {
        RemoveCustomControl(e.ControlFile);
    }

    /// <summary>
    /// Display error message occurred in CMS user control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/06/2008</date>
    protected void DisplayErrorMessage(object sender, DisplayErrorMessageEventArgs e)
    {
        lblErrorMessage.Text = e.ErrorMessage;
    }

    /// <summary>
    /// Redirects to loginUrl page if specified otherwise load user control for access denied.
    /// </summary>
    /// <author></author>
    /// <date>01/10/2008</date>
    public override void RedirectToLoginPage()
    {
        if (String.IsNullOrEmpty(CMSSettings.LoginUrl) || this.ResolveUrl(CMSSettings.LoginUrl) == Request.RawUrl)
        {
            LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
            args.UserRole = CMSRole.None;
            args.IsUserLoggedRole = true;
            LoadAccessDenied(this, args);
        }
        else
        {
            Response.Redirect(CMSSettings.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(Request.RawUrl),true);
        }
    }
}

