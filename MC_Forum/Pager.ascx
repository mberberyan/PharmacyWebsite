<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pager.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.Pager" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Import Namespace="Melon.Components.Forum" %>

<div id="divPager" runat="server">

    <!-- Item Details -->
    <table id="tblItemsDetails" runat="server" cellpadding="0" cellspacing="0" border="0"
        width="100%">
        <tr>
            <td width="50%" align="left">
                <b><asp:Label ID="lblTotalTitle" runat="server" meta:resourcekey="lblTotalTitle"/></b>&nbsp;
                <asp:Label ID="lblItemFound" runat="server" meta:resourcekey="lblItemFound" />
            </td>
            <td width="50%" align="right">
                <asp:Label ID="lblShowingItem" runat="server" meta:resourcekey="lblShowingItem" />
            </td>
        </tr>
    </table>
    
    <!-- Pages Details-->
    <div style="padding-top: 5px;">
        <table id="tblPageDetails" runat="server" cellpadding="0" cellspacing="0" border="0"
            width="100%">
            <tr>
                <td width="50%" align="left">
                    <melon:MelonLinkButton ID="lbtnFirstPage" runat="server" meta:resourcekey="lbtnFirstPage" Visible="false" CausesValidation="false"/>
                    <asp:Label ID="lblPage" runat="server" meta:resourcekey="lblPage" />
                    <melon:MelonLinkButton ID="lbtnPrevPages" runat="server" Visible="False" CausesValidation="False" meta:resourcekey="lbtnPrevPages"/>
                        
                    <asp:Repeater ID="rptPageLeft" runat="server">
                        <ItemTemplate>
                            <melon:MelonLinkButton ID="lbtnPageNumber" runat="server" CausesValidation="False" />&nbsp;
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Label ID="lblCurrentPage" runat="server"/>&nbsp;
                    
                    <asp:Repeater ID="rptPageRight" runat="server">
                        <ItemTemplate>
                            <melon:MelonLinkButton ID="lbtnPageNumber" runat="server" CausesValidation="False" /> &nbsp;
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <melon:MelonLinkButton ID="lbtnNextPages" runat="server" CausesValidation="False" Visible="False" meta:resourcekey="lbtnNextPages"/>
                    <melon:MelonLinkButton ID="lbtnLastPage" runat="server"  meta:resourcekey="lbtnLastPage" CausesValidation="False" Visible="False"/>
                </td>
                
                <td width="50%" align="right">
                    <melon:MelonLinkButton ID="lbtnPreviousPage" runat="server" CausesValidation="False" Visible="False"
                        meta:resourcekey="lbPreviousPage" />
                    &nbsp;
                    <melon:MelonLinkButton ID="lbtnNextPage" runat="server" CausesValidation="False" Visible="False"
                        meta:resourcekey="lbNextPage" />
                </td>
            </tr>
        </table>
    </div>
</div>
