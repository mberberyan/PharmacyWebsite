<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Breadcrumb.ascx.cs" Inherits=" Melon.Components.Forum.UI.CodeBehind.Breadcrumb" %>

<asp:Repeater ID="repBreadCrumb" runat="server">
	<ItemTemplate>
		<asp:LinkButton ID="lbtnBreadCrumb" runat="server" CssClass="mc_forum_breadcrumb_link" />
	</ItemTemplate>
	<SeparatorTemplate>
		<asp:Label ID="lblSeparator" runat="server" CssClass="mc_forum_breadcrumb_separator"/>	
    </SeparatorTemplate>
</asp:Repeater>
