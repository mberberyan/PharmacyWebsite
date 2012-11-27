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
using System.IO;
using Melon.Components.Forum.ComponentEngine;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for modifying forum user profile details.
    /// </summary>
    /// <remarks>
    ///     <para>The ForumUserProfileEdit user control contains only a frame where will be loaded web page ForumUserProfileEditPage 
    ///     which provides actually the interface for modifying the forum user profile. 
    ///     The using of a web page was required because of the AJAX forum version and the fact 
    ///     that the upload of forum user photo needs full post back.
    ///     The field <see cref="UserName"/> contains user name of forum user which profile will be displayed.
    ///     If there is no currently logged forum user and ForumUserProfileDetails control is loaded there is redirect to the Login page of the web site.
    ///     Otherwise the ForumUserProfileEditPage web page is loaded in the iframe with query parameters: 
    ///     username and themeName (theme of the web page where the forum component is integrated).
    ///     </para>
    ///</remarks>
    ///<seealso cref="Melon.Components.Forum.ForumUser"/>
    ///<seealso cref="Melon.Components.Forum.ForumUserInfo"/>
    ///<seealso cref="Melon.Components.Forum.ForumUserProvider"/>
	public partial class ForumUserProfileEdit : ForumControl
	{
        /// <summary>
        /// User name of forum user which profile details will be loaded and modified.
        /// </summary>
		public string UserName;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.UserName = (string)args[0];
        }

        /// <summary>
        /// Attach event handlers to the controls'events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.btnShowError.Click += new EventHandler(btnShowError_Click);
            base.OnInit(e);
        }

		/// <summary>
		/// The handler for the load event of the page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsControlPostBack)
			{
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(),"mc_forum_edit_profile_error",
                 @"function UserProfileErrorShow()
                   {
                        document.getElementById('" + hfError.ClientID + @"').value = this.errorMessage;
                        document.getElementById('" + btnShowError.ClientID + @"').click();
                   }", true);

				if (ParentControl.CurrentUser == null)
				{
					FormsAuthentication.RedirectToLoginPage();
				}
				else
				{
					if (!String.IsNullOrEmpty(this.UserName))
					{
						this.ifrProfile.Attributes.Add("src", ResolveUrl(".") + "/ForumUserProfileEditPage.aspx?username=" + this.UserName + ((this.Page.Theme != null) ? "&themeName=" + this.Page.Theme : ""));
					}
				}
			}
		}
        
        /// <summary>
        ///  Raises OnLoadErrorPopupEvent event of parent user control. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowError_Click(object sender, EventArgs e)
        {
            ParentControl.OnLoadErrorPopupEvent(sender, new LoadErrorPopupEventArgs(hfError.Value, false));
        }
	}
}
