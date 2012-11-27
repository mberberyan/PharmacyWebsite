using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Melon.Components.News.ComponentEngine;
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides interface for listing all visible categories for the current language in the front-end of the web site. 
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The list with categories is displayed as links with text the names of the categories. 
    ///     These links are to the web page which is configured by the developers in the configuration section of the component in the web config: news/frontEndInterface attribute newsListPageUrl.
    /// </para>
    /// The language for which will be listed the categories should be set in property <see cref="Language"/>.
    /// </remarks>
    public partial class Categories : MelonUserControl
    {
        #region Fields & Properties

        /// <summary>
        /// Language for which to list news categories.
        /// </summary>
        public CultureInfo Language;

        /// <summary>
        /// Css class applied to currently selected category.
        /// </summary>
        public string SelectedCategoryCssClass;

        #endregion

        /// <summary>
        /// Attaches event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            repCategories.ItemDataBound += new RepeaterItemEventHandler(repCategories_ItemDataBound);

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes the user control.
        /// </summary>
        /// <remarks>
        /// Calls method <see cref="CategoriesBind()"/> to display the categories for the language specified in property <see cref="Language"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                CategoriesBind();
            }
        }

        /// <summary>
        /// Event handler for event ItemDataBound of Repeater repCategories.
        /// </summary>
        /// <remarks>
        ///     Used to set category details in the columns of Repeater repCategories.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (Request.QueryString["cat_id"] == null)
                {
                    HyperLink hplAll = (HyperLink)e.Item.FindControl("hplAllNews");
                    if (hplAll != null)
                    {
                        hplAll.CssClass = SelectedCategoryCssClass;
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int categoryId = Convert.ToInt32(((DataRowView)e.Item.DataItem)["Id"]);
                HyperLink hplCategory = (HyperLink)e.Item.FindControl("hplCategory");
                hplCategory.Text = Server.HtmlEncode(Convert.ToString(((DataRowView)e.Item.DataItem)["Name"]));
                hplCategory.NavigateUrl = NewsSettings.NewsListPagePath + "?cat_id=" + Convert.ToString(categoryId);
                if (Request.QueryString["cat_id"] != null)
                {
                    int? selectedCategoryId = null;
                    try
                    {
                        selectedCategoryId = Convert.ToInt32(Request.QueryString["cat_id"]);
                    }
                    catch
                    {

                    }

                    if ((selectedCategoryId != null) && (selectedCategoryId.Value == categoryId))
                    {
                        hplCategory.CssClass = SelectedCategoryCssClass;
                    }

                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                if (Request.QueryString["cat_id"] != null)
                {
                    int? selectedCategoryId = null;
                    try
                    {
                        selectedCategoryId = Convert.ToInt32(Request.QueryString["cat_id"]);
                    }
                    catch
                    {

                    }
                    if ((selectedCategoryId != null) && (selectedCategoryId.Value == -1))
                    {
                        HyperLink hplNotCategorizedNews = (HyperLink)e.Item.FindControl("hplNotCategorizedNews");
                        if (hplNotCategorizedNews != null)
                        {
                            hplNotCategorizedNews.CssClass = SelectedCategoryCssClass;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays the categories for the language specified in property <see cref="Language"/>.
        /// </summary>
        /// <remarks>
        /// Calls method <see cref="Category.ListTranslatedOnly(Category)"/> to retrieve the categories translated for language <see cref="Language"/>.
        /// The language for which to return the categories is set in the filtering object passed as parameter to the method.
        /// </remarks>
        public void CategoriesBind()
        {
            Category objCategory = new Category();
            objCategory.IsVisible = true;
            objCategory.LanguageCulture = this.Language;

            DataTable dtCategories = Category.ListTranslatedOnly(objCategory);

            repCategories.DataSource = dtCategories;
            repCategories.DataBind();
        }
    }
}
