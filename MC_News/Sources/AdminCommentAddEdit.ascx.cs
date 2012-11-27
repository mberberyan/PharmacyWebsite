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
using Melon.Components.News.ComponentEngine;
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides interface for modification of a posted comment by news administrator.
    /// </summary>
    /// <remarks>
    ///     <para>The AdminCommentAddEdit user control provides user interface for 
    ///     updating comments posted by news readers.
    ///     Property CommentId contains the id of the comment which will be modified. 
    ///     The control is using <see cref="Melon.Components.News.Comment"/> class for loading and saving the comment details.  
    ///     </para>
    ///     <para>
    ///     All web controls from AdminCommentAddEdit user control are using local resources.
    ///     To customize them modify resource file AdminCommentAddEdit.ascx.resx placed in folder "MC_News/Sources/App_LocalResources".
    ///     </para>
    ///</remarks>
    /// <seealso cref="Comment"/>
    public partial class AdminCommentAddEdit : NewsControl
    {
        #region Fields & Properties

        /// <summary>
        /// Identifier of the comment which will be modified.
        /// </summary>
        public int CommentId;

        /// <summary>
        /// The event arguments with wich the user control "AdminCommentList.ascx" should be loaded in case button "Cancel" is clicked.
        /// Using these arguments comments listing will be loaded as it was before coming to edit the comment.
        /// </summary>
        public LoadCommentListEventArgs CommentListSettings;

        #endregion

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.CommentId = (int)args[0];
            this.CommentListSettings = (LoadCommentListEventArgs)args[1];
        }

        /// <summary>
        /// Attaches event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     Method <see cref="LoadCommentDetails"/> is called to load the comment details in the interface.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (!IsControlPostBack)
            {
                LoadCommentDetails(this.CommentId);
            }
        }

        /// <summary>
        /// Event handler for event Click of button btnSave.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     Updates the comment by gathering the details in the interface and pass them to 
        ///     method <see cref="Comment.Save(Comment)"/>.
        ///     </para>
        ///     <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise message for successful update of the comment is displayed.
        ///     </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                 UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                 if (currentUserRole == UserRole.Administrator)
                 {
                     Comment objComment = Comment.Load(this.CommentId);
                     if (objComment == null)
                     {
                         //Comment was deleted.
                         this.ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingComment").ToString(), false));

                         this.CommentListSettings.CommentIdToPositionOn = null;
                         this.ParentControl.OnLoadCommentListEvent(sender, this.CommentListSettings);
                         return;
                     }
                     else
                     {
                         objComment.Body = txtCommentText.Text.Trim();
                         if (NewsSettings.RequireApprovingComments)
                         {
                             objComment.IsApproved = chkIsApproved.Checked;
                         }

                         if (!NewsSettings.RequireLoginToPostComments)
                         {
                             objComment.Author = txtAuthor.Text.Trim();
                         }

                         try
                         {
                             Comment.Save(objComment);
                         }
                         catch
                         {
                             this.ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("SaveCommentErrorMessage").ToString(), false));
                             return;
                         }

                         //Successful save
                         lblMessage.Visible = true;
                         lblMessage.Text = Convert.ToString(GetLocalResourceObject("SaveCommentSuccessfulMessage"));
                     }
                 }
                 else
                 {
                     //The current logged user is not Administrator. So he could not edit comments.
                     LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                     args.IsUserLoggedRole = false;
                     args.UserRole = currentUserRole;
                     this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
                 }
            }
            else
            {
                //There is no logged user (session time out).
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of button btnCancel.
        /// </summary>
        /// <remarks>
        ///    Raises event <see cref="BaseNewsControl.RemoveNewsControlEvent"/> of parent user control 
        /// to remove from the interface the current user control "AdminCommentAddEdit.ascx".   
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.CommentListSettings.CommentIdToPositionOn = this.CommentId;
            this.ParentControl.OnLoadCommentListEvent(sender, this.CommentListSettings);
        }


        /// <summary>
        /// Retrieves from database details of the specified by id comment and displays them in the interface.
        /// </summary>
        /// <remarks>
        ///     Comment details are retrieved by calling static method <see cref="Comment.Load(int)"/>.
        /// </remarks>
        /// <param name="id">Comment identifier.</param>
        private void LoadCommentDetails(int id)
        {
            Comment objComment = Comment.Load(id);
            txtCommentText.Text = Server.HtmlDecode(objComment.Body);

            if (NewsSettings.RequireLoginToPostComments)
            {
                trAuthorDetails.Visible = false;
            }
            else
            {
                txtAuthor.Text = objComment.Author;
            }
            
            if (NewsSettings.RequireApprovingComments)
            {
                chkIsApproved.Checked = objComment.IsApproved.Value;
            }
            else
            {
                lblApprove.Visible = false;
                chkIsApproved.Visible = false;
            }
        }
    }
}
