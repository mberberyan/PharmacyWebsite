<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FECatalogDetails.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FECatalogDetails" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<div id="divCatalogDetails" class="mc_pc_productDetailsBox">
    <div id="divCatalogName" class="div_mc_pc_productDetailsTitle">
        <asp:Label ID="lblCatalogName" runat="server" CssClass="mc_pc_productDetailsTitle" />
    </div>
    <div id="divDescription" class="mc_pc_catalogDetailsLongDesc">
        <asp:Label ID="lblLongDescription" runat="server" />
    </div>    
    <div id="divCatalogProducts" runat="server" class="mc_pc_productList">        
        <div class="mc_pc_title"> <asp:Label ID="lblProductList" runat="server" meta:resourcekey="lblProductList" /></div>
        <!-- Pager for the grid view with product reviews-->
        <div class="mc_pc_pager">
            <melon:Pager ID="CatalogProductsPager" runat="server" ShowItemsDetails="true" />
        </div>
        <!-- *** Grid with found product reviews *** -->
        <asp:GridView ID="gvCatalogProducts" runat="server"            
                GridLines="None"
                AutoGenerateColumns="False"              
                ShowHeader="false" 
                CssClass="gv_productList"            
                RowStyle-CssClass="gv_productList_row" 
                AlternatingRowStyle-CssClass="gv_productList_alt_row"
                AllowPaging="true" 
                PagerSettings-Visible="false" 
                EmptyDataRowStyle-BackColor="#f7f7f7"
                AllowSorting="true"                                                             
            >
            <Columns>
                <asp:TemplateField ItemStyle-Width="100">                                    
                <ItemTemplate>
                    <div class="productsListImage">
                        <asp:HyperLink ID="hplProduct" runat="server">
                            <asp:Image id="imgProduct" runat="server" />
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="120">                                    
                <ItemTemplate>
                    <div class="productListTitle">
                        <asp:HyperLink id="lbName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'/>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="120">                    
                <ItemTemplate>
                    <asp:Label id="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryList") %>'/>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="80">                    
                <ItemTemplate>
                    <div  class="productListDescription">
                        <asp:Label id="lblPrice" runat="server" />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="200">
                <ItemTemplate>
                    <div  class="productListDescription">
                    <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>                
            </Columns>
        </asp:GridView>
    </div>    
    <div class="gv_pc_back">                    
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="shortButton" />
    </div>
</div>