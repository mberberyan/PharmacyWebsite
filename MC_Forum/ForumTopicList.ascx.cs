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
using System.Collections.Generic;
using Melon.Components.Forum.Configuration;
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.UI.Controls;

namespace Melon.Components.Forum.UI.CodeBehind
{
	/// <summary>
	/// Contains the layout and functionality for listing the topics within a forum.
	/// </summary>
	/// <remarks>
	///     <para>
	///		The control represents the list of topics in a chosen forum. The list is displayed in a table, containing the following columns:
	///		<list type="bullet">
	///			<item>The topic title.</item>
	///			<item>The author of the topic.</item>
	///			<item>The number of times the topic has been viewed.</item>
	///			<item>The number of posts within the topic.</item>
	///			<item>Last post information (date and author) and a link to the last post.</item>
	///		</list>
	///		The control may also contain a button for adding new topic (depending on the current forum's settings and current user rights).
	///		</para>
	///		<para> </para>
	/// 	<para>
	/// 	The following web controls build this control:
	///			<list type="bullet">
	///				<item><term>Label lblMessage:</term><description> used to display information messages (e.g. topic was deleted after administrator actually deleted a topic).</description></item>
	///				<item><term>Label lblErrorMessage:</term><description> used to display error messages (e.g. something went wrong during accessing database).</description></item>
	///				<item><term><see cref="Melon.Components.Forum.UI.CodeBehind.Pager">Pager</see> TopPager:</term><description> used to display the upper paging of the grid containing the topics.</description></item>
	///				<item><term>GridView gvForumTopics:</term><description> used to actually display the topic list.</description></item>
	///				<item><term>HtmlTable tblEmptyDataTemplate:</term><description> displayed when there are no topics in the current forum. Note that this is needed, because when EmptyDataTemplate of the GridView is used, the GridView header disappears.</description></item>
	/// 			<item><term><see cref="Melon.Components.Forum.UI.CodeBehind.Pager">Pager</see> BottomPager: </term><description> used to display the bottom paging of the grid containing the topics.</description></item>
	///  			<item><term>Button btnCreateForumTopic:</term> allows creation of new topics.<description></description></item>
	///			</list>
	/// 	</para>
	///		<para> </para>
	///		<para>
	///		Remarks on the GridView:
	///			<list type="table">
	///				<item><term>Header titles</term><description>Header titles are taken from associated resource file. The resource keys are TopicHeader, AuthorHeader, ViewsHeader, PostsHeader and LastPostHeader. These are also used in the tblEmptyDataTemplate HtmlTable.</description></item>
	///				<item><term>Image imgTopicTypeIcon</term><description>This image can be one of the following: announcement_topic.gif, sticky_topic.gif or normal_topic.gif depending on what is the type of the topic (announcement, sticky or normal). These images reside in ForumStyles/Images folder.</description></item>
	///				<item><term>LinkButton lbtnOpenForumTopic</term><description>This is used to navigate to the posts of the topic.</description></item>
	///				<item><term>Image imgClosed</term><description>Displayed if this topic is closed.</description></item>
	///		 		<item><term>Image imgInActive</term><description>Displayed if this topic is active. This is visible only to super administrators.</description></item>
	///				<item><term>Label lblTopicDescription</term><description>Used to show the description of the topic.</description></item>
	///				<item><term>LinkButton lbtnEditForumTopic</term><description>Used to load the ForumTopicAddEdit.ascx control, thus allowing editing of the selected topic. Visible to super administrators only.</description></item>
	///		 		<item><term>LinkButton lbtnDeleteForumTopic</term><description>Allows deletion of the selected topic. Visible to superadministrators only.</description></item>
	///				<item><term><see cref="Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay">ForumUserNicknameDisplay</see> cntrlTopicAuthorNickname</term><description>Used to display the author nickname in simple text or as a link depending on the authors profile settings.</description></item>
	///				<item><term>Label lblTopicAuthorRole</term><description>Displays the top role of the author of the topic. Displayed only if the author is administrator or moderator.</description></item>
	///				<item><term>Label lblTopicViewsCount</term><description>Displays the number of views of the topic.</description></item>
	///				<item><term>Label lblTopicPostsCount</term><description>Displays the number of posts in the topic.</description></item>
	///				<item><term>Div divLastPost</term><description>If there are no posts this will have <c>display:none</c> in its style.</description></item>
	///				<item><term>Last post date</term><description>This is displayed as text.</description></item>
	///				<item><term>ImageButton ibtnOpenPost</term><description>Used to navigate to the last post.</description></item>
	///				<item><term>Label lblAuthorTitle</term><description>Used to display the author of the last post.</description></item>
	///				<item><term><see cref="Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay">ForumUserNicknameDisplay</see> cntrlLastPostAuthorNickname</term><description>Used to display the last post's author nickname in simple text or as a link depending on the authors profile settings.</description></item>
	///				<item><term>Label lblLastPostAuthorRole</term><description>Displays the top role of the author of the last post. Displayed only if the author is administrator or moderator.</description></item>
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
    public partial class ForumTopicList : ForumControl
    {
		/// <summary>
		/// Field to store the forum id, which topics are being listed.
		/// </summary>
        public int? ForumId;

