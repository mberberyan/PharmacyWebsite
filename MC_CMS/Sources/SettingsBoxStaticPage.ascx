<%@ Control Language="C#" AutoEventWireup="true" Inherits="SettingsBoxStaticPage" CodeFile="SettingsBoxStaticPage.ascx.cs" %>

<div class="mc_cms_section">
    <h3>
    <asp:Label ID="lblStaticPage" runat="server" CssClass="mc_cms_heading" meta:resourcekey="lblStaticPage"/>
    </h3>
    <div class="mc_cms_settings_section">
        <table cellpadding="2" cellspacing="0" width="100%">
            <tr>
                <td style="padding-left:8px;">
                    <asp:RadioButtonList ID="rdolStaticPages" runat="server" AutoPostBack="true"  
                        RepeatDirection="Horizontal" CssClass="mc_cms_rdStaticPages">
                        <asp:ListItem Value="3" Text='<%$Resources:LocalPage %>' />
                        <asp:ListItem Value="4" Text='<%$Resources:ExternalPage %>'/>
                        <asp:ListItem Value="5" Text='<%$Resources:MenuPage %>'/>
                    </asp:RadioButtonList>
                    <asp:Label ID="lblStaticPageType" runat="server" CssClass="mc_cms_label"/>
                </td>
            </tr>
            <tr>
                <td valign="top">                    
                    <asp:PlaceHolder ID="phLocalPageSettings" runat="server">
                        <span class="mc_cms_validator" style="float:left;">*</span>
                        <div style="padding-left:7px;">
                            <div class="mc_cms_listbox_container">
                                <asp:TextBox ID="txtLocalPage" runat="server" style="display:none;"/>
                                <asp:Repeater ID="repLocalPages" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr><td style="width:100%;white-space:nowrap;padding-left:2px;">
                                          <div id='<%# repLocalPages.ID + ";" + Container.DataItem.ToString() %>' 
                                              onclick="SelectLocalPage(this);">
                                              <%# Container.DataItem.ToString() %>
                                         </div>
                                         </td></tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvLocalPage" runat="server" ControlToValidate="txtLocalPage" Display="Dynamic" SetFocusOnError="true"
                                CssClass="mc_cms_validator" meta:resourcekey="rfvLocalPage" ValidationGroup="NodeSettings"/>
                          </div>      
                    </asp:PlaceHolder>
                    
                    <asp:PlaceHolder ID="phExternalPageSettings" runat="server">
                        <span class="mc_cms_validator" style="vertical-align:top;">*</span>
                        <asp:TextBox ID="txtExternalPage" runat="server" CssClass="mc_cms_input_long"/>
                        <asp:Label ID="lblUrlHint" runat="server" CssClass="mc_cms_comment" meta:resourcekey="lblUrlHint"/> 
                        <br />
                        <asp:RequiredFieldValidator ID="rfvExternalPage" runat="server" ControlToValidate="txtExternalPage" Display="Dynamic" SetFocusOnError="true"
                            CssClass="mc_cms_validator" meta:resourcekey="rfvExternalPage" ValidationGroup="NodeSettings"/>
                        <asp:RegularExpressionValidator ID="revExternalPage" runat="server" ControlToValidate="txtExternalPage" Display="Dynamic" SetFocusOnError="true"
                            ValidationExpression="^((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+([\w\d:#@%/;$()~_?\+-=\\\.&](?!&#))*)$"
                            CssClass="mc_cms_validator" meta:resourcekey="revExternalPage" ValidationGroup="NodeSettings"/><br />
                             
                    </asp:PlaceHolder>
                        
                    <asp:PlaceHolder ID="phMenuPageSettings" runat="server">
                        
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="padding-right:4px;" valign="top"><span class="mc_cms_validator">*</span></td>
                                <td>
                                     <div class="mc_cms_listbox_container">
                                        <asp:TextBox ID="txtMenuPage" runat="server" style="display:none;"/>
                                        <asp:Repeater ID="repMenuPages" runat="server">
                                            <HeaderTemplate>
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr><td style="width:100%;white-space:nowrap;padding-left:2px;">
                                                     <div id='<%# repMenuPages.ID + ";" + Eval("Id") %>' 
                                                          onclick="SelectMenuPage(this);">
                                                          <%# Convert.ToString(GetLocalResourceObject("Explorer")) + " / " + Eval("Path")%>
                                                     </div>
                                                 </td></tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                    
                                </td>
                                <td valign="top" style="padding-left:10px;">
                                   <asp:Button ID="btnGoToReferredPage" runat="server" CssClass="mc_cms_button mc_cms_btn_106" meta:resourcekey="btnGoToReferredPage"/></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <asp:RequiredFieldValidator ID="rfvMenuPage" runat="server" ControlToValidate="txtMenuPage" Display="Dynamic" SetFocusOnError="true"
                                        CssClass="mc_cms_validator" meta:resourcekey="rfvMenuPage" ValidationGroup="NodeSettings"/> 
                                </td>
                            </tr>
                        </table>
                     
                    </asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </div>
</div>

<script language="javascript" type="text/javascript">
    var selectedLocalPageDiv = null;
    var txtLocalPage = document.getElementById('<%= txtLocalPage.ClientID %>');
    var rfvLocalPage = document.getElementById('<%= rfvLocalPage.ClientID %>');

    if (!!txtLocalPage && txtLocalPage.value != "")
    {
        selectedLocalPageDiv = document.getElementById(txtLocalPage.value);
        selectedLocalPageDiv.parentNode.className ="mc_cms_listbox_selectedItem";
    }     
     
    var selectedMenuPageDiv = null;
    var txtMenuPage = document.getElementById('<%= txtMenuPage.ClientID %>');
    var rfvMenuPage = document.getElementById('<%= rfvMenuPage.ClientID %>');

    if (!!txtMenuPage && txtMenuPage.value != "")
    {
        selectedMenuPageDiv = document.getElementById(txtMenuPage.value);
        selectedMenuPageDiv.parentNode.className ="mc_cms_listbox_selectedItem";
    }
</script>
