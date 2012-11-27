<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DescriptionPanel.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_DescriptionPanel" %>
<div id="divDescriptionPanel" runat="server" class="mc_pc_descpanel" >    
    <asp:Localize ID="locObjectDescName" runat="server" />    
    <asp:Label ID="lblObjectDescName" runat="server" CssClass="mc_pc_desclabel"/>    
    <asp:Localize ID="locObjectDescCode" runat="server" />    
    <asp:Label ID="lblObjectDescCode" runat="server" />        
</div>