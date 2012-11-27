<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MPProducts.master"  CodeFile="ObjectDetails.aspx.cs" Inherits="MC_ProductCatalog_Example_ObjectDetails"  Theme="Default"%>
<%@ Register TagPrefix="melon" TagName="FeaturedProducts" Src="~/MC_ProductCatalog/Sources/FEFeaturedProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="LatestProducts" Src="~/MC_ProductCatalog/Sources/FELatestProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="TopProducts" Src="~/MC_ProductCatalog/Sources/FETopProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="DynamicPropsBrowse" Src="~/MC_ProductCatalog/Sources/FEDynamicPropsBrowse.ascx" %>

<%@Register TagPrefix="melon" TagName="ProductDetails" Src="~/MC_ProductCatalog/Sources/FEProductDetails.ascx" %>
<%@Register TagPrefix="melon" TagName="CatalogDetails" Src="~/MC_ProductCatalog/Sources/FECatalogDetails.ascx" %>
<%@Register TagPrefix="melon" TagName="BundleDetails" Src="~/MC_ProductCatalog/Sources/FEBundleDetails.ascx" %>
<%@Register TagPrefix="melon" TagName="CollectionDetails" Src="~/MC_ProductCatalog/Sources/FECollectionDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphProducts" Runat="Server">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td valign="top">
            <div id="divNoObject" runat="server" visible="false">
                <asp:Literal ID="litNoObject" runat="server" Text="<%$ Resources: NoObjectDetails %>" />
            </div>
            <asp:Panel ID="plhObjectDetails" runat="server">
                <melon:ProductDetails ID="ucProductDetails" runat="server" Visible="false" />
                <melon:CatalogDetails ID="ucCatalogDetails" runat="server" Visible="false" />
                <melon:BundleDetails ID="ucBundleDetails" runat="server" Visible="false" />
                <melon:CollectionDetails ID="ucCollectionDetails" runat="server" Visible="false" />
            </asp:Panel>           
        </td>
    </tr>
</table>
</asp:Content>
