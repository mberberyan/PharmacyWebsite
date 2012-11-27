using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security;
using System.Web;
using System.Web.Security;
using System.Security.AccessControl;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.Forum;


namespace Melon.Components.Forum
{
    /// <summary>
    /// Implementation of abstract class Melon.Components.Forum.ForumUserProvider using ASPNET Membership.
    /// </summary>
    public class ASPNETForumUserProvider : ForumUserProvider
    {
        #region Fields

        private static string userPhotosFolderPath = ConfigurationManager.AppSettings["mcf_userPhotosFolderPath"];
        private static int userPhotoThumbnailWidth = Convert.ToInt32(ConfigurationManager.AppSettings["mcf_userPhotoThumbnailWidth"]);
        private static int userPhotoThumbnailHeight = Convert.ToInt32(ConfigurationManager.AppSettings["mcf_userPhotoThumbnailHeight"]);

        private static MembershipUserCollection availableUsers = new MembershipUserCollection();
        private static SortedList<Guid, MembershipUser> availableUsersByGuid = new SortedList<Guid, MembershipUser>();

        private static readonly object locker = new object();

        #endregion

        static ASPNETForumUserProvider()
        {
            lock (locker)
            {
                availableUsers = Membership.GetAllUsers();
                foreach (MembershipUser mu in availableUsers)
                {
                    availableUsersByGuid.Add((Guid)mu.ProviderUserKey, mu);
                }
            }
        }

        #region Implemented Methods of ForumUserProvider Class

        public override ForumUser Load()
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


                // Get MembershipUser object by user identifier so as to get membership information from database
                // instead of calling Membership.GetUser()(with no parameters) method which gets current user`s username from HttpContext object.
                if (aspnetUser == null)
                {
                    aspnetUser = Membership.GetUser((Guid?)aspnetUserHttpContext.ProviderUserKey);
                    // The user is found in database, so add it in memory too
                    if (aspnetUser != null)
                    {
                        lock (locker)
                        {
                            availableUsers.Add(aspnetUser);
                            availableUsersByGuid.Add((Guid)aspnetUser.ProviderUserKey, aspnetUser);
                        }
                    }
                }

