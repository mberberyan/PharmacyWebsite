<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminAccessDenied.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminAccessDenied" %>

<div style="padding:10px;">
    <asp:Label ID="lblMessage" runat="server" /><br /><br />
    <asp:Label ID="lblInstruction" runat="server" meta:resourcekey="lblInstruction"/>
    <br /><br />
    <asp:LinkButton ID="lbtnLogin" runat="server" meta:resourcekey="lbtnLogin" CssClass="mc_news_link_btn"/>
</div>