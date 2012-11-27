<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminCommentList.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminCommentList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="AdminPager.ascx" %>
<%@ Import Namespace="Melon.Components.News" %>

<!--Title -->
<asp:Label ID="lblManageComments" runat="server" meta:resourcekey="lblManageComments"
    CssClass="mc_news_title" />
<!-- Search area  -->
<div class="mc_news_comment_filter" onkeydown="fnTrapKD(btnSearch,event)">
   <table cellpadding="0" cellspacing="3" width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblEnterKeywords" runat="server" CssClass="mc_news_label" meta:resourcekey="lblEnterKeywords" />
                </td>
                <td width="325">
                    <asp:TextBox ID="txtKeywords" runat="server" CssClass="mc_news_input_long" Width="315" MaxLength="500"/>
                </td>
                <td>
                    <asp:Label ID="lblWrittenBy" runat="server" CssClass="mc_news_label" meta:resourcekey="lblWrittenBy" />
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="mc_news_input_short" MaxLength="256" Width="190"/>
                    <div class="mc_news_comment_filter_status">
                        <asp:Label ID="lblStatus" runat="server" CssClass="mc_news_label" meta:resourcekey="lblStatus" />
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="mc_news_dropdown" Width="190">
                            <asp:ListItem Value="all" Selected="true" Text="<%$Resources:All %>" />
                            <asp:ListItem Value="0" Text="<%$Resources:NotApproved %>" />
                            <asp:ListItem Value="1" Text="<%$Resources:Approved %>" />
                        </asp:DropDownList>
                    </div>
                </td>
                <td rowspan="3" class="mc_news_comments_filter_tdButtons">
                    <div class="mc_news_comments_filter_divBtnSearch">
                        <asp:Button ID="btnSearch" runat="server" CssClass="mc_news_button mc_news_btn_61" meta:resourcekey="btnSearch" 
                            CausesValidation="true" ValidationGroup="CommentFilter"/>
                    </div>
                    <div>
                        <asp:Button ID="btnReset" runat="server" CssClass="mc_news_button mc_news_btn_61" meta:resourcekey="btnReset"
                            CausesValidation="false" OnClientClick="ResetCommentSearchCriteria();return false;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCommentsPostedBetween" runat="server" CssClass="mc_news_label"
                        meta:resourcekey="lblCommentsPostedBetween" /></td>
                <td>
                    <div class="mc_news_divDate">
                        <asp:TextBox ID="txtDatePostedFrom" runat="server" CssClass="mc_news_input_short" Width="118" /><br />
                        <asp:Label ID="Label1" runat="server" CssClass="mc_news_comment" Text="<%$Resources:DateFormat%>" /><br />
                        <asp:RegularExpressionValidator ID="revDatePostedFrom" runat="server" ControlToValidate="txtDatePostedFrom"
                            ValidationGroup="CommentFilter" ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                            SetFocusOnError="true" Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revDatePostedFrom" />
                    </div>
                    <div style="float:left;">
                        <asp:ImageButton ID="ibtnOpenCalendarDateFrom" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/calendar.gif") %>' />
                        <ajaxToolkit:CalendarExtender ID="ceDateFrom" runat="server" Format="MM/dd/yyyy"
                            TargetControlID="txtDatePostedFrom" PopupButtonID="ibtnOpenCalendarDateFrom"
                            Enabled="True" CssClass="ajax__calendar  mc_news_calendar" />
                        <asp:Label ID="lblAnd" runat="server" CssClass="mc_news_label mc_news_lblAnd" meta:resourcekey="lblAnd" />
                    </div>
                    <div class="mc_news_divDate">
                        <asp:TextBox ID="txtDatePostedTo" runat="server" CssClass="mc_news_input_short" Width="118" /><br />
                        <asp:Label ID="Label2" runat="server" CssClass="mc_news_comment" Text="<%$Resources:DateFormat%>" /><br />
                         <asp:RegularExpressionValidator ID="revDatePostedTo" runat="server" ControlToValidate="txtDatePostedTo"
                            ValidationGroup="CommentFilter" ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                            SetFocusOnError="true" Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revDatePostedTo" />
                    </div>
                    <div style="float:left">
                        <asp:ImageButton ID="ibtnOpenCalendarDateTo" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/calendar.gif") %>' />
                        <ajaxToolkit:CalendarExtender ID="ceDateTo" runat="server" Format="MM/dd/yyyy" TargetControlID="txtDatePostedTo"
                            PopupButtonID="ibtnOpenCalendarDateTo" Enabled="True" CssClass="ajax__calendar  mc_news_calendar" />
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblNewsTitle" runat="server" CssClass="mc_news_label" meta:resourcekey="lblNewsTitle" />
                </td>
                <td>
                    <asp:TextBox ID="txtNewsTitle" runat="server" CssClass="mc_news_input_long" Width="300" MaxLength="256"/>
                </td>
                <td style="padding-left:10px;">
                    <asp:Label ID="lblNewsId" runat="server" CssClass="mc_news_label" meta:resourcekey="lblNewsId"/>
                </td>
                <td width="60">
                    <asp:TextBox ID="txtNewsId" runat="server" Width="55" MaxLength="9"/><br />
                    <asp:RegularExpressionValidator ID="revNewsId" runat="server" ControlToValidate="txtNewsId"
                        ValidationExpression="\d*" ValidationGroup="CommentFilter" SetFocusOnError="true"
                        Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revNewsId" />
                </td>
            </tr>
          
        </table>
