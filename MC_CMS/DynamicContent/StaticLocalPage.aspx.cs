using System;
using System.Collections.Generic;
using System.Text;

namespace Melon.Components.CMS.UI.CodeBehind
{
    /// <summary>
    /// Web page to which redirects the router page to preview static local page.
    /// </summary>
    /// <remarks>
    /// Router page do not redirect directly to local page because of the following problem:
    /// <para>
    /// When content-manageable page is opened for edit 
    /// and the user clicks on local page in the draft menu displayed in the master page of the content-manageable page
    /// the iframe will redirect to this local page while the content-manageable page is still selected in the cms tree.
    /// </para>
    /// </remarks>
    public partial class StaticLocalPage : System.Web.UI.Page
    {
        /// <summary>
        /// Redirects to the local page which should be previewed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Melon.Components.CMS.CMS.BrowseMode == PageMode.Preview)
                {
                    int? id = (Request["pageId"] != null) ? Convert.ToInt32(Request["pageId"]) : (int?)null;
                    if (id.HasValue)
                    {
						string localPagePath = Convert.ToString(Request["localPagePath"]).ToLower();
						if (localPagePath.EndsWith(".doc") || localPagePath.EndsWith(".pdf") || localPagePath.EndsWith(".xml"))
						{
							Response.Redirect(Convert.ToString(Request["localPagePath"]));
						}
						else
						{
							Server.Transfer(Convert.ToString(Request["localPagePath"] + "?"));
						}
                    }
                }
            }
        }
    }
}
