<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumAddEdit.ascx.cs"  Inherits="Melon.Components.Forum.UI.CodeBehind.ForumAddEdit" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Import Namespace="Melon.Components.Forum" %>

<asp:HiddenField ID="hfAdministrators" runat="server"/>
<asp:HiddenField ID="hfModerators" runat="server"/>

<table cellpadding="0" cellspacing="0" class="mc_forum_table mc_forum_forum_table">
    <tr>
        <th colspan="2">
            <!--Header -->
            <asp:Label ID="lblForumDetailsTitle" runat="server" meta:resourcekey="lblForumDetailsTitle" /></th>
    </tr>
    <tr>
        <td class="mc_forum_table_label_col mc_forum_table_row1_padding">
            <asp:Label ID="lblForumNameTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblForumNameTitle" />
            <span class="mc_forum_validator">*</span></td>
        <td class="mc_forum_table_label_col mc_forum_table_row1_padding">
            <asp:TextBox ID="txtForumName" runat="server" CssClass="mc_forum_input_long" MaxLength="256" /></td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvForumName" runat="server" ControlToValidate="txtForumName"
                ValidationGroup="ForumDetails" Display="Dynamic" CssClass="mc_forum_validator"
                meta:resourcekey="rfvForumName" /></td>
    </tr>
    <tr>
        <td class="mc_forum_table_label_col">
            <asp:Label ID="lblForumGroupTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblForumGroupTitle" />
            <span class="mc_forum_validator">*</span></td>
        <td>
            <asp:DropDownList ID="ddlForumGroups" runat="server" DataTextField="Name" DataValueField="Id" CssClass="mc_forum_dropdown_long" /></td>
    </tr>
    <tr>
        <td class="mc_forum_table_label_col" style="vertical-align: top;">
            <asp:Label ID="lblForumDescriptionTitle" runat="server" CssClass="mc_forum_label"
                meta:resourcekey="lblForumDescriptionTitle" /></td>
        <td>
            <asp:TextBox ID="txtForumDescription" runat="server" CssClass="mc_forum_input_long"
                TextMode="MultiLine" Rows="5" MaxLength="1024" /></td>
    </tr>
    <tr>
        <td class="mc_forum_table_label_col" style="vertical-align: top;">
            <asp:Label ID="lblUserRightsTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblUserRightsTitle" />
            
        </td>
        <td>
            <div class="mc_forum_divForumUsers">
                <!-- Forum Administrators -->
                <div class="mc_forum_divAdministrators">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblAvailableAdministratorsTitle" runat="server" CssClass="mc_forum_label"
                                    meta:resourcekey="lblAvailableForumUsersTitle" /></td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblAdministratorsTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblAdministratorsTitle" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstCandidateAdministrators" runat="server" Rows="10" SelectionMode="Multiple" CssClass="mc_forum_listbox"  
                                    DataTextField="Nickname" DataValueField="UserName" /></td>
                            <td>
                                <asp:ImageButton ID="ibtnAddAdministrator" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"ForumStyles/Images/move_right.gif")%>'
                                    meta:resourcekey="ibtnAddAdministrator" /><br />
                                <br />
                                <asp:ImageButton ID="ibtnRemoveAdministrator" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"ForumStyles/Images/move_left.gif")%>'
                                    meta:resourcekey="ibtnRemoveAdministrator" /></td>
                            <td>
                                <asp:ListBox ID="lstAdministrators" runat="server" Rows="10" SelectionMode="Multiple" CssClass="mc_forum_listbox"
                                    DataTextField="Nickname" DataValueField="UserName" /></td>
                        </tr>
                    </table>
                </div>
                <!-- Forum Moderators-->
                <div class="mc_forum_divModerators">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAvailableModeratorsTitle" runat="server" CssClass="mc_forum_label"
                                    meta:resourcekey="lblAvailableForumUsersTitle" /></td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblModeratorsTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblModeratorsTitle" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstCandidateModerators" runat="server" Rows="10" SelectionMode="Multiple" CssClass="mc_forum_listbox" 
                                    DataTextField="Nickname" DataValueField="UserName" /></td>
                            <td>
                                <asp:ImageButton ID="ibtnAddModerator" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/move_right.gif")%>'
                                    meta:resourcekey="ibtnAddModerator" /><br />
                                <br />
                                <asp:ImageButton ID="ibtnRemoveModerator" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"ForumStyles/Images/move_left.gif")%>'
                                    meta:resourcekey="ibtnRemoveModerator" /></td>
                            <td>
                                <asp:ListBox ID="lstModerators" runat="server" Rows="10" SelectionMode="Multiple" CssClass="mc_forum_listbox" 
                                    DataTextField="Nickname" DataValueField="UserName" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td></td>
        <td style="padding-top:10px;">
            <asp:CheckBox ID="chkIsActive" runat="server" meta:resourcekey="chkIsActive" Checked="false" CssClass="mc_forum_checkbox" />
            <asp:CheckBox ID="chkIsClosed" runat="server" meta:resourcekey="chkIsClosed" Checked="false" CssClass="mc_forum_checkbox" />
            <asp:CheckBox ID="chkIsPublic" runat="server" meta:resourcekey="chkIsPublic" Checked="false" CssClass="mc_forum_checkbox" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <div class="mc_forum_divForumButtonCancel">
                <asp:Button ID="btnCancel" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnCancel"  
                    CausesValidation="false" /></div>
            <div class="mc_forum_divForumButtonSave">
                <asp:Button ID="btnSaveForum" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnSaveForum" 
                    CausesValidation="true" ValidationGroup="ForumDetails"/></div></td>
    </tr>
</table>
