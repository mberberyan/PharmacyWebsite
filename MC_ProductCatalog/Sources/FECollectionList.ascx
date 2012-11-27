<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FECollectionList.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FECollectionList" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>

<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<script src='<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/ProductCatalog.js"%>' type="text/javascript"></script>
    <div class="mc_pc_title">
        <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle"/>
    </div>
    <div id="divCollectionListError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
    <!-- Pager for the grid view with product reviews-->
    <div class="mc_pc_pager">
        <melon:Pager ID="TopPager" runat="server" ShowItemsDetails="true" />
    </div>
    <!-- *** Grid with found from product search *** -->
    <asp:GridView ID="gvCollectionList" runat="server"            
            GridLines="None"
            AutoGenerateColumns="False" 
            EmptyDataText='<%$Resources:NoCollectionsErrorMessage %>' 
            ShowHeader="false" 
            CssClass="gv_productList"                        
            AllowPaging="true" 
            PagerSettings-Visible="false"             
            AllowSorting="true"                                                             
        >
        <Columns>
            <asp:TemplateField ItemStyle-Width="100px">                
                <ItemTemplate>
                    <div class="productsListImage">
                        <asp:Image id="imgCollection" runat="server" />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField>            
                <ItemTemplate>
                    <div class="productListTitle">
                        <asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                    </div>                    
                    <div  class="productListDescription">
                        <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                    </div>
                    <div class="productListView">
                        <asp:Button ID="btnViewCollection" runat="server" CssClass="longButton" meta:resourcekey="btnViewCollection" />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
