<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumSearchCriteria.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.ForumSearchCriteria" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Import Namespace="Melon.Components.Forum" %>
<%@ Import Namespace="Melon.Components.Forum.Configuration" %>

<!-- Search Criteria -->
<div onkeydown="fnTrapKD('<%= btnSearch.ClientID %>',event)" >
<table cellpadding="0" cellspacing="0" class="mc_forum_table mc_forum_search_table">
    <tr>
        <th colspan="2">
            <!--Header -->
            <asp:Label ID="lblForumSearchTitle" runat="server" meta:resourcekey="lblForumSearchTitle" /></th>
    </tr>
    <tr>
        <td class="mc_forum_table_row1_padding mc_forum_search_table_col1">
            <asp:Label ID="lblSearchFor" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblSearchFor" /></td>
        <td class="mc_forum_table_row1_padding mc_forum_search_table_col2">
            <asp:TextBox ID="txtKeywords" runat="server" MaxLength="1024" CssClass="mc_forum_input_long" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblWrittenByTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblWrittenByTitle" /></td>
        <td>
            <asp:TextBox ID="txtAuthor" runat="server" MaxLength="50" CssClass="mc_forum_input_long" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblLocationTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblLocationTitle" /></td>
        <td>
            <asp:TreeView ID="tvForums" runat="server" ShowCheckBoxes="All" ShowLines="true"
                ImageSet="custom" NodeStyle-ImageUrl='<%#Utilities.GetImageUrl(this.Page,"ForumStyles/Images/folder_open.gif")%>'
                CssClass="mc_forum_tree" meta:resourcekey="tvForums" EnableClientScript="false"
                SkipLinkText="">
                <DataBindings>
                    <asp:TreeNodeBinding DataMember="Root" Text="All" Value="0" SelectAction="None" />
                    <asp:TreeNodeBinding DataMember="Group" TextField="Name" ValueField="Id" SelectAction="None" />
                    <asp:TreeNodeBinding DataMember="Forum" TextField="Name" ValueField="Id" SelectAction="None" />
                </DataBindings>
            </asp:TreeView>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDateTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblDateTitle" /></td>
        <td>
            <asp:DropDownList ID="ddlDateCriteria" runat="server" CssClass="mc_forum_dropdown_short">
                <asp:ListItem Value="0" Selected="True" meta:resourcekey="ListItemAll" />
                <asp:ListItem Value="1" meta:resourcekey="ListItemOn" />
                <asp:ListItem Value="2" meta:resourcekey="ListItemBetween" />
            </asp:DropDownList>
            <span id="spanDateFrom" style="visibility: hidden;">
                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="mc_forum_input_short" />
                <asp:ImageButton ID="ibtnOpenCalendarFrom" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page, "ForumStyles/Images/calendar.gif")%>' />
            </span>
            <ajaxToolkit:CalendarExtender ID="CalendarExtenderDateFrom" runat="server" Format="ddd MMM dd, yyyy"
                TargetControlID="txtDateFrom" PopupButtonID="ibtnOpenCalendarFrom" Enabled="True"
                CssClass="ajax__calendar  mc_forum_calendar" />
            <span id="spanDateTo" style="visibility: hidden;">&nbsp;&nbsp;
                <asp:Label ID="lblAnd" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblAnd" />
                &nbsp;&nbsp;
                <asp:TextBox ID="txtDateTo" runat="server" CssClass="mc_forum_input_short" />
                <asp:ImageButton ID="ibtnOpenCalendarTo" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"ForumStyles/Images/calendar.gif")%>' />
            </span>
            <ajaxToolkit:CalendarExtender ID="CalendarExtenderDateTo" runat="server" Format="ddd MMM dd, yyyy"
                TargetControlID="txtDateTo" PopupButtonID="ibtnOpenCalendarTo" Enabled="True"
                CssClass="ajax__calendar mc_forum_calendar" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblSortedByTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblSortedByTitle" /></td>
        <td>
            <asp:DropDownList ID="ddlSortField" runat="server" CssClass="mc_forum_dropdown_short">
                <asp:ListItem Value="0" Selected="True" meta:resourcekey="ListItemDate" />
                <asp:ListItem Value="1" meta:resourcekey="ListItemForum" />
                <asp:ListItem Value="2" meta:resourcekey="ListItemTopic" />
                <asp:ListItem Value="3" meta:resourcekey="ListItemAuthor" />
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td colspan="2">
            <div class="mc_forum_divCancelButton">
                <asp:Button ID="btnClear" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnClear"
                    ValidationGroup="SearchDetails" CausesValidation="false" /></div>
            <div class="mc_forum_divSearchButton">
                <asp:Button ID="btnSearch" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnSearch"
                    ValidationGroup="SearchDetails" OnClick="btnSearch_Click" UseSubmitBehavior="false" />
                    
                    </div>
        </td>
    </tr>
</table>
<asp:Label ID="lblErrorMessage" runat="server" CssClass="mc_forum_error_message"
    Style="display: none;" Text="<%$Resources:MissingSearchLocation %>" />
<asp:CustomValidator ID="cvDateFromFormat" runat="server" ControlToValidate="txtDateFrom"
    ValidationGroup="SearchDetails" Display="Dynamic" CssClass="mc_forum_validator"
    ErrorMessage='<%# (this.ddlDateCriteria.SelectedIndex == 2)?GetLocalResourceObject("InvalidFirstDateFormat").ToString():GetLocalResourceObject("InvalidDateFormat").ToString()%>'
    OnServerValidate="cvDate_ServerValidateDateFormat" />
<asp:CustomValidator ID="cvDateToFormat" runat="server" ControlToValidate="txtDateTo"
    ValidationGroup="SearchDetails" Display="Dynamic" CssClass="mc_forum_validator"
    ErrorMessage='<%# GetLocalResourceObject("InvalidSecondDateFormat").ToString()%>'
    OnServerValidate="cvDate_ServerValidateDateFormat" />
<asp:CustomValidator ID="cvDateFrom" runat="server" ControlToValidate="txtDateFrom"
    ValidationGroup="SearchDetails" Display="Dynamic" CssClass="mc_forum_validator"
    ErrorMessage='<%# String.Format(GetLocalResourceObject("cvDateFrom.ErrorMessage").ToString(), ForumSettings.MinimumSearchDateValue.ToString("ddd MMM dd, yyyy"))%>'
    OnServerValidate="cvDate_ServerValidate" />
<asp:CustomValidator ID="cvDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="SearchDetails"
    Display="Dynamic" CssClass="mc_forum_validator" ErrorMessage='<%# String.Format(GetLocalResourceObject("cvDateTo.ErrorMessage").ToString(), ForumSettings.MinimumSearchDateValue.ToString("ddd MMM dd, yyyy"))%>'
    OnServerValidate="cvDate_ServerValidate" />

</div>
