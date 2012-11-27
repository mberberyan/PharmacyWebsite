using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.UI.Controls;

//Code-behind partial classes of the user controls used to form the interface of the forum component.
namespace Melon.Components.Forum.UI.CodeBehind
{
	/// <summary>
	/// Contains the layout and functionality for listing the forums and forum groups.
	/// </summary>
	/// <remarks>
	///     <para>
	///		The control represents the list of topics in a chosen forum. The list is displayed in a table, containing the following columns:
	///		<list type="bullet">
	///			<item>The forum title.</item>
	///			<item>The number of topics in this forum.</item>
	///			<item>The number of posts in this forum.</item>
	///			<item>Last post information (date and author) and a link to the last post.</item>
	///		</list>
	///		Additionally the forums in this table are grouped into forum groups, i.e. certain rows of the table contain only one cell (table-wide), which has as a text the name of a forum group. Below that row follow all the forums belonging to that group.
	///		The control may also contain a button for adding new topic (depending on the current forum's settings and current user rights).
	///		</para>
	///		<para> </para>
	/// 	<para>
	/// 	The following web controls build this control:
	///			<list type="bullet">
	///				<item><term>Label lblMessage:</term><description> used to display information messages (e.g. topic was deleted after administrator actually deleted a topic).</description></item>
	///				<item><term>Label lblErrorMessage:</term><description> used to display error messages (e.g. something went wrong during accessing database).</description></item>
	///  			<item><term>Repeater repForumGroups:</term> used to display the forum groups.<description></description></item>
	///			</list>
	/// 	</para>
	///		<para> </para>
	///		<para>
	///		Remarks on the Repeater repForumGroups:
	///			<list type="table">
	///				<item><term>Header texts</term><description>These are retrieved from the associated resource file. The keys used are lblForumTitle, lblTopicsTitle, lblPostsTitle, lblLastPostTitle.</description></item>
	///				<item>
	///					<term>Item template</term>
	///					<description>This consists of the following web controls:
	///						<list type="bullet">
	///							<item><term>Label lblForumGroupName: </term><description>used to display the name of the current forum group.</description></item>
	///							<item><term>Image imgIsActiveGroup: </term><description>used to indicate if the forum group is inactive. If the group is active this is not displayed. Valid only for super administrators. Other users do not see inactive groups and their forums.</description></item>
    ///							<item><term>ImageButton ibtnEditForumGroup: </term><description>allows editing of the selected group (super administrators only).</description></item>
    ///							<item><term>ImageButton ibtnDeleteForumGroup: </term><description>allows deletion of the selected group (super administrators only).</description></item>
    ///							<item><term>ImageButton ibtnMoveUpForumGroup: </term><description>allows moving up by one position of the selected group (super administrators only).</description></item>
    ///							<item><term>ImageButton ibtnMoveDownForumGroup: </term><description>allows moving down by one position of the selected group (super administrators only).</description></item>
    ///							<item><term>ImageButton ibtnCreateForum: </term><description>allows creation of a forum in the selected group (super administrators only).</description></item>
	///							<item><term>Repeater repForums: </term><description>used to display the forums.</description></item>
	///						</list>
	///					</description>
	///				</item>
    ///				<item><term>Button btnCreateForumGroup</term><description>Allows creating a new forum group. Accessible by super administrators only.</description></item>
	///			</list>
	///		</para>
	///		<para> </para>
	///		<para>
	///		Remarks on the Repeater repForums:
	///			<list type="table">
	///				<item><term>HtmlTableRow trForumDetails</term><description>Used to show/hide forums depending their settings or their group settings.</description></item>
	///				<item><term>LinkButton lbtnOpenForum</term><description>Allows navigation to forum topics.</description></item>
	///				<item><term>Image imgClosed</term><description>Displayed if the forum is closed.</description></item>
    ///				<item><term>Image imgIsActiveForum</term><description>Displayed if the forum is not active(super administrators only). If the logged user is not super administrator and the forum is not active, the entire table row is not shown.</description></item>
	///				<item><term>Label lblForumDescription</term><description>Displays the forum description text.</description></item>
    ///				<item><term>Div divForumButtons</term><description>Defines the area for edit/delete buttons. Visible for super administrators only.</description></item>
	///				<item><term>LinkButton lbtnEditForum</term><description>Allows editing of the selected forum.</description></item>
	///				<item><term>LinkButton lbtnDeleteForum</term><description>Allows deletion of the selected forum.</description></item>
	///				<item><term>Label lblTopicsCount</term><description>Displays the number of topics within the forum.</description></item>
	///				<item><term>Label lblPostsCount</term><description>Displays the number of posts within the forum.</description></item>
	///				<item><term>Div divLastPost</term><description>Defines the last post area. Not visible if the forum has no posts.</description></item>
	///				<item><term>Last post date</term><description>Displayed as a text.</description></item>
	///				<item><term>ImageButton ibtnOpenPost</term><description>Used to navigate to the last post.</description></item>
	///				<item><term>Label lblAuthorTitle</term><description>Displays the text from the resource file, key lblAuthorTitle.</description></item>
	///				<item><term><see cref="Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay">ForumUserNicknameDisplay</see> cntrlForumUserNickname</term><description>Used to display the author nickname in simple text or as a link depending on the authors profile settings.</description></item>
	///				<item><term>Label lblAuthorRole</term><description>Displays the top role of the author of the post. Displayed only if the author is administrator or moderator.</description></item>
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
    public partial class ForumList:ForumControl
    {
        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.Message = (string)args[0];
        }

