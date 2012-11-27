using System;
using System.Data;
using System.Configuration;
using System.Collections;
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
    /// Provides user interface for searching comments ans managing them.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The AdminCommentList user control contains search area where could be entered search criteria.
    ///     Dropdown for comment status appear only in case News Component is configured in a way the comments to require approving before going alive on the web site.
    ///     See chapter Configuration for more details.
    /// </para>
    /// <para> 
    ///     To perform search is called method <see cref="Melon.Components.News.Comment.Search(CommentsSearchCriteria)"/> of class <see cref="Melon.Components.News.Comment"/>.
    ///     After search results are displayed in GridView gvComments.
    /// </para>
    /// <para>
    ///     The GridView gvComments has the following columns:
    ///		<list type="bullet">
    ///         <listheader>
    ///             <term>Column</term>
    ///             <description>Content</description>
    ///         </listheader>
    ///			<item>
    ///             <term>Comment</term>
    ///             <description>Div with comment's text.</description>
    ///         </item>
    ///			<item>
    ///             <term>Author</term>
    ///             <description>Label with nickname of comment's author.</description>
    ///         </item>
    ///			<item>
    ///             <term>Posted On</term>
    ///             <description>Label with date when the comment was posted.</description>
    ///         </item>
    ///			<item>
    ///             <term>News Title</term>
    ///             <description>Label with the title of the news to which is posted the comment.</description>
    ///         </item>
    ///			<item>
    ///             <term>News Id</term>
    ///             <description>Label with the id of the news to which is posted the comment.</description>
    ///         </item>
    ///			<item>
    ///             <term>Actions</term>
    ///             <description>Approve/Disapprove, Edit, Delete.
    ///             <para>Approve/Disaprove link buttons are displayed only if News Componet is configured in a way 
    ///                 the comments to require approving before going alive on the web site. See configuration chapter for more details.</para>
    ///             </description>
    ///         </item>
    ///		</list>
    /// </para>
    /// <para>
    ///     The results in the GridView could be sorted. When sorting is performed icon appears in the header 
    ///     of the sorted column which indicate what is the sorting direction: ascending or descending.
    /// </para>
    ///     <para>
    ///     There is paging in of the results in the GridView. For the purpose is used Pager control at the top of the GridView.
    ///     The number of the results to be displayed on page is configurable from configuration section &lt;news /&gt; sub-section &lt;backEndInterface /&gt; attribute commentsPageSize in the web.config.
    /// </para>
    /// <para>
    ///     All web controls from AdminCommentList are using local resources.
    ///     To customize them modify resource file AdminCommentList.ascx.resx located in the MC_News/Sources/App_LocalResources folder.
    /// </para>
    /// </remarks>
    /// <seealso cref="Melon.Components.News.Comment"/>
    /// <seealso cref="Melon.Components.News.CommentsSearchCriteria"/>
    /// <seealso cref="Melon.Components.News.Providers.CommentUserProvider"/>
    /// <seealso cref="Melon.Components.News.CommentUserDataTable"/>
    public partial class AdminCommentList : NewsControl
    {
        #region Fields & Properties

        /// <summary>
        /// Stores the criteria used to search comments.
        /// </summary>
        public CommentsSearchCriteria SearchCriteria
        {
            get
            {
                if (ViewState["__mc_news_search_criteria"] != null)
                {
                    return (CommentsSearchCriteria)ViewState["__mc_news_search_criteria"];
                }
                else
                {
                    return new CommentsSearchCriteria();
                }
            }
            set
            {
                ViewState["__mc_news_search_criteria"] = value;
            }
        }

        /// <summary>
        /// Sort direction of the currently sorted column in GridView gvComments.
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
        /// Sort expression of the currently sorted column in GridView gvComments.
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
                    return "DatePosted";
                }
            }
            set
            {
                ViewState["__mc_news_sortExpression"] = value;
            }
        }

        /// <summary>
        /// The index of the page from GridView gvComments which to be opened.
        /// </summary>
        public int PageIndex = 0;

        /// <summary>
        /// Id of comment which to be found in GridView gvComments and 
        /// based on its position to be be estimated <see cref="PageIndex"/>.
        /// </summary>
        public int? CommentIdToPositionOn;

        #endregion

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.SearchCriteria = (CommentsSearchCriteria)args[0];
            this.SortExpression = (string)args[1];
            this.SortDirection = (string)args[2];
            this.CommentIdToPositionOn = (int?)args[3];
        }

        /// <summary>
        /// Attaches event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.topPager.PageChanged += new AdminPager.PagerEventHandler(topPager_PageChanged);
            this.gvComments.RowCreated += new GridViewRowEventHandler(gvComments_RowCreated);
            this.gvComments.RowDataBound += new GridViewRowEventHandler(gvComments_RowDataBound);
            this.gvComments.Sorting += new GridViewSortEventHandler(gvComments_Sorting);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     If the user control is loaded for first time method <see cref="PerformSearch"/> is called to 
        ///     search for comments corresponding to the search critera and display them in GridView gvComments.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                ibtnOpenCalendarDateFrom.DataBind();
                ibtnOpenCalendarDateTo.DataBind();

                if (!NewsSettings.RequireApprovingComments)
                {
                    lblStatus.Visible = false;
                    ddlStatus.Visible = false;
                    gvComments.Columns[gvComments.Columns.Count - 2].Visible = false; //hide column "Approve/Disapprove"
                }

                if (SearchCriteria.newsId != null)
                {
                    //Comming from News Add/Edit.
                    txtNewsId.Text = Convert.ToString(SearchCriteria.newsId.Value);
                    txtNewsTitle.Text = SearchCriteria.newsTitle;
                }

                PopulateSearchCriteria(this.SearchCriteria); //Needed if we return from Comment Edit.
                //Then we populate the search criteria of the last search before going to edit the comment.

                PerformSearch(SearchCriteria, this.CommentIdToPositionOn);
            }
        }


        /// <summary>
        /// Event handler for event Click of Button btnSearch.
        /// </summary>
        /// <remarks>
        ///     The methods calls method <see cref="CollectSearchCriteria"/> to collect the entered search criteria
        ///     and then method <see cref="PerformSearch"/> is called to 
        ///     search for comments corresponding to the search critera and display them in GridView gvComments.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                SearchCriteria = CollectSearchCriteria();
                PerformSearch(SearchCriteria,null);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event RowCreated of GridView gvComments.
        /// </summary>
        /// <remarks>
        ///     Attaches event handlers to buttons lbtnApproveDisapprove,lbtnEdit,lbtnDelete in the gridview.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComments_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (NewsSettings.RequireApprovingComments)
                {
                    LinkButton lbtnApproveDisapprove = (LinkButton)e.Row.FindControl("lbtnApproveDisapprove");
                    lbtnApproveDisapprove.Command += new CommandEventHandler(lbtnApproveDisapprove_Command);
                }

                LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                lbtnEdit.Command += new CommandEventHandler(lbtnEdit_Command);

                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                lbtnDelete.Command += new CommandEventHandler(lbtnDelete_Command);
            }
        }

        /// <summary>
        /// Event handler for event RowDataBound of GridView gvComments.
        /// </summary>
        /// <remarks>
        ///     Used to set comment's details in the columns of GridView gvComments.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlGenericControl divCommentText = (HtmlGenericControl)e.Row.FindControl("divCommentText");
                divCommentText.InnerHtml = Server.HtmlEncode(Convert.ToString(((DataRowView)e.Row.DataItem)["Body"])).Replace("\r\n", "<br/>");

                Label lblCommentAuthor = (Label)e.Row.FindControl("lblCommentAuthor");
                lblCommentAuthor.Text = Server.HtmlEncode(Convert.ToString(((DataRowView)e.Row.DataItem)["Author"]));

                Label lblNewsTitle = (Label)e.Row.FindControl("lblNewsTitle");
                lblNewsTitle.Text = Server.HtmlEncode(Convert.ToString(((DataRowView)e.Row.DataItem)["NewsTitle"]));

                Label lblNewsId = (Label)e.Row.FindControl("lblNewsId");
                lblNewsId.Text = Convert.ToString(((DataRowView)e.Row.DataItem)["NewsId"]);

                //Actions
                string strCommentID = Convert.ToString(((DataRowView)e.Row.DataItem)["Id"]);

                if (NewsSettings.RequireApprovingComments)
                {
                    LinkButton lbtnApproveDisapprove = (LinkButton)e.Row.FindControl("lbtnApproveDisapprove");
                    if (Convert.ToBoolean(((DataRowView)e.Row.DataItem)["IsApproved"]))
                    {
                        lbtnApproveDisapprove.Text = Convert.ToString(GetLocalResourceObject("Disapprove"));
                        lbtnApproveDisapprove.CommandArgument = "0;" + strCommentID;
                        lbtnApproveDisapprove.OnClientClick = "return ConfirmAction('" + Convert.ToString(this.GetLocalResourceObject("DisapproveCommentConfirmMessage")) + "');";
                    }
                    else
                    {
                        lbtnApproveDisapprove.Text = Convert.ToString(GetLocalResourceObject("Approve"));
                        lbtnApproveDisapprove.CommandArgument = "1;" + strCommentID;
                        lbtnApproveDisapprove.OnClientClick = "return ConfirmAction('" + Convert.ToString(this.GetLocalResourceObject("ApproveCommentConfirmMessage")) + "');";
                    }
                }

                LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                lbtnEdit.CommandArgument = strCommentID;
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                lbtnDelete.CommandArgument = strCommentID;
            }
        }

        /// <summary>
        /// Event handler for event Sorting of GridView gvComments.
        /// </summary>
        /// <remarks>
        ///     Save in view state the new sorting direction and expression 
        ///     and then calls method <see cref="PerformSearch"/> to perform the sorting.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComments_Sorting(object sender, GridViewSortEventArgs e)
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
        ///     Set property PageIndex of GridView gvComments to the new page number and then 
        ///     calls method <see cref="PerformSearch"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void topPager_PageChanged(object sender, AdminPager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                this.PageIndex = e.NewPage;
                PerformSearch(SearchCriteria, null);
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
        /// Calls method <see cref="Comment.Approve(int)"/> or <see cref="Comment.Disapprove(int)"/> to approve/disapprove the comment displayed in gvComments's row where the button is placed.
        /// </para>
        /// <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise method <see cref="PerformSearch"/> is called to refresh the data in GridView gvComments.
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
                    int commentId = Convert.ToInt32(args[1]);

                    try
                    {
                        if (doApprove)
                        {
                            Comment.Approve(commentId);
                        }
                        else
                        {
                            Comment.Disapprove(commentId);
                        }

                    }
                    catch (NewsException ex)
                    {
                        switch (ex.Code)
                        {
                            case NewsExceptionCode.ApproveDeletedComment:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("ApproveDeletedComment").ToString(), false));

                                this.PageIndex = gvComments.PageIndex;
                                PerformSearch(SearchCriteria, null);

                                return;
                            case NewsExceptionCode.DispproveDeletedComment:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("DisapproveDeletedComment").ToString(), false));

                                this.PageIndex = gvComments.PageIndex;
                                PerformSearch(SearchCriteria, null);

                                return;
                        }
                      
                    }
                    catch
                    {
                        if (doApprove)
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("CommentApproveErrorMessage").ToString(), false));
                        }
                        else
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("CommentDisapproveErrorMessage").ToString(), false));
                        }
                        return;
                    }

                    //Successful approve/disapprove => Refresh comments in the grid.
                    this.PageIndex = gvComments.PageIndex;
                    PerformSearch(SearchCriteria, null);
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
        /// Event <see cref="BaseNewsControl.LoadCommentAddEditEvent"/> of the parent control is raized for loading form for modifying the comment displayed in gvComments's row where the button is placed.
        /// </para>
        /// <para>
        ///     If the comment has been deleted by another user already 
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
                if (currentUserRole == UserRole.Administrator)
                {
                    int commentId = Convert.ToInt32(e.CommandArgument);
                    if (Comment.Exists(commentId))
                    {
                        LoadCommentAddEditEventArgs args = new LoadCommentAddEditEventArgs();
                        args.CommentId = commentId;

                        LoadCommentListEventArgs listArgs = new LoadCommentListEventArgs();
                        listArgs.CommentIdToPositionOn = commentId;
                        listArgs.SortExpression = this.SortExpression;
                        listArgs.SortDirection = this.SortDirection;
                        listArgs.SearchCriteria = this.SearchCriteria;

                        args.CommentListSettings = listArgs;
                        this.ParentControl.OnLoadCommentAddEditEvent(sender, args);
                    }
                    else
                    {
                        this.ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("EditDeletedComment").ToString(), false));
                        this.PageIndex = gvComments.PageIndex;
                        PerformSearch(SearchCriteria, null);
                        return;
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
        /// Calls method <see cref="Comment.Delete"/> to delete the comment displayed in gvComments's row where the button is placed.
        ///</para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise method <see cref="PerformSearch"/> is called to refresh the data in GridView gvComments.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    int commentId = Convert.ToInt32(e.CommandArgument);

                    try
                    {
                        Comment.Delete(commentId);
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("CommentDeleteErrorMessage").ToString(), false));
                        return;
                    }

                    //Successful deletion of comment => Refresh the comments in the grid.
                    this.PageIndex = gvComments.PageIndex;
                    PerformSearch(SearchCriteria, null);
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
        /// Displays in the interface the search criteria passed as parameter.
        /// </summary>
        /// <param name="criteria">Comments search criteria.</param>
        /// <seealso cref="Melon.Components.News.CommentsSearchCriteria"/>
        private void PopulateSearchCriteria(CommentsSearchCriteria criteria)
        {
            txtKeywords.Text = criteria.keywords;
            txtAuthor.Text = criteria.author;

            if (NewsSettings.RequireApprovingComments)
            {
                if (criteria.isApproved == null)
                {
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue("all"));
                }
                else
                {
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(Convert.ToString(Convert.ToInt32(criteria.isApproved.Value))));
                }
            }

            if (criteria.datePostedFrom != null)
            {
                txtDatePostedFrom.Text = criteria.datePostedFrom.Value.Date.ToString(ceDateFrom.Format);
            }
            if (criteria.datePostedTo != null)
            {
                txtDatePostedTo.Text = criteria.datePostedTo.Value.Date.ToString(ceDateTo.Format);
            }

            txtNewsTitle.Text = criteria.newsTitle;
            if (criteria.newsId != null)
            {
                txtNewsId.Text = Convert.ToString(criteria.newsId.Value);
            }
        }

        /// <summary>
        /// Returns the currently entered search criteria.
        /// </summary>
        /// <returns></returns>
        private CommentsSearchCriteria CollectSearchCriteria()
        {
            CommentsSearchCriteria criteria = new CommentsSearchCriteria();
            criteria.keywords = txtKeywords.Text.Trim();

            if (NewsSettings.RequireApprovingComments)
            {
                if (ddlStatus.SelectedValue == "all")
                {
                    criteria.isApproved = null;
                }
                else
                {
                    criteria.isApproved = Convert.ToBoolean(Convert.ToInt32(ddlStatus.SelectedValue));
                }
            }
            else
            {
                criteria.isApproved = null;
            }

            criteria.author = txtAuthor.Text.Trim();
           
            if (txtDatePostedFrom.Text.Trim() != string.Empty)
            {
                criteria.datePostedFrom = Convert.ToDateTime(txtDatePostedFrom.Text.Trim());
            }
            if (txtDatePostedTo.Text.Trim() != string.Empty)
            {
                criteria.datePostedTo = Convert.ToDateTime(txtDatePostedTo.Text.Trim());
            }

            criteria.newsTitle = txtNewsTitle.Text.Trim();
            if (txtNewsId.Text.Trim() != string.Empty)
            {
                criteria.newsId = Convert.ToInt32(txtNewsId.Text.Trim());
            }

            return criteria;
        }

        /// <summary>
        /// Searches for comments corresponding to the criteria passed as parameter.
        /// </summary>
        /// <remarks>
        ///     To find the comments corresponding to the search criteria static method <see cref="Comment.Search(CommentsSearchCriteria)"/> is called.
        ///     After that GridView gvComments is databound with the found comments.
        /// </remarks>
        /// <param name="criteria">Comment search criteria.</param>
        /// <param name="commentIdToPositionOn">
        /// Id of comment which to be found in GridView gvComments and 
        /// based on its position to be be estimated <see cref="PageIndex"/>.</param>
        private void PerformSearch(CommentsSearchCriteria criteria, int? commentIdToPositionOn)
        {
            DataTable dtResults = Comment.Search(criteria);

            //Display details of found users in GridView gvComments.
            DataView dvResults = new DataView(dtResults);
            if (dtResults.Rows.Count != 0)
            {
                dvResults.Sort = this.SortExpression + " " + this.SortDirection;
            }

            gvComments.PageSize = NewsSettings.BackEndCommentsPageSize;


            //Get index of page on which to position in the grid view.
            //---------------------------------------------------------
            int pageIndex = 0;
            int pageCount = 0;

            if (dtResults.Rows.Count > 0)
            {
                pageCount = (dtResults.Rows.Count / gvComments.PageSize) + ((dtResults.Rows.Count % gvComments.PageSize == 0) ? 0 : 1);

                if (commentIdToPositionOn != null)
                {
                    int? newsPageIndex = GetPageIndexOfComment(commentIdToPositionOn.Value, dvResults, gvComments.PageSize);
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
            gvComments.PageIndex = pageIndex;

            gvComments.DataSource = dvResults;
            gvComments.DataBind();

            //Display paging if there are comments found.
            if (dtResults.Rows.Count != 0)
            {
                topPager.Visible = true;
                topPager.FillPaging(gvComments.PageCount, gvComments.PageIndex + 1, 5, gvComments.PageSize, dtResults.Rows.Count);
            }
            else
            {
                topPager.Visible = false;
            }
        }

        /// <summary>
        /// Returns the index of the page where a comment will be displayed in GridView gvComments.
        /// </summary>
        /// <param name="commentId">Id of the comment to find.</param>
        /// <param name="dvResults">Gridview with the comments found from last search.</param>
        /// <param name="pageSize">Number of pages of GridView gvComments.</param>
        /// <returns></returns>
        private int? GetPageIndexOfComment(int commentId, DataView dvResults, int pageSize)
        {
            int? pageIndex = (int?)null;
            int? newsIndex = (int?)null;

            DataTable dt = dvResults.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                if (id == commentId)
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
