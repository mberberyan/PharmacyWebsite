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
using Melon.Components.Forum;
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.Exception;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for creating new forum group or updating existing one.
    /// </summary>
    /// <remarks>
    ///     <para>The ForumGroupAddEdit user control provides user interface for 
    ///     creating or updating forum group characterized with title and activation flag.
    ///     If field <see cref="ForumGroupId"/> contains forum group id then the control is in context of modifying existing forum group. 
    ///     If ForumGroupId is not set then new forum group will be created. The new forum group has the biggest order number. 
    ///     If forum group is not activated then the group and all forums in it are not visible in user control ForumList.
    ///     The control is using <see cref="Melon.Components.Forum.ForumGroup"/> and <see cref="Melon.Components.Forum.ForumGroupInfo"/> classes for saving the forum settings.  
    ///     </para>
    ///     <para>The following web controls build this user control:
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Web Control</term><description>Description</description>
    ///          </listheader>
    ///             <item><term>TextBox txtForumGroupName</term><description>Name of forum group.</description></item>
    ///             <item><term>CheckBox chkIsActive</term><description>Checked if the forum group is activated and is visible for the users.</description></item>
    ///             <item><term>Button btnSaveForum</term><description>Saves the forum group settings and close ForumGroupAddEdit control.</description></item>
    ///             <item><term>Button btnCancel</term><description>Close ForumGroupAddEdit control without saving the forum group settings.</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///     Required forum group setting is the name so there is RequiredFieldValidator control for txtForumGropName. 
    ///     </para>
    ///     <para>
    ///     All web controls from ForumGroupAddEdit are using the local resources.
    ///     To customize them modify resource file ForumGroupAddEdit.resx placed in the MC_Forum folder.
    ///     </para>
    ///</remarks>
    ///<seealso cref="Melon.Components.Forum.ForumGroup"/>
    ///<seealso cref="Melon.Components.Forum.ForumGroupInfo"/>
    public partial class ForumGroupAddEdit : ForumControl
    {
        /// <summary>
        /// Id of the forum group that will be modified. If new forum group is created it is not set.
        /// </summary>
        public int? ForumGroupId;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.ForumGroupId = (int?)args[0];
        }

        /// <summary>
        /// Attach event handlers.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            btnSaveForumGroup.Click += new EventHandler(btnSaveForumGroup_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
        }

        /// <summary>
        /// Loads forum group details if forum group is selected for modification (forumGroupId is not null).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                this.btnSaveForumGroup.Attributes.Add("onfocus", "document.getElementById('" + this.txtForumGroupName.ClientID + "').focus();");
                ScriptManager.GetCurrent(this.Page).SetFocus(this.btnSaveForumGroup);

                if (ForumGroupId != null)
                {
                    //Edit mode
                    LoadForumGroupDetails(ForumGroupId.Value);
                }
            }
        }

        /// <summary>
        /// Creates new or update existing forum group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveForumGroup_Click(object sender, EventArgs e)
        {
            if (ParentControl.CurrentUser == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                ForumGroup objForumGroup;
                if (ForumGroupId == null)
                {
                    //*** Create Forum Group ***
                    //Check rights - only super administrators could create forum groups.
                    if (!ParentControl.CurrentUser.IsSuperAdministrator())
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToAddForumGroup").ToString(),true));
                        return;
                    }

                    objForumGroup = new ForumGroup();
                }
                else
                {
                    //*** Update Forum group ***
                    //Check rights - only super administrators could edit forum groups.
                    if (!ParentControl.CurrentUser.IsSuperAdministrator())
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("UserNotAllowedToEditForumGroup").ToString(), true));
                        return;
                    }

                    objForumGroup = ForumGroupInfo.Load(ForumGroupId.Value);
                    if (objForumGroup == null)
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingForumGroup").ToString(), true));
                        return;
                    }

                }

                objForumGroup.Name = txtForumGroupName.Text.Trim();
                objForumGroup.IsActive = chkIsActive.Checked;

                //Try to save to database
                try
                {
                    ForumGroupInfo.Save(objForumGroup);
                }
                catch (ForumException ex)
                {
                    if (ex.Code == ForumExceptionCode.ForumGroupDuplicateName)
                    {
                        ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumGroupDuplicateName").ToString(), false));
                        return;
                    }
                }
                catch
                {
                    ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(GetLocalResourceObject("ErrorMessageForumGroupSave").ToString(), false));
                    return;
                }

                ParentControl.OnLoadForumListEvent(sender, new LoadForumListEventArgs());
            }
        }

        /// <summary>
        /// Closes user control for add/edit forum group (the current control).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ParentControl.OnRemoveForumControlEvent(sender, new RemoveForumControlEventArgs("ForumGroupAddEdit.ascx"));
        }

        /// <summary>
        /// Retrives from database details for the specified by id forum group and display them in the interface.
        /// </summary>
        /// <param name="groupId"></param>
        private void LoadForumGroupDetails(int groupId)
        {
            ForumGroup objForumGroup = ForumGroupInfo.Load(groupId);
            if (objForumGroup != null)
            {
                txtForumGroupName.Text = objForumGroup.Name;
                chkIsActive.Checked = objForumGroup.IsActive.Value;
            }
        }
    }
}
