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
using Melon.Components.Forum.Exception;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for creating new forum or updating existing one.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     The ForumAddEdit user control provides user interface for 
    ///     creating or updating forum characterized with title, description, administrators, moderators and some forum flags.
    ///     If field <see cref="ForumId"/> contains forum id then the control is in context of modifying existing forum. 
    ///     If ForumId is not set then new forum will be created and it will be created in forum group with id <see cref="ForumGroupId"/>.
    ///     The forum group could be changed  by selecting new forum group from list with available forum groups <see cref="ddlForumGroups"/>
    ///     The administrators and moderators of the forum are selected from lists with available forum users <see cref="lstCandidateAdministrators"/>
    ///     and <see cref="lstCandidateAdministrators"/>.
    ///     The control is using <see cref="Melon.Components.Forum.Forum"/> and <see cref="Melon.Components.Forum.ForumInfo"/> classes for saving the forum settings.  
    ///     </para>
    ///     <para>
    ///     The following web controls build this user control:
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Web Control</term><description>Description</description>
    ///         </listheader>
    ///         <item><term>TextBox txtForumName</term><description>Title of forum.</description></item>
    ///         <item><term>TextBox txtForumDescription</term><description>Description of forum</description></item>
    ///         <item><term>ddlForumGroups</term><description>DropDown list with the available forum groups.</description></item>
    ///         <item><term>ListBox lstCandidateAdministrators</term><description>List of existing forum users who could be administrators of the forum.</description></item>
    ///         <item><term>ListBox lstAdministrators</term><description>List of current administrators of the forum.</description></item>
    ///         <item><term>ListBox lstCandidateModerators</term><description>List of existing forum users who could be moderators of the forum.</description></item>
    ///         <item><term>ListBox lstModerators</term><description>List of current moderators of the forum.</description></item>
    ///         <item><term>CheckBox chkIsActive</term><description>Checked if the forum is activated and is visible for the users.</description></item>
    ///         <item><term>CheckBox chkIsClosed</term><description>Checked if the forum is closed. Forum users could not post in closed forums but they could browse them.</description></item>
    ///         <item><term>CheckBox chkIsPublic</term><description>Checked if the forum is public. In this case the visitors of the forum could post even if they are not registered.</description></item>
    ///         <item><term>Button btnSaveForum</term><description>Saves the forum settings and close ForumAddEdit control.</description></item>
    ///         <item><term>Button btnCancel</term><description>Close ForumAddEdit control without saving the forum settings.</description></item>
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
    ///<seealso cref="Melon.Components.Forum.Forum"/>
    ///<seealso cref="Melon.Components.Forum.ForumInfo"/>
    ///<seealso cref="Melon.Components.Forum.ForumUserRoles"/>
    public partial class ForumAddEdit : ForumControl
    {
        //#region Controls
        ///// <summary>
        ///// Codebehind declaration of ibtnAddAdministrator in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ImageButton ibtnAddAdministrator;
        ///// <summary>
        ///// Codebehind declaration of ibtnRemoveAdministrator in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ImageButton ibtnRemoveAdministrator;
        ///// <summary>
        ///// Codebehind declaration of ibtnAddModerator in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ImageButton ibtnAddModerator;
        ///// <summary>
        ///// Codebehind declaration of ibtnRemoveModerator in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ImageButton ibtnRemoveModerator;
        ///// <summary>
        ///// Codebehind declaration of lstAdministrators in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ListBox lstAdministrators;
        ///// <summary>
        ///// Codebehind declaration of lstCandidateAdministrators in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ListBox lstCandidateAdministrators;
        ///// <summary>
        ///// Codebehind declaration of lstModerators in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ListBox lstModerators;
        ///// <summary>
        ///// Codebehind declaration of lstCandidateModerators in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.ListBox lstCandidateModerators;
        ///// <summary>
        ///// Codebehind declaration of hfAdministrators in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.HiddenField hfAdministrators;
        ///// <summary>
        ///// Codebehind declaration of hfModerators in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.HiddenField hfModerators;
        ///// <summary>
        ///// Codebehind declaration of ddlForumGroups in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.DropDownList ddlForumGroups;
        ///// <summary>
        ///// Codebehind declaration of txtForumName in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.TextBox txtForumName;
        ///// <summary>
        ///// Codebehind declaration of txtForumDescription in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.TextBox txtForumDescription;
        ///// <summary>
        ///// Codebehind declaration of chkIsActive in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.CheckBox chkIsActive;
        ///// <summary>
        ///// Codebehind declaration of chkIsClosed in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.CheckBox chkIsClosed;
        ///// <summary>
        ///// Codebehind declaration of chkIsPublic in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.CheckBox chkIsPublic;
        ///// <summary>
        ///// Codebehind declaration of btnCancel in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.Button btnCancel;
        ///// <summary>
        ///// Codebehind declaration of btnSaveForum in the ascx control.
        ///// </summary>
        //public System.Web.UI.WebControls.Button btnSaveForum;

        //#endregion Controls

        /// <summary>
        /// The id of the forum group to which was initially selected to created forum. 
        /// </summary>
        public int? ForumGroupId;
        /// <summary>
        /// Id of forum which will be modified.
        /// </summary>
        public int? ForumId;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.ForumGroupId = (int?)args[0];
            this.ForumId = (int?)args[1];
        }

        /// <summary>
        /// Attach event handlers.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSaveForum.Click += new EventHandler(btnSaveForum_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Register possible valid values for ListBoxes: 
        /// lstCandidateAdministrators, lstAdministrators, lstCandidateModerators, lstModerators.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (ListItem item in lstCandidateAdministrators.Items)
            {
                this.Page.ClientScript.RegisterForEventValidation(lstAdministrators.UniqueID, item.Value);
            }
            foreach (ListItem item in lstAdministrators.Items)
            {
                this.Page.ClientScript.RegisterForEventValidation(lstCandidateAdministrators.UniqueID, item.Value);
            }

            foreach (ListItem item in lstCandidateModerators.Items)
            {
                this.Page.ClientScript.RegisterForEventValidation(lstModerators.UniqueID, item.Value);
            }
            foreach (ListItem item in lstModerators.Items)
            {
                this.Page.ClientScript.RegisterForEventValidation(lstCandidateModerators.UniqueID, item.Value);
            }

            base.Render(writer);
        }


        /// <summary>
        /// Loads forum details if forum is selected for modification (forumId is not null).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            { 
                //Attach javasript to buttons 
                ibtnAddAdministrator.DataBind();
                ibtnAddAdministrator.Attributes.Add("onclick", "javascript:includeIntoSelectedUsers('"
                                                              + lstAdministrators.ClientID + "', '"
                                                              + lstCandidateAdministrators.ClientID + "'); return false;");
                ibtnRemoveAdministrator.DataBind();
                ibtnRemoveAdministrator.Attributes.Add("onclick", "javascript:excludeFromSelectedUsers('"
                                                             + lstAdministrators.ClientID + "', '"
                                                             + lstCandidateAdministrators.ClientID + "'); return false;");
                ibtnAddModerator.DataBind();
                ibtnAddModerator.Attributes.Add("onclick", "javascript:includeIntoSelectedUsers('"
                                                            + lstModerators.ClientID + "', '"
                                                            + lstCandidateModerators.ClientID + "'); return false;");
                ibtnRemoveModerator.DataBind();
                ibtnRemoveModerator.Attributes.Add("onclick", "javascript:excludeFromSelectedUsers('"
                                                           + lstModerators.ClientID + "', '"
                                                           + lstCandidateModerators.ClientID + "'); return false;");
                btnSaveForum.Attributes.Add("onclick", "selectAllIncludedOptions(" + lstAdministrators.ClientID + ", " + hfAdministrators.ClientID + ");" +
                                                       "selectAllIncludedOptions(" + lstModerators.ClientID + ", " + hfModerators.ClientID + ")");

                //Display all forum groups in dropdown list
                ListForumGroups();

                if (ForumId == null)
                {
                    //CASE CREATE FORUM

                    // Select in dropdown ddlForumGroups the group to which is selected to belong the forum
                    ddlForumGroups.Items.FindByValue(ForumGroupId.ToString()).Selected = true;

                    ListCandidateAdminAndModerators();
                }
                else
                {
                    //CASE UPDATE FORUM
                    LoadForumDetails(ForumId.Value);
                }

				this.btnSaveForum.Attributes.Add("onfocus", "reFocus('" + this.txtForumName.ClientID + "','" + this.btnSaveForum.ClientID + "')");
				ScriptManager.GetCurrent(this.Page).SetFocus(this.btnSaveForum);
                
            }
        }

        /// <summary>
        /// Creates new or update existing forum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveForum_Click(object sender, EventArgs e)
        {
            if (ParentControl.CurrentUser == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                Melon.Components.Forum.Forum objForum;
                if (ForumId == null)
                {
                    //*** Create Forum ***

                    //Check rights - only super administrators could create forums.
                    if (!ParentControl.CurrentUser.IsSuperAdministrator())
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToAddForum").ToString(),true));
                        return;
                    }

                    objForum = new Melon.Components.Forum.Forum();
                }
                else
                {
                    //*** Update Forum ***

                    //Check rights - only super administrators or administrators of the specified forum could edit the forum.
                    if (!(ParentControl.CurrentUser.IsSuperAdministrator() || ParentControl.CurrentUser.IsInRole(ForumUserRole.Administrator,this.ForumId.Value)))
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToEditForum").ToString(), true));
                        return;
                    }

                    objForum = ForumInfo.Load(ForumId.Value);
                    if (objForum == null)
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingForum").ToString(), true));
                        return;
                    }
                }

                objForum.ForumGroupId = int.Parse(this.ddlForumGroups.SelectedValue);

                objForum.Name = txtForumName.Text.Trim();
                objForum.Description = HttpUtility.HtmlDecode(txtForumDescription.Text.Trim());
                objForum.IsActive = chkIsActive.Checked;
                objForum.IsClosed = chkIsClosed.Checked;
                objForum.IsPrivate = !chkIsPublic.Checked;


                //Try to save forum to database
                int? savedForumId = null;
                try
                {
                    savedForumId = ForumInfo.Save(objForum);
                }
                catch (ForumException ex)
                {
                    if (ex.Code == ForumExceptionCode.ForumDuplicateName)
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumDuplicateName").ToString(), false));
                        return;
                    }
                }
                catch
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumSave").ToString(), false));
                    return;
                }

                if (!SaveAdministrators(savedForumId.Value) || !SaveModerators(savedForumId.Value))
                {
                    return;
                }

                ParentControl.OnLoadForumListEvent(sender, new LoadForumListEventArgs());
            }
        }

        /// <summary>
        /// Closes user control for add/edit forum (the current control).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ParentControl.OnRemoveForumControlEvent(sender, new RemoveForumControlEventArgs("ForumAddEdit.ascx"));
        }


        /// <summary>
        /// Retrieves from database all forum groups and display them in dropdown ddlForumGroups.
        /// </summary>
        private void ListForumGroups()
        {
            ForumGroup objForumGroup = new ForumGroup();
            DataTable dtForumGroups = ForumGroupInfo.List(objForumGroup);

            ddlForumGroups.DataSource = dtForumGroups;
            ddlForumGroups.DataBind();
        }

        /// <summary>
        ///Retrieves all web site users and display them in dropdown listboxes:  
        ///lstCandidateAdministrators and lstCandidateModerators.
        /// </summary>
        private void ListCandidateAdminAndModerators()
        {
            DataTable dtForumUsers = ForumUserInfo.GetAllUsers();

            lstCandidateAdministrators.DataSource = dtForumUsers;
            lstCandidateAdministrators.DataBind();

            lstCandidateModerators.DataSource = dtForumUsers;
            lstCandidateModerators.DataBind();
        }

        /// <summary>
        /// Retrives from database details for the specified by id forum and display them in the interface.
        /// </summary>
        /// <param name="forumId"></param>
        private void LoadForumDetails(int forumId)
        {
            Melon.Components.Forum.Forum objForum = ForumInfo.Load(forumId);
            if (objForum != null)
            {
                txtForumName.Text = objForum.Name;
                ddlForumGroups.Items.FindByValue(objForum.ForumGroupId.ToString()).Selected = true;
                txtForumDescription.Text = objForum.Description;

                objForum.PrepareListsOfUsers();

				objForum.Administrators.Sort(new Comparison<ForumUser>(ForumUserInfo.CompareUserNickNames));
                lstAdministrators.DataSource = objForum.Administrators;
                lstAdministrators.DataBind();

				objForum.Moderators.Sort(new Comparison<ForumUser>(ForumUserInfo.CompareUserNickNames));
                lstModerators.DataSource = objForum.Moderators;
                lstModerators.DataBind();

                DataTable allUsers = ForumUserInfo.GetAllUsers();

                if (objForum.Administrators.Count > 0)
                {
                    lstCandidateAdministrators.DataSource = SubstructUserLists(allUsers, objForum.Administrators); 
                }
                else
                {
                    lstCandidateAdministrators.DataSource = allUsers;
                }
                lstCandidateAdministrators.DataBind();

                if (objForum.Moderators.Count > 0)
                {
                    lstCandidateModerators.DataSource = SubstructUserLists(allUsers, objForum.Moderators);
                }
                else
                {
                    lstCandidateModerators.DataSource = allUsers;
                }
                lstCandidateModerators.DataBind();

                chkIsActive.Checked = objForum.IsActive.Value;
                chkIsClosed.Checked = objForum.IsClosed.Value;
                chkIsPublic.Checked = !objForum.IsPrivate.Value;
            }
        }

        /// <summary>
        /// Returns DataTable which contains users from DataTable table which are not included in the user list.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable SubstructUserLists(DataTable table, List<ForumUser> list)
        {
            DataTable result = new DataTable();
            result.Columns.Add("UserName", typeof(String));
            result.Columns.Add("Nickname",typeof(String));

            foreach (DataRow row in table.Rows)
            {
                string username = Convert.ToString(row["UserName"]);
                bool found = false;
				foreach (ForumUser obj in list)
                {
					if (obj.UserName == username)
					{
						found = true;
						break;
					}
				}
				if(!found && result.Select("UserName='" + username + "'").Length == 0)
				{
                        DataRow userRow = result.NewRow();
                        userRow["UserName"] = username;
                        userRow["Nickname"] = row["Nickname"];
                        result.Rows.Add(userRow);
                    
                }
            }

			result.DefaultView.Sort = "Nickname ASC";

            return result;
        }

        /// <summary>
        /// Saves the administrators of the forum.
        /// </summary>
        private bool SaveAdministrators(int forumId)
        {
            List<string> administratorsUserNames = new List<string>();
            if (hfAdministrators.Value != string.Empty)
            {
                string[] arrAdministratorsUserNames = hfAdministrators.Value.Split(',');
                administratorsUserNames = new List<string>(arrAdministratorsUserNames);
            }

            //Try to save administrators
            try
            {
                ForumUserInfo.UpdateUsersToRole(administratorsUserNames, ForumUserRole.Administrator, forumId);
            }
            catch
            {
                ParentControl.OnLoadErrorPopupEvent(this, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumAdministratorsSave").ToString(),false));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Saves the moderators of the forum.
        /// </summary>
        /// <param name="forumId"></param>
        private bool SaveModerators(int forumId)
        {
            List<string> moderatorsUserNames = new List<string>();
            if (hfModerators.Value != string.Empty)
            {
                string[] arrModeratorsIds = hfModerators.Value.Split(',');
                moderatorsUserNames = new List<string>(arrModeratorsIds);
            }

            //Try to save moderators
            try
            {
                ForumUserInfo.UpdateUsersToRole(moderatorsUserNames, ForumUserRole.Moderator, forumId);
            }
            catch
            {
                ParentControl.OnLoadErrorPopupEvent(this, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumModeratorsSave").ToString(), false));
                return false;
            }

            return true;
        }   
    }
}
