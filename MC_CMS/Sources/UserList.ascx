<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserList" CodeFile="UserList.ascx.cs" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.CMS" %>
<%@ Import Namespace="Melon.Components.CMS.Configuration" %>

<asp:Label ID="lblManageUsers" runat="server" meta:resourcekey="lblManageUsers" CssClass="mc_cms_title" />
<!-- *** Search area *** -->
<div class="mc_cms_user_filter">
<table cellpadding="2" width="100%">
<tr class="mc_cms_table_row_padding">
    <td style="padding-left: 0px;">
        <asp:Label ID="lblFirstName" runat="server" CssClass="mc_cms_label" Text="<%$ Resources:FirstName %>" /><br />
        <asp:TextBox ID="txtFirstName" runat="server" CssClass="mc_cms_input_short mc_cms_input_filter_user" /></td>
    <td>
        <asp:Label ID="lblLastName" runat="server" CssClass="mc_cms_label" Text="<%$ Resources:LastName %>" /><br />
        <asp:TextBox ID="txtLastName" runat="server" CssClass="mc_cms_input_short mc_cms_input_filter_user" /></td>
    <td>
        <asp:Label ID="lblUserName" runat="server" CssClass="mc_cms_label" Text="<%$ Resources:UserName %>" /><br />
        <asp:TextBox ID="txtUserName" runat="server" CssClass="mc_cms_input_short mc_cms_input_filter_user" /></td>
    <td>
        <asp:Label ID="lblEmail" runat="server" CssClass="mc_cms_label" Text="<%$ Resources:Email %>" /><br />
        <asp:TextBox ID="txtEmail" runat="server" CssClass="mc_cms_input_long" Width="300" /></td>
    <td rowspan="2" class="mc_cms_user_filter_btns">
        <div class="mc_cms_user_filter_table_divBtnSearch">
            <asp:Button ID="btnSearch" runat="server" CssClass="mc_cms_button mc_cms_btn_61" Width="61"
                meta:resourcekey="btnSearch" CausesValidation="true" ValidationGroup="UsersFilter" />
        </div>
        <div>
            <asp:Button ID="btnReset" runat="server" CssClass="mc_cms_button mc_cms_btn_61" Width="61"
                meta:resourcekey="btnReset" CausesValidation="false" OnClientClick="ResetSearchCriteria();return false;" />
        </div>
    </td>
</tr>
<tr>
    <td colspan="5" style="padding-left: 0px;">
        <asp:Label ID="lblShowCMSRoles" runat="server" CssClass="mc_cms_label mc_cms_checkbox_label"
            meta:resourcekey="lblShowCMSRoles" />
        <asp:CheckBoxList ID="chklCMSRoles" runat="server" CssClass="mc_cms_checkbox" RepeatDirection="Horizontal">
            <asp:ListItem Text='<%$Resources:SuperAdministrator %>' Value="1" Selected="true" />
            <asp:ListItem Text='<%$Resources:Administrator %>' Value="2" Selected="true" />
            <asp:ListItem Text='<%$Resources:Writer %>' Value="3" Selected="true" />
            <asp:ListItem Text='<%$Resources:None %>' Value="0" Selected="false" />
        </asp:CheckBoxList>
    </td>
