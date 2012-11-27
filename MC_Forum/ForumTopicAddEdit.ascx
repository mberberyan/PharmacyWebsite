<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumTopicAddEdit.ascx.cs"  Inherits="Melon.Components.Forum.UI.CodeBehind.ForumTopicAddEdit" %>
 
<table cellpadding="0" cellspacing="0" class="mc_forum_table mc_forum_topic_table">
    <tr>
        <th colspan="2">
            <asp:Label ID="lblNewTopicTitle" runat="server" meta:resourcekey="lblNewTopicTitle" /></th>
    </tr>
    <tr>
        <td class="mc_forum_table_label_col mc_forum_table_row1_padding">
            <asp:Label ID="lblTopicTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblTopicTitle" />
            <span class="mc_forum_validator">*</span></td>
        <td class="mc_forum_table_row1_padding">
            <asp:TextBox ID="txtTopicTitle" runat="server" MaxLength="256" CssClass="mc_forum_input_long" /></td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvTopicTitle" runat="server" ControlToValidate="txtTopicTitle"
                Display="Dynamic" CssClass="mc_forum_validator" meta:resourcekey="rfvTopicTitle"
                ValidationGroup="TopicDetails" /></td>
    </tr>
    <tr>
        <td style="vertical-align: top;" class="mc_forum_table_label_col">
            <asp:Label ID="lblDescriptionTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblDescriptionTitle" /></td>
        <td>
            <asp:TextBox ID="txtTopicDescription" runat="server" CssClass="mc_forum_input_long"
                TextMode="MultiLine" MaxLength ="1024" Rows="10" /></td>
    </tr>
    <tr id="trTopicType" runat="server">
        <td class="mc_forum_table_label_col">
            <asp:Label ID="lblTopicTypeTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblTopicTypeTitle" /></td>
        <td>
            <asp:DropDownList ID="ddlTopicTypes" runat="server" CssClass="mc_forum_dropdown_long"> 
                <asp:ListItem Text='<%$Resources:TopicTypeNormal%>' Value="1" Selected="True"/>             
                <asp:ListItem Text='<%$Resources:TopicTypeSticky%>' Value="2" />
                <asp:ListItem Text='<%$Resources:TopicTypeAnnouncement%>' Value="3" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trTopicActions" runat="server">
        <td></td>
        <td style="padding-top:10px;">
            <asp:CheckBox ID="chkIsActive" runat="server" meta:resourcekey="chkIsActive" Checked="true" CssClass="mc_forum_checkbox"/>
            <asp:CheckBox ID="chkIsClosed" runat="server" meta:resourcekey="chkIsClosed" Checked="false" CssClass="mc_forum_checkbox"/>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div class="mc_forum_divForumTopicButtonCancel">
                <asp:Button ID="btnCancel" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnCancel"
                    CausesValidation="False"/></div>
            <div class="mc_forum_divForumTopicButtonPost">
                <asp:Button ID="btnPostTopic" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnPostTopic"
                    CausesValidation="true" ValidationGroup="TopicDetails"/></div>
        </td>
    </tr>
</table>

