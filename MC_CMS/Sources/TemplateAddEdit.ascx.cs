using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Exception;
using System.Web.Security;
using Melon.Components.CMS;


/// <summary>
/// Providers interface for creation and modification of template.
/// </summary>
/// <remarks>
///     <para>The TemplateAddEdit user control provides user interface for 
///     creating or updating templates which will be used for creation of content-manageable pages.
///     If field <see cref="TemplateId"/> contains template id then the control is in context of modifying existing template. 
///     If TemplateId is not set then new template will be created.
///     The control is using <see cref="Melon.Components.CMS.Template"/> class for saving the template settings.  
///     </para>
///     <para>
///     All web controls from TemplateAddEdit are using the local resources.
///     To customize them modify resource file TemplateAddEdit.resx placed in the MC_CMS folder.
///     </para>
///</remarks>
///<seealso cref="Template"/>
public partial class TemplateAddEdit : CMSControl
{
    #region Fields & Properties

    /// <summary>
    /// Identifier of template which will be modified.
    /// </summary>
    public int? TemplateId;

    #endregion


    /// <summary>
    /// Initializes the control's properties
    /// </summary>
    /// <param name="args">The values with which the properties will be initialized</param>
    public override void Initializer(object[] args)
    {
        this.TemplateId = (int?)args[0];
    }

    /// <summary>
    /// Attach event handlers for controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.btnSelectMasterPage.Click += new EventHandler(btnSelectMasterPage_Click);
        this.repPlaceholders.ItemDataBound += new RepeaterItemEventHandler(repPlaceholders_ItemDataBound);
        this.btnCancel.Click += new EventHandler(btnCancel_Click);
        this.btnSave.Click += new EventHandler(btnSave_Click);

        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     If the user control is loaded for first time then method <see cref="DisplayAllMasterPages"/>
    ///     is called to retrieve and display all existing master pages in the web site.
    ///     If the user control was displayed in context of edit template, 
    ///     method <see cref="LoadTemplateDetails"/> is called to load details of the template in the interface.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            divNoMasterPage.Visible = true;

            DisplayAllMasterPages();
            if (this.TemplateId != null)
            {
                LoadTemplateDetails(this.TemplateId.Value);
                divNoMasterPage.Visible = false;
            }

