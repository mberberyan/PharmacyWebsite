<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Languages.ascx.cs" Inherits="Languages" %>

<asp:Repeater ID="repLanguages" runat="server" >
    <HeaderTemplate>
        <table cellpadding="2" cellspacing="0" border="0" class="mc_news_fe_lang">
            <tr>
    </HeaderTemplate>
    <ItemTemplate>
                <td><asp:LinkButton ID="lbtnLanguage" runat="server"/></td>
    </ItemTemplate>
    <FooterTemplate>
                
            </tr>
        </table>
    </FooterTemplate>
</asp:Repeater>
