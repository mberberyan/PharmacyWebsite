<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumGroupAddEdit.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.ForumGroupAddEdit" %>
    
<table cellpadding="0" cellspacing="0" class="mc_forum_table mc_forum_group_table">
    <tr>
        <th colspan="2">
            <asp:Label ID="lblForumGroupDetailsTitle" runat="server" meta:resourcekey="lblForumGroupDetailsTitle" /></th>
    </tr>
    <tr>
        <td class="mc_forum_table_label_col mc_forum_table_row1_padding">
            <asp:Label ID="lblForumGroupNameTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblForumGroupNameTitle" /><span
                class="mc_forum_validator">*</span>
        </td>
        <td class="mc_forum_table_label_col mc_forum_table_row1_padding">
            <asp:TextBox ID="txtForumGroupName" runat="server" CssClass="mc_forum_input_long" MaxLength="256" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvForumGroupName" runat="server" ControlToValidate="txtForumGroupName" ValidationGroup="ForumGroupDetails"
                Display="Dynamic" CssClass="mc_forum_validator" meta:resourcekey="rfvForumGroupName" />
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            <asp:CheckBox ID="chkIsActive" runat="server" meta:resourcekey="chkIsActive" Checked="false" CssClass="mc_forum_checkbox" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div class="mc_forum_divForumGroupButtonCancel">
                <asp:Button ID="btnCancel" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnCancel"
                    CausesValidation="false"/></div>
            <div class="mc_forum_divForumGroupButtonSave">       
                <asp:Button ID="btnSaveForumGroup" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnSaveForumGroup"
                    CausesValidation="true" ValidationGroup="ForumGroupDetails"/></div> 
        </td>
    </tr>
</table>
