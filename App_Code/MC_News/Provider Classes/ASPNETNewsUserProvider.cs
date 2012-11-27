using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Melon.Components.News
{
    /// <summary>
    /// Implements News user provider using ASP.NET Membership.
    /// </summary>
    public class ASPNETNewsUserProvider : Melon.Components.News.Providers.NewsUserProvider
    {
        #region UserProvider methods

        public override Melon.Components.News.NewsUser Load()
        {
            //Get current logged user.
            MembershipUser aspnetUser = Membership.GetUser();

            if (aspnetUser != null)
            {
                Melon.Components.News.NewsUser newsUser = new Melon.Components.News.NewsUser();
                newsUser.UserName = aspnetUser.UserName;
                newsUser.Email = aspnetUser.Email;
                newsUser.FirstName = ((ProfileCommon)HttpContext.Current.Profile).MC_FirstName;
                newsUser.LastName = ((ProfileCommon)HttpContext.Current.Profile).MC_LastName;
                newsUser.AdditionalInfo = ((ProfileCommon)HttpContext.Current.Profile).MC_AdditionalInfo;

                return newsUser;
            }
            else
            {
                return null;
            }
        }

        public override Melon.Components.News.NewsUserDataTable List(Melon.Components.News.NewsUser objUser)
        {
            Melon.Components.News.NewsUserDataTable dtUsers = new Melon.Components.News.NewsUserDataTable();

            MembershipUserCollection users = Membership.GetAllUsers();

            foreach (MembershipUser user in users)
            {
                if ((user.UserName.ToLower().Contains(objUser.UserName.ToLower()))
                    && (user.Email.ToLower().Contains(objUser.Email.ToLower())))
                {
                    ProfileCommon userProfile = new ProfileCommon();
                    userProfile = userProfile.GetProfile(user.UserName);

                    if ((userProfile.MC_FirstName.ToLower().Contains(objUser.FirstName.ToLower()))
                        && (userProfile.MC_LastName.ToLower().Contains(objUser.LastName.ToLower())))
                    {
                        DataRow drUserDetails = dtUsers.NewRow();
                        drUserDetails["UserName"] = user.UserName;
                        drUserDetails["Email"] = user.Email;
                        drUserDetails["FirstName"] = userProfile.MC_FirstName;
                        drUserDetails["LastName"] = userProfile.MC_LastName;
                        drUserDetails["AdditionalInfo"] = userProfile.MC_AdditionalInfo;

                        dtUsers.Rows.Add(drUserDetails);
                    }
                }
            }

            return dtUsers;
        }

        #endregion
    }
}
