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
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Represents the main administration control of the News component, used to be included in the desired page on the web site 
    /// where component will be integrated.
    /// The control inherits most of its functionality from <see cref="Melon.Components.News.ComponentEngine.InnerBaseNewsControl"/> 
    /// and is a critical part of the News component engine.
    /// In short this control manages all the logic flow and by using a system of events decides how to proceed on every step. 
    /// This is deeply connected with the usage of dynamic control load, which is implemented here.
    /// </summary>
    /// <remarks>
    ///		<para>
    ///			Basic workflow of the News component engine:
    ///			<list type="bullet">
    ///				<listheader>
    ///					As a general rule in order events and data to be maintained between page recycling the state of the controls (dynamic or not) 
    ///					should be preserved. While in general ViewState helps with the static controls, for dynamic a little additional effort is needed – 
    ///					they must be recreated at proper time so the ASP.NET engine can match their ViewState from the previous page iteration.
    ///					In the suggested algorithm the following system was chosen:
    ///				</listheader>
    ///				<item>Before loading the ViewState a special refresh function is called which recreates the controls.</item>
    ///				<item>Since the refresh function should somehow know which controls to recreate 
    ///                 and the ViewState is obviously not the place for that information, the control state is used instead. 
    ///                 This is simply because the control state is loaded before the viewstate.</item>
    ///				<item>Moreover a special structure is needed to store the info about the controls in the control state. 
    ///             That structure contains the control file name (in case of web control) or the control class (in case of custom control). 
    ///             Because these child controls hold some data which depends on the other controls, special init blocks are needed so the control will match its state from the previous page request. 
    ///             These init blocks are implemented as instance methods Initializer of the user controls. 
    ///             Finally Initializer method has arguments with real values. These values are the second part of this <see cref="Melon.Components.News.ComponentEngine.ControlInitInfo">ControlInitInfo</see> class.</item>
    ///				<item>The above structure is stored in the control state and being read between cycles by the <see cref="Melon.Components.News.ComponentEngine.InnerBaseNewsControl.Refresh">Refresh</see> function. 
    ///				This function on the other hand calls the function <see cref="Melon.Components.News.ComponentEngine.InnerBaseNewsControl.LoadCustomControl(Melon.Components.News.ComponentEngine.ControlInitInfo[])">LoadCustomControl</see> 
    ///				which actually loads the controls and call the init blocks with their argument values.</item>
    ///				<item>The main control(NewsAdminisrtation) inherits <see cref="Melon.Components.News.ComponentEngine.BaseNewsControl"/> in which all these functions and the control state functionality are implemented. 
    ///				Moreover it defines a set of delegates, events and methods which provide the communication system between the controls. 
    ///				Every event describes a specific action within the component. This action can happen in each of the child controls. 
    ///				Moreover every event has a public fire method, which allows the child controls to initiate the execution of attached event handler(s). 
    ///				All the handling of the events is done in this class via handling its base’s events.</item>
    ///				<item>On the other hand all child controls inherit <see cref="Melon.Components.News.ComponentEngine.NewsControl"/> class, which defines the following set of features:
    ///					<list type="bullet">
    ///						<item>Property defining the parent control, which because of the above and the run-time creation of web controls should be the BaseNewsControl (this explains why this control handles its base events).</item>
    ///						<item>Implementation of IsControlPostBack feature, indicating that the control was loaded before and if the postback occurred by it.</item>
    ///					</list>
    ///				</item>
    ///			</list>
    ///		</para>
    /// </remarks>
    /// <seealso cref="Melon.Components.News.ComponentEngine.BaseNewsControl"/>
    /// <seealso cref="Melon.Components.News.ComponentEngine.InnerBaseNewsControl"/>
    /// <seealso cref="Melon.Components.News.ComponentEngine.NewsControl"/>
    /// <seealso cref="Melon.Components.News.ComponentEngine.ControlInitInfo"/>
    public partial class NewsAdministration : BaseNewsControl
    {
        /// <summary>
        /// Attaches event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.lbtnOpenNews.Click += new EventHandler(lbtnOpenNews_Click);
            this.lbtnOpenCategories.Click += new EventHandler(lbtnOpenCategories_Click);
            this.lbtnOpenComments.Click += new EventHandler(lbtnOpenComments_Click);
            this.lbtnOpenUsers.Click += new EventHandler(lbtnOpenUsers_Click);

            this.LoadAccessDeniedEvent += new LoadAccessDeniedEventHandler(LoadAccessDenied);
            this.LoadNewsListEvent += new LoadNewsListEventHandler(LoadNewsList);
            this.LoadNewsAddEditEvent += new LoadNewsAddEditEventHandler(LoadNewsAddEdit);
            this.LoadCategoryListEvent += new LoadCategoryListEventHandler(LoadCategoryList);
            this.LoadCategoryAddEditEvent += new LoadCategoryAddEditEventHandler(LoadCategoryAddEdit);
            this.LoadCommentListEvent += new LoadCommentListEventHandler(LoadCommentList);
            this.LoadCommentAddEditEvent += new LoadCommentAddEditEventHandler(LoadCommentAddEdit);
            this.RemoveNewsControlEvent+=new RemoveNewsControlEventHandler(RemoveNewsControl);
            this.DisplayErrorPopupEvent += new DisplayErrorPopupEventHandler(DisplayErrorPopup);
           
            base.OnInit(e);
        }

        /// <summary>
        /// Initializes the user control.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Check the current logged user. If there is no such user then redirect to News Administration login page.
        ///         If there is logged user but it is not News user "Access is denied" is displayed.
        ///     </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            MPE.Hide();
            if (!Page.IsPostBack)
            {
                CurrentUser = NewsUser.Load();

                if (CurrentUser == null)
                {
                    RedirectToLoginPage();
                }
                else if (CurrentUser.IsNewsUser())
                {
                    //The currently logged user is News user. Display screen Manage News.
                    lbtnOpenNews_Click(sender, e);

                    if (CurrentUser.IsInNewsUserRole(UserRole.Writer))
                    {
                        //For users from role Writer menu Categories,Comments and Users are not visible. 
                        tdOpenCategories.Visible = false;
                        tdOpenComments.Visible = false;
                        tdOpenUsers.Visible = false;
                    }
                    else
                    {
                        //Administrator
                        if (!NewsSettings.AllowPostingComments)
                        {
                            tdOpenComments.Visible = false;
                        }
                    }
                }
                else
                {
                    //The currently logged user is not News user. Display screen Access Denied and hide menu.
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = true;
                    args.UserRole = UserRole.None;
                    LoadAccessDenied(sender, args);
                }
            }
        }


        /// <summary>
        /// Event handler for event Click on button lbtnOpenNews.
        /// </summary>
        /// <remarks>
        ///     <para>It selects menu item "News" in the News Administration Panel and loads user control AdminNewsList.ascx.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenNews_Click(object sender, EventArgs e)
        {
            if (this.CurrentUser != null)
            {
                //Select menu item "News".
                tdOpenNews.Attributes.Add("class", "mc_news_menu_item_first_selected");
                tdOpenCategories.Attributes.Add("class", "mc_news_menu_item_middle");
                tdOpenComments.Attributes.Add("class", "mc_news_menu_item_middle");
                tdOpenUsers.Attributes.Add("class", "mc_news_menu_item_last");

                LoadNewsList(sender, new LoadNewsListEventArgs());
            }
            else
            {
                RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click on button lbtnOpenCategories.
        /// </summary>
        /// <remarks>
        ///     <para>It selects menu item "Categories" in the News Administration Panel and loads user control AdminCategoryList.ascx.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenCategories_Click(object sender, EventArgs e)
        {
            if (this.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    //Select menu item "Categories".
                    tdOpenNews.Attributes.Add("class", "mc_news_menu_item_first");
                    tdOpenCategories.Attributes.Add("class", "mc_news_menu_item_middle_selected");
                    tdOpenComments.Attributes.Add("class", "mc_news_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_news_menu_item_last");

                    LoadCategoryList(sender,new LoadCategoryListEventArgs());
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    LoadAccessDenied(sender, args);
                }
            }
            else
            {
                RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click on button lbtnOpenComments.
        /// </summary>
        /// <remarks>
        ///     <para>It selects menu item "Comments" in the News Administration Panel and loads user control AdminCommentList.ascx.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenComments_Click(object sender, EventArgs e)
        {
            if (this.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    LoadCommentList(sender, new LoadCommentListEventArgs());
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    LoadAccessDenied(sender, args);
                }
            }
            else
            {
                RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Click on button lbtnOpenUsers.
        /// </summary>
        /// <remarks>
        ///     <para>It selects menu item "Users" in the News Administration Panel and loads user control AdminUserList.ascx.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOpenUsers_Click(object sender, EventArgs e)
        {
            if (this.CurrentUser != null)
            {
                UserRole currentUserRole = NewsUser.GetUserRole(this.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    //Select menu item "Users".
                    tdOpenNews.Attributes.Add("class", "mc_news_menu_item_first");
                    tdOpenCategories.Attributes.Add("class", "mc_news_menu_item_middle");
                    tdOpenComments.Attributes.Add("class", "mc_news_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_news_menu_item_last_selected");

                    ControlInitInfo cntrlUserList = new ControlInitInfo("AdminUserList.ascx", false, null);
                    LoadCustomControl(new ControlInitInfo[] { cntrlUserList });
                }
                else
                {
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = false;
                    args.UserRole = currentUserRole;
                    LoadAccessDenied(sender, args);
                }
            }
            else
            {
                RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Loads user control AdminNewsList.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadNewsList(object sender, LoadNewsListEventArgs e)
        {
            ControlInitInfo cntrlNewsList = new ControlInitInfo("AdminNewsList.ascx", true, new object[] { e.SearchCriteria, e.SortExpression, e.SortDirection,e.NewsIdToPositionOn});
            LoadCustomControl(new ControlInitInfo[] { cntrlNewsList });
        }

        /// <summary>
        /// Loads user control AdminNewsAddEdit.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadNewsAddEdit(object sender, LoadNewsAddEditEventArgs e)
        {
            ControlInitInfo cntrlNewsAddEdit = new ControlInitInfo("AdminNewsAddEdit.ascx", true, new object[] { e.NewsId,e.Message,e.NewsListSettings });
            LoadCustomControl(new ControlInitInfo[] {cntrlNewsAddEdit });
        }
 
        /// <summary>
        /// Loads user control AdminCategoryList.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadCategoryList(object sender, LoadCategoryListEventArgs e)
        {
            ControlInitInfo cntrlCategoryList = new ControlInitInfo("AdminCategoryList.ascx", true,null);
            LoadCustomControl(new ControlInitInfo[] { cntrlCategoryList });
        }

        /// <summary>
        /// Loads user controls AdminCategoryList.ascx and AdminCategoryAddEdit.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadCategoryAddEdit(object sender, LoadCategoryAddEditEventArgs e)
        {
            ControlInitInfo cntrlCategoryList = new ControlInitInfo("AdminCategoryList.ascx", false,null);
            ControlInitInfo cntrlCategoryAddEdit = new ControlInitInfo("AdminCategoryAddEdit.ascx", true, new object[] { e.CategoryId });
            LoadCustomControl(new ControlInitInfo[] { cntrlCategoryList, cntrlCategoryAddEdit });
        }

        /// <summary>
        /// Loads user control AdminCommentList.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadCommentList(object sender, LoadCommentListEventArgs e)
        {
            //Select menu item "Comments".
            tdOpenNews.Attributes.Add("class", "mc_news_menu_item_first");
            tdOpenCategories.Attributes.Add("class", "mc_news_menu_item_middle");
            tdOpenComments.Attributes.Add("class", "mc_news_menu_item_middle_selected");
            tdOpenUsers.Attributes.Add("class", "mc_news_menu_item_last");

            ControlInitInfo cntrlCommentList = new ControlInitInfo("AdminCommentList.ascx", true, new object[] { e.SearchCriteria, e.SortExpression,e.SortDirection,e.CommentIdToPositionOn});
            LoadCustomControl(new ControlInitInfo[] { cntrlCommentList });
        }

        /// <summary>
        /// Loads user control AdminCommentAddEdit.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadCommentAddEdit(object sender, LoadCommentAddEditEventArgs e)
        {
            ControlInitInfo cntrlCommentAddEdit = new ControlInitInfo("AdminCommentAddEdit.ascx", true, new object[] { e.CommentId,e.CommentListSettings });
            LoadCustomControl(new ControlInitInfo[] { cntrlCommentAddEdit });
        }

        /// <summary>
        /// Loads user control AdminAccessDenied.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadAccessDenied(object sender, LoadAccessDeniedEventArgs e)
        {
            divNavigation.Visible = false;

            //ControlInitializer cntrlNewsAccessDeniedInit = delegate(Control cntrl, object[] arg)
            //{
            //    Type t = cntrl.GetType();
            //    t.GetField("UserRole").SetValue(cntrl, arg[0]);
            //    t.GetField("IsUserLoggedRole").SetValue(cntrl, arg[1]);
            //};

            ControlInitInfo cntrlNewsAccessDenied = new ControlInitInfo("AdminAccessDenied.ascx", false, new object[] { e.UserRole, e.IsUserLoggedRole });
            LoadCustomControl(new ControlInitInfo[] { cntrlNewsAccessDenied });
        }

        /// <summary>
        /// Removes user control from the News panel where it is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemoveNewsControl(object sender, RemoveNewsControlEventArgs e)
        {
            RemoveCustomControl(e.ControlFile);
        }

        /// <summary>
        /// Displays popup modal window with error message in it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DisplayErrorPopup(object sender, DisplayErrorPopupEventArgs e)
        {
            lblError.Text = Server.HtmlEncode(e.ErrorMessage);
            if (e.RefreshScreen)
            {
                MPE.OnOkScript = "document.location.reload(true);";
            }
            MPE.Show();
        }


        /// <summary>
        /// Redirects to the page for login in the News Administration if specified in the component configuration, otherwise load user control for access denied.
        /// </summary>
        public override void RedirectToLoginPage()
        {
            if (String.IsNullOrEmpty(NewsSettings.BackEndLoginURL) || this.ResolveUrl(NewsSettings.BackEndLoginURL) == Request.RawUrl)
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.UserRole = UserRole.None;
                args.IsUserLoggedRole = true;
                LoadAccessDenied(this, args);
            }
            else
            {
                Response.Redirect(NewsSettings.BackEndLoginURL + "?ReturnUrl=" + Server.UrlEncode(Request.RawUrl), true);
            }
        }
    }
}
