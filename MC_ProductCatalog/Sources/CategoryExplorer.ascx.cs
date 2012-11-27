using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Web.UI.WebControls;
using System.Xml;
using Melon.Components.ProductCatalog.Enumerations;
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.Configuration;
using System.Web.UI;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for displaying and managing tree of all existing categories in the system.
    /// </summary>
    /// <remarks>    
    /// <para>
    ///     All existing categories are retrieved with method <see cref="Category.ListCategoryExplorerNodes(string)"/> of class <see cref="Category"/> 
    ///     and are displayed in TreeView with root "Category Explorer".         
    /// </para>
    /// </remarks>
    public partial class CodeBehind_CategoryExplorer : ProductCatalogControl
    {        
        
        /// <summary>
        /// Initializes the control's properties
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized</param>
        public override void Initializer(object[] args)
        {
            SelectedObjectId = (int?)args[0];
        }

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/03/2009</date>
        protected override void OnInit(EventArgs e)
        {
            tvCategoryExplorer.TreeNodeDataBound += new TreeNodeEventHandler(tvCategoryExplorer_TreeNodeDataBound);
            tvCategoryExplorer.SelectedNodeChanged += new EventHandler(tvCategoryExplorer_SelectedNodeChanged);
            
            btnCreateCategory.Click += new ImageClickEventHandler(btnCreateCategory_Click);
            btnCreateProduct.Click += new ImageClickEventHandler(btnCreateProduct_Click);
            btnListProducts.Click += new ImageClickEventHandler(btnListProducts_Click);
            btnDeleteCategory.Click += new ImageClickEventHandler(btnDeleteCategory_Click);
            btnMoveUp.Click += new ImageClickEventHandler(btnMoveUp_Click);
            btnMoveDown.Click += new ImageClickEventHandler(btnMoveDown_Click);
            btnMoveLeft.Click += new ImageClickEventHandler(btnMoveLeft_Click);
            btnMoveRight.Click += new ImageClickEventHandler(btnMoveRight_Click);

            base.OnInit(e);
        }        

        /// <summary>
        /// Event handler for event SelectedNodeChanged of TreeView tvCategoryExplorer.
        /// </summary>
        /// <remarks>
        /// Raises event LoadCategoryEvent in order to load selected category in Category.ascx user control
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>09/8/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvCategoryExplorer_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                int? selectedNodeId = (String.IsNullOrEmpty(tvCategoryExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCategoryExplorer.SelectedNode.Value);

                LoadCategoryEventArgs args = new LoadCategoryEventArgs();                
                args.SelectedCategoryId = selectedNodeId;
                args.RefreshExplorer = true;
                args.SelectedObjectType = selectedNodeId != null ? ComponentObjectEnum.Category : ComponentObjectEnum.Unknown;
                args.SelectedTab = selectedNodeId != null ? ProductCatalogTabs.GeneralInformation : ProductCatalogTabs.Unknown;
                this.ParentControl.OnLoadCategory(sender, args);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event TreeNodeDataBound of TreeView tvCategoryExplorer.
        /// </summary>
        /// <remarks>
        /// This method is used to set text and value properties for each node in tvCategoryExplorer
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>09/8/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvCategoryExplorer_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            XmlElement currentNode = (XmlElement)e.Node.DataItem;

            if (currentNode.Name == "root")
            {
                e.Node.Text = "<span>" + GetLocalResourceObject("CategoryExplorer").ToString() + "</span>";
                e.Node.Value = "" ;
            }
            else
            {
                e.Node.Text = "<span>" + currentNode.Attributes["Name"].Value+"</span>";
                e.Node.Value = currentNode.Attributes["Id"].Value;
            }

            if (this.ParentControl.ExpandedNodes != null)
            {
                if (currentNode.Name == "root")
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
                else
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
        /// Initialize the user control
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/8/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                //1. Load the Category tree.
                //2. Select the node with id that is passed to the user control.
                //3. If there is no passed node id, select the root node: "Cateogry Explorer".
                LoadCategoryExplorer();                

                if (SelectedObjectId != null)
                {
                    TreeNode selectedNode = FindNode(SelectedObjectId.Value);
                    if (selectedNode != null)
                    {
                        selectedNode.Select();
                        ExpandNodeParents(selectedNode);
                    }
                    else
                    {
                        //Display error message if node with SelectedCategoryId doesn't exist in the tree.                        
                        DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                        errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedCategoryByAnotherUserErrorMessage"));
                        this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                        //Refresh Category explorer;
                        LoadCategoryEventArgs args = new LoadCategoryEventArgs();
                        args.RefreshExplorer = true;
                        args.SelectedObjectType = ComponentObjectEnum.Unknown;
                        this.ParentControl.OnLoadCategory(sender, args);

                        return;
                    }
                }
                else
                {
                    //Select node "Explorer".
                    tvCategoryExplorer.Nodes[0].Select();
                }

                //Store in hidden field hfExpandedCategories ids of expanded nodes.
                if (this.ParentControl.ExpandedNodes == null)
                {
                    string expandedNodeIds = "";
                    GetExpandedNodes(tvCategoryExplorer.Nodes[0], ref expandedNodeIds);
                    hfExpandedCategories.Value = expandedNodeIds;
                }
                else
                {
                    hfExpandedCategories.Value = ",";
                    foreach (int? id in this.ParentControl.ExpandedNodes)
                    {
                        if (id.HasValue)
                        {
                            hfExpandedCategories.Value += id.Value + ",";
                        }
                        else
                        {
                            hfExpandedCategories.Value += "null,";
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
                //The last element of the array is the selected node value (category explorer node id). 
                //The previous elements are the ids of the selected node parent nodes.

                //Check whether the event is raised by the treeview tvCategoryExplorer.
                if (eTarget == tvCategoryExplorer.UniqueID)
                {
                    //Check whether this event is selection of node.
                    if (eArgument[0].ToLower() == "s")
                    {
                        if (eArgument.Length > 1)
                        {
                            if (tvCategoryExplorer.SelectedValue != null && tvCategoryExplorer.SelectedValue == eArgument[eArgument.Length - 1])
                            {
                                tvCategoryExplorer.Nodes[0].Select();
                                tvCategoryExplorer.SelectedNode.Select();
                            }
                        }
                        else
                        {
                            //"Explorer" is selected.
                            if (string.IsNullOrEmpty(tvCategoryExplorer.SelectedValue))
                            {
                                if (tvCategoryExplorer.Nodes[0].ChildNodes.Count > 0)
                                {
                                    tvCategoryExplorer.Nodes[0].ChildNodes[0].Select();
                                    tvCategoryExplorer.SelectedNode.Select();
                                }
                                else
                                {
                                    tvCategoryExplorer.Nodes[0].ChildNodes.Add(new TreeNode());
                                    tvCategoryExplorer.Nodes[0].ChildNodes[0].Select();
                                    tvCategoryExplorer.SelectedNode.Select();
                                    tvCategoryExplorer.Nodes[0].ChildNodes.Clear();
                                }
                            }
                        }
                    }
                }
            }            
        }                

        /// <summary>
        /// Stores in <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.ExpandedNodes"/> ids of expanded nodes before submit.
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/4/2009</date>
        private void StoreExpandedNodes()
        {
            List<int?> ids = new List<int?>();
            string strExpandedNodes = hfExpandedCategories.Value.Substring(1, hfExpandedCategories.Value.Length - 2);//Remove first and last comma;
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

        /// <summary>
        /// Store in string <paramref name="strExpandedNodes"/> ids of nodes which are expanded in TreeView tvCategoryExplorer.
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="strExpandedNodes"></param>
        /// <author>Melon Team</author>
        /// <date>09/4/2009</date>
        private void GetExpandedNodes(TreeNode rootNode, ref string strExpandedNodes)
        {
            if (rootNode.Expanded.HasValue && rootNode.Expanded == true)
            {
                //Node is expanded, therefore it needs to be added to the string with ids.
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
        /// Searches for tree node in TreeView tvCategoryExplorer. 
        /// </summary>
        /// <remarks>
        ///     The method searches for TreeNode specified by node identifier in TreeView tvCategoryExplorer.
        /// </remarks>
        /// <param name="nodeId">Identifier of node.</param>
        /// <returns>TreeNode object corresponding to the found node if any. Otherwise returns null.</returns>
        /// <seealso cref="CheckNodeMatch"/>
        /// <author>Melon Team</author>
        /// <date>09/4/2009</date>
        private TreeNode FindNode(int nodeId)
        {
            if (tvCategoryExplorer.Nodes[0].ChildNodes.Count > 0) //Childs of node "Category Explorer"
            {
                TreeNode foundTreeNode = null;
                for (int i = 0; i < tvCategoryExplorer.Nodes[0].ChildNodes.Count; i++)
                {
                    foundTreeNode = CheckNodeMatch(tvCategoryExplorer.Nodes[0].ChildNodes[i], nodeId);
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
        /// <date>09/4/2009</date>
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
        /// <author>Melon Team</author>
        /// <date>09/4/2009</date>
        private void ExpandNodeParents(TreeNode node)
        {
            if (node.Parent != null)
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
        /// Loads categories in TreeView tvCategoryExplorer.
        /// </summary>
        /// <remarks>
        ///     Calls method <see cref="Melon.Components.ProductCatalog.Node.ListCategoryExplorerNodes"/> to retrieve xml with all nodes
        ///     visible for the specified user group <paramref name="visibilityFilter"/> and databind TreeView tvCategoryExplorer with this xml.
        /// </remarks>
        /// <param name="visibilityFilter">User group for which to be visible the nodes in the tree.</param>
        /// <author>Melon Team</author>
        /// <date>09/4/2009</date>
        private void LoadCategoryExplorer()
        {
            XmlDataSource xml = new XmlDataSource();
            xml.EnableCaching = false;
            xml.Data = Category.ListCategoryExplorerNodes(null,null).DocumentElement.OuterXml;

            tvCategoryExplorer.DataSource = xml;
            tvCategoryExplorer.DataBind();            
        }        

        /// <summary>
        /// Move category in desired direction
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/9/2009</date>
        /// <param name="direction"></param>
        private void MoveCategory(object sender, CategoryMoveDirection direction)
        { 
            TreeNode selectedNode = tvCategoryExplorer.SelectedNode;
            int nodeId = Convert.ToInt32(selectedNode.Value);            

            try
            {
                Category.Move(nodeId, direction);
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();

                if (ex.Code == ProductCatalogExceptionCode.CategoryNotFoundException)
                {
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("DeletedCategoryByAnotherUserErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);

                    LoadCategoryEventArgs args = new LoadCategoryEventArgs();
                    args.SelectedCategoryId = null;
                    args.RefreshExplorer = true;                    
                    this.ParentControl.OnLoadCategory(sender, args);

                    return;
                }
                else if (ex.Code == ProductCatalogExceptionCode.PreviousCategoryNotExist
                       ||ex.Code == ProductCatalogExceptionCode.NextCategoryNotExist
                       || ex.Code == ProductCatalogExceptionCode.ParentCategoryNotExist)
                {
                    LoadCategoryEventArgs args = new LoadCategoryEventArgs();
                    args.SelectedCategoryId = nodeId;
                    args.RefreshExplorer = true;
                    this.ParentControl.OnLoadCategory(sender, args);

                    return;
                }
            }

            //Successful move => Refresh Category explorer.
            LoadCategoryEventArgs e = new LoadCategoryEventArgs();
            e.SelectedCategoryId = nodeId;
            e.RefreshExplorer = true;
            this.ParentControl.OnLoadCategory(sender, e);
        }

        /// <summary>
        /// Sets accessibility of the action buttons for the Product Catalog tree: Move Up, Move Down, Move Left, Move Rigth, Create, Delete, List Products.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     The accessibility of the buttons depends on the currently selected node.</para>
        /// <para>
        ///     Node accessibility restrictions:
        ///     <list type="bullet">
        ///         <item>btnMoveUp - disabled if selected node is first child of its parent.</item>
        ///         <item>btnMoveDown - disabled if selected node is last child of its parent.</item>
        ///         <item>btnMoveLeft - disabled if selected node has no grand-parent.</item>
        ///         <item>btnMoveRight - disabled if selected node is first child of its parent.</item>        
        ///         <item>btnDeleteCategory - disabled if not category item is selected.</item>
        ///         <item>btnListProducts - disabled if not category item is selected.</item>
        ///     </list>
        /// </para>
        /// </remarks>        
        private void ManageActionButtons()
        {
            TreeNode currentNode = tvCategoryExplorer.SelectedNode;
            if (currentNode.Parent != null)
            {                
                //Check whether the current category is first child of its parent.
                if (currentNode.Parent.ChildNodes.IndexOf(currentNode) == 0)
                {
                    //The current category is first child of its parent so it is not allowed to move it up or right.
                    btnMoveUp.Enabled = false;                    
                    btnMoveRight.Enabled = false;                    
                }
                else
                {
                    btnMoveUp.Enabled = true;                    
                    btnMoveRight.Enabled = true;                    
                }

                //Check whether the current category is last child of its parent.
                if (currentNode.Parent.ChildNodes.IndexOf(currentNode) == (currentNode.Parent.ChildNodes.Count - 1))
                {
                    //The current category is last child of its parent so it is not allowed to move it down.
                    btnMoveDown.Enabled = false;                    
                }
                else
                {
                    btnMoveDown.Enabled = true;                    
                }

                //Check whether the current category has grand-parent.
                if (currentNode.Parent.Parent == null)
                {
                    btnMoveLeft.Enabled = false;                    
                }
                else
                {
                    btnMoveLeft.Enabled = true;                    
                }

                //Check whether the current category has childs.
                if (currentNode.ChildNodes.Count > 0)
                {
                    //The current category has childs so deletion is not allowed.
                    btnDeleteCategory.Enabled = false;                    
                }
                else
                {
                    btnDeleteCategory.Enabled = true;                    
                }
            }
            else
            {
                //"Category Explorer" is selected.
                btnMoveUp.Enabled = false;                
                btnMoveDown.Enabled = false;                
                btnMoveLeft.Enabled = false;                
                btnMoveRight.Enabled = false;
                btnListProducts.Enabled = false;
                btnDeleteCategory.Enabled = false;                
            }

            btnCreateCategory.ImageUrl = Utilities.GetImageUrl(this.Page, "create_cat" + (btnCreateCategory.Enabled ? "" : "_disabled") + ".gif");
            btnCreateProduct.ImageUrl = Utilities.GetImageUrl(this.Page, "create_product" + (btnCreateProduct.Enabled ? "" : "_disabled") + ".gif");
            btnListProducts.ImageUrl = Utilities.GetImageUrl(this.Page, "list_products" + (btnListProducts.Enabled ? "" : "_disabled") + ".gif");
            btnDeleteCategory.ImageUrl = Utilities.GetImageUrl(this.Page, "delete_cat" + (btnDeleteCategory.Enabled ? "" : "_disabled") + ".gif");
            btnMoveUp.ImageUrl = Utilities.GetImageUrl(this.Page, "move_up" + (btnMoveUp.Enabled ? "" : "_disabled") + ".gif");
            btnMoveDown.ImageUrl = Utilities.GetImageUrl(this.Page, "move_down" + (btnMoveDown.Enabled ? "" : "_disabled") + ".gif");
            btnMoveLeft.ImageUrl = Utilities.GetImageUrl(this.Page, "move_left" + (btnMoveLeft.Enabled ? "" : "_disabled") + ".gif");
            btnMoveRight.ImageUrl = Utilities.GetImageUrl(this.Page, "move_right" + (btnMoveRight.Enabled ? "" : "_disabled") + ".gif");

            btnCreateCategory.DataBind();
            btnCreateProduct.DataBind();
            btnListProducts.DataBind();
            btnDeleteCategory.DataBind();
            btnMoveUp.DataBind();
            btnMoveDown.DataBind();
            btnMoveLeft.DataBind();
            btnMoveRight.DataBind();
        }

        #region Buttons
        /// <summary>
        /// Event handler for event Click of ImageButton btnCreateCategory.
        /// </summary>
        /// <remarks>
        ///     Raises event LoadCategoryEvent of the parent user control in order to load details for selected category.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>                
        protected void btnCreateCategory_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {                
                int? selectedNodeId = (String.IsNullOrEmpty(tvCategoryExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCategoryExplorer.SelectedNode.Value);

                if (selectedNodeId!=null && FindNode(selectedNodeId.Value).Depth >= ProductCatalogSettings.CategoryLevel)
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("CategoryLevelExceedErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(this, errorArgs);
                    return;
                }

                LoadCategoryEventArgs args = new LoadCategoryEventArgs();
                args.ParentCategoryId = selectedNodeId;                
                args.RefreshExplorer = true;
                args.SelectedObjectType = ComponentObjectEnum.Category;
                args.SelectedTab = ProductCatalogTabs.GeneralInformation;
                this.ParentControl.OnLoadCategory(sender, args);
            }
            else
            {

                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnCreateProduct.
        /// </summary>
        /// <remarks>
        ///     Raises event LoadProductEvent of the parent user control in order to initialize information for new product item.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        protected void btnCreateProduct_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                int? selectedCategoryId = (String.IsNullOrEmpty(tvCategoryExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCategoryExplorer.SelectedNode.Value);

                LoadProductEventArgs args = new LoadProductEventArgs();
                args.SelectedCategoryId = selectedCategoryId;
                args.RefreshExplorer = false;
                args.SelectedObjectType = ComponentObjectEnum.Product;
                args.SelectedTab = ProductCatalogTabs.GeneralInformation;
                this.ParentControl.OnLoadProduct(sender, args);
            }
            else
            {

                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnListProduct.
        /// </summary>
        /// <remarks>
        ///     Raises event LoadProductListEvent of the parent user control in order to load product listing control.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnListProducts_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                int? selectedCategoryId = (String.IsNullOrEmpty(tvCategoryExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCategoryExplorer.SelectedNode.Value);

                if (selectedCategoryId == null)
                {
                    DisplayErrorMessageEventArgs errArgs = new DisplayErrorMessageEventArgs();
                    errArgs.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotSelectedCategoryErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errArgs);

                    return;
                }

                LoadProductListEventArgs args = new LoadProductListEventArgs();
                args.SelectedCategoryId = selectedCategoryId;
                args.SelectedObjectType = ComponentObjectEnum.Category;
                args.SelectedTab = SelectedTab;
                args.RefreshExplorer = false;
                this.ParentControl.OnLoadProductList(sender, args);
            }
            else
            {

                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnDeleteCategory.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Calls method <see cref="Melon.Components.ProductCatalog.Category.Delete"/> to delete the currently selected category
        ///         in TreeView tvCategoryExplorer. Category couldn't be deleted if it is referred i.e. it is parent category.
        ///         When such category is tried to be deleted error message is displayed in Label lblErrorMessage.</para>
        ///     <para>
        ///         If node is deleted successfully then event LoadCategoryEvent of the parent user control is raised 
        ///         in order to refresh the tree. 
        ///         If there is error during the deletion then event for displaying error message of the parent control is raised.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="Melon.Components.CMS.Node.Delete"/>
        /// <author></author>
        /// <date>12/03/2008</date>
        protected void btnDeleteCategory_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                int? PreviousCategoryId = null;
                try
                {
                    PreviousCategoryId = Category.Delete(SelectedObjectId.Value);
                }
                catch (ProductCatalogException args)
                {
                    DisplayErrorMessageEventArgs errArgs = new DisplayErrorMessageEventArgs();
                    errArgs.ErrorMessage = args.Message;
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errArgs);

                    return;
                }                

                //Refresh Category explorer;
                LoadCategoryEventArgs catArgs = new LoadCategoryEventArgs();
                catArgs.RefreshExplorer = true;
                catArgs.SelectedCategoryId = PreviousCategoryId;
                catArgs.SelectedObjectType = ComponentObjectEnum.Category;
                catArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                this.ParentControl.OnLoadCategory(sender, catArgs);
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
        ///     Calls method <see cref="MoveCategory"/> with parameter <see cref="CategoryMoveDirection.Up"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="MoveCategory"/>
        /// <seealso cref="Melon.Components.ProductCatalog.Enumeration.CategoryMoveDirection"/>        
        protected void btnMoveUp_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                MoveCategory(sender, CategoryMoveDirection.Up);
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
        ///     Calls method <see cref="MoveCategory"/> with parameter <see cref="CategoryMoveDirection.Down"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="MoveCategory"/>
        /// <seealso cref="Melon.Components.ProductCatalog.Enumeration.CategoryMoveDirection"/>
        protected void btnMoveDown_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                MoveCategory(sender, CategoryMoveDirection.Down);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnMoveLeft.
        /// </summary>
        /// <remarks>
        ///     Calls method <see cref="MoveCategory"/> with parameter <see cref="CategoryMoveDirection.Left"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="MoveCategory"/>
        /// <seealso cref="Melon.Components.ProductCatalog.Enumeration.CategoryMoveDirection"/>
        protected void btnMoveLeft_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                MoveCategory(sender, CategoryMoveDirection.Left);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of ImageButton btnMoveRight.
        /// </summary>
        /// <remarks>
        ///     Calls method <see cref="MoveCategory"/> with parameter <see cref="CategoryMoveDirection.Right"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="MoveCategory"/>
        /// <seealso cref="Melon.Components.ProductCatalog.Enumeration.CategoryMoveDirection"/>
        protected void btnMoveRight_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                MoveCategory(sender, CategoryMoveDirection.Right);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }
        #endregion

    }
}
