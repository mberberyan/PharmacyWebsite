using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text.RegularExpressions;
using Melon.Components.Forum.Configuration;
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.SearchEngine;

namespace Melon.Components.Forum.UI.CodeBehind
{
    ///<summary>
    ///	Provides user interface for setting criteria for searching in the forum posts.
    ///</summary>
    ///<remarks>
    ///	<para>
    ///		The ForumSearchCriteria user control provides table with search criteria controls where
    ///		will be entered the criteria by which will be performed search in the forum posts.
    ///		The possible search criteria are: words or phrases in the posts, date when posts were made,
    ///		author of posts. Also could be specified forums where to search.
    ///		The control uses <see cref="Melon.Components.Forum.SearchEngine.ForumSearchEngine"/> object
    ///		to store the search criteria. When search button is clicked the event
    ///		<see cref="Melon.Components.Forum.ComponentEngine.BaseForumControl.LoadForumSearchResultsEvent"/>
    ///		of the main user controls is raised with event argument the ForumSearchEngine object. The <see cref="Forum"/> user control (main user control) handles
    ///		this event and load the results in control <see cref="ForumSearchResults"/>.
    ///	</para>
    ///	<para>
    ///		The following web controls build this web page:
    ///		<list type="table">
    ///			<listheader>
    ///				<term>Web Control</term>
    ///				<description>Description</description>
    ///			</listheader>
    ///			<item>
    ///				<term>TextBox txtKeywords</term>
    ///				<description>Words or phrases for which to search in the text of the posts. Phrases are in double quotes.</description>
    ///			</item>
    ///			<item>
    ///				<term>TextBox txtAuthor</term>
    ///				<description>Nickname of posts' author</description>
    ///			</item>
    ///				<item>
    ///					<term>DropDownList ddlDateCriteria</term>
    ///					<description>List of date  criteria: On, Between</description>
    ///				</item>
    ///				<item>
    ///					<term>TextBox txtDateFrom</term>
    ///					<description>
    ///						Posts created on this date will be searched.
    ///						In case time period was selected as date criteria then posts created on this date or latter will be searched.
    ///				</description>
    ///			</item>
    ///				<item>
    ///					<term>TextBox txtDateTo</term>
    ///					<description>
    ///						This control is visible if date criteria  Between is selected.
    ///						Posts with date before the specified in this control date will be searched.
    ///					</description>
    ///				</item>
    ///				<item>
    ///					<term>TreeView tvForums</term>
    ///					<description>Tree with all forum groups and forums visible for the current logged user.</description>
    ///				</item>
    ///				<item>
    ///					<term>DropDownList ddlSortField</term>
    ///					<description>List with properties by which could be sorted the search results.</description>
    ///				</item>
    ///				<item>
    ///					<term>Button btnSearch</term>
    ///					<description>Raises event LoadForumSearchResultsEventArgs of main user control.</description>
    ///				</item>
    ///				<item>
    ///					<term>Button btnClear</term>
    ///					<description>Reset the search criteria.</description>
    ///				</item>
    ///
    ///			
    ///	</list>
    ///</para>
    ///<para>
    ///	All web controls from ForumSearchCriteria are using the local resources.
    ///	To customize them modify resource file ForumSearchCriteria.resx placed in the MC_Forum folder.
    ///</para>
    ///</remarks>
    ///<seealso cref="Melon.Components.Forum.UI.CodeBehind.ForumSearchResults"/>
    ///<seealso cref="Melon.Components.Forum.SearchEngine.ForumSearchEngine"/> 
    public partial class ForumSearchCriteria : ForumControl
    {
        /// <summary>
        /// Loads forums tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterInitScript();

			Type cstype = this.GetType();
			
