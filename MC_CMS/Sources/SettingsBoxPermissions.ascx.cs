using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS;


/// <summary>
/// Contains layout for displaying permission's node settings.
/// </summary>
///     <para>
///     All web controls from SettingsBoxPermissions are using local resources.
///     To customize them modify resource file SettingsBoxPermissions.resx located in the MC_CMS folder.
///     </para>
public partial class SettingsBoxPermissions : SettingsControl
{
    #region Fileds & Properties

    /// <summary>
    /// Flag whether to display the accessibility options.
    /// </summary>
    /// <remarks>
    /// For folders and external pages it shouldn't be displayed.
    /// </remarks>
    public bool ShowAccessibleForRoles
    {
      set
      {  
            phAccessibleFor.Visible = value; 
            lstAccessibleFor.Visible = value;
            rfvAccessibleFor.Visible = value;
      }
    }

    private List<Permission> _VisibleFor;
    /// <summary>
    /// Gets or sets the permissions for visibility.
    /// </summary>
    public List<Permission> VisibleFor
    {
        get 
        {
            List<Permission> permissions = new List<Permission>();
            foreach (ListItem item in lstVisibleFor.Items)
            {
                if (item.Selected)
                {
                    Permission objPermission = new Permission();
                    objPermission.Name = item.Value;
                    permissions.Add(objPermission);
                }
            }
            return permissions;
        }
        set 
        {
            _VisibleFor = value;
           
        }
    }

    private List<Permission> _AccessibleFor;
    /// <summary>
    /// Gets or sets the permissions for accessibility.
    /// </summary>
    public List<Permission> AccessibleFor
    {
        get
        {
            List<Permission> permissions = new List<Permission>();
            if (this.CurrentNodeType == NodeType.Folder || this.CurrentNodeType == NodeType.StaticExternalPage)
            {
                permissions = this.VisibleFor;
            }
            else
            {
                if (hfAccessibleFor.Value != String.Empty)
                {
                    string[] permissionCodes = hfAccessibleFor.Value.Split(',');
                    foreach (string code in permissionCodes)
                    {
                        Permission objPermission = new Permission();
                        objPermission.Name = code;
                        permissions.Add(objPermission);
                    }
                }
            }
            return permissions;
        }
        set
        {
            _AccessibleFor = value;
        }
    }

    #endregion

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     Loads all possible visibility and accessibility permission options in listboxes and select in then the permissions of the node.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Get all roles.
        DataTable rolePermissionOptions = Role.List();

