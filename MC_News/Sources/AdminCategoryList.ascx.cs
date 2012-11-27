using System;
using System.Data;
using System.Configuration;
using System.Collections;
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
    /// Provides interface for managing news categories.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The AdminCategoryList.ascx user control is listing all existing categories translated in the current selected language.
    ///     To retrieve them method <see cref="Category.List(Category)"/> of class <see cref="Category"/> is called. 
    ///     The found categories are displayed in GridView gvCategories.
    /// </para>
    ///	<para>
    ///		The GridView gvCategories has the following columns:
    ///			<list type="table">
    ///				<item><term>Id</term><description>Id of the category.</description></item>
    ///             <item><term>Name</term><description>Name of the category.</description></item>
    ///				<item><term>Is Visible</term><description>Flag whether the category is visible in the front-end ofn the web site.</description></item>
    ///				<item><term>News Count</term><description>Number of news translated on the currently selected language grouped in the category /
    /// Total number of news grouped in the category</description></item>
    ///				<item><term>Column with actions</term><description>Actions move up, move down, edit, delete are available.</description></item>
    ///			</list>
    ///	</para>
    /// <para>
    ///     All web controls from user control AdminCategoryList.ascx are using local resources.
    ///     To customize them modify resource file AdminCategoryList.ascx.resx located in folder "MC_News/Sources/App_LocalResources".
    /// </para>
    /// </remarks>
    /// <seealso cref="Category"/>
    public partial class AdminCategoryList : NewsControl
    {
        /// <summary>
        /// Attaches event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.cntrlLanguages.ChangeLanguage += new ChangeLanguageEventHandler(cntrlLanguages_ChangeLanguage);
            this.gvCategories.RowCreated += new GridViewRowEventHandler(gvCategories_RowCreated);
            this.gvCategories.RowDataBound += new GridViewRowEventHandler(gvCategories_RowDataBound);
            this.btnAddCategory.Click += new EventHandler(btnAddCategory_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     Method <see cref="ListCategories"/> is called
        ///     to retrieve all categories translated for the current language and display them in GridView gvCategories.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                cntrlLanguages.SelectedLanguage = this.ParentControl.CurrentLanguage;
                ListCategories();
            }
        }


        /// <summary>
        /// Event handler for event RowCreated of GridView gvCategories.
        /// </summary>
        /// <remarks>
        ///     Attaches event handlers to buttons lbtnMoveUp, lbtnMoveDown, lbtnEdit, lbtnDelete
        ///     placed in the GridView.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCategories_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnMoveUp = (LinkButton)e.Row.FindControl("lbtnMoveUp");
                lbtnMoveUp.Command += new CommandEventHandler(lbtnMoveUp_Command);

                LinkButton lbtnMoveDown = (LinkButton)e.Row.FindControl("lbtnMoveDown");
                lbtnMoveDown.Command += new CommandEventHandler(lbtnMoveDown_Command);

                LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                lbtnEdit.Command += new CommandEventHandler(lbtnEdit_Command);

                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                lbtnDelete.Command += new CommandEventHandler(lbtnDelete_Command);
            }
        }

        /// <summary>
        /// Event handler for event RowDataBound of GridView gvCategories.
        /// </summary>
        /// <remarks>
        ///     Used to set the categories' details in the columns of the GridView.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                Label lblId = (Label)e.Row.FindControl("lblId");
                lblId.Text = Convert.ToString(drv["Id"]);

                Label lblName = (Label)e.Row.FindControl("lblName");
                if (drv["Name"] == DBNull.Value)
                {
                    //The category is not tranlsated on the current language.
                    lblName.Text = Convert.ToString(GetLocalResourceObject("NotTranslatedCategory"));
                    lblName.CssClass = "mc_news_lblNotTranslated";
                }
                else
                {
                    lblName.Text = Server.HtmlEncode(Convert.ToString(drv["Name"]));
                }

                Label lblIsVisible = (Label)e.Row.FindControl("lblIsVisible");
                bool isVisible = Convert.ToBoolean(drv["IsVisible"]);
                if (isVisible)
                {
                    lblIsVisible.Text = Convert.ToString(GetLocalResourceObject("Yes"));
                }
                else
                {
                    lblIsVisible.Text = Convert.ToString(GetLocalResourceObject("No"));
                }

                Label lblTotalNewsCount = (Label)e.Row.FindControl("lblTotalNewsCount");
                lblTotalNewsCount.Text = Convert.ToString(drv["TotalNewsCount"]);
                Label lblNewsCount = (Label)e.Row.FindControl("lblNewsCount");
                lblNewsCount.Text = Convert.ToString(drv["NewsCount"]);

                //-- Action "Move Up" --
                Label lblDisabledLnkMoveUp = (Label)e.Row.FindControl("lblDisabledLnkMoveUp");
                LinkButton lbtnMoveUp = (LinkButton)e.Row.FindControl("lbtnMoveUp");
                lbtnMoveUp.CommandArgument = Convert.ToString(drv["Id"]);
                //First category couldn't be moved up.
                if (e.Row.DataItemIndex == 0)
                {
                    lblDisabledLnkMoveUp.Visible = true;
                    lbtnMoveUp.Visible = false;
                }
                else
                {
                    lblDisabledLnkMoveUp.Visible = false;
                    lbtnMoveUp.Visible = true;
                }

                //-- Action "Move Down" --
                Label lblDisabledLnkMoveDown = (Label)e.Row.FindControl("lblDisabledLnkMoveDown");
                LinkButton lbtnMoveDown = (LinkButton)e.Row.FindControl("lbtnMoveDown");
                lbtnMoveDown.CommandArgument = Convert.ToString(drv["Id"]);
                //Last category couldn't be moved down.
                if (e.Row.DataItemIndex == (((System.Data.DataTable)gvCategories.DataSource).Rows.Count-1))
                {
                    lblDisabledLnkMoveDown.Visible = true;
                    lbtnMoveDown.Visible = false;
                }
                else
                {
                    lblDisabledLnkMoveDown.Visible = false;
                    lbtnMoveDown.Visible = true;
                }

                //-- Action "Edit" --
                LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                lbtnEdit.CommandArgument = Convert.ToString(drv["Id"]);

                //-- Action "Delete" --
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                lbtnDelete.CommandArgument = Convert.ToString(drv["Id"]);
            }

        }


        /// <summary>
        /// Event handler for event Command of LinkButton lbtnMoveUp.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Calls method <see cref="Category.Move"/> to move up in the list, the category displayed in gvCategories's row where the button is placed.
        /// </para>
        /// <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise event <see cref="BaseNewsControl.LoadCategoryListEvent"/> of the parent user control is raised 
        ///     in order to refresh data in GridView gvCategories.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnMoveUp_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    int categoryId = Convert.ToInt32(e.CommandArgument);
                   
                    try
                    {
                        Category.Move(categoryId, Direction.Up);
                    }
                    catch (NewsException ex)
                    {
                        if (ex.Code == NewsExceptionCode.MoveDeletedCategory)
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("MoveDeletedCategory").ToString(), categoryId), false));
                            ParentControl.OnLoadCategoryListEvent(sender, new LoadCategoryListEventArgs());
                            return;
                        }
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("MoveCategoryErrorMessage").ToString(), categoryId), false));
                        return;
                    }

                    ParentControl.OnLoadCategoryListEvent(sender, new LoadCategoryListEventArgs());
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
        /// Event handler for event Command of LinkButton lbtnMoveDown.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Calls method <see cref="Category.Move"/> to move down in the list, the category displayed in gvCategories's row where the button is placed.
        /// </para>
        /// <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise event <see cref="BaseNewsControl.LoadCategoryListEvent"/> of the parent user control is raised 
        ///     in order to refresh data in GridView gvCategories.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnMoveDown_Command(object sender, CommandEventArgs e)
        {
            if (ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    int categoryId = Convert.ToInt32(e.CommandArgument);

                    try
                    {
                        Category.Move(categoryId, Direction.Down);
                    }
                    catch (NewsException ex)
                    {
                        if (ex.Code == NewsExceptionCode.MoveDeletedCategory)
                        {
                            ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("MoveDeletedCategory").ToString(), categoryId), false));
                            ParentControl.OnLoadCategoryListEvent(sender, new LoadCategoryListEventArgs());
                            return;
                        }
                    }
                    catch
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("MoveCategoryErrorMessage").ToString(), categoryId), false));
                        return;
                    }

                    ParentControl.OnLoadCategoryListEvent(sender, new LoadCategoryListEventArgs());
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
        /// Raises event <see cref="BaseNewsControl.LoadCategoryAddEditEvent"/> of the parent control to load
        /// user control "AdminCategoryAddEdit.ascx" for modifying the category displayed in gvCategories's row where the button is placed.
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
                    int categoryId = Convert.ToInt32(e.CommandArgument);
                    if (Category.Exists(categoryId))
                    {
                        LoadCategoryAddEditEventArgs args = new LoadCategoryAddEditEventArgs();
                        args.CategoryId = categoryId;
                        this.ParentControl.OnLoadCategoryAddEditEvent(sender, args);
                    }
                    else
                    {
                        ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("EditDeletedCategory").ToString(), categoryId), false));
                        ParentControl.OnLoadCategoryListEvent(sender, new LoadCategoryListEventArgs());
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
        /// Calls method <see cref="Category.Delete"/> to delete the category displayed in gvCategories's row where the button is placed.
        ///</para>
        /// <para>
        ///     If error occurrs then event <see cref="BaseNewsControl.DisplayErrorPopupEvent"/> for displaying error message of the parent control is raized 
        ///     (the error is displayed in AJAX pop-up), 
        ///     otherwise event <see cref="BaseNewsControl.LoadCategoryListEvent"/> of the parent user control is raised 
        ///     in order to refresh data in GridView gvCategories.
        /// </para>
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
                     int categoryId = Convert.ToInt32(e.CommandArgument);

                     try
                     {
                         Category.Delete(categoryId);
                     }
                     catch (NewsException ex)
                     {
                         if (ex.Code == NewsExceptionCode.DeleteNotEmptyCategory)
                         {
                             ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("DeleteNotEmptyCategory").ToString(), categoryId), false));
                             return;
                         }
                     }
                     catch
                     {
                         ParentControl.OnDisplayErrorPopupEvent(sender, new DisplayErrorPopupEventArgs(String.Format(GetLocalResourceObject("CategoryDeleteErrorMessage").ToString(), categoryId), false));
                         return;
                     }

                     ParentControl.OnLoadCategoryListEvent(sender, new LoadCategoryListEventArgs());
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
        /// Event handler for event Click of Button btnAddCategory.
        /// </summary>
        /// <remarks>
        ///     Raises event <see cref="BaseNewsControl.LoadCategoryAddEditEvent"/> of the parent user control for loading 
        ///     user control "AdminCategoryAddEdit.ascx" in order to create new category.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    LoadCategoryAddEditEventArgs args = new LoadCategoryAddEditEventArgs();
                    this.ParentControl.OnLoadCategoryAddEditEvent(sender, args);
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
        /// Event ChangeLanguage of the parent control is raized because if the user control for Add/Edit category is loaded
        /// it should load the category again for the new language.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cntrlLanguages_ChangeLanguage(object sender, ChangeLanguageArgs args)
        {
            this.ParentControl.CurrentLanguage = args.NewLanguage;
            ListCategories();

            this.ParentControl.OnChangeLanguageEvent(sender, new ChangeLanguageEventArgs(args.NewLanguage));
        }


        /// <summary>
        /// Retrieves all existing categories translated in the current language and displays them in GridView gvCategories.
        /// </summary>
        /// <remarks>
        ///     The method calls the static method List of class <see cref="Melon.Components.News.Category"/> 
        ///     to retrieve the categories. After that GridView gvCategories is databound with the returned categories.
        /// </remarks>
        private void ListCategories()
        {
            Category objCategory = new Category();
            objCategory.LanguageCulture = this.ParentControl.CurrentLanguage;
            DataTable dtCategories = Category.List(objCategory);

            gvCategories.DataSource = dtCategories;
            gvCategories.DataBind();

            if (dtCategories.Rows.Count == 0)
            {
                tblEmptyDataTemplate.Visible = true;
            }
            else
            {
                tblEmptyDataTemplate.Visible = false;
            }
        }
    }
}
