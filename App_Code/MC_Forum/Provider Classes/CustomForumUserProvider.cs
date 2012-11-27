using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Melon.Components.Forum;

namespace Melon.Components.Forum
{

    /// <summary>
    /// Implementation of abstract class Melon.Components.Forum.ForumUserProvider.
    /// </summary>
    public class CustomForumUserProvider: ForumUserProvider
    {
        public CustomForumUserProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override ForumUser Load()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override ForumUser Load(string username)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override ForumUserDataTable GetAllUsers()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override ForumUserDataTable List(System.Collections.Generic.List<string> userNames, ForumUser filter)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override void Save(ForumUser forumUser)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
    }

}
