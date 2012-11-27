using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Xml;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Enumerations;
using Melon.Components.ProductCatalog.Exception;
using System.Web.UI;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for displaying and managing tree of all existing objects from type <see cref="Melon.Components.ProductCatalog.ComponentObjectEnum"/> in the system.
    /// </summary>
    /// <remarks>    
    /// <para>
    ///     All existing objects are retrieved and are displayed in TreeView with root "[Object] Explorer" where [Object] is the name of object`s type.         
    /// </para>
    /// </remarks>
    public partial class CodeBehind_Explorer : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Enumeration of type <see cref="Melon.Components.ProductCatalog.ComponentObjectEnum"/> that holds current object`s type
        /// </summary>
        private ComponentObjectEnum currentObjectType
        {
            get
            {
                if (ViewState["mc_pc_objecttype"] == null)
                {
                    return ComponentObjectEnum.Unknown;                    
                }

                return (ComponentObjectEnum)ViewState["mc_pc_objecttype"];
            }
            set
            {
                ViewState["mc_pc_objecttype"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Initializes the control's properties
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized</param>
        public override void Initializer(object[] args)
        {
            SelectedObjectId = (int?)args[0];
            SelectedObjectType = (ComponentObjectEnum)args[1];
        }

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        protected override void OnInit(EventArgs e)
        {
            tvObjectExplorer.TreeNodeDataBound += new TreeNodeEventHandler(tvObjectExplorer_TreeNodeDataBound);
            tvObjectExplorer.SelectedNodeChanged += new EventHandler(tvObjectExplorer_SelectedNodeChanged);

            btnCreateObject.Click += new ImageClickEventHandler(btnCreateObject_Click);
            btnDeleteObject.Click += new ImageClickEventHandler(btnDeleteObject_Click);
            btnMoveUp.Click += new ImageClickEventHandler(btnMoveUp_Click);
            btnMoveDown.Click += new ImageClickEventHandler(btnMoveDown_Click);

            btnCreateObject.ToolTip = GetLocalResourceObject("CreateNew").ToString() + " " + SelectedObjectType.ToString().ToLower();
            btnDeleteObject.ToolTip = GetLocalResourceObject("DeleteCurrent").ToString() + " " + SelectedObjectType.ToString().ToLower();
            btnMoveUp.ToolTip = GetLocalResourceObject("MoveUp").ToString() + " " + SelectedObjectType.ToString().ToLower();
            btnMoveDown.ToolTip = GetLocalResourceObject("MoveDown").ToString() + " " + SelectedObjectType.ToString().ToLower();

            base.OnInit(e);
        }
        
        /// <summary>
        /// Event handler for event SelectedNodeChanged of TreeView tvObjectExplorer.
        /// </summary>
        /// <remarks>
        /// Raises event in order to load selected object in object`s user control
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvObjectExplorer_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                int? selectedNodeId = (String.IsNullOrEmpty(tvObjectExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvObjectExplorer.SelectedNode.Value);

                switch (SelectedObjectType)
                { 
                    case ComponentObjectEnum.Catalog:
                        LoadCatalogEventArgs args = new LoadCatalogEventArgs();
                        args.SelectedCatalogId = selectedNodeId;
                        args.RefreshExplorer = true;
                        args.SelectedObjectType = ComponentObjectEnum.Catalog;
                        args.SelectedTab = selectedNodeId != null ? ProductCatalogTabs.GeneralInformation : ProductCatalogTabs.Unknown;
                        this.ParentControl.OnLoadCatalog(sender, args);
                        break;
                    case ComponentObjectEnum.Bundle:
                        LoadBundleEventArgs argsBundle = new LoadBundleEventArgs();
                        argsBundle.SelectedBundleId = selectedNodeId;
                        argsBundle.RefreshExplorer = true;
                        argsBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                        argsBundle.SelectedTab = selectedNodeId != null ? ProductCatalogTabs.GeneralInformation : ProductCatalogTabs.Unknown;
                        this.ParentControl.OnLoadBundle(sender, argsBundle);
                        break;
                    case ComponentObjectEnum.Collection:
                        LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                        collArgs.SelectedCollectionId = selectedNodeId;
                        collArgs.RefreshExplorer = true;
                        collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                        collArgs.SelectedTab = selectedNodeId != null ? ProductCatalogTabs.GeneralInformation : ProductCatalogTabs.Unknown;
                        this.ParentControl.OnLoadCollection(sender, collArgs);
                        break;
                    case ComponentObjectEnum.Discount:
                        LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                        discountArgs.SelectedDiscountId = selectedNodeId;
                        discountArgs.RefreshExplorer = true;
                        discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                        discountArgs.SelectedTab = selectedNodeId != null ? ProductCatalogTabs.GeneralInformation : ProductCatalogTabs.Unknown;
                        this.ParentControl.OnLoadDiscount(sender, discountArgs);
                        break;
                }                
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event TreeNodeDataBound of TreeView tvObjectExplorer.
        /// </summary>
        /// <remarks>
        /// This method is used to set text and value properties for each node in tvObjectExplorer
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvObjectExplorer_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            XmlElement currentNode = (XmlElement)e.Node.DataItem;

            if (currentNode.Name == "root")
            {
                e.Node.Text = "<span>" + SelectedObjectType.ToString() + " " + GetLocalResourceObject("Explorer").ToString() + "</span>";
                e.Node.Value = "";
            }
            else
            {
                string IsActiveStr = Convert.ToBoolean(Convert.ToInt32(currentNode.Attributes["IsActive"].Value)) ? GetLocalResourceObject("Active").ToString() : GetLocalResourceObject("NotActive").ToString();
                string productCntStr = currentNode.Attributes["ProductCount"].Value + (Convert.ToInt32(currentNode.Attributes["ProductCount"].Value) == 1 ? (" "+GetLocalResourceObject("Product").ToString()+"; ") : (" "+GetLocalResourceObject("Products").ToString()+"; "));
                
                e.Node.Text = "<span><b>" + currentNode.Attributes["Name"].Value + "</b> (" + productCntStr + IsActiveStr + ")</span>";
                e.Node.Value = currentNode.Attributes["Id"].Value;
            }
        }

        /// <summary>
        /// Initialize the user control
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack || SelectedObjectType!=currentObjectType)
            {
                currentObjectType = SelectedObjectType;

                //1. Load the Object tree.
                //2. Select the node with id that is passed to the user control.
                //3. If there is no passed node id, select the root node: "Object Explorer".
                LoadObjectExplorer();

                if (SelectedObjectId != null && SelectedObjectId != 0)
                {
                    TreeNode selectedNode = FindNode(SelectedObjectId.Value);
                    if (selectedNode != null)
                    {
                        selectedNode.Select();                        
                    }
                    else
                    {
                        //Display error message if node with SelectedObjectId doesn't exist in the tree.                        
                        DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                        switch (SelectedObjectType)
                        { 
                            case ComponentObjectEnum.Catalog:
                                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedCatalogByAnotherUserErrorMessage"));
                                break;
                            case ComponentObjectEnum.Bundle:
                                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedBundleByAnotherUserErrorMessage"));
                                break;
                            case ComponentObjectEnum.Collection:
                                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedCollectionByAnotherUserErrorMessage"));
                                break;
                            case ComponentObjectEnum.Discount:
                                errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedBundleByAnotherUserErrorMessage"));
                                break;
                        }
                        
                        this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                        switch (SelectedObjectType)
                        { 
                            case ComponentObjectEnum.Catalog:
                                //Refresh Catalog explorer;
                                LoadCatalogEventArgs args = new LoadCatalogEventArgs();
                                args.RefreshExplorer = true;
                                args.SelectedObjectType = ComponentObjectEnum.Catalog;
                                this.ParentControl.OnLoadCatalog(sender, args);
                                break;
                            case ComponentObjectEnum.Bundle:
                                //Refresh Catalog explorer;
                                LoadBundleEventArgs argsBundle = new LoadBundleEventArgs();
                                argsBundle.RefreshExplorer = true;
                                argsBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                                this.ParentControl.OnLoadBundle(sender, argsBundle);
                                break;
                            case ComponentObjectEnum.Collection:
                                //Refresh Catalog explorer;
                                LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                                collArgs.RefreshExplorer = true;
                                collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                                this.ParentControl.OnLoadCollection(sender, collArgs);
                                break;
                            case ComponentObjectEnum.Discount:
                                //Refresh Discount explorer;
                                LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                                discountArgs.RefreshExplorer = true;
                                discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                                this.ParentControl.OnLoadDiscount(sender, discountArgs);
                                break;
                        }                        

                        return;
                    }
                }
                else
                {
                    //Select node "Explorer".
                    tvObjectExplorer.Nodes[0].Select();
                }                

                ManageActionButtons();
            }
            else
            {                
                //Event SelectedNodeChanged wasn't fired when we select already selected node.
                //So we force firing of the event.
                string eTarget = Request.Form["__EVENTTARGET"];
                string[] eArgument = Request.Form["__EVENTARGUMENT"].Replace("\\", "|").Split('|');
                //eArguments is array with the following format: 
                //First element [0] contains string which indicates what is the event. "s" is for selection of node.
                //The array has other n elements which is the level on which is the node which is selected.
                //The last element of the array is the selected node value (category explorer node id). 
                //The previous elements are the ids of the selected node parent nodes.

                //Check whether the event is raised by the treeview tvCategoryExplorer.
                if (eTarget == tvObjectExplorer.UniqueID)
                {
                    //Check whether this event is selection of node.
                    if (eArgument[0].ToLower() == "s")
                    {
                        if (eArgument.Length > 1)
                        {
                            if (tvObjectExplorer.SelectedValue != null && tvObjectExplorer.SelectedValue == eArgument[eArgument.Length - 1])
                            {
                                tvObjectExplorer.Nodes[0].Select();
                                tvObjectExplorer.SelectedNode.Select();
                            }
                        }
                        else
                        {
                            //"Explorer" is selected.
                            if (string.IsNullOrEmpty(tvObjectExplorer.SelectedValue))
                            {
                                if (tvObjectExplorer.Nodes[0].ChildNodes.Count > 0)
                                {
                                    tvObjectExplorer.Nodes[0].ChildNodes[0].Select();
                                    tvObjectExplorer.SelectedNode.Select();
                                }
                                else
                                {
                                    tvObjectExplorer.Nodes[0].ChildNodes.Add(new TreeNode());
                                    tvObjectExplorer.Nodes[0].ChildNodes[0].Select();
                                    tvObjectExplorer.SelectedNode.Select();
                                    tvObjectExplorer.Nodes[0].ChildNodes.Clear();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Searches for tree node in TreeView tvObjectExplorer. 
        /// </summary>
        /// <remarks>
        ///     The method searches for TreeNode specified by node identifier in TreeView tvObjectExplorer.
        /// </remarks>
        /// <param name="nodeId">Identifier of node.</param>
        /// <returns>TreeNode object corresponding to the found node if any. Otherwise returns null.</returns>
        /// <seealso cref="CheckNodeMatch"/>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        private TreeNode FindNode(int nodeId)
        {
            if (tvObjectExplorer.Nodes[0].ChildNodes.Count > 0) //Childs of node "Object Explorer"
            {
                TreeNode foundTreeNode = null;
                for (int i = 0; i < tvObjectExplorer.Nodes[0].ChildNodes.Count; i++)
                {
                    foundTreeNode = CheckNodeMatch(tvObjectExplorer.Nodes[0].ChildNodes[i], nodeId);
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
        ///     The function is recursive. After checking the passed as parameter TreeNode <paramref name="node"/> 
        ///     the verification continue for the child nodes if any.
        ///     This recursion continue until the node with id equal to parameter <paramref name="nodeId"/> is found 
        ///     or until all nodes are checked.
        /// </remarks>
        /// <param name="node">TreeNode to check.</param>
        /// <param name="nodeId">Identifier of node.</param>
        /// <returns>TreeNode object corresponding to the found matched node.</returns>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
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
        /// Loads object nodes in TreeView tvObjectExplorer.
        /// </summary>
        /// <remarks>
        ///     Calls method <see cref="Melon.Components.ProductCatalog.Catalog.ListObjectExplorerNodes"/> to retrieve xml with all nodes
        ///     visible for the specified user group <paramref name="visibilityFilter"/> and databind TreeView tvObjectExplorer with this xml.
        /// </remarks>        
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        private void LoadObjectExplorer()
        {
            XmlDataSource xml = new XmlDataSource();
            xml.EnableCaching = false;

            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Catalog:
                    xml.Data = Catalog.ListCatalogExplorerNodes().DocumentElement.OuterXml;
                    break;
                case ComponentObjectEnum.Bundle:
                    xml.Data = Bundle.ListBundleExplorerNodes().DocumentElement.OuterXml;
                    break;
                case ComponentObjectEnum.Collection:
                    xml.Data = Collection.ListCollectionExplorerNodes().DocumentElement.OuterXml;
                    break;
                case ComponentObjectEnum.Discount:
                    xml.Data = Discount.ListDiscountExplorerNodes().DocumentElement.OuterXml;
                    break;
            }
            
            tvObjectExplorer.DataSource = xml;            
            tvObjectExplorer.DataBind();
        }

        
        /// <summary>
        /// Move object in desired direction
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        /// <param name="direction"></param>
        private void MoveObject(object sender, ObjectMoveDirection direction)
        {
            TreeNode selectedNode = tvObjectExplorer.SelectedNode;
            int nodeId = Convert.ToInt32(selectedNode.Value);

            try
            {
                switch (SelectedObjectType)
                { 
                    case ComponentObjectEnum.Catalog:
                        Catalog.Move(nodeId, direction);
                        break;
                    case ComponentObjectEnum.Bundle:
                        Bundle.Move(nodeId, direction);
                        break;
                    case ComponentObjectEnum.Collection:
                        Collection.Move(nodeId, direction);
                        break;
                    case ComponentObjectEnum.Discount:
                        Discount.Move(nodeId, direction);
                        break;
                }
                
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();

                if (ex.Code == ProductCatalogExceptionCode.CatalogNotFoundException)
                {
                    switch (SelectedObjectType)
                    { 
                        case ComponentObjectEnum.Catalog:
                            errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedCatalogByAnotherUserErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadCatalogEventArgs args = new LoadCatalogEventArgs();
                            args.SelectedCatalogId = null;
                            args.RefreshExplorer = true;
                            args.SelectedObjectType = ComponentObjectEnum.Catalog;
                            this.ParentControl.OnLoadCatalog(sender, args);
                            break;
                        case ComponentObjectEnum.Bundle:
                            errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedBundleByAnotherUserErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadBundleEventArgs argsBundle = new LoadBundleEventArgs();
                            argsBundle.SelectedBundleId = null;
                            argsBundle.RefreshExplorer = true;
                            argsBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                            this.ParentControl.OnLoadBundle(sender, argsBundle);
                            break;
                        case ComponentObjectEnum.Collection:
                            errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedCollectionByAnotherUserErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                            collArgs.SelectedCollectionId = null;
                            collArgs.RefreshExplorer = true;
                            collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                            this.ParentControl.OnLoadCollection(sender, collArgs);
                            break;
                        case ComponentObjectEnum.Discount:
                            errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedDiscountByAnotherUserErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                            discountArgs.SelectedDiscountId = null;
                            discountArgs.RefreshExplorer = true;
                            discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                            this.ParentControl.OnLoadDiscount(sender, discountArgs);
                            break;
                    }                    

                    return;
                }
                else if (ex.Code == ProductCatalogExceptionCode.PreviousObjectNotExist
                       || ex.Code == ProductCatalogExceptionCode.NextObjectNotExist)
                {
                    switch (ex.Code)
                    {
                        case ProductCatalogExceptionCode.PreviousObjectNotExist:
                            errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("PreviousObjectNotExistErrorMessage")), SelectedObjectType.ToString());
                            break;
                        case ProductCatalogExceptionCode.NextObjectNotExist:
                            errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("NextObjectNotExistErrorMessage")), SelectedObjectType.ToString());
                            break;
                    }

                    switch (SelectedObjectType)
                    { 
                        case ComponentObjectEnum.Catalog:                                                        
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadCatalogEventArgs args = new LoadCatalogEventArgs();
                            args.SelectedCatalogId = nodeId;
                            args.RefreshExplorer = true;
                            args.SelectedObjectType = ComponentObjectEnum.Catalog;
                            this.ParentControl.OnLoadCatalog(sender, args);
                            break;
                        case ComponentObjectEnum.Bundle:
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadBundleEventArgs argsBundle = new LoadBundleEventArgs();
                            argsBundle.SelectedBundleId = nodeId;
                            argsBundle.RefreshExplorer = true;
                            argsBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                            this.ParentControl.OnLoadBundle(sender, argsBundle);
                            break;
                        case ComponentObjectEnum.Collection:                            
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                            collArgs.SelectedCollectionId = nodeId;
                            collArgs.RefreshExplorer = true;
                            collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                            this.ParentControl.OnLoadCollection(sender, collArgs);
                            break;
                        case ComponentObjectEnum.Discount:
                            this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                            LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                            discountArgs.SelectedDiscountId = nodeId;
                            discountArgs.RefreshExplorer = true;
                            discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                            this.ParentControl.OnLoadDiscount(sender, discountArgs);
                            break;
                    }                    

                    return;
                }
            }

            //Successful move => Refresh explorer.
            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Catalog:
                    LoadCatalogEventArgs e = new LoadCatalogEventArgs();
                    e.SelectedCatalogId = nodeId;
                    e.RefreshExplorer = true;
                    e.SelectedObjectType = ComponentObjectEnum.Catalog;
                    this.ParentControl.OnLoadCatalog(sender, e);
                    break;
                case ComponentObjectEnum.Bundle:
                    LoadBundleEventArgs args = new LoadBundleEventArgs();
                    args.SelectedBundleId = nodeId;
                    args.RefreshExplorer = true;
                    args.SelectedObjectType = ComponentObjectEnum.Bundle;
                    this.ParentControl.OnLoadBundle(sender, args);
                    break;
                case ComponentObjectEnum.Collection:
                    LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                    collArgs.SelectedCollectionId = nodeId;
                    collArgs.RefreshExplorer = true;
                    collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                    this.ParentControl.OnLoadCollection(sender, collArgs);
                    break;
                case ComponentObjectEnum.Discount:
                    LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                    discountArgs.SelectedDiscountId = nodeId;
                    discountArgs.RefreshExplorer = true;
                    discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                    this.ParentControl.OnLoadDiscount(sender, discountArgs);
                    break;
            }            
        }

        /// <summary>
        /// Sets accessibility of the action buttons for the Product Catalog tree: Move Up, Move Down, Create, Delete.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Node accessibility restrictions:
        ///     <list type="bullet">
        ///         <item>btnMoveUp - disabled if selected node is first child of its parent.</item>
        ///         <item>btnMoveDown - disabled if selected node is last child of its parent.</item>        
        ///         <item>btnDeleteCategory - disabled if not category item is selected.</item>        
        ///     </list>
        /// </para>
        /// </remarks>        
        private void ManageActionButtons()
        {
            TreeNode currentNode = tvObjectExplorer.SelectedNode;
            if (currentNode.Parent != null)
            {
                //Check whether the current object is first child of its parent.
                if (currentNode.Parent.ChildNodes.IndexOf(currentNode) == 0)
                {
                    //The current object is first child of its parent so it is not allowed to move it up.
                    btnMoveUp.Enabled = false;                    
                }
                else
                {
                    btnMoveUp.Enabled = true;                    
                }

                //Check whether the current object is last child of its parent.
                if (currentNode.Parent.ChildNodes.IndexOf(currentNode) == (currentNode.Parent.ChildNodes.Count - 1))
                {
                    //The current object is last child of its parent so it is not allowed to move it down.
                    btnMoveDown.Enabled = false;
                }
                else
                {
                    btnMoveDown.Enabled = true;
                }                                                
            }
            else
            {
                //"Object Explorer" is selected.
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                btnDeleteObject.Enabled = false;
            }

            btnCreateObject.ToolTip = String.Format(GetLocalResourceObject("CreateObject").ToString(), SelectedObjectType.ToString().ToLower());
            btnDeleteObject.ToolTip = String.Format(GetLocalResourceObject("DeleteObject").ToString(), SelectedObjectType.ToString().ToLower());
            btnMoveUp.ToolTip = String.Format(GetLocalResourceObject("MoveUpObject").ToString(), SelectedObjectType.ToString().ToLower());
            btnMoveDown.ToolTip = String.Format(GetLocalResourceObject("MoveDownObject").ToString(), SelectedObjectType.ToString().ToLower());

            btnCreateObject.ImageUrl = Utilities.GetImageUrl(this.Page, "create_cat" + (btnCreateObject.Enabled ? "" : "_disabled") + ".gif");            
            btnDeleteObject.ImageUrl = Utilities.GetImageUrl(this.Page, "delete_cat" + (btnDeleteObject.Enabled ? "" : "_disabled") + ".gif");
            btnMoveUp.ImageUrl = Utilities.GetImageUrl(this.Page, "move_up" + (btnMoveUp.Enabled ? "" : "_disabled") + ".gif");
            btnMoveDown.ImageUrl = Utilities.GetImageUrl(this.Page, "move_down" + (btnMoveDown.Enabled ? "" : "_disabled") + ".gif");            

            btnCreateObject.DataBind();
            btnDeleteObject.DataBind();
            btnMoveUp.DataBind();
            btnMoveDown.DataBind();
        }

        #region Buttons
        /// <summary>
        /// Event handler for event Click of ImageButton btnCreateObject.
        /// </summary>
        /// <remarks>
        ///     Raises event Load[Object]Event of the parent user control in order to load details for selected object, where object is object`s type name of type <see cref="Melon.Components.ProductCatalog.ComponentObjectEnum"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>                
        protected void btnCreateObject_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                int? selectedNodeId = (String.IsNullOrEmpty(tvObjectExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvObjectExplorer.SelectedNode.Value);

                switch (SelectedObjectType)
                { 
                    case ComponentObjectEnum.Catalog:
                        LoadCatalogEventArgs args = new LoadCatalogEventArgs();                        
                        args.RefreshExplorer = true;
                        args.SelectedCatalogId = 0;
                        args.SelectedObjectType = ComponentObjectEnum.Catalog;
                        args.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadCatalog(sender, args);
                        break;
                    case ComponentObjectEnum.Bundle:
                        LoadBundleEventArgs argsBundle = new LoadBundleEventArgs();
                        argsBundle.RefreshExplorer = true;
                        argsBundle.SelectedBundleId = 0;
                        argsBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                        argsBundle.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadBundle(sender, argsBundle);
                        break;
                    case ComponentObjectEnum.Collection:
                        LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                        collArgs.SelectedCollectionId = 0;
                        collArgs.RefreshExplorer = true;                        
                        collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                        collArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadCollection(sender, collArgs);
                        break;
                    case ComponentObjectEnum.Discount:
                        LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                        discountArgs.SelectedDiscountId = 0;
                        discountArgs.RefreshExplorer = true;
                        discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                        discountArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadDiscount(sender, discountArgs);
                        break;
                }                
            }
            else
            {

                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnDeleteObject.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Calls method to delete the currently selected object in TreeView tvObjectExplorer.         
        ///     <para>
        ///         If node is deleted successfully then event Load[Object]Event of the parent user control is raised 
        ///         in order to refresh the tree, where object is object`s type name of type <see cref="Melon.Components.ProductCatalog.ComponentObjectEnum"/>. 
        ///         If there is error during the deletion then event for displaying error message of the parent control is raised.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="Melon.Components.CMS.Node.Delete"/>
        /// <author></author>
        /// <date>12/03/2008</date>
        protected void btnDeleteObject_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                int? PreviousObjectId = null;
                try
                {
                    switch (SelectedObjectType)
                    { 
                        case ComponentObjectEnum.Catalog:
                            PreviousObjectId = Catalog.Delete(SelectedObjectId.Value);
                            break;
                        case ComponentObjectEnum.Bundle:
                            PreviousObjectId = Bundle.Delete(SelectedObjectId.Value);
                            break;
                        case ComponentObjectEnum.Collection:
                            PreviousObjectId = Collection.Delete(SelectedObjectId.Value);
                            break;
                        case ComponentObjectEnum.Discount:
                            PreviousObjectId = Discount.Delete(SelectedObjectId.Value);
                            break;
                    }
                    
                }
                catch (ProductCatalogException args)
                {
                    DisplayErrorMessageEventArgs errArgs = new DisplayErrorMessageEventArgs();
                    errArgs.ErrorMessage = args.Message;
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errArgs);

                    return;
                }

                //Refresh Object explorer;
                switch (SelectedObjectType)
                { 
                    case ComponentObjectEnum.Catalog:
                        LoadCatalogEventArgs catArgs = new LoadCatalogEventArgs();
                        catArgs.RefreshExplorer = true;
                        catArgs.SelectedCatalogId = PreviousObjectId;
                        catArgs.SelectedObjectType = ComponentObjectEnum.Catalog;
                        catArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadCatalog(sender, catArgs);
                        break;
                    case ComponentObjectEnum.Bundle:
                        LoadBundleEventArgs bundleArgs = new LoadBundleEventArgs();
                        bundleArgs.RefreshExplorer = true;
                        bundleArgs.SelectedBundleId = PreviousObjectId;
                        bundleArgs.SelectedObjectType = ComponentObjectEnum.Bundle;
                        bundleArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadBundle(sender, bundleArgs);
                        break;
                    case ComponentObjectEnum.Collection:
                        LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                        collArgs.RefreshExplorer = true;
                        collArgs.SelectedCollectionId = PreviousObjectId;
                        collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                        collArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadCollection(sender, collArgs);
                        break;
                    case ComponentObjectEnum.Discount:
                        LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                        discountArgs.RefreshExplorer = true;
                        discountArgs.SelectedDiscountId = PreviousObjectId;
                        discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                        discountArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadDiscount(sender, discountArgs);
                        break;
                }                
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnMoveUp.
        /// </summary>
        /// <remarks>
        ///     Calls method <see cref="MoveObject"/> with parameter <see cref="ObjectMoveDirection.Up"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="MoveObject"/>
        /// <seealso cref="Melon.Components.ProductCatalog.Enumeration.ObjectMoveDirection"/>        
        protected void btnMoveUp_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                MoveObject(sender, ObjectMoveDirection.Up);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnMoveDown.
        /// </summary>
        /// <remarks>
        ///     Calls method <see cref="MoveObject"/> with parameter <see cref="ObjectMoveDirection.Down"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="MoveObject"/>
        /// <seealso cref="Melon.Components.ProductCatalog.Enumeration.ObjectMoveDirection"/>    
        protected void btnMoveDown_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                MoveObject(sender, ObjectMoveDirection.Down);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }
        #endregion
    }
}
