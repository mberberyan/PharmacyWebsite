using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides RSS feed for the web site news.
    /// </summary>
    public partial class RSS : System.Web.UI.Page
    {
        /// <summary>
        /// Initializes the web page.
        /// </summary>
        /// <remarks>
        /// Calls method <see cref="BuildRssString"/> to buold the rss feed of the web site news and render it as content of the page. 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["lang"] != null) //It should be name of .NET culture
            {
                string language = Convert.ToString(Request["lang"]);
                CultureInfo culture = null;
                try
                {
                    culture = CultureInfo.GetCultureInfo(language);
                }
                catch
                {
                    return;
                }

                if (culture != null)
                {
                    Response.ContentType = "text/xml";
                    Response.ContentEncoding = Encoding.UTF8;

                    string rss = BuildRssString(culture);

                    Response.Write(rss);
                    Response.End();
                }
            }
        }

        /// <summary>
        /// Returns xml string in RSS format for the news translated in the language specified in <paramref name="culture"/>.
        /// </summary>
        /// <param name="culture">Language for which to retrive the news and build RSS.</param>
        /// <returns></returns>
        private string BuildRssString(CultureInfo culture) 
        {
            CultureInfo websiteCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            string title = Convert.ToString(GetLocalResourceObject("ChannelTitle"));
            string description = Convert.ToString(GetLocalResourceObject("ChannelDescription"));

            System.Threading.Thread.CurrentThread.CurrentUICulture = websiteCulture;

            string siteURL = GetWebSiteURL();
            string ttl = "60"; 

            StringBuilder strBuilder = new StringBuilder(); 
            strBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>"); 
            strBuilder.Append("<rss version=\"2.0\"><channel>"); 

            strBuilder.Append("<title>");
            strBuilder.Append(title); 
            strBuilder.Append("</title>"); 

            strBuilder.Append("<link>");
            strBuilder.Append(siteURL); 
            strBuilder.Append("</link>"); 
            
            strBuilder.Append("<description>"); 
            strBuilder.Append(description); 
            strBuilder.Append("</description>"); 
            
            strBuilder.Append("<ttl>"); 
            strBuilder.Append(ttl); 
            strBuilder.Append("</ttl>");

            AppendItems(strBuilder, culture); 
            strBuilder.Append("</channel></rss>");
            
            return strBuilder.ToString(); 
        
        } 

        /// <summary>
        /// Builds rss items collection for the tranlsated news and appends them to the main RSS string.
        /// </summary>
        /// <param name="strBuilder"></param>
        /// <param name="culture"></param>
        private void AppendItems(StringBuilder strBuilder,CultureInfo culture) 
        {
            DataTable dtNews = News.List(null, culture);

            foreach( DataRow dr in dtNews.Rows )  
            {
                string category = Server.HtmlEncode(Convert.ToString(dr["CategoryName"]));
                string title = Server.HtmlEncode(Convert.ToString(dr["Title"]));
                string link = ResolveUrl(NewsSettings.NewsDetailsPagePath) + "?news_id=" + Convert.ToString(dr["Id"]);
                string description = Server.HtmlEncode( Convert.ToString(dr["Summary"]));
                string date = Convert.ToDateTime(dr["DatePosted"]).ToString("R");    //RFC1123Pattern 
                string author = Server.HtmlEncode(Convert.ToString(dr["Author"]));
 
                strBuilder.Append("<item>");

                strBuilder.Append("<category>");
                strBuilder.Append(category);
                strBuilder.Append("</category>"); 

                strBuilder.Append("<title>");        
                strBuilder.Append(  title);         
                strBuilder.Append("</title>");     
     
                strBuilder.Append("<link>");
                strBuilder.Append(link);         
                strBuilder.Append("</link>"); 
         
                strBuilder.Append("<guid isPermaLink=\"true\">");
                strBuilder.Append(link);         
                strBuilder.Append("</guid>");

                strBuilder.Append("<description>");
                strBuilder.Append(description);
                strBuilder.Append("</description>");   
       
                strBuilder.Append("<pubDate>");
                strBuilder.Append(date);         
                strBuilder.Append("</pubDate>");

                strBuilder.Append("<author>");
                strBuilder.Append(author);
                strBuilder.Append("</author>"); 

                strBuilder.Append("</item>");       
            } 
        }

        /// <summary>
        /// Returns the url of the current web site.
        /// </summary>
        /// <returns>url string</returns>
        private string GetWebSiteURL()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "") + HttpContext.Current.Request.ApplicationPath;
        }
    }
}
