<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductSet.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductSet" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<asp:HiddenField ID="hfProductSetCategoryList" runat="server" />
<asp:HiddenField ID="hfProductSetCategoryName" runat="server" />

<span class="mc_pc_welcome_page_text_bold"><%= String.Format(this.GetLocalResourceObject(SelectedObjectType == ComponentObjectEnum.Product ? "AddRelatedProductsMessage" : "AddProductsMessage").ToString(), SelectedObjectType.ToString())%></span>
<table  class="mc_pc_table_listing" onkeydown="SetDefaultButton(document.getElementById(getName('input','btnSearch')), event)">
    <tr>
        <td><asp:Localize ID="locCategory" runat="server" meta:resourcekey="locCategory" /></td>
        <td colspan="2">            
            <div class="mc_pc_table_category_wrapper">
                <asp:ListBox ID="lbCategoryList" runat="server" 
                    Width="200px"                             
                    SelectionMode="Multiple"                                                                               
                >                        
                </asp:ListBox>
            </div>                    
            <asp:Button ID="btnAddCategory" runat="server" meta:resourcekey="btnAddCategory" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick="javascript:AddProductSetCategories();return false;" />
            <div id="divCategoryListSearch" class="mc_pc_table_category_list hidden">                    
            </div>
        </td>
    </tr>
    <tr>
        <td><asp:Localize ID="locKeywords" runat="server" meta:resourcekey="locKeywords" /></td>
        <td colspan="2"><asp:TextBox ID="txtKeywords" runat="server" CssClass="mc_pc_input_long" /></td>        
    </tr>    
    <tr>
        <td><asp:Localize ID="locPriceBetween" runat="server" meta:resourcekey="locPriceBetween" /></td>
        <td>
            <asp:TextBox ID="txtPriceFrom" runat="server" CssClass="mc_pc_input_extra_short"/><span class="mc_pc_space_bottom"><asp:Localize ID="locAnd" runat="server" meta:resourcekey="locAnd" /></span><asp:TextBox ID="txtPriceTo" runat="server" CssClass="mc_pc_input_extra_short"/>
            <div>
                <asp:CompareValidator ID="cvPriceFrom" runat="server" meta:resourcekey="cvPriceFrom" ControlToValidate="txtPriceFrom" Display="Dynamic" Type="Double" Operator="DataTypeCheck" ValidationGroup="Search"/>
            </div>
            <div>
                <asp:CompareValidator ID="cvPriceTo" runat="server" meta:resourcekey="cvPriceTo" ControlToValidate="txtPriceTo" Display="Dynamic" Type="Double" Operator="DataTypeCheck" ValidationGroup="Search"/>
            </div>
            <div>
                <asp:CustomValidator ID="cvComparePrices" runat="server" meta:resourcekey="cvComparePrices" ClientValidationFunction="CheckPriceRange" Display="Dynamic" ValidationGroup="Search" />
            </div>
        </td>
        <td></td>            
    </tr>
    <tr>
        <td><asp:Localize ID="locSearchIn" runat="server" meta:resourcekey="locSearchIn" /></td>
        <td colspan="2">
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
        <td colspan="2">
            <div class="left">
            <asp:Localize ID="locActive" runat="server" meta:resourcekey="locActive"/>
            <br />
            <asp:DropDownList ID="ddlActive" runat="server">
                <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                <asp:ListItem Text="<%$ Resources: Yes %>" Value="1" />
                <asp:ListItem Text="<%$ Resources: No %>" Value="0" />
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            </div>
            
            <div class="left">
            <asp:Localize ID="locInStock" runat="server" meta:resourcekey="locInStock"/>
            <br />
            <asp:DropDownList ID="ddlInStock" runat="server">
                <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                <asp:ListItem Text="<%$ Resources: Yes %>" Value="1" />
                <asp:ListItem Text="<%$ Resources: No %>" Value="0" />
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            </div>
            
            <div class="left">
            <asp:Localize ID="locFeatured" runat="server" meta:resourcekey="locFeatured" />
            <br />
            <asp:DropDownList ID="ddlFeatured" runat="server">
                <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                <asp:ListItem Text="<%$ Resources: Yes %>" Value="1" />
                <asp:ListItem Text="<%$ Resources: No %>" Value="0" />
            </asp:DropDownList>
            </div>
            <div class="clear"></div>
        </td>        
    </tr>
    <tr>
        <td colspan="2">
            <span class="mc_pc_comment"><%= String.Format(this.GetLocalResourceObject("NotIncludeObjectProductsMessage").ToString(), SelectedObjectType.ToString())  %></span>
        </td>
        <td align="right">
            <asp:Button ID="btnClear" runat="server" meta:resourcekey="btnClear" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick="javascript:ResetSearchCriteria(); return false;" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearch" CssClass="mc_pc_button mc_pc_btn_61" ValidationGroup="Search" />
        </td>
    </tr>
