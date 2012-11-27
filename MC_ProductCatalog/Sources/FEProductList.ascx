<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEProductList.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEProductList" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductFilter" Src="FEProductFilter.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<script src='<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/ProductCatalog.js"%>' type="text/javascript"></script>
    <div class="mc_pc_title">
        <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" />
    </div>
    <div id="divProductListError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
    <!-- Filter control to filter product results -->
    <melon:ProductFilter ID="ucProductFitler" runat="server" />
    <!-- Pager for the grid view with product reviews-->
    <div class="mc_pc_pager">
        <melon:Pager ID="TopPager" runat="server" ShowItemsDetails="true" />
    </div>
    <!-- *** Grid with found from product search *** -->
    <asp:GridView ID="gvProductList" runat="server"            
            GridLines="None"
            AutoGenerateColumns="False" 
            EmptyDataText='<%$Resources:NoProductsErrorMessage %>' 
            CssClass="gv_productList"
            ShowHeader="false"                         
            AllowPaging="true" 
            PagerSettings-Visible="false"             
            AllowSorting="true"                                                             
        >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="productsListImage">
                        <asp:HyperLink ID="hplProductImage" runat="server">
                            <asp:Image id="imgProduct" runat="server" />
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField>                
                <ItemTemplate>
                    <div class="productListTitle">
                        <a id="aName" runat="server" />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField>                
                <ItemTemplate>
                    <div  class="productListDescription">
                        <asp:Label id="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryList") %>'/>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField>                
                <ItemTemplate>
                    <div class="mc_pc_grid_row_startprice">
                        <asp:Label id="lblPrice" runat="server" />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField>                
                <ItemTemplate>
                <div  class="productListDescription">
                    <asp:Label id="lblDescription" runat="server" />
                </div>
                </ItemTemplate>                     
            </asp:TemplateField>                
        </Columns>
    </asp:GridView>

    