            cvDateFromFormat.DataBind();
			this.tvForums.EnableClientScript = false;
            if (!IsControlPostBack)
            {
                LoadForumsTree();

                ibtnOpenCalendarFrom.DataBind();
                ibtnOpenCalendarTo.DataBind();
                cvDateFrom.DataBind();
                cvDateTo.DataBind();
                cvDateToFormat.DataBind();

                if (ForumSettings.EnableFullTextSearch)
                {
                    ScriptManager.RegisterClientScriptInclude(this, this.Page.GetType(), "Tooltip", ResolveUrl("./Scripts/Tooltip/wz_tooltip.js"));
                    ScriptManager.RegisterClientScriptInclude(this, this.Page.GetType(), "Baloon", ResolveUrl("./Scripts/Tooltip/tip_balloon.js"));

                    this.txtKeywords.Attributes.Add("onmouseover", "Tip('" + GetLocalResourceObject("txtKeywords.ToopTip").ToString() + "',BALLOON, true, BALLOONIMGPATH,'" + ResolveUrl("./Scripts/Tooltip/tip_balloon/") + "')");
                }
				
				this.ddlDateCriteria.Attributes.Add("onchange", "DisplayDateCriteria('" + ddlDateCriteria.ClientID + "')");
                //this.btnSearch.Attributes.Add("onclick", "return CheckIsSearchLocationSpecified('" + tvForums.ClientID + "','" + lblErrorMessage.ClientID + "')");
                this.btnSearch.Attributes.Add("onclick", "if(CheckIsSearchLocationSpecified('" + tvForums.ClientID + "','" + lblErrorMessage.ClientID + "')){" + this.btnSearch.Attributes["onclick"] + "}");
            }

            this.btnClear.Attributes.Add("onclick", @"ClearControls('" + this.txtKeywords.ClientID + @"',
																    '" + this.txtAuthor.ClientID + @"',
																    '" + this.tvForums.ClientID + @"',
																    '" + this.ddlDateCriteria.ClientID + @"',
																    '" + this.txtDateFrom.ClientID + @"',
																    '" + this.txtDateTo.ClientID + @"',
																    '" + this.ddlSortField.ClientID + @"',
																    '" + this.cvDateFrom.ClientID + @"',
																    '" + this.cvDateFromFormat.ClientID + @"',
																    '" + this.cvDateTo.ClientID + @"',
																    '" + this.cvDateToFormat.ClientID + @"');return false;");
        }

        /// <summary>
        /// Raises event LoadForumSearchResultsEvent of parent control. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //Collect search criteria and prepare event arguments
                ForumSearchEngine objForumSearchEngine = new ForumSearchEngine();

                if (txtKeywords.Text.Trim() != String.Empty)
                {
                    if (ForumSettings.EnableFullTextSearch)
                    {
                        objForumSearchEngine.KeywordsPredicateCondition = GetSearchPredicateCondition();
                    }
                    else
                    {
                        objForumSearchEngine.KeywordsPredicateCondition = txtKeywords.Text.Trim(); 
                    }
                }
                objForumSearchEngine.ForumIds = GetSelectedForumIds();
                if (txtAuthor.Text.Trim() != string.Empty)
                {
                    objForumSearchEngine.Author = txtAuthor.Text.Trim();
                }
                objForumSearchEngine.DateCriteriaType = (SearchDateCriteriaType)Convert.ToInt32(ddlDateCriteria.SelectedValue);
                switch (objForumSearchEngine.DateCriteriaType)
                {
                    case SearchDateCriteriaType.On:
                        if (txtDateFrom.Text.Trim() != string.Empty)
                        {
                            objForumSearchEngine.Date = Convert.ToDateTime(txtDateFrom.Text.Trim());
                        }
                        break;
                    case SearchDateCriteriaType.Between:
                        if (txtDateFrom.Text.Trim() != string.Empty)
                        {
                            objForumSearchEngine.DateFrom = Convert.ToDateTime(txtDateFrom.Text.Trim());
                        }
                        if (txtDateTo.Text.Trim() != string.Empty)
                        {
                            objForumSearchEngine.DateTo = Convert.ToDateTime(txtDateTo.Text.Trim());
                        }
                        break;
                    default: break;
                }

                objForumSearchEngine.SortField = (SearchResultsSortField)Convert.ToInt32(ddlSortField.SelectedValue);

