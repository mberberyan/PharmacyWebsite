using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Web.UI.WebControls;
using System.Data;
using Melon.Components.ProductCatalog.Configuration;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Provides user interface for searching users and assign/unassign admin role to them.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The Users user control contains search area where could be specified criteria for searching users.
    ///     To perform search is called method <see cref="Melon.Components.ProductCatalog.User.List"/> of class <see cref="Melon.Components.ProductCatalog.User"/>.
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
    ///             <term>Admin Role</term>
    ///             <description>Radio buttons with Admin role. 
    ///             The radio button corresponding to the role to which belongs the user is selected.
    ///             When admin role is selected method  
    ///             <see cref="Melon.Components.CMS.User.AddUserToAdminRole"/> of class User is called to assign the role to the related user.
    ///             If radio button None is selected then method <see cref="Melon.Components.CMS.User.RemoveUserFromAdminRole"/> of class <see cref="Melon.Components.CMS.User"/> is called
    ///             to unassign the previous admin role from user.
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
    ///     The number of the result to be displayed on page is configurable from configuration section &lt;cms /&gt; attribute tablePageSize in the web.config.
    /// </para>
    /// <para>
    ///     All web controls from Users are using local resources.
    ///     To customize them modify resource file Users.ascx.resx located in the MC_ProductCatalog folder.
    /// </para>
    /// </remarks>
    /// <seealso cref="Melon.Components.ProductCatalog.User"/>
    /// <seealso cref="Melon.Components.ProductCatalog.ProductCatalogRole"/>
    /// <seealso cref="Melon.Components.ProductCatalog.Providers.UserProvider"/>
    /// <seealso cref="Melon.Components.ProductCatalog.UserDataTable"/>
    public partial class CodeBehind_Users : ProductCatalogControl
    {        

        #region Fields & Properties

        /// <summary>
        /// Sort direction of the currently sorted column in the GridView with users gvUsers.
        /// It is "ASC" for ascending sorting and "DESC" for descending sorting. 
        /// </summary>
        public string SortDirection
        {
            get
            {
                if (ViewState["__mc_pc_sortDirection"] != null)
                {
                    return ViewState["__mc_pc_sortDirection"].ToString();
                }
                else
                {
                    return "ASC";
                }
            }
            set
            {
                ViewState["__mc_pc_sortDirection"] = value;
            }
        }

        /// <summary>
        /// Sort expression of the currently sorted column in the GridView with users gvUsers.
        /// </summary>
        public string SortExpression
        {
            get
            {
                if (ViewState["__mc_pc_sortExpression"] != null)
                {
                    return ViewState["__mc_pc_sortExpression"].ToString();
                }
                else
                {
                    return "isAdmin";
                }
            }
            set
            {
                ViewState["__mc_pc_sortExpression"] = value;
            }
        }

        /// <summary>
        /// Instance of <see cref="Melon.Components.ProductCatalog.User"/> which is used as filter for the search.
        /// </summary>
        public User UserFilter
        {
            get
            {
                if (ViewState["__mc_pc_userFilter"] != null)
                {
                    return (User)ViewState["__mc_pc_userFilter"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["__mc_pc_userFilter"] = value;
            }
        }

        /// <summary>
        /// Roles used as filter for the search.
        /// </summary>
        public List<ProductCatalogRole> roleTypes
        {
            get
            {
                if (ViewState["__mc_pc_rolesFilter"] != null)
                {
                    return (List<ProductCatalogRole>)ViewState["__mc_pc_rolesFilter"];
                }
                else
                {
                    return new List<ProductCatalogRole>();
                }
            }
            set
            {
                ViewState["__mc_pc_rolesFilter"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Attach event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        protected override void OnInit(EventArgs e)
        {
            this.gvUsers.RowCreated += new GridViewRowEventHandler(gvUsers_RowCreated);
            this.gvUsers.RowDataBound += new GridViewRowEventHandler(gvUsers_RowDataBound);
            this.gvUsers.Sorting += new GridViewSortEventHandler(gvUsers_Sorting);
            this.TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(Pager_PageChanged);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     If the user control is loaded for first time method <see cref="GetFilterCriteria"/> is called to get the search criteria
        ///     and then method <see cref="ListUsers"/> is called to 
        ///     search for Product Catalog users corresponding to the search critera and display them in GridView gvUsers.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                bool isAdmin = User.GetAadminRole(this.ParentControl.CurrentUser.UserName);
                if (isAdmin)
                {
                    //Gether search criteria from the interface.
                    GetFilterCriteria();

                    //Search users.
                    ListUsers();
                }
            }
        }


        /// <summary>
        /// Event handler for event RowCreated of GridView gvUsers.
        /// </summary>
        /// <remarks>
        ///     Attach event handler <see cref="rdolAdminRole_SelectedIndexChanged"/> for event SelectedIndexChanged 
        ///     of radio-buttons with name "rdolAdminRole" in GridView gvUsers.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        protected void gvUsers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RadioButtonList rdolAdminRole = (RadioButtonList)e.Row.FindControl("rdolAdminRole");
                rdolAdminRole.SelectedIndexChanged += new EventHandler(rdolAdminRole_SelectedIndexChanged);
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
        /// <author>Melon Team</author>
        /// <date>19/02/2008</date>
        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                string firstName = Convert.ToString(drv["FirstName"]);
                string lastName = Convert.ToString(drv["LastName"]);
                string userName = Convert.ToString(drv["UserName"]);
                string email = Convert.ToString(drv["Email"]);
                string currentUserRoleId = String.IsNullOrEmpty(drv["isAdmin"].ToString()) ? "0" : "1";

                Label lblFirstName = (Label)e.Row.FindControl("lblFirstName");
                lblFirstName.Text = firstName;

                Label lblLastName = (Label)e.Row.FindControl("lblLastName");
                lblLastName.Text = lastName;

                Label lblUserName = (Label)e.Row.FindControl("lblUserName");
                lblUserName.Text = userName;

                Label lblEmail = (Label)e.Row.FindControl("lblEmail");
                lblEmail.Text = email;

                //Select radio button corresponding to the admin role to which belong the user related to the current gridview row.
                RadioButtonList rdolAdminRole = (RadioButtonList)e.Row.FindControl("rdolAdminRole");
                rdolAdminRole.SelectedIndex = rdolAdminRole.Items.IndexOf(rdolAdminRole.Items.FindByValue(currentUserRoleId));


                //Format of radio button value: roleId;username;currentRoleId
                foreach (ListItem role in rdolAdminRole.Items)
                {
                    role.Value += ";" + userName + ";" + currentUserRoleId;
                }


                //Current logged user couldn't change his role so we disable all radio buttons with admin roles in his row.
                //Also we display on his row image to indicate that these are his details.
                Image imgMe = (Image)e.Row.FindControl("imgMe");
                if (userName == ParentControl.CurrentUser.UserName)
                {
                    imgMe.Visible = true;

                    //Disable all radio buttons for roles.
                    rdolAdminRole.Enabled = false;                    
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
        ///     and then calls method <see cref="ListUsers"/> to perform the sorting.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
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

                ListUsers();
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event PageChange for user control Pager.ascx.
        /// </summary>
        /// <remarks>
        ///     Set property PageIndex of GridView gvUsers to the new page number and then 
        ///     calls method <see cref="ListUsers"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        protected void Pager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                gvUsers.PageIndex = e.NewPage;
                ListUsers();
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }


        /// <summary>
        /// Event handler for event Click of Button btnSearch.
        /// </summary>
        /// <remarks>
        ///     The methods calls method <see cref="GetFilterCriteria"/> to get the search criteria
        ///     and then method <see cref="ListUsers"/> is called to 
        ///     search for Product Catalog users corresponding to the search critera and display them in GridView gvUsers.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                GetFilterCriteria();
                ListUsers();
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event SelectedIndexChanged of the RadioButtonList controls with name "rdolAdminRole"
        /// in GridView gvUsers.
        /// </summary>
        /// <remarks>
        ///     Assign or unassign admin role to the user which details are displayed in the row 
        ///     from which is raised the event.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        protected void rdolAdminRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                string selectedValue = ((RadioButtonList)sender).SelectedValue;
                string[] args = selectedValue.Split(';');
                int newRoleId = Convert.ToInt32(args[0]);
                string userExternalCode = Convert.ToString(args[1]);
                int oldRoleId = Convert.ToInt32(args[2]);

                bool isAdmin = User.GetAadminRole(this.ParentControl.CurrentUser.UserName);
                if (isAdmin)
                {
                    if (newRoleId == 0)
                    {
                        //None is checked
                        User.RemoveUserFromAdminRole(userExternalCode);
                    }
                    else
                    {
                        //Administrator
                        User.AddUserToAdminRole(userExternalCode, isAdmin);
                    }
                }
                else
                {
                    LoadAccessDeniedEventArgs argsAccessDenied = new LoadAccessDeniedEventArgs();
                    argsAccessDenied.IsUserLoggedRole = false;                    
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
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        private void GetFilterCriteria()
        {
            //Create filtering object.
            User objUser = new User();
            objUser.FirstName = txtFirstName.Text.Trim();
            objUser.LastName = txtLastName.Text.Trim();
            objUser.UserName = txtUserName.Text.Trim();
            objUser.Email = txtEmail.Text.Trim();

            this.UserFilter = objUser;

            List<ProductCatalogRole> searchRole = new List<ProductCatalogRole>();
            //Create list of selected roles.                                    
            if (cbxlRoles.Items.FindByValue("Admin").Selected)
            {
                searchRole.Add(ProductCatalogRole.Administrator);
            }

            if (cbxlRoles.Items.FindByValue("None").Selected)
            {
                searchRole.Add(ProductCatalogRole.None);
            }

            roleTypes = searchRole;
        }

        /// <summary>
        /// Search for users corresponding to the criteria entered in the search area (first name, last name, email, user name, role).
        /// </summary>
        /// <remarks>
        ///     The method get search criteria from properties <see cref="UserFilter"/>,<see cref="RolesFilter"/> and <see cref="IncludeNonAdminUsers"/> passed them to 
        ///     static method List of class <see cref="Melon.Components.CMS.User"/>.
        ///     After that GridView gvUsers is databound with the found users.
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>19/02/2008</date>
        private void ListUsers()
        {
            //Search for users. 
            DataTable dtUsers = User.List(this.UserFilter, this.roleTypes);

            //Display details of found users in GridView gvUsers.
            DataView dvUsers = new DataView(dtUsers);
            if (dtUsers.Rows.Count != 0)
            {
                dvUsers.Sort = this.SortExpression + " " + this.SortDirection;
            }

            gvUsers.PageSize = ProductCatalogSettings.TablePageSize;
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
