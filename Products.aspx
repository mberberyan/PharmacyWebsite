<%@ Page Title="" Language="C#" MasterPageFile="~/MPProducts.master" AutoEventWireup="true"
    CodeFile="Products.aspx.cs" Inherits="Products" Theme="Default" %>

<%@ Register TagPrefix="melon" TagName="LatestProducts" Src="~/MC_ProductCatalog/Sources/FELatestProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="TopProducts" Src="~/MC_ProductCatalog/Sources/FETopProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="~/MC_ProductCatalog/Sources/Pager.ascx" %>
<asp:Content ID="cProducts" ContentPlaceHolderID="cphProducts" runat="server">
    <table>
        <tr>
            <td valign="top" colspan="2">
                <div class="landingcontent">
                    <div id="divProductGridError" runat="server" class="mc_pc_short_error_message" visible="false">
                    </div>
                    <!-- Pager for the grid view with product reviews-->
                    <div class="mc_pc_pager">
                        <melon:Pager ID="TopPager" runat="server" ShowItemsDetails="true" />
                    </div>
                    <!-- *** Grid with found from product search *** -->
                    <asp:DataList ID="dlProductGrid" runat="server" RepeatColumns="3" GridLines="None"
                        RepeatDirection="Horizontal" RepeatLayout="Table" ShowHeader="false" CssClass="mc_pc_grid"
                        ItemStyle-CssClass="mc_pc_grid_row" AlternatingItemStyle-CssClass="mc_pc_grid_row_alt"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <div class="mc_pc_grid_row_name">
                                <a id="aName" runat="server" />
                            </div>
                            <div class="mc_pc_grid_row_image">
                                <asp:HyperLink ID="lnkImageProduct" runat="server">
                                    <asp:Image ID="imgProduct" runat="server" />
                                </asp:HyperLink>
                            </div>
                            <div class="mc_pc_grid_row_startprice">
                                <asp:Label ID="lblPrice" runat="server" />
                            </div>
                            <div class="mc_pc_grid_row_description">
                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </td>
        </tr>
        <tr>
            <td >
                <melon:TopProducts ID="ucTopProducts" runat="server" />
            </td>
            <td>
                <melon:LatestProducts ID="ucLatestProducts" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
