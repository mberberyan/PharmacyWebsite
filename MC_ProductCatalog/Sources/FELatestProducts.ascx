<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FELatestProducts.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FELatestProducts" %>
    <h2><asp:Label ID="lblLatestProducts" runat="server" meta:resourcekey="lblLatestProducts" /></h2>
    <div id="divLatestProductsError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
    <asp:GridView ID="gvLatestProducts" runat="server"            
            GridLines="None"
            AutoGenerateColumns="False" 
            EmptyDataText='<%$Resources:NoLatestProductsErrorMessage %>' 
            ShowHeader="false"             
            AllowPaging="true" 
            PagerSettings-Visible="false"                         
        >
        <Columns>
            <asp:TemplateField>                
                <ItemTemplate>
                    <div class="gv_boxImage">
                        <asp:HyperLink ID="hplLatestProduct" runat="server">
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
