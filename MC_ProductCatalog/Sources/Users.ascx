<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Users.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Users" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<div id="divSearchDetails" runat="server" onkeydown="SetDefaultButton(document.getElementById(getName('input','btnSearch')), event)">
<table class="mc_pc_table_listing">
    <tr>
        <td class="mc_pc_table_listing_clear_padding">
            <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" /><br />
            <asp:TextBox ID="txtFirstName" runat="server" CssClass="mc_pc_input_short"/></td>
        <td>
            <asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" /><br />
            <asp:TextBox ID="txtLastName" runat="server" CssClass="mc_pc_input_short"/></td>
        <td>
            <asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" /><br />
            <asp:TextBox ID="txtUserName" runat="server" CssClass="mc_pc_input_short"/></td>
        <td>
            <asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" /><br />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="mc_pc_input_short" /></td>            
    </tr>
    <tr>
        <td colspan="3">                
            <asp:Localize ID="locSelectRole" runat="server" meta:resourcekey="locSelectRole" />
            <asp:CheckBoxList ID="cbxlRoles" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" >
                <asp:ListItem Text="<%$ Resources: Administrator %>"  Value="Admin" Selected="True" />
                <asp:ListItem Text="<%$ Resources: None %>"  Value="None" Selected="False" />
            </asp:CheckBoxList>
        </td>
        <td align="right">                
            <asp:Button ID="btnReset" runat="server" meta:resourcekey="btnReset" CssClass="mc_pc_button mc_pc_btn_61" CausesValidation="false" OnClientClick="ResetSearchCriteria();return false;" />                
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearch" CssClass="mc_pc_button mc_pc_btn_61" CausesValidation="true" ValidationGroup="UsersFilter" />                                                        
        </td>
    </tr>
    </table>
</div>  

<table>
    <tr>
        <td>
            <!-- Pager for the grid view with users-->
            <melon:Pager ID="TopPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="true" />
        </td>
    </tr>
    <tr>
        <td>
            <!-- *** Grid with found from search users *** -->
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" GridLines="None"
                EmptyDataText='<%$Resources:NoUsers %>' 
                PageSize="1"
                ShowHeader="true" 
                CssClass="mc_pc_grid"
                HeaderStyle-CssClass="mc_pc_grid_header" 
                RowStyle-CssClass="mc_pc_grid_row" 
                AlternatingRowStyle-CssClass="mc_pc_grid_alt_row"
                AllowPaging="true" PagerSettings-Visible="false" EmptyDataRowStyle-BackColor="#f7f7f7"
                AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="mc_pc_grid_header_colFirst">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByFirstName" runat="server" meta:resourcekey="lbtnSortByFirstName" CommandName="Sort" CommandArgument="FirstName" />
                        <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                        <asp:Image ID="imgSortDirectionFirstName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="FirstName" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFirstName" runat="server" />
                        <!-- Image which is displayed only on the row which contains details for the current logged user.-->
                        <asp:Image ID="imgMe" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"user.png") %>'
                            runat="server" meta:resourcekey="imgMe" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByLastName" runat="server" meta:resourcekey="lbtnSortByLastName" CommandName="Sort" CommandArgument="LastName" />
                        <asp:Image ID="imgSortDirectionLastName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="LastName" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblLastName" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByUserName" runat="server" meta:resoureckey="lbtnSortByUserName" CommandName="Sort" CommandArgument="UserName" />
                        <asp:Image ID="imgSortDirectionUserName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="UserName" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUserName" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByEmail" runat="server" meta:resourcekey="lbtnSortByEmail" CommandName="Sort" CommandArgument="Email" />
                        <asp:Image ID="imgSortDirectionEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Email" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEmail" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  HeaderStyle-CssClass="mc_pc_grid_header_colLast" ItemStyle-HorizontalAlign="left">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByRoleID" runat="server" meta:resourcekey="lbtnSortByRoleID" CommandName="Sort" CommandArgument="isAdmin" />
                        <asp:Image ID="imgSortDirectionRoleID" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="RoleID" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>                        
                        <asp:RadioButtonList ID="rdolAdminRole" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">                        
                            <asp:ListItem Text='<%$Resources:Administrator %>' Value="1" Selected="true" />
                            <asp:ListItem Text='<%$Resources:None %>' Value="0" Selected="true" />
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>


<script language="javascript" type="text/javascript">
    var txtFirstName = document.getElementById('<%= txtFirstName.ClientID %>');
	var txtLastName = document.getElementById('<%= txtLastName.ClientID %>');
    var txtUserName = document.getElementById('<%= txtUserName.ClientID %>');
    var txtEmail = document.getElementById('<%= txtEmail.ClientID %>');
    var chkAdminRole = document.getElementById('<%= cbxlRoles.ClientID %>' + '_0');
    var chkNonadminRole = document.getElementById('<%= cbxlRoles.ClientID %>' + '_1'); 	
</script>