<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FESimpleSearch.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FESimpleSearch" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<script src='<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/ProductCatalog.js"%>'></script>

<table onkeydown="SetDefaultButton(document.getElementById(getName('input','btnSearch')), event)">
    <tr>
        <td><asp:Localize ID="locKeyword" runat="server" meta:resourcekey="locKeyword" /></td>
        <td>
            <asp:TextBox ID="txtSimpleSearchKeywords" runat="server" CssClass="mc_pc_input_short"/>            
        </td>    
        <td align="right">
            <asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearch" CssClass="searchButton" ValidationGroup="Search" CausesValidation="true"/>
        </td>
    </tr>
    <tr>
        <td colspan="3">            
            <asp:RegularExpressionValidator ID="revSimpleSearch" runat="server" ControlToValidate="txtSimpleSearchKeywords" Display="Dynamic" SetFocusOnError="true"
                    CssClass="mc_pc_validator" meta:resourcekey="revSimpleSearch" ValidationGroup="Search"
                    ValidationExpression="[a-zA-Zа-яА-Я0-9\'\x22\-_\s,.]*"/>            
        </td>
    </tr>
    <tr>
        <td colspan="3" align="right">
            <asp:HyperLink ID="lnkAdvancedSearch" runat="server" Text="Advanced Search" NavigateUrl="~/ObjectList.aspx?objType=AdvancedSearch" />
        </td>
    </tr>
</table>
<script type="text/javascript" language="javascript">
    var txtSimpleSearchKeywords = document.getElementById('<%= txtSimpleSearchKeywords.ClientID %>');    
</script>