            this.btnSave.Attributes.Add("onfocus", "document.getElementById('" + this.txtName.ClientID + "').focus();");
            this.Page.SetFocus(this.btnSave);
        }
    }


    /// <summary>
    /// Event handler for event Click of Button btnSelectMasterPage.
    /// </summary>
    /// <remarks>
    ///     Get all content placeholders from the currently selected master page by calling method 
    ///     <see cref="Melon.Components.CMS.Template.GetMasterPagePlaceholders"/>
    ///     and display them in Repeater repPlaceholders.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/07/2008</date>
    protected void btnSelectMasterPage_Click(object sender, EventArgs e)
    {
        divNoMasterPage.Visible = false;

        string masterPageVirtualPath = txtMasterPage.Text.Trim().Split(';')[1];
        string masterPagePhysicalPath = Server.MapPath(masterPageVirtualPath);

        List<string> placeholders = new List<string>();
        try
        {
            //Get all placeholders from the master page file.
            placeholders = Template.GetMasterPagePlaceholders(masterPagePhysicalPath);
        }
        catch (CMSException ex)
        {
            if (ex.Code == CMSExceptionCode.MasterPageNotFound)
            {
                //Error - master page doesn't exist.
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("MasterPageNotExist")), masterPageVirtualPath);
                ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);
                return;
            }
            if (ex.Code == CMSExceptionCode.PlaceholdersNotFound)
            {
                //Error - master page without placeholders.
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("PlaceholdersNotFound"));
                ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);
                return;
            }
        }

        //Display list of found placeholders.
        repPlaceholders.DataSource = placeholders;
        repPlaceholders.DataBind();          
    }

    /// <summary>
    /// Event handler for event ItemDataBound of Repeater repPlaceholders.
    /// </summary>
    /// <remarks>
    ///     Used to set content placeholder id in Repeater repPlaceholders.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void repPlaceholders_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            Label lblPlaceholderName = (Label)e.Item.FindControl("lblPlaceholderName");
            lblPlaceholderName.Text = Convert.ToString(e.Item.DataItem);
        }
    }

    /// <summary>
    /// Event handler for event Click of button btnCancel.
    /// </summary>
    /// <remarks>
    ///    Raises event RemoveCMSControlEvent of parent user control 
    ///     to remove from CMS panel the current user control "TemplateAddEdit.ascx".   
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        RemoveCMSControlEventArgs args = new RemoveCMSControlEventArgs("TemplateAddEdit.ascx");
        ParentControl.OnRemoveCMSControlEvent(sender, args);
    }

    /// <summary>
    /// Event handler for event Click of button btnSave.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Create new or update existing template by gathering the details in the interface and pass then to 
    ///     method <see cref="Template.Save(Template)"/>.
    ///     </para>
    ///     <para>
    ///     If error occurred then event for displaying error message of the parent control is raized 
    ///     is displayed in Label lblErrorMessage,
    ///     otherwise event LoadTemplateListEvent of the parent user control is raised in order to refresh data in GridView gvTemplates in 
    ///     user control "TemplateList.ascx".
    ///     </para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="Template.Save(Template)"/>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator)
            {
                Template objTemplate;
                if (this.TemplateId == null)
                {
                    //Create template
                    objTemplate = new Template();
                }
                else
                {
                    //Update template
                    objTemplate = Template.Load(this.TemplateId.Value);
                }

                objTemplate.Name = HttpUtility.HtmlDecode(txtName.Text.Trim());
                objTemplate.MasterPage = txtMasterPage.Text.Trim().Split(';')[1];

                int? savedTemplateId = null;
                try
                {
                    savedTemplateId = Template.Save(objTemplate);
                }
                catch (CMSException ex)
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();

                    if (ex.Code == CMSExceptionCode.TemplateDuplicatedName)
                    {
                        errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("TemplateDuplicatedNameErrorMessage"));
                    }
                    else
                    {
                        errorArgs.ErrorMessage = ex.Message;
                    }

                    ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);
                    return;
                }
                catch
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("SaveTemplateErrorMessage"));
                    ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);
                    return;
                }

                //Successful save.
                this.ParentControl.OnLoadTemplateListEvent(sender, new LoadTemplateListEventArgs());
            }
            else
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole = false;
                args.UserRole = currentUserRole;
                this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }


    /// <summary>
    /// Gets all master pages in the web site directory and display them in ListBox lstMasterPages.
    /// </summary>
    /// <author></author>
    /// <date>27/02/2008</date>
    private void DisplayAllMasterPages()
    {
        //Get all master pages existing on the web site.
        string webSitePath = Server.MapPath("~/");
        string[] masterPagesPhysicalPaths = Directory.GetFiles(webSitePath, "*.master", SearchOption.AllDirectories);
        string[] masterPagesVirtualPaths = new string[masterPagesPhysicalPaths.Length];
        for (int i = 0; i < masterPagesPhysicalPaths.Length; i++)
        {
            masterPagesVirtualPaths[i] = "~/" + masterPagesPhysicalPaths[i].Remove(0, webSitePath.Length).Replace("\\", "/");
        }

        //Display the found master pages in listbox lstMasterPages.
        repMasterPages.DataSource = masterPagesVirtualPaths;
        repMasterPages.DataBind();
    }

    /// <summary>
    /// Retrieves from database details of the specified by id template and display them in the interface.
    /// </summary>
    /// <remarks>
    ///     Template details are retrieved by calling static method <see cref="Template.Load(int)"/>.
    /// </remarks>
    /// <param name="templateId">Template identifier.</param>
    /// <author></author>
    /// <date>25/02/2008</date>
    private void LoadTemplateDetails(int templateId)
    {
        //Retrieve details from database.
        Template objTemplate = Template.Load(templateId);;

        //Display details in interface.
        txtName.Text = objTemplate.Name;
        txtMasterPage.Text = repMasterPages.ID + ";" + objTemplate.MasterPage;

        List<string> placeholders = new List<string>();
        string masterPagePhysicalPath = Server.MapPath(objTemplate.MasterPage);
        try
        {
            placeholders = Template.GetMasterPagePlaceholders(masterPagePhysicalPath);
        }
        catch (CMSException ex)
        {
            if (ex.Code == CMSExceptionCode.MasterPageNotFound)
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("MasterPageNotExist")), masterPagePhysicalPath);
                ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);
                return;
            }
        }

        repPlaceholders.DataSource = placeholders;
        repPlaceholders.DataBind();
    }
}
