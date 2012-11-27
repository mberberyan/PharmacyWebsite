<%@ Control Language="C#" AutoEventWireup="true" Inherits="Languages" CodeFile="Languages.ascx.cs" %>

<asp:Repeater ID="repLanguages" runat="server">
    <HeaderTemplate>
        <table cellpadding="2" cellspacing="0" border="0">
            <tr>
    </HeaderTemplate>
    <ItemTemplate>
                <td><asp:LinkButton ID="lbtnLanguage" runat="server" CssClass="mc_cms_lang_link"  /> &nbsp;|&nbsp; </td>
    </ItemTemplate>
    <FooterTemplate>
                <td><div class="mc_cms_lang_delimiter"></div></td>
            </tr>
        </table>
    </FooterTemplate>
</asp:Repeater>