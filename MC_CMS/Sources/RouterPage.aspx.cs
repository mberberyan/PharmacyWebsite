using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS;


/// <summary>
/// Router page which redirects to cms page in edit or preview mode.
/// </summary>
/// <remarks>
/// It is used to set browse settings defined in <see cref="CMS"/>. 
///</remarks>
public partial class RouterPage:System.Web.UI.Page
{
    /// <summary>
    /// Sets some session variables which will be read from web pages for dynamic displaying placed in folder "DynamicContent".
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Melon.Components.CMS.CMS.IsCMSBrowse = true;

            //Set page version (live, draft) that should be opened.
            NodeVersion version = NodeVersion.Live;
            if (Request["version"] != null)
            {
                if (Convert.ToString(Request["version"]) == "draft")
                {
                    version = NodeVersion.Draft;
                }
            }
            Melon.Components.CMS.CMS.BrowseVersion = version;

         
            //Set mode (edit/preview)
            PageMode mode = PageMode.Preview;
            if (Request["mode"] != null)
            {
                if (Convert.ToString(Request["mode"]) == "edit")
                {
                    mode = PageMode.Edit;
                }
            }
            Melon.Components.CMS.CMS.BrowseMode = mode;

            //Set web site language
            if (Request["lang"] != null)
            {
                if (CMSSettings.WebSiteLanguageProperty != null)
                {
                    CultureInfo language = CultureInfo.GetCultureInfo(Convert.ToString(Request["lang"]));
                    CMSSettings.WebSiteLanguageProperty.SetValue(null, language, null);
                }
            }
   
            //Redirect
            if (Request["url"] != null)
            {
                string url = Request["url"];
                if (url.Contains("ContentManageablePage"))
                {
                    if (url.Contains("?"))
                    {
                        url += "&cmsBrowse=1";
                    }
                    else
                    {
                        url += "?cmsBrowse=1";
                    }
                    Response.Redirect(url);
                }
                else
                {
                    Response.Redirect(Convert.ToString(Request["url"]));
                }
                
                
            }
          
        }
    }
}
