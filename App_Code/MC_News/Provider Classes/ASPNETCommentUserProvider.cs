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
    /// Implements Comment user provider using ASP.NET Membership.
    /// </summary>
    public class ASPNETCommentUserProvider : Melon.Components.News.Providers.CommentUserProvider
    {
        public override CommentUser Load()
        {
            MembershipUser aspnetUser = Membership.GetUser();

            if (aspnetUser != null)
            {
                CommentUser currentUser = new CommentUser();
                currentUser.UserName = aspnetUser.UserName;
                currentUser.NickName = aspnetUser.UserName;
                currentUser.Photo = ((ProfileCommon)HttpContext.Current.Profile).MC_PhotoPath;
                currentUser.CreationDate = aspnetUser.CreationDate;

                return currentUser;
            }
            else
            {
                return null;
            }
        }

        public override CommentUserDataTable List(System.Collections.Generic.List<string> lstNicknames)
        {
            Melon.Components.News.CommentUserDataTable dt = new Melon.Components.News.CommentUserDataTable();

            MembershipUserCollection users = Membership.GetAllUsers();

            foreach (MembershipUser user in users)
            {
                if (lstNicknames.Contains(user.UserName))
                {
                     DataRow drUserDetails = dt.NewRow();
                     drUserDetails["UserName"] = user.UserName;
                     drUserDetails["NickName"] = user.UserName;
                     drUserDetails["CreationDate"] = user.CreationDate;

                     ProfileCommon userProfile = new ProfileCommon();
                     userProfile = userProfile.GetProfile(user.UserName);
                     drUserDetails["PhotoPath"] = userProfile.MC_FirstName;
                    
                     dt.Rows.Add(drUserDetails);
                }
            }

            return dt;
        }
    }
}
