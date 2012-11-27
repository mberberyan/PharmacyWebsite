<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pager" CodeFile="Pager.ascx.cs" %>

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
                    <asp:LinkButton ID="lbtnFirstPage" runat="server" meta:resourcekey="lbtnFirstPage" Visible="false" CausesValidation="false"/>
                    <asp:Label ID="lblPage" runat="server" meta:resourcekey="lblPage" CssClass="mc_cms_pager_lbl"/>
                    <asp:LinkButton ID="lbtnPrevPages" runat="server" Visible="False" CausesValidation="False" meta:resourcekey="lbtnPrevPages"/>
                        
                    <asp:Repeater ID="rptPagesLeft" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnPageNumber" runat="server" CausesValidation="False" />&nbsp;
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Label ID="lblCurrentPage" runat="server"/>&nbsp;
                    
                    <asp:Repeater ID="rptPagesRight" runat="server">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnPageNumber" runat="server" CausesValidation="False" /> &nbsp;
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:LinkButton ID="lbtnNextPages" runat="server" CausesValidation="False" Visible="False" meta:resourcekey="lbtnNextPages"/>
                    <asp:LinkButton ID="lbtnLastPage" runat="server"  meta:resourcekey="lbtnLastPage" CausesValidation="False" Visible="False"/>
                </td>
                
                <td width="50%" align="right">
                    <asp:LinkButton ID="lbtnPreviousPage" runat="server" CausesValidation="False" Visible="False"
                        meta:resourcekey="lbPreviousPage" CssClass="mc_cms_previous" />
                    &nbsp;
                    <asp:LinkButton ID="lbtnNextPage" runat="server" CausesValidation="False" Visible="False"
                        meta:resourcekey="lbNextPage" CssClass="mc_cms_next" />
                </td>
            </tr>
        </table>
    </div>
</div>
