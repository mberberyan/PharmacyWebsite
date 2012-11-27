using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Melon.Components.News.ComponentEngine;
using Melon.Components.News.Exception;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides interface for creation and modification of a news category.
    /// </summary>
    /// <remarks>
    ///     <para>The AdminCategoryAddEdit user control provides user interface for 
    ///     creating or updating categories which will be used for grouping news with similar subjects.
    ///     If property <see cref="CategoryId"/> is set then the control is loaded in context of modifying an existing category. 
    ///     If CategoryId is not set then a new category will be created.
    ///     The control is using <see cref="Melon.Components.News.Category"/> class for loading and saving the category details.  
    ///     </para>
    ///     <para>
    ///     All web controls from AdminCategoryAddEdit user control are using local resources.
    ///     To customize them modify resource file AdminCategoryAddEdit.ascx.resx placed in folder "MC_News/Sources/App_LocalResources".
    ///     </para>
    ///</remarks>
    /// <seealso cref="Category"/>
    public partial class AdminCategoryAddEdit : NewsControl
    {
        #region Fields & Properties

        /// <summary>
        /// Identifier of the category which will be modified.
        /// This property is not set if the user control is loaded in mode of a new category creation.
        /// </summary>
        public int? CategoryId;

        #endregion

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.CategoryId = (int?)args[0];
        }

        /// <summary>
        /// Attaches event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.ParentControl.ChangeLanguageEvent += new Melon.Components.News.ComponentEngine.ChangeLanguageEventHandler(ParentControl_ChangeLanguageEvent);

            this.btnCancel.Click +=new EventHandler(btnCancel_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
          
            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     If the user control was loaded to edit a category
        ///     method <see cref="LoadCategoryDetails"/> is called to load the category details in the interface.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                if (this.CategoryId == null)
                {
                    //*** Mode: Create new category ********************************

                    //Label "Id"  and its value are displayed only in update mode.
                    lblIdTitle.Visible = false;
                    lblId.Visible = false;                   
                }
                else
                {
                    //*** Mode: Update existing category ***************************

                    lblIdTitle.Visible = true;
                    lblId.Visible = true;

                    //Load in the interface the details of the category with id CategoryId.
                    LoadCategoryDetails(this.CategoryId.Value, this.ParentControl.CurrentLanguage);
                }

                //Scroll to the level of button Save and set focus on TextBox txtName.
                this.btnSave.Attributes.Add("onfocus", "document.getElementById('" + this.txtName.ClientID + "').focus();");
                this.Page.SetFocus(this.btnSave);
            }
        }

        /// <summary>
        /// Event handler for event Click of button btnSave.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     Creates a new category or update an existing category by gathering the details in the interface and pass them to 
        ///     method <see cref="Category.Save(Category)"/>.
        ///     </para>
        ///     <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise event <see cref="BaseNewsControl.LoadCategoryListEvent"/> of the parent user control is raised 
        ///     in order to refresh data in GridView gvCategories in user control "AdminCategoryList.ascx".
        ///     </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    Category objCategory;
                    if (this.CategoryId == null)
                    {
                        //Create category
                        objCategory = new Category();
                        objCategory.LanguageCulture = this.ParentControl.CurrentLanguage;
                    }
                    else
                    {
                        //Update category
                        objCategory = Category.Load(this.CategoryId.Value,this.ParentControl.CurrentLanguage);
                        if (objCategory == null)
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(GetLocalResourceObject("TryToEditNotExistingCategory").ToString(), false));
                            ParentControl.OnLoadCategoryListEvent(sender,new LoadCategoryListEventArgs());
                            return;
                        }
                    }

                    objCategory.Name = HttpUtility.HtmlDecode(txtName.Text.Trim());
                    objCategory.IsVisible = chkIsVisible.Checked;

                    try
                    {
                        //Try to save to database.
                        Category.Save(objCategory);
                    }
                    catch (NewsException ex)
                    {
                        string errorMessage;
                        if (ex.Code == NewsExceptionCode.CategoryDuplicatedName)
                        {
                            errorMessage = Convert.ToString(GetLocalResourceObject("CategoryDuplicatedNameErrorMessage"));
                        }
                        else
                        {
                            errorMessage = ex.Message;
                        }

                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(errorMessage,false));
                        return;
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(Convert.ToString(GetLocalResourceObject("SaveCategoryErrorMessage")), false));
                        return;
                    }

                    //Successful save.
                    this.ParentControl.OnLoadCategoryListEvent(sender, new LoadCategoryListEventArgs());
                }
                else
                {
                    //The current logged user is not Administrator. So he could not create/update news categories.
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
        ///    Raises event <see cref="BaseNewsControl.RemoveNewsControlEvent"/> of parent user control 
        /// to remove from the interface the current user control "AdminCategoryAddEdit.ascx".   
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveNewsControlEventArgs args = new RemoveNewsControlEventArgs("AdminCategoryAddEdit.ascx");
            ParentControl.OnRemoveNewsControlEvent(sender, args);
        }

        /// <summary>
        /// Event handler for event <see cref="BaseNewsControl.ChangeLanguageEvent"/> of parent user control.
        /// </summary>
        /// <remarks>
        /// When language is changed in the interface, method <see cref="LoadCategoryDetails"/> is called again to retrieve and display
        /// the category settings for the new language.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ParentControl_ChangeLanguageEvent(object sender, ChangeLanguageEventArgs e)
        {
            if (this.CategoryId != null)
            {
                LoadCategoryDetails(this.CategoryId.Value, e.NewLanguage);
            }
        }


        /// <summary>
        /// Retrieves from database details of the specified by id category and displays them in the interface.
        /// </summary>
        /// <remarks>
        ///     Category details are retrieved by calling static method <see cref="Category.Load(int,CultureInfo)"/>.
        /// </remarks>
        /// <param name="categoryId">Category identifier.</param>
        /// <param name="currentLanguage">Language for which to load category details.</param>
        private void LoadCategoryDetails(int categoryId, CultureInfo currentLanguage)
        {
            //Retrieve details from database.
            Category objCategory = Category.Load(categoryId,currentLanguage);

            //Display details in interface.
            lblId.Text = Convert.ToString(objCategory.Id);
            txtName.Text = objCategory.Name ?? ""; 
            chkIsVisible.Checked = objCategory.IsVisible.Value;
        }
    }
}