        /// <summary>
        /// Attach event handlers.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            repForumGroups.ItemCreated += new RepeaterItemEventHandler(repForumGroups_ItemCreated);
            repForumGroups.ItemDataBound+=new RepeaterItemEventHandler(repForumGroups_ItemDataBound);

            base.OnInit(e);
        }


        /// <summary>
        /// Initially loads all possible (according to logged user and settings) forum groups and their forums.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (!this.IsControlPostBack)
            {
                if (Message != null)
                {
                    lblMessage.Text = Message;
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Visible = false;
                }
                ListForums();
            }
        }


        /// <summary>
        /// Attach event handlers of controls in the repeater.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repForumGroups_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || ((e.Item.ItemType == ListItemType.AlternatingItem)))
            {
                Repeater repForums = (Repeater)e.Item.FindControl("repForums");
                repForums.ItemCreated += new RepeaterItemEventHandler(repForums_ItemCreated);
                repForums.ItemDataBound += new RepeaterItemEventHandler(repForums_ItemDataBound);

                HtmlGenericControl divForumGroupAdminButtons = (HtmlGenericControl)e.Item.FindControl("divForumGroupAdminButtons");

                ImageButton ibtnEditForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnEditForumGroup");
                ibtnEditForumGroup.Command += new CommandEventHandler(ibtnEditForumGroup_Command);

                ImageButton ibtnDeleteForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnDeleteForumGroup");
                ibtnDeleteForumGroup.Command += new CommandEventHandler(ibtnDeleteForumGroup_Command);

                ImageButton ibtnMoveUpForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnMoveUpForumGroup");
                ibtnMoveUpForumGroup.Command += new CommandEventHandler(ibtnMoveUpForumGroup_Command);

                ImageButton ibtnMoveDownForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnMoveDownForumGroup");
                ibtnMoveDownForumGroup.Command += new CommandEventHandler(ibtnMoveDownForumGroup_Command);

                ImageButton ibtnCreateForum = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnCreateForum");
                ibtnCreateForum.Command += new CommandEventHandler(ibtnCreateForum_Command);
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Button btnCreateForumGroup = (Button)e.Item.FindControl("btnCreateForumGroup");
                if (IsCurrentUserSuperAdmin())
                {
                    btnCreateForumGroup.Click += new EventHandler(btnCreateForumGroup_Click);
                    btnCreateForumGroup.Visible = true;
                }
                else
                {
                    btnCreateForumGroup.Visible = false;
                }
            }
        }
     
        /// <summary>
        /// Data bind forum groups.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repForumGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || ((e.Item.ItemType == ListItemType.AlternatingItem)))
            {
                KeyValuePair<object, List<ForumView>> forumlist = (KeyValuePair<object, List<ForumView>>)e.Item.DataItem;

                //Set forum group details.
                Label lblForumGroupName = (Label)e.Item.FindControl("lblForumGroupName");
                lblForumGroupName.Text = Server.HtmlEncode(forumlist.Value[0].ForumGroupName);

                Image imgIsActiveGroup = (Image)e.Item.FindControl("imgIsActiveGroup");
                imgIsActiveGroup.Visible = IsCurrentUserSuperAdmin() && !forumlist.Value[0].ForumGroupIsActive.Value;

                //Set properties of action buttons for forum group.
                HtmlGenericControl divForumGroupAdminButtons = (HtmlGenericControl)e.Item.FindControl("divForumGroupAdminButtons");
                if (IsCurrentUserSuperAdmin())
                {
                    divForumGroupAdminButtons.Style.Add("display", "inline");

                    ImageButton ibtnEditForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnEditForumGroup");
                    ibtnEditForumGroup.CommandArgument = forumlist.Value[0].ForumGroupId.ToString() + ";" + forumlist.Value[0].ForumGroupName;

                    ImageButton ibtnDeleteForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnDeleteForumGroup");
                    ibtnDeleteForumGroup.CommandArgument = Convert.ToString(forumlist.Value[0].ForumGroupId) + ";" + forumlist.Value[0].ForumGroupName;

                    ImageButton ibtnMoveUpForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnMoveUpForumGroup");
                    ibtnMoveUpForumGroup.CommandArgument = Convert.ToString(forumlist.Value[0].ForumGroupId) + ";" + forumlist.Value[0].ForumGroupName;

                    ImageButton ibtnMoveDownForumGroup = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnMoveDownForumGroup");
                    ibtnMoveDownForumGroup.CommandArgument = Convert.ToString(forumlist.Value[0].ForumGroupId) + ";" + forumlist.Value[0].ForumGroupName;

                    ImageButton ibtnCreateForum = (ImageButton)divForumGroupAdminButtons.FindControl("ibtnCreateForum");
                    ibtnCreateForum.CommandArgument = Convert.ToString(forumlist.Value[0].ForumGroupId) + ";" + forumlist.Value[0].ForumGroupName;
                }
                else
                {
                    divForumGroupAdminButtons.Style.Add("display", "none");
                }

                //Check whether the current forum group is empty
                if (!((forumlist.Value.Count == 1) && (forumlist.Value[0].Id == null)))
                {
                    //The forum group has forums.It is not empty
                    //Data bind the repeater for the forums of the current forum group 
                    Repeater repForums = (Repeater)e.Item.FindControl("repForums");
                    repForums.DataSource = forumlist.Value;
                    repForums.DataBind();
                }
              
            }
        }


        /// <summary>
        /// Attach events of the controls in repeater repForum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repForums_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || ((e.Item.ItemType == ListItemType.AlternatingItem)))
            {
                HtmlTableRow trForumDetails = (HtmlTableRow)e.Item.FindControl("trForumDetails");

				MelonLinkButton lbtnOpenForum = (MelonLinkButton)trForumDetails.FindControl("lbtnOpenForum");
                lbtnOpenForum.Command += new CommandEventHandler(lbtnOpenForum_Command);

                LinkButton lbtnEditForum = (LinkButton)trForumDetails.FindControl("lbtnEditForum");
                lbtnEditForum.Command += new CommandEventHandler(lbtnEditForum_Command);

                LinkButton lbtnDeleteForum = (LinkButton)trForumDetails.FindControl("lbtnDeleteForum");
                lbtnDeleteForum.Command += new CommandEventHandler(lbtnDeleteForum_Command);

                ImageButton ibtnOpenPost = (ImageButton)trForumDetails.FindControl("ibtnOpenPost");
                ibtnOpenPost.Command += new CommandEventHandler(ibtnOpenPost_Command);

                Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlForumUserNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)trForumDetails.FindControl("cntrlForumUserNickname");
                cntrlForumUserNickname.LoadForumUserProfileEvent+=new Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay.LoadForumUserProfileEventHandler(cntrlForumUserNickname_LoadForumUserProfileEvent);
            }
        }

        /// <summary>
        /// Used to set visibility and formatting of various items within a forum table row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repForums_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || ((e.Item.ItemType == ListItemType.AlternatingItem)))
            {
                ForumView objForumView = (ForumView)e.Item.DataItem;

                //Check whether the forum is visible or not for the logged user.
                HtmlTableRow trForumDetails = (HtmlTableRow)e.Item.FindControl("trForumDetails");
                if (objForumView.Id != null)
                {
                    if (CheckEditDeleteRights(objForumView.Id.Value) || objForumView.IsActive.Value)
                    {
                        //Visible for the current logged user.
                        trForumDetails.Style.Remove("display");

                        //Set forum details.
						MelonLinkButton lbtnOpenForum = (MelonLinkButton)trForumDetails.FindControl("lbtnOpenForum");
                        lbtnOpenForum.Text = Server.HtmlEncode(objForumView.Name);
						lbtnOpenForum.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?forumId=" + Convert.ToString(objForumView.Id);
                        lbtnOpenForum.CommandArgument = Convert.ToString(objForumView.Id) + ";" + objForumView.Name;
                        
                        Image imgClosed = (Image)trForumDetails.FindControl("imgClosed");
                        imgClosed.Visible = objForumView.IsClosed.Value;

                        Image imgIsActiveForum = (Image)trForumDetails.FindControl("imgIsActiveForum");
                        imgIsActiveForum.Visible = CheckEditDeleteRights(objForumView.Id.Value) && !objForumView.IsActive.Value;

                        Label lblForumDescription = (Label)trForumDetails.FindControl("lblForumDescription");
                        lblForumDescription.Text = Server.HtmlEncode(objForumView.Description).Replace("\r\n", "<br/>");

                        HtmlGenericControl divForumButtons = (HtmlGenericControl)trForumDetails.FindControl("divForumButtons");
                        if (CheckEditDeleteRights(objForumView.Id.Value))
                        {
                            divForumButtons.Style.Add("display", "inline");
                            LinkButton lbtnEditForum = (LinkButton)divForumButtons.FindControl("lbtnEditForum");
                            lbtnEditForum.CommandArgument = Convert.ToString(objForumView.Id) + ";" + objForumView.Name + ";" + objForumView.ForumGroupName;

                            LinkButton lbtnDeleteForum = (LinkButton)divForumButtons.FindControl("lbtnDeleteForum");
                            lbtnDeleteForum.CommandArgument = Convert.ToString(objForumView.Id) + ";" + objForumView.Name + ";" + objForumView.ForumGroupName;
                        }
                        else
                        {
                            divForumButtons.Style.Add("display", "none");
                        }

                        Label lblTopicsCount = (Label)trForumDetails.FindControl("lblTopicsCount");
                        lblTopicsCount.Text = Convert.ToString(objForumView.TopicsCount);

                        Label lblPostsCount = (Label)trForumDetails.FindControl("lblPostsCount");
                        lblPostsCount.Text = Convert.ToString(objForumView.PostsCount.Value);

                        HtmlGenericControl divLastPost = (HtmlGenericControl)trForumDetails.FindControl("divLastPost");
                        if (objForumView.LastPostId != null)
                        {
                            divLastPost.Style.Add("display", "inline");

                            ImageButton ibtnOpenPost = (ImageButton)divLastPost.FindControl("ibtnOpenPost");
                            ibtnOpenPost.CommandArgument = Convert.ToString(objForumView.Id) + ";" + objForumView.Name + ";" + Convert.ToString(objForumView.LastPostTopicId) + ";" + objForumView.LastPostTopicName;
                          
                            if (objForumView.LastPostAuthorId != null)
                            {
                                Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay cntrlForumUserNickname = (Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay)divLastPost.FindControl("cntrlForumUserNickname");
                                cntrlForumUserNickname.UserName = objForumView.LastPostAuthorUserName;
                                cntrlForumUserNickname.Nickname = objForumView.LastPostAuthorNickname;
                                cntrlForumUserNickname.IsProfileVisible = objForumView.LastPostAuthorIsProfileVisible.Value;

                                Label lblAuthorRole = (Label)divLastPost.FindControl("lblAuthorRole");
                                if (objForumView.LastPostAuthorTopRole != ForumUserRole.NormalUser)
                                {
                                    lblAuthorRole.Text = "(" + GetLocalResourceObject(objForumView.LastPostAuthorTopRole.ToString()).ToString()  + ")";
                                }
                            }
                        }
                        else
                        {
                            divLastPost.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        //Not visible for the current logged user.
                        trForumDetails.Style.Add("display", "none");
                    }

                }
                else
                {
                    trForumDetails.Style.Add("display", "none");
                }
            }
        }


        /// <summary>
        /// Raise event for load ForumGroupAddEdit in the context of creating new forum group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateForumGroup_Click(object sender, EventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if (ParentControl.CurrentUser.IsSuperAdministrator()) //Only super administrators could create forum groups.
                {
                    ParentControl.OnLoadForumGroupAddEditEvent(sender, new LoadForumGroupAddEditEventArgs());
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToAddForumGroup").ToString(),true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Raise event for load ForumGroupAddEdit in the context of editing existing forum group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnEditForumGroup_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if (ParentControl.CurrentUser.IsSuperAdministrator()) //Only super administrators could edit forum groups.
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    int forumGroupId = Convert.ToInt32(args[0]);
                    string forumGroupName = args[1];

                    if (ForumGroupInfo.Exists(forumGroupId))
                    {
                        ParentControl.OnLoadForumGroupAddEditEvent(sender, new LoadForumGroupAddEditEventArgs(forumGroupId));
                    }
                    else
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumGroupNotExist").ToString(), forumGroupName), true));
                        return;
                    }
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToEditForumGroup").ToString(), true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
		/// Raise event for load ForumList in the context of deleting existing forum group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnDeleteForumGroup_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if (ParentControl.CurrentUser.IsSuperAdministrator()) //Only super administrators could delete forum groups.
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    int forumGroupId = Convert.ToInt32(args[0]);
                    string forumGroupName = args[1];
                    try
                    {
                        ForumGroupInfo.Delete(forumGroupId);
                    }
                    catch
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumGroupDelete").ToString(), forumGroupName), false));
                        return;
                    }

                    LoadForumListEventArgs eventArgs = new LoadForumListEventArgs();
                    eventArgs.Message = String.Format(GetLocalResourceObject("MessageSuccessfulForumGroupDelete").ToString(), forumGroupName);
                    ParentControl.OnLoadForumListEvent(sender, eventArgs);
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToDeleteForumGroup").ToString(), true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Moves a forum group up and forces reload of the forum list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnMoveUpForumGroup_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if (ParentControl.CurrentUser.IsSuperAdministrator()) //Only super administrators could move up forum groups.
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    int forumGroupId = Convert.ToInt32(args[0]);
                    string forumGroupName = args[1];

                    if (ForumGroupInfo.Exists(forumGroupId))
                    {
                        try
                        {
                            ForumGroupInfo.MoveUp(forumGroupId);
                        }
                        catch
                        {
                            ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumGroupMoveUp").ToString(), false));
                            return;
                        }
                        ListForums();
                    }
                    else
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumGroupNotExist").ToString(), forumGroupName), true));
                        return;
                    }
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToMoveUpForumGroup").ToString(), true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Moves a forum group down and forces reload of the forum list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnMoveDownForumGroup_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if (ParentControl.CurrentUser.IsSuperAdministrator())//Only super administrators could move down forum groups.
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    int forumGroupId = Convert.ToInt32(args[0]);
                    string forumGroupName = args[1];

                    if (ForumGroupInfo.Exists(forumGroupId))
                    {
                        try
                        {
                            ForumGroupInfo.MoveDown(forumGroupId);
                        }
                        catch
                        {
                            ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumGroupMoveDown").ToString(), false));
                            return;
                        }

                        ListForums();
                    }
                    else
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumGroupNotExist").ToString(), forumGroupName), true));
                        return;
                    }
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToMoveDownForumGroup").ToString(), true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Raise event of parent control for loading ForumAddEdit in the context of creating forum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnCreateForum_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                if (ParentControl.CurrentUser.IsSuperAdministrator()) // Only super administrators could create forums.
                {
                    string[] args = e.CommandArgument.ToString().Split(';');
                    int forumGroupId = Convert.ToInt32(args[0]);
                    string forumGroupName = args[1];

                    if (ForumGroupInfo.Exists(forumGroupId))
                    {
                        LoadForumAddEditEventArgs objLoadForumAddEditEventArgs = new LoadForumAddEditEventArgs();
                        objLoadForumAddEditEventArgs.ForumGroupId = forumGroupId;
                        ParentControl.OnLoadForumAddEditEvent(sender, objLoadForumAddEditEventArgs);
                    }
                    else
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumGroupNotExist").ToString(), forumGroupName), true));
                        return;
                    }
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToAddForum").ToString(), true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Raise event of parent control for loading ForumAddEdit in the context of editing forum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditForum_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                string[] args = e.CommandArgument.ToString().Split(';');
                int forumId = Convert.ToInt32(args[0]);
                string forumName = args[1];
                string forumGroupName = args[2];

                //Only super administrators or administrators of the specified forum could edit the forum deteails.
                if (ParentControl.CurrentUser.IsSuperAdministrator() || ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, forumId))
                {
                    if (ForumInfo.Exists(forumId))
                    {
                        ParentControl.OnLoadForumAddEditEvent(sender, new LoadForumAddEditEventArgs(forumId));
                    }
                    else
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumNotExist").ToString(), forumName, forumGroupName), true));
                        return;
                    }
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToEditForum").ToString(), true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
		/// Raise event of parent control for loading ForumList in the context of deleting a forum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeleteForum_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                string[] args = e.CommandArgument.ToString().Split(';');
                int forumId = Convert.ToInt32(args[0]);
                string forumName = args[1];
                string forumGroupName = args[2];

                //Only super administrators or administrators of the specified forum could delete the forum.
                if (ParentControl.CurrentUser.IsSuperAdministrator() || ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, forumId))
                {
                    try
                    {
                        ForumInfo.Delete(forumId);
                    }
                    catch
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(String.Format(GetLocalResourceObject("ErrorMessageForumDelete").ToString(), forumName, forumGroupName), false));
                        return;
                    }

                    LoadForumListEventArgs eventArgs = new LoadForumListEventArgs();
                    eventArgs.Message = String.Format(GetLocalResourceObject("MessageSuccessfulForumDelete").ToString(), forumName, forumGroupName);
                    ParentControl.OnLoadForumListEvent(sender, eventArgs);
                }
                else
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToDeleteForum").ToString(), true));
                    return;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }


        /// <summary>
		/// Raise event of parent control for loading ForumTopicList in the context of opening forum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenForum_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split(';');
            int forumId = Convert.ToInt32(args[0]);
            string forumName = args[1];

            //nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + forumId);

            LoadForumTopicListEventArgs objOpenForumEventArgs = new LoadForumTopicListEventArgs(forumId, forumName);
            ParentControl.OnLoadForumTopicListEvent(this, objOpenForumEventArgs);
        }

        /// <summary>
		/// Raise event of parent control for loading ForumPostList in the context of opening forum topic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnOpenPost_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split(';');
            int forumId = Convert.ToInt32(args[0]);
            string forumName = args[1];
            int topicId = Convert.ToInt32(args[2]);
            string topicName = args[3];

			//nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:" + forumId + ";topicId:" + topicId);

            LoadForumPostListEventArgs objOpenForumTopicEventArgs = new LoadForumPostListEventArgs();
            objOpenForumTopicEventArgs.ForumId = forumId;
            objOpenForumTopicEventArgs.ForumName = forumName;
            objOpenForumTopicEventArgs.ForumTopicId = topicId;
            objOpenForumTopicEventArgs.ForumTopicName = topicName;
            ParentControl.OnLoadForumPostListEvent(this, objOpenForumTopicEventArgs);
        }

        /// <summary>
		/// Raise event of parent control for loading ForumUserDetails.ascx if there is currently logged user.
        /// Otherwise redirect to login page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cntrlForumUserNickname_LoadForumUserProfileEvent(object sender, CommandEventArgs e)
        {
            string username = Convert.ToString(e.CommandArgument);
            if (ParentControl.CurrentUser == null)
            {
                string url = Request.Url.PathAndQuery + "?mode=profile&username=" + username;
                Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), false);
            }
            else
            {
                nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("mode:profile;username:" + username);
                ParentControl.OnLoadForumUserDetailsEvent(sender, new LoadForumUserDetailEventArgs(username));
            }
        }


        /// <summary>
        /// Edit and Delete of forum are accessible only if the current logged user is super administrator or administrator of the forum.
        /// </summary>
        /// <param name="forumId"></param>
        /// <returns>true/false</returns>
        protected bool CheckEditDeleteRights(int forumId)
        {
            bool result = false;
            if ((ParentControl.CurrentUser != null)
                && ((ParentControl.CurrentUser.IsSuperAdministrator()) || (ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator, forumId)))
                )
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Check whether the currently logged user is super administrator.
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentUserSuperAdmin()
        {
            bool result = false;
            if ((ParentControl.CurrentUser != null) && (ParentControl.CurrentUser.IsSuperAdministrator()))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Display all forums grouped by forum groups in two repeaters
        /// The first repeater (repForumGroups) is for the forum groups. 
        /// The inner repeater (repForums) is for the forums from the relevant forum group.
        /// </summary>
        private void ListForums()
        {
            ForumView objForumView = new ForumView();

            //Inactive forum groups are visible only for SuperAdministrators
            if ((ParentControl.CurrentUser == null) || (!ParentControl.CurrentUser.IsSuperAdministrator()))
            {
                objForumView.ForumGroupIsActive = true;
            }

            SortedList<object, List<ForumView>> forumList = ForumView.ListAsGeneralList("ForumGroupOrderNumber", objForumView);
            repForumGroups.DataSource = forumList;
            repForumGroups.DataBind();

            if (repForumGroups.Items.Count != 0)
            {
                //First forum group couldn't be moved up
                ImageButton ibtnMoveUpForumGroup = (ImageButton)repForumGroups.Items[0].FindControl("ibtnMoveUpForumGroup");
                ibtnMoveUpForumGroup.Enabled = false;
                ibtnMoveUpForumGroup.ImageUrl = Utilities.GetImageUrl(this.Page, "ForumStyles/Images/move_up_off.gif");

                //Last forum group couldn't be moved down
                ImageButton ibtnMoveDownForumGroup = (ImageButton)repForumGroups.Items[repForumGroups.Items.Count - 1].FindControl("ibtnMoveDownForumGroup");
                ibtnMoveDownForumGroup.Enabled = false;
                ibtnMoveDownForumGroup.ImageUrl = Utilities.GetImageUrl(this.Page, "ForumStyles/Images/move_down_off.gif");
            }
        }
    }
}