                if (aspnetUser != null)
                {
                    //Create ForumUser instance with the found user details.
                    ForumUser objForumUser = new ForumUser();

                    objForumUser.UserName = aspnetUser.UserName;
                    objForumUser.Nickname = aspnetUser.UserName;
                    objForumUser.Email = aspnetUser.Email;
                    objForumUser.CreationDate = aspnetUser.CreationDate;
                    objForumUser.FirstName = ((ProfileCommon)HttpContext.Current.Profile).MC_FirstName;
                    objForumUser.LastName = ((ProfileCommon)HttpContext.Current.Profile).MC_LastName;
                    objForumUser.ICQNumber = ((ProfileCommon)HttpContext.Current.Profile).MC_ICQNumber;
                    objForumUser.PhotoPath = ((ProfileCommon)HttpContext.Current.Profile).MC_PhotoPath;
                    objForumUser.IsProfileVisible = ((ProfileCommon)HttpContext.Current.Profile).MC_IsProfileVisible;

                    return objForumUser;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public override ForumUser Load(string username)
        {
            //Get user by username.
            MembershipUser aspnetUser = null;
            try
            {
                aspnetUser = availableUsers[username];
                if (aspnetUser == null)
                {
                    aspnetUser = Membership.GetUser(username);
                    // The user is found in database, so add it in memory too
                    if (aspnetUser != null)
                    {
                        lock (locker)
                        {
                            availableUsers.Add(aspnetUser);
                            availableUsersByGuid.Add((Guid)aspnetUser.ProviderUserKey, aspnetUser);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            if (aspnetUser != null)
            {
                //Create ForumUser instance with the found user details.
                ForumUser objForumUser = new ForumUser();

                ProfileCommon userProfile = new ProfileCommon();
                userProfile = userProfile.GetProfile(aspnetUser.UserName);

                objForumUser.UserName = aspnetUser.UserName;
                objForumUser.Nickname = aspnetUser.UserName;
                objForumUser.Email = aspnetUser.Email;
                objForumUser.CreationDate = aspnetUser.CreationDate;
                if (userProfile != null)
                {
                    objForumUser.FirstName = userProfile.MC_FirstName;
                    objForumUser.LastName = userProfile.MC_LastName;
                    objForumUser.ICQNumber = userProfile.MC_ICQNumber;
                    objForumUser.PhotoPath = userProfile.MC_PhotoPath;
                    objForumUser.IsProfileVisible = userProfile.MC_IsProfileVisible;
                }

                return objForumUser;
            }
            else
            {
                return null;
            }

        }

        public override ForumUserDataTable GetAllUsers()
        {
            ForumUserDataTable foundUsers = new ForumUserDataTable();

            // Good point to refresh users
            lock (locker)
            {
                availableUsers = Membership.GetAllUsers();
                availableUsersByGuid = new SortedList<Guid, MembershipUser>();
                foreach (MembershipUser mu in availableUsers)
                {
                    availableUsersByGuid.Add((Guid)mu.ProviderUserKey, mu);
                }
            }
            MembershipUserCollection users = availableUsers;


            foreach (MembershipUser aspnetUser in users)
            {
                ProfileCommon profile = new ProfileCommon();
                profile = profile.GetProfile(aspnetUser.UserName);

                DataRow dr = foundUsers.NewRow();

                dr["UserName"] = aspnetUser.UserName;
                dr["NickName"] = aspnetUser.UserName;
                dr["Email"] = aspnetUser.Email;
                dr["DateCreated"] = aspnetUser.CreationDate;

                if (profile != null)
                {
                    dr["FirstName"] = profile.MC_FirstName;
                    dr["LastName"] = profile.MC_LastName;
                    dr["ICQNumber"] = profile.MC_ICQNumber;
                    dr["PhotoPath"] = profile.MC_PhotoPath;
                    dr["IsProfileVisible"] = profile.MC_IsProfileVisible;
                }

                foundUsers.Rows.Add(dr);
            }

            return foundUsers;
        }

        public override ForumUserDataTable List(List<string> usernames, ForumUser filter)
        {
            ForumUserDataTable foundUsers = new ForumUserDataTable();

            // Good point to refresh users
            lock (locker)
            {
                availableUsers = Membership.GetAllUsers();
                availableUsersByGuid = new SortedList<Guid, MembershipUser>();
                foreach (MembershipUser mu in availableUsers)
                {
                    availableUsersByGuid.Add((Guid)mu.ProviderUserKey, mu);
                }
            }
            MembershipUserCollection users = availableUsers;

            foreach (MembershipUser aspnetUser in users)
            {
                if (usernames.Contains(aspnetUser.UserName))
                {
                    ProfileCommon profile = new ProfileCommon();
                    profile = profile.GetProfile(aspnetUser.UserName);

                    if (filter != null)
                    {
                        if (((String.IsNullOrEmpty(filter.UserName) || (profile != null && filter.UserName == profile.UserName)))
                               && ((String.IsNullOrEmpty(filter.Nickname) || (profile != null && filter.Nickname == profile.UserName)))
                               && ((String.IsNullOrEmpty(filter.FirstName) || (profile != null && filter.FirstName == profile.MC_FirstName)))
                               && ((String.IsNullOrEmpty(filter.LastName) || (profile != null && filter.FirstName == profile.MC_LastName)))
                               && ((String.IsNullOrEmpty(filter.Email) || (profile != null && filter.Email == aspnetUser.Email)))
                               && ((String.IsNullOrEmpty(filter.ICQNumber) || (profile != null && filter.ICQNumber == profile.MC_ICQNumber)))
                               && ((!filter.IsProfileVisible.HasValue) || (profile != null && filter.IsProfileVisible == profile.MC_IsProfileVisible)))
                        {
                            DataRow dr = foundUsers.NewRow();

                            dr["UserName"] = aspnetUser.UserName;
                            dr["NickName"] = aspnetUser.UserName;
                            dr["Email"] = aspnetUser.Email;
                            dr["DateCreated"] = aspnetUser.CreationDate;

                            if (profile != null)
                            {
                                dr["FirstName"] = profile.MC_FirstName;
                                dr["LastName"] = profile.MC_LastName;
                                dr["ICQNumber"] = profile.MC_ICQNumber;
                                dr["PhotoPath"] = profile.MC_PhotoPath;
                                dr["IsProfileVisible"] = profile.MC_IsProfileVisible;
                            }

                            foundUsers.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = foundUsers.NewRow();

                        dr["UserName"] = aspnetUser.UserName;
                        dr["NickName"] = aspnetUser.UserName;
                        dr["Email"] = aspnetUser.Email;
                        dr["DateCreated"] = aspnetUser.CreationDate;

                        if (profile != null)
                        {
                            dr["FirstName"] = profile.MC_FirstName;
                            dr["LastName"] = profile.MC_LastName;
                            dr["ICQNumber"] = profile.MC_ICQNumber;
                            dr["PhotoPath"] = profile.MC_PhotoPath;
                            dr["IsProfileVisible"] = profile.MC_IsProfileVisible;
                        }

                        foundUsers.Rows.Add(dr);
                    }
                }
            }

            return foundUsers;
        }

        public override void Save(ForumUser forumUser)
        {
            MembershipUser aspnetUser = Membership.GetUser(forumUser.UserName);
            aspnetUser.Email = forumUser.Email;
            Membership.UpdateUser(aspnetUser);

            // Reget the users
            lock (locker)
            {
                availableUsers = Membership.GetAllUsers();
                availableUsersByGuid = new SortedList<Guid, MembershipUser>();
                foreach (MembershipUser mu in availableUsers)
                {
                    availableUsersByGuid.Add((Guid)mu.ProviderUserKey, mu);
                }
            }

            ProfileCommon profile = new ProfileCommon();
            profile = profile.GetProfile(aspnetUser.UserName);
            profile.MC_FirstName = forumUser.FirstName;
            profile.MC_LastName = forumUser.LastName;
            profile.MC_ICQNumber = forumUser.ICQNumber;
            profile.MC_IsProfileVisible = forumUser.IsProfileVisible.HasValue ? forumUser.IsProfileVisible.Value : false;

            if (forumUser.Photo.RemovePreviousPhoto)
            {
                //Remove Photo
                string fileAbsolutePath = HttpContext.Current.Server.MapPath(forumUser.PhotoPath);
                File.Delete(fileAbsolutePath);
                profile.MC_PhotoPath = null;
            }
            else
            {
                if (forumUser.Photo.BinaryInfo.Length != 0)
                {
                    //Upload Photo

                    //Check whether directory where photo should be uploaded exists.
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(userPhotosFolderPath)))
                    {
                        throw new DirectoryNotFoundException();
                    }

                    string fileName = forumUser.UserName + forumUser.Photo.FileExtension;
                    string fileVirtualPath = userPhotosFolderPath + "/" + fileName;
                    string fileAbsolutePath = HttpContext.Current.Server.MapPath(fileVirtualPath);

                    //*** Make Thumbnail ***
                    Bitmap originalPhoto = new Bitmap(new System.IO.MemoryStream(forumUser.Photo.BinaryInfo));

                    int originalWidth = originalPhoto.Width;
                    int originalHeight = originalPhoto.Height;

                    int tempHeight = (userPhotoThumbnailWidth * originalHeight) / originalWidth;
                    int tempWidth;

                    if (tempHeight > userPhotoThumbnailHeight)
                    {
                        tempWidth = (originalWidth * userPhotoThumbnailHeight) / originalHeight;
                        tempHeight = userPhotoThumbnailHeight;
                    }
                    else
                    {
                        tempWidth = userPhotoThumbnailWidth;
                    }

                    //Save Thumbnail
                    Bitmap thumbPhoto = new Bitmap(originalPhoto, tempWidth, tempHeight);
                    originalPhoto.Dispose();

                    thumbPhoto.Save(fileAbsolutePath);
                    originalPhoto.Dispose();

                    profile.MC_PhotoPath = fileVirtualPath;
                }
            }

            profile.Save();

        }

        #endregion

        
    }
}