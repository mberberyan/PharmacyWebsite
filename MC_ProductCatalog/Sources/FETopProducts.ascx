<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FETopProducts.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FETopProducts" %>
<h2><asp:Label ID="lblTopProducts" runat="server" meta:resourcekey="lblTopProducts" /></h2>
<div id="divTopProductsError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
    <asp:GridView ID="gvTopProducts" runat="server"            
            GridLines="None"
            AutoGenerateColumns="False" 
            EmptyDataText='<%$Resources:NoTopProductsErrorMessage %>' 
            ShowHeader="false"             
            AllowPaging="true" 
            PagerSettings-Visible="false"             
            AllowSorting="false"                                                             
        >
        <Columns>
            <asp:TemplateField>                
                <ItemTemplate>
                    <div class="gv_boxImage">
                        <asp:HyperLink ID="hplTopProduct" runat="server">
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
                        <asp:Label id="lblLowestPrice" runat="server" CssClass="productPrice" />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>                
        </Columns>
    </asp:GridView>
