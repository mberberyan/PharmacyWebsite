<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminCategoryAddEdit.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminCategoryAddEdit" %>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <!-- Table with category details -->
            <table cellpadding="0" cellspacing="0" border="0" class="mc_news_table mc_news_tbl_category">
                <tr>
                    <th colspan="2">
                        <asp:Label ID="lblTemplate" runat="server" meta:resourcekey="lblCategory" />
                    </th>
                </tr>
                <tr>
                    <td class="mc_news_table_row_padding" >
                        <asp:Label Id="lblIdTitle" runat="server" CssClass="mc_news_label" meta:resourcekey="lblIdTitle"/>
                    </td>
                    <td>
                        <asp:Label Id="lblId" runat="server" CssClass="mc_news_label"/>
                    </td>
                </tr>
              
                <tr>
                    <td nowrap>
                        <asp:Label ID="lblName" runat="server" CssClass="mc_news_label" meta:resourcekey="lblName" />
                        <span class="mc_news_validator">*</span></td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="mc_news_input_long" MaxLength="50"  /></td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                            ValidationGroup="CategorySettings" Display="Dynamic" meta:resourcekey="rfvName"
                            CssClass="mc_news_validator" SetFocusOnError="True" /></td>
                </tr>
                <tr>
                    <td nowrap>
                        <asp:Label ID="lblIsVisible" runat="server" meta:resourcekey="lblIsVisible" CssClass="mc_news_label" /></td>
                    <td>
                        <asp:CheckBox ID="chkIsVisible" runat="server" Checked="True" CssClass="mc_news_chkIsVisible" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="mc_news_button mc_news_btn_61" CausesValidation="true"
                ValidationGroup="CategorySettings" /> &nbsp;
            <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="mc_news_button mc_news_btn_61"
                CausesValidation="false" />
        </td>
    </tr>
</table>