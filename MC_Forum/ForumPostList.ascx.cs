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
using Melon.Components.Forum.Exception;

namespace Melon.Components.Forum.UI.CodeBehind
{
	/// <summary>
	/// Contains the layout and functionality for listing the posts within a topic.
	/// </summary>
	/// <remarks>
	///     <para>
	///		The control represents the list of posts in a chosen topic. The list is displayed in a table, containing the following two columns:
	///		<list type="bullet">
	///			<item>A column with information about the author - avatar, name, number of posts and date when the author joined the forum.</item>
	///			<item>A column with information about the post - date of posting and actual content.</item>
	///		</list>
	///		The control may also contain a button for adding new post (depending on the current forum/topic settings and current user rights).
	///		</para>
	///		<para> </para>
	/// 	<para>
	/// 	The following web controls build this control:
	///			<list type="bullet">
	///				<item><term>Label lblErrorMessage:</term><description> used to display error messages (e.g. something went wrong during accessing database).</description></item>
	///				<item><term><see cref="Melon.Components.Forum.UI.CodeBehind.Pager">Pager</see> TopPager:</term><description> used to display the upper paging of the grid containing the posts.</description></item>
	///				<item><term>GridView gvPosts:</term><description> used to actually display the post list.</description></item>
	/// 			<item><term><see cref="Melon.Components.Forum.UI.CodeBehind.Pager">Pager</see> BottomPager: </term><description> used to display the bottom paging of the grid containing the posts.</description></item>
    ///  			<item><term>Button btnCreateForumPost:</term><description> allows creation of new posts.</description></item>
	///			</list>
	/// 	</para>
	///		<para> </para>
	///		<para>
	///		Remarks on the GridView:
	///			<list type="table">
	///				<item><term>Label lblAnonymousUser</term><description>This label is displayed if the post is made by an non-logged user. The text is retrieved from the associated resource file, the key is lblAnonymousUser.</description></item>
	///				<item><term>Div divAuthorDetails</term><description>This div is displayed if the post is made by a user that has had logged at the time of posting.</description></item>
	///				<item><term>Image imgPostAuthor</term><description>This image displays the avatar of the user, who made the post.</description></item>
	///				<item><term><see cref="Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay">ForumUserNicknameDisplay</see> cntrlForumUserNickname</term><description>Used to display the author nickname in simple text or as a link depending on the authors profile settings.</description></item>
	///				<item><term>Label lblPostAuthorRole</term><description>Displays the top role of the author of the post. Displayed only if the author is administrator or moderator.</description></item>
	///				<item><term>Label lblPostsTitle</term><description>Displays the text from the resource file, key lblPostsTitle.</description></item>
	///				<item><term>Label lblAuthorPostsCount</term><description>Displays the count of posts the users has made.</description></item>
	///				<item><term>Label lblRegistratedOnTitle</term><description>Displays the text from the resource file, key lblRegistratedOnTitle. The actual date is displayed as a text next to this control.</description></item>
	///				<item><term>Label lblPostedOn</term><description>Displays the text from the resource file, key lblPostedOn.</description></item>
	///				<item><term>HtmlAnchor aPostPosition</term><description>Defines an anchor used when navigating to this post from the search results.</description></item>
	///				<item><term>Text DateCreated</term><description>Displays the date of the post as a text.</description></item>
	///				<item><term>Label lblPostText</term><description>Displays the text of post.</description></item>
	///				<item><term>LinkButton lbtnEditForumPost</term><description>Allows editing of the post. This is visible if the logged user has the rights to edit it (i.e. he is super administrator, administrator, moderator or author of the post).</description></item>
    ///				<item><term>LinkButton lbtnDeleteForumPost</term><description>Allows deletion of the post. This is visible if the logged user has the rights to delete it (i.e. he is super administrator, administrator or moderator).</description></item>
	///				<item><term>EmptyDataTemplate</term><description>Displays the text from resource file, key NoPostsEntered.</description></item>
	///			</list>
	///		</para>
	///		<para> </para>
	///		<para>
	///			<list type="table">
	///				<listheader><description>Note</description></listheader>
	///				<item><description>Unless explicitly mentioned all resources and css classes are used according as in the initial source provided.</description></item>
	///			</list>
	///		</para>
	/// </remarks>
	/// <seealso cref="Melon.Components.Forum.UI.CodeBehind.Pager"/>
	/// <seealso cref="Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay"/>
    public partial class ForumPostList : ForumControl
    {
        /// <summary>
        /// Field to store the currently requested forumId.
        /// </summary>
        public int? ForumId;

