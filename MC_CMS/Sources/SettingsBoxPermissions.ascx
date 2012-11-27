<%@ Control Language="C#" AutoEventWireup="true" Inherits="SettingsBoxPermissions" CodeFile="SettingsBoxPermissions.ascx.cs" %>
<%@ Import Namespace="Melon.Components.CMS" %>
<%@ Import Namespace="System.Data" %>

<div class="mc_cms_section">
    <h3>
    <asp:Label ID="lblPermissions" runat="server" CssClass="mc_cms_heading" meta:resourcekey="lblPermissions"/>
    </h3>
    <div class="mc_cms_settings_section">
        <asp:HiddenField ID="hfAccessibleFor" runat="server" EnableViewState="false"/>
        <table cellpadding="2" cellspacing="0" width="100%">
            <tr>
                <td class="mc_cms_settings_label_column" valign="top">
                    <asp:Label ID="lblVisibleFor" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblVisibleFor"/>
                    <span class="mc_cms_validator">*</span></td>
                <td>
                    <asp:ListBox ID="lstVisibleFor" runat="server" Rows="5" CssClass="mc_cms_input_short" SelectionMode="Multiple"
                        DataTextField="Name" DataValueField="Code"/></td>
                <td class="mc_cms_settings_label_column" valign="top">
                    <asp:PlaceHolder ID="phAccessibleFor" runat="server">
                        <asp:Label ID="lblAccessibleFor" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblAccessibleFor"/>
                        <span class="mc_cms_validator">*</span>
                    </asp:PlaceHolder></td>
                <td>
                    <asp:ListBox ID="lstAccessibleFor" runat="server" Rows="5" CssClass="mc_cms_input_short" SelectionMode="Multiple"
                        DataTextField="Name" DataValueField="Code"/></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RequiredFieldValidator ID="rfvVisibleFor" runat="server" ControlToValidate="lstVisibleFor" ValidationGroup="NodeSettings" 
                        CssClass="mc_cms_validator" Display="Dynamic" SetFocusOnError="true" meta:resourcekey="rfvVisibleFor"/>
                    <asp:CustomValidator ID="cvVisibleFor" runat="server" ControlToValidate="lstVisibleFor" ValidationGroup="NodeSettings"
                        CssClass="mc_cms_validator" Display="Dynamic" SetFocusOnError="true" ErrorMessage='<%$Resources:InvalidPermissionsSelection %>'
                        ClientValidationFunction="ValidateVisibilitySelections"/></td>
                <td colspan="2">
                    <asp:RequiredFieldValidator ID="rfvAccessibleFor" runat="server" ControlToValidate="lstAccessibleFor" ValidationGroup="NodeSettings" 
                        CssClass="mc_cms_validator" Display="Dynamic" SetFocusOnError="true" meta:resourcekey="rfvAccessibleFor"/>
                    <asp:CustomValidator ID="cvAccessibleFor" runat="server" ControlToValidate="lstAccessibleFor" ValidationGroup="NodeSettings"
                        CssClass="mc_cms_validator" Display="Dynamic" SetFocusOnError="true" ErrorMessage='<%$Resources:InvalidPermissionsSelection %>'
                        ClientValidationFunction="ValidateAccessibilitySelections" /></td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript" language="javascript">

var lstVisibleFor = document.getElementById('<%=lstVisibleFor.ClientID%>');
var lstAccessibleFor = document.getElementById('<%=lstAccessibleFor.ClientID%>');
var hfAccessibleFor = document.getElementById('<%=hfAccessibleFor.ClientID%>');

var codeAll = '<%=Convert.ToString(PermissionOption.All)%>';
var codeAnonymousUsersOnly = '<%=Convert.ToString(PermissionOption.AnonymousUsersOnly)%>';
var codeLoggedUsersOnly = '<%=Convert.ToString(PermissionOption.LoggedUsersOnly)%>';

var textAll = '<%=Convert.ToString(GetLocalResourceObject("All"))%>';
var textAnonymousUsersOnly = '<%=Convert.ToString(GetLocalResourceObject("AnonymousUsersOnly"))%>';
var textLoggedUsersOnly = '<%=Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"))%>';


function GetAllRoles()
{
    //Get all roles.
    <%
        DataTable dt = Role.List();
        int __rolescount = dt.Rows.Count; 
    %>
    var rolesCount = <%= __rolescount%>;
    var roles = new Array(rolesCount);
    
    <%
    for(int i=0;i<__rolescount;i++)
    {
        %>
        roles[<%=i%>] = new Array(2);
        roles[<%=i%>][0] = '<%= dt.Rows[i]["Code"].ToString()%>';
        roles[<%=i%>][1] = '<%= dt.Rows[i]["Name"].ToString()%>';
        <%  
    }
    %>
    
    return roles;
}
</script>