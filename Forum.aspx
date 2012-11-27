<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/MPBase.master"
    AutoEventWireup="true" CodeFile="Forum.aspx.cs" Inherits="Forum"  Theme="Default"%>
<%@ Register TagPrefix="melon" TagName="Forum" Src="~/MC_Forum/Forum.ascx" %>

<asp:Content ID="cBase" ContentPlaceHolderID="cphBase" runat="Server">
    <h1 class="page_title">Forum</h1>
    <melon:Forum ID="cntrlForum" runat="server" />
</asp:Content>
