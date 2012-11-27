<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumUserProfileDetails.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.ForumUserProfileDetails" %>
<table cellpadding="0" cellspacing="0" class="mc_forum_table mc_forum_profile_table">
    <tr>
        <th colspan="2">
            <asp:Label ID="lblProfileDetailsTitle" runat="server" meta:resourcekey="lblProfileDetailsTitle" />
        </th>
    </tr>
    <tr>
        <td class="mc_forum_table_row1_padding">
            <asp:Image ID="imgForumUserPhoto" runat="server" /><br />
            <asp:Label ID="lblNickname" runat="server" /><br />
            <br />
            <div>
                <asp:Label ID="lblTotalPostsTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblTotalPostsTitle" />
                <asp:Label ID="lblTotalPostsCount" runat="server" />
            </div>
            <div style="padding-top:5px;">
                <asp:Label ID="lblRegitratedOnTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblRegitratedOnTitle" />
                <asp:Label ID="lblRegistrationDate" runat="server" />
            </div>    
        </td>
        <td class="mc_forum_table_row1_padding" valign="top">
            <div>
                <asp:Label ID="lblFirstNameTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblFirstNameTitle" />
                <asp:Label ID="lblFirstName" runat="server" />
            </div>
            <div style="padding-top:5px;">
                <asp:Label ID="lblLastNameTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblLastNameTitle" />
                <asp:Label ID="lblLastName" runat="server" />
            </div>
            <div style="padding-top:5px;">    
                <asp:Label ID="lblEmailTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblEmailTitle" />
                <asp:HyperLink ID="hlnkEmail" runat="server" CssClass="mc_forum_link" />
            </div>
            <div style="padding-top:5px;">
                <asp:Label ID="lblICQNumberTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblICQNumberTitle" />
                <asp:Label ID="lblICQNumber" runat="server" />
            </div>
        </td>
    </tr>
</table>
