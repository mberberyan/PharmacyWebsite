using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    public partial class CodeBehind_FECategoryExplorer : ProductCatalogControl
    {        
        #region Fields && Properties
        /// <summary>
        /// Collection with ids of expanded nodes in Category tree.
        /// </summary>
        public List<int?> ExpandedNodes
        {
            get
            {
                if (ViewState["__mc_ProductCatalog_ExpandedNodes"] != null)
                {
                    return (List<int?>)ViewState["__mc_ProductCatalog_ExpandedNodes"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["__mc_ProductCatalog_ExpandedNodes"] = value;
            }
        }

        private static int? _SelectedObjectId;
        /// <summary>
        /// Id of current object in Explorer Panel and Central Panel
        /// </summary>
        public static int? SelectedObjectId
        {
            get { return _SelectedObjectId; }
            set { _SelectedObjectId = value; }
        }
        #endregion

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

            base.OnInit(e);
        }

        /// <summary>
        /// Event handler for event SelectedNodeChanged of TreeView tvCategoryExplorer.
        /// </summary>
        /// <remarks>
        /// Raises event LoadCategoryEvent in order to load selected object in user control
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>09/8/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvCategoryExplorer_SelectedNodeChanged(object sender, EventArgs e)
        {            
            int? selectedNodeId = (String.IsNullOrEmpty(tvCategoryExplorer.SelectedNode.Value)) ? (int?)null : Convert.ToInt32(tvCategoryExplorer.SelectedNode.Value);

            BaseMasterPageControl.FECategoryExplorerEventArgs ceEventArgs = new BaseMasterPageControl.FECategoryExplorerEventArgs();
            ceEventArgs.SelectedCategoryId = selectedNodeId;

            this.MasterPageControl.OnFECategoryExplorer(sender, ceEventArgs);
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
                e.Node.Text = "<span>" + this.GetLocalResourceObject("CategoryExplorer").ToString() + "</span>";
                e.Node.Value = "";
            }
            else
            {
                e.Node.Text = "<span>" + currentNode.Attributes["Name"].Value + "</span>";
                e.Node.Value = currentNode.Attributes["Id"].Value;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                }
                else
                {
                    //Select node "Explorer".
                    tvCategoryExplorer.Nodes[0].Select();
                }

                //Store in hidden field hfExpandedCategories ids of expanded nodes.
                if (ExpandedNodes == null)
                {
                    string expandedNodeIds = "";
                    GetExpandedNodes(tvCategoryExplorer.Nodes[0], ref expandedNodeIds);
                    hfExpandedCategories.Value = expandedNodeIds;
                }
                else
                {
                    hfExpandedCategories.Value = ",";
                    foreach (int? id in ExpandedNodes)
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
            ExpandedNodes = ids;
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
                if (!ExpandedNodes.Contains(nodeId))
                {
                    ExpandedNodes.Add(nodeId);
                }
                ExpandNodeParents(node.Parent);
            }
        }

        /// <summary>
        /// Loads cms nodes in TreeView tvCategoryExplorer.
        /// </summary>
        /// <remarks>
        ///     Calls method <see cref="Melon.Components.CMS.Node.ListCategoryExplorerNodes"/> to retrieve xml with all nodes
        ///     visible for the specified user group <paramref name="visibilityFilter"/> and databind TreeView tvCategoryExplorer with this xml.
        /// </remarks>
        /// <param name="visibilityFilter">User group for which to be visible the nodes in the tree.</param>
        /// <author>Melon Team</author>
        /// <date>09/4/2009</date>
        private void LoadCategoryExplorer()
        {
            XmlDataSource xml = new XmlDataSource();
            xml.EnableCaching = false;
            xml.Data = Category.ListCategoryExplorerNodes(null, true).DocumentElement.OuterXml;

            tvCategoryExplorer.DataSource = xml;
            tvCategoryExplorer.DataBind();
        }
    }
}