</table>

<div id="divProductSetSearch">        
    <div class="mc_pc_tabProductSetResults_wrapper">
        <!-- Expand/Collapse Advanced Search -->
        <asp:HiddenField ID="hfProductSetSearchStatus" runat="server" />
        <a id="lnkProductSetSearch" class="mc_pc_lnk_expand" onclick="CollapseExpandProductSetSearchArea();">
            <asp:Label ID="lblProductSetSearch" runat="server" meta:resourcekey="lblProductSetSearch" />
        </a>
    </div>
    <table id="tabProductSetSearchResult" class="mc_pc_tabProductSetResults">        
        <tr>
            <td>
                <!-- Pager for the grid view with product results -->                
                <melon:Pager ID="ProductSetPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="false" />                
            </td>
        </tr>
        <tr>
            <td>                       
                <asp:GridView ID="gvProductList" runat="server"                        
                        AutoGenerateColumns="False" 
                        GridLines="None"
                        EmptyDataText='<%$Resources:NoProductsErrorMessage %>'
                        EmptyDataRowStyle-BackColor="#f7f7f7" 
                        ShowHeader="true" 
                        CssClass="mc_pc_grid"
                        HeaderStyle-CssClass="mc_pc_grid_header" 
                        RowStyle-CssClass="mc_pc_grid_row" 
                        AlternatingRowStyle-CssClass="mc_pc_grid_alt_row"
                        AllowPaging="true" 
                        PagerSettings-Visible="false"                         
                        AllowSorting="true"      
                        Width="700px"                                          
                    >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="100" HeaderStyle-Width="100" HeaderStyle-CssClass="mc_pc_grid_header_colFirst">
                            <HeaderTemplate>                        
                                <asp:Localize ID="locCode" runat="server" meta:resourcekey="locCode" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hfProductId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                <asp:Label id="lblCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="140" HeaderStyle-Width="140" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locName" runat="server" meta:resourcekey="locName" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="140" HeaderStyle-Width="140" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locCategoryGrid" runat="server" meta:resourcekey="locCategoryGrid" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblCategoryList" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryList") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="40" HeaderStyle-Width="40" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locPrice" runat="server" meta:resourcekey="locPrice" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblPrice" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem, "CommonPrice")!=DBNull.Value ? (DataBinder.Eval(Container.DataItem, "CommonPrice")+ " " + ProductCatalogSettings.Currency) : "") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>                        
                        <asp:TemplateField ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locDescription" runat="server" meta:resourcekey="locDescription" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField >
                        <asp:TemplateField ItemStyle-Width="80" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="mc_pc_grid_header_colLast">                    
                            <ItemTemplate>
                                <asp:Button ID="btnAddToObject" runat="server" meta:resourcekey="btnAddToObject" CssClass="mc_pc_button mc_pc_btn_61" CommandName="Add" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />                        
                            </ItemTemplate>                    
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnAddAll" runat="server" meta:resourcekey="btnAddAll" CssClass="mc_pc_button mc_pc_btn_61" Visible="false" />
                <asp:Button ID="btnAddAllFromPage" runat="server" meta:resourcekey="btnAddAllFromPage" CssClass="mc_pc_button mc_pc_btn_106" Visible="false"/>
            </td>
        </tr>
    </table>
