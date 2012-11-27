<%@ Control Language="C#" AutoEventWireup="true" Inherits="SettingsBoxLog" CodeFile="SettingsBoxLog.ascx.cs" %>

<div class="mc_cms_section">
    <h3>
    <asp:Label ID="lblLog" runat="server" CssClass="mc_cms_heading" meta:resourcekey="lblLog"/>
    </h3>
    <div class="mc_cms_settings_section">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblCreated" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblCreated"/>&nbsp;
                    <asp:Label ID="lblDateCreated" runat="server" CssClass="mc_cms_static_text"/>&nbsp;
                    <asp:Label ID="lblCreatedBy" runat="server" CssClass="mc_cms_static_text"/></td>
                <td>
                    <asp:PlaceHolder ID="phLangVersionLastUpdated" runat="server">
                        <asp:Label ID="lblLangVersionLastUpdated" runat="server" CssClass="mc_cms_label"/>&nbsp; 
                        <asp:Label ID="lblLangVersionDateLastUpdated" runat="server" CssClass="mc_cms_static_text"/> &nbsp;     
                        <asp:Label ID="lblLangVersionLastUpdatedBy" runat="server" CssClass="mc_cms_static_text"/>
                    </asp:PlaceHolder></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLastUpdated" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblLastUpdated"/>&nbsp;
                    <asp:Label ID="lblDateLastUpdated" runat="server" CssClass="mc_cms_static_text"/>&nbsp;
                    <asp:Label ID="lblLastUpdatedBy" runat="server" CssClass="mc_cms_static_text"/></td>
                <td>
                    <asp:PlaceHolder ID="phLangVersionLastPublished" runat="server">
                        <asp:Label ID="lblLangVersionLastPublished" runat="server" CssClass="mc_cms_label"/>&nbsp;
                        <asp:Label ID="lblLangVersionDateLastPublished" runat="server" CssClass="mc_cms_static_text"/>&nbsp;
                        <asp:Label ID="lblLangVersionLastPublishedBy" runat="server" CssClass="mc_cms_static_text" />
                    </asp:PlaceHolder></td>
            </tr>
        </table>
    </div>
</div>