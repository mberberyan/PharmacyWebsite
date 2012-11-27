<%@ Page Title="" Language="C#" MasterPageFile="MPExample.master" AutoEventWireup="true" CodeFile="LandingPage.aspx.cs" Inherits="MC_ProductCatalog_Example_LandingPage" %>
<%@ Register TagPrefix="melon" TagName="LatestProducts" Src="../Sources/FELatestProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="TopProducts" Src="../Sources/FETopProducts.ascx" %>
<%@ Register TagPrefix="melon" TagName="DynamicPropsBrowse" Src="../Sources/FEDynamicPropsBrowse.ascx" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="../Sources/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td valign="top">
            <div class="leftcontent">
                <melon:DynamicPropsBrowse ID="ucDynamicPropsBrowse" runat="server" />
            </div>
        </td>
        <td valign="top">
            <div class="landingcontent">
                <div id="divProductGridError" runat="server" class="mc_pc_short_error_message" visible="false"></div>
                    <!-- Pager for the grid view with product reviews-->
                    <div class="mc_pc_pager">
                        <melon:Pager ID="TopPager" runat="server" ShowItemsDetails="true"  />
                    </div>
                    <!-- *** Grid with found from product search *** -->
                    <asp:DataList ID="dlProductGrid" runat="server"
                        RepeatColumns="3" 
                        GridLines="None" 
                        RepeatDirection="Horizontal" 
                        RepeatLayout="Table" 
                        ShowHeader="false"      
                        CssClass="mc_pc_grid"                                          
                        ItemStyle-CssClass="mc_pc_grid_row"                        
                        AlternatingItemStyle-CssClass="mc_pc_grid_row_alt"        
                        ItemStyle-HorizontalAlign="Center"     
                        ItemStyle-VerticalAlign="Top"                                                                                   
                        >                                       
                        <ItemTemplate>            
                            <div class="mc_pc_grid_row_name">
                                <a id="aName" runat="server" />
                            </div>
                            <div class="mc_pc_grid_row_image">      
                                <asp:HyperLink ID="lnkImageProduct" runat="server">                    
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
                </div>
        </td>
        <td valign="top">
            <div class="rightcontent">
                <div class="gv_box">
                    <melon:TopProducts ID="ucTopProducts" runat="server" />
                </div>            
                <div class="gv_box">
                    <melon:LatestProducts ID="ucLatestProducts" runat="server" />
                </div>
            </div>            
        </td>
    </tr>    
</table>
</asp:Content>

