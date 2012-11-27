<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Controls_Header" %>

<asp:Menu ID="cntrlMainMenu" runat="server" Orientation="Horizontal" DataSourceID="dsMainMenu" 
    MaximumDynamicDisplayLevels="0" 
    CssClass="mainMenu"
    StaticDisplayLevels="1"
    StaticEnableDefaultPopOutImage = "false" 
    StaticMenuItemStyle-CssClass="menu_item"
    StaticHoverStyle-CssClass = "selected" SkipLinkText="" >
</asp:Menu>

<asp:SiteMapDataSource ID="dsMainMenu" runat="server" ShowStartingNode="false"  />