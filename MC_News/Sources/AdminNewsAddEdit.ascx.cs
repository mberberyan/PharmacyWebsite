using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    /// Provides interface for creation and modification of news.
    /// </summary>
    /// <remarks>
    ///     <para>The AdminNewsAddEdit user control provides user interface for 
    ///     updating an existing news or creating a new one.
    ///     If property <see cref="NewsId"/> is set then the control is loaded in context of modifying an existing news. 
    ///     If NewsId is not set then a news will be created.
    ///     The control is using <see cref="Melon.Components.News.News"/> class for loading and saving the news details.  
    ///     </para>
    ///     <para>The news can have linked news with similar subject. They are listed in Repeater repLinkedNews.
    ///     The search of related news which to link to the current news is used AJAX popup window.</para>
    ///     <para>
    ///         If there are posted comments to the news displayed in the interface then a link for to these comments is displayed.
    ///     The user can use it to go and manage (edit/delete) these comments.
    ///     Note: This link apears if the posting of comments is allowed in the configuration of News Component.
    ///     </para>
    ///     <para>
    ///     All web controls from AdminNewsAddEdit user control are using local resources.
    ///     To customize them modify resource file AdminNewsAddEdit.ascx.resx placed in folder "MC_News/Sources/App_LocalResources".
    ///     </para>
    ///</remarks>
    /// <seealso cref="News"/>
    /// <seealso cref="Comment"/>
    public partial class AdminNewsAddEdit : NewsControl
    {
        #region Fields & Properties

        /// <summary>
        /// Id of the news to edit.
        /// </summary>
        public int? NewsId;

        /// <summary>
        /// Message to display after successful save.
        /// </summary>
        public string Message;

        /// <summary>
        /// The event arguments with which the user control "AdminNewsList.ascx" should be loaded in case button "Cancel" is clicked.
        /// Using these arguments news listing will be loaded as it was before coming to edit the news.
        /// </summary>
        public LoadNewsListEventArgs NewsListSettings;

        /// <summary>
        /// The creation date of the news loaded for modification.
        /// </summary>
        public DateTime dateCreated;

        /// <summary>
        /// The date of last update of the news loaded for modification. 
        /// </summary>
        public DateTime dateLastUpdated;

        /// <summary>
        /// The title of the news loaded for modification which is currently stored in the database.
        /// </summary>
        internal string NewsTitle
        {
            get 
            {
                if (ViewState["mc_news_NewsTitle"] != null)
                {
                    return Convert.ToString(ViewState["mc_news_NewsTitle"]);
                }
                else
                {
                    return string.Empty;
                }
            }
            set 
            {
                ViewState["mc_news_NewsTitle"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.NewsId = (int?)args[0];
            this.Message = (string)args[1];
            this.NewsListSettings = (LoadNewsListEventArgs)args[2];
        }

        /// <summary>
        /// Attaches event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.cntrlLanguages.ChangeLanguage += new ChangeLanguageEventHandler(cntrlLanguages_ChangeLanguage);

            this.btnAddNews.Click += new EventHandler(btnAddNews_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.repLinkedNews.ItemCreated += new RepeaterItemEventHandler(repLinkedNews_ItemCreated);
            this.repLinkedNews.ItemDataBound += new RepeaterItemEventHandler(repLinkedNews_ItemDataBound);
            this.btnUpdateLinkedNewsList.Click += new EventHandler(btnUpdateLinkedNewsList_Click);

            this.lbtnManageComments.Click += new EventHandler(lbtnManageComments_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     If the user control was loaded to edit a news
        ///     method <see cref="LoadNewsDetails"/> is called to load the news details in the interface.
        /// <para>
        /// To load the available categories, the possible hours and minutes in the dropdown lists are called respectively methods 
        /// <see cref="ListCategories"/>, <see cref="ListHours"/>, <see cref="ListMinutes"/>.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                if (String.IsNullOrEmpty(this.Message))
                {
                    lblMessage.Visible = false;
                }
                else
                {
                    lblMessage.Text = this.Message;
                    lblMessage.Visible = true;
                }
                cntrlLanguages.SelectedLanguage = this.ParentControl.CurrentLanguage;

                txtDatePosted.Attributes.Add("onblur","ValidatorValidate(document.getElementById('" + cvTimePosted.ClientID + "'))");
                ddlHourPosted.Attributes.Add("onblur", "ValidatorValidate(document.getElementById('" + cvTimePosted.ClientID + "'))");
                ddlMinutesPosted.Attributes.Add("onblur", "ValidatorValidate(document.getElementById('" + cvTimePosted.ClientID + "'))");

                ibtnOpenCalendarDatePosted.DataBind();
                ibtnOpenCalendarDateValidFrom.DataBind();
                ibtnOpenCalendarDateValidTo.DataBind();

                lblPhotoUploadInstructions.Text = String.Format(Convert.ToString(GetLocalResourceObject("PhotoUploadInstructions")), String.Concat(NewsSettings.PhotosAllowedExtensions.ToArray()),NewsSettings.PhotosMaxSize);
                btnClosePreviewNews.DataBind();
                btnCloseLinkedNews.DataBind();
                btnCloseLinkedNews.OnClientClick = "document.getElementById('" + btnUpdateLinkedNewsList.ClientID + "').click();";

                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    chkIsApproved.Enabled = true;
                }
                else
                {
                    chkIsApproved.Enabled = false;
                }

                HTMLEditor.BasePath = NewsSettings.fckEditorBasePath;

                ListCategories();
                ListHours();
                ListMinutes();

                if (this.NewsId == null)
                {
                    //Case: Create News
                    lblNewsAddEditTitle.Text = Convert.ToString(GetLocalResourceObject("NewsAddTitle"));
                    lblIdTitle.Visible = false;
                    lblId.Visible = false;
                    txtDatePosted.Text = DateTime.Now.Date.ToString(ceDatePosted.Format);
                    ddlHourPosted.SelectedIndex = ddlHourPosted.Items.IndexOf(ddlHourPosted.Items.FindByValue(Convert.ToString(DateTime.Now.Hour)));
                    ddlMinutesPosted.SelectedIndex = ddlMinutesPosted.Items.IndexOf(ddlMinutesPosted.Items.FindByValue(Convert.ToString(DateTime.Now.Minute)));
                    trLogDetails.Visible = false;
                    btnPreview.Visible = false;
                    btnAddNews.Visible = false;

                    tblLinkedNews.Visible = false;
                    lbtnManageComments.Visible = false;
                }
                else
                {
                    //Case: Update News
                    lblNewsAddEditTitle.Text = Convert.ToString(GetLocalResourceObject("NewsEditTitle"));
                    lblIdTitle.Visible = true;
                    lblId.Visible = true;
                    trLogDetails.Visible = true;

                    tblLinkedNews.Visible = true;
                  
                    LoadNewsDetails(this.NewsId.Value, this.ParentControl.CurrentLanguage);
                }
            }
            if (this.NewsId != null)
            {
                cntrlLinkedNews.NewsId = this.NewsId.Value;
            }
        }


        /// <summary>
        /// Event handler for event Click of button btnSave.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     Create/Update news by gathering the details in the interface and pass them to 
        ///     method <see cref="News.Save(News)"/>.
        ///     </para>
        ///     <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise message for successful update of the news is displayed and the user control is reloaded in context of modification of the created/updated news.
        ///     </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //IsValid is set to true for the regular expression validator for the news photo path 
            //because when save news with photo the validator displayed the error message even the path in the textbox is empty then (actually it is valid).
            revPhotoPath.IsValid = true;

            if (this.ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole != UserRole.None)
                {
                    News objNews;

                    #region Set details of the created/updated news.

                    if (this.NewsId == null)
                    {
                        //Create news
                        objNews = new News();
                        objNews.LanguageCulture = this.ParentControl.CurrentLanguage;
                    }
                    else
                    {
                        //Update news
                        objNews = News.Load(this.NewsId.Value, this.ParentControl.CurrentLanguage);
                        if (objNews == null)
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingNews").ToString(), false));
                            NewsListSettings.NewsIdToPositionOn = null;
                            ParentControl.OnLoadNewsListEvent(sender, NewsListSettings);
                            return;
                        }
                        else
                        {
                            if (currentUserRole == UserRole.Writer && objNews.IsApproved.Value)
                            {
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("WriterTryToEditApprovedNews").ToString(), false));
                                NewsListSettings.NewsIdToPositionOn = objNews.Id;
                                ParentControl.OnLoadNewsListEvent(sender, NewsListSettings);
                                return;
                            }
                        }
                    }

                    objNews.UserId = this.ParentControl.CurrentUser.AdapterId;
                    objNews.Title = txtTitle.Text.Trim();
                    objNews.SubTitle = txtSubTitle.Text.Trim();
                    if (!String.IsNullOrEmpty(ddlCategory.SelectedValue))
                    {
                        objNews.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                    }
                    else
                    {
                        objNews.CategoryId = null;
                    }

                    if (!chkRemovePhoto.Checked)
                    {
                        if (fuPhoto.HasFile)
                        {
                            //New photo will be uploaded.
                            NewsPhoto objNewsPhoto = new NewsPhoto();
                            objNewsPhoto.BinaryInfo = fuPhoto.FileBytes;
                            objNewsPhoto.FileName = fuPhoto.FileName;
                            objNews.photo = objNewsPhoto;
                        }
                    }
                    else
                    {
                        //Current photo should be deleted.
                        NewsPhoto objNewsPhoto = new NewsPhoto();
                        objNewsPhoto.RemovePreviousPhoto = true;
                        objNews.photo = objNewsPhoto;
                    }

                    objNews.PhotoDescription = txtPhotoDescription.Text.Trim();
                    if (txtDatePosted.Text.Trim() != "")
                    {
                        DateTime date = Convert.ToDateTime(txtDatePosted.Text.Trim());
                        if (ddlHourPosted.SelectedIndex != 0)
                        {
                            date = date.AddHours(Convert.ToDouble(ddlHourPosted.SelectedValue));
                            if (ddlMinutesPosted.SelectedIndex != 0)
                            {
                                date = date.AddMinutes(Convert.ToDouble(ddlMinutesPosted.SelectedValue));
                            }
                        }
                        objNews.DatePosted = date;
                    }
                    else
                    {
                        objNews.DatePosted = null;
                    }
                    objNews.Author = txtAuthor.Text.Trim();
                    objNews.Source = txtSource.Text.Trim();
                    if (txtDateValidFrom.Text.Trim() != "")
                    {
                        objNews.DateValidFrom = Convert.ToDateTime(txtDateValidFrom.Text.Trim());
                    }
                    else
                    {
                        objNews.DateValidFrom = null;
                    }
                    if (txtDateValidTo.Text.Trim() != "")
                    {
                        objNews.DateValidTo = Convert.ToDateTime(txtDateValidTo.Text.Trim());
                    }
                    else
                    {
                        objNews.DateValidTo = null;
                    }
                    objNews.Tags = txtTags.Text.Trim();
                    objNews.IsFeatured = chkIsFeatured.Checked;
                    objNews.IsApproved = chkIsApproved.Checked;
                    
                    objNews.Body = HTMLEditor.Value;
                    string bodyWithoutHTMLTags = Utilities.RemoveHTMLTags(objNews.Body);
                    objNews.Summary = Utilities.Cut(bodyWithoutHTMLTags, " ", NewsSettings.NEWS_SUMMARY_LENGTH, NewsSettings.NEWS_SUMMARY_LENGTH);

                    #endregion

                    try
                    {
                        //Try to save to database.
                        News.Save(objNews);
                    }
                    catch (NewsException ex)
                    {
                        switch (ex.Code)
                        {
                            case NewsExceptionCode.PhotoNotAllowedSize:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(Convert.ToString(GetLocalResourceObject("PhotoNotAllowedSizeErrorMessage")), ex.AdditionalInfo,NewsSettings.PhotosMaxSize), false));
                                return;
                            case NewsExceptionCode.PhotoNotAllowedExtension:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(Convert.ToString(GetLocalResourceObject("PhotoNotAllowedExtensionErrorMessage")), ex.AdditionalInfo), false));
                                return;
                            case NewsExceptionCode.UnauthorizedAccessException:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(Convert.ToString(GetLocalResourceObject("UnauthorizedAccessException")), ex.AdditionalInfo), false));
                                return;
                            case NewsExceptionCode.PhotoRemoveFailure:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(Convert.ToString(GetLocalResourceObject("PhotoRemoveErrorMessage")), ex.AdditionalInfo), false));
                                return;
                            case NewsExceptionCode.PhotoUploadFailure:
                                ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(Convert.ToString(GetLocalResourceObject("PhotoUploadErrorMessage")), ex.AdditionalInfo), false));
                                return;
                        }
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(Convert.ToString(GetLocalResourceObject("SaveNewsErrorMessage")), false));
                        return;
                    }

                    //Successfull save of news.
                    LoadNewsAddEditEventArgs args = new LoadNewsAddEditEventArgs();
                    args.NewsId = objNews.Id;
                    args.Message = Convert.ToString(GetLocalResourceObject("SaveNewsSuccessfulMessage"));
                    this.NewsListSettings.NewsIdToPositionOn = objNews.Id;
                    args.NewsListSettings = this.NewsListSettings;
                    this.ParentControl.OnLoadNewsAddEditEvent(sender, args);
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
                //There is no logged user (session time out).
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of button btnCancel.
        /// </summary>
        /// <remarks>
        /// Raises event <see cref="BaseNewsControl.LoadNewsListEvent"/> of parent user control 
        /// to load user control for news listing "AdminNewsList.ascx". The event is called with the arguments
        /// stored in <see cref="NewsListSettings"/>.Using these arguments news listing will be loaded as it was before coming to edit the news.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Set the selected language to be this which was before coming to the screen for edit
            this.ParentControl.CurrentLanguage = this.NewsListSettings.SearchCriteria.languageCulture;
            this.NewsListSettings.NewsIdToPositionOn = this.NewsId;
            this.ParentControl.OnLoadNewsListEvent(sender, this.NewsListSettings);
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

                args.NewsListSettings = this.NewsListSettings;
                this.ParentControl.OnLoadNewsAddEditEvent(sender, args);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event ItemCreated of Repeater repLinkedNews.
        /// </summary>
        /// <remarks>
        ///     Attaches event handler to buttons lbtnRemove in the Repeater.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repLinkedNews_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnRemove = (LinkButton)e.Item.FindControl("lbtnRemove");
                lbtnRemove.Command += new CommandEventHandler(lbtnRemove_Command);
            }
        }

        /// <summary>
        /// Event handler for event ItemDataBound of Repeater repLinkedNews.
        /// </summary>
        /// <remarks>
        ///     Used to set linked news's details in the columns of Repeater repLinkedNews.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repLinkedNews_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
           if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
           {
               Label lblId = (Label)e.Item.FindControl("lblId");
               lblId.Text = Convert.ToString(((KeyValuePair<int,string>)e.Item.DataItem).Key);

               Label lblTitle = (Label)e.Item.FindControl("lblTitle");
               if (!String.IsNullOrEmpty(((KeyValuePair<int, string>)e.Item.DataItem).Value))
               {
                   lblTitle.Text = Server.HtmlEncode(((KeyValuePair<int, string>)e.Item.DataItem).Value);
               }
               else
               {
                   lblTitle.Text = Convert.ToString(GetLocalResourceObject("NA"));
               }

               LinkButton lbtnRemove = (LinkButton)e.Item.FindControl("lbtnRemove");
               lbtnRemove.CommandArgument = Convert.ToString(((KeyValuePair<int, string>)e.Item.DataItem).Key);
           }
        }

        /// <summary>
        /// Event handler for event Click of Button lbtnRemove in Repeater repLinkedNews.
        /// </summary>
        /// <remarks>
        /// Method <see cref="News.RemoveLinkedNews"/> is called to remove the linked news displayed in repLinkedNews's row where the button is placed.
        /// The list with the linked news is refreshed with the remaining linked news retrieved by method <see cref="News.GetAddLinkedNews"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRemove_Command(object sender, CommandEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole != UserRole.None)
                {
                    int linkedNewsId = Convert.ToInt32(e.CommandArgument);
                    try
                    {
                        //Try to save to database.
                        News.RemoveLinkedNews(this.NewsId.Value, linkedNewsId);
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(Convert.ToString(GetLocalResourceObject("RemoveLinkedNewsErrorMessage")), false));
                        return;
                    }

                    //Successful remove of linked news
                    Dictionary<int, string> linkedNews = News.GetAddLinkedNews(this.NewsId.Value, this.ParentControl.CurrentLanguage);
                    repLinkedNews.DataSource = linkedNews;
                    repLinkedNews.DataBind();
                    lblLinkedNews.Text = String.Format(Convert.ToString(GetLocalResourceObject("LinkedNewsTitle")), linkedNews.Count);
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
                //There is no logged user (session time out).
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click of Button btnUpdateLinkedNewsList.
        /// </summary>
        /// <remarks>
        ///    This button is hidden. Its event onclick is raized when button ibtnCloseLinkedNews (button Close in the popup for searching news) is clicked.
        ///    Then method <see cref="News.GetAddLinkedNews"/> is called to retrieve all linked news of the the news which is updated and 
        ///    the list with the linked news is refreshed in the interface.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateLinkedNewsList_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> linkedNews = News.GetAddLinkedNews(this.NewsId.Value, this.ParentControl.CurrentLanguage);
            repLinkedNews.DataSource = linkedNews;
            repLinkedNews.DataBind();
            lblLinkedNews.Text = String.Format(Convert.ToString(GetLocalResourceObject("LinkedNewsTitle")), linkedNews.Count);
        }


        /// <summary>
        /// Event handler for event Click of LinkButton lbtnManageComments.
        /// </summary>
        /// <remarks>
        ///     Raises event <see cref="BaseNewsControl.LoadCommentListEvent"/> of the parent user control for loading 
        ///     user control "AdminCommentList.ascx" and listing all comments posted to the current news.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnManageComments_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    CommentsSearchCriteria criteria = new CommentsSearchCriteria();
                    criteria.newsId = this.NewsId;
                    criteria.newsTitle = this.NewsTitle;

                    LoadCommentListEventArgs args = new LoadCommentListEventArgs();
                    args.SearchCriteria = criteria;

                    this.ParentControl.OnLoadCommentListEvent(sender, args);
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
                //There is no logged user (session time out).
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event <see cref="AdminLanguages.ChangeLanguage"/> of user control cntrlLanguages.
        /// </summary>
        /// <remarks>
        /// When language is changed in the interface, method <see cref="ListCategories"/> is called to retrieve and display
        /// the categories translated in the new language. The news could be grouped in one of these categories.
        /// If the user control is loaded in order to modify an existing news then method <see cref="LoadNewsDetails"/> is called to 
        /// display the news details for the new language.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cntrlLanguages_ChangeLanguage(object sender, ChangeLanguageArgs args)
        {
            this.ParentControl.CurrentLanguage = args.NewLanguage;

            ListCategories();
            if (this.NewsId != null)
            {
                LoadNewsDetails(this.NewsId.Value, this.ParentControl.CurrentLanguage);
            }
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
           
            for(int i=0; i < dtCategories.Rows.Count;  i++)
            {
                if (!Convert.ToBoolean(dtCategories.Rows[i]["IsVisible"]))
                {
                    ddlCategory.Items[i].Attributes.Add("class","mc_news_not_visible_category");
                }
            }

            ddlCategory.Items.Insert(0, new ListItem());
        }

        /// <summary>
        /// Displays in DropDown ddlHourPosted all possible hours.
        /// </summary>
        private void ListHours()
        {
            ddlHourPosted.Items.Add(new ListItem("",""));
            for (int i = 0; i <= 23; i++)
            {
                string hour = Convert.ToString(i);
                if (!String.IsNullOrEmpty(ddlHourPosted.DataTextFormatString))
                {
                    hour = String.Format(ddlHourPosted.DataTextFormatString, i);
                }
                ddlHourPosted.Items.Add(new ListItem(hour, i.ToString()));
            }
        }

        /// <summary>
        /// Displays in DropDown ddlMinutesPosted all possible minutes of an hour.
        /// </summary>
        private void ListMinutes()
        {
            ddlMinutesPosted.Items.Add(new ListItem("", ""));
            for (int i = 0; i <= 59; i++)
            {
                string minutes = Convert.ToString(i);
                if (!String.IsNullOrEmpty(ddlMinutesPosted.DataTextFormatString))
                {
                    minutes = String.Format(ddlMinutesPosted.DataTextFormatString, i);
                }
                ddlMinutesPosted.Items.Add(new ListItem(minutes, i.ToString()));
            }
        }

        /// <summary>
        /// Retrieves from database details of the specified by id news and displays them in the interface.
        /// </summary>
        /// <remarks>
        ///     News details are retrieved by calling static method <see cref="News.Load(int,CultureInfo)"/>.
        /// </remarks>
        /// <param name="newsId">News identifier.</param>
        /// <param name="language">Language for which to load news details.</param>
        private void LoadNewsDetails(int newsId, CultureInfo language)
        {
            News objNews = News.Load(newsId, language);
            this.NewsTitle = objNews.Title;

            if (objNews.IdOfChildTable != null)
            {
                btnPreview.Visible = true;
                string previewUrl = NewsSettings.newsPreviewPageUrl + "?" + ((objNews.CategoryId != null)? "cat_id=" + Convert.ToString(objNews.CategoryId.Value) + "&" : "");
                previewUrl += "news_id=" + Convert.ToString(objNews.Id.Value) + "&lang=" + this.ParentControl.CurrentLanguage.Name;
                btnPreview.OnClientClick = "document.getElementById('frameNewsPreview').src='" 
                    + ResolveUrl(previewUrl)
                    + "';";
            }
            else
            {
                btnPreview.Visible = false;
            }

            lblId.Text = Convert.ToString(objNews.Id);
            txtTitle.Text = objNews.Title;
            txtSubTitle.Text = objNews.SubTitle;
            if (objNews.CategoryId.HasValue)
            {
                ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(Convert.ToString(objNews.CategoryId)));

            }
            if (!String.IsNullOrEmpty(objNews.PhotoPath))
            {
                divPhoto.Visible = true;
                imgPhoto.ImageUrl = objNews.PhotoPath;
                chkRemovePhoto.Checked = false;
            }
            else
            {
                divPhoto.Visible = false;
            }
            txtPhotoDescription.Text = objNews.PhotoDescription;
            if (objNews.DatePosted.HasValue)
            {
                txtDatePosted.Text = objNews.DatePosted.Value.Date.ToString(ceDatePosted.Format);
                ddlHourPosted.SelectedIndex = ddlHourPosted.Items.IndexOf(ddlHourPosted.Items.FindByValue(Convert.ToString(objNews.DatePosted.Value.Hour)));
                ddlMinutesPosted.SelectedIndex = ddlMinutesPosted.Items.IndexOf(ddlMinutesPosted.Items.FindByValue(Convert.ToString(objNews.DatePosted.Value.Minute)));
            }
            txtAuthor.Text = objNews.Author;
            txtSource.Text = objNews.Source;
            if (objNews.DateValidFrom.HasValue)
            {
                txtDateValidFrom.Text = objNews.DateValidFrom.Value.ToString(ceDateValidFrom.Format);
            }
            if (objNews.DateValidTo.HasValue)
            {
                txtDateValidTo.Text = objNews.DateValidTo.Value.ToString(ceDateValidTo.Format);
            }
            txtTags.Text = objNews.Tags;
            chkIsFeatured.Checked = objNews.IsFeatured.Value;
            chkIsApproved.Checked = objNews.IsApproved.Value;

            //Log Details
            this.dateCreated = objNews.DateCreated.Value;
            lblDateCreated.DataBind();
            lblCreatedByUsername.Text = Server.HtmlEncode(objNews.CreatedByUserName);

            if (objNews.DateLastUpdated != null)
            {
                divLogLastUpdated.Visible = true;
                this.dateLastUpdated = objNews.DateLastUpdated.Value;
                lblDateLastUpdated.DataBind();
                if (!String.IsNullOrEmpty(objNews.LastUpdatedByUserName))
                {
                    lblLastUpdatedByUserName.Text = Server.HtmlEncode(objNews.LastUpdatedByUserName);
                }
                else
                {
                    lblLastUpdatedBy.Visible = false;
                    lblLastUpdatedByUserName.Visible = false;
                }
            }
            else
            {
                divLogLastUpdated.Visible = false;
            }
            
            HTMLEditor.Value = objNews.Body;

            //Linked News Details
            lblLinkedNews.Text = String.Format(Convert.ToString(GetLocalResourceObject("LinkedNewsTitle")), objNews.LinkedNews.Count);
            repLinkedNews.DataSource = objNews.LinkedNews;
            repLinkedNews.DataBind();

            if ((this.ParentControl.CurrentUser.IsInNewsUserRole(UserRole.Administrator)) & NewsSettings.AllowPostingComments && (objNews.CommentsTotalCount != 0))
            {
                lbtnManageComments.Text = String.Format(Convert.ToString(GetLocalResourceObject("Comments")), objNews.CommentsLanguageCount,this.ParentControl.CurrentLanguage.Name, objNews.CommentsTotalCount);
                lbtnManageComments.Visible = true;
            }
            else
            {
                lbtnManageComments.Visible = false;
            }
        }
    }
}
