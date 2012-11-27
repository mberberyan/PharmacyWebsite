<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminCommentAddEdit.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminCommentAddEdit" %>

<asp:Label ID="lblEditComments" runat="server" CssClass="mc_news_title" meta:resourcekey="lblEditComments"/>
<table cellpadding="0" cellspacing="0" class="mc_news_tbl_edit_comments">
    <tr>
        <td> <asp:Label ID="lblMessage" runat="server" CssClass="mc_news_message" /></td>
    </tr>
    <tr>
        <td>
            <!-- Table with comment details -->
            <table cellpadding="0" cellspacing="0" border="0" class="mc_news_table">
                <tr>
                    <th colspan="2">
                        <asp:Label ID="lblEditComment" runat="server" meta:resourcekey="lblEditComment" />
                    </th>
                </tr>
                <tr id="trAuthorDetails" runat="server">
                    <td>
                        <asp:Label Id="lblAuthorTitle" runat="server" CssClass="mc_news_label" meta:resourcekey="lblAuthorTitle"/>
                        <span class="mc_news_validator">*</span>
                    </td>
                    <td>
                        <asp:TextBox Id="txtAuthor" runat="server" MaxLength="256"/><br />
                        <asp:RequiredFieldValidator ID="rfvAuthor" runat="server" ControlToValidate="txtAuthor"
                            ValidationGroup="CommentSettings" SetFocusOnError="true"
                            Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="rfvAuthor" />
                    </td>
                </tr>
              
                <tr>
                    <td>
                        <asp:Label ID="lblCommentTextTitle" runat="server" CssClass="mc_news_label" meta:resourcekey="lblCommentTextTitle" />
                        <span class="mc_news_validator">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCommentText" runat="server" CssClass="mc_news_listbox" TextMode="MultiLine" Rows="5" MaxLength="350"/><br />
                        <asp:RequiredFieldValidator ID="rfvCommentText" runat="server" ControlToValidate="txtCommentText"
                            ValidationGroup="CommentSettings" SetFocusOnError="true"
                            Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="rfvCommentText" />
                        <asp:RegularExpressionValidator ID="revCommentText" runat="server" ControlToValidate="txtCommentText"
                            ValidationExpression="(.|\n){0,350}"
                            ValidationGroup="CommentSettings" Display="Dynamic" CssClass="mc_news_validator"
                            meta:resourcekey="revCommentText" /></td>
                </tr>
              
                <tr>
                    <td nowrap>
                        <asp:Label ID="lblApprove" runat="server" meta:resourcekey="lblApprove" CssClass="mc_news_label" /></td>
                    <td>
                        <asp:CheckBox ID="chkIsApproved" runat="server" CssClass="mc_news_cooment_chk_approve"/></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="mc_news_button mc_news_btn_61" CausesValidation="true"
                ValidationGroup="CommentSettings" /> &nbsp;
            <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="mc_news_button mc_news_btn_61"
                CausesValidation="false"/>
        </td>
    </tr>
</table>
