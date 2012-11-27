<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Bundle.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Bundle" %>
<%@ Register TagPrefix="melon" TagName="MenuTabs" Src="MenuTabs.ascx" %>
<%@ Register TagPrefix="melon" TagName="GeneralInformation" Src="GeneralInformation.ascx" %>
<%@ Register TagPrefix="melon" TagName="ProductSet" Src="ProductSet.ascx" %>
<%@ Register TagPrefix="melon" TagName="Images" Src="Images.ascx" %>
<%@ Register TagPrefix="melon" TagName="Audio" Src="Audio.ascx" %>
<%@ Register TagPrefix="melon" TagName="Video" Src="Video.ascx" %>
<%@ Register TagPrefix="melon" TagName="DynamicPropDef" Src="DynamicPropDefinition.ascx" %>
<%@ Register TagPrefix="melon" TagName="DescPanel" Src="DescriptionPanel.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<asp:HiddenField ID="hfSelectedTab" runat="server"/>

<div  id="divExplorerLayout" runat="server" class="mc_pc_explorer_layout">
    &nbsp;
</div>

<melon:DescPanel ID="ucDescPanel" runat="server" />

<div id="divBundleDetails" runat="server" class="mc_pc_panels_inner_section_explorer_padding" >
    <melon:MenuTabs ID="ucMenuTabs" runat="server" />                        
    <div class="mc_pc_clear">&nbsp;</div>
    
    <div class="mc_pc_panels_inner_section_content">
        <div id="divGeneralInformation" class="mc_pc_width">
            <melon:GeneralInformation ID="ucGeneralInformation" runat="server" />
            <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="mc_pc_button mc_pc_btn_61 right" ValidationGroup="GeneralInformation"/>
        </div>
        <div id="divProducts">
            <melon:ProductSet ID="ucProductSet" runat="server" />
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
        <div id="divNoPropValues" runat="server" visible="false"><asp:Literal ID="litNoPropValuesEmptyMessage" runat="server" meta:resourcekey="litNoPropValuesEmptyMessage" /></div>
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
                    <tr id="Tr1" runat="server">
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
                        <asp:Button ID="btnInsert" runat="server" CommandName="InsertAction" meta:resourcekey="btnInsert" CssClass="mc_pc_button mc_pc_btn_61" Visible="false" ValidationGroup="InsertProperty" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="InsertCancel" meta:resourcekey="btnCancel"  Visible="false" CssClass="mc_pc_button mc_pc_btn_61" CausesValidation="false" />
                        <div>
                            <asp:RequiredFieldValidator ID="rfvInsertItem" runat="server" ControlToValidate="txtEdit" CssClass="mc_pc_validator" Display="Dynamic" meta:resourcekey="rfvInsertItem" ValidationGroup="InsertProperty"/>
                        </div>
                    </td>
                </ItemTemplate>
                <EditItemTemplate>                
                    <td valign="top" colspan="2">
                      <asp:HiddenField ID="hfPropertyValueId" runat="server" Value='<%# Eval("PropertyValueId") %>' />
                      <asp:TextBox ID="txtEdit" runat="server" Text='<%# Eval("PropertyValue") %>' CssClass="mc_pc_input_extra_short" />                
                      <asp:Button ID="btnUpdate" runat="server" CommandName="Update" meta:resourcekey="btnUpdate" CssClass="mc_pc_button mc_pc_btn_61" CausesValidation="false" />                  
                      <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" meta:resourcekey="btnCancel" CssClass="mc_pc_button mc_pc_btn_61" CausesValidation="false" />
                    </td>
                </EditItemTemplate>
                <InsertItemTemplate></InsertItemTemplate>
            </asp:ListView>
            <asp:Button ID="btnAddPropValue" runat="server" meta:resourcekey="btnAddPropValue" CssClass="mc_pc_button mc_pc_btn_61" />
        </div>
    </div>
    <div class="mc_pc_panels_inner_section_footer">&nbsp;</div>
</div>
<script type="text/javascript" language="javascript">
    var divGeneralInformation = document.getElementById('divGeneralInformation');
    var divProducts = document.getElementById('divProducts');
    var divImages             = document.getElementById('divImages');
    var divAudio              = document.getElementById('divAudio');
    var divVideo              = document.getElementById('divVideo');
    var divDynamicProperties = document.getElementById('divDynamicProperties');    
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
            default:
                selectedTabStr = ProductCatalogTabs.Unknown.ToString();
                break;    
        }
    %>
    
    SelectMenuTab('<%= selectedTabStr %>');
               
</script>