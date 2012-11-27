using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS.Exception;
using System.Web.Security;
using Melon.Components.CMS;


/// <summary>
/// Provides interface for creation and modification if cms node.
/// </summary>
/// <remarks>
/// <para>
///     To create cms node the user should fill in its settings and if the node is from type <see cref="Melon.Components.CMS.NodeType.ContentManageablePage"/>
///     to fill in content. For content-manageable pages settings and content are displayed in different tabs.
/// </para>
/// <para>
///     The nodes from different types have common and specific only for them settings. So settings
///     are grouped and organized in settings user controls (<see cref="SettingsBoxGeneral"/>,<see cref="SettingsBoxTemplate"/>,<see cref="SettingsBoxMetaTags"/>,<see cref="SettingsBoxStaticPage"/>,<see cref="SettingsBoxPermissions"/>) 
///     which are displayed or hidden depending on the node type.
/// </para>
/// <para>
///    The settings and content in the interface will be saved for the currently selected language from user control <see cref="Languages"/> at the top of the screen.
/// </para>
/// </remarks>
/// <seealso cref="Node"/>
/// <seealso cref="Node.Save(Node,string)"/>
/// <seealso cref="Languages"/>
/// <seealso cref="SettingsBoxGeneral"/>
/// <seealso cref="SettingsBoxTemplate"/>
/// <seealso cref="SettingsBoxMetaTags"/>
/// <seealso cref="SettingsBoxStaticPage"/>
/// <seealso cref="SettingsBoxPermissions"/>
/// <seealso cref="SettingsBoxLog"/>
public partial class WorkArea:CMSControl
{
    #region Properties

    /// <summary>
    /// Type of cms node for which settings to be displayed.
    /// </summary>
    public NodeType NodeType
    {
        get
        {
            return (NodeType)Convert.ToInt32(ViewState["__mc_cms_NodeType"]);
        }
        set
        {
            ViewState["__mc_cms_NodeType"] = value;
        }
    }

    /// <summary>
    /// Id of cms node which settings are displayed in the interface.
    /// </summary>
    /// <remarks>
    ///     It is null if user control "Settings.ascx" is loaded for cms node creation.
    /// </remarks>
    public int? NodeId;

    /// <summary>
    /// Id of cms parent node.
    /// </summary>
    /// <remarks>If it is null the node is a root node.</remarks>
    public int? NodeParentId;

    /// <summary>
    /// Tab which is currently selected in the work area (Settings/Content).
    /// </summary>
    public WorkAreaTabs SelectedTab;

    /// <summary>
    /// Location path of the node.
    /// </summary>
    /// <remarks>
    /// The location is formed by codes of all parent nodes separated with separator "/".
    /// </remarks>
    public string NodeLocation;

