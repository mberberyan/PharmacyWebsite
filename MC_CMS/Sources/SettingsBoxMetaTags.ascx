<%@ Control Language="C#" AutoEventWireup="true" Inherits="SettingsBoxMetaTags" CodeFile="SettingsBoxMetaTags.ascx.cs" %>
<%@ Import Namespace="Melon.Components.CMS.Configuration" %>

<div class="mc_cms_section">
    <h3>
    <asp:Label ID="lblMetaTags" runat="server" CssClass="mc_cms_heading" meta:resourcekey="lblMetaTags"/>
    </h3>
    <div class="mc_cms_settings_section">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td colspan="2">
                    <table cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td class="mc_cms_settings_label_column">
                                <asp:Label ID="lblAuthor" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblAuthor"/></td>
                            <td>
                                <asp:TextBox ID="txtAuthor" runat="server" MaxLength="256" CssClass="mc_cms_input_short"/>
                                <asp:RegularExpressionValidator ID="revAuthor" runat="server" ControlToValidate="txtAuthor" Display="Dynamic" SetFocusOnError="true"
                                    CssClass="mc_cms_validator" meta:resourcekey="revAuthor" ValidationGroup="NodeSettings" 
                                    ValidationExpression="[a-zA-Zа-яА-Я\s]*"/></td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr>
                <td colspan="2">
                    <table cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td class="mc_cms_settings_label_column">
                                <asp:Label ID="lblMetaTitle" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblMetaTitle" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMetaTitle" runat="server" MaxLength="256" CssClass="mc_cms_input_long" Width="615" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" width="50%">
                    <asp:Label ID="lblKeywords" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblKeywords"/><br />
                    <asp:TextBox ID="txtKeywords" runat="server" TextMode="MultiLine" Rows="5" MaxLength="1024" CssClass="mc_cms_meta_tags_input"/>
                    <asp:RegularExpressionValidator ID="revKeywords" runat="server" ControlToValidate="txtKeywords" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revKeywords" ValidationGroup="NodeSettings"
                        ValidationExpression="[a-zA-Zа-яА-Я0-9\'\x22\-_\s,.]*"/>
                </td>
                <td valign="top" width="50%">
                    <asp:Label ID="lblDescription" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblDescription"/><br />
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" MaxLength="1024" CssClass="mc_cms_meta_tags_input"/>
                    <asp:RegularExpressionValidator ID="revDescription" runat="server" ControlToValidate="txtDescription" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revDescription" ValidationGroup="NodeSettings"
                        ValidationExpression="[a-zA-Zа-яА-Я0-9\'\x22\-_\s,.]*"/>
                </td>
            </tr>
        </table>
    </div>
</div>