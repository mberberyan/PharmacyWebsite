using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS;


/// <summary>
/// Contains layout for displaying static page settings.
/// </summary>
///     <para>
///     All web controls from SettingsBoxStaticPage are using local resources.
///     To customize them modify resource file SettingsBoxStaticPage.resx located in the MC_CMS folder.
///     </para>
public partial class SettingsBoxStaticPage : SettingsControl
{
    #region Events

    /// <summary>
    /// Event raised when the type  of the static page is changed.
    /// </summary>
    public event ChangeStaticPageTypeEventHandler ChangeStaticPageType;

    #endregion 

    #region Fields & Properties

    /// <summary>
    /// Flag whether the user control is loaded in content of updating static page. 
    /// </summary>
    /// <remarks>
    /// This flag is needed to know when the radio buttons for static page type should be disabled.
    /// They should be disabled in update context, because once created page couldn't change its type.
    /// </remarks>
    public bool IsUpdateContext = false;

    private string _LocalPagePath;
    /// <summary>
    /// Path to file of local page.
    /// </summary>
    public string LocalPagePath
    {
        get 
        {
            return txtLocalPage.Text.Trim().Split(';')[1];
        }
        set { _LocalPagePath = value; }
    }

    private string _ExternalPageURL;
    /// <summary>
    /// URL of external page.
    /// </summary>
    public string ExternalPageURL
    {
        get { return txtExternalPage.Text.Trim(); }
        set { _ExternalPageURL = value; }
    }

    private int? _ReferredNodeId;
    /// <summary>
    /// Id of page that is referred from menu page.
    /// </summary>
    public int? ReferredNodeId
    {
        get 
        {
            return Convert.ToInt32(txtMenuPage.Text.Trim().Split(';')[1]);
        }
        set { _ReferredNodeId = value; }
    }

    #endregion

    /// <summary>
    /// Attach event handlers to the controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.rdolStaticPages.SelectedIndexChanged += new EventHandler(rdolStaticPages_SelectedIndexChanged);
        this.btnGoToReferredPage.Click += new EventHandler(btnGoToReferredPage_Click);

        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     If the user control is loaded for first time methods <see cref="LoadAllLocalPages"/> and <see cref="LoadAllCandidateMenuPages"/>
    ///     are called to load data in Repeaters repLocalPages and repMenuPages.
    ///     Depending on the static page type are displayed the related to the type controls.</para>
    ///     <para>
    ///     If the user control is loaded in context of updating static page then its settings are displayed
    ///     in the interface.
    ///     </para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>11/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            LoadAllLocalPages();
            LoadAllCandidateMenuPages();

            if (this.IsUpdateContext)
            {
                //Update Context
                rdolStaticPages.Visible = false;
                if (this.CurrentNodeType == NodeType.StaticLocalPage)
                {
                    lblStaticPageType.Text = Convert.ToString(GetLocalResourceObject("LocalPage"));
                }
                else if (this.CurrentNodeType == NodeType.StaticExternalPage)
                {
                    lblStaticPageType.Text = Convert.ToString(GetLocalResourceObject("ExternalPage"));
                }
                else if (this.CurrentNodeType == NodeType.StaticMenuPage)
                {
                    lblStaticPageType.Text = Convert.ToString(GetLocalResourceObject("MenuPage"));
                }
                lblStaticPageType.Visible = true;
            }
            else
            {
                //Create Context
                rdolStaticPages.Visible = true;
              
                //Select in radio button which corresponds to the static page type from which is the current selected node.
                //The values of the radio buttons correspond to the enumeration NodeType integer values.
                rdolStaticPages.SelectedIndex = rdolStaticPages.Items.IndexOf(rdolStaticPages.Items.FindByValue(Convert.ToString(Convert.ToInt32(this.CurrentNodeType))));

                lblStaticPageType.Visible = false;
            }

            DisplayNodeTypeControls(this.CurrentNodeType);

