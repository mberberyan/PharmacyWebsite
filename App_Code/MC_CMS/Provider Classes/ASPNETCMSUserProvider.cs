using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace Melon.Components.CMS
{
    /// <summary>
    /// Implements CMS user provider using ASP.NET Membership.
    /// </summary>
    public class ASPNETCMSUserProvider : Melon.Components.CMS.Providers.UserProvider
    {
        #region Fields & Properties

        private static SortedList<Guid, MembershipUser> availableUsersByGuid = new SortedList<Guid, MembershipUser>();

        private static readonly object locker = new object();

        #endregion

        #region Constructors

        static ASPNETCMSUserProvider()
        {
            lock (locker)
            {
                MembershipUserCollection availableUsers = Membership.GetAllUsers();
                foreach (MembershipUser user in availableUsers)
                {
                    availableUsersByGuid.Add((Guid)user.ProviderUserKey, user);
                }
            }
        }

        #endregion

        #region UserProvider methods

        public override Melon.Components.CMS.User Load()
        {
            //Get current logged user.
            MembershipUser aspnetUserHttpContext = Membership.GetUser();
            MembershipUser aspnetUser = null;

            if (aspnetUserHttpContext != null)
            {
                // Try to get the user from memory
                if (availableUsersByGuid.ContainsKey((Guid)aspnetUserHttpContext.ProviderUserKey))
                {
                    aspnetUser = availableUsersByGuid[(Guid)aspnetUserHttpContext.ProviderUserKey];
                }

                if (aspnetUser == null)
                {
                    aspnetUser = Membership.GetUser((Guid?)aspnetUserHttpContext.ProviderUserKey);
                    // The user is found in database, so add it in memory too
                    if (aspnetUser != null)
                    {
                        lock (locker)
                        {
                            availableUsersByGuid.Add((Guid)aspnetUser.ProviderUserKey, aspnetUser);
                        }
                    }
                }
                Melon.Components.CMS.User cmsUser = new Melon.Components.CMS.User();
                cmsUser.UserName = aspnetUser.UserName;
                cmsUser.Email = aspnetUser.Email;
                cmsUser.FirstName = ((ProfileCommon)HttpContext.Current.Profile).MC_FirstName;
                cmsUser.LastName = ((ProfileCommon)HttpContext.Current.Profile).MC_LastName;

                return cmsUser;
            }
            else
            {
                return null;
            }
        }

        public override Melon.Components.CMS.UserDataTable List(Melon.Components.CMS.User objUser)
        {
            Melon.Components.CMS.UserDataTable dtUsers = new Melon.Components.CMS.UserDataTable();
            MembershipUserCollection users = null;

            // Good point to refresh users in memory
            lock (locker)
            {
                users = Membership.GetAllUsers();
                availableUsersByGuid = new SortedList<Guid, MembershipUser>();
                foreach (MembershipUser user in users)
                {
                    availableUsersByGuid.Add((Guid)user.ProviderUserKey, user);
                }
            }


            foreach (MembershipUser user in users)
            {
                if ((user.UserName.Contains(objUser.UserName))
                    && (user.Email.Contains(objUser.Email)))
                {
                    ProfileCommon userProfile = new ProfileCommon();
                    userProfile = userProfile.GetProfile(user.UserName);

                    if ((userProfile.MC_FirstName.Contains(objUser.FirstName))
                        && (userProfile.MC_LastName.Contains(objUser.LastName)))
                    {
                        DataRow drUserDetails = dtUsers.NewRow();
                        drUserDetails["UserName"] = user.UserName;
                        drUserDetails["Email"] = user.Email;
                        drUserDetails["FirstName"] = userProfile.MC_FirstName;
                        drUserDetails["LastName"] = userProfile.MC_LastName;

                        dtUsers.Rows.Add(drUserDetails);
                    }
                }
            }

            return dtUsers;
        }

        #endregion
    }
}
