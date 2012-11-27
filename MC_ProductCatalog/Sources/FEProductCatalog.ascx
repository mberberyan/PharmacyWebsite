<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEProductCatalog.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEProductCatalog" %>
<%@ Register TagPrefix="melon" TagName="AdvancedSearch" Src="../Sources/FEAdvancedSearch.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductList" Src="../Sources/FEProductList.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductGrid" Src="../Sources/FEProductGrid.ascx" %>
<%@ Register TagPrefix="melon" TagName="BundleList" Src="../Sources/FEBundleList.ascx" %>
<%@ Register TagPrefix="melon" TagName="CatalogList" Src="../Sources/FECatalogList.ascx" %>
<%@ Register TagPrefix="melon" TagName="CollectionList" Src="../Sources/FECollectionList.ascx" %>
<%@ Register TagPrefix="melon" TagName="FeaturedProducts" Src="../Sources/FEFeaturedProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="LatestProducts" Src="../Sources/FELatestProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="TopProducts" Src="../Sources/FETopProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="DynamicPropsBrowse" Src="../Sources/FEDynamicPropsBrowse.ascx" %>
<table cellpadding="0" cellspacing="0">
    <tr>
       
        <td valign="top">
            <table>                        
                <tr>
                    <td valign="top">
                        <melon:AdvancedSearch ID="ucAdvancedSearch" runat="server" />
                    </td>
                </tr>            
                <tr>
                    <td>
                        <asp:Panel ID="plhObjectList" runat="server" CssClass="centercontent">                        
                            <melon:ProductList ID="ucProductList" runat="server" />                        
                            <melon:ProductGrid ID="ucProductGrid" runat="server" />                        
                            <melon:CatalogList ID="ucCatalogList" runat="server" />                        
                            <melon:BundleList ID="ucBundleList" runat="server" />                        
                            <melon:CollectionList ID="ucCollectionList" runat="server" />                        
                        </asp:Panel>
                    </td>
                </tr>            
            </table>
        </td>                
    </tr>                 
</table>