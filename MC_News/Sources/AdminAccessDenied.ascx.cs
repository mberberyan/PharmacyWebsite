using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Melon.Components.News;
using Melon.Components.News.ComponentEngine;
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// This control is loaded when a user who is working with News Administration has no rights to do some action.
    /// The reason for that could be that another user changed his rights whike he was working with News Administration.
    /// <para>
    ///     A message is displayed with information for the user's current role.
    /// </para>
    /// </summary>
    public partial class AdminAccessDenied : NewsControl
    {
        #region Properties

        /// <summary>
        /// User role of user for whome the access is denied.
        /// </summary>
        public UserRole UserRole;

        /// <summary>
        /// Flag whether the specified role in <see cref="UserRole"/> is the role 
        /// with which the user initially logged to News Administration Panel or it was changed by another user while working with the News Administration Panel. 
        /// </summary>
        public bool IsUserLoggedRole;

        #endregion

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.UserRole = (UserRole)args[0];
            this.IsUserLoggedRole = (bool)args[1];
        }

        /// <summary>
        /// Attaches event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.lbtnLogin.Click += new EventHandler(lbtnLogin_Click);
            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        /// Displays a message to the user that he has no rights 
        /// and informs him for his current rights in the News Administration Panel.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                if (String.IsNullOrEmpty(NewsSettings.BackEndLoginURL) || this.ResolveUrl(NewsSettings.BackEndLoginURL) == Request.RawUrl)
                {
                    lbtnLogin.Visible = false;
                    lblInstruction.Visible = false;
                }
                else
                {
                    lbtnLogin.Visible = true;
                    lblInstruction.Visible = true;
                }

                if (UserRole == UserRole.None && IsUserLoggedRole)
                {
                    lblMessage.Text = Convert.ToString(GetLocalResourceObject("AccessDenied"));
                }
                else
                {
                    if (UserRole == UserRole.None)
                    {
                        lblMessage.Text = Convert.ToString(GetLocalResourceObject("UserRoleChangedToNone"));
                    }
                    else
                    {
                        lblMessage.Text = String.Format(Convert.ToString(GetLocalResourceObject("UserRoleChanged")), Convert.ToString(GetLocalResourceObject(this.UserRole.ToString())));
                    }
                }

            }
        }

        /// <summary>
        /// Signs out current user and redirect to the login page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnLogin_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            this.ParentControl.RedirectToLoginPage();
        }
    }
}
