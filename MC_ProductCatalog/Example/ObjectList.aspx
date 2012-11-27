<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MPExample.master"  CodeFile="ObjectList.aspx.cs" Inherits="_ObjectList" EnableEventValidation="false" %>
<%@ Register TagPrefix="melon" TagName="FEProductCatalog" Src="../Sources/FEProductCatalog.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <melon:FEProductCatalog id="ucProductCatalog" runat="server" />                
</asp:Content>
