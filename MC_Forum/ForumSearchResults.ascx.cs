using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.Forum.Configuration;
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.SearchEngine;
using Melon.Components.Forum.UI.Controls;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for listing all posts found from search.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     The ForumSearchResults user control perform search based on search criteria stored in <see cref="SearchEngine"/> property.
    ///     After search results are displayed in GridView gvResults.
    ///     </para>
    ///     <para>
    ///     The GridView gvResults displays the following information for the found posts:
    ///		<list type="bullet">
    ///			<item>Forum to which belongs the the found post.</item>
    ///			<item>Topic to which belongs the found post.</item>
    ///			<item>Text of the found post.</item>
    ///			<item>Author of the found post.</item>
    ///		</list>
    ///     The forum and topic of the posts are links so there is access to them.
    ///     </para>
    ///     <para>
    ///     There are Pager controls at the top and the bottom of the GridView with the results.
    ///     </para>
    ///     <para>
    ///     All web controls from ForumSearchResults are using the local resources.
    ///     To customize them modify resource file ForumSearchResults.resx placed in the MC_Forum folder.
    ///     </para>
    /// </remarks>
    public partial class ForumSearchResults : ForumControl
    {
		/// <summary>
		/// A reference to the search engine class
		/// </summary>
        public ForumSearchEngine SearchEngine;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.SearchEngine = (ForumSearchEngine)args[0];
        }

        /// <summary>
        /// Attach event handlers to the controls'events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.gvResults.RowCreated += new GridViewRowEventHandler(gvResults_RowCreated);
            this.gvResults.RowDataBound += new GridViewRowEventHandler(gvResults_RowDataBound);
            this.TopPager.PageChanged += new Pager.PagerEventHandler(Pager_PageChanged);
            this.BottomPager.PageChanged += new Pager.PagerEventHandler(Pager_PageChanged);
            base.OnInit(e);
        }
           
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ListSearchResults();
        }


        /// <summary>
        /// Attach event handlers to the events of the controls in the GridView gvResults.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResults_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
				MelonLinkButton lbtnOpenForum = (MelonLinkButton)e.Row.FindControl("lbtnOpenForum");
                lbtnOpenForum.Command+=new CommandEventHandler(lbtnOpenForum_Command);

				MelonLinkButton lbtnOpenTopic = (MelonLinkButton)e.Row.FindControl("lbtnOpenTopic");
                lbtnOpenTopic.Command+=new CommandEventHandler(lbtnOpenTopic_Command);

                Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlForumUserNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlForumUserNickname");
                cntrlForumUserNickname.LoadForumUserProfileEvent+=new Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay.LoadForumUserProfileEventHandler(cntrlForumUserNickname_LoadForumUserProfileEvent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                int forumId = Convert.ToInt32(drv["ForumId"]);
                int topicId = Convert.ToInt32(drv["TopicId"]);
                int postId = Convert.ToInt32(drv["Id"]);
                string forumName = Convert.ToString(drv["ForumName"]);
                string topicName = Convert.ToString(drv["TopicName"]);
                string postText = Convert.ToString(drv["Text"]);

                int? forumUserId = (drv["MC_ForumUserId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["MC_ForumUserId"]);
                string forumUserUsername = (drv["PostAuthorUserName"] == DBNull.Value) ? String.Empty : Convert.ToString(drv["PostAuthorUserName"]);
                string forumUserNickname = (drv["PostAuthorNickname"] == DBNull.Value) ? String.Empty : Convert.ToString(drv["PostAuthorNickname"]);
                bool? isProfileVisible = (drv["PostAuthorIsProfileVisible"] == DBNull.Value) ? (bool?)null : Convert.ToBoolean(drv["PostAuthorIsProfileVisible"]);

                //Set Forum and Topic details of the found post.
				MelonLinkButton lbtnOpenForum = (MelonLinkButton)e.Row.FindControl("lbtnOpenForum");
                lbtnOpenForum.Text = Server.HtmlEncode(forumName);
				lbtnOpenForum.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?forumId=" + forumId.ToString();
                lbtnOpenForum.CommandArgument = Convert.ToString(forumId) + ";" + forumName;

                MelonLinkButton lbtnOpenTopic = (MelonLinkButton)e.Row.FindControl("lbtnOpenTopic");
                lbtnOpenTopic.Text = Server.HtmlEncode(topicName);
				lbtnOpenTopic.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?forumId=" + forumId.ToString() + "&topicId=" + topicId.ToString() + "&postId=" + postId.ToString();
                lbtnOpenTopic.CommandArgument = Convert.ToString(topicId) + ";" + topicName + ";" + Convert.ToString(forumId) + ";" + forumName + ";" + postId;

                //Set Post Details.
                if (forumUserId != null)
                {
                    Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlForumUserNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlForumUserNickname");
                    cntrlForumUserNickname.UserName = forumUserUsername;
                    cntrlForumUserNickname.Nickname = forumUserNickname;
                    cntrlForumUserNickname.IsProfileVisible = isProfileVisible.Value;
                }

                Label lblPostText = (Label)e.Row.FindControl("lblPostText");
                lblPostText.Text = Server.HtmlEncode(postText).Replace("\r\n", "<br/>");
            }
        }

        /// <summary>
        /// Event handler for event OnChange for both pager controls: TopPager and Bottom Pager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_PageChanged(object sender, Melon.Components.Forum.UI.CodeBehind.Pager.PagerEventArgs e)
        {
            gvResults.PageIndex = e.NewPage;
            ListSearchResults();
        }


        /// <summary>
        /// Raises the event LoadForumTopicListEvent of the parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenForum_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            int? forumId = Convert.ToInt32(commandArgs[0]);
            string forumName = commandArgs[1];

            LoadFoundForumItemEventArgs args = new LoadFoundForumItemEventArgs();
            args.ForumId = forumId;
            args.ForumName = forumName;

			nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + forumId);

            ParentControl.OnLoadFoundForumItemEvent(sender, args);
        }

        /// <summary>
        /// Raises the event LoadForumPostListEvent of the parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenTopic_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            int? topicId = Convert.ToInt32(commandArgs[0]);
            string topicName = commandArgs[1];
            int? forumId = Convert.ToInt32(commandArgs[2]);
            string forumName = commandArgs[3];
			int? postId = Convert.ToInt32(commandArgs[4]);

            LoadFoundForumItemEventArgs args = new LoadFoundForumItemEventArgs();
            args.ForumId = forumId;
            args.ForumName = forumName;
            args.TopicId = topicId;
            args.TopicName = topicName;
			args.PostId = postId;

			//nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + forumId + ";topicId:" + topicId + ";postId:" + postId);

			ParentControl.OnLoadFoundForumItemEvent(sender, args);
        }

        /// <summary>
        /// Raises the event LoadForumUserProfileDeatailsEvent of the parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cntrlForumUserNickname_LoadForumUserProfileEvent(object sender, CommandEventArgs e)
        {
            string username = Convert.ToString(e.CommandArgument);

			nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("mode:profile;username:" + username);

            ParentControl.OnLoadForumUserDetailsEvent(sender, new LoadForumUserDetailEventArgs(username));
        }
     

        /// <summary>
        /// Performs search in forum posts and display the results in GridView gvResults.
        /// </summary>
        private void ListSearchResults()
        {
            if (SearchEngine != null)
            {
                DataTable dtSearchResults = SearchEngine.Search();
                gvResults.PageSize = ForumSettings.SearchResultsPageSize;

                string sortExpression = "DateCreated";
                string sortDirection = "DESC";
                switch (SearchEngine.SortField)
                {
                    case SearchResultsSortField.PostedDate:
                        sortExpression = "DateCreated";
                        sortDirection = "DESC";
                        break;
                    case SearchResultsSortField.ForumName:
                        sortExpression = "ForumName";
                        sortDirection = "ASC";
                        break;
                    case SearchResultsSortField.TopicName:
                        sortExpression = "TopicName";
                        sortDirection = "ASC";
                        break;
                    case SearchResultsSortField.AuthorNickname:
                        sortExpression = "PostAuthorNickname";
                        sortDirection = "ASC";
                        break;
                }

                DataView dv = dtSearchResults.DefaultView;
                dv.Sort = sortExpression + " " + sortDirection;

                gvResults.DataSource = dv;
                gvResults.DataBind();

                if (dtSearchResults.Rows.Count != 0)
                {
					TopPager.FillPaging(gvResults.PageCount, gvResults.PageIndex + 1, 5, gvResults.PageSize, dtSearchResults.Rows.Count, "mode:search");
					BottomPager.FillPaging(gvResults.PageCount, gvResults.PageIndex + 1, 5, gvResults.PageSize, dtSearchResults.Rows.Count, "mode:search");
                }
                else
                {
                    TopPager.Visible = false;
                    BottomPager.Visible = false;
                }
            }

        }
    }
}