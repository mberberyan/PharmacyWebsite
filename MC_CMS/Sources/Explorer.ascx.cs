using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.CMS;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Exception;
using System.Text.RegularExpressions;
using System.Web.Security;
using Melon.Components.CMS.Configuration;


/// <summary>
/// Provides user interface for displaying and managing tree of all existing cms nodes.
/// </summary>
/// <remarks>
/// <para>
///     Controls which build user control Explorer:
///     <list type="table">
///         <listheader>
///             <item><term>Web Control</term><description>Description</description></item>
///         </listheader>
///         <item><term>TreeView tvCMSExplorer</term><description>Used to display the hierarchical structure of cms nodes built by CMS system.</description></item>
///         <item><term>DropDown lblFilterTitle</term><description>Used to filter the cms nodes in the tree by visibility permissions.</description></item>
///         <item><term>ImageButton ibtnCreateFolder</term><description>Allows creation of node from type <see cref="NodeType.Folder"/>.</description></item>
///         <item><term>ImageButton ibtnCreateContentPage</term><description>Allows creation of node from type <see cref="NodeType.ContentManageablePage"/>.</description></item>    
///         <item><term>ImageButton ibtnCreateStaticPage</term><description>Allows creation of node from type <see cref="NodeType.StaticLocalPage"/>,<see cref="NodeType.StaticExternalPage"/> or <see cref="NodeType.StaticMenuPage"/>.</description></item>
///         <item><term>ImageButton ibtnRecursivePublish</term>Allows recursive publish of the selected in the tree node for the currently selected language.If the root node is selected the whole tree is published.<description></description></item>
///         <item><term>ImageButton ibtnMoveUp</term><description>Allows moving up of the selected in the tree node.</description></item>
///         <item><term>ImageButton ibtnMoveDown</term><description>Allows moving down of the selected in the tree node.</description></item>
///         <item><term>ImageButton ibtnMoveLeft</term><description>Allows moving left of the selected in the tree node.</description></item>
///         <item><term>ImageButton ibtnMoveRight</term><description>Allows moving right of the selected in the tree node.</description></item>
///         <item><term>ImageButton ibtnDelete</term><description>Allows deletion of the  selected in the tree node.</description></item>
///     </list>
/// </para>
/// <para>
///     All existing cms nodes are retrieved with method <see cref="Node.ListCMSExplorerNodes(string)"/> of class <see cref="Node"/> 
///     and are displayed in TreeView with root "Explorer". 
///     There are different icons (located in MC_CMS/Sources/CMS_Styles/Images) for the different types of nodes and specific icon for these nodes which are hidden in navigation. 
///     Method <see cref="GetTreeNodeImageIcon(NodeType,bool)"/> returns the icon to display in the treeview on the base of the type and hidden in navigation flag.
/// </para>
/// </remarks>
public partial class Explorer : CMSControl
{
    #region Fields & Properties

    /// <summary>
    /// Identifier of the currently selected node.
    /// </summary>
    public int? SelectedNodeId;

    #endregion


    /// <summary>
    /// Initializes the control's properties
    /// </summary>
    /// <param name="args">The values with which the properties will be initialized</param>
    public override void Initializer(object[] args)
    {
        this.SelectedNodeId = (int?)args[0];
    }

    /// <summary>
    /// Attach event handlers to the controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>07/03/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.ddlFilter.SelectedIndexChanged += new EventHandler(ddlFilter_SelectedIndexChanged);

        this.tvCMSExplorer.TreeNodeDataBound += new TreeNodeEventHandler(tvCMSExplorer_TreeNodeDataBound);
        this.tvCMSExplorer.SelectedNodeChanged += new EventHandler(tvCMSExplorer_SelectedNodeChanged);

        this.ibtnCreateFolder.Click += new ImageClickEventHandler(ibtnCreateFolder_Click);
        this.ibtnCreateContentPage.Click += new ImageClickEventHandler(ibtnCreateContentPage_Click);
        this.ibtnCreateStaticPage.Click += new ImageClickEventHandler(ibtnCreateStaticPage_Click);
        this.ibtnRecursivePublish.Click += new ImageClickEventHandler(ibtnRecursivePublish_Click);
        this.ibtnMoveUp.Click += new ImageClickEventHandler(ibtnMoveUp_Click);
        this.ibtnMoveDown.Click += new ImageClickEventHandler(ibtnMoveDown_Click);
        this.ibtnMoveLeft.Click += new ImageClickEventHandler(ibtnMoveLeft_Click);
        this.ibtnMoveRight.Click += new ImageClickEventHandler(ibtnMoveRight_Click);
        this.ibtnDelete.Click += new ImageClickEventHandler(ibtnDelete_Click);

        this.ParentControl.CreateFolderEvent += new CreateFolderEventHandler(ParentControl_CreateFolderEvent);
        this.ParentControl.CreateStaticPageEvent += new CreateStaticPageEventHandler(ParentControl_CreateStaticPageEvent);
        this.ParentControl.CreateContentPageEvent += new CreateContentPageEventHandler(ParentControl_CreateContentPageEvent);

