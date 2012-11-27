using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Web page which is used to preview the news as it will look like in the front-end of the web site.
    /// </summary>
    /// <remarks>
    /// In this web page is loaded user control "NewsDetails.ascx". The control is loaded in preview mode and thus linked news, comments, link back are not displayed.
    /// </remarks>
    public partial class AdminNewsPreview : System.Web.UI.Page
    {
        /// <summary>
        /// Initializes the user control.
        /// </summary>
        /// <remarks>
        /// Sets properties CategoryId, NewsId, Language, PreviewMode of the NewsDetails control.
        /// <para>
        /// Properties CategoryId, NewsId, Language are passed by the query string.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cat_id"] != null)
            {
                cntrlNewsPreview.CategoryId = Convert.ToInt32(Request.QueryString["cat_id"]);
            }
            cntrlNewsPreview.NewsId = Convert.ToInt32(Request.QueryString["news_id"]);
            cntrlNewsPreview.Language = CultureInfo.GetCultureInfo(Convert.ToString(Request.QueryString["lang"]));
            cntrlNewsPreview.PreviewMode = true;
        }
    }
}
