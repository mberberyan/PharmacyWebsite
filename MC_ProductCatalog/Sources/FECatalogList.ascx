<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FECatalogList.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FECatalogList" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<script src='<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/ProductCatalog.js"%>' type="text/javascript"></script>
    <div class="mc_pc_title">
        <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" />
    </div>
    <div id="divCatalogListError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
    <!-- Pager for the grid view with product reviews-->
    <div class="mc_pc_pager">
        <melon:Pager ID="TopPager" runat="server" ShowItemsDetails="true" />
    </div>
    <!-- *** Grid with found from product search *** -->
    <asp:GridView ID="gvCatalogList" runat="server"            
            GridLines="None"
            AutoGenerateColumns="False" 
            EmptyDataText='<%$Resources:NoCatalogsErrorMessage %>' 
            CssClass="gv_productList"
            ShowHeader="false"             
            AllowPaging="true" 
            PagerSettings-Visible="false"             
            AllowSorting="false"                                                             
        >
        <Columns>            
            <asp:TemplateField>                                    
                <ItemTemplate>
                    <div class="productListTitle">
                        <asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />                        
                    </div>                    
                    <div  class="productListDescription">
                        <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                    </div>
                    <div class="productListView">
                        <asp:Button ID="btnViewCatalog" runat="server" meta:resourcekey="btnViewCatalog" CssClass="longButton"/>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
