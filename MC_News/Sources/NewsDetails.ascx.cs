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
using Melon.Components.News.ComponentEngine;
using System.Globalization;
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides interface in which are displayed the details of specific news.
    /// </summary>
    public partial class NewsDetails : NewsControl
    {
        #region Fields & Properties

        /// <summary>
        /// The id of the category to which news list we should return when using "back" link. 
        /// </summary>
        public int? CategoryId;

        /// <summary>
        /// The id of the news which details to display.
        /// </summary>
        public int NewsId;

        /// <summary>
        /// The language in which should be translated the news.
        /// </summary>
        public CultureInfo Language;

        private DateTime _datePosted;
        /// <summary>
        /// Date when the news is posted.
        /// </summary>
        public DateTime DatePosted
        {
            get { return _datePosted; }
            set { _datePosted = value; }
        }

        /// <summary>
        /// Id of the language version of the news.
        /// </summary>
        private int? NewsLanguageVersionId
        {
            get 
            {
                if (ViewState["mc_news_NewsLanguageVersionId"] != null)
                {
                    return Convert.ToInt32(ViewState["mc_news_NewsLanguageVersionId"]);
                }
                else
                {
                    return (int?)null;
                }
            }
            set 
            { 
                ViewState["mc_news_NewsLanguageVersionId"] = value; 
            }
        }

        /// <summary>
        /// Flag whether the user control is displayed in preview mode from the administration part of the News component.
        /// </summary>
        public bool PreviewMode = false;

        #endregion

        #region Constants

        private string POST_COMMENT_COOKIE = "mc_news_post_comment";

        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            repLinkedNews.ItemDataBound += new RepeaterItemEventHandler(repLinkedNews_ItemDataBound);

            topPager.PageChanged += new AdminPager.PagerEventHandler(topPager_PageChanged);
            gvComment.RowDataBound += new GridViewRowEventHandler(gvComment_RowDataBound);
            lbtnPostComment.Click += new EventHandler(lbtnPostComment_Click);
            btnSaveComment.Click += new EventHandler(btnSaveComment_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        /// Calls method <see cref="NewsDetailsBind()"/> to retrive news details and display them in the interface.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblPostCommentNeedApprovement.Visible = false;
            lblPostCommentError.Visible = false;
            if (!IsControlPostBack)
            {
                NewsDetailsBind();

                if (this.CategoryId == null)
                {
                    lnkBack.NavigateUrl = NewsSettings.NewsListPagePath;
                }
                else
                {
                    lnkBack.NavigateUrl = NewsSettings.NewsListPagePath + "?cat_id=" + Convert.ToString(this.CategoryId);
                }

                divCommentForm.Visible = false;
                btnCancelComment.OnClientClick = @" if (!!document.getElementById('" + txtCommentAuthor.ClientID + @"'))
                                                        document.getElementById('" + txtCommentAuthor.ClientID + @"').value='';
                                                    document.getElementById('" + txtCommmentText.ClientID + @"').value='';
                                                    document.getElementById('" + divCommentForm.ClientID + @"').style.display ='none';
                                                    return false;";

                if ((Request.Cookies[POST_COMMENT_COOKIE] != null)
                    && (Request.Cookies[POST_COMMENT_COOKIE].Value == "true")
                    && NewsSettings.RequireLoginToPostComments 
                    && (CommentUser.Load() != null))
                {
                    trCommentAuthorDetails.Visible = false;
                    divCommentForm.Visible=true;

                    //Set focus on input for entering comment text but also scroll the page to make button Post Comment visible.
                    btnSaveComment.Attributes.Add("onfocus", "document.getElementById('" + this.txtCommmentText.ClientID + "').focus();");
                    btnSaveComment.Focus();
                }
            }
        }


        /// <summary>
        /// Event handler for event ItemDataBound of Repeater repLinkedNews.
        /// </summary>
        /// <remarks>
        /// Used to set linked news details in Repeater repLinkedNews.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repLinkedNews_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink lnkLinkedNews = (HyperLink)e.Item.FindControl("lnkLinkedNews");
                lnkLinkedNews.Text = Server.HtmlEncode(Convert.ToString(((DataRowView)e.Item.DataItem)["Title"]));
                lnkLinkedNews.NavigateUrl = NewsSettings.NewsDetailsPagePath
                    + "?cat_id=" + ((((DataRowView)e.Item.DataItem)["CategoryId"]== DBNull.Value)? "-1" : Convert.ToString(((DataRowView)e.Item.DataItem)["CategoryId"]))
                    + "&news_id=" + Convert.ToString(((DataRowView)e.Item.DataItem)["Id"]);

            }
        }


        /// <summary>
        /// Event handler for event RowDataBound of GridView gvComment.
        /// </summary>
        /// <remarks>
        /// Used to set comments details in GridView gvComment.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgAuthorPhoto = (Image)e.Row.FindControl("imgAuthorPhoto");
                Label lblRegisteredOnTitle = (Label)e.Row.FindControl("lblRegisteredOnTitle");
                Label lblRegisteredOn = (Label)e.Row.FindControl("lblRegisteredOn");
                if (NewsSettings.RequireLoginToPostComments)
                {
                    string photoPath = Convert.ToString(((DataRowView)e.Row.DataItem)["AuthorPhotoPath"]);
                    if (!String.IsNullOrEmpty(photoPath))
                    {
                        imgAuthorPhoto.ImageUrl = photoPath;
                    }
                    else
                    {
                        imgAuthorPhoto.ImageUrl = Utilities.GetImageUrl(this.Page,"NewsStyles/FrontEndImages/photo.gif");
                    }
                }
                else
                {
                    imgAuthorPhoto.Visible = false;
                    lblRegisteredOnTitle.Visible = false;
                    lblRegisteredOn.Visible = false;
                }

                Label lblAuthorNickname = (Label)e.Row.FindControl("lblAuthorNickname");
                lblAuthorNickname.Text = Server.HtmlEncode(Convert.ToString(((DataRowView)e.Row.DataItem)["Author"]));

                HtmlGenericControl divCommentText = (HtmlGenericControl)e.Row.FindControl("divCommentText");
                divCommentText.InnerHtml = Server.HtmlEncode(Convert.ToString(((DataRowView)e.Row.DataItem)["Body"])).Replace("\r\n", "<br/>");
            }
        }

        /// <summary>
        /// Event handler for event PageChange for user control AdminPager.ascx.
        /// </summary>
        /// <remarks>
        ///     Set property PageIndex of GridView gvComment to the new page number and then 
        ///     calls methods <see cref="Comment.List(Comment)"/> and <see cref="DisplayComments"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void topPager_PageChanged(object sender, AdminPager.PagerEventArgs e)
        {
            gvComment.PageIndex = e.NewPage;

            Comment filter = new Comment();
            filter.NewsId = this.NewsLanguageVersionId;
            filter.IsApproved = true;
            DataTable dtCommments = Comment.List(filter);
            DisplayComments(dtCommments);
        }

        /// <summary>
        /// Event handler for event Click of LinkButton lbtnPostComment.
        /// </summary>
        /// <remarks>
        /// Displays the form for posting comment to the currently displayed news.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnPostComment_Click(object sender, EventArgs e)
        {
            if (NewsSettings.RequireLoginToPostComments)
            {
                CommentUser currentUser = CommentUser.Load();
                if (currentUser != null)
                {
                    trCommentAuthorDetails.Visible = false;
                    divCommentForm.Visible = true;
                    //Set focus on input for entering comment text but also scroll the page to make button Post Comment visible.
                    btnSaveComment.Attributes.Add("onfocus", "document.getElementById('" + this.txtCommmentText.ClientID + "').focus();");
                    btnSaveComment.Focus();
                }
                else
                {
                    Response.Cookies[POST_COMMENT_COOKIE].Value = "true";
                    Response.Cookies[POST_COMMENT_COOKIE].Expires = DateTime.Now.AddMinutes(5);

                    Response.Redirect(NewsSettings.PostCommentsLoginUrl + "?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
                }
            }
            else
            {
                trCommentAuthorDetails.Visible = true;
                divCommentForm.Visible = true;
                //Set focus on input for entering comment text but also scroll the page to make button Post Comment visible.
                btnSaveComment.Attributes.Add("onfocus", "document.getElementById('" + this.txtCommentAuthor.ClientID + "').focus();");
                btnSaveComment.Focus();
            }
        }

        /// <summary>
        /// Event handler for event Click of Button btnSaveComment.
        /// </summary>
        /// <remarks>
        /// Saves the comment have been posted to the current news by calling method <see cref="Comment.Save(Comment)"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveComment_Click(object sender, EventArgs e)
        {
            if (News.Exists(this.NewsId))
            {
                Comment objComment = new Comment();

                if (NewsSettings.RequireLoginToPostComments)
                {
                    CommentUser currentUser = CommentUser.Load();
                    if (currentUser != null)
                    {
                        objComment.Author = currentUser.NickName;
                    }
                    else
                    {
                        Response.Redirect(NewsSettings.PostCommentsLoginUrl + "?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
                    }
                }
                else
                {
                    objComment.Author = txtCommentAuthor.Text.Trim();
                }

                objComment.Body = txtCommmentText.Text.Trim();
                objComment.NewsId = this.NewsLanguageVersionId;
                objComment.DatePosted = DateTime.Now;

                if (NewsSettings.RequireApprovingComments)
                {
                    objComment.IsApproved = false;
                }
                else
                {
                    objComment.IsApproved = true;
                }

                try
                {
                    Comment.Save(objComment);
                }
                catch
                {
                    lblPostCommentError.Text = Convert.ToString(GetLocalResourceObject("PostCommentErrorMessage"));
                    lblPostCommentError.Visible = true;
                    return;
                }

                //Successful post of comment = >clear and close comment form.
                txtCommentAuthor.Text = "";
                txtCommmentText.Text = "";
                divCommentForm.Visible = false;

                //Refresh comments list.
                Comment filter = new Comment();
                filter.NewsId = this.NewsLanguageVersionId;
                filter.IsApproved = true;
                DataTable dtCommments = Comment.List(filter);
                DisplayComments(dtCommments);


                if (NewsSettings.RequireApprovingComments)
                {
                    lblPostCommentNeedApprovement.Visible = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "<script>document.getElementById('" + lblPostCommentNeedApprovement.ClientID + "').tabIndex=1;document.getElementById('" + lblPostCommentNeedApprovement.ClientID + "').focus();</script>");
                }
                else
                {
                    if (NewsSettings.RequireLoginToPostComments)
                    {
                        Response.Cookies[POST_COMMENT_COOKIE].Value = "false";
                        if (dtCommments.Rows.Count > 0)
                        {
                            //Set focus on the last comment;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "<script>document.getElementById('" + gvComment.Rows[0].FindControl("lblRegisteredOn").ClientID + "').tabIndex=1;document.getElementById('" + gvComment.Rows[0].FindControl("lblRegisteredOn").ClientID + "').focus();</script>");
                        }
                    }
                    else
                    {
                        if (dtCommments.Rows.Count > 0)
                        {
                            //Set focus on the last comment;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "<script>document.getElementById('" + gvComment.Rows[0].FindControl("lblAuthorNickname").ClientID + "').tabIndex=1;document.getElementById('" + gvComment.Rows[0].FindControl("lblAuthorNickname").ClientID + "').focus();</script>");
                        }
                    }
                }
            }
            else
            {
                phNewsDetails.Visible = false;
                phLinkedNews.Visible = false;
                phComments.Visible = false;
                lblError.Text = Convert.ToString(GetLocalResourceObject("TryToPostCommentToDeletedNews"));
                lblError.Visible = true;
            }
        }


        /// <summary>
        /// Retrieves and displayed in the interface the news details translated in the current language set in property <see cref="Language"/>.
        /// </summary>
        /// <remarks>
        /// Calls method <see cref=" News.LoadDetails(int, CultureInfo)"/> to retrieve news details, linked news and comments.
        /// <para>The method should be called every time the language is changed.</para>
        /// </remarks>
        public void NewsDetailsBind()
        {
            DataSet ds;
            try
            {
                ds = News.LoadDetails(this.NewsId, this.Language);
            }
            catch
            {
                phNewsDetails.Visible = false;
                phLinkedNews.Visible = false;
                phComments.Visible = false;
                lblError.Text = Convert.ToString(GetLocalResourceObject("CanNotLoadNewsDetailsMessage"));
                lblError.Visible = true;
                return;
            }

            DataTable dtNewsDetails = ds.Tables[0];
            DataTable dtRelatedNews = ds.Tables[1];
            DataTable dtCommments = ds.Tables[2];

            if (dtNewsDetails.Rows.Count > 0)
            {
                DataRow drNews = dtNewsDetails.Rows[0];

                if (!Convert.ToBoolean(drNews["IsApproved"]) && !PreviewMode)
                {
                      phNewsDetails.Visible = false;
                      phLinkedNews.Visible = false;
                      phComments.Visible = false;
                      lblError.Text = Convert.ToString(GetLocalResourceObject("NewsIsDisapproved"));
                      lblError.Visible = true;
                      return;
                }

                phNewsDetails.Visible = true;
                lblError.Visible = false;

                this.NewsLanguageVersionId = Convert.ToInt32(drNews["IdOfChildTable"]);

                if (!IsControlPostBack && !PreviewMode)
                {
                    //Register visit of news.
                    News.RegisterNewsVisit(Convert.ToInt32(drNews["IdOfChildTable"]));
                }

                #region Display News Details

                //Date Posted
                if (drNews.IsNull("DatePosted"))
                {
                    lblPublishedOnTitle.Visible = false;
                    lblDatePublished.Visible = false;
                }
                else
                {
                    lblPublishedOnTitle.Visible = true;
                    lblDatePublished.Visible = true;
                    this._datePosted = Convert.ToDateTime(drNews["DatePosted"]);
                    lblDatePublished.DataBind();
                }

                //Author
                string author = Convert.ToString(drNews["Author"]);
                if (String.IsNullOrEmpty(author))
                {
                    lblBy.Visible = false;
                    lblAuthor.Visible = false;
                }
                else
                {
                    lblAuthor.Text = Server.HtmlEncode(author);
                }

                //Category
                string categoryName = Convert.ToString(drNews["CategoryName"]);
                if (String.IsNullOrEmpty(categoryName))
                {
                    lblCategoryTitle.Visible = false;
                    lblCategoryName.Visible = false;
                }
                else
                {
                    lblCategoryName.Text = Server.HtmlEncode(categoryName);
                }

                //Source
                string source = Convert.ToString(drNews["Source"]);
                if (String.IsNullOrEmpty(source))
                {
                    lblSourceTitle.Visible = false;
                    lblSource.Visible = false;
                }
                else
                {
                    lblSource.Text = Server.HtmlEncode(source);
                }

                //Title
                lblTitle.Text = Server.HtmlEncode(Convert.ToString(drNews["Title"]));

                //SubTitle
                string subTitle = Convert.ToString(drNews["SubTitle"]);
                if (String.IsNullOrEmpty(subTitle))
                {
                    lblSubTitle.Visible = false;
                }
                else
                {
                    lblSubTitle.Text = Server.HtmlEncode(subTitle);
                }

                //Photo & Description
                string photoPath = Convert.ToString(drNews["PhotoPath"]);
                if (String.IsNullOrEmpty(photoPath))
                {
                    imgPhoto.Visible = false;
                    lblPhotoDescription.Visible = false;
                }
                else
                {
                    imgPhoto.ImageUrl = photoPath;
                    string photoDescription = Convert.ToString(drNews["PhotoDescription"]);
                    if (String.IsNullOrEmpty(photoDescription))
                    {
                        lblPhotoDescription.Visible = false;
                    }
                    else
                    {
                        lblPhotoDescription.Text = photoDescription;
                    }
                }

                //Text
                divText.InnerHtml = Convert.ToString(drNews["Body"]);//Server.HtmlEncode(Convert.ToString(drNews["Body"]));

                //Tags
                string tags = Convert.ToString(drNews["Tags"]);
                if (String.IsNullOrEmpty(tags))
                {
                    lblTagsTitle.Visible = false;
                }
                else
                {
                    lblTags.Text = Server.HtmlEncode(tags);
                }

                //Views
                lblViewsCount.Text = Convert.ToString(Convert.ToInt32(drNews["ViewsCount"]));

                #endregion


                if (!PreviewMode)
                {
                    //Linked News
                    DisplayLinkedNews(dtRelatedNews);

                    //Comments
                    if (NewsSettings.AllowPostingComments)
                    {
                        phComments.Visible = true;
                        DisplayComments(dtCommments);
                    }
                    else
                    {
                        phComments.Visible = false;
                    }
                }
                else
                {
                    lnkBack.Visible = false;
                    phLinkedNews.Visible = false;
                    phComments.Visible = false;
                }
            }
            else
            {
                //The news is not found.
                phNewsDetails.Visible = false;
                phLinkedNews.Visible = false;
                phComments.Visible = false;
                lblError.Visible = true;
                lblError.Text = Convert.ToString(GetLocalResourceObject("NewsNotFoundMessage"));
                return;
            }
        }


        /// <summary>
        /// Databinds Repeater repLinkedNews with the news linked to the current displayed news.
        /// </summary>
        /// <param name="dtLinkedNews">The linked news of the current displayed news.</param>
        private void DisplayLinkedNews(DataTable dtLinkedNews)
        {
            if (dtLinkedNews.Rows.Count > 0)
            {
                repLinkedNews.DataSource = dtLinkedNews;
                repLinkedNews.DataBind();
            }
            else
            {
                repLinkedNews.Visible = false;
            }
        }

        /// <summary>
        ///  Databinds GridView gvComment with the comments posted to the current displayed news.
        /// </summary>
        /// <param name="dtComments">The comments posted to the current displayed news.</param>
        private void DisplayComments(DataTable dtComments)
        {
            dtComments = Comment.AddAuthorDetails(dtComments);
            lblCommentsTitle.Text = String.Format(Convert.ToString(GetLocalResourceObject("CommentsTitle")), dtComments.Rows.Count);

            gvComment.PageSize = 10;

            gvComment.DataSource = dtComments;
            gvComment.DataBind();

            if (dtComments.Rows.Count > 0)
            {
                topPager.FillPaging(gvComment.PageCount, gvComment.PageIndex + 1, 5, gvComment.PageSize, dtComments.Rows.Count);
            }
            else
            {
                topPager.Visible = false;
            }
        }
    }
}