</div>
<div id="divObjectProducts" class="mc_pc_grid_wrapper">    
    <span class="mc_pc_welcome_page_text_bold"><%= this.GetLocalResourceObject(SelectedObjectType == ComponentObjectEnum.Product ? "AddedRelatedProductsMessage" : "AddedProductsMessage").ToString()%></span>
    <div>
        <melon:Pager ID="ObjectProductListPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="false" />
    </div>
    <table>
        <tr>
            <td>
                <asp:GridView ID="gvObjectProductsList" runat="server"
                    PageSize="10"
                    AutoGenerateColumns="False" 
                    GridLines="None"             
                    ShowHeader="true" 
                    CssClass="mc_pc_grid"
                    HeaderStyle-CssClass="mc_pc_grid_header" 
                    RowStyle-CssClass="mc_pc_grid_row" 
                    AlternatingRowStyle-CssClass="mc_pc_grid_alt_row"
                    AllowPaging="true" 
                    PagerSettings-Visible="false" 
                    EmptyDataRowStyle-BackColor="#f7f7f7"
                    AllowSorting="true"
                    Width="700"
                >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="mc_pc_grid_header_colFirst">
                            <HeaderTemplate>                        
                                <asp:Localize ID="locCode2" runat="server" meta:resourcekey="locCode2" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locName2" runat="server" meta:resourcekey="locName2" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locCategoryGrid2" runat="server" meta:resourcekey="locCategoryGrid2" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblCategoryList" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryList") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="20" HeaderStyle-Width="40" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locPrice2" runat="server" meta:resourcekey="locPrice2" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblPrice" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem, "CommonPrice")!=DBNull.Value ? (DataBinder.Eval(Container.DataItem, "CommonPrice")+ " " + ProductCatalogSettings.Currency) : "") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="40" HeaderStyle-Width="40" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locAppliedDiscounts" runat="server" meta:resourcekey="locAppliedDiscounts" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblDiscountStr" runat="server" />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="200" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locDescription2" runat="server" meta:resourcekey="locDescription2" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField >
                        <asp:TemplateField ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="mc_pc_grid_header_colLast">                    
                            <ItemTemplate>
                                <asp:Button ID="btnRemoveFromObject" runat="server" meta:resourcekey="btnRemoveFromObject" CssClass="mc_pc_button mc_pc_btn_61" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageRemoveProduct").ToString() + "\");" %>' />                        
                            </ItemTemplate>                    
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnRemoveAll" runat="server" meta:resourcekey="btnRemoveAll" CssClass="mc_pc_button mc_pc_btn_61" CommandName="RemoveAll"/>
            </td>
        </tr>
    </table>    
</div>
<script language="javascript" type="text/javascript">
    var hfProductSetSearchStatus = document.getElementById('<%= hfProductSetSearchStatus.ClientID %>');

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

	var lbCategoryList = document.getElementById('<%= lbCategoryList.ClientID %>');
	var hfProductSetCategoryList = document.getElementById('<%= hfProductSetCategoryList.ClientID %>');
	var hfProductSetCategoryName = document.getElementById('<%= hfProductSetCategoryName.ClientID %>');
	var divCategoryListSearch = document.getElementById('divCategoryListSearch');
	
    addLoadEvent(
        RenderSelectedCategoryListSearch, 
        document.getElementById('<%= hfProductSetCategoryList.ClientID %>'), 
        document.getElementById('<%= hfProductSetCategoryName.ClientID %>'),
        document.getElementById('divCategoryListSearch'),
        '<%= "http://"+(Request.Url.AbsoluteUri.Contains("localhost") ? "localhost": Request.Url.Host) + Page.ResolveUrl(ProductCatalogSettings.BasePath) %>'
        );
        
        function AddProductSetCategories()
        {
            AddCategoryToList(
                document.getElementById('<%= hfProductSetCategoryList.ClientID %>'), 
                document.getElementById('<%= hfProductSetCategoryName.ClientID %>'), 
                document.getElementById('<%= lbCategoryList.ClientID %>'),
                document.getElementById('divCategoryListSearch'),
                'ucProductSet',
                '<%= "http://"+(Request.Url.AbsoluteUri.Contains("localhost") ? "localhost": Request.Url.Host) + Page.ResolveUrl(ProductCatalogSettings.BasePath) %>'
            );
        }

</script>