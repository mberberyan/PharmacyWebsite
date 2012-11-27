using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.IO;


public partial class FCKApplyStyles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string virtualStylesPath = "~/App_Themes/Default/styles.css";
        string content = File.ReadAllText(Server.MapPath(virtualStylesPath));
        content = content.Replace("url('images", "url('" + ResolveUrl("~/App_Themes/Default/images"));
        Response.Write(content);
    }
}
