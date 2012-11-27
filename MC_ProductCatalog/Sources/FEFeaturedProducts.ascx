<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEFeaturedProducts.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEFeaturedProducts" %>

<div id="divFeaturedProductsError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
<!-- Pager for the grid view with featured products -->

<asp:GridView ID="gvFeaturedProducts" runat="server"            
        GridLines="None"
        AutoGenerateColumns="False" 
        EmptyDataText='<%$Resources:NoFeaturedProductsErrorMessage %>' 
        ShowHeader="false"         
        AllowPaging="true" 
        PagerSettings-Visible="false"         
        AllowSorting="false"                                                             
    >
    <Columns>
        <asp:TemplateField>            
            <ItemTemplate>
                <div class="gv_boxImage">
                    <asp:HyperLink ID="hplProductImage" runat="server">
                        <asp:Image id="imgProduct" runat="server" />
                    </asp:HyperLink>
                </div>
            </ItemTemplate>                     
        </asp:TemplateField>            
        <asp:TemplateField>            
            <ItemTemplate>
                <div class="productName">
                    <a id="aName" runat="server" />
                </div>
                <div class="productCategory">
                    <asp:Label id="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryList") %>'/>
                </div>
                <div>
                    <asp:Label id="lblLowestPrice" runat="server" CssClass="productPrice"/>
                </div>
            </ItemTemplate>                     
        </asp:TemplateField>                
    </Columns>
</asp:GridView>

