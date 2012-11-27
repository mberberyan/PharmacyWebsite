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
using Melon.Components.News.ComponentEngine;
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides interface for listing the popular news translated in the current language in the front-end of the web site. 
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The news list is displayed as links with text the title of the news. 
    ///     These links are to the web page which is configured by the developers in the configuration section of the component in the web config: news/frontEndInterface attribute newsDetailsPageUrl.
    /// </para>
    /// <para>
    /// The language for which will be listed the popular news should be set in property <see cref="NewsListBase.Language"/>.
    /// </para>
    /// <para>
    /// If the popular news from specific category should be listed then set the id of this category in property <see cref="NewsListBase.CategoryId"/>.
    /// </para>
    /// </remarks>
    public partial class PopularNews : MelonUserControl
    {
        #region Fields & Properties

        private int _newsCount = 5;
        /// <summary>
        /// Maximum number of news to list.
        /// </summary>
        public int NewsCount
        {
            get { return _newsCount; }
            set { _newsCount = value; }
        }

        private int? _categoryId = (int?)null;
        /// <summary>
        /// Id of category from which should be the listed news.
        /// </summary>
        /// <remarks>
        /// Set the property to "-1" to list the news without category. 
        /// To list news without considering the category set the property to null. Initially it is set to null.
        ///</remarks>
        public int? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        private CultureInfo _language;
        /// <summary>
        /// Language in which the listed news should be translated.
        /// </summary>
        public CultureInfo Language
        {
            get { return _language; }
            set { _language = value; }
        }

        private bool _allowPaging = false;
        /// <summary>
        /// Flag whether paging is needed for the news list.
        /// </summary>
        public bool AllowPaging
        {
            get { return _allowPaging; }
            set { _allowPaging = value; }
        }

        private int _pageSize = 10;
        /// <summary>
        /// Maximum news to display per page in the news list.
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        #endregion

        /// <summary>
        /// Attaches event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            gvNews.RowDataBound += new GridViewRowEventHandler(gvNews_RowDataBound);
            topPager.PageChanged += new AdminPager.PagerEventHandler(Pager_PageChanged);
            bottomPager.PageChanged += new AdminPager.PagerEventHandler(Pager_PageChanged);

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes the user control.
        /// </summary>
        /// <remarks>
        /// Calls method <see cref="NewsBind()"/> to retrive and display the news list.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            gvNews.AllowPaging = this.AllowPaging;
            if (this.AllowPaging)
            {
                gvNews.PageSize = this.PageSize;
            }
            gvNews.PagerSettings.Visible = false;

            if (!IsControlPostBack)
            {
                NewsBind();
            }
        }

        /// <summary>
        /// Event handler for event RowDataBound of GridView gvNews.
        /// </summary>
        /// <remarks>
        /// Used to hide labels lblPublishedOnTitle and lblBy if there are no set date published and author of the news.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNews_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Date published
                Label lblPublishedOnTitle = (Label)e.Row.FindControl("lblPublishedOnTitle");
                if (((DataRowView)e.Row.DataItem)["DatePosted"] == DBNull.Value)
                {
                    if (lblPublishedOnTitle != null)
                        lblPublishedOnTitle.Visible = false;
                }

                //Author
                Label lblBy = (Label)e.Row.FindControl("lblBy");
                string author = Convert.ToString(((DataRowView)e.Row.DataItem)["Author"]);
                if (String.IsNullOrEmpty(author))
                {
                    if (lblBy != null)
                        lblBy.Visible = false;
                }
            }
        }

        /// <summary>
        /// Event handler for event <see cref="AdminPager.PageChanged"/> of user controls topPager and bottomPager.
        /// </summary>
        /// <remarks>
        /// Set the new page index to GridView gvNews and calls method <see cref="NewsBind"/> to databind the GridView thus displaying in the grid the new page.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_PageChanged(object sender, AdminPager.PagerEventArgs e)
        {
            gvNews.PageIndex = e.NewPage;
            NewsBind();
        }

        /// <summary>
        /// Method to retrieve and display the popular news.
        /// </summary>
        /// <remarks>
        /// The method calls static method <see cref="News.ListPopular"/> to retrive the popular news and display them in 
        /// GridView gvNews.
        /// </remarks>
        public void NewsBind()
        {
            DataTable dtNews = News.ListPopular(this.CategoryId, this.Language, this.NewsCount);

            if (this.AllowPaging)
                gvNews.PageSize = this.PageSize;

            gvNews.DataSource = dtNews;
            gvNews.DataBind();

            if (this.AllowPaging && dtNews.Rows.Count != 0)
            {
                topPager.Visible = true;
                bottomPager.Visible = true;
                topPager.FillPaging(gvNews.PageCount, gvNews.PageIndex + 1, 5, gvNews.PageSize, dtNews.Rows.Count);
                bottomPager.FillPaging(gvNews.PageCount, gvNews.PageIndex + 1, 5, gvNews.PageSize, dtNews.Rows.Count);
            }
            else
            {
                topPager.Visible = false;
                bottomPager.Visible = false;
            }

        }
    }
}

