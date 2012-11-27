<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminLanguages.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminLanguages" %>
<asp:Repeater ID="repLanguages" runat="server">
    <HeaderTemplate>
        <table cellpadding="2" cellspacing="0" border="0" class="mc_news_langs">
            <tr>
    </HeaderTemplate>
    <ItemTemplate>
        <td>
            <asp:LinkButton ID="lbtnLanguage" runat="server" CssClass="mc_news_lang_link" /></td>
        <td>
            <div class="mc_news_lang_delimiter">|
            </div>
        </td>
    </ItemTemplate>
    <FooterTemplate>
        </tr> </table>
    </FooterTemplate>
</asp:Repeater>
