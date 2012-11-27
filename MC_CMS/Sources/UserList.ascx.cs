using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.CMS;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Configuration;
using System.Web.Security;

/// <summary>
/// Provides user interface for searching users and assign/unassign CMS roles to them.
/// </summary>
/// <remarks>
/// <para>
///     The CMSUserList user control contains search area where could be specified criteria for searching users.
///     To perform search is called method <see cref="Melon.Components.CMS.User.List"/> of class <see cref="Melon.Components.CMS.User"/>.
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
///             <term>CMS Role</term>
///             <description>Radio buttons with all CMS roles. 
///             The radio button corresponding to the role to which belongs the user is selected.
///             When CMS role Super Administrator, Administrator or Writer is selected method  
///             <see cref="Melon.Components.CMS.User.AddUserToCMSRole"/> of class User is called to assign the role to the related user.
///             If radio button None is selected then method <see cref="Melon.Components.CMS.User.RemoveUserFromCMSRole"/> of class <see cref="Melon.Components.CMS.User"/> is called
///             to unassign the previous CMS role from user.
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
///     The number of the result to be displayed on page is configurable from configuration section &lt;cms /&gt; attribute usersPageSize in the web.config.
/// </para>
/// <para>
///     All web controls from CMSUserList are using local resources.
///     To customize them modify resource file CMSUserList.resx located in the MC_CMS folder.
/// </para>
/// </remarks>
/// <seealso cref="Melon.Components.CMS.User"/>
/// <seealso cref="Melon.Components.CMS.CMSRole"/>
/// <seealso cref="Melon.Components.CMS.Providers.UserProvider"/>
/// <seealso cref="Melon.Components.CMS.UserDataTable"/>
public partial class UserList:CMSControl
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
            if (ViewState["__mc_cms_sortDirection"] != null)
            {
                return ViewState["__mc_cms_sortDirection"].ToString();
            }
            else
            {
                return "ASC";
            }
        }
        set 
        {
            ViewState["__mc_cms_sortDirection"] = value; 
        }
    }

    /// <summary>
    /// Sort expression of the currently sorted column in the GridView with users gvUsers.
    /// </summary>
    public string SortExpression
    {
        get
        {
            if (ViewState["__mc_cms_sortExpression"] != null)
            {
                return ViewState["__mc_cms_sortExpression"].ToString();
            }
            else
            {
                return "RoleID";
            }
        }
        set
        {
            ViewState["__mc_cms_sortExpression"] = value;
        }
    }

    /// <summary>
    /// Instance of <see cref="Melon.Components.CMS.User"/> which is used as filter for the search.
    /// </summary>
    public User UserFilter
    {
        get
        {
            if (ViewState["__mc_cms_userFilter"] != null)
            {
                return (User)ViewState["__mc_cms_userFilter"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            ViewState["__mc_cms_userFilter"] = value;
        }
    }

    /// <summary>
    /// Roles used as filter for the search.
    /// </summary>
    public List<CMSRole> RolesFilter
    {
        get
        {
            if (ViewState["__mc_cms_rolesFilter"] != null)
            {
                return (List<CMSRole>)ViewState["__mc_cms_rolesFilter"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            ViewState["__mc_cms_rolesFilter"] = value;
        }
    }

    /// <summary>
    /// Flag which indicates whether to include in the search users without CMS role.
    /// </summary>
    public bool IncludeNonCMSUsers
    {
        get
        {
            if (ViewState["__mc_cms_includeNonCMSUsers"] != null)
            {
                return Convert.ToBoolean(ViewState["__mc_cms_includeNonCMSUsers"]);
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["__mc_cms_includeNonCMSUsers"] = value;
        }
    }

    #endregion

    /// <summary>
    /// Attach event handlers for controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>19/02/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.gvUsers.RowCreated += new GridViewRowEventHandler(gvUsers_RowCreated);
        this.gvUsers.RowDataBound += new GridViewRowEventHandler(gvUsers_RowDataBound);
        this.gvUsers.Sorting += new GridViewSortEventHandler(gvUsers_Sorting);
        this.TopPager.PageChanged+=new Pager.PagerEventHandler(Pager_PageChanged);
        this.btnSearch.Click += new EventHandler(btnSearch_Click);

        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     If the user control is loaded for first time method <see cref="GetFilterCriteria"/> is called to get the search criteria
    ///     and then method <see cref="ListUsers"/> is called to 
    ///     search for CMS users corresponding to the search critera and display them in GridView gvUsers.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>19/02/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator || currentUserRole == CMSRole.Administrator)
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
    ///     Attach event handler <see cref="rdolCMSRoles_SelectedIndexChanged"/> for event SelectedIndexChanged 
    ///     of radio-buttons with name "rdolCMSRoles" in GridView gvUsers.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>20/02/2008</date>
    protected void gvUsers_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadioButtonList rdolCMSRoles = (RadioButtonList)e.Row.FindControl("rdolCMSRoles");
            rdolCMSRoles.SelectedIndexChanged += new EventHandler(rdolCMSRoles_SelectedIndexChanged);
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
    /// <author></author>
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
            string currentUserRoleId = String.IsNullOrEmpty(drv["RoleID"].ToString()) ? "0" : drv["RoleID"].ToString();

            Label lblFirstName = (Label)e.Row.FindControl("lblFirstName");
            lblFirstName.Text = firstName;

            Label lblLastName = (Label)e.Row.FindControl("lblLastName");
            lblLastName.Text = lastName;

            Label lblUserName = (Label)e.Row.FindControl("lblUserName");
            lblUserName.Text = userName;

            Label lblEmail = (Label)e.Row.FindControl("lblEmail");
            lblEmail.Text = email;

            //Select radio button corresponding to the CMS Role to which belong the user related to the current gridview row.
            RadioButtonList rdolCMSRoles = (RadioButtonList)e.Row.FindControl("rdolCMSRoles");
            rdolCMSRoles.SelectedIndex = rdolCMSRoles.Items.IndexOf(rdolCMSRoles.Items.FindByValue(currentUserRoleId ));


            //Format of radio button value: roleId;username;currentRoleId
            foreach (ListItem role in rdolCMSRoles.Items)
            {
                role.Value += ";" + userName + ";" + currentUserRoleId;
            }

         
            //Current logged user couldn't change his role so we disable all radio buttons with CMS roles in his row.
            //Also we display on his row image to indicate that these are his details.
            Image imgMe = (Image)e.Row.FindControl("imgMe");
            if (userName == ParentControl.CurrentUser.UserName)
            {
                imgMe.Visible = true;

                //Disable all radio buttons for roles.
                foreach (ListItem role in rdolCMSRoles.Items)
                {
                    role.Enabled = false;
                }
            }
            else
            {
                imgMe.Visible = false;
            }

            //If current logged user is Administrator, he could not assign role Super Administrator and couldn't change role of current Super Administrators.
            if (ParentControl.CurrentUser.IsInCMSRole(CMSRole.Administrator))
            {
                //Disable all radio buttons for user who is Super Administrator
                if ((CMSRole)Convert.ToInt32(currentUserRoleId) == CMSRole.SuperAdministrator)
                {
                    //Disable all radio buttons for roles.   
                    foreach (ListItem role in rdolCMSRoles.Items)
                    {
                        role.Enabled = false;
                    }
                }
                //Disable radio button Super Administrator
                rdolCMSRoles.Items.FindByValue(((int)CMSRole.SuperAdministrator).ToString() + ";" + userName + ";" + currentUserRoleId).Enabled = false;
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
    /// <author></author>
    /// <date>21/02/2008</date>
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
    /// <author></author>
    /// <date>21/02/2008</date>
    protected void Pager_PageChanged(object sender, Pager.PagerEventArgs e)
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
    ///     search for CMS users corresponding to the search critera and display them in GridView gvUsers.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>20/02/2008</date>
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
    /// Event handler for event SelectedIndexChanged of the RadioButtonList controls with name "rdolCMSRoles"
    /// in GridView gvUsers.
    /// </summary>
    /// <remarks>
    ///     Assign or unassign CMS role to the user which details are displayed in the row 
    ///     from which is raised the event.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>20/02/2008</date>
    protected void rdolCMSRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            string selectedValue = ((RadioButtonList)sender).SelectedValue;
            string[] args = selectedValue.Split(';');
            int newRoleId = Convert.ToInt32(args[0]);
            string userExternalCode = Convert.ToString(args[1]);
            int oldRoleId = Convert.ToInt32(args[2]);

            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator
                || (currentUserRole == CMSRole.Administrator &&
                    newRoleId != Convert.ToInt32(CMSRole.SuperAdministrator) &&
                    oldRoleId != Convert.ToInt32(CMSRole.SuperAdministrator)))
            {
                if (newRoleId == 0)
                {
                    //None is checked
                    User.RemoveUserFromCMSRole(userExternalCode);
                }
                else
                {
                    //Super Administrator, Administrator or Writer is checked
                    User.AddUserToCMSRole(userExternalCode, (CMSRole)newRoleId);
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
    /// <author></author>
    /// <date>26/06/2008</date>
    private void GetFilterCriteria()
    {
        //Create filtering object.
        User objUser = new User();
        objUser.FirstName = txtFirstName.Text.Trim();
        objUser.LastName = txtLastName.Text.Trim();
        objUser.UserName = txtUserName.Text.Trim();
        objUser.Email = txtEmail.Text.Trim();

        this.UserFilter = objUser;

        //Create list of selected roles.
        List<CMSRole> roles = new List<CMSRole>();
        bool includeNonCMSUsers = false;
        foreach (ListItem item in chklCMSRoles.Items)
        {
            if (item.Selected)
            {
                int itemValue = Convert.ToInt32(item.Value);
                if (itemValue == 0)
                {
                    //Checkbox "None" is selected.
                    includeNonCMSUsers = true;
                }
                else
                {
                    //Checkbox "Super Administrator" or "Administrator" or "Writer" is selected.
                    roles.Add((CMSRole)Convert.ToInt32(itemValue));
                }
            }
        }

        this.RolesFilter = roles;
        this.IncludeNonCMSUsers = includeNonCMSUsers;

    }

    /// <summary>
    /// Search for users corresponding to the criteria entered in the search area (first name, last name, email, user name, roles).
    /// </summary>
    /// <remarks>
    ///     The method get search criteria from properties <see cref="UserFilter"/>,<see cref="RolesFilter"/> and <see cref="IncludeNonCMSUsers"/> passed them to 
    ///     static method List of class <see cref="Melon.Components.CMS.User"/>.
    ///     After that GridView gvUsers is databound with the found users.
    /// </remarks>
    /// <author></author>
    /// <date>19/02/2008</date>
    private void ListUsers()
    {       
        //Search for users. 
        DataTable dtUsers = User.List(this.UserFilter, this.RolesFilter, this.IncludeNonCMSUsers);

        //Display details of found users in GridView gvUsers.
        DataView dvUsers = new DataView(dtUsers);
        if (dtUsers.Rows.Count != 0)
        {
            dvUsers.Sort = this.SortExpression + " " + this.SortDirection;
        }
        
        gvUsers.PageSize = CMSSettings.UsersPageSize;
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
