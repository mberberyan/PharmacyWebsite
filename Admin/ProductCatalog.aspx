<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MPAdmin.master" AutoEventWireup="true" CodeFile="ProductCatalog.aspx.cs" Inherits="Admin_ProductCatalog" Theme="Admin" %>
<%@ Register TagPrefix="melon" TagName="ProductCatalog" Src="~/MC_ProductCatalog/Sources/ProductCatalog.ascx" %>

<asp:Content ID="cHead" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="cPageContent" ContentPlaceHolderID="cphPageContent" Runat="Server">
    <melon:ProductCatalog ID="cntrlProductCatalog" runat="server"/>
</asp:Content>