        /// <summary>
        /// Name of currently requested forum.
        /// </summary>
        public string ForumName;

        /// <summary>
        /// Field to store the currently requested topicId.
        /// </summary>
        public int? TopicId;

        /// <summary>
        /// Name of currently requested topic.
        /// </summary>
        public string TopicName;

		/// <summary>
		/// Field to store the currently requested postId
		/// </summary>
		public int? PostId;
		


        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.ForumId = (int?)args[0];
            this.ForumName = (string)args[1];
            this.TopicId = (int?)args[2];
            this.TopicName = (string)args[3];
            this.PostId = (int?)args[4];
            this.CurrentPage = (int?)args[5];
        }

        /// <summary>
        /// Attaches event handlers to the conrols'events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.gvPosts.RowCreated += new GridViewRowEventHandler(gvPosts_RowCreated);
            this.gvPosts.RowDataBound += new GridViewRowEventHandler(gvPosts_RowDataBound);
            this.TopPager.PageChanged += new Pager.PagerEventHandler(Pager_OnPageChanged);
            this.BottomPager.PageChanged += new Pager.PagerEventHandler(Pager_OnPageChanged);
            this.btnCreateForumPost.Click += new EventHandler(btnCreateForumPost_Click);
            base.OnInit(e);
        }


        /// <summary>
        /// List all posts from specified topic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsControlPostBack)
            {
                if (ForumTopicInfo.Exists(this.TopicId.Value))
                {
                    //Register view for the topic which posts are listed.
                    int? currentUserId = null;
                    if (ParentControl.CurrentUser != null)
                    {
                        currentUserId = ParentControl.CurrentUser.AdapterId;
                    }
                    ForumTopicInfo.RegisterTopicVisit(this.TopicId.Value, currentUserId);

                    //Display posts for the selected forum topic.
                    ListPosts(TopicId);

                    if (!ForumTopicInfo.ArePostsAllowed(TopicId.Value))
                    {
                        btnCreateForumPost.Visible = false;
                    }
                    else
                    {
                        btnCreateForumPost.Visible = true;
                    }
                }
                else
                {
                    //The topic tried to be open doesn't exist.
                    ParentControl.OnLoadForumErrorInformationEvent(sender, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("TryToOpenNotExistingTopic").ToString()));
                    return;
                }
            }

        }


        /// <summary>
        /// Attach event handlers to the events of the controls in the GridView gvPosts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPosts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlForumUserNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlForumUserNickname");
                cntrlForumUserNickname.LoadForumUserProfileEvent += new Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay.LoadForumUserProfileEventHandler(cntrlForumUserNickname_LoadForumUserProfileEvent);

                LinkButton lbtnEditForumPost = (LinkButton)e.Row.FindControl("lbtnEditForumPost");
                lbtnEditForumPost.Command += new CommandEventHandler(lbtnEditForumPost_Command);

                LinkButton lbtnDeleteForumPost = (LinkButton)e.Row.FindControl("lbtnDeleteForumPost");
                lbtnDeleteForumPost.Command += new CommandEventHandler(lbtnDeleteForumPost_Command);
            }
        }

        /// <summary>
		/// Used to show/hide the divAuthorDetails area, set author details, prepare some links for ajax navigation, formatting of text, etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPosts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                int? postAuthorId = (drv["MC_ForumUserId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["MC_ForumUserId"]);
                string postAuthorUserName = (drv["PostAuthorUserName"] == DBNull.Value) ? string.Empty : Convert.ToString(drv["PostAuthorUserName"]);
                string postAuthorNickname = (drv["AuthorNickname"] == DBNull.Value) ? string.Empty : Convert.ToString(drv["AuthorNickname"]);
                bool postAuthorIsProfileVisible = (drv["AuthorIsProfileVisible"] == DBNull.Value) ? false : Convert.ToBoolean(drv["AuthorIsProfileVisible"]);
                int? postAuthorRoleId = (drv["PostAuthorTopRoleId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["PostAuthorTopRoleId"]);
                int? postAuthorPostsCount = (drv["AuthorPostsCount"] == DBNull.Value) ? 0 : Convert.ToInt32(drv["AuthorPostsCount"]);

                int? forumId = (drv["MC_ForumId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["MC_ForumId"]);
                int? topicId = (drv["MC_ForumTopicId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["MC_ForumTopicId"]);
                int? postId = (drv["Id"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["Id"]);
                string postText = (drv["Text"] == DBNull.Value) ? string.Empty : Convert.ToString(drv["Text"]);

                Label lblAnonymousUser = (Label)e.Row.FindControl("lblAnonymousUser");
                HtmlGenericControl divAuthorDetails = (HtmlGenericControl)e.Row.FindControl("divAuthorDetails");
                if (postAuthorId != null && !String.IsNullOrEmpty(postAuthorNickname))
                {
                    //Set Author details.
                    lblAnonymousUser.Visible = false;
                    divAuthorDetails.Style.Add("display", "inline");

                    //Author Nickname
                    Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlForumUserNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlForumUserNickname");
                    cntrlForumUserNickname.UserName = postAuthorUserName;
                    cntrlForumUserNickname.Nickname = postAuthorNickname;
                    cntrlForumUserNickname.IsProfileVisible = postAuthorIsProfileVisible;

                    //Author Role
                    if (postAuthorRoleId != null)
                    {
                        Label lblPostAuthorRole = (Label)e.Row.FindControl("lblPostAuthorRole");
                        lblPostAuthorRole.Text = "(" + GetLocalResourceObject(Convert.ToString(((ForumUserRole)postAuthorRoleId))).ToString() + ")";
                    }

                    //Auhtor Posts Count
                    Label lblAuthorPostsCount = (Label)e.Row.FindControl("lblAuthorPostsCount");
                    lblAuthorPostsCount.Text = Convert.ToString(postAuthorPostsCount);
                }
                else
                {
                    lblAnonymousUser.Visible = true;
                    divAuthorDetails.Style.Add("display", "none");
                }

                //Set Post Details.
				HtmlAnchor aPostPosition = (HtmlAnchor)e.Row.FindControl("aPostPosition");
				aPostPosition.HRef = "#forumId:" + Convert.ToString(forumId) + ";topicId:" + topicId + ";postId:" + postId;
				aPostPosition.Name = aPostPosition.HRef;

                Label lblPostText = (Label)e.Row.FindControl("lblPostText");
                lblPostText.Text = Server.HtmlEncode(postText).Replace("\r\n", "<br/>");

                LinkButton lbtnEditForumPost = (LinkButton)e.Row.FindControl("lbtnEditForumPost");
                lbtnEditForumPost.Visible = CouldEditPost(postAuthorId, forumId.Value);
                lbtnEditForumPost.CommandArgument = Convert.ToString(postId);
                if (postAuthorId != null)
                {
                    lbtnEditForumPost.CommandArgument += ";" + Convert.ToString(postAuthorId);
                }

                LinkButton lbtnDeleteForumPost = (LinkButton)e.Row.FindControl("lbtnDeleteForumPost");
                lbtnDeleteForumPost.Visible = CouldDeletePost(forumId.Value);
                lbtnDeleteForumPost.CommandArgument = Convert.ToString(postId);

              
            }
        }

        /// <summary>
        /// Event handler for event OnChange from both pager controls: TopPager and Bottom Pager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_OnPageChanged(object sender, Melon.Components.Forum.UI.CodeBehind.Pager.PagerEventArgs e)
        {
            gvPosts.PageIndex = e.NewPage;
            this.CurrentPage = null;
            ListPosts(TopicId);
        }


        /// <summary>
        /// Raises OnLoadForumPostAddEditEvent event of parent user control. 
        /// if there is currently logged user or the forum is public. Otherwise redirect to login page. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateForumPost_Click(object sender, EventArgs e)
        {
            if (ForumTopicInfo.Exists(this.TopicId.Value))
            {
                //Check whether the current logged user has permissions to create post.
                //If he doesn't then redirect to login page.
                if (!ForumTopicInfo.CouldForumUserPost(ParentControl.CurrentUser, TopicId.Value))
                {
                    //Before redirect to login page create cookie that will save information that the user requested to create post 
                    //It will be checked after login and 
                    //if it exists then the form for create post will be displayed on the screen
                    string createPostCookie = "mc_forum_mode_create_post";
                    Response.Cookies[createPostCookie].Value = "true";
                    Response.Cookies[createPostCookie].Expires = DateTime.Now.AddMinutes(5);

                    string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString() + "&topicId=" + TopicId.ToString();
                    Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
                }
                else
                {
                    LoadForumPostAddEditEventArgs args = new LoadForumPostAddEditEventArgs();
                    args.ForumTopicId = this.TopicId;
                    args.ForumId = this.ForumId;
                    ParentControl.OnLoadForumPostAddEditEvent(sender, args);
                }
            }
            else
            {
               
                Melon.Components.Forum.Forum objForum = ForumInfo.Load(this.ForumId.Value);
                if (objForum != null)
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToCreatePostInNotExistingTopic").ToString(), false));
                    LoadForumTopicListEventArgs argsTopicList = new LoadForumTopicListEventArgs();
                    argsTopicList.ForumId = objForum.Id;
                    argsTopicList.ForumName = objForum.Name;
                    ParentControl.OnLoadForumTopicListEvent(sender, argsTopicList);
                    return;
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToCreatePostInNotExistingForum").ToString(), false));
                    ParentControl.OnLoadForumListEvent(sender, new LoadForumListEventArgs());
                    return;
                }
            }
        }

        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditForumPost_Command(object sender, CommandEventArgs e)
        {
            string[] editArgs = e.CommandArgument.ToString().Split(';');
            int postId = Convert.ToInt32(editArgs[0]);
            int? postAuthorId = (editArgs.Length > 1) ? Convert.ToInt32(editArgs[1]) : (int?)null; //posts from anonymous users have no author id in the command args;

            if (ParentControl.CurrentUser != null)
            {
               
                if ((ParentControl.CurrentUser.IsSuperAdministrator())
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, this.ForumId.Value))
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, this.ForumId.Value))
                   || (postAuthorId.HasValue && ParentControl.CurrentUser.AdapterId == postAuthorId))
                {
                    if (ForumPostInfo.Exists(postId))
                    {
                        LoadForumPostAddEditEventArgs args = new LoadForumPostAddEditEventArgs(this.ForumId, this.TopicId, postId);
                        args.CurrentPage = this.gvPosts.PageIndex + 1;
                        ParentControl.OnLoadForumPostAddEditEvent(sender, args);
                    }
                    else
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(Convert.ToString(GetLocalResourceObject("TryToEditNotExistingPost")), true));
                    }
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(Convert.ToString(GetLocalResourceObject("UserNotAllowedToEditPost")),true));
                    return;
                }
            }
            else
            {
                string url = Request.Url.AbsolutePath + "?forumId=" + Convert.ToString(this.ForumId) + "&topicId=" + Convert.ToString(this.TopicId);
                Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
            }
        }

        /// <summary>
        /// Delete post. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeleteForumPost_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if ((ParentControl.CurrentUser.IsSuperAdministrator())
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, this.ForumId.Value))
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, this.ForumId.Value)))
                {
                    int postId = Convert.ToInt32(e.CommandArgument);
                    try
                    {
                        ForumPostInfo.Delete(postId);
                    }
                    catch
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumPostDelete").ToString(), false));
                        return;
                    }

                    LoadForumPostListEventArgs eventArgs = new LoadForumPostListEventArgs();
                    if (gvPosts.Rows.Count > 1)
                    {
                        eventArgs.CurrentPage = gvPosts.PageIndex + 1;
                    }
                    else
                    {
                        eventArgs.CurrentPage = (gvPosts.PageIndex > 0) ? gvPosts.PageIndex : 1;
                    }
                    eventArgs.ForumId = this.ForumId;
                    eventArgs.ForumName = this.ForumName;
                    eventArgs.ForumTopicId = this.TopicId;
                    eventArgs.ForumTopicName = this.TopicName;

                    ParentControl.OnLoadForumPostListEvent(sender, eventArgs);
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToDeletePost").ToString(),true));
                    return;
                }
            }
            else
            {
                string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString() + "&topicId=" + TopicId.ToString();
                Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
            }
        }


        /// <summary>
        /// Raises event LoadForumUserDetailsEvent of parent control if there is currently logged user.
        /// Otherwise redirect to login page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cntrlForumUserNickname_LoadForumUserProfileEvent(object sender, CommandEventArgs e)
        {
            string username = Convert.ToString(e.CommandArgument);
            if (ParentControl.CurrentUser == null)
            {
                string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString() + "&topicId=" + this.TopicId.ToString() + "&mode=profile&username=" + username;
                Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
            }
            else
            {
                nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + this.ForumId.ToString() + ";topicId:" + this.TopicId.ToString() + ";mode:profile;username:" + username);
                ParentControl.OnLoadForumUserDetailsEvent(sender, new LoadForumUserDetailEventArgs(username));
            }
        }


        /// <summary>
        /// Check whether the currently logged user could Edit post.
        /// He has this right in case he is Super Administrators, Administrator of the forum, 
        /// or Moderator of the forum or Author of the post.
        /// </summary>
        /// <param name="postAuthorId"></param>
        /// <param name="forumId"></param>
        /// <returns>true/false</returns>
        protected bool CouldEditPost(int? postAuthorId, int forumId)
        {
            bool result = false;
            if (ParentControl.CurrentUser != null)
            {
                if ((ParentControl.CurrentUser.IsSuperAdministrator())
                    || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, forumId))
                    || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, forumId))
                    )
                {
                    result = true;
                }
                else
                {
                    if (ParentControl.CurrentUser.AdapterId == postAuthorId)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check whether the currently logged user could Delete post.
        /// He has this right in case he is Super Administrators, Administrator of the forum, 
        /// or Moderator of the forum.
        /// </summary>
        /// <param name="forumId"></param>
        /// <returns>true/false</returns>
        protected bool CouldDeletePost(int forumId)
        {
            bool result = false;
            if (ParentControl.CurrentUser != null)
            {
                if ((ParentControl.CurrentUser.IsSuperAdministrator())
                    || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, forumId))
                    || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, forumId)))
                {
                    result = true;
                }
            }

            return result;
        }


        /// <summary>
        /// List all posts from topic specified by id
        /// </summary>
        /// <param name="topicId"></param>
        private void ListPosts(int? topicId)
        {
            ForumPostView objForumPostView = new ForumPostView();
            objForumPostView.ForumTopicId = topicId;
            DataTable dtPosts = ForumPostView.List(objForumPostView);

            gvPosts.PageSize = ForumSettings.PostsPageSize;
            gvPosts.DataSource = dtPosts;
            gvPosts.DataBind();

            if (dtPosts.Rows.Count != 0)
            {
                if (!IsControlPostBack)
                {
                    //Show last page with posts when we open page with topic posts
                    gvPosts.PageIndex = gvPosts.PageCount - 1;
                    if (this.CurrentPage != null && this.CurrentPage <= this.gvPosts.PageCount)
                    {
                        this.gvPosts.PageIndex = this.CurrentPage.Value - 1;
                    }

					if (PostId != null)
                    {
						string searchedID = PostId.ToString();
                        // Find the id in the datasource
                        int foundInPage = 0;
                        int recordsCounter = 0;
                        foreach (DataRow dr in dtPosts.Rows)
                        {
                            if (Convert.ToString(dr["Id"]) == searchedID)
                            {
                                break;
                            }
                            recordsCounter++;
                            if (recordsCounter > ForumSettings.TopicsPageSize)
                            {
                                recordsCounter = 0;
                                foundInPage++;
                            }
                        }
                        if (foundInPage > this.gvPosts.PageCount)
                        {
                            foundInPage = this.gvPosts.PageCount - 1;
                        }

                        this.gvPosts.PageIndex = foundInPage;
                    }

                    gvPosts.DataBind();
                }

				int forumId = Convert.ToInt32(dtPosts.Rows[0]["MC_ForumId"]);
				int forumTopicId = Convert.ToInt32(dtPosts.Rows[0]["MC_ForumTopicId"]);

				TopPager.FillPaging(gvPosts.PageCount, gvPosts.PageIndex + 1, 5, gvPosts.PageSize, dtPosts.Rows.Count, "forumId:" + forumId.ToString() + ";topicId:" + forumTopicId);
				BottomPager.FillPaging(gvPosts.PageCount, gvPosts.PageIndex + 1, 5, gvPosts.PageSize, dtPosts.Rows.Count, "forumId:" + forumId.ToString() + ";topicId:" + forumTopicId);
            }
            else
            {
                TopPager.Visible = false;
                BottomPager.Visible = false;
            }

        }
    }
}