using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using Melon.Components.CMS.Configuration;

namespace Melon.Components.CMS.UI.CodeBehind
{
    /// <summary>
    /// Web page to which redirects the router page to preview static external page.
    /// </summary>
    public partial class StaticExternalPage : System.Web.UI.Page
    {
        /// <summary>
        /// Calls method <see cref="DisplayContent"/> to preview external page.
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
        /// Redirects to external page.
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
                Params.Add(isLiveVersion);

                DataRow dr = DataAccess.ExecuteDataRow("MC_CMS_DynamicStaticExternalPageGet", Params);

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
