<%@ Control Language="C#" AutoEventWireup="true" Inherits="SettingsBoxTemplate" CodeFile="SettingsBoxTemplate.ascx.cs" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Melon.Components.CMS" %>

<div class="mc_cms_section">
    <h3>
    <asp:Label ID="lblTemplate" runat="server" CssClass="mc_cms_heading" meta:resourcekey="lblTemplate"/>
    </h3>
    <div class="mc_cms_settings_section">
        <table cellpadding="2" cellspacing="0" width="100%">
            <tr>
                <td valign="top" class="mc_cms_settings_label_column">
                     <asp:Label ID="lblSelectTemplate" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblSelectTemplate"/>
                     <span class="mc_cms_validator">*</span></td>
                <td valign="top" class="mc_cms_dropdown_templates_column">
                    <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="mc_cms_dropdown_short"
                        DataTextField="Name" DataValueField="Id" AutoPostBack ="true" CausesValidation ="false"/><br />
                    <asp:RequiredFieldValidator ID="rfvTemplate" runat="server" ControlToValidate="ddlTemplates" 
                        ValidationGroup="NodeSettings" CssClass="mc_cms_validator" meta:resourcekey="rfvTemplate" SetFocusOnError="true"/></td>
                <td>
                    <!-- Placeholders of the selected template. -->
                    <asp:Repeater ID="repPlaceholders" runat="server" >
                        <HeaderTemplate>
                            <asp:Label ID="lblPlaceholders" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblPlaceholders"/>
                            <ul class="mc_cms_content_placeholders">
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li>
                                    <asp:Label ID="lblPlaceholderName" runat="server" CssClass="mc_cms_static_text"/>
                                </li>
                        </ItemTemplate>
                        <FooterTemplate>
                             </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
          
        </table>
    </div>
</div>

<script type="text/javascript" language="javascript">
    var contentPlaceholders;
    <%
        int __placeholdersCount = lstPlaceholders.Count; 
    %>
    var placeholdersCount = <%= __placeholdersCount%>;
    contentPlaceholders = new Array(placeholdersCount);
    
    <%
    for(int i = 0; i < __placeholdersCount; i++)
    {
        %>
        contentPlaceholders[<%=i%>] = '<%= lstPlaceholders[i] %>';
        <%  
    }
    %>
</script>