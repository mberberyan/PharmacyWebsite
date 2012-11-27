using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MC_News_Examples_NewsDetails : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        //((Languages)((MPNews)this.Master).cntrlMasterLanguages).ChangeLanguage += new ChangeLanguageEventHandler(News_ChangeLanguage);
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cntrlNewsDetails.CategoryId = (Request.QueryString["cat_id"] == null) ? (int?)null : Convert.ToInt32(Request.QueryString["cat_id"]);
            cntrlNewsDetails.NewsId = Convert.ToInt32(Request.QueryString["news_id"]);
        }
        catch (FormatException)
        {
            Response.Redirect("ErrorPage.aspx", true);
        }
        cntrlNewsDetails.Language = Melon.Components.News.NewsLanguage.CurrentLanguage;
    }

    //protected void News_ChangeLanguage(object sender, ChangeLanguageArgs args)
    //{
    //    cntrlNewsDetails.Language = args.NewLanguage;
    //    cntrlNewsDetails.NewsDetailsBind();
    //}
}
