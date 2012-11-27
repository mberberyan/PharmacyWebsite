using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using Melon.Components.CMS.Configuration;

namespace Melon.Components.CMS.UI.CodeBehind
{
    /// <summary>
    /// Web page to which redirects the router page to preview static menu page.
    /// </summary>
    public partial class StaticMenuPage : System.Web.UI.Page
    {
        /// <summary>
        /// Calls method <see cref="DisplayContent"/> to preview menu page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Melon.Components.CMS.CMS.BrowseMode == PageMode.Preview)
                {
                    DisplayContent();
                }
            }
        }

        /// <summary>
        /// Redirects to menu page.
        /// </summary>
        private void DisplayContent()
        {
            int? id = (Request["pageId"] != null) ? Convert.ToInt32(Request["pageId"]) : (int?)null;
            if (id.HasValue)
            {
                string languageName;
                if (CMSSettings.WebSiteLanguageProperty != null)
                {
                    languageName = ((CultureInfo)CMSSettings.WebSiteLanguageProperty.GetValue(null, null)).Name;
                }
                else
                {
                    languageName = CMSSettings.DefaultLanguage.Name;
                }
                bool isLiveVersion = (Melon.Components.CMS.CMS.BrowseVersion == NodeVersion.Live);

                ArrayList Params = new ArrayList();
                Params.Add(id);
                Params.Add(languageName);
                Params.Add(CMSSettings.DefaultLanguage.Name);
                Params.Add(CMSSettings.DynamicContentManageablePagePath);
                Params.Add(isLiveVersion);

                DataRow dr = DataAccess.ExecuteDataRow("MC_CMS_DynamicStaticMenuPageGet", Params);

                if (!dr.IsNull("Title"))
                {
                    Page.Title = Convert.ToString(dr["Title"]);
                }

                if (!dr.IsNull("Url"))
                {
                    Response.Redirect(Convert.ToString(dr["Url"]));
                }
            }
        }
    }
}
