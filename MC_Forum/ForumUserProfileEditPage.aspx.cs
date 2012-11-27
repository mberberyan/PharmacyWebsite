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
using Melon.Components.Forum.Configuration;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for modifying forum user profile details.
    /// </summary>
    /// <remarks>
    ///     <para>This web page is loaded in iframe in user control <see cref="ForumUserProfileEdit"/> and provides user interface for 
    ///     modifying the profile details of forum user.
    ///     The id of the forum user is retrieved from the query string.
    ///     </para>
    ///     <para>The following web controls build this web page:
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Web Control</term><description>Description</description>
    ///          </listheader>
    ///             <item><term>Image imgPhoto</term><description>Photo (avatar) of forum user.</description></item>
    ///             <item><term>CheckBox chkRemovePhoto</term><description>CheckBox which is visible if the forum user has uploaded photo (avatar).
    ///                 Checked if the current photo has to be removed.</description></item>
    ///             <item><term>TextBox txtFirstName</term><description>First name of forum user.</description></item>
    ///             <item><term>TextBox txtLastName</term><description>Last name of the forum user.</description></item>
    ///             <item><term>TextBox txtICQNumber</term><description>ICQ number of forum user.</description></item>
    ///             <item><term>TextBox txtEmail</term><description>Email address of forum user.</description></item>
    ///             <item><term>Button btnSave</term><description>Save forum user settings.</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///     All web controls from ForumUserProfileEditPage are using the local resources.
    ///     To customize them modify resource file ForumUserProfileEditPage.resx placed in the MC_Forum folder.
    ///     </para>
    ///</remarks>
    ///<seealso cref="Melon.Components.Forum.ForumUser"/>
    ///<seealso cref="Melon.Components.Forum.ForumUserInfo"/>
    ///<seealso cref="Melon.Components.Forum.ForumUserProvider"/>
    public partial class ForumUserProfileEditPage : System.Web.UI.Page
    {
        /// <summary>
        /// User name of forum user which profile will be modified. It is feeded from query parameter username.
        /// </summary>
        public string UserName;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ForumUserProfileEditPage()
		{
			this.PreInit += new EventHandler(ForumUserProfileEdit_PreInit);
		}

        /// <summary>
        /// Sets the theme of the web page to the theme passed as a query parameter themeName.
        /// This theme is the theme of the web page where the forum component is integrated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void ForumUserProfileEdit_PreInit(object sender, EventArgs e)
		{
			string themeName = Request.QueryString["themeName"];
			if (themeName != null)
			{
                if (File.Exists(Server.MapPath("~/App_Themes/themeName")))
                {
                    this.Page.Theme = themeName;
                    this.link.Href = "";
                }
			}
		}

		/// <summary>
        /// Reads from query string parameter username, retrieves the forum user details and display them.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
            this.UserName = (Request.QueryString["username"] == null) ? (string)null : Request.QueryString["username"];
            lblPhotoInstructions.Text = String.Format(Convert.ToString(GetLocalResourceObject("PhotoInstructions")),ForumSettings.UserPhotoAllowedExtensions,ForumSettings.UserPhotoMaxSize.Value.ToString());

			if (!Page.IsPostBack)
			{
				if (!String.IsNullOrEmpty(this.UserName))
				{
					LoadForumUserDetails(this.UserName);
				}
			}
		}

		/// <summary>
		/// Updates forum user details.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSave_Click(object sender, EventArgs e)
		{

			ForumUser objForumUser = ForumUserInfo.Load(this.UserName);

            if (objForumUser != null)
            {
                objForumUser.FirstName = txtFirstName.Text.Trim();
                objForumUser.LastName = txtLastName.Text.Trim();
                objForumUser.Email = txtEmail.Text.Trim();
                objForumUser.ICQNumber = txtICQNumber.Text.Trim();
                objForumUser.IsProfileVisible = chkPublishProfileDetails.Checked;

                Photo objPhoto = new Photo(cntrlFileUpload.FileBytes.Length);
                if (!chkRemovePhoto.Checked)
                {
                    if (cntrlFileUpload.HasFile)
                    {
                        objPhoto.BinaryInfo = cntrlFileUpload.FileBytes;
                        objPhoto.FileExtension = Path.GetExtension(cntrlFileUpload.PostedFile.FileName).ToUpper();
                        objPhoto.FileSize = cntrlFileUpload.FileBytes.Length;

                        if (ForumSettings.UserPhotoAllowedExtensions != null)
                        {
                            //*** Check the allowed extensions ***
                            string[] arrExtensions = ForumSettings.UserPhotoAllowedExtensions.Split(',');
                            ArrayList allowedExtensions = new ArrayList(arrExtensions);
                            if (!allowedExtensions.Contains(objPhoto.FileExtension))
                            {
                                RegisterScriptShowError(String.Format(GetLocalResourceObject("ForumUserPhotoNotAllowedExtension").ToString(), ForumSettings.UserPhotoAllowedExtensions));
                                lblMessage.Visible = false;
                                return;
                            }
                        }

                        if (ForumSettings.UserPhotoMaxSize != null)
                        {
                            //*** Check the allowed size ***
                            //ForumSettings.UserPhotoMaxSize is set in KB.
                            if (objPhoto.FileSize > (ForumSettings.UserPhotoMaxSize * 1024))
                            {
                                RegisterScriptShowError(String.Format(GetLocalResourceObject("ForumUserPhotoNotAllowedSize").ToString(), ForumSettings.UserPhotoMaxSize));
                                lblMessage.Visible = false;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    objPhoto.RemovePreviousPhoto = true;
                }
                objForumUser.Photo = objPhoto;


                //Try to save the forum user details
                try
                {
                    ForumUserInfo.Save(objForumUser);
                }
                catch (DirectoryNotFoundException)
                {
                    RegisterScriptShowError(GetLocalResourceObject("DirectoryNotFoundException").ToString());
                    lblMessage.Visible = false;
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    RegisterScriptShowError(GetLocalResourceObject("ErrorMessageUnauthorizedException").ToString());
                    lblMessage.Visible = false;
                    return;
                }
                catch
                {
                    RegisterScriptShowError(GetLocalResourceObject("ErrorMessageForumUserSave").ToString());
                    lblMessage.Visible = false;
                    return;
                }

                //After successful save refresh the new details
                lblMessage.Text = GetLocalResourceObject("MessageSuccessfulForumUserSave").ToString();
                lblMessage.Visible = true;

                LoadForumUserDetails(this.UserName);
            }
            else
            {

            }


		}

		/// <summary>
		/// Retrieves from database details for the forum user and display them in the interface.
		/// </summary>
		///<param name="username"/>
		private void LoadForumUserDetails(string username)
		{
            ForumUser objForumUser = ForumUserInfo.Load(username);

            if (objForumUser != null)
            {
                if ((objForumUser.PhotoPath != null) && (objForumUser.PhotoPath != string.Empty))
                {
                    imgPhoto.ImageUrl = objForumUser.PhotoPath;
                    chkRemovePhoto.Visible = true;
                }
                else
                {
                    imgPhoto.ImageUrl = Utilities.GetImageUrl(this.Page, "ForumStyles/Images/snimka.gif");
                    chkRemovePhoto.Visible = false;
                }
                chkRemovePhoto.Checked = false;
                txtFirstName.Text = objForumUser.FirstName;
                txtLastName.Text = objForumUser.LastName;
                txtEmail.Text = objForumUser.Email;
                txtICQNumber.Text = objForumUser.ICQNumber;
                chkPublishProfileDetails.Checked = objForumUser.IsProfileVisible.Value;
            }
		}

        /// <summary>
        /// Register startup client script for displaying error.
        /// </summary>
        /// <param name="errorMessage">Message of the error.</param>
        private void RegisterScriptShowError(string errorMessage)
        {
           ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"show_error",
                @"var forumError = new parent.UserProfileError('" + errorMessage + "');forumError.show();", true);
        } 
    }
}