        if (!IsControlPostBack)
        {
            LoadVisibilityPermissions(rolePermissionOptions);
            SelectVisibilityPermissions();

            LoadAccessibilityPermissions(rolePermissionOptions);
            SelecteAccessibilityPermissions();
        }
        else
        {
            //When there is postback then accessibility options should be recovered as they are before the submit.
            if (!String.IsNullOrEmpty(Request.Form[lstAccessibleFor.UniqueID]))
            {
                LoadAccessibilityPermissions(rolePermissionOptions);
                string[] options = Request.Form[lstAccessibleFor.UniqueID].Split(',');
                foreach (string option in options)
                {
                    ListItem item = lstAccessibleFor.Items.FindByValue(option);
                    if (item != null)
                        lstAccessibleFor.Items.FindByValue(option).Selected = true;
                }
            }
        }
    }


    /// <summary>
    /// Register possible valid values for ListBox lstAccessibleFor. 
    /// The valid values are all values from ListBox lstVisibleFor.
    /// </summary>
    /// <param name="writer"></param>
    /// <author></author>
    /// <date>25/03/2008</date>
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (ListItem item in lstVisibleFor.Items)
        {
            this.Page.ClientScript.RegisterForEventValidation(lstAccessibleFor.UniqueID, item.Value);
        }
    
        base.Render(writer);
    }

    /// <summary>
    /// Event handler for event ReLoadSettings of the user control inherited from 
    /// parent user control <see cref="Melon.Components.CMS.ComponentEngine.SettingsControl"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The event is fired from the parent user control Settings.ascx 
    ///         when node settings are restored from live version. In this case are loaded 
    ///         live version permissions.</para>
    /// </remarks>
    /// <param name="args"></param>
    /// <author></author>
    /// <date>26/03/2008</date>
    protected override void OnReLoadSettings(EventArgs args)
    {
        base.OnReLoadSettings(args);

        //Unselect all currently selected options in listboxes.
        lstVisibleFor.SelectedIndex = -1;
        lstAccessibleFor.SelectedIndex = -1;

        //Get all roles.
        DataTable rolePermissionOptions = Role.List();

        SelectVisibilityPermissions();

        LoadAccessibilityPermissions(rolePermissionOptions);
        SelecteAccessibilityPermissions();
    }

    
    /// <summary>
    /// Load all possible options for visibility permissions in ListBox lstVisibleFor.
    /// </summary>
    /// <param name="rolePermissionOptions">Table with all user roles existing on the web site.</param>
    private void LoadVisibilityPermissions(DataTable rolePermissionOptions)
    {   
        //Fill in options in "Visible For" ListBox.
        DataTable dtVisibilityOptions = rolePermissionOptions.Copy();

        //Add option "All".
        DataRow row0 = dtVisibilityOptions.NewRow();
        row0["Code"] = Convert.ToString(PermissionOption.All);
        row0["Name"] = Convert.ToString(GetLocalResourceObject("All"));
        dtVisibilityOptions.Rows.InsertAt(row0, 0);

        //Add option "Anonymous users only".
        DataRow row1 = dtVisibilityOptions.NewRow();
        row1["Code"] = Convert.ToString(PermissionOption.AnonymousUsersOnly); ;
        row1["Name"] = Convert.ToString(GetLocalResourceObject("AnonymousUsersOnly"));
        dtVisibilityOptions.Rows.InsertAt(row1, 1);

        //Add option "Logged users only".
        DataRow row2 = dtVisibilityOptions.NewRow();
        row2["Code"] = Convert.ToString(PermissionOption.LoggedUsersOnly);
        row2["Name"] = Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"));
        dtVisibilityOptions.Rows.InsertAt(row2, 2);

        lstVisibleFor.DataSource = dtVisibilityOptions;
        lstVisibleFor.DataBind();

        lstVisibleFor.Attributes.Add("onchange", "OnSelectVisibilityOption(this)");
    }

    /// <summary>
    /// Select visibility permissions in Listbox lstVisibleFor which are set for the node. 
    /// </summary>
    private void SelectVisibilityPermissions()
    {
        //Select options in "Visible For" ListBox.
        if ((this._VisibleFor == null) || (this._VisibleFor.Count == 0))
        {
            lstVisibleFor.Items.FindByValue(Convert.ToString(PermissionOption.All)).Selected = true;
        }
        else
        {
            foreach (Permission permission in this._VisibleFor)
            {
                lstVisibleFor.Items.FindByValue(permission.Name).Selected = true;
            }
        }
    }

    /// <summary>
    /// Load all possible options for accessibility permissions in ListBox lstAccessibleFor
    /// depending on which is the selected visibility permission in ListBox lstVisibleFor.
    /// </summary>
    /// <param name="rolePermissionOptions">Table with all user roles existing on the web site.</param>
    private void LoadAccessibilityPermissions(DataTable rolePermissionOptions)
    {
        //Fill in options in "Accessible For" ListBox depending on what is selected in ListBox lstVisibleFor.
        DataTable dtAccessibilityOptions = rolePermissionOptions.Clone();

        if (lstVisibleFor.Items.FindByValue(Convert.ToString(PermissionOption.All)).Selected)
        {
            dtAccessibilityOptions = rolePermissionOptions.Copy();

            //Add option "All".
            DataRow r0 = dtAccessibilityOptions.NewRow();
            r0["Code"] = Convert.ToString(PermissionOption.All);
            r0["Name"] = Convert.ToString(GetLocalResourceObject("All"));
            dtAccessibilityOptions.Rows.InsertAt(r0, 0);

            //Add option "Logged users only".
            DataRow r1 = dtAccessibilityOptions.NewRow();
            r1["Code"] = Convert.ToString(PermissionOption.LoggedUsersOnly);
            r1["Name"] = Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"));
            dtAccessibilityOptions.Rows.InsertAt(r1, 1);
        }
        else if (lstVisibleFor.Items.FindByValue("AnonymousUsersOnly").Selected)
        {
            //Add option "AnonymousUsersOnly".
            DataRow r0 = dtAccessibilityOptions.NewRow();
            r0["Code"] = Convert.ToString(PermissionOption.AnonymousUsersOnly);
            r0["Name"] = Convert.ToString(GetLocalResourceObject("AnonymousUsersOnly"));
            dtAccessibilityOptions.Rows.InsertAt(r0, 0);
        }
        else if (lstVisibleFor.Items.FindByValue(Convert.ToString(PermissionOption.LoggedUsersOnly)).Selected)
        {
            //Add option "LoggedUsersOnly".
            DataRow r0 = dtAccessibilityOptions.NewRow();
            r0["Code"] = Convert.ToString(PermissionOption.LoggedUsersOnly);
            r0["Name"] = Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"));
            dtAccessibilityOptions.Rows.InsertAt(r0, 0);
        }
        else
        {
            foreach (DataRow row in rolePermissionOptions.Rows)
            {
                if (lstVisibleFor.Items.FindByValue(Convert.ToString(row["Code"])).Selected)
                {
                    DataRow roleRow = dtAccessibilityOptions.NewRow();
                    roleRow["Code"] = row["Code"];
                    roleRow["Name"] = row["Name"];
                    dtAccessibilityOptions.Rows.InsertAt(roleRow, dtAccessibilityOptions.Rows.Count);
                }
            }
        }

        lstAccessibleFor.DataSource = dtAccessibilityOptions;
        lstAccessibleFor.DataBind();
        
    }

    /// <summary>
    /// Select accessibility permissions in Listbox lstVisibleFor which are set for the node. 
    /// </summary>
    private void SelecteAccessibilityPermissions()
    {
        //Select options in "Accessible For" ListBox.
        if ((this._AccessibleFor == null) || (this._AccessibleFor.Count == 0))
        {
            //Select the first option.
            lstAccessibleFor.Items[0].Selected = true;
        }
        else
        {
            foreach (Permission permission in this._AccessibleFor)
            {
                lstAccessibleFor.Items.FindByValue(permission.Name).Selected = true;
            }
        }
    }
}
