<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SubNavigation.ascx.cs" Inherits="Controls_SubNavigation" %>

<asp:Menu ID="cntrlSubMenu" runat="server" DataSourceID="dsSubMenu"
    CssClass="sub_menu"
    StaticDisplayLevels="1"
    StaticEnableDefaultPopOutImage = "false" 
    StaticMenuItemStyle-CssClass="sub_menu_item"
    StaticHoverStyle-CssClass = "selected"
    StaticSelectedStyle-CssClass = "selected">
</asp:Menu>
<asp:SiteMapDataSource ID="dsSubMenu" runat="server" ShowStartingNode="false" />