            DisplayStaticPageSettings();
        }
    }


    /// <summary>
    /// Event handler for event ReLoadSettings of the user control inherited from 
    /// parent user control <see cref="Melon.Components.CMS.ComponentEngine.SettingsControl"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Calls method <see cref="DisplayStaticPageSettings"/> to reload 
    ///         static page settings in the interface.</para>
    ///     <para>
    ///         The event is fired from the parent user control Settings.ascx 
    ///         when node settings are restored from live version. In this case are loaded 
    ///         static page settings in the live version.</para>
    /// </remarks>
    /// <param name="args"></param>
    /// <author></author>
    /// <date>26/03/2008</date>
    protected override void OnReLoadSettings(EventArgs args)
    {
        base.OnReLoadSettings(args);
        DisplayStaticPageSettings();
    }

    /// <summary>
    /// Load the setting controls for the currently selected static page type. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>11/03/2008</date>
    protected void rdolStaticPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangeStaticPageTypeArgs args = new ChangeStaticPageTypeArgs();
        args.NewType = (NodeType)Convert.ToInt32(rdolStaticPages.SelectedValue);

        this.OnChangeStaticPageType(sender, args);
    }

    /// <summary>
    /// If there is selected page in ListBox lstMenuPages raises event LoadSettings of
    /// parent control for the selected page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>19/03/2008</date>
    protected void btnGoToReferredPage_Click(object sender, EventArgs e)
    {
        if (txtMenuPage.Text != "")
        {
            int referredNodeId = Convert.ToInt32(txtMenuPage.Text.Trim().Split(';')[1]);

            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.RefreshExplorer = true;
            args.NodeId = referredNodeId;
            ((CMSControl)this.Parent.Parent).ParentControl.OnLoadWorkAreaEvent(sender, args);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeType"></param>
    /// <author></author>
    /// <date>11/03/2008</date>
    private void DisplayNodeTypeControls(NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.StaticLocalPage:
                phLocalPageSettings.Visible = true;
                rfvLocalPage.Visible = true;

                phExternalPageSettings.Visible = false;
                rfvExternalPage.Visible = false;

                phMenuPageSettings.Visible = false;
                rfvMenuPage.Visible = false;

                break;
            case NodeType.StaticExternalPage:
                phLocalPageSettings.Visible = false; 
                rfvLocalPage.Visible = false;

                phExternalPageSettings.Visible = true;
                rfvExternalPage.Visible = true;

                phMenuPageSettings.Visible = false;
                rfvMenuPage.Visible = false;

                break;
            case NodeType.StaticMenuPage:
                phLocalPageSettings.Visible = false;
                rfvLocalPage.Visible = false;

                phExternalPageSettings.Visible = false;
                rfvExternalPage.Visible = false;

                phMenuPageSettings.Visible = true;
                rfvMenuPage.Visible = true;

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <author></author>
    /// <date>11/03/2008</date>
    private void DisplayStaticPageSettings()
    {
       switch (this.CurrentNodeType)
       {
            case NodeType.StaticLocalPage:
                if (!String.IsNullOrEmpty(this._LocalPagePath))
                {
                    //The local page was opened for update. Select in the list of local pages the current one.
                    txtLocalPage.Text = repLocalPages.ID + ";" + this._LocalPagePath;
                }
                break;
            case NodeType.StaticExternalPage:
                txtExternalPage.Text = this._ExternalPageURL;
                break;
            case NodeType.StaticMenuPage:
                if (this._ReferredNodeId != null)
                {
                    //The menu page was opened for update. Select in the list of menu pages the referred one.
                    txtMenuPage.Text = repMenuPages.ID + ";" + this._ReferredNodeId.ToString();
                }
                break;
            default:
                break;
       }
    }

    /// <summary>
    /// Retrieve all web pages in from the directotory of the web site and display them in Repeater repLocalPages.
    /// </summary>
    /// <author></author>
    /// <date>18/03/2008</date>
    private void LoadAllLocalPages()
    {
        //Get all web pages existing on the web site.
        string webSitePath = Server.MapPath("~/");

        List<string> fileListVirtualPaths = new List<string>();
        foreach (string filePhysicalPath in Directory.GetFiles(webSitePath, "*", SearchOption.AllDirectories))
        {
            string fileVirtualPath = "~/" + filePhysicalPath.Remove(0, webSitePath.Length).Replace("\\", "/");
            if (Regex.IsMatch(fileVirtualPath, CMSSettings.LocalPagesFilter, RegexOptions.IgnoreCase))
            {
                fileListVirtualPaths.Add(fileVirtualPath);
            }
        }
       
        //Sort alphabetically
        fileListVirtualPaths.Sort();

        repLocalPages.DataSource = fileListVirtualPaths;
        repLocalPages.DataBind();
    }

    /// <summary>
    /// Retrieve all pages that could be referred to create menu pages and display them in Repeater repMenuPages.
    /// </summary>
    /// <author></author>
    /// <date>18/03/2008</date>
    private void LoadAllCandidateMenuPages()
    {
        DataTable dtCandidateMenuPages = Node.ListCandidateMenuPages();

        repMenuPages.DataSource = dtCandidateMenuPages;
        repMenuPages.DataBind();           
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>11/03/2008</date>
    public virtual void OnChangeStaticPageType(object sender, ChangeStaticPageTypeArgs e)
    {
        DisplayNodeTypeControls(e.NewType);
        if (ChangeStaticPageType != null)
        {
            ChangeStaticPageType(sender, e);
        }
    }
}

/// <summary>
/// Event arguments for event ChangeStaticPageTypeEvent.
/// </summary>
public class ChangeStaticPageTypeArgs:EventArgs
{
    private NodeType _NewType;
    /// <summary>
    /// The new node type which was selected.
    /// </summary>
    public NodeType NewType
    {
        get { return _NewType; }
        set { _NewType = value; }
    }
}
/// <summary>
/// Represents event handler of event ChangeStaticPageTypeEvent.
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void ChangeStaticPageTypeEventHandler(object sender, ChangeStaticPageTypeArgs args);
