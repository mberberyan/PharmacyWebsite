<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Category.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Category" %>
<%@ Register TagPrefix="melon" TagName="MenuTabs" Src="MenuTabs.ascx" %>
<%@ Register TagPrefix="melon" TagName="GeneralInformation" Src="GeneralInformation.ascx" %>
<%@ Register TagPrefix="melon" TagName="Images" Src="Images.ascx" %>
<%@ Register TagPrefix="melon" TagName="DynamicPropDef" Src="DynamicPropDefinition.ascx" %>
<%@ Register TagPrefix="melon" TagName="DescPanel" Src="DescriptionPanel.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<asp:HiddenField ID="hfSelectedTab" runat="server"/>
<div  id="divExplorerLayout" runat="server" class="mc_pc_explorer_layout">
    &nbsp;
</div>

<melon:DescPanel ID="ucDescPanel" runat="server" />

<div id="divCategoryDetails" runat="server" class="mc_pc_panels_inner_section_explorer_padding" >
        
    <melon:MenuTabs ID="ucMenuTabs" runat="server" />                        
    <div class="mc_pc_clear">&nbsp;</div>
    
    <div class="mc_pc_panels_inner_section_content">
        <div id="divGeneralInformation" class="mc_pc_width">
            <melon:GeneralInformation ID="ucGeneralInformation" runat="server" />
            <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="mc_pc_button mc_pc_btn_61 right" ValidationGroup="GeneralInformation"/>
        </div>
        <div id="divImages" class="mc_pc_width">
            <melon:Images ID="ucImages" runat="server" />
        </div>
        <div id="divDynamicProperties" class="mc_pc_width">        
            <melon:DynamicPropDef id="ucDynamicPropDef" runat="server" />
        </div>
    </div> 
    <div class="mc_pc_panels_inner_section_footer">&nbsp;</div>       
</div>
<script type="text/javascript" language="javascript">
    var divGeneralInformation = document.getElementById('divGeneralInformation');
    var divImages = document.getElementById('divImages');
    var divDynamicProperties = document.getElementById('divDynamicProperties');    
    var hfSelectedTab = document.getElementById('<%= hfSelectedTab.ClientID %>');
      
    <% 
        string selectedTabStr="";        
        switch(SelectedTab)
        {
            case ProductCatalogTabs.GeneralInformation:
                selectedTabStr = ProductCatalogTabs.GeneralInformation.ToString();
                break;
            case ProductCatalogTabs.Images:
                selectedTabStr = ProductCatalogTabs.Images.ToString();
                break;
            case ProductCatalogTabs.DynamicProperties:
                selectedTabStr = ProductCatalogTabs.DynamicProperties.ToString();
                break;            
            default:
                selectedTabStr = ProductCatalogTabs.Unknown.ToString();
                break;    
        }
    %>
    
    SelectMenuTab('<%= selectedTabStr %>');
               
</script>