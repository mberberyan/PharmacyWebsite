<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MPAdmin.master" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="Admin_News" Theme="Admin" ValidateRequest="false" %>
<%@ Register TagPrefix="melon" TagName="NewsManagement" Src="~/MC_News/Sources/NewsAdministration.ascx" %>

<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="cPageContent" ContentPlaceHolderID="cphPageContent" Runat="Server">
     <melon:NewsManagement ID="cntrlNewsManagement" runat="server"/>
</asp:Content>




