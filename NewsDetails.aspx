<%@ Page Language="C#" MasterPageFile="~/MPNews.master" AutoEventWireup="true" CodeFile="NewsDetails.aspx.cs" Inherits="MC_News_Examples_NewsDetails" meta:resourcekey="Page" ValidateRequest="false" Theme="Default" %>
<%@ Reference VirtualPath="~/Controls/Languages.ascx" %>
<%@ Register TagPrefix="melon" TagName="NewsDetails" Src="~/MC_News/Sources/NewsDetails.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphNews" Runat="Server">
    <melon:NewsDetails ID="cntrlNewsDetails" runat="server"/>
</asp:Content>

