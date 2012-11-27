<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEAdvancedSearch.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEAdvancedSearch" %>
<asp:HiddenField ID="hfAdvancedSearchCategoryList" runat="server" />
<asp:HiddenField ID="hfAdvancedSearchCategoryName" runat="server" />
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<%@ Import Namespace="System.Web" %>

<script src='<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/ProductCatalog.js"%>'></script>
<div id="divAdvancedSearch" class="mc_pc_panel_advancedSearch" >        
<table>
    <tr>
        <th colspan="2" align="left"><h3><asp:Localize ID="locAdvancedSearch" runat="server" meta:resourcekey="locAdvancedSearch" /></h3></th>
    </tr>
    <tr>
        <td><asp:Localize ID="locKeyword" runat="server" meta:resourcekey="locKeyword" /></td>
        <td><asp:TextBox ID="txtAdvancedSearchKeywords" runat="server" CssClass="mc_pc_input_short"/></td>
        <td class="mc_pc_advSearch_Category" rowspan="4"><asp:Localize ID="locCategory" runat="server" meta:resourcekey="locCategory" /></td>
        <td rowspan="4">            
            <div style="overflow:auto;width:200px;">
                <asp:ListBox ID="lbCategoryList" runat="server" 
                    Width="200px"                             
                    SelectionMode="Multiple"                                                                               
                >                        
                </asp:ListBox>
            </div>                    
            <asp:Button ID="btnAddCategory" runat="server" meta:resourcekey="btnAddCategory" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick="javascript:AddAdvancedSearchCategories();return false;" />            
        </td>
    </tr>    
    <tr>        
        <td><asp:Localize ID="locPrice" runat="server" meta:resourcekey="locPrice" /></td>
        <td>
            <asp:TextBox ID="txtPriceFrom" runat="server" CssClass="mc_pc_input_extra_short"/><asp:Localize ID="locTo" runat="server" meta:resourcekey="locTo" /><asp:TextBox ID="txtPriceTo" runat="server" CssClass="mc_pc_input_extra_short"/>
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
    </tr>
    <tr>
        <td><asp:Localize ID="locSearchIn" runat="server" meta:resourcekey="locSearchIn" /></td>
        <td>
            <asp:CheckBoxList ID="cbxObjectType" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                <asp:ListItem Text="Products" Value="ProductList" Selected="True" />
                <asp:ListItem Text="Bundle" Value="BundleList" Selected="True" />
                <asp:ListItem Text="Catalogs" Value="CatalogList" Selected="True" />
                <asp:ListItem Text="Collections" Value="CollectionList" Selected="True" />
            </asp:CheckBoxList>
        </td>        
    </tr>       
    <tr>
        <td><asp:Localize ID="locInStock" runat="server" meta:resourcekey="locInStock" /></td>
        <td>
            <asp:DropDownList ID="ddlInStock" runat="server">
                <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                <asp:ListItem Text="<%$ Resources: Yes %>" Value="0" />
                <asp:ListItem Text="<%$ Resources: No %>" Value="1" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="3"></td>
        <td>
            <div id="divCategoryListFEAdvancedSearch">                    
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="4" align="right">
            <asp:Button ID="btnClear" runat="server" meta:resourcekey="btnClear" CssClass="searchButton" CausesValidation="false" OnClientClick="javascript:ResetSearchCriteria(); return false;" />&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearch" CssClass="searchButton" ValidationGroup="Search" CausesValidation="true"/>
        </td>
    </tr>
</table>
</div>
<script type="text/javascript" language="javascript">    
    addLoadEvent(
        RenderSelectedCategoryListSearch,
        document.getElementById('<%= hfAdvancedSearchCategoryList.ClientID %>'),
        document.getElementById('<%= hfAdvancedSearchCategoryName.ClientID %>'),
        document.getElementById('divCategoryListFEAdvancedSearch'),
        '<%= "http://"+(Request.Url.AbsoluteUri.Contains("localhost") ? "localhost": Request.Url.Host) + Page.ResolveUrl(ProductCatalogSettings.BasePath) %>'
        );

    function AddAdvancedSearchCategories() {        
        AddCategoryToList(
                document.getElementById('<%= hfAdvancedSearchCategoryList.ClientID %>'),
                document.getElementById('<%= hfAdvancedSearchCategoryName.ClientID %>'),
                document.getElementById('<%= lbCategoryList.ClientID %>'),
                document.getElementById('divCategoryListFEAdvancedSearch'),
                'ucAdvancedSearch',
                '<%= "http://"+(Request.Url.AbsoluteUri.Contains("localhost") ? "localhost": Request.Url.Host) + Page.ResolveUrl(ProductCatalogSettings.BasePath) %>'
            );
    }

    var txtAdvancedSearchKeywords = document.getElementById('<%= txtAdvancedSearchKeywords.ClientID %>');
    var hfAdvancedSearchCategoryList = document.getElementById('<%= hfAdvancedSearchCategoryList.ClientID %>');
    var hfAdvancedSearchCategoryName = document.getElementById('<%= hfAdvancedSearchCategoryName.ClientID %>');
    var divCategoryListSearch = document.getElementById('divCategoryListFEAdvancedSearch');
    var txtPriceFrom = document.getElementById('<%= txtPriceFrom.ClientID %>');
    var txtPriceTo = document.getElementById('<%= txtPriceTo.ClientID %>');
    var ddlInStock = document.getElementById('<%= ddlInStock.ClientID %>');
</script>