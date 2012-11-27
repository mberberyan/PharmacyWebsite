using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Controls_Header : System.Web.UI.UserControl
{
    /// <summary>
    /// Set styles of the menu items in the administration menu.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tdCMS.Attributes.Add("class", "menu_item_first");
            tdProductCatalog.Attributes.Add("class", "menu_item");
            tdNews.Attributes.Add("class", "menu_item_last");
           

            string pageName = Request.RawUrl.ToLower().Substring(Request.RawUrl.LastIndexOf('/') + 1, Request.RawUrl.LastIndexOf('.') - Request.RawUrl.LastIndexOf('/') - 1);
            switch (pageName)
            {
                case "default":
                    tdCMS.Attributes.Add("class", "menu_item_first_selected");
                    break;
                case "productcatalog":
                    tdProductCatalog.Attributes.Add("class", "menu_item_selected");
                    break;
                case "news":
                    tdNews.Attributes.Add("class", "menu_item_last_selected");
                    break;
            }
        }
    }

    /// <summary>
    /// On logout from admininistration is deleteed the cookie for work with CMS.
    /// If this cookie exists this means that we are working with the CMS and then in the content placeholders are displayed editors for managing content.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cntrlLoginStatus_LoggedOut(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Cookies["CMS"].Expires = DateTime.Now.AddDays(-1d);

        Response.Redirect("~/Default.aspx", true);
    }
}