    /// <summary>
    /// Gets or sets the current node from/in the view state.
    /// </summary>
    public Node CurrentNode
    {
        get
        {
            if (ViewState["__mc_cms_CurrentNode"] != null)
            {
                return (Node)ViewState["__mc_cms_CurrentNode"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            ViewState["__mc_cms_CurrentNode"] = value;
        }
    }

    /// <summary>
    /// Flag which indicated whether live version of node is loaded.
    /// It is set to true when action RestoreFromLive occurs.
    /// </summary>
    protected bool LiveVersionLoaded = false;

    #endregion

    /// <summary>
    /// Attach event handlers to the controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <date>10/03/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.cntrlLanguages.ChangeLanguage += new ChangeLanguageEventHandler(cntrlLanguages_ChangeLanguage);
        this.cntrlStaticPageSettings.ChangeStaticPageType += new ChangeStaticPageTypeEventHandler(cntrlStaticPageSettings_ChangeStaticPageType);

        this.lbtnPreviewDraft.Click += new EventHandler(lbtnPreviewDraft_Click);
        this.lbtnPreviewLive.Click += new EventHandler(lbtnPreviewLive_Click);

        this.btnSaveDraft.Click += new EventHandler(btnSaveDraft_Click);
        this.btnSaveAndPublish.Click += new EventHandler(btnSaveAndPublish_Click);
        this.btnLoadFromLive.Click += new EventHandler(btnLoadFromLive_Click);
        this.btnCompare.Click += new EventHandler(btnCompare_Click);
        this.btnLoadPage.Click += new EventHandler(btnLoadPage_Click);


        this.lbtnClickHereCMSExplorerCreateFolder.Click += new EventHandler(lbtnClickHereCMSExplorerCreateFolder_Click);
        this.lbtnClickHereCMSExplorerCreateStatic.Click += new EventHandler(lbtnClickHereCMSExplorerCreateStatic_Click);
        this.lbtnClickHereCMSExplorerCreateContent.Click += new EventHandler(lbtnClickHereCMSExplorerCreateContent_Click);

        this.lbtnClickHereTemplatesCreate.Click += new EventHandler(lbtnClickHereTemplatesCreate_Click);
        this.lbtnClickHereTemplatesEdit.Click += new EventHandler(lbtnClickHereTemplatesCreate_Click);

        this.lbtnClickHereUsers.Click += new EventHandler(lbtnClickHereUsers_Click);

        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     Display the needed settings controls for the the current node type.
    ///     If node is open for update then method <see cref="LoadNodeVersion"/> 
    ///     is called to load draft settings of the node.
    ///     <para>Javascript function getAccessibilityPermissions() is attached to buttons btnSaveAndPublish
    ///         and btnSaveDraft. This function is included as script block in code behind of
    ///         user control SettingsBoxPermissions.ascx.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <date>10/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            cntrlLanguages.SelectedLanguage = this.ParentControl.CurrentLanguage;
            btnLoadFromLive.Visible = false;
            btnCompare.Visible = false;
            lbtnPreviewDraft.Visible = false;
            lbtnPreviewLive.Visible = false;

            Node currentNode = null;
            if (this.NodeId != null)
            {
                lbtnPreviewDraft.Visible = true;

                currentNode = Node.Load(this.NodeId.Value, this.ParentControl.CurrentLanguage);
                if (currentNode == null)
                {
                    //Display error message that node doesn't exist.
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedNodeByAnotherUserErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    //Refresh cms explorer.
                    LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                    args.RefreshExplorer = true;
                    args.ExplorerNodeType = NodeType.Explorer;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, args);

                    return;
                }
                else
                {
                    this.NodeType = currentNode.Type;
                    this.NodeParentId = currentNode.ParentId;
                    this.NodeLocation = currentNode.Location;
                    if (currentNode.LiveVersion != null)
                    {
                        //There is live version of node.
                        lbtnPreviewLive.Visible = true;
                        btnLoadFromLive.Visible = true;
                        btnCompare.Visible = true;
                    }
                }
            }
            this.CurrentNode = currentNode;

            cntrlGeneralSettings.CurrentNodeType = this.NodeType;
            cntrlGeneralSettings.Location = this.NodeLocation;

            if (this.NodeType == NodeType.Explorer)
            {
                divExplorerLayout.Visible = true;
                divWorkArea.Visible = false;
            }
            else
            {
                divExplorerLayout.Visible = false;
                divWorkArea.Visible = true;
                if (this.ParentControl.CurrentUser != null)
                {
                    if (this.ParentControl.CurrentUser.IsInCMSRole(CMSRole.Writer))
                    {
                        btnSaveAndPublish.Visible = false;
                    }
                }
                else
                {
                    this.ParentControl.RedirectToLoginPage();
                }

                if (this.NodeType == NodeType.ContentManageablePage)
                {
                    //Javascript function getAccessibilityPermissions is included as script block in code behind of
                    //user control "SettingsBoxPermissions.ascx", function getPageContent is defined here.
                    btnSaveAndPublish.Attributes.Add("onclick", "GetAccessibilityPermissions();GetPageContent();");
                    btnSaveDraft.Attributes.Add("onclick", "GetAccessibilityPermissions();GetPageContent();");
                    btnCompare.Attributes.Add("onclick", "GetAccessibilityPermissions();GetPageContent();");

                    if ((this.NodeId != null) && (this.SelectedTab == WorkAreaTabs.Unknown))
                    {
                        this.SelectedTab = WorkAreaTabs.Content;
                    }
                    else
                    {
                        if (this.SelectedTab == WorkAreaTabs.Unknown)
                        {
                            this.SelectedTab = WorkAreaTabs.Settings;
                        }
                    }
                }
                else
                {
                    //Javascript function getAccessibilityPermissions is included as script block in code behind of
                    //user control "SettingsBoxPermissions.ascx".
                    btnSaveAndPublish.Attributes.Add("onclick", "GetAccessibilityPermissions();");
                    btnSaveDraft.Attributes.Add("onclick", "GetAccessibilityPermissions();");
                    btnCompare.Attributes.Add("onclick", "GetAccessibilityPermissions()");
                }


                #region Set Controls Visibility

                //1. Set visibility of settings controls based on the node type.
                switch (this.NodeType)
                {
                    case NodeType.Folder:
                        lbtnPreviewDraft.Visible = false;
                        lbtnPreviewLive.Visible = false;

                        //**** Visible settings for folder (General, Permissions).
                        cntrlGeneralSettings.Visible = true;
                        cntrlPermissionsSettings.Visible = true;
                        cntrlPermissionsSettings.ShowAccessibleForRoles = false;
                        //*******************************************************

                        cntrlTemplateSettings.Visible = false;
                        cntrlMetaTagsSettings.Visible = false;
                        cntrlStaticPageSettings.Visible = false;
                        break;
                    case NodeType.ContentManageablePage:

                        //**** Visible settings for content-manageable page (General, Template, Meta Tags, Permissions).
                        cntrlGeneralSettings.Visible = true;
                        cntrlTemplateSettings.Visible = true;
                        cntrlMetaTagsSettings.Visible = true;
                        cntrlPermissionsSettings.Visible = true;
                        cntrlPermissionsSettings.ShowAccessibleForRoles = true;
                        //*******************************************************

                        cntrlStaticPageSettings.Visible = false;
                        break;
                    case NodeType.StaticLocalPage:

                        //**** Visible settings for local page (General, StaticPage, Permissions).
                        cntrlGeneralSettings.Visible = true;
                        cntrlStaticPageSettings.Visible = true;
                        cntrlPermissionsSettings.Visible = true;
                        cntrlPermissionsSettings.ShowAccessibleForRoles = true;
                        //*******************************************************

                        cntrlTemplateSettings.Visible = false;
                        cntrlMetaTagsSettings.Visible = false;
                        break;
                    case NodeType.StaticExternalPage:

                        //**** Visible settings for external page (General, StaticPage, Permissions).
                        cntrlGeneralSettings.Visible = true;
                        cntrlStaticPageSettings.Visible = true;
                        cntrlPermissionsSettings.Visible = true;
                        cntrlPermissionsSettings.ShowAccessibleForRoles = false;
                        //*******************************************************

                        cntrlTemplateSettings.Visible = false;
                        cntrlMetaTagsSettings.Visible = false;
                        break;
                    case NodeType.StaticMenuPage:

                        //**** Visible settings for menu page (General, StaticPage).
                        cntrlGeneralSettings.Visible = true;
                        cntrlStaticPageSettings.Visible = true;

                        cntrlTemplateSettings.Visible = false;
                        cntrlMetaTagsSettings.Visible = false;
                        cntrlPermissionsSettings.Visible = false;
                        break;
                    default:
                        break;
                }

                cntrlGeneralSettings.CurrentNodeType = this.NodeType;
                cntrlPermissionsSettings.CurrentNodeType = this.NodeType;
                cntrlStaticPageSettings.CurrentNodeType = this.NodeType;
                cntrlTemplateSettings.CurrentNodeType = this.NodeType;

                if (this.CurrentNode == null)
                {
                    cntrlLog.Visible = false;
                }
                else
                {
                    cntrlLog.Visible = true;
                }

                #endregion

                #region Display Settings

                if (this.CurrentNode != null)
                {
                    LoadNodeVersion(NodeVersion.Draft);

                    /*** Log ***/
                    cntrlLog.DateCreated = this.CurrentNode.DateCreated;
                    cntrlLog.UserWhoCreated = this.CurrentNode.CreatedByUserCode;
                    cntrlLog.DateLastUpdated = this.CurrentNode.DateLastUpdated;
                    cntrlLog.UserWhoLastUpdated = this.CurrentNode.LastUpdatedByUserName;
                    if (this.CurrentNode.ExistsLanguageVersion)
                    {
                        cntrlLog.LanguageName = this.CurrentNode.LanguageCulture.Name;
                        cntrlLog.LangVersionDateLastUpdated = this.CurrentNode.LangVersionDateLastUpdated;
                        cntrlLog.LangVersionUserWhoLastUpdated = this.CurrentNode.LangVersionLastUpdatedByUserName;
                        if (this.CurrentNode.LiveVersion != null)
                        {
                            cntrlLog.LangVersionDateLastPublished = this.CurrentNode.LiveVersion.LangVersionDateLastUpdated;
                            cntrlLog.LangVersionUserWhoLastPublished = this.CurrentNode.LiveVersion.LangVersionLastUpdatedByUserName;
                        }
                    }
                    else
                    {
                        lblDefaultSettingsLoaded.Visible = true;
                    }
                }

                #endregion
            }
        }
        else
        {
            if (this.NodeType == NodeType.ContentManageablePage)
            {
                WorkAreaTabs tab = WorkAreaTabs.Content;
                if (hfSelectedTab.Value == "tabContent")
                {
                    tab = WorkAreaTabs.Content;
                }
                else if (hfSelectedTab.Value == "tabSettings")
                {
                    tab = WorkAreaTabs.Settings;
                }
                this.SelectedTab = tab;
            }
        }
    }


    /// <summary>
    /// Event handler for event Click of Button btnCompare.
    /// </summary>
    /// <remarks>
    /// Opens screen in which are compared settings displayed on the screen and settings from live version.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <date>05/06/2008</date>
    protected void btnCompare_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            if (this.CurrentNode.Exists())
            {
                if (this.CurrentNode.Type == NodeType.ContentManageablePage)
                {
                    Session["mc_cms_current_content"] = hfPageContent.Value.Replace("&lt;PageContent&gt;", "<PageContent>")
                            .Replace("&lt;/PageContent&gt;", "</PageContent>")
                            .Replace("&lt;ContentPlaceholder", "<ContentPlaceholder")
                            .Replace("&gt;&lt;/ContentPlaceholder&gt;", "></ContentPlaceholder>");

                    if (hfSelectedTab.Value == "tabContent")
                    {
                        this.SelectedTab = WorkAreaTabs.Content;
                    }
                    else
                    {
                        this.SelectedTab = WorkAreaTabs.Settings;
                    }
                }
                else
                {
                    Session["mc_cms_current_content"] = null;
                }

                Node objNode = this.CurrentNode;
                #region  Gether node settings currently on screen

                objNode.Location = this.NodeLocation;
                objNode.Type = this.NodeType;

                //*** Set General settings ***
                objNode.Type = this.NodeType;
                objNode.Code = cntrlGeneralSettings.Code;
                objNode.Alias = cntrlGeneralSettings.Alias;
                objNode.LanguageCulture = this.ParentControl.CurrentLanguage;
                objNode.Title = cntrlGeneralSettings.Title;
                objNode.Image = cntrlGeneralSettings.Image;
                objNode.HoverImage = cntrlGeneralSettings.HoverImage;
                if (this.NodeType != NodeType.Folder)
                {
                    objNode.SelectedImage = cntrlGeneralSettings.SelectedImage;
                    objNode.Target = cntrlGeneralSettings.Target;
                }
                objNode.IsHiddenInNavigation = cntrlGeneralSettings.IsHiddenInNavigation;

                //*** Set Template settings ***
                if (this.NodeType == NodeType.ContentManageablePage)
                {
                    objNode.TemplateId = cntrlTemplateSettings.TemplateId;
                    //Set Meta tags
                    objNode.Author = cntrlMetaTagsSettings.Author;
                    objNode.Keywords = cntrlMetaTagsSettings.Keywords;
                    objNode.Description = cntrlMetaTagsSettings.Description;
                }

                //*** Set Static local page settings
                if (this.NodeType == NodeType.StaticLocalPage)
                {
                    objNode.LocalPagePath = cntrlStaticPageSettings.LocalPagePath;
                }

                //*** Set Static external page settings
                if (this.NodeType == NodeType.StaticExternalPage)
                {
                    objNode.ExternalPageURL = cntrlStaticPageSettings.ExternalPageURL;
                }

                //*** Set Static external page settings
                if (this.NodeType == NodeType.StaticMenuPage)
                {
                    objNode.ReferredNodeId = cntrlStaticPageSettings.ReferredNodeId;
                }

                //*** Set Permission settings ***
                if (this.NodeType != NodeType.StaticMenuPage)
                {
                    //Only StaticMenuPages has no permissions because they get permissions from the page they reffer.
                    objNode.VisibleFor = cntrlPermissionsSettings.VisibleFor;
                    objNode.AccessibleFor = cntrlPermissionsSettings.AccessibleFor;
                }
                #endregion

                Session["mc_cms_current_node"] = objNode;
                Session["mc_cms_live_node"] = this.CurrentNode.LiveVersion;

                if (!Page.ClientScript.IsClientScriptBlockRegistered("openCompareWindow"))
                {
                    // >> --07/10/2009 by Mario:(Compare front-end code is moved to a user control)
                    //string script = "<script type='text/javascript'> window.open('" + this.Page.ResolveUrl(CMSSettings.BasePath + "Sources/Compare.aspx") + "','popup','menubar=0,resizable=1,scrollbars=1');</script>";
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openCompareWindow", script);
                    // >> --

                    // >> ++07/10/2009 by Mario:(Compare front-end code is moved to a user control)                                               
                    string script = "<script type='text/javascript'> centerPopup();loadPopup();</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "openCompareWindow", script);
                    // >> ++
                }

                // >> ++07/10/2009 by Mario:(Compare front-end code is moved to a user control)                                               
                ((Compare)ucCompare).LoadComparePages();
                // >> ++
            }
            else
            {
                //Display error message that node which is tried to be updated doesn't exists.
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedNodeByAnotherUserErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                //Refresh CMS explorer.
                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.ExplorerNodeType = NodeType.Explorer;
                args.RefreshExplorer = true;
                this.ParentControl.OnLoadWorkAreaEvent(this.Page, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }

    }

    /// <summary>
    /// Event handler for event Click of Button btnLoadFormLive.
    /// </summary>
    /// <remarks>
    ///    Live version settings of node <see cref="CurrentNode"/> should be loaded in the interface.
    ///    For the purpose method <see cref="LoadNodeVersion"/> is called 
    ///    to set properties of all settings user controls (SettingsBoxGeneral.ascx, SettingsBoxMetaTags.ascx,SettingsBoxPermissions.ascx,SettingsBoxTemplate.ascx,SettingsBoxStaticPage.ascx ) 
    ///    and then is raised their event ReLoadSettings. 
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <date>10/03/2008</date>
    protected void btnLoadFromLive_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            if (this.CurrentNode.Exists())
            {
                if (this.CurrentNode.Type == NodeType.ContentManageablePage)
                {
                    LiveVersionLoaded = true;
                    if (hfSelectedTab.Value == "tabContent")
                    {
                        this.SelectedTab = WorkAreaTabs.Content;
                    }
                    else
                    {
                        this.SelectedTab = WorkAreaTabs.Settings;
                    }
                }

                LoadNodeVersion(NodeVersion.Live);

                cntrlGeneralSettings.ReLoadSettings();
                cntrlTemplateSettings.ReLoadSettings();
                cntrlMetaTagsSettings.ReLoadSettings();
                cntrlStaticPageSettings.ReLoadSettings();
                cntrlPermissionsSettings.ReLoadSettings();
            }
            else
            {
                //Display error message that node which is tried to be updated doesn't exists.
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedNodeByAnotherUserErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                //Refresh CMS explorer.
                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.ExplorerNodeType = NodeType.Explorer;
                args.RefreshExplorer = true;
                this.ParentControl.OnLoadWorkAreaEvent(this.Page, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Event handler for event Click of Button btnSaveAndPublish.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="SaveNode"/> to save the live version of node.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="SaveNode"/>
    /// <date>18/03/2007</date>
    protected void btnSaveAndPublish_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator || currentUserRole == CMSRole.Administrator)
            {
                SaveNode(NodeVersion.Live);
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
    /// Event handler for event Click of Button btnSaveDraft.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="SaveNode"/> to save the draft version of node.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="SaveNode"/>
    /// <date>18/03/2007</date>
    protected void btnSaveDraft_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole != CMSRole.None)
            {
                SaveNode(NodeVersion.Draft);
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
    /// Event handler for event Click of Button btnLoadPage.
    /// </summary>
    /// <remarks>
    ///     <para>Event Click of Button btnLoadPage is fired with javascript.</para>
    ///     <para>There is javascript timer "timer_FCKEditors" which is started when content page is opened
    ///     and attribute src of iframe "iFramePageContent" is set. This timer execute javascript function 
    ///     "checkFrameUrl" on every 100 ms to check whether another page is not loaded in the iframe. 
    ///     In this case the new frame src is set in hidden field hfNewFrameUrl and event Click of hidden button btnLoadPage is fired.</para>
    ///     <para>The parameter pageId is read from the new frame src (value of hidden field hfNewFrameUrl) and 
    ///     event of parent user control <see cref="Melon.Components.CMS.ComponentEngine.BaseCMSControl.LoadWorkAreaEvent"/> is raised.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <date>15/04/2007</date>
    protected void btnLoadPage_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            string newFrameUrl = hfNewFrameUrl.Value;
            if (newFrameUrl.Contains(CMSSettings.DYNAMIC_CONTENT_FOLDER + "/" + CMSSettings.DYNAMIC_CONTENT_MANAGEABLE_PAGE)
                || newFrameUrl.Contains(CMSSettings.DYNAMIC_CONTENT_FOLDER + "/" + CMSSettings.DYNAMIC_STATIC_EXTERNAL_PAGE)
                || newFrameUrl.Contains(CMSSettings.DYNAMIC_CONTENT_FOLDER + "/" + CMSSettings.DYNAMIC_STATIC_MENU_PAGE)
                || newFrameUrl.Contains(CMSSettings.DYNAMIC_CONTENT_FOLDER + "/" + CMSSettings.DYNAMIC_STATIC_LOCAL_PAGE))
            {
                int? pageId = null;
                string urlParams = newFrameUrl.Substring(newFrameUrl.IndexOf('?'));
                string[] keyValuePairs = urlParams.Split('&');

                foreach (string keyValue in keyValuePairs)
                {
                    if (keyValue.Contains("pageId"))
                    {
                        string[] keyValueSplitted = keyValue.Split('=');
                        pageId = Convert.ToInt32(keyValueSplitted[1]);
                    }
                }

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.RefreshExplorer = true;
                args.NodeId = pageId;

                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
            else
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("NavigateToNonCMSPageMessage")), newFrameUrl);
                this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.RefreshExplorer = true;
                args.NodeId = null;
                args.ExplorerNodeType = NodeType.Explorer;
                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }


    /// <summary>
    /// Event handler for event Click of Button btnPreviewLive.
    /// </summary>
    /// <remarks>
    /// Calls method <see cref="PreviewNode(NodeVersion)"/> to preview live version of page.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnPreviewLive_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            PreviewNode(NodeVersion.Live);
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Event handler for event Click of Button btnPreviewDraft.
    /// </summary>
    /// <remarks>
    /// Calls method <see cref="PreviewNode(NodeVersion)"/> to preview draft version of page.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnPreviewDraft_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            PreviewNode(NodeVersion.Draft);
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Event handler for event Click of Button lbtnClickHereUsers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnClickHereUsers_Click(object sender, EventArgs e)
    {
        this.ParentControl.OnLoadUsersListEvent(sender, e);
    }

    /// <summary>
    /// Event handler for event Click of Button lbtnClickHereCMSExplorerCreateContent.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnClickHereCMSExplorerCreateContent_Click(object sender, EventArgs e)
    {
        this.ParentControl.OnCreateContentPageEvent(sender, e);
    }

    /// <summary>
    /// Event handler for event Click of Button lbtnClickHereCMSExplorerCreateStatic.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnClickHereCMSExplorerCreateStatic_Click(object sender, EventArgs e)
    {
        this.ParentControl.OnCreateStaticPageEvent(sender, e);
    }

    /// <summary>
    /// Event handler for event Click of Button lbtnClickHereTemplatesCreate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnClickHereTemplatesCreate_Click(object sender, EventArgs e)
    {
        this.ParentControl.OnLoadTemplateListEvent(sender, new LoadTemplateListEventArgs());
    }

    /// <summary>
    /// Event handler for event Click of Button lbtnClickHereCMSExplorerCreateFolder.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnClickHereCMSExplorerCreateFolder_Click(object sender, EventArgs e)
    {
        this.ParentControl.OnCreateFolderEvent(sender, e);
    }


    /// <summary>
    /// Event handler for event ChangeStaticPageType of user control cntrlStaticPageSettings.
    /// </summary>
    /// <remarks>
    ///     When type of static page is changed it is needed to hide/show some settings controls 
    ///     because they differ for the different types. The new type is stored in user control property 
    ///     <see cref="NodeType"/>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <date>13/03/2008</date>
    protected void cntrlStaticPageSettings_ChangeStaticPageType(object sender, ChangeStaticPageTypeArgs args)
    {
        if (args.NewType == NodeType.StaticLocalPage)
        {
            cntrlPermissionsSettings.Visible = true;
            cntrlPermissionsSettings.ShowAccessibleForRoles = true;
        }
        if (args.NewType == NodeType.StaticExternalPage)
        {
            cntrlPermissionsSettings.Visible = true;
            cntrlPermissionsSettings.ShowAccessibleForRoles = false;
        }
        if (args.NewType == NodeType.StaticMenuPage)
        {
            cntrlPermissionsSettings.Visible = false;
        }
        this.NodeType = args.NewType;
    }

    /// <summary>
    /// Event handler for event ChangeLanguage of user control cntrlLanguages.
    /// </summary>
    /// <remarks>
    ///     When language is changed the settings to the language version should be displayed.
    ///     That's why event LoadSettingsEvent is raised in order to be retrieved and displayed language version settings. 
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <date>13/03/2008</date>
    protected void cntrlLanguages_ChangeLanguage(object sender, ChangeLanguageArgs args)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            this.ParentControl.CurrentLanguage = args.NewLanguage;

            LoadWorkAreaArgs settingsArgs = new LoadWorkAreaArgs();
            settingsArgs.RefreshExplorer = false;
            if (this.NodeId == null)
            {
                settingsArgs.ExplorerNodeType = this.NodeType;
                settingsArgs.NodeLocation = this.NodeLocation;
                settingsArgs.NodeParentId = this.NodeParentId;
            }
            else
            {
                settingsArgs.NodeId = this.NodeId;
            }
            this.ParentControl.OnLoadWorkAreaEvent(sender, settingsArgs);
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }


    /// <summary>
    /// Displays in the interface settings of draft or live version of the current node.
    /// </summary>
    /// <param name="nodeVersion">Version which settings to be loaded in the interface.</param>
    /// <seealso cref="Melon.Components.CMS.NodeVersion"/>
    /// <date>13/03/2008</date>
    private void LoadNodeVersion(NodeVersion nodeVersion)
    {
        Node node;
        if (nodeVersion == NodeVersion.Draft)
        {
            node = this.CurrentNode;
        }
        else
        {
            node = this.CurrentNode.LiveVersion;
        }

        //*** General settings ***
        cntrlGeneralSettings.Code = node.Code;
        cntrlGeneralSettings.Alias = node.Alias;
        cntrlGeneralSettings.Title = node.Title;
        cntrlGeneralSettings.ImagePath = node.ImagePath;
        cntrlGeneralSettings.HoverImagePath = node.HoverImagePath;
        if (node.Type != NodeType.Folder)
        {
            cntrlGeneralSettings.SelectedImagePath = node.SelectedImagePath;
        }
        cntrlGeneralSettings.IsHiddenInNavigation = node.IsHiddenInNavigation.Value;
        cntrlGeneralSettings.Target = node.Target;

        //*** Template settings ***
        if (node.Type == NodeType.ContentManageablePage)
        {
            cntrlTemplateSettings.TemplateId = node.TemplateId.Value;
            //Meta tags
            cntrlMetaTagsSettings.Author = node.Author;
            cntrlMetaTagsSettings.MetaTitle = node.MetaTitle;
            cntrlMetaTagsSettings.Keywords = node.Keywords;
            cntrlMetaTagsSettings.Description = node.Description;
        }

        cntrlStaticPageSettings.IsUpdateContext = true;
        //*** Static local page settings
        if (node.Type == NodeType.StaticLocalPage)
        {
            cntrlStaticPageSettings.LocalPagePath = node.LocalPagePath;
        }

        //*** Static external page settings
        if (node.Type == NodeType.StaticExternalPage)
        {
            cntrlStaticPageSettings.ExternalPageURL = node.ExternalPageURL;
        }

        //*** Static external page settings
        if (node.Type == NodeType.StaticMenuPage)
        {
            cntrlStaticPageSettings.ReferredNodeId = node.ReferredNodeId.Value;
        }

        //*** Permission settings ***
        if (node.Type != NodeType.StaticMenuPage)
        {
            //Only StaticMenuPages has no permissions because they get permissions from the page they reffer.
            cntrlPermissionsSettings.VisibleFor = node.VisibleFor;
            cntrlPermissionsSettings.AccessibleFor = node.AccessibleFor;
        }
    }

    /// <summary>
    /// Saves all node settings set in the interface.
    /// <remarks>
    ///     <para>
    ///         Calls method Save of class <see cref="Melon.Components.CMS.Node"/> to save the node.</para>
    ///     <para>
    ///         If error occured during the save then event for displaying error message of the parent control is raised.
    ///         If save was successful then event LoadSettingsEvent of the parent user control is raised 
    ///         in order to refresh the CMS tree.</para>
    /// </remarks>
    /// </summary>
    /// <param name="nodeVersion">Version of node which have to be saved.</param>
    /// <seealso cref="Melon.Components.CMS.Node.Save"/>
    /// <date>18/03/2007</date>
    private void SaveNode(NodeVersion nodeVersion)
    {
        //IsValid is set to true for the regular expression validators from control cntrlGeneralSettings
        //because when error occurs during the save error messages of the validators are displayed (bug in .NET).
        ((RegularExpressionValidator)cntrlGeneralSettings.FindControl("revImage")).IsValid = true;
        ((RegularExpressionValidator)cntrlGeneralSettings.FindControl("revHoverImage")).IsValid = true;
        ((RegularExpressionValidator)cntrlGeneralSettings.FindControl("revSelectedImage")).IsValid = true;

        Node objNode;
        int? savedNodeId = null;

        if (this.NodeId == null)
        {
            //*** Case: Create Node ***
            objNode = new Node();
            objNode.ParentId = this.NodeParentId;
            objNode.Location = this.NodeLocation;
        }
        else
        {
            //*** Case: Update Node ***
            objNode = Node.Load(this.NodeId.Value, this.ParentControl.CurrentLanguage);
        }

        if (objNode == null)
        {
            //Display error message that node which is tried to be updated doesn't exists.
            DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
            errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedNodeByAnotherUserErrorMessage"));
            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

            //Refresh CMS explorer.
            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.ExplorerNodeType = NodeType.Explorer;
            args.RefreshExplorer = true;
            this.ParentControl.OnLoadWorkAreaEvent(this.Page, args);
        }
        else
        {
            #region 1. Set details in object Node.

            //*** Set General settings ***
            objNode.IsLiveVersion = Convert.ToBoolean(Convert.ToInt32(nodeVersion));
            objNode.Type = this.NodeType;
            objNode.Code = cntrlGeneralSettings.Code;
            objNode.Alias = cntrlGeneralSettings.Alias;
            objNode.LanguageCulture = this.ParentControl.CurrentLanguage;
            objNode.Title = cntrlGeneralSettings.Title;
            objNode.Image = cntrlGeneralSettings.Image;
            objNode.HoverImage = cntrlGeneralSettings.HoverImage;
            if (this.NodeType != NodeType.Folder)
            {
                objNode.SelectedImage = cntrlGeneralSettings.SelectedImage;
                objNode.Target = cntrlGeneralSettings.Target;
            }
            objNode.IsHiddenInNavigation = cntrlGeneralSettings.IsHiddenInNavigation;

            //*** Set Template settings ***
            if (this.NodeType == NodeType.ContentManageablePage)
            {
                objNode.TemplateId = cntrlTemplateSettings.TemplateId;
                //Set Meta tags
                objNode.MetaTitle = cntrlMetaTagsSettings.MetaTitle;
                objNode.Author = cntrlMetaTagsSettings.Author;
                objNode.Keywords = cntrlMetaTagsSettings.Keywords;
                objNode.Description = cntrlMetaTagsSettings.Description;
            }

            //*** Set Static local page settings
            if (this.NodeType == NodeType.StaticLocalPage)
            {
                objNode.LocalPagePath = cntrlStaticPageSettings.LocalPagePath;
            }

            //*** Set Static external page settings
            if (this.NodeType == NodeType.StaticExternalPage)
            {
                objNode.ExternalPageURL = cntrlStaticPageSettings.ExternalPageURL;
            }

            //*** Set Static external page settings
            if (this.NodeType == NodeType.StaticMenuPage)
            {
                objNode.ReferredNodeId = cntrlStaticPageSettings.ReferredNodeId;
            }

            //*** Set Permission settings ***
            if (this.NodeType != NodeType.StaticMenuPage)
            {
                //Only StaticMenuPages has no permissions because they get permissions from the page they reffer.
                objNode.VisibleFor = cntrlPermissionsSettings.VisibleFor;
                objNode.AccessibleFor = cntrlPermissionsSettings.AccessibleFor;
            }
            objNode.UserId = this.ParentControl.CurrentUser.AdapterId;

            string pageContent = null;
            if (objNode.Type == NodeType.ContentManageablePage)
            {
                pageContent = hfPageContent.Value.Replace("&lt;PageContent&gt;", "<PageContent>")
                    .Replace("&lt;/PageContent&gt;", "</PageContent>")
                    .Replace("&lt;ContentPlaceholder", "<ContentPlaceholder")
                    .Replace("&gt;&lt;/ContentPlaceholder&gt;", "></ContentPlaceholder>");
            }

            #endregion

            #region 2. Try to save node.

            try
            {
                savedNodeId = Node.Save(objNode, pageContent);
            }
            catch (CMSException ex)
            {
                //Exception occured during the save. Display relevant error message.
                string labelImageErrorName = null;
                Label lblImageError = null;

                string errorMessage = null;

                switch (ex.Code)
                {
                    case CMSExceptionCode.ImageNotAllowedSize:
                        //ex.AdditionalInfo - file name of image;
                        if (objNode.Image != null && ex.AdditionalInfo == objNode.Image.FileName)
                        {
                            labelImageErrorName = "lblImageError";
                        }
                        else if (objNode.HoverImage != null && ex.AdditionalInfo == objNode.HoverImage.FileName)
                        {
                            labelImageErrorName = "lblHoverImageError";
                        }
                        else if (objNode.SelectedImage != null && ex.AdditionalInfo == objNode.SelectedImage.FileName)
                        {
                            labelImageErrorName = "lblSelectedImageError";
                        }

                        lblImageError = (Label)cntrlGeneralSettings.FindControl(labelImageErrorName);
                        lblImageError.Visible = true;
                        lblImageError.Text = String.Format(
                            Convert.ToString(GetLocalResourceObject("ImageNotAllowedSizeErrorMessage")),
                            ex.AdditionalInfo, CMSSettings.ImagesMaxSize);
                        break;
                    case CMSExceptionCode.ImageNotAllowedExtension:
                        //ex.AdditionalInfo - file name of image;
                        if (objNode.Image != null && ex.AdditionalInfo == objNode.Image.FileName)
                        {
                            labelImageErrorName = "lblImageError";
                        }
                        else if (objNode.HoverImage != null && ex.AdditionalInfo == objNode.HoverImage.FileName)
                        {
                            labelImageErrorName = "lblHoverImageError";
                        }
                        else if (objNode.SelectedImage != null && ex.AdditionalInfo == objNode.SelectedImage.FileName)
                        {
                            labelImageErrorName = "lblSelectedImageError";
                        }

                        lblImageError = (Label)cntrlGeneralSettings.FindControl(labelImageErrorName);
                        lblImageError.Visible = true;
                        lblImageError.Text = String.Format(
                            Convert.ToString(GetLocalResourceObject("ImageNotAllowedExtensionErrorMessage")),
                            ex.AdditionalInfo);
                        break;
                    case CMSExceptionCode.ImageUploadFailure:
                        errorMessage = String.Format(
                           Convert.ToString(GetLocalResourceObject("ImageUploadErrorMessage")),
                           ex.AdditionalInfo);
                        break;
                    case CMSExceptionCode.ImageRemoveFailure:
                        errorMessage = String.Format(
                           Convert.ToString(GetLocalResourceObject("ImageRemoveErrorMessage")),
                           ex.AdditionalInfo);
                        break;
                    case CMSExceptionCode.ParentNodeNotExist:
                        if (this.NodeType == NodeType.Folder)
                        {
                            errorMessage = Convert.ToString(GetLocalResourceObject("FolderParentNotExistsErrorMessage"));
                        }
                        else
                        {
                            errorMessage = Convert.ToString(GetLocalResourceObject("PageParentNotExistsErrorMessage"));
                        }
                        break;
                    case CMSExceptionCode.NodeDuplicateCode:
                        errorMessage = Convert.ToString(GetLocalResourceObject("NodeDuplicateCodeErrorMessage"));
                        break;
                    case CMSExceptionCode.NodeDuplicateAlias:
                        errorMessage = Convert.ToString(GetLocalResourceObject("NodeDuplicateAliasErrorMessage"));
                        break;
                    case CMSExceptionCode.ParentNotPublished:
                        errorMessage = Convert.ToString(GetLocalResourceObject("ParentNotPublishedErrorMessage"));
                        break;
                    case CMSExceptionCode.PublishMenuPageWithoutReferrence:
                        errorMessage = Convert.ToString(GetLocalResourceObject("PublishMenuPageWithoutReferrenceErrorMessage"));
                        break;
                    case CMSExceptionCode.PublishWithoutDefaultVersion:
                        errorMessage = Convert.ToString(GetLocalResourceObject("PublishWithoutDefaultVersionErrorMessage"));
                        break;
                    case CMSExceptionCode.UnauthorizedAccessException:
                        errorMessage = String.Format(
                            Convert.ToString(GetLocalResourceObject("UnauthorizedAccessErrorMessage")),
                            ex.AdditionalInfo);
                        break;
                }

                if (ex.Code != CMSExceptionCode.ImageNotAllowedSize
                    && ex.Code != CMSExceptionCode.ImageNotAllowedExtension)
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs(errorMessage);
                    this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);
                }
                else if (ex.Code == CMSExceptionCode.ParentNodeNotExist)
                {
                    LoadWorkAreaArgs argsRefresh = new LoadWorkAreaArgs();
                    argsRefresh.ExplorerNodeType = NodeType.Explorer;
                    argsRefresh.RefreshExplorer = true;
                    this.ParentControl.OnLoadWorkAreaEvent(this, argsRefresh);
                }

                return;
            }
            //catch
            //{
            //    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
            //    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("SaveNodeErrorMessage"));
            //    this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);
            //    return;
            //}

            #endregion

            //Raise event LoadSettings of parent control in order to refresh 
            //the CMS Explorer tree and the node settings of the saved node.

            Melon.Components.CMS.CMS.IsSitemapStructureChanged = true;

            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.ExplorerNodeType = objNode.Type;
            args.NodeId = savedNodeId;
            args.NodeParentId = objNode.ParentId;
            args.RefreshExplorer = true;
            if (objNode.Type == NodeType.ContentManageablePage)
            {
                WorkAreaTabs tab = WorkAreaTabs.Content;
                if (hfSelectedTab.Value == "tabContent")
                {
                    tab = WorkAreaTabs.Content;
                }
                else if (hfSelectedTab.Value == "tabSettings")
                {
                    tab = WorkAreaTabs.Settings;
                }
                args.SelectedTab = tab;
            }
            this.ParentControl.OnLoadWorkAreaEvent(this.Page, args);
        }
    }

    /// <summary>
    /// Raises event LoadPagePreviewEvent of parent user control.
    /// </summary>
    /// <param name="nodeVersion"></param>
    private void PreviewNode(NodeVersion nodeVersion)
    {
        //Check whether node exists.(It could be deleted by another user.)
        if (this.CurrentNode.Exists())
        {
            //Node exists.Preview it.
            string url = "";
            if (this.CurrentNode.Type == NodeType.StaticLocalPage)
            {
                string localPagePath = (nodeVersion == NodeVersion.Draft) ? this.CurrentNode.LocalPagePath : this.CurrentNode.LiveVersion.LocalPagePath;
                url = CMSSettings.DynamicStaticLocalPagePath + "?pageId=" + Convert.ToString(this.CurrentNode.Id) + "&localPagePath=" + localPagePath;
            }
            else if (this.CurrentNode.Type == NodeType.ContentManageablePage)
            {
                url = CMSSettings.DynamicContentManageablePagePath + "?templateId=" + Convert.ToString(this.CurrentNode.TemplateId) + "&pageId=" + Convert.ToString(this.CurrentNode.Id);
            }
            else if (this.CurrentNode.Type == NodeType.StaticExternalPage)
            {
                url = CMSSettings.DynamicStaticExternalPagePath + "?pageId=" + Convert.ToString(this.CurrentNode.Id);
            }
            else if (this.CurrentNode.Type == NodeType.StaticMenuPage)
            {
                url = CMSSettings.DynamicStaticMenuPagePath + "?pageId=" + Convert.ToString(this.CurrentNode.Id);
            }

            LoadPagePreviewArgs args = new LoadPagePreviewArgs();
            args.Version = nodeVersion;
            args.PageId = this.CurrentNode.Id.Value;
            args.Url = url;
            if (hfSelectedTab.Value == "tabContent")
            {
                args.SelectedTab = WorkAreaTabs.Content;
            }
            else
            {
                args.SelectedTab = WorkAreaTabs.Settings;
            }

            this.ParentControl.OnLoadPagePreviewEvent(this.Page, args);
        }
        else
        {
            //Node doesn't exist.
            //Display error message that node which is tried to be previewed doesn't exist.
            DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
            errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedNodeByAnotherUserErrorMessage"));
            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

            //Refresh CMS explorer.
            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.ExplorerNodeType = NodeType.Explorer;
            args.RefreshExplorer = true;
            this.ParentControl.OnLoadWorkAreaEvent(this.Page, args);
        }
    }

    /// <summary>
    /// Initializes the control's properties
    /// </summary>
    /// <param name="args">The values with which the properties will be initialized</param>
    public override void Initializer(object[] args)
    {
        this.NodeType = (NodeType)args[0];
        this.NodeId = (int?)args[1];
        this.NodeParentId = (int?)args[2];
        this.NodeLocation = (string)args[3];
        this.SelectedTab = (WorkAreaTabs)args[4];
    }
}
