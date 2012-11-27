<%@ Control Language="C#" AutoEventWireup="true" Inherits="PagePreview" CodeFile="PagePreview.ascx.cs" %>

<asp:Label ID="lblPreviewTitle" runat="server" CssClass="mc_cms_heading mc_cms_preview_heading"/>
<asp:Label ID="lblLoginLogoutWarning" runat="server" CssClass="mc_cms_warning_message" meta:resourcekey="lblLoginLogoutWarning"/>
<div>
<iframe id="iFramePreview" runat="server" src="" class="mc_cms_contentFrame" frameborder="0"></iframe>
</div>