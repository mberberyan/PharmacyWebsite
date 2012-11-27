<%@ Control Language="C#" AutoEventWireup="true" Inherits="TemplateAddEdit" CodeFile="TemplateAddEdit.ascx.cs" %>
<%@ Import Namespace="Melon.Components.CMS.Configuration" %>

<div class="mc_cms_margin_top">   
    <table cellpadding="0" cellspacing="0" width="730"> 
        <tr>
            <td>
                <!-- Table with template details -->
                <table cellpadding="0" cellspacing="0" border="0" class="mc_cms_table" width="100%">
                    <!-- Header -->
                    <tr>
                        <th colspan="2">
                            <asp:Label ID="lblTemplate" runat="server" meta:resourcekey="lblTemplate" />
                        </th>
                    </tr>
                    
                    <!-- Name of template -->
                    <tr class="mc_cms_table_row_padding">
                        <td>
                            <asp:Label ID="lblName" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblName" />
                            <span class="mc_cms_validator">*</span></td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="mc_cms_input_short" MaxLength="50" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ValidationGroup="TemplateSettings"
                                Display="Dynamic" meta:resourcekey="rfvName" CssClass="mc_cms_validator" SetFocusOnError="true"/>
                            <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName" ValidationGroup="TemplateSettings"
                                Display="Dynamic" meta:resourcekey="revName" CssClass="mc_cms_validator" SetFocusOnError="true"
                                ValidationExpression="[a-zA-Zа-яА-Я0-9_]*"/>
                    </tr>
                   
                    <tr>
                        <td style="vertical-align:top;">
                             <asp:Label ID="lblSelectMasterPage" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblSelectMasterPage" />
                             <span class="mc_cms_validator">*</span></td>
                        <td>
                            <!-- All master pages on the web site. -->
                            <div style="float:left;">
                                <div class="mc_cms_listbox_container mc_cms_lstMasterPages" >
                                    <asp:TextBox ID="txtMasterPage" runat="server" style="display:none;"/>
                                    <asp:Button ID="btnSelectMasterPage" runat="server" style="display:none;"/>
                                    <asp:Repeater ID="repMasterPages" runat="server">
                                        <HeaderTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr><td style="width:100%;white-space:nowrap;padding:0 0 0 2px !important;">
                                                <div id='<%# repMasterPages.ID + ";" + Container.DataItem.ToString() %>' 
                                                      onclick="SelectMasterPage(this);">
                                                      <%# Container.DataItem.ToString() %>
                                                </div>
                                            </td></tr> 
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>                         
                            </div>
                            <div style="float:left;padding-left:10px;">
                                 <asp:Label ID="lblPlaceholders" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblPlaceholders"/>
                                <div class="mc_cms_divPlaceholders">
                                    <div id="divNoMasterPage" runat="server">
                                        <asp:Label ID="lblNoMasterPage" runat="server" meta:resourcekey="lblNoMasterPage" CssClass="mc_cms_comment"/>
                                    </div>
                                    <!-- Placeholders of the selected master page.-->
                                    <asp:Repeater ID="repPlaceholders" runat="server">
                                        <HeaderTemplate>
                                            <ul>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <li>
                                                    <asp:Label ID="lblPlaceholderName" runat="server" />
                                                </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                             </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr class="mc_cms_table_row_padding">
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvMasterPage" runat="server" ControlToValidate="txtMasterPage" ValidationGroup="TemplateSettings"
                                Display="Dynamic" CssClass="mc_cms_validator" meta:resourcekey="rfvMasterPage" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right">
               <span style="padding-right: 5px;">
                    <asp:Button ID="btnCancel" runat="server" CssClass="mc_cms_button mc_cms_btn_61" meta:resourcekey="btnCancel"
                        CausesValidation="false" />
               </span>
               <span>
                    <asp:Button ID="btnSave" runat="server"  CssClass="mc_cms_button mc_cms_btn_61" meta:resourcekey="btnSave"
                        CausesValidation="true" ValidationGroup="TemplateSettings"/>
               </span>
            </td> 
        </tr>
    </table>
</div>
 <script language="javascript" type="text/javascript">
    var selectedMasterPageDiv = null;
    var txtMasterPage = document.getElementById('<%= txtMasterPage.ClientID %>');                                    
    var rfvMasterPage = document.getElementById('<%= rfvMasterPage.ClientID %>');
    var revName = document.getElementById('<%= revName.ClientID %>');
    var btnSelectMasterPage = document.getElementById('<%=btnSelectMasterPage.ClientID %>');
    
    if (txtMasterPage.value != "")
    {
        selectedMasterPageDiv = document.getElementById(txtMasterPage.value);
        selectedMasterPageDiv.parentNode.className ="mc_cms_listbox_selectedItem";
    }
</script>    

