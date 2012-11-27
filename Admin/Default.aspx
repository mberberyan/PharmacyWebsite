<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MPAdmin.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" Theme="Admin" %>
<%@ Register TagPrefix="melon" TagName="CMS" Src="~/MC_CMS/Sources/CMS.ascx" %>

<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="cPageContent" ContentPlaceHolderID="cphPageContent" Runat="Server">
    <melon:CMS ID="cntrlCMS" runat="server"/>
</asp:Content>

