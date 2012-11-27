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

public partial class MPNews : System.Web.UI.MasterPage
{
    //public Languages cntrlMasterLanguages
    //{
    //    get { return cntrlLanguages; }
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="e"></param>
    //protected override void OnInit(EventArgs e)
    //{
    //    cntrlLanguages.ChangeLanguage += new ChangeLanguageEventHandler(cntrlLanguages_ChangeLanguage);
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        cntrlNewsRSS.Language = Melon.Components.News.NewsLanguage.CurrentLanguage;
        cntrlCategories.Language = Melon.Components.News.NewsLanguage.CurrentLanguage;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="args"></param>
    //protected void cntrlLanguages_ChangeLanguage(object sender, ChangeLanguageArgs args)
    //{
    //    cntrlNewsRSS.Language = Melon.Components.News.NewsLanguage.CurrentLanguage;
    //    cntrlNewsRSS.Bind();

    //    cntrlCategories.Language = args.NewLanguage;
    //    cntrlCategories.CategoriesBind();
    //}
}
