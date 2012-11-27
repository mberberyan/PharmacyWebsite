<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumUserProfileEdit.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.ForumUserProfileEdit" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>

<iframe id="ifrProfile" class="mc_forum_userprofile_iframe" runat="server" frameborder="0"></iframe>

<asp:HiddenField ID="hfError" runat="server"/>
<asp:Button ID="btnShowError" runat="server" style="display:none;"/>


