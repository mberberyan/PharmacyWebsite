<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MPProducts.master"  CodeFile="ObjectList.aspx.cs" Inherits="_ObjectList" EnableEventValidation="false" Theme="Default" %>
<%@ Register TagPrefix="melon" TagName="FEProductCatalog" Src="~/MC_ProductCatalog/Sources/FEProductCatalog.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphProducts" Runat="Server">
    <melon:FEProductCatalog id="ucProductCatalog" runat="server" />                
</asp:Content>
