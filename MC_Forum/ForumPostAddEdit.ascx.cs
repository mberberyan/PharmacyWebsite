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
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.Exception;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for creating new forum post or updating existing one.
    /// </summary>
    /// <remarks>
    ///     <para>The ForumPostAddEdit user control provides user interface for 
    ///     creating or updating forum posts characterized only with text.
    ///     If field <see cref="PostId"/> contains forum post id then the control is in context of modifying existing forum post. 
    ///     If PostId is not set then new forum post will be created. The new post is created to the topic with id set in property <see cref="TopicId"/>.
    ///     The control is using <see cref="Melon.Components.Forum.ForumPost"/> and <see cref="Melon.Components.Forum.ForumPostInfo"/> classes for saving the forum post settings.  
    ///     </para>
    ///     <para>The following web controls build this user control:
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Web Control</term><description>Description</description>
    ///             </listheader>
    ///             <item><term>TextBox txtPostText</term><description>Text of forum post.</description></item>
    ///             <item><term>Button btnPost</term><description>Saves the forum post settings and closes ForumPostAddEdit control.</description></item>
    ///             <item><term>Button btnCancel</term><description>Closes ForumPostAddEdit control without saving the forum post settings.</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///     Required forum post setting is the text so there is RequiredFieldValidator control for txtPostText. 
    ///     </para>
    ///     <para>
    ///     All web controls from ForumPostAddEdit are using the local resources.
    ///     To customize them modify resource file ForumPostAddEdit.resx placed in the MC_Forum folder.
    ///     </para>
    ///</remarks>
    ///<seealso cref="Melon.Components.Forum.ForumPost"/>
    ///<seealso cref="Melon.Components.Forum.ForumPostInfo"/>
    public partial class ForumPostAddEdit : ForumControl
    {
		/// <summary>
		/// The forum id being edited. Null if adding new one.
		/// </summary>
		public int? ForumId
		{
			get
			{
				return (ViewState["__mc_forumpostaddedit"] == null) ? (int?)null : int.Parse(ViewState["__mc_forumpostaddedit"].ToString());
			}
			set
			{
				ViewState["__mc_forumpostaddedit"] = value;
			}
		}

        /// <summary>
        /// Id of the forum topic to which will be created the new post.
        /// </summary>
        public int? TopicId;

        /// <summary>
        /// The id of the forum post which will be modified.
        /// </summary>
        public int? PostId;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.ForumId = (int?)args[0];
            this.TopicId = (int?)args[1];
            this.PostId = (int?)args[2];
            this.CurrentPage = (int?)args[3];
        }

        /// <summary>
        /// Attach event handlers to controls.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.btnPost.Click += new EventHandler(btnPost_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
        }

        /// <summary>
        /// Loads forum posts details if forum post is selected for modification (PostId is not null).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsControlPostBack)
            {
                if (PostId != null)
                {
                    LoadPostDetails(PostId.Value);
                }

                this.btnPost.Attributes.Add("onfocus", "document.getElementById('" + this.txtPostText.ClientID + "').focus();");
				ScriptManager.GetCurrent(this.Page).SetFocus(this.btnPost);
            }
        }

        /// <summary>
        /// Creates new or update existing post.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPost_Click(object sender, EventArgs e)
        {
            ForumTopic objForumTopic = null;
            Melon.Components.Forum.Forum objForum = ForumInfo.Load(this.ForumId.Value);
            if (objForum == null)
            {
                ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToCreatePostInNotExistingForum").ToString(), false));
                ParentControl.OnLoadForumListEvent(sender, new LoadForumListEventArgs());
                return;
            }
            else
            {
                objForumTopic = ForumTopicInfo.Load(this.TopicId.Value);
                if (objForumTopic == null)
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToCreatePostInNotExistingTopic").ToString(), false));
                    LoadForumTopicListEventArgs args = new LoadForumTopicListEventArgs();
                    args.ForumId = this.ForumId;
                    args.ForumName = objForum.Name;
                    ParentControl.OnLoadForumTopicListEvent(sender, args);
                    return;
                }
            }

            ForumPost objForumPost = null;
            if (PostId == null)
            {
                //*** Create post ***

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

                objForumPost = new ForumPost();
                objForumPost.ForumTopicId = TopicId;
                if (ParentControl.CurrentUser != null)
                {
                    objForumPost.ForumUserId = ParentControl.CurrentUser.AdapterId;
                }

            }
            else
            {
                //*** Update post ***

                if (ParentControl.CurrentUser == null)
                {
                    string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString() + "&topicId=" + this.TopicId.ToString();
                    Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
                }
                else
                {
                    objForumPost = ForumPostInfo.Load(PostId.Value);
                    if (objForumPost == null)
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingPost").ToString(), true));
                        return;
                    }
                    else
                    {
                        if (!((ParentControl.CurrentUser.IsSuperAdministrator())
                              || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, this.ForumId.Value))
                              || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, this.ForumId.Value))
                              || (ParentControl.CurrentUser.AdapterId == objForumPost.ForumUserId)))
                        {
                            ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToEditPost").ToString(), true));
                            return;
                        }
                    }
                }
            }

            objForumPost.Text = HttpUtility.HtmlDecode(txtPostText.Text.Trim());

            int? savedPostId = null;
            try
            {
                savedPostId = ForumPostInfo.Save(objForumPost);
            }
            catch
            {
                ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMesagePost").ToString(),false));
                return;
            }

            LoadForumPostListEventArgs argsLoadForumPostList = new LoadForumPostListEventArgs();
            argsLoadForumPostList.ForumId = this.ForumId;
            argsLoadForumPostList.ForumName = objForum.Name;
            argsLoadForumPostList.ForumTopicId = this.TopicId;
            argsLoadForumPostList.ForumTopicName = objForumTopic.Name;
            argsLoadForumPostList.PostId = savedPostId;
            argsLoadForumPostList.CurrentPage = this.CurrentPage;

            ParentControl.OnLoadForumPostListEvent(this, argsLoadForumPostList);
        }

        /// <summary>
        /// Closes user control for add/edit forum post (the current control).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ParentControl.OnRemoveForumControlEvent(sender, new RemoveForumControlEventArgs("ForumPostAddEdit.ascx"));
        }

        /// <summary>
        /// Retrives from database details for the specified by id forum post and display them in the interface.
        /// </summary>
        /// <param name="postId"></param>
        private void LoadPostDetails(int postId)
        {
            ForumPost objForumPost = ForumPostInfo.Load(postId);
            if (objForumPost != null)
            {
                txtPostText.Text = objForumPost.Text;
            }
        }
    }
}
