<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductStatistics.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductStatistics" %>
<asp:Label ID="lblNoStatistics" runat="server" Visible="false" meta:resourcekey="lblNoStatistics" CssClass="mc_pc_short_error_message" />
<table id="tabStatistics" runat="server" class="mc_pc_main" width="500" cellspacing="2" cellpadding="2">
    <tr>
        <td>
            <asp:Localize ID="locTotalViews" runat="server" meta:resourcekey="locTotalViews" />&nbsp;&nbsp;<asp:Label ID="lblTotalViews" runat="server" />
        </td>
        <td>
            <asp:Localize ID="locAddedOn" runat="server" meta:resourcekey="locAddedOn" />&nbsp;&nbsp;<asp:Label ID="lblAddedOn" runat="server" />
        </td>   
    </tr>
    <tr>
        <td>
            <asp:Localize ID="locViewsToday" runat="server" meta:resourcekey="locViewsToday" />&nbsp;&nbsp;<asp:Label ID="lblViewsToday" runat="server" />
        </td>
        <td>
            <asp:Localize ID="locUpdatedOn" runat="server" meta:resourcekey="locUpdatedOn" />&nbsp;&nbsp;<asp:Label ID="lblUpdatedOn" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Localize ID="locViewsMonth" runat="server" meta:resourcekey="locViewsMonth" />&nbsp;&nbsp;<asp:Label ID="lblViewsMonth" runat="server" />
        </td>
        <td>
            <asp:Localize ID="locAddedBy" runat="server" meta:resourcekey="locAddedBy" />&nbsp;&nbsp;<asp:Label ID="lblAddedBy" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Localize ID="locViewsPerDay" runat="server" meta:resourcekey="locViewsPerDay" />&nbsp;&nbsp;<asp:Label ID="lblViewsPerDay" runat="server" />
        </td>
        <td>
            <asp:Localize ID="locUpdatedBy" runat="server" meta:resourcekey="locUpdatedBy" />&nbsp;&nbsp;<asp:Label ID="lblUpdatedBy" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Localize ID="locViewsPerMonth" runat="server" meta:resourcekey="locViewsPerMonth" />&nbsp;&nbsp;<asp:Label ID="lblViewsPerMonth" runat="server" />
        </td>
        <td></td>
    </tr>
</table>