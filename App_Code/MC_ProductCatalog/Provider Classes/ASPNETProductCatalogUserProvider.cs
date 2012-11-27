using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Melon.Components.ProductCatalog;
using System.Web.Profile;

namespace Melon.Components.ProductCatalog
{
    /// <summary>
    /// Implements ProductCatalog user provider using ASP.NET Membership.
    /// </summary>
    public class ProductCatalogUserProvider : Melon.Components.ProductCatalog.Providers.UserProvider
    {
        public ProductCatalogUserProvider()
        {

        }

        public override User Load()
        {
            //Get current logged user.
            MembershipUser member = Membership.GetUser();

            if (member != null)
            {
                member = Membership.GetUser((Guid?)member.ProviderUserKey);

                Melon.Components.ProductCatalog.User user = new Melon.Components.ProductCatalog.User();
                user.UserName = member.UserName;
                user.Email = member.Email;
                user.FirstName = ((ProfileCommon)HttpContext.Current.Profile).MC_FirstName;
                user.LastName = ((ProfileCommon)HttpContext.Current.Profile).MC_LastName;

                return user;
            }
            else
            {
                return null;
            }
        }

        public override UserDataTable List(User objUser)
        {
            UserDataTable dtUsers = new UserDataTable();
            MembershipUserCollection members = Membership.GetAllUsers();

            foreach (MembershipUser member in members)
            {
                if ((member.UserName.Contains(objUser.UserName))
                    && (member.Email.Contains(objUser.Email)))
                {
                    ProfileCommon userProfile = new ProfileCommon();
                    userProfile = userProfile.GetProfile(member.UserName);

                    if ((userProfile.MC_FirstName.Contains(objUser.FirstName))
                        && (userProfile.MC_LastName.Contains(objUser.LastName)))
                    {
                        DataRow drUserDetails = dtUsers.NewRow();
                        drUserDetails["UserName"] = member.UserName;
                        drUserDetails["Email"] = member.Email;
                        drUserDetails["FirstName"] = userProfile.MC_FirstName;
                        drUserDetails["LastName"] = userProfile.MC_LastName;

                        dtUsers.Rows.Add(drUserDetails);
                    }
                }
            }

            return dtUsers;
        }
    }
}