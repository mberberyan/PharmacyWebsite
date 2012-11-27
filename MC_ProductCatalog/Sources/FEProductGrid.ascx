<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEProductGrid.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEProductGrid" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductFilter" Src="FEProductFilter.ascx" %>
    <div class="mc_pc_title">
        <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" />
    </div>
    <div id="divProductGridError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
    <!-- Filter control to filter product results -->
    <melon:ProductFilter ID="ucProductFitler" runat="server" />
    <!-- Pager for the grid view with product reviews-->
    <div class="mc_pc_pager">
        <melon:Pager ID="TopPager" runat="server" ShowItemsDetails="true" />
    </div>
    <!-- *** Grid with found from product search *** -->
    <asp:DataList ID="dlProductGrid" runat="server"
        RepeatColumns="6" 
        GridLines="None" 
        RepeatDirection="Horizontal" 
        RepeatLayout="Table" 
        ShowHeader="true"        
        CssClass="mc_pc_grid"
        ItemStyle-CssClass="mc_pc_grid_row"
        ItemStyle-Width="100"
        AlternatingItemStyle-CssClass="mc_pc_grid_row_alt"        
        ItemStyle-HorizontalAlign="Center"                
        >                       
        <ItemTemplate>            
            <div class="mc_pc_grid_row_name">           
                <a id="aName" runat="server" />
            </div>
            <div class="productsListImage">
                <asp:HyperLink ID="hplProductImage" runat="server">
                    <asp:Image id="imgProduct" runat="server" />
                </asp:HyperLink>
            </div>
            <div class="mc_pc_grid_row_startprice">
                <asp:Label id="lblPrice" runat="server" />
            </div>
            <div class="mc_pc_grid_row_description">
                <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
            </div>
        </ItemTemplate>
    </asp:DataList>
