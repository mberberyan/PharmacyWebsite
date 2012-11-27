using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.News.ComponentEngine;
using Melon.Components.News.Configuration;
using Melon.Components.News.Exception;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides interface for managing news.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The AdminNewsList user control contains search area where could be entered search criteria. The criteria are devided into simple and advanced.
    ///     Tha advanced criteria are initially collaped and only if user need more specific search he can expand the advanced criteria and use them.
    /// </para>
    ///	<para>
    ///		The GridView gvNews has the following columns:
    ///			<list type="table">
    ///				<item>
    ///                 <term>Id</term>
    ///                 <description>Id of the news.</description>
    ///             </item>
    ///             <item>
    ///                 <term>Title</term>
    ///                 <description>Title of the news translated on the current language. If it is not translated "NA" message is displayed.</description>
    ///             </item>
    ///             <item>
    ///                 <term>Sub-Title</term>
    ///                 <description>Sub-Title of the news translated on the current language. If it is not translated "NA" message is displayed.</description>
    ///             </item>
    ///             <item>
    ///                 <term>Category</term>
    ///                 <description>Name of category in which the news is grouped translated on the current language. If it is not translated "NA" message is displayed.</description>
    ///             </item>
    ///             <item>
    ///                 <term>Author</term>
    ///                 <description>Name of news author translated on the current language. If it is not translated "NA" message is displayed.</description>
    ///             </item>
    ///             <item>
    ///                 <term>Published On</term>
    ///                 <description>Date of news publication.</description>
    ///             </item>
    ///             <item>
    ///                 <term>Featured</term>
    ///                 <description>Flag whether the news is marked as featured.</description>
    ///             </item>
    ///             <item>
    ///                 <term>Is Approved</term>
    ///                 <description>Link button for approving/disapproving the news.</description>
    ///             </item>
    ///				<item>
    ///                 <term>Column with actions</term>
    ///                 <description>Actions preview, edit, delete are available.If the news is not tranlated on the current language then the preview action is not available.
    ///                 Edit and Delete actions are available only for administrators and writers which are owners of the news.</description>
    ///             </item>
    ///			</list>
    ///	</para>
    ///  <para>
    ///     The results in the GridView could be sorted. When sorting is performed icon appears in the header 
    ///     of the sorted column which indicate what is the sorting direction: ascending or descending.
    /// </para>
    ///     <para>
    ///     There is paging in of the results in the GridView. For the purpose is used Pager control at the top of the GridView.
    ///     The number of the results to be displayed on page is configurable from configuration section &lt;news /&gt; sub-section &lt;backEndInterface /&gt; attribute newsPageSize in the web.config.
    /// </para>
    /// <para>
    ///     All web controls from user control AdminNewsList.ascx are using local resources.
    ///     To customize them modify resource file AdminNewsList.ascx.resx located in folder "MC_News/Sources/App_LocalResources".
    /// </para>
    /// </remarks>
    /// <seealso cref="Melon.Components.News.News"/>
    /// <seealso cref="Melon.Components.News.NewsSearchCriteria"/>
    public partial class AdminNewsList : NewsControl
    {
        #region Fields & Properties

        /// <summary>
        /// Stores the criteria used to search news.
        /// </summary>
        public NewsSearchCriteria SearchCriteria
        {
            get
            {
                if (ViewState["__mc_news_search_criteria"] != null)
                {
                    return (NewsSearchCriteria)ViewState["__mc_news_search_criteria"];
                }
                else
                {
                    return new NewsSearchCriteria();
                }
            }
            set
            {
                ViewState["__mc_news_search_criteria"] = value;
            }
        }
       
        /// <summary>
        /// Sort direction of the currently sorted column in the GridView gvNews.
        /// It is "ASC" for ascending sorting and "DESC" for descending sorting. 
        /// </summary>
        public string SortDirection
        {
            get
            {
                if (ViewState["__mc_news_sortDirection"] != null)
                {
                    return ViewState["__mc_news_sortDirection"].ToString();
                }
                else
                {
                    return "DESC";
                }
            }
            set
            {
                ViewState["__mc_news_sortDirection"] = value;
            }
        }

        /// <summary>
        /// Sort expression of the currently sorted column in the GridView gvNews.
        /// </summary>
        public string SortExpression
        {
            get
            {
                if (ViewState["__mc_news_sortExpression"] != null)
                {
                    return ViewState["__mc_news_sortExpression"].ToString();
                }
                else
                {
                    return "Id";
                }
            }
            set
            {
                ViewState["__mc_news_sortExpression"] = value;
            }
        }

        /// <summary>
        /// Flag whether the user control is loaded as popup for searching news to link to a specific news.
        /// </summary>
        /// <remarks>
        /// If the user conrol is loaded in a popup window for searching news to link to a specific news then 
        /// actions Approve/Disapprove, Edit, Delete are not available in GridView gvNews. The control can be used 
        /// only to link news not to manage them.
        /// </remarks>
        public bool IsLoadedAsLinkNewsPopUp = false;

        /// <summary>
        /// Id of news to which to add linked news.
        /// </summary>
        public int NewsId;

        /// <summary>
        /// The index of the page from GridView gvNews which to be opened.
        /// </summary>
        public int PageIndex = 0;

        /// <summary>
        /// Id of news which to be found in GridView gvNews and 
        /// based on its position to be be estimated <see cref="PageIndex"/>.
        /// </summary>
        public int? NewsIdToPositionOn;

        #endregion

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.SearchCriteria = (NewsSearchCriteria)args[0];
            this.SortExpression = Convert.ToString(args[1]);
            this.SortDirection = Convert.ToString(args[2]);
            this.NewsIdToPositionOn = (int?)args[3];
        }

        /// <summary>
        /// Attaches event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.cntrlLanguages.ChangeLanguage += new ChangeLanguageEventHandler(cntrlLanguages_ChangeLanguage);

            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnAddNews.Click += new EventHandler(btnAddNews_Click);
            this.btnDeleteExpiredNews.Click += new EventHandler(btnDeleteExpiredNews_Click);
            this.btnLinkNews.Click += new EventHandler(btnLinkNews_Click);

            this.gvNews.RowCreated += new GridViewRowEventHandler(gvNews_RowCreated);
            this.gvNews.RowDataBound += new GridViewRowEventHandler(gvNews_RowDataBound);
            this.gvNews.Sorting += new GridViewSortEventHandler(gvNews_Sorting);

            this.TopPager.PageChanged += new AdminPager.PagerEventHandler(TopPager_PageChanged);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     If the user control is loaded for first time method <see cref="PerformSearch"/> is called to 
        ///     search for news corresponding to the search critera and display them in GridView gvNews.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Regsiter script for initial collase/expand of advanced search area.
            string script = @"<script language='javascript' type='text/javascript'>
                if (typeof(hfAdvancedSearchStatusID)!= 'undefined')
                {
                    if (document.getElementById(hfAdvancedSearchStatusID).value == 'collapsed')
                    {
                        document.getElementById('lnkAdvancedSearch').className = 'mc_news_lnk_expand';
                        document.getElementById('divAdvancedSearch').style.display = 'none';
                    }
                    else
                    {
                        document.getElementById('lnkAdvancedSearch').className = 'mc_news_lnk_collapse';
                        document.getElementById('divAdvancedSearch').style.display = '';
                    }
                }
                </script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CollaspseExpand", script, false);

            lblMessage.Visible = false;
            lblErrorMessage.Visible = false;

            if (!IsControlPostBack)
            {
                cntrlLanguages.SelectedLanguage = this.ParentControl.CurrentLanguage;

                //Initialize "Search Area"
                ibtnOpenCalendarFrom.DataBind();
                ibtnOpenCalendarTo.DataBind();
                hfAdvancedSearchStatus.Value = "collapsed";
                
                ListCategories();

                btnDeleteExpiredNews.DataBind();
                btnClosePreviewNews.DataBind();

                if (IsLoadedAsLinkNewsPopUp)
                {
                    lblManageNews.Text = Convert.ToString(GetLocalResourceObject("SearchNews"));
                    gvNews.Columns[0].Visible = true;
                    gvNews.Columns[0].HeaderStyle.CssClass = gvNews.Columns[0].HeaderStyle.CssClass + " mc_news_newslist_header_first_column";
                    gvNews.Columns[gvNews.Columns.Count - 1].Visible = false;//Hide Edit/Delete
                    btnAddNews.Visible = false;
                    btnDeleteExpiredNews.Visible = false;
                    btnLinkNews.Visible = true;
                }
                else
                {
                    lblManageNews.Text = Convert.ToString(GetLocalResourceObject("ManageNews"));
                    gvNews.Columns[0].Visible = false;
                    gvNews.Columns[1].HeaderStyle.CssClass = gvNews.Columns[1].HeaderStyle.CssClass + " mc_news_newslist_header_first_column";
                    gvNews.Columns[gvNews.Columns.Count - 1].Visible = true;//Show Edit/Delete
                    UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                    if (currentUserRole == UserRole.Administrator)
                    {
                        btnDeleteExpiredNews.Visible = true;
                    }
                    else
                    {
                        btnDeleteExpiredNews.Visible = false;
                    }
                    btnAddNews.Visible = true;
                    btnLinkNews.Visible = false;

                    PopulateSearchCriteria(this.SearchCriteria); //Needed if we return from News Edit.
                    //Then we populate the search criteria of the last search before going to edit item.

                    PerformSearch(SearchCriteria,this.NewsIdToPositionOn);
                }
            }
        }


        /// <summary>
        /// Event handler for event Click of Button btnSearch.
        /// </summary>
        /// <remarks>
        ///     The methods calls method <see cref="CollectSearchCriteria"/> to collect the entered search criteria
        ///     and then method <see cref="PerformSearch"/> is called to 
        ///     search for news corresponding to the search critera and display them in GridView gvNews.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                SearchCriteria = CollectSearchCriteria();
                PerformSearch(SearchCriteria, null);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }

        }


        /// <summary>
        /// Event handler for event RowCreated of GridView gvNews.
        /// </summary>
        /// <remarks>
        ///     Attaches event handlers to buttons lbtnApproveDisapprove,lbtnEdit,lbtnDelete in the gridview.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNews_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnApproveDisapprove = (LinkButton)e.Row.FindControl("lbtnApproveDisapprove");
                lbtnApproveDisapprove.Command += new CommandEventHandler(lbtnApproveDisapprove_Command);

                LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                lbtnEdit.Command += new CommandEventHandler(lbtnEdit_Command);

                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                lbtnDelete.Command += new CommandEventHandler(lbtnDelete_Command);
            }
        }

        /// <summary>
        /// Event handler for event RowDataBound of GridView gvNews.
        /// </summary>
        /// <remarks>
        ///     Used to set news's details in the columns of GridView gvNews.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNews_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                //language independent settings
                Label lblId = (Label)e.Row.FindControl("lblId");
                lblId.Text = Convert.ToString(drv["Id"]);

                Label lblIsFeatured = (Label)e.Row.FindControl("lblIsFeatured");
                if (Convert.ToBoolean(drv["IsFeatured"]))
                {
                    lblIsFeatured.Text = Convert.ToString(GetLocalResourceObject("Yes"));
                }
                else
                {
                    lblIsFeatured.Text = Convert.ToString(GetLocalResourceObject("No"));
                }

                Label lblCategory = (Label)e.Row.FindControl("lblCategory");
                string category = Convert.ToString(drv["CategoryName"]);
                if (drv["CategoryName"] == DBNull.Value && drv["CategoryId"] != DBNull.Value) 
                {
                    lblCategory.Text = Convert.ToString(GetLocalResourceObject("NA"));
                    lblCategory.CssClass = "mc_news_lblNotTranslated";
                }
                else
                {
                    lblCategory.Text = Server.HtmlEncode(category);
                }

                //language dependent settings
                Label lblTitle = (Label)e.Row.FindControl("lblTitle");
                Label lblSubTitle = (Label)e.Row.FindControl("lblSubTitle");
                Label lblAuthor = (Label)e.Row.FindControl("lblAuthor");

                if (drv["IdofChildTable"] == DBNull.Value)
                {
                    string strNotAvailable = Convert.ToString(GetLocalResourceObject("NA"));
                    lblTitle.Text = strNotAvailable;
                    lblSubTitle.Text = strNotAvailable;
                    lblAuthor.Text = strNotAvailable;

                    lblTitle.CssClass = "mc_news_lblNotTranslated";
                    lblSubTitle.CssClass = "mc_news_lblNotTranslated";
                    lblAuthor.CssClass = "mc_news_lblNotTranslated";
                }
                else
                {
                    lblTitle.Text = Server.HtmlEncode(Convert.ToString(drv["Title"]));
                    lblSubTitle.Text = Server.HtmlEncode(Convert.ToString(drv["SubTitle"]));
                    lblAuthor.Text = Server.HtmlEncode(Convert.ToString(drv["Author"]));
                }

                //Actions

                //Preview
                Label lblDisabledLnkPreview = (Label)e.Row.FindControl("lblDisabledLnkPreview");
                LinkButton lbtnPreview = (LinkButton)e.Row.FindControl("lbtnPreview");
                if (drv["IdofChildTable"] != DBNull.Value)
                {
                    lblDisabledLnkPreview.Visible = false;
                    lbtnPreview.Visible = true;
                    string previewUrl = NewsSettings.newsPreviewPageUrl + "?" + ((drv["CategoryId"] != DBNull.Value) ? "cat_id=" + Convert.ToString(drv["CategoryId"]) + "&" : "");
                    previewUrl += "news_id=" + Convert.ToString(drv["Id"]) + "&lang=" + this.ParentControl.CurrentLanguage.Name;

                    lbtnPreview.OnClientClick = @"document.getElementById('frameNewsPreview').src='" + ResolveUrl(previewUrl) + @"';
                                              document.getElementById('" + btnFireOpenPreview.ClientID + "').click();return false;";
                }
                else
                {
                    lblDisabledLnkPreview.Visible = true;
                    lbtnPreview.Visible = false;
                }

                LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                Label lblDisabledLnkEdit = (Label)e.Row.FindControl("lblDisabledLnkEdit");
                Label lblDisabledLnkDelete = (Label)e.Row.FindControl("lblDisabledLnkDelete");

                lbtnEdit.CommandArgument = Convert.ToString(drv["Id"]);
                lbtnDelete.CommandArgument = Convert.ToString(drv["Id"]);

                //Approve/Disapprove
                Label lblIsApproved = (Label)e.Row.FindControl("lblIsApproved");
                HtmlGenericControl divIsApprovedButton = (HtmlGenericControl)e.Row.FindControl("divIsApprovedButton");
                if (this.ParentControl.CurrentUser.IsInNewsUserRole(UserRole.Administrator) && !IsLoadedAsLinkNewsPopUp)
                {
                    //Current user is Administrtor - he could do everything.
                    lblIsApproved.Visible = false;
                    divIsApprovedButton.Visible = true;
                    LinkButton lbtnApproveDisapprove = (LinkButton)e.Row.FindControl("lbtnApproveDisapprove");
                    if (Convert.ToBoolean(drv["IsApproved"]))
                    {
                        lbtnApproveDisapprove.Text = Convert.ToString(GetLocalResourceObject("Disapprove"));
                        lbtnApproveDisapprove.CommandArgument = "0;" + Convert.ToString(drv["Id"]);
                        lbtnApproveDisapprove.OnClientClick = "return ConfirmAction('" + Convert.ToString(this.GetLocalResourceObject("DisapproveNewsConfirmMessage")) + "');";
                    }
                    else
                    {
                        lbtnApproveDisapprove.Text = Convert.ToString(GetLocalResourceObject("Approve"));
                        lbtnApproveDisapprove.CommandArgument = "1;" + Convert.ToString(drv["Id"]);
                        lbtnApproveDisapprove.OnClientClick = "return ConfirmAction('" + Convert.ToString(this.GetLocalResourceObject("ApproveNewsConfirmMessage")) + "');";
                    }

                    lbtnEdit.Visible = true;
                    lbtnDelete.Visible = true;
                    lblDisabledLnkEdit.Visible = false;
                    lblDisabledLnkDelete.Visible = false;
                }
                else
                {
                    //Current user is Writer - he could NOT Approve/Disaprove.
                    lblIsApproved.Visible = true;
                    divIsApprovedButton.Visible = false;

                    //The Writer could edit/delete only news which he wrote and are not approved.
                    if (Convert.ToBoolean(drv["IsApproved"]))
                    {
                        lblIsApproved.Text = Convert.ToString(GetLocalResourceObject("Yes"));
                        lbtnEdit.Visible = false;
                        lbtnDelete.Visible = false;
                        lblDisabledLnkEdit.Visible = true;
                        lblDisabledLnkDelete.Visible = true;
                    }
                    else
                    {
                        lblIsApproved.Text = Convert.ToString(GetLocalResourceObject("No"));
                        int? authorId = (drv["CreatedBy"] == null) ? (int?)null : Convert.ToInt32(drv["CreatedBy"]);
                        if (authorId.HasValue && authorId.Value == this.ParentControl.CurrentUser.AdapterId.Value)
                        {
                            //The current logged Writer wrote this news
                            lbtnEdit.Visible = true;
                            lbtnDelete.Visible = true;
                            lblDisabledLnkEdit.Visible = false;
                            lblDisabledLnkDelete.Visible = false;
                        }
                        else
                        {
                            lbtnEdit.Visible = false;
                            lbtnDelete.Visible = false;
                            lblDisabledLnkEdit.Visible = true;
                            lblDisabledLnkDelete.Visible = true;
                        }
                    }
                  
                }
             
            }
        }

        /// <summary>
        /// Event handler for event Sorting of GridView gvNews.
        /// </summary>
        /// <remarks>
        ///     Save in view state the new sorting direction and expression 
        ///     and then calls method <see cref="PerformSearch"/> to perform the sorting.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNews_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                string newSortExpression = e.SortExpression;
                if (this.SortExpression == newSortExpression)
                {
                    //If the old sort expression is the same as the new sort expression, we invert the sort direction.
                    this.SortDirection = (this.SortDirection == "ASC") ? "DESC" : "ASC";
                }
                else
                {
                    //We sort by new column, so set the sorting direction to be acsending.
                    this.SortExpression = newSortExpression;
                    this.SortDirection = "ASC";
                }

                PerformSearch(SearchCriteria,null);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event PageChange for user control AdminPager.ascx.
        /// </summary>
        /// <remarks>
        ///     Set property PageIndex of GridView gvNews to the new page number and then 
        ///     calls method <see cref="PerformSearch"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopPager_PageChanged(object sender, AdminPager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                this.PageIndex = e.NewPage;
                PerformSearch(SearchCriteria,null);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event Command of LinkButton lbtnApproveDisapprove.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Calls method <see cref="News.Approve(int)"/> or <see cref="News.Disapprove(int)"/> to approve/disapprove the news displayed in gvNews's row where the button is placed.
        /// </para>
        /// <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise method <see cref="PerformSearch"/> is called to refresh the data in GridView gvNews.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnApproveDisapprove_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    string[] args = Convert.ToString(e.CommandArgument).Split(';');
                    bool doApprove = Convert.ToBoolean(Convert.ToInt32(args[0]));
                    int newsId = Convert.ToInt32(args[1]);

                    try
                    {
                        if (doApprove)
                        {
                            News.Approve(newsId);
                        }
                        else
                        {
                            News.Disapprove(newsId);
                        }
                       
                    }
                    catch (NewsException ex)
                    {
                        switch (ex.Code)
                        {
                            case NewsExceptionCode.ApproveDeletedNews:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("ApproveDeletedNews").ToString(), false));

                                this.PageIndex = gvNews.PageIndex;
                                PerformSearch(SearchCriteria,null);
                                return;
                            case NewsExceptionCode.DispproveDeletedNews:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("DisapproveDeletedNews").ToString(), false));

                                this.PageIndex = gvNews.PageIndex;
                                PerformSearch(SearchCriteria, null);
                                return;
                        }

                    }
                    catch
                    {
                        if (doApprove)
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("NewsApproveErrorMessage").ToString(), newsId), false));
                        }
                        else
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("NewsDisapproveErrorMessage").ToString(), newsId), false));
                        }
                        return;
                    }

                    //Successful approve/disapprove of news =>Refresh the grid with news.
                    this.PageIndex = gvNews.PageIndex;
                    PerformSearch(SearchCriteria,null);
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
                }
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Command of LinkButton lbtnEdit.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Event <see cref="BaseNewsControl.LoadNewsAddEditEvent"/> of the parent control is raized for loading form for modifying the news displayed in gvComments's row where the button is placed.
        /// </para>
        /// <para>
        ///     If the news has been deleted by another user already 
        ///     event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up).
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEdit_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole != UserRole.None)
                {
                    int newsId = Convert.ToInt32(e.CommandArgument);

                    if (currentUserRole == UserRole.Writer)
                    {
                        News objNews = News.Load(newsId, this.ParentControl.CurrentLanguage);
                        if ((objNews != null) && objNews.IsApproved.Value)
                        {
                            //Writer can not edit approved news.
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("WriterEditApprovedNewsErrorMessage").ToString(), false));
                            this.PageIndex = gvNews.PageIndex;
                            PerformSearch(SearchCriteria, null);
                            return;
                        }
                    }

                    if (News.Exists(newsId))
                    {
                        LoadNewsAddEditEventArgs args = new LoadNewsAddEditEventArgs();
                        args.NewsId = newsId;

                        LoadNewsListEventArgs listArgs = new LoadNewsListEventArgs();
                        listArgs.NewsIdToPositionOn = newsId;
                        listArgs.SortExpression = this.SortExpression;
                        listArgs.SortDirection = this.SortDirection;
                        listArgs.SearchCriteria = this.SearchCriteria;

                        args.NewsListSettings = listArgs;
                        this.ParentControl.OnLoadNewsAddEditEvent(sender, args);
                    }
                    else
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("EditDeletedNews").ToString(), false));
                        this.PageIndex = gvNews.PageIndex;
                        PerformSearch(SearchCriteria,null);
                    }
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
                }
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Command of LinkButton lbtnDelete.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Calls method <see cref="News.Delete"/> to delete the news displayed in gvNews's row where the button is placed.
        ///</para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise method <see cref="PerformSearch"/> is called to refresh the data in GridView gvNews.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole != UserRole.None)
                {
                    int newsId = Convert.ToInt32(e.CommandArgument);

                    if (currentUserRole == UserRole.Writer)
                    {
                        News objNews = News.Load(newsId, this.ParentControl.CurrentLanguage);
                        if ((objNews != null) && objNews.IsApproved.Value)
                        {
                            //Writer can not delete approved news.
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("WriterDeleteApprovedNewsErrorMessage").ToString(), false));
                            this.PageIndex = gvNews.PageIndex;
                            PerformSearch(SearchCriteria, null);
                            return;
                        }
                    }

                    try
                    {
                        News.Delete(newsId);
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("NewsDeleteErrorMessage").ToString(), newsId), false));
                        return;
                    }

                    //Successful deletion of news =>Refresh the grid with news.
                    this.PageIndex = gvNews.PageIndex;
                    PerformSearch(SearchCriteria,null);
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
                }
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event Click of Button btnAddNews.
        /// </summary>
        /// <remarks>
        ///     Raises event <see cref="BaseNewsControl.LoadNewsAddEditEvent"/> of the parent user control for loading 
        ///     user control "AdminNewsAddEdit.ascx" in order to create news.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNews_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                LoadNewsAddEditEventArgs args = new LoadNewsAddEditEventArgs();

                LoadNewsListEventArgs listArgs = new LoadNewsListEventArgs();
                listArgs.SortExpression = this.SortExpression;
                listArgs.SortDirection = this.SortDirection;
                listArgs.SearchCriteria = this.SearchCriteria;

                args.NewsListSettings = listArgs;
                this.ParentControl.OnLoadNewsAddEditEvent(sender, args);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of Button btnDeleteExpiredNews.
        /// </summary>
        /// <remarks>
        /// Calls method <see cref="News.DeleteAllExpired"/> to delete all expired news.
        /// <para>
        ///    If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise method <see cref="PerformSearch"/> is called to refresh the data in GridView gvNews.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteExpiredNews_Click(object sender, EventArgs e)
        {
            if (ParentControl.CurrentUser != null )
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    try
                    {
                        News.DeleteAllExpired();
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("NewsDeleteExpiredErrorMessage").ToString(), false));
                        return;
                    }

                    //Successful deletion of news =>Refresh the grid with news.
                    this.PageIndex = gvNews.PageIndex;
                    PerformSearch(SearchCriteria,null);
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
                }
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of Button btnLinkNews.
        /// </summary>
        /// <remarks>
        ///  Calls method <see cref="GetSelectedNewsFromGrid()"/> to collect the ids of the news to link to the news with id <see cref="NewsId"/>.
        /// The creation of the relations is performed by method <see cref="News.AddLinkedNews"/>.
        /// <para>
        ///    If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise message for success is displayed at the top of the screen in label lblMessage.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLinkNews_Click(object sender, EventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole != UserRole.None)
                {
                    List<int> selectedNewsIds = GetSelectedNewsFromGrid();
                    if (selectedNewsIds.Count > 0)
                    {
                        try
                        {
                            News.AddLinkedNews(this.NewsId, selectedNewsIds);
                        }
                        catch
                        {
                            lblMessage.Visible = false;
                            lblErrorMessage.Text = Convert.ToString(GetLocalResourceObject("LinkNewsErrorMessage"));
                            lblErrorMessage.Visible = true;
                            return;
                        }

                        //Successful link of news.
                        lblErrorMessage.Visible = true;
                        lblMessage.Text = Convert.ToString(GetLocalResourceObject("LinkNewsSuccessfulMessage"));
                        lblMessage.Visible = true;
                    }
                
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
                }
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event <see cref="AdminLanguages.ChangeLanguage"/> of user control cntrlLanguages.
        /// </summary>
        /// <remarks>
        /// When language is changed in the interface, method <see cref="ListCategories"/> is called to retrieve and display
        /// the categories translated in the new language.
        ///<para>
        /// The serach criteria are gathered from the interface with method <see cref="CollectSearchCriteria"/> and for the returned search criteria
        /// a new search is performed by calling method <see cref="PerformSearch"/>.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cntrlLanguages_ChangeLanguage(object sender, ChangeLanguageArgs args)
        {
            this.ParentControl.CurrentLanguage = args.NewLanguage;
            ListCategories();

            SearchCriteria = CollectSearchCriteria();
            PerformSearch(SearchCriteria, null);
        }


        /// <summary>
        /// Displays in DropDown ddlCategory all existing categories translated in the current selected language.
        /// </summary>
        /// <remarks>
        ///  The categories are retrived by calling method <see cref="Category.ListTranslatedOnly(Category)"/>.
        /// The language for which to return the categories is set in the filtering object passed as parameter to the method.
        /// </remarks>
        private void ListCategories()
        {
            Category objCategory = new Category();
            objCategory.LanguageCulture = this.ParentControl.CurrentLanguage;

            DataTable dtCategories = Category.ListTranslatedOnly(objCategory);

            ddlCategory.DataSource = dtCategories;
            ddlCategory.DataBind();

            for (int i = 0; i < dtCategories.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(dtCategories.Rows[i]["IsVisible"]))
                {
                    ddlCategory.Items[i].Attributes.Add("class", "mc_news_not_visible_category");
                }
            }

            ddlCategory.Items.Insert(0, new ListItem(Convert.ToString(GetLocalResourceObject("All")),"all"));
            ddlCategory.Items.Insert(1, new ListItem(Convert.ToString(GetLocalResourceObject("None")), "none"));
        }

        /// <summary>
        /// Returns list with the ids of the news selected in GridView gvNews.
        /// </summary>
        /// <returns></returns>
        private List<int> GetSelectedNewsFromGrid()
        {
            List<int> selectedNewsIds = new List<int>();

            foreach (GridViewRow row in gvNews.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    HtmlInputCheckBox checkbox = (HtmlInputCheckBox)row.FindControl("chkNews");
                    if (checkbox != null && checkbox.Checked)
                    {
                        selectedNewsIds.Add(Convert.ToInt32(checkbox.Value));
                    }
                }
            }

            return selectedNewsIds;
        }

        /// <summary>
        /// Displays in the interface the search criteria passed as parameter.
        /// </summary>
        /// <param name="criteria">News search criteria.</param>
        /// <seealso cref="Melon.Components.News.NewsSearchCriteria"/>
        private void PopulateSearchCriteria(NewsSearchCriteria criteria)
        {
            txtKeywords.Text = criteria.keywords;
            if (criteria.categoryId == null)
            {
                ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue("all"));
            }
            else if (criteria.categoryId == -1)
            {
                ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue("none"));
            }
            else
            {
                ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(Convert.ToString(criteria.categoryId.Value)));
            }
            txtAuthor.Text = criteria.author;

            chkTitle.Checked = criteria.keywordsPlaceholders.Contains(NewsSearchField.Title);
            chkSubTitle.Checked = criteria.keywordsPlaceholders.Contains(NewsSearchField.SubTitle);
            chkText.Checked = criteria.keywordsPlaceholders.Contains(NewsSearchField.Body);

            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(Convert.ToString(Convert.ToInt32(criteria.status))));
            if (criteria.datePostedFrom != null)
            {
                txtDatePostedFrom.Text = criteria.datePostedFrom.Value.Date.ToString(ceDateFrom.Format);
            }
            if (criteria.datePostedTo != null)
            {
                txtDatePostedTo.Text = criteria.datePostedTo.Value.Date.ToString(ceDateTo.Format);
            }

            chkShowFeaturedOnly.Checked = criteria.featuredOnly;
            chkShowTranslatedOnly.Checked = criteria.translatedOnly;
            if (criteria.newsId != null)
                txtNewsId.Text = Convert.ToString(criteria.newsId.Value);
        }

        /// <summary>
        /// Returns the currently entered search criteria.
        /// </summary>
        /// <returns></returns>
        private NewsSearchCriteria CollectSearchCriteria()
        {
            NewsSearchCriteria criteria = new NewsSearchCriteria();
            criteria.keywords = txtKeywords.Text.Trim();
         
            if (ddlCategory.SelectedValue == "all")
            {
                criteria.categoryId = null;
            }
            else if (ddlCategory.SelectedValue == "none")
            {
                criteria.categoryId = -1;
            }
            else
            {
                criteria.categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            }
            criteria.author = txtAuthor.Text.Trim();
            List<NewsSearchField> fields = new List<NewsSearchField>();
            if (chkTitle.Checked)
            {
                fields.Add(NewsSearchField.Title);
            }
            if (chkSubTitle.Checked)
            {
                fields.Add(NewsSearchField.SubTitle);
            }
            if (chkText.Checked)
            {
                fields.Add(NewsSearchField.Body);
            }
            //If nothing is cheched from Title, Subtitle and Text then search will be performed in Title field.
            if (fields.Count == 0)
            {
                fields.Add(NewsSearchField.Title);
            }
            criteria.keywordsPlaceholders = fields;

            criteria.status = (NewsStatus)Convert.ToInt32(ddlStatus.SelectedValue);
            if (txtDatePostedFrom.Text.Trim() != string.Empty)
            {
                criteria.datePostedFrom = Convert.ToDateTime(txtDatePostedFrom.Text.Trim());
            }
            if (txtDatePostedTo.Text.Trim() != string.Empty)
            {
                criteria.datePostedTo = Convert.ToDateTime(txtDatePostedTo.Text.Trim());
            }
            criteria.featuredOnly = chkShowFeaturedOnly.Checked;
            criteria.translatedOnly = chkShowTranslatedOnly.Checked;

            if (txtNewsId.Text.Trim() != string.Empty)
            {
                criteria.newsId = Convert.ToInt32(txtNewsId.Text.Trim());
            }

            criteria.languageCulture = this.ParentControl.CurrentLanguage;

            return criteria;
        }

        /// <summary>
        /// Searches for news corresponding to the criteria passed as parameter.
        /// </summary>
        /// <remarks>
        ///     To find the news corresponding to the search criteria static method <see cref="News.Search(NewsSearchCriteria)"/> is called.
        ///     After that GridView gvNews is databound with the found news.
        /// </remarks>
        /// <param name="criteria">News search criteria.</param>
        /// <param name="newsIdToPositionOn">
        /// Id of news which to be found in GridView gvNews and 
        /// based on its position to be be estimated <see cref="PageIndex"/>.</param>
        private void PerformSearch(NewsSearchCriteria criteria, int? newsIdToPositionOn)
        {
            if (criteria.languageCulture == null)
            {
                criteria.languageCulture = this.ParentControl.CurrentLanguage;
            }
            DataTable dtResults = News.Search(criteria);
           
            //Display details of found users in GridView gvNews.
            DataView dvResults = new DataView(dtResults);
            if (dtResults.Rows.Count != 0)
            {
                dvResults.Sort = this.SortExpression + " " + this.SortDirection;
            }

            gvNews.PageSize = NewsSettings.BackEndNewsPageSize;

            //Get index of page on which to position in the grid view.
            //---------------------------------------------------------
            int pageIndex = 0;
            int pageCount = 0;

            if (dtResults.Rows.Count > 0)
            {
                pageCount = (dtResults.Rows.Count / gvNews.PageSize) + ((dtResults.Rows.Count % gvNews.PageSize == 0) ? 0 : 1);

                if (newsIdToPositionOn != null)
                {
                    int? newsPageIndex = GetPageIndexOfNews(newsIdToPositionOn.Value, dvResults, gvNews.PageSize);
                    if (newsPageIndex != null)
                    {
                        pageIndex = newsPageIndex.Value;
                    }
                    else
                    {
                        pageIndex = pageCount - 1; //Go to last page
                    }
                }
                else
                {
                    if (this.PageIndex != 0)
                    {
                        if ((pageCount - 1) > this.PageIndex)
                        {
                            pageIndex = this.PageIndex;
                        }
                        else
                        {
                            pageIndex = pageCount - 1; //Go to last page
                        }
                    }
                }
            }

            //-----------------------------------------------------------

            gvNews.PageIndex = pageIndex;

            gvNews.DataSource = dvResults;
            gvNews.DataBind();

            //Display paging if there are news found.
            if (dtResults.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvNews.PageCount, gvNews.PageIndex + 1, 5, gvNews.PageSize, dtResults.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }
        }

        /// <summary>
        /// Returns the index of the page where a news will be displayed in GridView gvNews.
        /// </summary>
        /// <param name="newsId">Id of the news to find.</param>
        /// <param name="dvResults">Gridview with the news found from last search.</param>
        /// <param name="pageSize">Number of pages of GridView gvNews.</param>
        /// <returns></returns>
        private int? GetPageIndexOfNews(int newsId, DataView dvResults, int pageSize)
        {
            int? pageIndex = (int?) null;
            int? newsIndex = (int?)null;

            DataTable dt = dvResults.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                if (id == newsId)
                {
                    newsIndex = i;
                    break;
                }
            }
          
            if (newsIndex != null)
            {
                pageIndex = newsIndex / pageSize;
            }

            return pageIndex;

        }
    }
}
