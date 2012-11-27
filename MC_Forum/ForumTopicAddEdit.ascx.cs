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

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for creating new forum topic or updating existing one.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     The ForumTopicAddEdit user control provides user interface for 
    ///     creating or updating forum topics characterized with title, type, description, and some forum topic flags.
    ///     The type and the flags settings are visible only for administrators and moderators of the forum to which belongs the topic.
    ///     If field <see cref="TopicId"/> contains forum topic id then the control is in context of modifying existing forum topic. 
    ///     If TopicId is not set then new forum topic will be created and it will be created in forum with id <see cref="ForumId"/>.
    ///     The topic types are three: Announcement, Sticky and Normal. The topics appear in the forum topics list in the following order: 
    ///     announcement, sticky, normal topics.
    ///     The control is using <see cref="Melon.Components.Forum.ForumTopic"/> and <see cref="Melon.Components.Forum.ForumTopicInfo"/> classes for saving the forum settings.  
    ///     </para>
    ///     <para>
    ///     The following web controls build this user control:
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Web Control</term><description>Description</description>
    ///         </listheader>
    ///         <item><term>TextBox txtTopicTitle</term><description>Title of forum topic.</description></item>
    ///         <item><term>TextBox txtTopicDescription</term><description>Description of forum topic.</description></item>
    ///         <item><term>ddlTopicTypes</term><description>DropDownList with the available forum topic types: Announcement, Sticky, Normal.</description></item>
    ///         <item><term>CheckBox chkIsActive</term><description>Checked if the forum topic is activated and is visible for the users.</description></item>
    ///         <item><term>CheckBox chkIsClosed</term><description>Checked if the forum topic is closed. Forum users could not post in closed topics but they could browse them.</description></item>
    ///         <item><term>Button btnPostTopic</term><description>Saves the forum topic settings and close ForumTopicAddEdit control.</description></item>
    ///         <item><term>Button btnCancel</term><description>Close ForumTopicAddEdit control without saving the forum topic settings.</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///     Required forum setting is the title so there is RequiredFieldValidator control for txtForumName. 
    ///     </para>
    ///     <para>
    ///     All web controls from ForumAddEdit are using the local resources.
    ///     To customize them modify resource file ForumAddEdit.resx placed in the MC_Forum folder.
    ///     </para>
    ///</remarks>
    ///<seealso cref="Melon.Components.Forum.ForumTopic"/>
    ///<seealso cref="Melon.Components.Forum.ForumTopicInfo"/>
    ///<seealso cref="Melon.Components.Forum.ForumTopicType"/>
    public partial class ForumTopicAddEdit : ForumControl
    {
        /// <summary>
        /// Id of forum to which will be created the new topic.
        /// </summary>
        public int? ForumId;

        /// <summary>
        /// Id of the forum topic which will be modified.
        /// </summary>
        public int? TopicId;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.ForumId = (int?)args[0];
            this.TopicId = (int?)args[1];
            this.CurrentPage = (int?)args[2];
        }

        /// <summary>
        /// Attach event handlers to controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnPostTopic.Click += new EventHandler(btnPostTopic_Click);
            base.OnInit(e);
        }

        /// <summary>
        /// Loads forum topic details if topic is selected for modification (TopicId is not null).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsControlPostBack)
            {
                DisplayAllowedTopicSetting();
                if (TopicId != null)
                {
                    LoadTopicDetails(TopicId.Value);
                }

                this.btnPostTopic.Attributes.Add("onfocus", "document.getElementById('" + this.txtTopicTitle.ClientID + "').focus();");
				ScriptManager.GetCurrent(this.Page).SetFocus(this.btnPostTopic);
            }
        }

        /// <summary>
        /// Creates new or update existing topic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPostTopic_Click(object sender, EventArgs e)
        {
            Melon.Components.Forum.Forum objForum = ForumInfo.Load(this.ForumId.Value);
            if (objForum != null)
            {
                ForumTopic objForumTopic = null;
                if (TopicId == null)
                {
                    //*** Create topic ***

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
                        objForumTopic = new ForumTopic();

                        objForumTopic.ForumId = ForumId;
                        if (ParentControl.CurrentUser != null)
                        {
                            objForumTopic.ForumUserId = ParentControl.CurrentUser.AdapterId;
                        }
                    }
                }
                else
                {
                    //*** Update topic ***
                    if (ParentControl.CurrentUser != null)
                    {
                        objForumTopic = ForumTopicInfo.Load(TopicId.Value);
                        if (objForumTopic == null)
                        {
                            ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingTopic").ToString(), true));
                            return;
                        }
                        else
                        {
                            if (!((ParentControl.CurrentUser.IsSuperAdministrator())
                              || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, this.ForumId.Value))
                              || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, this.ForumId.Value))
                              || (ParentControl.CurrentUser.AdapterId == objForumTopic.ForumUserId)))
                            {
                                ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToEditTopic").ToString(), true));
                                return;
                            }
                        }
                    }
                    else
                    {
                        string url = Request.Url.AbsolutePath + "?forumId=" + this.ForumId.ToString();
                        Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
                    }
                }

                objForumTopic.Name = txtTopicTitle.Text.Trim();
                if (txtTopicDescription.Text.Trim() != string.Empty)
                {
                    objForumTopic.Description = HttpUtility.HtmlDecode(txtTopicDescription.Text.Trim());
                }
                objForumTopic.ForumTopicTypeId = Convert.ToInt32(ddlTopicTypes.SelectedValue);
                objForumTopic.IsActive = chkIsActive.Checked;
                objForumTopic.IsClosed = chkIsClosed.Checked;

                int savedTopicId;
                try
                {
                    savedTopicId = ForumTopicInfo.Save(objForumTopic);
                }
                catch
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMesageTopicPost").ToString(), false));
                    return;
                }

                //If topic is created successfully we go to the page with the topic posts
                if (TopicId == null)
                {
                    // KEY POINT: After creating topic the list of posts should open, but we have to show the new topic in the url
                    // The only way this to happen (in pre-AJAX version) is to use response redirect).
                    // Comment the line below in AJAX version and uncomment the next.
                    // Response.Redirect(Utilities.GeneratePostBackUrl(Request.Url.PathAndQuery, "topicId=" + savedTopicId.ToString(), false));

                    LoadForumPostListEventArgs args = new LoadForumPostListEventArgs();
                    args.ForumId = objForum.Id;
                    args.ForumName = objForum.Name;
                    args.ForumTopicId = savedTopicId;
                    args.ForumTopicName = objForumTopic.Name;

                    ParentControl.OnLoadForumPostListEvent(sender, args);
                }
                else
                {
                    //If topic is updated successfully we list topics again to update the list with the  changes
                    LoadForumTopicListEventArgs args = new LoadForumTopicListEventArgs(objForumTopic.ForumId, objForumTopic.ForumName);
                    args.CurrentPage = this.CurrentPage;
                    ParentControl.OnLoadForumTopicListEvent(sender, args);
                }
            }
            else
            {
                ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("NotExistingForum").ToString(), false));
                ParentControl.OnLoadForumListEvent(sender, new LoadForumListEventArgs());
            }
        }

        /// <summary>
        /// Closes user control for add/edit forum topic (the current control).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ParentControl.OnRemoveForumControlEvent(sender, new RemoveForumControlEventArgs("ForumTopicAddEdit.ascx"));
        }


        /// <summary>
        /// Displays topic details accessible for setting from the current logged user role.
        /// </summary>
        private void DisplayAllowedTopicSetting()
        {
            if (ParentControl.CurrentUser != null)
            {
                // If the current logged user is SuperAdministrator, Administrator or Moderator of the selected forum
                // he could set topic type and statuses (Is Active, Is Closed)

                if ((ParentControl.CurrentUser.IsSuperAdministrator())
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, ForumId.Value))
                   || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Moderator, ForumId.Value)))
                {
                    trTopicActions.Style.Add("display", "");
                    trTopicType.Style.Add("display", "");
                }
                else
                {
                    trTopicActions.Style.Add("display", "none");
                    trTopicType.Style.Add("display", "none");
                }
            }
            else
            {
                trTopicActions.Style.Add("display", "none");
                trTopicType.Style.Add("display", "none");
            }

        }

        /// <summary>
        /// This method is called when topic is selected for edit. 
        /// The details of the topic are retrieved from the database and displayed.
        /// </summary>
        private void LoadTopicDetails(int topicId)
        {
            ForumTopic objForumTopic = ForumTopicInfo.Load(topicId);
            if (objForumTopic != null)
            {
                txtTopicTitle.Text = objForumTopic.Name;
                txtTopicDescription.Text = objForumTopic.Description;

                if (objForumTopic.ForumTopicTypeId != null)
                {
                    ddlTopicTypes.SelectedIndex = objForumTopic.ForumTopicTypeId.Value - 1;
                }

                chkIsActive.Checked = objForumTopic.IsActive.Value;
                chkIsClosed.Checked = objForumTopic.IsClosed.Value;
            }
        }
    }
}