</tr>
</table>
</div>
<div class="mc_cms_clear">
</div>
<!-- Pager for the grid view with users-->
<melon:Pager ID="TopPager" runat="server" CssClass="mc_cms_pager" ShowItemsDetails="false" />
<!-- *** Grid with found from search users *** -->
<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" GridLines="None"
EmptyDataText='<%$Resources:NoUsers %>' ShowHeader="true" CssClass="mc_cms_grid"
HeaderStyle-CssClass="mc_cms_grid_header" RowStyle-CssClass="mc_cms_grid_row" AlternatingRowStyle-CssClass="mc_cms_grid_alt_row"
AllowPaging="true" PagerSettings-Visible="false" EmptyDataRowStyle-BackColor="white"
AllowSorting="true">
<Columns>
    <asp:TemplateField HeaderStyle-CssClass="mc_cms_userlist_header_col1" ItemStyle-CssClass="mc_cms_userlist_col1">
        <HeaderTemplate>
            <asp:LinkButton ID="lbtnSortByFirstName" runat="server" Text="<%$ Resources:FirstName %>"
                CommandName="Sort" CommandArgument="FirstName" CssClass="mc_cms_header_link"/>
            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
            <asp:Image ID="imgSortDirectionFirstName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="FirstName" %>' />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Label ID="lblFirstName" runat="server" />
            <!-- Image which is displayed only on the row which contains details for the current logged user.-->
            <asp:Image ID="imgMe" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/user.png") %>'
                runat="server" meta:resourcekey="imgMe" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderStyle-CssClass="mc_cms_userlist_header_col2" ItemStyle-CssClass="mc_cms_userlist_col2">
        <HeaderTemplate>
            <asp:LinkButton ID="lbtnSortByLastName" runat="server" Text="<%$ Resources:LastName %>"
                CommandName="Sort" CommandArgument="LastName" CssClass="mc_cms_header_link" />
            <asp:Image ID="imgSortDirectionLastName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="LastName" %>' />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Label ID="lblLastName" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderStyle-CssClass="mc_cms_userlist_header_col3" ItemStyle-CssClass="mc_cms_userlist_col3">
        <HeaderTemplate>
            <asp:LinkButton ID="lbtnSortByUserName" runat="server" Text="<%$ Resources:UserName %>"
                CommandName="Sort" CommandArgument="UserName" CssClass="mc_cms_header_link" />
            <asp:Image ID="imgSortDirectionUserName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="UserName" %>' />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Label ID="lblUserName" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderStyle-CssClass="mc_cms_userlist_header_col4" ItemStyle-CssClass="mc_cms_userlist_col4">
        <HeaderTemplate>
            <asp:LinkButton ID="lbtnSortByEmail" runat="server" Text="<%$ Resources:Email %>"
                CommandName="Sort" CommandArgument="Email" CssClass="mc_cms_header_link" />
            <asp:Image ID="imgSortDirectionEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Email" %>' />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Label ID="lblEmail" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderStyle-CssClass="mc_cms_userlist_header_col5" ItemStyle-CssClass="mc_cms_userlist_col5">
        <HeaderTemplate>
            <asp:LinkButton ID="lbtnSortByRoleID" runat="server" Text="<%$ Resources:CMSRole %>"
                CommandName="Sort" CommandArgument="RoleID" CssClass="mc_cms_header_link" />
            <asp:Image ID="imgSortDirectionRoleID" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="RoleID" %>' />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:RadioButtonList ID="rdolCMSRoles" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                CssClass="mc_cms_radiobutton">
                <asp:ListItem Text='<%$Resources:SuperAdministrator %>' Value="1" Selected="true" />
                <asp:ListItem Text='<%$Resources:Administrator %>' Value="2" Selected="true" />
                <asp:ListItem Text='<%$Resources:Writer %>' Value="3" Selected="false" />
                <asp:ListItem Text='<%$Resources:None %>' Value="0" Selected="true" />
            </asp:RadioButtonList>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>


<script language="javascript" type="text/javascript">
    var txtUserFirstName = document.getElementById('<%= txtFirstName.ClientID %>');
	var txtUserLastName = document.getElementById('<%= txtLastName.ClientID %>');
    var txtUserName = document.getElementById('<%= txtUserName.ClientID %>');
	var txtUserEmail = document.getElementById('<%= txtEmail.ClientID %>');
	
	var chkSuperAdministrator = document.getElementById('<%= chklCMSRoles.ClientID %>' + '_0'); //Super Administrator 
	var chkAdministrator = document.getElementById('<%= chklCMSRoles.ClientID %>' + '_1'); //Administrator
	var chkWriter = document.getElementById('<%= chklCMSRoles.ClientID %>' + '_2'); //Writer
	var chkNone = document.getElementById('<%= chklCMSRoles.ClientID %>' + '_3');//None
</script>

