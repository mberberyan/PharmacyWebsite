<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Admin_Controls_Header" %>
<div class="header">
    <div class="left">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td id="tdCMS" class="menu_item_first" runat="server">
                    <a href="Default.aspx">CMS</a>
                </td>
                <td id="tdProductCatalog" class="menu_item_last" runat="server">
                    <a href="ProductCatalog.aspx">Product Catalog</a>
                </td>
                <td id="tdNews" class="menu_item_last" runat="server">
                    <a href="News.aspx">News</a>
                </td>
            </tr>
        </table>
    </div>
    <div class="right login_status">
        <asp:LoginStatus ID="cntrlLoginStatus" runat="server" OnLoggedOut="cntrlLoginStatus_LoggedOut" />
    </div>
    <div class="clear">
    </div>
</div>
