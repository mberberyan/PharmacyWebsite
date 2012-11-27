<%@ Control Language="C#" AutoEventWireup="true" Inherits="AccessDenied" CodeFile="AccessDenied.ascx.cs" %>

<div style="padding:10px;">
    <asp:Label ID="lblMessage" runat="server" /><br /><br />
    <asp:Label ID="lblInstruction" runat="server" meta:resourcekey="lblInstruction"/>
    <br /><br />
    <asp:LinkButton ID="lbtnLogin" runat="server" meta:resourcekey="lbtnLogin" CssClass="mc_cms_link_btn"/>
</div>