        /// <summary>
        /// Name of forum which topics are being listed.
        /// </summary>
        public string ForumName;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.ForumId = (int?)args[0];
            this.ForumName = (string)args[1];
            this.CurrentPage = (int?)args[2];
            this.Message = (string)args[3];
        }

        /// <summary>
        /// Attach event handlers to the controls'events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.gvForumTopics.RowCreated += new GridViewRowEventHandler(gvForumTopics_RowCreated);
            this.gvForumTopics.RowDataBound+=new GridViewRowEventHandler(gvForumTopics_RowDataBound);

            this.TopPager.PageChanged += new Pager.PagerEventHandler(Pager_PageChanged);
            this.BottomPager.PageChanged += new Pager.PagerEventHandler(Pager_PageChanged);

            this.btnCreateForumTopic.Click += new EventHandler(btnCreateForumTopic_Click);
            base.OnInit(e);
        }


        /// <summary>
        /// Performs some initialization of the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (!this.IsControlPostBack)
            {
                //In case the forum is closed then it is not allowed to users to post new topics
                //=>hide button btnCreateForumTopic
                Melon.Components.Forum.Forum objForum = ForumInfo.Load(ForumId.Value);
                this.ForumName = objForum.Name;
                if (objForum == null)
                {
                    //Forum doesn't exist.
                    ParentControl.OnLoadForumErrorInformationEvent(sender, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("TryToOpenNotExistingForum").ToString()));
                    return;
                }
                else
                {
                    if (objForum.IsClosed.Value)
                    {
                        btnCreateForumTopic.Visible = false;
                    }
                    else
                    {
                        btnCreateForumTopic.Visible = true;
                    }

                    //Display list of forum topics. This happens only the first time the user control is loaded
                    ListForumTopics(this.ForumId);

                    if (Message != null)
                    {
                        lblMessage.Text = Message;
                        lblMessage.Visible = true;
                    }
                    else
                    {
                        lblMessage.Visible = false;
                    }
                }

            }
        }


        /// <summary>
		/// Used to attach all events in the child controls of a row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvForumTopics_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
				MelonLinkButton lbtnOpenForumTopic = (MelonLinkButton)e.Row.FindControl("lbtnOpenForumTopic");
                lbtnOpenForumTopic.Command += new CommandEventHandler(lbtnOpenForumTopic_Command);

                LinkButton lbtnEditForumTopic = (LinkButton)e.Row.FindControl("lbtnEditForumTopic");
                lbtnEditForumTopic.Command += new CommandEventHandler(lbtnEditForumTopic_Command);

                LinkButton lbtnDeleteForumTopic = (LinkButton)e.Row.FindControl("lbtnDeleteForumTopic");
                lbtnDeleteForumTopic.Command += new CommandEventHandler(lbtnDeleteForumTopic_Command);

                Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlTopicAuthorNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlTopicAuthorNickname");
                cntrlTopicAuthorNickname.LoadForumUserProfileEvent += new Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay.LoadForumUserProfileEventHandler(cntrlForumUserNickname_LoadForumUserProfileEvent);

                ImageButton ibtnOpenPost = (ImageButton)e.Row.FindControl("ibtnOpenPost");
                ibtnOpenPost.Command += new CommandEventHandler(ibtnOpenPost_Command);

                Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlLastPostAuthorNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlLastPostAuthorNickname");
                cntrlLastPostAuthorNickname.LoadForumUserProfileEvent += new Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay.LoadForumUserProfileEventHandler(cntrlForumUserNickname_LoadForumUserProfileEvent);

            }
        }

        /// <summary>
        /// Used to display/hide topics depending on whether the topic could be seen. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvForumTopics_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                bool isTopicActive = Convert.ToBoolean(drv["IsActive"]);
                int forumId = Convert.ToInt32(drv["MC_ForumId"]);


                if (!CouldSeeTopic(isTopicActive, forumId))
                {
                    e.Row.Visible = false;
                }
                else
                {
                    ForumTopicType topicType = (ForumTopicType)Convert.ToInt32(drv["MC_ForumTopicTypeId"]);
                    int topicId = Convert.ToInt32(drv["Id"]);
                    bool topicIsClosed = Convert.ToBoolean(drv["IsClosed"]);
                    string topicName = Convert.ToString(drv["Name"]);
                    string topicDescription = (drv["Description"] == DBNull.Value) ? String.Empty : Convert.ToString(drv["Description"]);
                    int topicViewsCount = Convert.ToInt32(drv["ViewsCount"]);
                    int topicPostsCount = Convert.ToInt32(drv["PostsCount"]);

                    int? topicAuthorId = (drv["MC_ForumUserId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["MC_ForumUserId"]);
                    string topicAuthorUserName = (drv["TopicAuthorUserName"] == DBNull.Value) ? String.Empty : Convert.ToString(drv["TopicAuthorUserName"]);
                    string topicAuthorNickname = (drv["TopicAuthorNickname"] == DBNull.Value) ? String.Empty : Convert.ToString(drv["TopicAuthorNickname"]);
                    bool topicAuthorIsProfileVisible = (drv["TopicAuthorIsProfileVisible"] == DBNull.Value) ? false : Convert.ToBoolean(drv["TopicAuthorIsProfileVisible"]);
                    int? topicAuthorRoleId = (drv["TopicAuthorTopRoleId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["TopicAuthorTopRoleId"]);
                
                    int? lastPostId = (drv["LastPostId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["LastPostId"]);

                    string postAuthorUserName = (drv["LastPostAuthorUserName"] == DBNull.Value) ? String.Empty : Convert.ToString(drv["LastPostAuthorUserName"]);
                    string postAuthorNickname = (drv["LastPostAuthorNickname"] == DBNull.Value) ? String.Empty : Convert.ToString(drv["LastPostAuthorNickname"]);
                    bool postAuthorIsProfileVisible = (drv["LastPostAuthorIsProfileVisible"] == DBNull.Value) ? false : Convert.ToBoolean(drv["LastPostAuthorIsProfileVisible"]);
                    int? postAuthorRolelId = (drv["LastPostAuthorTopRoleId"] == DBNull.Value) ? (int?)null : Convert.ToInt32(drv["LastPostAuthorTopRoleId"]);
                   

                    //Set Topic Details
                    Image imgTopicTypeIcon = (Image)e.Row.FindControl("imgTopicTypeIcon");
                    imgTopicTypeIcon.ImageUrl = Utilities.GetImageUrl(this.Page, GetTopicIcon(topicType));
                    imgTopicTypeIcon.ToolTip = GetTopicIconTooltip(topicType);

					MelonLinkButton lbtnOpenForumTopic = (MelonLinkButton)e.Row.FindControl("lbtnOpenForumTopic");
					lbtnOpenForumTopic.Href = Request.Url.AbsoluteUri.Split(new char[]{'?'})[0] + "?forumId=" + forumId.ToString() + "&topicId=" + topicId.ToString();
                    lbtnOpenForumTopic.Text = Server.HtmlEncode(topicName);
                    lbtnOpenForumTopic.CommandArgument = topicId.ToString() + ";" + topicName;

                    Image imgClosed = (Image)e.Row.FindControl("imgClosed");
                    imgClosed.Visible = topicIsClosed;

                    Image imgInActive = (Image)e.Row.FindControl("imgInActive");
                    imgInActive.Visible = CouldDeleteTopic(forumId) && !isTopicActive;

                    Label lblTopicDescription = (Label)e.Row.FindControl("lblTopicDescription");
                    lblTopicDescription.Text = Server.HtmlEncode(topicDescription).Replace("\r\n", "<br/>");

                    LinkButton lbtnEditForumTopic = (LinkButton)e.Row.FindControl("lbtnEditForumTopic");
                    lbtnEditForumTopic.Visible = CouldEditTopic(topicAuthorId, forumId);
                    lbtnEditForumTopic.CommandArgument = topicId.ToString();
                    if (topicAuthorId != null)
                    {
                        lbtnEditForumTopic.CommandArgument += ";" + topicAuthorId.ToString();
                    }

                    LinkButton lbtnDeleteForumTopic = (LinkButton)e.Row.FindControl("lbtnDeleteForumTopic");
                    lbtnDeleteForumTopic.Visible = CouldDeleteTopic(forumId);
                    lbtnDeleteForumTopic.CommandArgument = topicId.ToString() + ";" + topicName;

                    Label lblTopicViewsCount = (Label)e.Row.FindControl("lblTopicViewsCount");
                    lblTopicViewsCount.Text = topicViewsCount.ToString();

                    Label lblTopicPostsCount = (Label)e.Row.FindControl("lblTopicPostsCount");
                    lblTopicPostsCount.Text = topicPostsCount.ToString();

                    //Set Topic Author Details
                    if (topicAuthorId != null)
                    {
                        Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlTopicAuthorNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlTopicAuthorNickname");
                        cntrlTopicAuthorNickname.UserName = topicAuthorUserName;
                        cntrlTopicAuthorNickname.Nickname = topicAuthorNickname;
                        cntrlTopicAuthorNickname.IsProfileVisible = topicAuthorIsProfileVisible;
                    }
                    if (topicAuthorRoleId != null)
                    {
                        Label lblTopicAuthorRole = (Label)e.Row.FindControl("lblTopicAuthorRole");
                        lblTopicAuthorRole.Text = "(" + GetLocalResourceObject(Convert.ToString(((ForumUserRole)topicAuthorRoleId))).ToString() + ")";
                    }

                    HtmlGenericControl divLastPost = (HtmlGenericControl)e.Row.FindControl("divLastPost");
                    if (lastPostId != null)
                    {
                        divLastPost.Style.Add("display", "inline");
                        ImageButton ibtnOpenPost = (ImageButton)e.Row.FindControl("ibtnOpenPost");
                        ibtnOpenPost.CommandArgument = Convert.ToString(topicId) + ";" + topicName;
                        
                        //Set details of last post author.
                        if (postAuthorUserName != null)
                        {
                            Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlLastPostAuthorNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)e.Row.FindControl("cntrlLastPostAuthorNickname");
                            cntrlLastPostAuthorNickname.UserName = postAuthorUserName;
                            cntrlLastPostAuthorNickname.Nickname = postAuthorNickname;
                            cntrlLastPostAuthorNickname.IsProfileVisible = postAuthorIsProfileVisible;

                            //Last Post Author Role
                            if (postAuthorRolelId != null)
                            {
                                Label lblLastPostAuthorRole = (Label)e.Row.FindControl("lblLastPostAuthorRole");
                                lblLastPostAuthorRole.Text = "(" + GetLocalResourceObject(Convert.ToString(((ForumUserRole)postAuthorRolelId))).ToString() + ")";
                            }
                        }
                    }
                    else
                    {
                        divLastPost.Style.Add("display", "none");
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for eventOnPageChange for both pager controls: TopPager and Bottom Pager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_PageChanged(object sender, Pager.PagerEventArgs e)
        {
            gvForumTopics.PageIndex = e.NewPage;
            
            this.CurrentPage = null;
            ListForumTopics(ForumId);
        }


        /// <summary>
        /// Raises OnLoadForumTopicAddEditEvent event of parent user control 
        /// if there is currently logged user or the forum is public. Otherwise redirect to login page. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateForumTopic_Click(object sender, EventArgs e)
        {
            if (ForumInfo.Exists(this.ForumId.Value))
            {
                //Check whether the current user has permissions to create topic.
                //If he doesn't then redirect to login page.
                if (!ForumInfo.CouldForumUserPostTopic(ParentControl.CurrentUser, ForumId.Value))
                {
                    //Before redirect to login page create cookie that will save information that the user requested to create topic
                    //It will be checked after login and 
                    //if it exists and there is currently logged user  
                    //then the form for create topic will be displayed on the screen

                    string createTopicCookie = "mc_forum_mode_create_topic";
                    Response.Cookies[createTopicCookie].Value = "true";
                    Response.Cookies[createTopicCookie].Expires = DateTime.Now.AddMinutes(5);

                    string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString();
                    Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
                }
                else
                {
                    ParentControl.OnLoadForumTopicAddEditEvent(sender, new LoadForumTopicAddEditEventArgs(this.ForumId));
                }
            }
            else
            {
                ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToCreateTopicInNotExistingForum").ToString(), false));
                ParentControl.OnLoadForumListEvent(sender, new LoadForumListEventArgs());
            }
        }

        /// <summary>
        /// Used to perform the navigation to open the post list of the selected topic. Also maintains the ajax navigation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenForumTopic_Command(object sender, CommandEventArgs e)
        {
            //Get details of the topic which is tried to be opened.
            string[] forumTopicDetails = e.CommandArgument.ToString().Split(';');
            int forumTopicId = Convert.ToInt32(forumTopicDetails[0]);
            string forumTopicName = forumTopicDetails[1];

			//nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + this.ForumId.ToString() + ";topicId:" + forumTopicId.ToString());

            //Open topic
            LoadForumPostListEventArgs objOpenForumTopicEventArgs = new LoadForumPostListEventArgs();
			objOpenForumTopicEventArgs.ForumId = this.ForumId;
            objOpenForumTopicEventArgs.ForumName = this.ForumName;
            objOpenForumTopicEventArgs.ForumTopicId = forumTopicId;
            objOpenForumTopicEventArgs.ForumTopicName = forumTopicName;
			ParentControl.OnLoadForumPostListEvent(sender, objOpenForumTopicEventArgs);
        }

        /// <summary>
        /// Raises event LoadForumTopicAddEditEvent of parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditForumTopic_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                string[] editArgs = e.CommandArgument.ToString().Split(';');
                int topicId = Convert.ToInt32(editArgs[0]);
                int? topicAuthorId = (editArgs.Length > 1) ? Convert.ToInt32(editArgs[1]) : (int?)null;//topics from anonymous users have no author id in the command args;

                if ((ParentControl.CurrentUser.IsSuperAdministrator())
                  || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, this.ForumId.Value))
                  || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, this.ForumId.Value))
                  || (topicAuthorId.HasValue && ParentControl.CurrentUser.AdapterId == topicAuthorId))
                {
                    if (ForumTopicInfo.Exists(topicId))
                    {
                        LoadForumTopicAddEditEventArgs args = new LoadForumTopicAddEditEventArgs(this.ForumId, topicId);
                        args.CurrentPage = gvForumTopics.PageIndex + 1;
                        ParentControl.OnLoadForumTopicAddEditEvent(sender, args);
                    }
                    else
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingTopic").ToString(), true));
                        return;
                    }
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToEditTopic").ToString(), true));
                    return;
                }
            }
            else
            {
                string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString();
                Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
            }
        }

        /// <summary>
        /// Delete forum topic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeleteForumTopic_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if ((ParentControl.CurrentUser.IsSuperAdministrator())
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, this.ForumId.Value))
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, this.ForumId.Value)))
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    int topicId = Convert.ToInt32(args[0]);
                    string topicTitle = args[1];
                    try
                    {
                        ForumTopicInfo.Delete(topicId);
                    }
                    catch
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumTopicDelete").ToString(), topicTitle), false));
                        return;
                    }

                    LoadForumTopicListEventArgs eventArgs = new LoadForumTopicListEventArgs(ForumId);

                    if (gvForumTopics.Rows.Count > 1)
                    {
                        eventArgs.CurrentPage = gvForumTopics.PageIndex + 1;
                    }
                    else
                    {
                        eventArgs.CurrentPage = (gvForumTopics.PageIndex > 0) ? gvForumTopics.PageIndex : 1;
                    }

                    eventArgs.ForumId = this.ForumId;
                    eventArgs.ForumName = this.ForumName;
                    eventArgs.Message = String.Format(GetLocalResourceObject("MessageSuccessfulForumTopicDelete").ToString(), topicTitle);
                    ParentControl.OnLoadForumTopicListEvent(sender, eventArgs);
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToDeleteTopic").ToString(), true));
                    return;
                }
            }
            else
            {
                string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString();
                Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
            }
        }


        /// <summary>
        /// Register visit for the topic from which is the post and open the post by raising 
        /// event LoadForumPostListEvent of parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnOpenPost_Command(object sender, CommandEventArgs e)
        {
            //Get details of the last topic post which is tried to be opened
            string[] selectedPostDetails = e.CommandArgument.ToString().Split(';');
            int topicId = Convert.ToInt32(selectedPostDetails[0]);
            string topicName = selectedPostDetails[1];

			//nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + this.ForumId.ToString() + ";topicId:" + topicId.ToString());

            //Go to post
            LoadForumPostListEventArgs objOpenForumTopicEventArgs = new LoadForumPostListEventArgs();
            objOpenForumTopicEventArgs.ForumId = this.ForumId;
            objOpenForumTopicEventArgs.ForumName = this.ForumName;
            objOpenForumTopicEventArgs.ForumTopicId = topicId;
            objOpenForumTopicEventArgs.ForumTopicName = topicName;
            ParentControl.OnLoadForumPostListEvent(sender, objOpenForumTopicEventArgs);
        }

        /// <summary>
        /// Raises LoadForumUserDetailsEvent event of parent control if there is currently logged user.
        /// Otherwise redirect to login page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cntrlForumUserNickname_LoadForumUserProfileEvent(object sender, CommandEventArgs e)
        {
            string username = Convert.ToString(e.CommandArgument);
            if (ParentControl.CurrentUser == null)
            {
                string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString() + "&mode=profile&username=" + username;
                Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
            }
            else
            {
                nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + this.ForumId.ToString() + ";mode:profile;username:" + username);
                ParentControl.OnLoadForumUserDetailsEvent(sender, new LoadForumUserDetailEventArgs(username));
            }
        }


        /// <summary>
        /// Check whether the currently logged user could Edit topic.
        /// He has this right in case he is Super Administrator or Administrator of the forum, 
        /// or Moderator of the forum or author of the topic.
        /// </summary>
        /// <param name="topicAuthorId"></param>
        /// <param name="forumId"></param>
        /// <returns>true/false</returns>
        protected bool CouldEditTopic(int? topicAuthorId, int forumId)
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
                    if (ParentControl.CurrentUser.AdapterId == topicAuthorId)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check whether the currently logged user could Delete topic.
        /// He has this right in case he is Super Administrator or Administrator of the forum, 
        /// or Moderator of the forum.
        /// </summary>
        /// <param name="forumId"></param>
        /// <returns>true/false</returns>
        protected bool CouldDeleteTopic(int forumId)
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
        /// Check whether the currently logged user could could see topic.
        /// In case the topic is inactive the topic is visible only for 
        /// Super Administrators, Administrators or Moderators of the forum. 
        /// </summary>
        /// <param name="isTopicActive"></param>
        /// <param name="forumId"></param>
        /// <returns></returns>
        protected bool CouldSeeTopic(bool isTopicActive, int forumId)
        {
            bool result = false;
            if (isTopicActive)
            {
                result = true;
            }
            else
            {
                if (ParentControl.CurrentUser != null)
                {
                    if ((ParentControl.CurrentUser.IsSuperAdministrator())
                        || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, forumId))
                        || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, forumId)))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Return icon for a specific topic type.
        /// </summary>
        /// <param name="topicType"></param>
        /// <returns></returns>
        protected string GetTopicIcon(ForumTopicType topicType)
        {
            string icon = "";
            switch (topicType)
            {
                case ForumTopicType.Announcement:
                    icon = "ForumStyles/Images/announcement_topic.gif";
                    break;
                case ForumTopicType.Sticky:
                    icon = "ForumStyles/Images/sticky_topic.gif";
                    break;
                case ForumTopicType.Normal:
                    icon = "ForumStyles/Images/normal_topic.gif";
                    break;
                default:
                    icon = "ForumStyles/Images/normal_topic.gif";
                    break;
            }

            return icon;
        }

        /// <summary>
        /// Return tooltip for a specific topic type.
        /// </summary>
        /// <param name="topicType"></param>
        /// <returns></returns>
        protected string GetTopicIconTooltip(ForumTopicType topicType)
        {
            string tooltip = "";
            switch (topicType)
            {
                case ForumTopicType.Announcement:
                    tooltip = GetLocalResourceObject("AnnouncementTopic").ToString();
                    break;
                case ForumTopicType.Sticky:
                    tooltip = GetLocalResourceObject("StickyTopic").ToString();
                    break;
                case ForumTopicType.Normal:
                    tooltip = GetLocalResourceObject("NormalTopic").ToString();
                    break;
                default:
                    tooltip = GetLocalResourceObject("NormalTopic").ToString();
                    break;
            }

            return tooltip;
        }


        /// <summary>
        /// Find all topics for forum specified by id and display their details in DataGrid: dgForumTopics.
        /// </summary>
        /// <param name="forumId"></param>
        private void ListForumTopics(int? forumId)
        {
            ForumTopicView objForumTopicView = new ForumTopicView();
            objForumTopicView.ForumId = forumId;
            DataTable dtForumTopics = ForumTopicView.List(objForumTopicView);

            gvForumTopics.PageSize = ForumSettings.TopicsPageSize;


            gvForumTopics.DataSource = dtForumTopics;
            gvForumTopics.DataBind();

            if (this.CurrentPage != null && this.CurrentPage <= this.gvForumTopics.PageCount)
            {
                this.gvForumTopics.PageIndex = this.CurrentPage.Value - 1;
                this.CurrentPage = null;
                this.gvForumTopics.DataBind();
            }

            if (dtForumTopics.Rows.Count != 0)
            {
                tblEmptyDataTemplate.Visible = false;
				TopPager.FillPaging(gvForumTopics.PageCount, gvForumTopics.PageIndex + 1, 5, gvForumTopics.PageSize, dtForumTopics.Rows.Count, "forumId:" + ForumId.ToString());
				BottomPager.FillPaging(gvForumTopics.PageCount, gvForumTopics.PageIndex + 1, 5, gvForumTopics.PageSize, dtForumTopics.Rows.Count, "forumId:" + ForumId.ToString());
            }
            else
            {
                tblEmptyDataTemplate.Visible = true;
                TopPager.Visible = false;
                BottomPager.Visible = false;
            }
        }
    }
}