                //Raise event
                ParentControl.OnLoadForumSearchResultsEvent(sender, new LoadForumSearchResultsEventArgs(objForumSearchEngine));
            }

        }

        /// <summary>
        /// Validate dates in search. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cvDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime enteredDate = Convert.ToDateTime(args.Value);
                if ((enteredDate >= ForumSettings.MinimumSearchDateValue))
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            catch
            {
                args.IsValid = true;
            }

        }

        /// <summary>
        /// Validate date formats in search.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cvDate_ServerValidateDateFormat(object source, ServerValidateEventArgs args)
        {
            if (!IsDate(args.Value))
            {
                if (((source as CustomValidator).ID == cvDateFromFormat.ID && ddlDateCriteria.SelectedIndex > 0) ||
                    ((source as CustomValidator).ID == cvDateToFormat.ID && ddlDateCriteria.SelectedIndex == 2)
                    )
                    args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }

        }


        /// <summary>
        /// Display in TreeView tvForums all forum groups and forums.
        /// </summary>
        private void LoadForumsTree()
        {
            XmlDocument xml = ForumInfo.LoadForumsTree(ParentControl.CurrentUser);
            XmlDataSource objXmlDataSource = new XmlDataSource();
            objXmlDataSource.EnableCaching = false;
			
			objXmlDataSource.Data = xml.InnerXml;
			if (xml.FirstChild.ChildNodes.Count == 0)
			{
				tvForums.ShowCheckBoxes = TreeNodeTypes.None;
				tvForums.DataBindings[0].Text = Convert.ToString(GetLocalResourceObject("NoAvailableForums"));
			}

			tvForums.DataSource = objXmlDataSource;
			tvForums.DataBind();
			
			
        }

        /// <summary>
        /// Returns string of separated words and phrases with "OR" operator between them.
        /// This string will be used for the full text search in SQL.
        /// </summary>
        /// <returns></returns>
        private string GetSearchPredicateCondition()
        {
            string result = string.Empty;

            //Regex r = new Regex(@"(""(.*?)""|((\w|\*)+))");
            Regex r = new Regex(@"(""(.*?)""|((\w|\*|\W)+))");
            MatchCollection matches = r.Matches(txtKeywords.Text.Trim());

            if (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count - 1; i++)
                {
                    if (!matches[i].Value.Contains(@""""))
                    {
                        result += @"""" + matches[i].Value + @""" OR";
                    }
                    else
                    {
                        result += matches[i].Value + " OR";
                    }
                }
                if (!matches[matches.Count - 1].Value.Contains(@""""))
                {
                    result += @"""" + matches[matches.Count - 1].Value + @"""";
                }
                else
                {
                    result += matches[matches.Count - 1].Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns array with the ids of the selected forums.
        /// </summary>
        /// <returns></returns>
        private List<int> GetSelectedForumIds()
        {
            List<int> result = new List<int>();
            TreeNodeCollection checkedNodes = tvForums.CheckedNodes;
            foreach (TreeNode node in checkedNodes)
            {
                //Check whether the depth of the node in the tree is 2. On this level are displayed the forums.
                if (node.Depth == 2)
                {
                    result.Add(Convert.ToInt32(node.Value));
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if the argument is date.
        /// </summary>
        /// <param name="dateInString">The string representation of the date.</param>
        private bool IsDate(string dateInString)
        {
            try
            {
                DateTime tempDate = Convert.ToDateTime(dateInString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Register startup client script which initialize the user control.
        /// </summary>
        private void RegisterInitScript()
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(),"InitSearchArea",
                @"var ddlDateCriteriaClientID = '" + ddlDateCriteria.ClientID + @"';
	            var isControlPostBack = '" + (IsControlPostBack ? "1" : "0") + @"';
	            if(!!document.getElementById(ddlDateCriteriaClientID))
	            {
		            DisplayDateCriteria(ddlDateCriteriaClientID);
	            }
            	
	            var tvForumsClientID = '" + tvForums.ClientID + @"';
	            if(!!document.getElementById(tvForumsClientID))
	            {
		            AttachTreeCheckBoxEvent(tvForumsClientID);
		            if(isControlPostBack == 0)
		            {
			            CheckAllTreeNodes(tvForumsClientID);
		            }
	            } 
            	
	            var treeNodeCollapsed = new Image();
	            treeNodeCollapsed.src = '" + ResolveUrl(Utilities.GetImageUrl(this.Page, "ForumStyles/Images/treeNodeCollapsed.gif")) + @"';
	            var treeNodeExpanded = new Image();
	            treeNodeExpanded.src = '" + ResolveUrl(Utilities.GetImageUrl(this.Page, "ForumStyles/Images/treeNodeExpanded.gif")) + @"';
	            AttachCollapse(tvForumsClientID);", true);
        }
                    
    }
}