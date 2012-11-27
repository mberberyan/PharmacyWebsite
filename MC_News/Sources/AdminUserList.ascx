<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminUserList.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.UserList" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="AdminPager.ascx" %>
<%@ Import Namespace="Melon.Components.News" %>

<asp:Label ID="lblManageUsers" runat="server" meta:resourcekey="lblManageUsers" CssClass="mc_news_title" />
<!-- *** Search area *** -->
<div class="mc_news_user_filter">
<table cellpadding="2" onkeydown="fnTrapKD(btnSearch,event)" width="100%">
    <tr class="mc_news_table_row_padding">
        <td>
            <asp:Label ID="lblFirstName" runat="server" CssClass="mc_news_label" Text="<%$ Resources:FirstName %>" /><br />
            <asp:TextBox ID="txtFirstName" runat="server" CssClass="mc_news_input_short" MaxLength="256" Width="205"/></td>
        <td>
            <asp:Label ID="lblLastName" runat="server" CssClass="mc_news_label" Text="<%$ Resources:LastName %>" /><br />
            <asp:TextBox ID="txtLastName" runat="server" CssClass="mc_news_input_short" MaxLength="256" Width="205"/></td>
        <td>
            <asp:Label ID="lblUserName" runat="server" CssClass="mc_news_label" Text="<%$ Resources:UserName %>" /><br />
            <asp:TextBox ID="txtUserName" runat="server" CssClass="mc_news_input_short" MaxLength="256" Width="205"/></td>
        <td>
            <asp:Label ID="lblEmail" runat="server" CssClass="mc_news_label" Text="<%$ Resources:Email %>" /><br />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="mc_news_input_long" MaxLength="256"/></td>
        <td rowspan="2" class="mc_news_user_filter_btns">
            <div class="mc_news_user_filter_table_divBtnSearch">
                <asp:Button ID="btnSearch" runat="server" CssClass="mc_news_button mc_news_btn_61" meta:resourcekey="btnSearch"
                    CausesValidation="true" ValidationGroup="UsersFilter" />
            </div>
            <div>
                <asp:Button ID="btnReset" runat="server" CssClass="mc_news_button mc_news_btn_61" meta:resourcekey="btnReset"
                    CausesValidation="false" OnClientClick="ResetUserSearchCriteria();return false;" />
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Label ID="lblShowUsersWithRoles" runat="server" CssClass="mc_news_label mc_news_lbl_checkbox_roles"
                meta:resourcekey="lblShowUsersWithRoles" />
            <asp:CheckBoxList ID="chklUserRoles" runat="server" CssClass="mc_news_checkbox_roles" RepeatDirection="Horizontal">
                <asp:ListItem Text='<%$Resources:Administrator %>' Value="1" Selected="true" />
                <asp:ListItem Text='<%$Resources:Writer %>' Value="2" Selected="true" />
                <asp:ListItem Text='<%$Resources:None %>' Value="0" Selected="false" />
            </asp:CheckBoxList>
        </td>
    </tr>
</table>
</div>
<!-- Pager for the grid view with users-->
<melon:Pager ID="TopPager" runat="server" CssClass="mc_news_pager" ShowItemsDetails="false" />
<!-- *** Grid with found from search users *** -->
<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" GridLines="None"
    EmptyDataText='<%$Resources:NoUsers %>' ShowHeader="true" CssClass="mc_news_grid" EmptyDataRowStyle-BackColor="white"
    HeaderStyle-CssClass="mc_news_grid_header" RowStyle-CssClass="mc_news_grid_row" AlternatingRowStyle-CssClass="mc_news_grid_alt_row"
    AllowPaging="true" PagerSettings-Visible="false" AllowSorting="true">
    <Columns>
        <asp:TemplateField HeaderStyle-CssClass="mc_news_userlist_header_col1" ItemStyle-CssClass="mc_news_userlist_col1">
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByFirstName" runat="server" Text="<%$ Resources:FirstName %>"
                    CommandName="Sort" CommandArgument="FirstName" CssClass="mc_news_header_link" />
                <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                <asp:Image ID="imgSortDirectionFirstName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="FirstName" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblFirstName" runat="server" />
                <!-- Image which is displayed only on the row which contains details for the current logged user.-->
                <asp:Image ID="imgMe" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/user.png") %>'
                    runat="server" meta:resourcekey="imgMe" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-CssClass="mc_news_userlist_header_col2" ItemStyle-CssClass="mc_news_userlist_col2">
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByLastName" runat="server" Text="<%$ Resources:LastName %>"
                    CommandName="Sort" CommandArgument="LastName" CssClass="mc_news_header_link" />
                <asp:Image ID="imgSortDirectionLastName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="LastName" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblLastName" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-CssClass="mc_news_userlist_header_col3" ItemStyle-CssClass="mc_news_userlist_col3">
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByUserName" runat="server" Text="<%$ Resources:UserName %>"
                    CommandName="Sort" CommandArgument="UserName" CssClass="mc_news_header_link"/>
                <asp:Image ID="imgSortDirectionUserName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="UserName" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblUserName" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-CssClass="mc_news_userlist_header_col4" ItemStyle-CssClass="mc_news_userlist_col4">
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByEmail" runat="server" Text="<%$ Resources:Email %>"
                    CommandName="Sort" CommandArgument="Email" CssClass="mc_news_header_link"/>
                <asp:Image ID="imgSortDirectionEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Email" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblEmail" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-CssClass="mc_news_userlist_header_col5" ItemStyle-CssClass="mc_news_userlist_col5"
            HeaderText="<%$Resources:AdditionalInfo %>">
            <ItemTemplate>
                <asp:Label ID="lblAdditionalInfo" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-CssClass="mc_news_userlist_header_col6" ItemStyle-CssClass="mc_news_userlist_col6">
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByRoleID" runat="server" Text="<%$ Resources:NewsRole %>"
                    CommandName="Sort" CommandArgument="RoleID" CssClass="mc_news_header_link" />
                <asp:Image ID="imgSortDirectionRoleID" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="RoleID" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:RadioButtonList ID="rdolUserRoles" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                    CssClass="mc_news_radiobutton" ValidationGroup="UsersFilter">
                    <asp:ListItem Text='<%$Resources:Administrator %>' Value="1" Selected="true" />
                    <asp:ListItem Text='<%$Resources:Writer %>' Value="2" Selected="true" />
                    <asp:ListItem Text='<%$Resources:None %>' Value="0" Selected="false" />
                </asp:RadioButtonList>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<script language="javascript" type="text/javascript">
    var txtUserFirstNameID = '<%= txtFirstName.ClientID %>';
	var txtUserLastNameID = '<%= txtLastName.ClientID %>';
    var txtUserNameID = '<%= txtUserName.ClientID %>';
	var txtUserEmailID = '<%= txtEmail.ClientID %>';
	
	var chkAdministratorID = '<%= chklUserRoles.ClientID %>' + '_0'; //Administrator
	var chkWriterID = '<%= chklUserRoles.ClientID %>' + '_1'; //Writer
	var chkNoneID = '<%= chklUserRoles.ClientID %>' + '_2';//None

    var btnSearch = document.getElementById('<%= btnSearch.ClientID %>');
</script>

