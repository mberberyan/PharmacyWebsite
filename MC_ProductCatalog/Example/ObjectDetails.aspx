<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MPExample.master"  CodeFile="ObjectDetails.aspx.cs" Inherits="MC_ProductCatalog_Example_ObjectDetails" %>
<%@ Register TagPrefix="melon" TagName="FeaturedProducts" Src="../Sources/FEFeaturedProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="LatestProducts" Src="../Sources/FELatestProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="TopProducts" Src="../Sources/FETopProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="DynamicPropsBrowse" Src="../Sources/FEDynamicPropsBrowse.ascx" %>

<%@Register TagPrefix="melon" TagName="ProductDetails" Src="../Sources/FEProductDetails.ascx" %>
<%@Register TagPrefix="melon" TagName="CatalogDetails" Src="../Sources/FECatalogDetails.ascx" %>
<%@Register TagPrefix="melon" TagName="BundleDetails" Src="../Sources/FEBundleDetails.ascx" %>
<%@Register TagPrefix="melon" TagName="CollectionDetails" Src="../Sources/FECollectionDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td valign="top">                   
            <div class="leftcontent">
                <melon:DynamicPropsBrowse ID="ucDynamicPropsBrowse" runat="server" />
                <div class="gv_box">
                    <melon:FeaturedProducts ID="ucFeaturedProducts" runat="server" ProductsPageSize="Two" />
                </div>
                <div class="gv_box">
                    <melon:LatestProducts ID="ucLatestProducts" runat="server" ProductsCount="Two" />
                </div>
                <div class="gv_box">
                    <melon:TopProducts ID="ucTopProducts" runat="server" ProductsCount="Two" />
                </div>
            </div>
        </td>
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
