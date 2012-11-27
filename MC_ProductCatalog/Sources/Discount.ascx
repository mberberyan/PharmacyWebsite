<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Discount.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Discount" %>
<%@ Register TagPrefix="melon" TagName="MenuTabs" Src="MenuTabs.ascx" %>
<%@ Register TagPrefix="melon" TagName="DiscountInformation" Src="DiscountInformation.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductSet" Src="ProductSet.ascx" %>
<%@ Register TagPrefix="melon" TagName="DescPanel" Src="DescriptionPanel.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<asp:HiddenField ID="hfSelectedTab" runat="server"/>
<div  id="divExplorerLayout" runat="server" class="mc_pc_explorer_layout">
    &nbsp;
</div>

<melon:DescPanel ID="ucDescPanel" runat="server" />

<div id="divDiscountDetails" runat="server" class="mc_pc_panels_inner_section_explorer_padding" >
    <melon:MenuTabs ID="ucMenuTabs" runat="server" />                        
    <div class="mc_pc_clear">&nbsp;</div>
    
    <div class="mc_pc_panels_inner_section_content">
        <div id="divGeneralInformation" class="mc_pc_discount_width">
            <melon:DiscountInformation ID="ucDiscountInformation" runat="server" />
            <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="mc_pc_button mc_pc_btn_61 right" ValidationGroup="Discount" />
        </div>
        <div id="divProducts">
            <melon:ProductSet ID="ucProductSet" runat="server" />
        </div>
    </div>
    <div class="mc_pc_panels_inner_section_footer">&nbsp;</div>
</div>
<script type="text/javascript" language="javascript">
    var divGeneralInformation = document.getElementById('divGeneralInformation');
    var divProducts           = document.getElementById('divProducts');
        
    var hfSelectedTab = document.getElementById('<%= hfSelectedTab.ClientID %>');
    
    <% 
        string selectedTabStr="";
        switch(SelectedTab)
        {
            case ProductCatalogTabs.GeneralInformation:
                selectedTabStr = ProductCatalogTabs.GeneralInformation.ToString();
                break;            
            case ProductCatalogTabs.Products:
                selectedTabStr = ProductCatalogTabs.Products.ToString();
                break;        
            default:                
                selectedTabStr = ProductCatalogTabs.Unknown.ToString();
                break;    
        }
    %>
    
    SelectMenuTab('<%= selectedTabStr %>');
               
</script>