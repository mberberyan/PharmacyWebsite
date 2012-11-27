using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BusinessLogic
{
    /// <summary>
    /// Summary description for BaseWebPage
    /// </summary>
    public class BaseWebPage : System.Web.UI.Page
    {
        public BaseWebPage()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void InitializeCulture()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = Melon.Components.CMS.Language.CurrentLanguage;
            System.Threading.Thread.CurrentThread.CurrentUICulture = Melon.Components.CMS.Language.CurrentLanguage;
            base.InitializeCulture();
        }

        public void iculture()
        {
            this.InitializeCulture();
        }
    }
}