</div>
<melon:Pager ID="topPager" runat="server" ShowItemsDetails="false" CssClass="mc_news_pager"
    Visible="false" />
<asp:GridView ID="gvComments" runat="server" AutoGenerateColumns="false" GridLines="None"
    Width="100%" CssClass="mc_news_grid" HeaderStyle-CssClass="mc_news_grid_header" EmptyDataRowStyle-BackColor="White"
    RowStyle-CssClass="mc_news_grid_row" AlternatingRowStyle-CssClass="mc_news_grid_alt_row" EmptyDataText="<%$Resources:NoCommentFound%>"
    AllowPaging="true" PagerSettings-Visible="false" AllowSorting="true">
    <Columns>
        <asp:TemplateField HeaderText="<%$Resources:Comment %>">
            <ItemTemplate>
                <div id="divCommentText" runat="server" />
            </ItemTemplate>
            <HeaderStyle CssClass="mc_news_commentlist_header_col1" />
            <ItemStyle CssClass="mc_news_commentlist_col1" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByAuthor" runat="server" Text="<%$ Resources:Author %>"
                    CommandName="Sort" CommandArgument="Author" CssClass="mc_news_header_link" />
                <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                <asp:Image ID="imgSortDirectionAuthor" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="Author" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblCommentAuthor" runat="server" />
            </ItemTemplate>
            <HeaderStyle CssClass="mc_news_commentlist_header_col2" />
            <ItemStyle CssClass="mc_news_commentlist_col2" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByDatePosted" runat="server" Text="<%$ Resources:PostedOn %>"
                    CommandName="Sort" CommandArgument="DatePosted" CssClass="mc_news_header_link" />
                <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                <asp:Image ID="imgSortDirectionDatePosted" runat="server" ImageAlign="AbsMiddle"
                    ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="DatePosted" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblDatePosted" runat="server" Text='<%#Eval("DatePosted","{0:MM/dd/yyyy, hh:mm}")%>' />
            </ItemTemplate>
            <HeaderStyle CssClass="mc_news_commentlist_header_col3" />
            <ItemStyle CssClass="mc_news_commentlist_col3" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByNewsTitle" runat="server" Text="<%$ Resources:NewsTitle %>"
                    CommandName="Sort" CommandArgument="NewsTitle" CssClass="mc_news_header_link" />
                <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                <asp:Image ID="imgSortDirectionNewsTitle" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="NewsTitle" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblNewsTitle" runat="server" />
            </ItemTemplate>
            <HeaderStyle CssClass="mc_news_commentlist_header_col4" />
            <ItemStyle CssClass="mc_news_commentlist_col4" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByNewsId" runat="server" Text="<%$ Resources:NewsId %>"
                    CommandName="Sort" CommandArgument="NewsId" CssClass="mc_news_header_link" />
                <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                <asp:Image ID="imgSortDirectionNewsId" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="NewsId" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lblNewsId" runat="server" />
            </ItemTemplate>
            <HeaderStyle CssClass="mc_news_commentlist_header_col5" />
            <ItemStyle CssClass="mc_news_commentlist_col5" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:LinkButton ID="lbtnSortByIsApproved" runat="server" Text="<%$ Resources:IsApproved %>"
                    CommandName="Sort" CommandArgument="IsApproved" CssClass="mc_news_header_link" />
                <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                <asp:Image ID="imgSortDirectionIsApproved" runat="server" ImageAlign="AbsMiddle"
                    ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                    ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="IsApproved" %>' />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="lbtnApproveDisapprove" runat="server" CssClass="mc_news_link_btn" />
            </ItemTemplate>
            <HeaderStyle CssClass="mc_news_commentlist_header_col6" />
            <ItemStyle CssClass="mc_news_commentlist_col6" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <table cellpadding="0" cellspacing="0" class="mc_news_actions">
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtnEdit" runat="server" meta:resourcekey="lbtnEdit" CssClass="mc_news_link_btn" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lbtnDelete" runat="server" meta:resourcekey="lbtnDelete" CssClass="mc_news_link_btn" 
                                OnClientClick='<%# "return ConfirmAction(\"" + this.GetLocalResourceObject("DeleteCommentConfirmMessage").ToString() + "\");" %>' />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <HeaderStyle CssClass="mc_news_commentlist_header_col7" />
            <ItemStyle CssClass="mc_news_commentlist_col7" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<script language="javascript">
    var txtKeywordsID = '<%= txtKeywords.ClientID %>';
    var txtAuthorID = '<%= txtAuthor.ClientID %>';
    var ddlStatusID= '<%= ddlStatus.ClientID %>';
    var txtDatePostedFromID = '<%= txtDatePostedFrom.ClientID %>';
    var txtDatePostedToID = '<%= txtDatePostedTo.ClientID %>';
    var txtNewsTitleID = '<%= txtNewsTitle.ClientID %>';
    var txtNewsIdID = '<%= txtNewsId.ClientID %>';
    
    var revDatePostedFromID = '<%= revDatePostedFrom.ClientID %>';
	var revDatePostedToID = '<%= revDatePostedTo.ClientID %>';   
	var revNewsIdID = '<%= revNewsId.ClientID %>'; 
	
	var btnSearch = document.getElementById('<%= btnSearch.ClientID %>');
</script>
