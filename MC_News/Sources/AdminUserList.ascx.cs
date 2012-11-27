using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.News;
using Melon.Components.News.ComponentEngine;
using Melon.Components.News.Configuration;

namespace Melon.Components.News.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for searching users and assign/unassign News user roles to them.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The AdminUserList user control contains search area where could be specified criteria for searching users.
    ///     To perform search is called method <see cref="Melon.Components.News.NewsUser.List"/> of class <see cref="Melon.Components.News.NewsUser"/>.
    ///     After search results are displayed in GridView gvUsers.
    /// </para>
    /// <para>
    ///     The GridView gvUsers has the following columns:
    ///		<list type="bullet">
    ///         <listheader>
    ///             <term>Column</term>
    ///             <description>Content</description>
    ///         </listheader>
    ///			<item>
    ///             <term>First Name</term>
    ///             <description>Label with first name of user. 
    ///             Next to the name appears icon if the row display details of the current logged user.</description>
    ///         </item>
    ///			<item>
    ///             <term>Last Name</term>
    ///             <description>Label with last name of user.</description>
    ///         </item>
    ///			<item>
    ///             <term>User Name</term>
    ///             <description>Label with user name of user.</description>
    ///         </item>
    ///			<item>
    ///             <term>Email</term>
    ///             <description>Label with email of user.</description>
    ///         </item>
    ///			<item>
    ///             <term>Additional Info</term>
    ///             <description>Additional information for the user.</description>
    ///         </item>
    ///			<item>
    ///             <term>News Role</term>
    ///             <description>Radio buttons with all News user roles. 
    ///             The radio button corresponding to the role to which belongs the user is selected.
    ///             When News role: Administrator or Writer is selected method  
    ///             <see cref="Melon.Components.News.NewsUser.AddUserToRole"/> of class User is called to assign the role to the related user.
    ///             If radio button None is selected then method <see cref="Melon.Components.News.NewsUser.RemoveUserFromRole"/> of class <see cref="Melon.Components.News.NewsUser"/> is called
    ///             to unassign the previous News user role from the user.
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
    ///     The number of the results to be displayed on page is configurable from configuration section &lt;news /&gt; sub-section &lt;backEndInterface /&gt; attribute usersPageSize in the web.config.
    /// </para>
    /// <para>
    ///     All web controls from AdminUserList are using local resources.
    ///     To customize them modify resource file AdminUserList.ascx.resx located in the MC_News/App_LocalResources folder.
    /// </para>
    /// </remarks>
    /// <seealso cref="Melon.Components.News.NewsUser"/>
    /// <seealso cref="Melon.Components.News.UserRole"/>
    /// <seealso cref="Melon.Components.News.Providers.NewsUserProvider"/>
    /// <seealso cref="Melon.Components.News.NewsUserDataTable"/>
    public partial class UserList : NewsControl
    {
        #region Fields & Properties

        /// <summary>
        /// Sort direction of the currently sorted column in GridView gvUsers.
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
                    return "ASC";
                }
            }
            set
            {
                ViewState["__mc_news_sortDirection"] = value;
            }
        }

        /// <summary>
        /// Sort expression of the currently sorted column in GridView gvUsers.
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
                    return "RoleID";
                }
            }
            set
            {
                ViewState["__mc_news_sortExpression"] = value;
            }
        }

        /// <summary>
        /// Instance of <see cref="Melon.Components.News.NewsUser"/> in which are gathered all search criteria from the interface. 
        /// It is used as filter for the search.
        /// </summary>
        public NewsUser UserFilter
        {
            get
            {
                if (ViewState["__mc_news_userFilter"] != null)
                {
                    return (NewsUser)ViewState["__mc_news_userFilter"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["__mc_news_userFilter"] = value;
            }
        }

        /// <summary>
        /// List with the user roles selected in the search area.
        /// It wil be used for the search of users.
        /// </summary>
        public List<UserRole> RolesFilter
        {
            get
            {
                if (ViewState["__mc_news_rolesFilter"] != null)
                {
                    return (List<UserRole>)ViewState["__mc_news_rolesFilter"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["__mc_news_rolesFilter"] = value;
            }
        }

        /// <summary>
        /// Flag which indicates whether to include in the search the users without news user role.
        /// </summary>
        public bool IncludeNonNewsUsers
        {
            get
            {
                if (ViewState["__mc_news_includeNonNewsUsers"] != null)
                {
                    return Convert.ToBoolean(ViewState["__mc_news_includeNonNewsUsers"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["__mc_news_includeNonNewsUsers"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Attaches event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.gvUsers.RowCreated += new GridViewRowEventHandler(gvUsers_RowCreated);
            this.gvUsers.RowDataBound += new GridViewRowEventHandler(gvUsers_RowDataBound);
            this.gvUsers.Sorting += new GridViewSortEventHandler(gvUsers_Sorting);
            this.TopPager.PageChanged += new AdminPager.PagerEventHandler(Pager_PageChanged);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     If the user control is loaded for first time method <see cref="CollectFilterCriteria"/> is called to get the search criteria
        ///     and then method <see cref="SearchUsers"/> is called to 
        ///     search for users corresponding to the search critera and display them in GridView gvUsers.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                //Gether search criteria from the interface.
                CollectFilterCriteria();

                //Search users.
                SearchUsers();
            }
        }


        /// <summary>
        /// Event handler for event Click of Button btnSearch.
        /// </summary>
        /// <remarks>
        ///     The methods calls method <see cref="CollectFilterCriteria"/> to get the search criteria
        ///     and then method <see cref="SearchUsers"/> is called to 
        ///     search for users corresponding to the search critera and display them in GridView gvUsers.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                CollectFilterCriteria();
                SearchUsers();
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event RowCreated of GridView gvUsers.
        /// </summary>
        /// <remarks>
        ///     Attaches event handler <see cref="rdolUserRoles_SelectedIndexChanged"/> for event SelectedIndexChanged 
        ///     of radio-buttons with id "rdolUserRoles" in GridView gvUsers.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUsers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RadioButtonList rdolUserRoles = (RadioButtonList)e.Row.FindControl("rdolUserRoles");
                rdolUserRoles.SelectedIndexChanged += new EventHandler(rdolUserRoles_SelectedIndexChanged);
            }
        }

        /// <summary>
        /// Event handler for event RowDataBound of GridView gvUsers.
        /// </summary>
        /// <remarks>
        ///     Used to set user's details in the columns of GridView gvUsers.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                string firstName = Convert.ToString(drv["FirstName"]);
                string lastName = Convert.ToString(drv["LastName"]);
                string userName = Convert.ToString(drv["UserName"]);
                string email = Convert.ToString(drv["Email"]);
                string additionalInfo = Convert.ToString(drv["AdditionalInfo"]);
                string currentUserRoleId = String.IsNullOrEmpty(drv["RoleID"].ToString()) ? "0" : drv["RoleID"].ToString();

                Label lblFirstName = (Label)e.Row.FindControl("lblFirstName");
                lblFirstName.Text = firstName;

                Label lblLastName = (Label)e.Row.FindControl("lblLastName");
                lblLastName.Text = lastName;

                Label lblUserName = (Label)e.Row.FindControl("lblUserName");
                lblUserName.Text = userName;

                Label lblEmail = (Label)e.Row.FindControl("lblEmail");
                lblEmail.Text = email;

                Label lblAdditionalInfo = (Label)e.Row.FindControl("lblAdditionalInfo");
                lblAdditionalInfo.Text = additionalInfo;


                //Select radio button corresponding to the News user role to which belong the user related to the current gridview row.
                RadioButtonList rdolUserRoles = (RadioButtonList)e.Row.FindControl("rdolUserRoles");
                rdolUserRoles.SelectedIndex = rdolUserRoles.Items.IndexOf(rdolUserRoles.Items.FindByValue(currentUserRoleId));

                //Format of radio button value: roleId;username;currentRoleId
                foreach (ListItem role in rdolUserRoles.Items)
                {
                    role.Value += ";" + userName + ";" + currentUserRoleId;
                }


                //Current logged user couldn't change his role so we disable all radio buttons with News user roles in his row.
                //Also we display on his row image to indicate that these are his details.
                Image imgMe = (Image)e.Row.FindControl("imgMe");
                if (userName == ParentControl.CurrentUser.UserName)
                {
                    imgMe.Visible = true;

                    //Disable all radio buttons for roles.
                    foreach (ListItem role in rdolUserRoles.Items)
                    {
                        role.Enabled = false;
                    }
                }
                else
                {
                    imgMe.Visible = false;
                }
            }
        }

        /// <summary>
        /// Event handler for event Sorting of GridView gvUsers.
        /// </summary>
        /// <remarks>
        ///     Save in view state the new sorting direction and expression 
        ///     and then calls method <see cref="SearchUsers"/> to perform the sorting.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
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

                SearchUsers();
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
        ///     Set property PageIndex of GridView gvUsers to the new page number and then 
        ///     calls method <see cref="SearchUsers"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_PageChanged(object sender, AdminPager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                gvUsers.PageIndex = e.NewPage;
                SearchUsers();
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event SelectedIndexChanged of the RadioButtonList controls with id "rdolUserRoles"
        /// in GridView gvUsers.
        /// </summary>
        /// <remarks>
        ///     Assign or unassign News user role to the user which details are displayed in the row 
        ///     from which is raised the event.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdolUserRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                string selectedValue = ((RadioButtonList)sender).SelectedValue;
                string[] args = selectedValue.Split(';');
                int newRoleId = Convert.ToInt32(args[0]);
                string userExternalCode = Convert.ToString(args[1]);
                int oldRoleId = Convert.ToInt32(args[2]);

                UserRole currentUserRole = NewsUser.GetUserRole(this.ParentControl.CurrentUser.UserName);
                if (currentUserRole == UserRole.Administrator)
                {
                    if (newRoleId == 0)
                    {
                        //None is checked
                        NewsUser.RemoveUserFromRole(userExternalCode);
                    }
                    else
                    {
                        //Administrator or Writer is checked
                        NewsUser.AddUserToRole(userExternalCode, (UserRole)newRoleId);
                    }
                }
                else
                {
                    LoadAccessDeniedEventArgs argsAccessDenied = new LoadAccessDeniedEventArgs();
                    argsAccessDenied.IsUserLoggedRole = false;
                    argsAccessDenied.UserRole = currentUserRole;
                    this.ParentControl.OnLoadAccessDeniedEvent(sender, argsAccessDenied);
                }
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Get from interface entered search criteria and save them in the view state.
        /// </summary>
        private void CollectFilterCriteria()
        {
            //Create filtering object.
            NewsUser objUser = new NewsUser();
            objUser.FirstName = txtFirstName.Text.Trim();
            objUser.LastName = txtLastName.Text.Trim();
            objUser.UserName = txtUserName.Text.Trim();
            objUser.Email = txtEmail.Text.Trim();

            this.UserFilter = objUser;

            //Create list of selected roles.
            List<UserRole> roles = new List<UserRole>();
            bool includeNonNewsUsers = false;
            foreach (ListItem item in chklUserRoles.Items)
            {
                if (item.Selected)
                {
                    int itemValue = Convert.ToInt32(item.Value);
                    if (itemValue == 0)
                    {
                        //Checkbox "None" is selected.
                        includeNonNewsUsers = true;
                    }
                    else
                    {
                        //Checkbox "Administrator" or "Writer" is selected.
                        roles.Add((UserRole)Convert.ToInt32(itemValue));
                    }
                }
            }

            this.RolesFilter = roles;
            this.IncludeNonNewsUsers = includeNonNewsUsers;

        }

        /// <summary>
        /// Search for users corresponding to the criteria entered in the search area (first name, last name, email, user name, roles).
        /// </summary>
        /// <remarks>
        ///     The method get search criteria from properties <see cref="UserFilter"/>,<see cref="RolesFilter"/> and <see cref="IncludeNonNewsUsers"/> passed them to 
        ///     static method List of class <see cref="Melon.Components.News.NewsUser"/>.
        ///     After that GridView gvUsers is databound with the found users.
        /// </remarks>
        private void SearchUsers()
        {
            //Search for users. 
            DataTable dtUsers = NewsUser.List(this.UserFilter, this.RolesFilter, this.IncludeNonNewsUsers);

            //Display details of found users in GridView gvUsers.
            DataView dvUsers = new DataView(dtUsers);
            if (dtUsers.Rows.Count != 0)
            {
                dvUsers.Sort = this.SortExpression + " " + this.SortDirection;
            }

            gvUsers.PageSize = NewsSettings.BackEndUsersPageSize;
            gvUsers.DataSource = dvUsers;
            gvUsers.DataBind();

            //Display paging if there are users found.
            if (dtUsers.Rows.Count != 0)
            {
                TopPager.FillPaging(gvUsers.PageCount, gvUsers.PageIndex + 1, 5, gvUsers.PageSize, dtUsers.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }
        }
    }
}
