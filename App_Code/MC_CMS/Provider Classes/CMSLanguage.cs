using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace Melon.Components.CMS
{
    public static class Language
    {
        //Stores current UI language on the web site.
        public static CultureInfo CurrentLanguage
        {
            get
            {
                CultureInfo lang = System.Threading.Thread.CurrentThread.CurrentUICulture;
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session["lang"] != null))
                {
                    lang = CultureInfo.GetCultureInfo(Convert.ToString(HttpContext.Current.Session["lang"]));
                }
                return lang;
            }
            set
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = (CultureInfo)value;
                HttpContext.Current.Session["lang"] = ((CultureInfo)value).Name;
            }
        }
    }
}