        base.OnInit(e);
    }        

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    /// <para>When the user control is loaded for first time initial state of the control is formed by calling methods:</para>
    /// <list type="bullet">
    ///     <item>
    ///         <see cref="LoadFilterOptions"/> - to load filter options in DropDownList ddlFilter.
    ///         If filter option is passed to the user control in property <see cref="Melon.Components.CMS.CMS.VisibilityFilter"/> it is selected in DropDownList ddlFilter.
    ///     </item>
    ///     <item>
    ///         <see cref="LoadCMSExplorer"/> - to load in TreeView tvCMSExplorer the CMS nodes which correspond to the visibility filter currently selected.
    ///         If node id is passed to the user control in property <see cref="SelectedNodeId"/> the corresponding node is selected in TreeView tvCMSExplorer,
    ///         otherwise the root node "Explorer" is selected.
    ///     </item>
    ///     <item>
    ///         Also the visibility and accessibility of the action buttons for the CMS tree is set with method <see cref="ManageActionButtons"/>.
    ///     </item>
    /// </list>
    /// </remarks>  
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>07/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            //Databind image buttons to estimate their imageURL.
            ibtnCreateFolder.DataBind();
            ibtnCreateContentPage.DataBind();
            ibtnCreateStaticPage.DataBind();
            ibtnRecursivePublish.DataBind();
            ibtnMoveUp.DataBind();
            ibtnMoveDown.DataBind();
            ibtnMoveLeft.DataBind();
            ibtnMoveRight.DataBind();
            ibtnDelete.DataBind();

            //Load all filter options in dropdown. 
            LoadFilterOptions();
          
            //Select in the dropdown the passed to the user control filter. 
            if (!String.IsNullOrEmpty(Melon.Components.CMS.CMS.VisibilityFilter))
            {
                ddlFilter.SelectedIndex = ddlFilter.Items.IndexOf(ddlFilter.Items.FindByValue(Melon.Components.CMS.CMS.VisibilityFilter));
            }

            //Load in the CMS tree all nodes that correspond to the visibility filter.
            //Select in the tree the node with id that is passed to the user control.
            //If there is no passed node id, select the root node: "Explorer".
            LoadCMSExplorer(Melon.Components.CMS.CMS.VisibilityFilter);

            if (this.SelectedNodeId != null)
            {
                TreeNode selectedNode = FindNode(this.SelectedNodeId.Value);
                if (selectedNode != null)
                {
                    selectedNode.Select();
                    ExpandNodeParents(selectedNode);
                }
                else
                {
                    //The node with id=SelectedNodeId doesn't exist in the tree.

                    //Display error message.
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedNodeByAnotherUserErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    //Refresh CMS explorer;
                    LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                    args.ExplorerNodeType = NodeType.Explorer;
                    args.RefreshExplorer = true;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, args);

                    return;
                }
            }
            else
            {
                //Select node "Explorer".
                tvCMSExplorer.Nodes[0].Select();
            }


            //Store in hidden field hfExpandedNodes ids of expanded nodes.
            if (this.ParentControl.ExpandedNodes == null)
            {
                string expandedNodeIds = "";
                GetExpandedNodes(tvCMSExplorer.Nodes[0], ref expandedNodeIds);
                hfExpandedNodes.Value = expandedNodeIds;
            }
            else
            {
                hfExpandedNodes.Value = ",";
                foreach (int? id in this.ParentControl.ExpandedNodes)
                {
                    if (id.HasValue)
                    {
                        hfExpandedNodes.Value += id.Value + ",";
                    }
                    else
                    {
                        hfExpandedNodes.Value += "null,";
                    }
                }
            }
            ManageActionButtons();
        }
        else
        {
            StoreExpandedNodes();

            //Event SelectedNodeChanged wasn't fired when we select already selected node.
            //So we force firing of the event.
            string eTarget = Request.Form["__EVENTTARGET"];
            string[] eArgument = Request.Form["__EVENTARGUMENT"].Replace("\\", "|").Split('|');
            //eArguments is array with the following format: 
            //First element [0] contains string which indicates what is the event. "s" is for selection of node.
            //The array has other n elements which is the level on which is the node which is selected.
            //The last element of the array is the selected node value (cms node id). 
            //The previous elements are the ids of the selected node parent nodes.

            //Check whether the event is raised by the treeview tvCMSExplorer.
            if (eTarget == tvCMSExplorer.UniqueID)
            {
                //Check whether this event is selection of node.
                if (eArgument[0].ToLower() == "s")
                {
                    if (eArgument.Length > 1)
                    {
                        if (tvCMSExplorer.SelectedValue != null && tvCMSExplorer.SelectedValue == eArgument[eArgument.Length - 1])
                        {
                            tvCMSExplorer.Nodes[0].Select();
                            tvCMSExplorer.SelectedNode.Select();
                        }
                    }
                    else
                    {
                        //"Explorer" is selected.
                        if (string.IsNullOrEmpty(tvCMSExplorer.SelectedValue))
                        {
                            if (tvCMSExplorer.Nodes[0].ChildNodes.Count > 0)
                            {
                                tvCMSExplorer.Nodes[0].ChildNodes[0].Select();
                                tvCMSExplorer.SelectedNode.Select();
                            }
                            else
                            {
                                tvCMSExplorer.Nodes[0].ChildNodes.Add(new TreeNode());
                                tvCMSExplorer.Nodes[0].ChildNodes[0].Select();
                                tvCMSExplorer.SelectedNode.Select();
                                tvCMSExplorer.Nodes[0].ChildNodes.Clear();
                            }
                        }
                    }
                }
            }
        }
      
    }


    /// <summary>
    /// Event handler for event Click of DropDownList ddlFilter.
    /// </summary>
    /// <remarks>
    /// Raises event LoadSettingsEvent of parent user control.
    /// This is done because the CMS tree should be rebind with the nodes that are visible 
    /// for the user group just selected in ddlFilter 
    /// and the settings of the previously selected node should be closed because we don't know whether 
    /// this node will continue to be visibly after filtering.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            Melon.Components.CMS.CMS.VisibilityFilter = ddlFilter.SelectedValue;

            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.RefreshExplorer = true;
            args.ExplorerNodeType = NodeType.Explorer;
            this.ParentControl.OnLoadWorkAreaEvent(sender, args);
        }
        else
        {
            
            this.ParentControl.RedirectToLoginPage();
        }
    }


    /// <summary>
    /// Event handler for event TreeNodeDataBound of TreeView tvCMSExplorer.
    /// </summary>
    /// <remarks>
    ///    The method is used to set ImageURL, Text, Value of tree node in tvCMSExplorer.
    ///    <list type="bullet">
    ///         <item>ImageURL - depends of properties Type and IsHiddenInNavigation of <see cref="Melon.Components.CMS.Node"/>. 
    ///             Method <see cref="GetTreeNodeImageIcon"/> is called to retrieve the path to the correct image for the current node.</item>
    ///         <item>Text - it is set equal to property Code of <see cref="Melon.Components.CMS.Node"/>.</item>
    ///         <item>Value - it is set equal to property Id of <see cref="Melon.Components.CMS.Node"/>.</item>
    ///    </list>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected void tvCMSExplorer_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        XmlElement currentNode = (XmlElement)e.Node.DataItem;
        NodeType nodeType = (currentNode.Attributes["TypeId"] != null) ? (NodeType)Convert.ToInt32(currentNode.Attributes["TypeId"].Value) : NodeType.Explorer;

        if (nodeType == NodeType.Explorer)
        {
            e.Node.Text = "<img src='" + ResolveUrl(Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/explorer.gif"))
                + "' title='" + Convert.ToString(GetLocalResourceObject("Explorer")) + "' border='0'/>"
                + "<span>" + Convert.ToString(GetLocalResourceObject("Explorer")) + "</span>";

            e.Node.Value = "";
            if (this.ParentControl.ExpandedNodes != null)
            {
                if (this.ParentControl.ExpandedNodes.Contains(null))
                {
                    e.Node.Expanded = true;
                }
                else
                {
                    e.Node.Expanded = false;
                }
            }
        }
        else
        {
            string tooltip = "";
            switch (nodeType)
            {
                case NodeType.Folder:
                    tooltip = Convert.ToString(GetLocalResourceObject("Folder"));
                    break;
                case NodeType.ContentManageablePage:
                    tooltip = Convert.ToString(GetLocalResourceObject("ContentManageablePage"));
                    break;
                case NodeType.StaticLocalPage:
                    tooltip = Convert.ToString(GetLocalResourceObject("StaticLocalPage"));
                    break;
                case NodeType.StaticExternalPage:
                    tooltip = Convert.ToString(GetLocalResourceObject("StaticExternalPage"));
                    break;
                case NodeType.StaticMenuPage:
                    tooltip = Convert.ToString(GetLocalResourceObject("StaticMenuPage"));
                    break;
            }
            bool isHiddenInNavigation = Convert.ToBoolean(Convert.ToInt32(currentNode.Attributes["IsHiddenInNavigation"].Value));
            e.Node.Text = "<img src='" + ResolveUrl(Utilities.GetImageUrl(this.Page, GetTreeNodeImageIcon(nodeType, isHiddenInNavigation)))
                + "' title='" + tooltip + "' border='0'/>"
                + "<span>" + currentNode.Attributes["Code"].Value + "</span>";

            e.Node.Value = currentNode.Attributes["Id"].Value;
            if (this.ParentControl.ExpandedNodes != null)
            {
                if (this.ParentControl.ExpandedNodes.Contains(Convert.ToInt32(e.Node.Value)))
                {
                    e.Node.Expanded = true;
                }
                else
                {
                    e.Node.Expanded = false;
                }
            }
        }
    }

    /// <summary>
    /// Event handler for event SelectedNodeChanged of TreeView tvCMSExplorer.
    /// </summary>
    /// <remarks>
    ///     Raises event LoadSettingsEvent of the parent user control 
    ///     in order to load settings of the cms node just selected in TreeView tvCMSExplorer.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected void tvCMSExplorer_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            int? selectedNodeId = (String.IsNullOrEmpty(tvCMSExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCMSExplorer.SelectedNode.Value);

            LoadWorkAreaArgs args = new LoadWorkAreaArgs();
            args.RefreshExplorer = true;
            args.NodeId = selectedNodeId;
            this.ParentControl.OnLoadWorkAreaEvent(sender, args);
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }               

    /// <summary>
    /// Calls the method to create a new folder
    /// </summary>
    /// <author>Mario Berberyan</author>
    /// <date>287/07/2009</date>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ParentControl_CreateFolderEvent(object sender, EventArgs e)
    {
        ibtnCreateFolder_Click(sender, new ImageClickEventArgs(0, 0));
    }

    /// <summary>
    /// Event handler for event Click of ImageButton ibtnCreateFolder.
    /// </summary>
    /// <remarks>
    ///     Raises event LoadSettingsEvent of the parent user control in order to load settings for node type Folder.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="Melon.Components.CMS.NodeType"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnCreateFolder_Click(object sender, ImageClickEventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            int? nodeParentId = (String.IsNullOrEmpty(tvCMSExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCMSExplorer.SelectedNode.Value);

            if (tvCMSExplorer.SelectedNode.Depth < CMSSettings.MaxNodesDepth)
            {
                if (nodeParentId.HasValue && !Node.Exists(nodeParentId.Value))
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("FolderParentNotExistsErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    LoadWorkAreaArgs argsRefresh = new LoadWorkAreaArgs();
                    argsRefresh.ExplorerNodeType = NodeType.Explorer;
                    argsRefresh.RefreshExplorer = true;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, argsRefresh);

                    return;
                }

                string nodeLocation = "";
                GetNodeLocation(tvCMSExplorer.SelectedNode, ref nodeLocation);

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.ExplorerNodeType = NodeType.Folder;
                args.NodeParentId = nodeParentId;
                args.NodeLocation = nodeLocation;
                args.RefreshExplorer = true;
                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
            else
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("MaximumNodesDepthExceeded")),CMSSettings.MaxNodesDepth);
                this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.NodeId = nodeParentId;
                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
        
    }

    /// <summary>
    /// Calls the method to create a new content-manageable page
    /// </summary>
    /// <author>Mario Berberyan</author>
    /// <date>287/07/2009</date>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ParentControl_CreateContentPageEvent(object sender, EventArgs e)
    {
        ibtnCreateContentPage_Click(sender, new ImageClickEventArgs(0, 0));
    }

    /// <summary>
    /// Event handler for event Click of ImageButton ibtnCreateContentPage.
    /// </summary>
    /// <remarks>
    ///     Raises event LoadSettingsEvent of the parent user control in order to load settings for node type ContentManageablePage.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="Melon.Components.CMS.NodeType"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnCreateContentPage_Click(object sender, ImageClickEventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            int? nodeParentId = (String.IsNullOrEmpty(tvCMSExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCMSExplorer.SelectedNode.Value);

            if (tvCMSExplorer.SelectedNode.Depth < CMSSettings.MaxNodesDepth)
            {
                if (nodeParentId.HasValue && !Node.Exists(nodeParentId.Value))
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("PageParentNotExistsErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    LoadWorkAreaArgs argsRefresh = new LoadWorkAreaArgs();
                    argsRefresh.ExplorerNodeType = NodeType.Explorer;
                    argsRefresh.RefreshExplorer = true;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, argsRefresh);

                    return;
                }

                string nodeLocation = "";
                GetNodeLocation(tvCMSExplorer.SelectedNode, ref nodeLocation);

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.ExplorerNodeType = NodeType.ContentManageablePage;
                args.NodeParentId = nodeParentId;
                args.NodeLocation = nodeLocation;
                args.RefreshExplorer = true;
                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
            else
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("MaximumNodesDepthExceeded")), CMSSettings.MaxNodesDepth);
                this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.NodeId = nodeParentId;
                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Calls the method to create a new static page
    /// </summary>
    /// <author>Mario Berberyan</author>
    /// <date>287/07/2009</date>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ParentControl_CreateStaticPageEvent(object sender, EventArgs e)
    {
        ibtnCreateStaticPage_Click(sender, new ImageClickEventArgs(0, 0));
    }

    /// <summary>
    /// Event handler for event Click of ImageButton ibtnCreateStaticPage.
    /// </summary>
    /// <remarks>
    ///     Raises event LoadSettingsEvent of the parent user control in order to load settings for node type StaticLocalPage.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="Melon.Components.CMS.NodeType"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnCreateStaticPage_Click(object sender, ImageClickEventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            int? nodeParentId = (String.IsNullOrEmpty(tvCMSExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCMSExplorer.SelectedNode.Value);

            if (tvCMSExplorer.SelectedNode.Depth < CMSSettings.MaxNodesDepth)
            {
                if (nodeParentId.HasValue && !Node.Exists(nodeParentId.Value))
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("PageParentNotExistsErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    LoadWorkAreaArgs argsRefresh = new LoadWorkAreaArgs();
                    argsRefresh.ExplorerNodeType = NodeType.Explorer;
                    argsRefresh.RefreshExplorer = true;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, argsRefresh);

                    return;
                }

                string nodeLocation = "";
                GetNodeLocation(tvCMSExplorer.SelectedNode, ref nodeLocation);

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.ExplorerNodeType = NodeType.StaticLocalPage;
                args.NodeParentId = nodeParentId;
                args.NodeLocation = nodeLocation;
                args.RefreshExplorer = true;
                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
            else
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("MaximumNodesDepthExceeded")), CMSSettings.MaxNodesDepth);
                this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.NodeId = nodeParentId;
                this.ParentControl.OnLoadWorkAreaEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }


    /// <summary>
    /// Event handler for event Click of ImageButton ibtnRecursivePublish.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnRecursivePublish_Click(object sender, ImageClickEventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator || currentUserRole == CMSRole.Administrator)
            {
                TreeNode selectedNode = tvCMSExplorer.SelectedNode;
                if (selectedNode != null)
                {
                    int? nodeId = String.IsNullOrEmpty(selectedNode.Value) ? (int?)null : Convert.ToInt32(selectedNode.Value);
                    Match nodeCodeMatch = Regex.Match(tvCMSExplorer.SelectedNode.Text, "(.*?)<span>(.*?)</span>");
                    string nodeCode = nodeCodeMatch.Groups[2].Value;

                    try
                    {
                        Node.RecursivePublish(nodeId, this.ParentControl.CurrentLanguage);
                    }
                    catch (CMSException ex)
                    {
                        DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                        switch (ex.Code)
                        {
                            case CMSExceptionCode.ParentNotPublished:
                                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("ParentNotPublishedErrorMessage"));
                                break;
                            case CMSExceptionCode.PublishMenuPageWithoutReferrence:
                                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("PublishMenuPageWithoutReferrenceErrorMessage")),
                                                                    ex.AdditionalInfo);
                                break;
                            case CMSExceptionCode.PublishWithoutDefaultVersion:
                                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("PublishWithoutDefaultVersionErrorMessage")),
                                                                  ex.AdditionalInfo);
                                break;
                            case CMSExceptionCode.PublishWithoutDraft:
                                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("PublishWithoutDraftErrorMessage")),
                                                                 ex.AdditionalInfo);
                                break;
                            case CMSExceptionCode.UnauthorizedAccessException:
                                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("UnauthorizedAccessErrorMessage")),
                                                                     ex.AdditionalInfo);
                                break;
                            case CMSExceptionCode.FileNotFoundException:
                                errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("FileNotFoundErrorMessage")),
                                                                     ex.AdditionalInfo);
                                break;
                            default:
                                throw;

                        }

                        this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);
                        return;
                    }
                    catch
                    {
                        DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                        errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("RecursivePublishErrorMessage"));
                        this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);
                        return;
                    }

                    //Successful recursive publish.

                    LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                    args.NodeId = nodeId;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, args);
                }

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
    /// Event handler for event Click of ImageButton ibtnMoveUp.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="MoveNode"/> with parameter <see cref="NodeMoveDirection.Up"/>.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="MoveNode"/>
    /// <seealso cref="Melon.Components.CMS.NodeMoveDirection"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnMoveUp_Click(object sender, ImageClickEventArgs e)
    {
        MoveNode(NodeMoveDirection.Up);
    }

    /// <summary>
    /// Event handler for event Click of ImageButton ibtnMoveDown.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="MoveNode"/> with parameter <see cref="NodeMoveDirection.Down"/>.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="MoveNode"/>
    /// <seealso cref="Melon.Components.CMS.NodeMoveDirection"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnMoveDown_Click(object sender, ImageClickEventArgs e)
    {
        MoveNode(NodeMoveDirection.Down);
    }

    /// <summary>
    /// Event handler for event Click of ImageButton ibtnMoveLeft.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="MoveNode"/> with parameter <see cref="NodeMoveDirection.Left"/>.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="MoveNode"/>
    /// <seealso cref="Melon.Components.CMS.NodeMoveDirection"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnMoveLeft_Click(object sender, ImageClickEventArgs e)
    {
        MoveNode(NodeMoveDirection.Left);
    }

    /// <summary>
    /// Event handler for event Click of ImageButton ibtnMoveRight.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="MoveNode"/> with parameter <see cref="NodeMoveDirection.Right"/>.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="MoveNode"/>
    /// <seealso cref="Melon.Components.CMS.NodeMoveDirection"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnMoveRight_Click(object sender, ImageClickEventArgs e)
    {
        MoveNode(NodeMoveDirection.Right);
    }


    /// <summary>
    /// Event handler for event Click of ImageButton ibtnDelete.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Calls method <see cref="Melon.Components.CMS.Node.Delete"/> to delete the currently selected node 
    ///         in TreeView tvCMSExplorer. Node couldn't be deleted if it is referred i.e. it is used by a menu page.
    ///         When such node is tried to be deleted error message is displayed in Label lblErrorMessage.</para>
    ///     <para>
    ///         If node is deleted successfully then event LoadSettingsEvent of the parent user control is raised 
    ///         in order to refresh the tree. 
    ///         If there is error during the deletion then event for displaying error message of the parent control is raised.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <seealso cref="Melon.Components.CMS.Node.Delete"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole != CMSRole.None)
            {
                TreeNode selectedNode = tvCMSExplorer.SelectedNode;
                int nodeId = Convert.ToInt32(selectedNode.Value);
                Match nodeCodeMatch = Regex.Match(tvCMSExplorer.SelectedNode.Text, "(.*?)<span>(.*?)</span>");
                string nodeCode = nodeCodeMatch.Groups[2].Value;

                try
                {
                    Node.Delete(nodeId);
                }
                catch (CMSException ex)
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    if (ex.Code == CMSExceptionCode.DeleteReferredNode)
                    {
                        errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("DeleteReferredNodeErrorMessage")), nodeCode, ex.AdditionalInfo);
                    }
                    else if (ex.Code == CMSExceptionCode.UnauthorizedAccessException)
                    {
                        errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("UnauthorizedAccessErrorMessage")), ex.AdditionalInfo);
                    }
                    else if (ex.Code == CMSExceptionCode.DeleteNodeWithChildren)
                    {
                        errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("DeleteNodeWithChildrenErrorMessage")), nodeCode);
                    }
                    else
                    {
                        errorArgs.ErrorMessage = ex.Message;
                    }

                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                    args.RefreshExplorer = true;
                    args.NodeId = nodeId;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, args);

                    return;
                }
                catch
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("NodeDeleteErrorMessage")), nodeCode);
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                    args.RefreshExplorer = true;
                    args.NodeId = nodeId;
                    this.ParentControl.OnLoadWorkAreaEvent(sender, args);

                    return;
                }

                //Successful delete.We refresh the CMS Explorer Tree and load settings of parent node.

                Melon.Components.CMS.CMS.IsSitemapStructureChanged = true;

                LoadWorkAreaArgs argsRefresh = new LoadWorkAreaArgs();
                argsRefresh.RefreshExplorer = true;
                argsRefresh.NodeId = (String.IsNullOrEmpty(selectedNode.Parent.Value)) ? (int?)null : Convert.ToInt32(selectedNode.Parent.Value);
                this.ParentControl.OnLoadWorkAreaEvent(sender, argsRefresh);
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
    /// Sets accessibility of the action buttons for the CMS tree: Move Up, Move Down, Move Left, Move Rigth, Delete, Publish.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The accessibility of the buttons depends of the CMS role of the currently logged user and from 
    ///     the currently selected node.</para>
    /// <para>
    ///     CMS role accessibility restrictions: For writers it is not allowed to move, delete and publish nodes so
    ///     buttons ibtnMoveUp, ibtnMoveDown, ibtnMoveLeft, ibtnMoveRight, ibtnDelete, ibtnPublish are disabled.</para>
    /// <para>
    ///     Node accessibility restrictions:
    ///     <list type="bullet">
    ///         <item>ibtnMoveUp - disabled if selected node is first child of its parent.</item>
    ///         <item>ibtnMoveDown - disabled if selected node is last child of its parent.</item>
    ///         <item>ibtnMoveLeft - disabled if selected node has no grand-parent.</item>
    ///         <item>ibtnMoveRight - disabled if selected node is first child of its parent.</item>
    ///         <item>ibtnDelete - disabled if selected node has childs.</item>
    ///     </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="Melon.Components.CMS.CMSRole"/>
    /// <author></author>
    /// <date>10/03/2008</date>
    private void ManageActionButtons()
    {
        if (this.ParentControl.CurrentUser != null)
        {
            if (this.ParentControl.CurrentUser.IsInCMSRole(CMSRole.Writer))
            {
                //The current logged user in CMS system is Writer. 
                //So he could not delete,move,publish nodes.
                ibtnRecursivePublish.Enabled = false;
                ibtnRecursivePublish.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/publish_disabled.gif");
                ibtnMoveUp.Enabled = false;
                ibtnMoveUp.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_up_disabled.gif");
                ibtnMoveDown.Enabled = false;
                ibtnMoveDown.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_down_disabled.gif");
                ibtnMoveLeft.Enabled = false;
                ibtnMoveLeft.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_left_disabled.gif");
                ibtnMoveRight.Enabled = false;
                ibtnMoveRight.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_right_disabled.gif");
                ibtnDelete.Enabled = false;
                ibtnDelete.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/delete_disabled.gif");
            }
            else
            {
                ibtnRecursivePublish.Enabled = true;
                ibtnRecursivePublish.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/publish.gif");

                TreeNode currentNode = tvCMSExplorer.SelectedNode;
                if (currentNode.Parent != null)
                {
                    //CMS Node is selected.

                    //Check whether the current node is first child of its parent.
                    if (currentNode.Parent.ChildNodes.IndexOf(currentNode) == 0)
                    {
                        //The current node is first child of its parent so it is not allowed to move it up or right.
                        ibtnMoveUp.Enabled = false;
                        ibtnMoveUp.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_up_disabled.gif");
                        ibtnMoveRight.Enabled = false;
                        ibtnMoveRight.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_right_disabled.gif");
                    }
                    else
                    {
                        ibtnMoveUp.Enabled = true;
                        ibtnMoveUp.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_up.gif");
                        ibtnMoveRight.Enabled = true;
                        ibtnMoveRight.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_right.gif");
                    }

                    //Check whether the current node is last child of its parent.
                    if (currentNode.Parent.ChildNodes.IndexOf(currentNode) == (currentNode.Parent.ChildNodes.Count - 1))
                    {
                        //The current node is last child of its parent so it is not allowed to move it down.
                        ibtnMoveDown.Enabled = false;
                        ibtnMoveDown.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_down_disabled.gif");
                    }
                    else
                    {
                        ibtnMoveDown.Enabled = true;
                        ibtnMoveDown.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_down.gif");
                    }

                    //Check whether the current node has grand-parent.
                    if (currentNode.Parent.Parent == null)
                    {
                        ibtnMoveLeft.Enabled = false;
                        ibtnMoveLeft.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_left_disabled.gif");
                    }
                    else
                    {
                        ibtnMoveLeft.Enabled = true;
                        ibtnMoveLeft.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_left.gif");
                    }

                    //Check whether the current node has childs.
                    if (currentNode.ChildNodes.Count > 0)
                    {
                        //The current node has childs so deletion is not allowed.
                        ibtnDelete.Enabled = false;
                        ibtnDelete.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/delete_disabled.gif");
                    }
                    else
                    {
                        ibtnDelete.Enabled = true;
                        ibtnDelete.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/delete.gif");
                    }

                }
                else
                {
                    //"Explorer" is selected.
                    ibtnMoveUp.Enabled = false;
                    ibtnMoveUp.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_up_disabled.gif");
                    ibtnMoveDown.Enabled = false;
                    ibtnMoveDown.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_down_disabled.gif");
                    ibtnMoveLeft.Enabled = false;
                    ibtnMoveLeft.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_left_disabled.gif");
                    ibtnMoveRight.Enabled = false;
                    ibtnMoveRight.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/move_right_disabled.gif");
                    ibtnDelete.Enabled = false;
                    ibtnDelete.ImageUrl = Utilities.GetImageUrl(this.Page, "CMS_Styles/Images/delete_disabled.gif");
                }
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Loads filter options in DropDownList ddlFilter.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Each filter option represents a group of users.
    ///     When filter options is selected in the TreeView tvCMSExplorer will be displayed only the nodes which are visible
    ///     for the group of users correspoding to the filter options.</para>
    /// <para>
    ///     Filter options are all user roles from the web site where the CMS component is integrated 
    ///     and the following additional options:
    /// <list type="bullet">
    ///     <item>"--" - When selected there is no node filtering.</item>
    ///     <item>"Anonymous  users" - When selected are in the CMS tree are listed only the nodes visible for not logged, anonymous users.</item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <returns>DataTable with two columns: Code and Name.</returns>
    /// <author></author>
    /// <date>10/03/2008</date>
    private void LoadFilterOptions()
    {
        //Get all roles.
        DataTable rolePermissionOptions = Role.List();

        //Fill in visibility filter options.
        DataTable dtVisibilityFilter = rolePermissionOptions.Copy();

        //Add option "--".
        DataRow row0 = dtVisibilityFilter.NewRow();
        row0["Code"] = "";
        row0["Name"] = "---";
        dtVisibilityFilter.Rows.InsertAt(row0, 0);

        //Add option "Anonymous users".
        DataRow row1 = dtVisibilityFilter.NewRow();
        row1["Code"] = "__mc_cms_AnonymousUsers";
        row1["Name"] = Convert.ToString(GetLocalResourceObject("AnonymousUsers"));
        dtVisibilityFilter.Rows.InsertAt(row1, 1);

        if (rolePermissionOptions.Rows.Count == 0)
        {
            //There are no roles so =>
            //Add option "Logged users".
            DataRow row2 = dtVisibilityFilter.NewRow();
            row2["Code"] = "__mc_cms_LoggedUsers";
            row2["Name"] = Convert.ToString(GetLocalResourceObject("LoggedUsers"));
            dtVisibilityFilter.Rows.InsertAt(row2, 2);
        }

        //Databind dropdown with filter options.
        ddlFilter.DataSource = dtVisibilityFilter;
        ddlFilter.DataBind();
    }

    /// <summary>
    /// Loads cms nodes in TreeView tvCMSExplorer.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="Melon.Components.CMS.Node.ListCMSExplorerNodes"/> to retrieve xml with all nodes
    ///     visible for the specified user group <paramref name="visibilityFilter"/> and databind TreeView tvCMSExplorer with this xml.
    /// </remarks>
    /// <param name="visibilityFilter">User group for which to be visible the nodes in the tree.</param>
    /// <author></author>
    /// <date>10/03/2008</date>
    private void LoadCMSExplorer(string visibilityFilter)
    {
        XmlDataSource xml = new XmlDataSource();
        xml.EnableCaching = false;
        xml.Data = Node.ListCMSExplorerNodes(visibilityFilter).DocumentElement.OuterXml;

        tvCMSExplorer.DataSource = xml;
        tvCMSExplorer.DataBind();
    }

    /// <summary>
    /// Moves tree node in the desired direction.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Calls method <see cref="Melon.Components.CMS.Node.Move"/> to move the currently selected node 
    ///         in TreeView tvCMSExplorer in the direction specified in the parameter <paramref name="direction"/>.</para>
    ///     <para>
    ///         If node is moved successfully then event LoadSettingsEvent of the parent control is raised 
    ///         in order to refresh the tree. 
    ///         If there is error during the move then event for displaying error message of the parent control is raised.</para>
    /// </remarks>
    /// <param name="direction">Direction in which to move the node.</param>
    /// <seealso cref="Melon.Components.CMS.NodeMoveDirection"/>
    /// <seealso cref="Melon.Components.CMS.Node.Move"/>
    /// <author></author>
    /// <date>12/03/2008</date>
    private void MoveNode(NodeMoveDirection direction)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole != CMSRole.None)
            {
                TreeNode selectedNode = tvCMSExplorer.SelectedNode;
                int nodeId = Convert.ToInt32(selectedNode.Value);
                Match nodeCodeMatch = Regex.Match(tvCMSExplorer.SelectedNode.Text, "(.*?)<span>(.*?)</span>");
                string nodeCode = nodeCodeMatch.Groups[2].Value;

                try
                {
                    Node.Move(nodeId, direction);
                }
                catch (CMSException ex)
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    if (ex.Code == CMSExceptionCode.UnauthorizedAccessException)
                    {
                        errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("UnauthorizedAccessErrorMessage")), ex.AdditionalInfo);
                        this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);
                        return;
                    }
                    if (ex.Code == CMSExceptionCode.NodeNotFound)
                    {
                        errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedNodeByAnotherUserErrorMessage"));
                        this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                        LoadWorkAreaArgs refreshArgs = new LoadWorkAreaArgs();
                        refreshArgs.ExplorerNodeType = NodeType.Explorer;
                        refreshArgs.RefreshExplorer = true;
                        this.ParentControl.OnLoadWorkAreaEvent(this.Page, refreshArgs);

                        return;
                    }
                    else if (ex.Code == CMSExceptionCode.PreviousNodeNotExist
                        || ex.Code == CMSExceptionCode.NextNodeNotExist
                        || ex.Code == CMSExceptionCode.ParentNodeNotExist)
                    {
                        LoadWorkAreaArgs refreshArgs = new LoadWorkAreaArgs();
                        refreshArgs.RefreshExplorer = true;
                        refreshArgs.NodeId = nodeId;
                        this.ParentControl.OnLoadWorkAreaEvent(this.Page, refreshArgs);
                        return;
                    }
                    else if (ex.Code == CMSExceptionCode.NodeDuplicateAlias)
                    {
                        errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("NodeDuplicateAliasErrorMessage"));
                        this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                        LoadWorkAreaArgs refreshArgs = new LoadWorkAreaArgs();
                        refreshArgs.ExplorerNodeType = NodeType.Explorer;
                        refreshArgs.RefreshExplorer = true;
                        this.ParentControl.OnLoadWorkAreaEvent(this.Page, refreshArgs);

                        return;
                    }

                }
                catch
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("NodeMoveErrorMessage")), nodeCode);
                    this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);
                    return;
                }

                //Successful move => Refresh CMS explorer.

                Melon.Components.CMS.CMS.IsSitemapStructureChanged = true;

                LoadWorkAreaArgs args = new LoadWorkAreaArgs();
                args.RefreshExplorer = true;
                args.NodeId = nodeId;
                this.ParentControl.OnLoadWorkAreaEvent(this, args);
            }
            else
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole = false;
                args.UserRole = currentUserRole;
                this.ParentControl.OnLoadAccessDeniedEvent(this, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Returns in the location of the a tree node.
    /// </summary>
    /// <remarks>
    ///     Estimates the location path of a tree node passed as parameter <paramref name="node"/>. 
    ///     The location is formed by property Text of all parent nodes and is returned in reference parameter <paramref name="location"/>.
    /// </remarks>
    /// <param name="node">TreeNode which location we need.</param>
    /// <param name="location">String variable which stores the location.</param>
    /// <author></author>
    /// <date>18/03/2008</date>
    private void GetNodeLocation(TreeNode node, ref string location)
    {
        if (node.Parent != null)
        {
            Match nodeCode = Regex.Match(node.Text, "(.*?)<span>(.*?)</span>");
            if (location == string.Empty)
            {
                location = nodeCode.Groups[2].Value;
            }
            else
            {

                location = nodeCode.Groups[2].Value + "/" + location;
            }

            if (node.Parent.Parent != null)
            {
                GetNodeLocation(node.Parent, ref location);
            }
        }
        else
        {
            location = "";
        }
    }

    /// <summary>
    /// Returns the path of the image icon for a specified node type.
    /// </summary>
    /// <remarks>
    ///     The icon depends on the node type and visivility in navigation.
    ///     <list type="table">
    ///         <listheader>
    ///             <item>
    ///                 <term>Node Type</term>
    ///                 <description>Icon</description>
    ///             </item>
    ///         </listheader>
    ///         <item>
    ///             <term>Folder</term>
    ///             <description>
    ///                 <list type="bullet">
    ///                     <item>CMS_Styles/Images/folder.gif</item>
    ///                     <item>CMS_Styles/Images/folder_hidden.gif (if hidden in navigation)</item>
    ///                 </list>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>ContentManageablePage</term>
    ///             <description>
    ///                 <list type="bullet">
    ///                     <item>CMS_Styles/Images/content_page.gif</item>
    ///                     <item>CMS_Styles/Images/content_page_hidden.gif (if hidden in navigation)</item>
    ///                 </list>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>StaticLocalPage</term>
    ///             <description>
    ///                 <list type="bullet">
    ///                     <item>CMS_Styles/Images/static_local_page.gif</item>
    ///                     <item>CMS_Styles/Images/static_local_page_hidden.gif (if hidden in navigation)</item>
    ///                 </list>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>StaticExternalPage</term>
    ///             <description>
    ///                 <list type="bullet">
    ///                     <item>CMS_Styles/Images/static_external_page.gif</item>
    ///                     <item>CMS_Styles/Images/static_external_page_hidden.gif (if hidden in navigation)</item>
    ///                 </list>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>StaticMenuPage</term>
    ///             <description>
    ///                 <list type="bullet">
    ///                     <item>CMS_Styles/Images/static_menu_page.gif</item>
    ///                     <item>CMS_Styles/Images/static_menu_page_hidden.gif (if hidden in navigation)</item>
    ///                 </list>
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <param name="nodeType">Type of node.</param>
    /// <param name="isHiddenInNavigation">Flag whether the node is hidden in navigation.</param>
    /// <returns>Icon path</returns>
    /// <seealso cref="Melon.Components.CMS.NodeType"/>
    /// <author></author>
    /// <date>10/03/2008</date>
    private string GetTreeNodeImageIcon(NodeType nodeType, bool isHiddenInNavigation)
    {
        switch (nodeType)
        {
            case NodeType.Folder:
                if (isHiddenInNavigation)
                {
                    return "CMS_Styles/Images/folder_hidden.gif";
                }
                else
                {
                    return "CMS_Styles/Images/folder.gif";
                }
            case NodeType.ContentManageablePage:
                if (isHiddenInNavigation)
                {
                    return "CMS_Styles/Images/content_page_hidden.gif";
                }
                else
                {
                    return "CMS_Styles/Images/content_page.gif";
                }
            case NodeType.StaticLocalPage:
                if (isHiddenInNavigation)
                {
                    return "CMS_Styles/Images/static_local_page_hidden.gif";
                }
                else
                {
                    return "CMS_Styles/Images/static_local_page.gif";
                }
            case NodeType.StaticExternalPage:
                if (isHiddenInNavigation)
                {
                    return "CMS_Styles/Images/static_external_page_hidden.gif";
                }
                else
                {
                    return "CMS_Styles/Images/static_external_page.gif";
                }
            case NodeType.StaticMenuPage:
                if (isHiddenInNavigation)
                {
                    return "CMS_Styles/Images/static_menu_page_hidden.gif";
                }
                else
                {
                    return "CMS_Styles/Images/static_menu_page.gif";
                }
            default:
                return "";
        }
    }

    /// <summary>
    /// Searches for tree node in TreeView tvCMSEplorer. 
    /// </summary>
    /// <remarks>
    ///     The method searches for TreeNode specified by node identifier in TreeView tvCMSEplorer.
    /// </remarks>
    /// <param name="nodeId">Identifier of node.</param>
    /// <returns>TreeNode object corresponding to the found node if any. Otherwise returns null.</returns>
    /// <seealso cref="CheckNodeMatch"/>
    /// <author></author>
    /// <date>19/03/2008</date>
    private TreeNode FindNode(int nodeId)
    {
        if (tvCMSExplorer.Nodes[0].ChildNodes.Count > 0) //Childs of node "Explorer"
        {
            TreeNode foundTreeNode = null;
            for (int i = 0; i < tvCMSExplorer.Nodes[0].ChildNodes.Count; i++)
            {
                foundTreeNode = CheckNodeMatch(tvCMSExplorer.Nodes[0].ChildNodes[i], nodeId);
                if (foundTreeNode != null)
                {
                    break;
                }
            }
            return foundTreeNode;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Check whether node or some of its child nodes has id specified in nodeId.
    /// </summary>
    /// <remarks>
    ///     The function is recursive. After checking the passed as parameter TreeNode <paramref name="ndoe"/> 
    ///     the verification continue for the child nodes if any.
    ///     This recursion continue until the node with id equal to parameter <paramref name="nodeId"/> is found 
    ///     or until all nodes are checked.
    /// </remarks>
    /// <param name="node">TreeNode to check.</param>
    /// <param name="nodeId">Identifier of node.</param>
    /// <returns>TreeNode object corresponding to the found matched node.</returns>
    /// <author></author>
    /// <date>19/03/2008</date>
    private TreeNode CheckNodeMatch(TreeNode node, int nodeId)
    {
        if (Convert.ToInt32(node.Value) == nodeId)
        {
            return node;
        }
        else
        {
            if (node.ChildNodes.Count > 0)
            {
                //Check children nodes.
                TreeNode foundTreeNode = null;
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    foundTreeNode = CheckNodeMatch(node.ChildNodes[i], nodeId);
                    if (foundTreeNode != null)
                    {
                        break;
                    }
                }
                return foundTreeNode;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Expands all parent nodes of specified tree node.
    /// </summary>
    /// <param name="node">Node which parents should be expands.</param>
    /// <author></author>
    /// <date>02/07/2008</date>
    private void ExpandNodeParents(TreeNode node)
    {
        if (node.Parent!= null)
        {
            node.Parent.Expand();
            int? nodeId = (node.Parent.Value == "") ? (int?)null : Convert.ToInt32(node.Parent.Value);
            if (!this.ParentControl.ExpandedNodes.Contains(nodeId))
            {
                this.ParentControl.ExpandedNodes.Add(nodeId);
            }
            ExpandNodeParents(node.Parent);
        }
    }

    /// <summary>
    /// Store in string <paramref name="strExpandedNodes"/> ids of nodes which are expanded in TreeView tvCMSExplorer.
    /// </summary>
    /// <param name="rootNode"></param>
    /// <param name="strExpandedNodes"></param>
    private void GetExpandedNodes(TreeNode rootNode, ref string strExpandedNodes)
    {
        if (rootNode.Expanded.HasValue && rootNode.Expanded == true)
        {
            //Node is expanded => Add it to string with ids.
            if (String.IsNullOrEmpty(strExpandedNodes))
            {
                strExpandedNodes = ",";
            }

            if (String.IsNullOrEmpty(rootNode.Value))
            {
                //"Explorer" node is expanded.
                strExpandedNodes += "null,";
            }
            else
            {
                //CMS node is expanded.
                strExpandedNodes += rootNode.Value + ",";
            }
        }

        //Check children.
        foreach (TreeNode node in rootNode.ChildNodes)
        {
            GetExpandedNodes(node, ref strExpandedNodes);
        }
    }

    /// <summary>
    /// Stores in <see cref="Melon.Components.CMS.ComponentEngine.BaseCMSControl.ExpandedNodes"/> ids of expanded nodes before submit.
    /// </summary>
    /// <author></author>
    /// <date>04/07/2008</date>
    private void StoreExpandedNodes()
    {
        List<int?> ids = new List<int?>();
        string strExpandedNodes = hfExpandedNodes.Value.Substring(1, hfExpandedNodes.Value.Length - 2);//Remove first and last comma;
        string[] arrNodeIds = strExpandedNodes.Split(',');
        foreach (string nodeId in arrNodeIds)
        {
            if (nodeId == "null")
            {
                ids.Add(null);
            }
            else
            {
                ids.Add(Convert.ToInt32(nodeId));
            }
        }
        this.ParentControl.ExpandedNodes = ids;
    }
}

