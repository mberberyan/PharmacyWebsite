<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumPostAddEdit.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.ForumPostAddEdit" %>
    
<table cellpadding="0" cellspacing="0" class="mc_forum_table mc_forum_post_table">
    <tr>
        <th colspan="2">
            <asp:Label ID="lblPostReplyTitle" runat="server" meta:resourcekey="lblPostReplyTitle" /></th>
    </tr>
    <tr>
        <td style="vertical-align: top;" class="mc_forum_table_label_col mc_forum_table_row1_padding">
            <asp:Label ID="lblTextTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblTextTitle" />
            <span class="mc_forum_validator">*</span></td>
        <td class="mc_forum_table_row1_padding">
            <asp:TextBox ID="txtPostText" runat="server" CssClass="mc_forum_input_long" TextMode="MultiLine"
                Rows="10"/></td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvPostText" runat="server" ControlToValidate="txtPostText" ValidationGroup="PostDetails" Display="Dynamic"
                CssClass="mc_forum_validator" meta:resourcekey="rfvPostText" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <div class="mc_forum_divForumPostButtonCancel">
                <asp:Button ID="btnCancel" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnCancel" 
                    CausesValidation="false"/></div>
            <div class="mc_forum_divForumPostButtonPost">
                <asp:Button ID="btnPost" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnPost"
                    CausesValidation="true" ValidationGroup="PostDetails"/></div>
        </td>
    </tr>
</table>
