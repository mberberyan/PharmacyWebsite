<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductList.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductList" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Register TagPrefix="melon" TagName="DescPanel" Src="DescriptionPanel.ascx" %>

<melon:DescPanel ID="ucDescPanel" runat="server" />
<div class="mc_pc_table_listing_wrapper" onkeydown="SetDefaultButton(document.getElementById(getName('input','btnSearch')), event)">    
    <span class="mc_pc_welcome_page_text_bold"><asp:Localize id="locProductList" runat="server" meta:resourcekey="locProductList"/></span>
    <table class="mc_pc_table_listing" >
        <tr>
            <td colspan="2" align="center"><h3></h3></td>
        </tr>
        <tr>
            <td><asp:Localize ID="locKeywords" runat="server" meta:resourcekey="locKeywords" /></td>
            <td colspan="2"><asp:TextBox ID="txtKeywords" runat="server" CssClass="mc_pc_input_long" /></td>        
        </tr>
        <tr>
            <td><asp:Localize ID="locPriceFrom" runat="server" meta:resourcekey="locPriceFrom" /></td>
            <td colspan="2">
                <asp:TextBox ID="txtPriceFrom" runat="server" CssClass="mc_pc_input_extra_short" /><span class="bottom"><asp:Localize ID="locTo" runat="server" meta:resourcekey="locTo" /></span><asp:TextBox ID="txtPriceTo" runat="server" CssClass="mc_pc_input_extra_short"/>
                <div>
                    <asp:CompareValidator ID="cvPriceFrom" runat="server" meta:resourcekey="cvPriceFrom" ControlToValidate="txtPriceFrom" Display="Dynamic" Type="Double" Operator="DataTypeCheck" ValidationGroup="ProductList"/>
                </div>
                <div>
                    <asp:CompareValidator ID="cvPriceTo" runat="server" meta:resourcekey="cvPriceTo" ControlToValidate="txtPriceTo" Display="Dynamic" Type="Double" Operator="DataTypeCheck" ValidationGroup="ProductList"/>
                </div>
                <div>                
                    <asp:CustomValidator ID="cvComparePrices" runat="server" meta:resourcekey="cvComparePrices" ClientValidationFunction="CheckPriceRange" Display="Dynamic" ValidationGroup="ProductList" />
                </div>                                        
        </tr>
        <tr>
            <td><asp:Localize ID="locSearchIn" runat="server" meta:resourcekey="locSearchIn" /></td>
            <td colspan="2" class="bottom">
                <asp:CheckBoxList ID="cbxlSearchCriteria" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" Selected="True" />
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Description %>" Value="Description" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Tags %>" Value="Tags" Selected="True"/>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locIncludeOnly" runat="server" meta:resourcekey="locIncludeOnly" /></td>
            <td>
                <span class="bottom"><asp:Localize ID="locActive" runat="server" meta:resourcekey="locActive"/></span>
                <asp:DropDownList ID="ddlActive" runat="server" >
                    <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                    <asp:ListItem Text="<%$ Resources: Yes %>" Value="0" />
                    <asp:ListItem Text="<%$ Resources: No %>" Value="1" />
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;
                <span class="bottom"><asp:Localize ID="locInStock" runat="server" meta:resourcekey="locInStock"/></span>
                <asp:DropDownList ID="ddlInStock" runat="server">
                    <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                    <asp:ListItem Text="<%$ Resources: Yes %>" Value="0" />
                    <asp:ListItem Text="<%$ Resources: No %>" Value="1" />
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;
                <span class="bottom"><asp:Localize ID="locFeatured" runat="server" meta:resourcekey="locFeatured" /></span>
                <asp:DropDownList ID="ddlFeatured" runat="server">
                    <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                    <asp:ListItem Text="<%$ Resources: Yes %>" Value="0" />
                    <asp:ListItem Text="<%$ Resources: No %>" Value="1" />
                </asp:DropDownList>                                 
            </td>
            <td align="right">
                <asp:Button ID="btnClear" runat="server" meta:resourcekey="btnClear" CssClass="mc_pc_button mc_pc_btn_61"  OnClientClick="javascript:ResetSearchCriteria(); return false;" />&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearch" CssClass="mc_pc_button mc_pc_btn_61" ValidationGroup="ProductList" />
            </td>
        </tr>
    </table>
</div>
<table>
    <tr>
        <td>
            <!-- Pager for the grid view with product reviews-->
            <melon:Pager ID="TopPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="true" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gvProductList" runat="server"
                AutoGenerateColumns="false"
                GridLines="None"                        
                ShowHeader="true" 
                EmptyDataText
                CssClass="mc_pc_grid"            
                HeaderStyle-CssClass="mc_pc_grid_header"
                RowStyle-CssClass="mc_pc_grid_row" 
                AlternatingRowStyle-CssClass="mc_pc_grid_alt_row"
                AllowPaging="true" 
                PagerSettings-Visible="false" 
                EmptyDataRowStyle-BackColor="#f7f7f7"
                AllowSorting="true"                
                meta:resourcekey="gvProductList"                    
                >
                <Columns>
                    <asp:TemplateField ItemStyle-Width="100"  HeaderStyle-CssClass="mc_pc_grid_header_colFirst">
                        <HeaderTemplate>                        
                            <asp:Localize ID="locCode" runat="server" meta:resourcekey="locCode" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label id="lblCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code") %>' />
                        </ItemTemplate>                     
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="120"  HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                        <HeaderTemplate>
                            <asp:Localize ID="locName" runat="server" meta:resourcekey="locName" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                        </ItemTemplate>                     
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="170"  HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                        <HeaderTemplate>
                            <asp:Localize ID="locDescription" runat="server" meta:resourcekey="locDescription" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                        </ItemTemplate>                     
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="mc_pc_grid_header_colLast">
                        <HeaderTemplate>
                            <asp:Localize ID="locNavigate" runat="server" meta:resourcekey="locNavigate" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btnNavigate" runat="server" meta:resourcekey="btnNavigate" CssClass="mc_pc_button mc_pc_btn_26"  CommandName="Navigate" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            <asp:Button ID="btnRemove" runat="server" meta:resourcekey="btnRemove" CssClass="mc_pc_button mc_pc_btn_26" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteProduct").ToString() + "\");" %>'/>                
                        </ItemTemplate>                    
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    var txtKeywords = document.getElementById('<%= txtKeywords.ClientID %>');
    var txtPriceFrom = document.getElementById('<%= txtPriceFrom.ClientID %>');
    var txtPriceTo = document.getElementById('<%= txtPriceTo.ClientID %>');
    var cvPriceFrom = document.getElementById('<%= cvPriceFrom.ClientID %>');
    var cvPriceTo = document.getElementById('<%= cvPriceTo.ClientID %>');
    var cvComparePrices = document.getElementById('<%= cvComparePrices.ClientID %>');    
    
    var chkCode = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_0'); 
	var chkName = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_1'); 
	var chkDescription = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_2');
	var chkTags = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_3');

	var ddlActive = document.getElementById('<%= ddlActive.ClientID %>');
	var ddlInStock = document.getElementById('<%= ddlInStock.ClientID %>');
	var ddlFeatured = document.getElementById('<%= ddlFeatured.ClientID %>');
	
</script>