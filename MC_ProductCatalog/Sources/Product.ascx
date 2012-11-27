<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Product.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Product" %>
<%@ Register TagPrefix="melon" TagName="MenuTabs" Src="MenuTabs.ascx" %>
<%@ Register TagPrefix="melon" TagName="GeneralInformation" Src="GeneralInformation.ascx" %>
<%@ Register TagPrefix="melon" TagName="Images" Src="Images.ascx" %>
<%@ Register TagPrefix="melon" TagName="Audio" Src="Audio.ascx" %>
<%@ Register TagPrefix="melon" TagName="Video" Src="Video.ascx" %>
<%@ Register TagPrefix="melon" TagName="DynamicPropDef" Src="DynamicPropDefinition.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductSet" Src="ProductSet.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductStatistics" Src="ProductStatistics.ascx" %>
<%@ Register TagPrefix="melon" TagName="DescPanel" Src="DescriptionPanel.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<asp:HiddenField ID="hfSelectedTab" runat="server"/>

<melon:DescPanel ID="ucDescPanel" runat="server" />

<div id="divProductDetails" runat="server" class="mc_pc_panels_inner_section_explorer_padding" >
    <melon:MenuTabs ID="ucMenuTabs" runat="server" />                        
    <div class="mc_pc_clear">&nbsp;</div>
    
    <div class="mc_pc_panels_inner_section_content">    
        <div id="divGeneralInformation" class="mc_pc_width">
            <melon:GeneralInformation ID="ucGeneralInformation" runat="server" />
            <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="mc_pc_button mc_pc_btn_61 right" ValidationGroup="GeneralInformation" />
        </div>
        <div id="divImages">
            <melon:Images ID="ucImages" runat="server" />
        </div>
        <div id="divAudio">
            <melon:Audio ID="ucAudio" runat="server" />
        </div>
        <div id="divVideo">
            <melon:Video ID="ucVideo" runat="server" />
        </div>
        <div id="divNoPropValues" runat="server" visible="false"><asp:Localize ID="locNoPropValuesEmptyMessage" runat="server" meta:resourcekey="locNoPropValuesEmptyMessage" /></div>
        <div id="divDynamicProperties" class="mc_pc_props_table_wrapper">
            <melon:DynamicPropDef ID="ucDynamicPropDef" runat="server" />
            <asp:ListView ID="lvPropValues" runat="server" > 
                <LayoutTemplate>                                               
                            <table id="tabPropValues" runat="server" class="mc_pc_props_table">                        
                            <!-- Header row will be filled with property names -->                                        
                            <th>
                                <tr>                                
                                </tr>
                            </th>
                            <!-- End Header row -->
                            <tr ID="groupPlaceholder" runat="server" class="mc_pc_props_row" />
                            </table>                            
                </LayoutTemplate>
                <GroupTemplate>                                   
                    <tr runat="server">
                        <td id="itemPlaceholder" runat="server">                        
                        </td>                    
                    </tr>
                </GroupTemplate>
                <ItemTemplate>                                                                
                    <td id="cellView" runat="server">
                        <asp:Label ID="lblPropValue" runat="server" Text='<%# Eval("PropertyValue") %>' />                
                    </td>
                    <td id="cellUpdate" runat="server">
                        <asp:HiddenField ID="hfPropertyValueId" runat="server" Value='<%# Eval("PropertyValueId") %>' />
                        <asp:TextBox ID="txtEdit" runat="server" Text='<%# Eval("PropertyValue") %>' CssClass="mc_pc_input_extra_short"/>                    
                    </td>
                    <td id="cellEdit" runat="server" class="mc_pc_props_edit_row">
                        <asp:Button ID="btnEdit" runat="server" meta:resourcekey="btnEdit" CssClass="mc_pc_button mc_pc_btn_61" CommandName="Edit" CausesValidation="false" />
                        <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="mc_pc_button mc_pc_btn_61" CommandName="Delete" CausesValidation="false" OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeletePropertyValue").ToString() + "\");" %>' />
                        <asp:Button ID="btnInsert" runat="server" meta:resourcekey="btnInsert" CssClass="mc_pc_button mc_pc_btn_61" CommandName="InsertAction" Visible="false" ValidationGroup="InsertProperty" />
                        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="mc_pc_button mc_pc_btn_61" CommandName="InsertCancel" Visible="false" CausesValidation="false" />                    
                        <div>
                            <asp:CustomValidator ID="cvInsertItem" runat="server" meta:resourcekey="cvInsertItem" ClientValidationFunction="ValidateDynPropEntries" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="InsertProperty" />
                        </div>
                    </td>
                </ItemTemplate>
                <EditItemTemplate>                
                    <td valign="top" colspan="2">
                      <asp:HiddenField ID="hfPropertyValueId" runat="server" Value='<%# Eval("PropertyValueId") %>' />
                      <asp:TextBox ID="txtEdit" runat="server" Text='<%# Eval("PropertyValue") %>' CssClass="mc_pc_input_extra_short" />                
                      <asp:Button ID="btnUpdate" runat="server" meta:resourcekey="btnUpdate" CssClass="mc_pc_button mc_pc_btn_61" CommandName="Update" CausesValidation="false" />                  
                      <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="mc_pc_button mc_pc_btn_61" CommandName="Cancel" CausesValidation="false" />
                    </td>
                </EditItemTemplate>
                <InsertItemTemplate></InsertItemTemplate>
            </asp:ListView>
            <asp:Button ID="btnAddPropValue" runat="server" meta:resourcekey="btnAddPropValue" CssClass="mc_pc_button mc_pc_btn_61"/>
        </div>
        <div id="divRelatedProducts">
            <melon:ProductSet ID="ucProductSet" runat="server" />
        </div>
        <div id="divStatistics">
            <melon:ProductStatistics ID="ucProductStatistics" runat="server" />
        </div>
    </div>    
    <div class="mc_pc_panels_inner_section_footer">&nbsp;</div>
</div>
<script type="text/javascript" language="javascript">
    var divGeneralInformation = document.getElementById('divGeneralInformation');
    var divImages             = document.getElementById('divImages');
    var divAudio              = document.getElementById('divAudio');
    var divVideo              = document.getElementById('divVideo');
    var divDynamicProperties = document.getElementById('divDynamicProperties');    
    var divRelatedProducts    = document.getElementById('divRelatedProducts');
    var divStatistics         = document.getElementById('divStatistics');
    var hfSelectedTab         = document.getElementById('<%= hfSelectedTab.ClientID %>');
        
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
            case ProductCatalogTabs.Audio:
                selectedTabStr = ProductCatalogTabs.Audio.ToString();
                break;
            case ProductCatalogTabs.Video:
                selectedTabStr = ProductCatalogTabs.Video.ToString();
                break;
            case ProductCatalogTabs.DynamicProperties:
                selectedTabStr = ProductCatalogTabs.DynamicProperties.ToString();
                break;
            case ProductCatalogTabs.RelatedProducts:
                selectedTabStr = ProductCatalogTabs.RelatedProducts.ToString();
                break;
            case ProductCatalogTabs.Statistics:
                selectedTabStr = ProductCatalogTabs.Statistics.ToString();
                break;
            default:
                selectedTabStr = ProductCatalogTabs.Unknown.ToString();
                break;    
        }
    %>
    
    SelectMenuTab('<%= selectedTabStr %>');
               
</script>