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
    /// Provides user interface for displaying forum user profile details.
    /// </summary>
    /// <remarks>
    ///     <para>The ForumUserProfileDetails user control provides user interface for 
    ///     displaying the profile details of forum user who made his profile visible in the forums.
    ///     The field <see cref="UserName"/> contains user name of forum user which profile will be displayed.
    ///     If there is no currently logged forum user and ForumUserProfileDetails control is loaded there is redirect to the Login page of the web site.
    ///     </para>
    ///     <para>The following web controls build this user control:
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Web Control</term><description>Description</description>
    ///          </listheader>
    ///             <item><term>Image imgForumUserPhoto</term><description>Photo (avatar) of forum user.</description></item>
    ///             <item><term>Label lblNickname</term><description>Nickname of forum user.</description></item>
    ///             <item><term>Label lblRegistrationDate</term><description>Date on which the forum user created his account.</description></item>
    ///             <item><term>Label lblFirstName</term><description>First name of forum user.</description></item>
    ///             <item><term>Label lblLastName</term><description>Last name of the forum user.</description></item>
    ///             <item><term>Label lblTotalPostsCount</term><description>Number of posts forum user posted in the forums of the web site.</description></item>
    ///             <item><term>Label lblICQNumber</term><description>ICQ number of forum user.</description></item>
    ///             <item><term>HyperLink hlnkEmail</term><description>Email address of forum user.</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///     All web controls from ForumUserProfileDetails are using the local resources.
    ///     To customize them modify resource file ForumUserProfileDetails.resx placed in the MC_Forum folder.
    ///     </para>
    ///</remarks>
    ///<seealso cref="Melon.Components.Forum.ForumUser"/>
    ///<seealso cref="Melon.Components.Forum.ForumUserInfo"/>
    ///<seealso cref="Melon.Components.Forum.ForumUserProvider"/>
    public partial class ForumUserProfileDetails : ForumControl
    {
        /// <summary>
        /// User name of the forum user which profile details will be displayed.
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
        /// Display profile details for the selected forum user with user name specified in <see cref="UserName"/>. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                if (!String.IsNullOrEmpty(this.UserName))
                {
                    LoadForumUserDetails(this.UserName);
                }           
            }
        }


        /// <summary>
        /// Retrieves from database the details for the forum user with specified user name and display them in the interface.
        /// </summary>
        /// <param name="username"></param>
        private void LoadForumUserDetails(string username)
        {
            ForumUser objForumUser = ForumUserInfo.Load(username);
            if ((objForumUser != null) && (objForumUser.IsProfileVisible.Value))
            {
                if ((objForumUser.PhotoPath != null) && (objForumUser.PhotoPath != string.Empty))
                {
                    imgForumUserPhoto.ImageUrl = objForumUser.PhotoPath;
                }
                else
                {
                    imgForumUserPhoto.ImageUrl = Utilities.GetImageUrl(this.Page, "ForumStyles/Images/snimka.gif");
                }

                lblNickname.Text = Server.HtmlEncode(objForumUser.Nickname);
                lblRegistrationDate.Text = ((DateTime)objForumUser.CreationDate).ToString("ddd MMM dd, yyyy, hh:mm");

                lblFirstName.Text = Server.HtmlEncode(objForumUser.FirstName);
                lblLastName.Text = Server.HtmlEncode(objForumUser.LastName);
                hlnkEmail.Text = Server.HtmlEncode(objForumUser.Email);
                hlnkEmail.NavigateUrl = "mailto:" + objForumUser.Email;
                lblICQNumber.Text = Server.HtmlEncode(objForumUser.ICQNumber);

                //Set statistics
                DataRow drStatistics = objForumUser.GetForumStatistics();
                lblTotalPostsCount.Text = drStatistics["TotalPosts"].ToString();
            }
        }
    }
}
