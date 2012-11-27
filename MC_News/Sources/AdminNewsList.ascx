<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminNewsList.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminNewsList" %>
<%@ Register TagPrefix="melon" TagName="Languages" Src="AdminLanguages.ascx" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="AdminPager.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="Melon.Components.News" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left">
            <asp:Label ID="lblManageNews" runat="server" CssClass="mc_news_title" />
        </td>
        <td align="right">
            <melon:Languages ID="cntrlLanguages" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblMessage" runat="server" CssClass="mc_news_message" />
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="mc_news_error_message" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <!-- *** Search area *** -->
            <div class="mc_news_filter" onkeydown="fnTrapKD(btnSearch,event)">
                <div class="mc_news_filter_inner">
                    <div>
                        <!-- Simple Search -->
                        <table cellpadding="0" cellspacing="3" style="float: left; display: inline;">
                            <tr>
                                <td nowrap>
                                    <asp:Label ID="lblEnterKeywords" runat="server" CssClass="mc_news_label" meta:resourcekey="lblEnterKeywords" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKeywords" runat="server" CssClass="mc_news_input_long" Width="323" />
                                </td>
                                <td nowrap>
                                    <asp:Label ID="lblCategory" runat="server" CssClass="mc_news_label mc_news_lblCategory"
                                        meta:resourcekey="lblCategory" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="Id"
                                        CssClass="mc_news_dropdown" Width="178" />
                                </td>
                                <td nowrap>
                                    <asp:Label ID="lblWrittenBy" runat="server" CssClass="mc_news_label mc_news_lblWrittenBy"
                                        meta:resourcekey="lblWrittenBy" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="mc_news_input_short" MaxLength="256"
                                        Width="178" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="padding-top: 7px;">
                                    <!-- Expand/Collapse Advanced Search -->
                                    <asp:HiddenField ID="hfAdvancedSearchStatus" runat="server" />
                                    <a id="lnkAdvancedSearch" class="mc_news_lnk_expand" onclick="CollapseExpandNewsSearchArea();">
                                        <asp:Label ID="lblAdvancedSearch" runat="server" meta:resourcekey="lblAdvancedSearch" /></a>
                                </td>
                            </tr>
                        </table>
                        <!-- Buttons Reset and Search -->
                        <div style="float: right;">
                            <asp:Button ID="btnSearch" runat="server" CssClass="mc_news_button mc_news_btn_61 mc_news_btnSearch"
                                meta:resourcekey="btnSearch" ValidationGroup="NewsFilter" TabIndex="0" /><br />
                            <asp:Button ID="btnReset" runat="server" CssClass="mc_news_button mc_news_btn_61 mc_news_btnReset"
                                meta:resourcekey="btnReset" CausesValidation="False" OnClientClick="ResetNewsSearchCriteria();return false;" />
                        </div>
                    </div>
                    <!-- Advanced Search -->
                    <div id="divAdvancedSearch" class="mc_news_divAdvancedSearch">
                        <table cellpadding="0" cellspacing="3">
                            <tr>
                                <td class="mc_news_lbl_checkbox_search_in">
                                    <asp:Label ID="lblSearchIn" runat="server" CssClass="mc_news_label" meta:resourcekey="lblSearchIn" /></td>
                                <td>
                                    <div class="mc_news_checkbox_search_in">
                                        <asp:CheckBox ID="chkTitle" runat="server" CssClass="mc_news_label " meta:resourcekey="chkTitle"
                                            Checked="true" />
                                        <asp:CheckBox ID="chkSubTitle" runat="server" CssClass="mc_news_label mc_news_checkbox_search_in"
                                            meta:resourcekey="chkSubTitle" />
                                        <asp:CheckBox ID="chkText" runat="server" CssClass="mc_news_label mc_news_checkbox_search_in"
                                            meta:resourcekey="chkText" />
                                    </div>
                                    <div>
                                        <asp:Label ID="lblSearchFieldsComment" runat="server" CssClass="mc_news_comment"
                                            meta:resourcekey="lblSearchFieldsComment" />
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblStatus" runat="server" CssClass="mc_news_label" meta:resourcekey="lblStatus" />
                                </td>
                                <td width="126">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="mc_news_dropdown" Width="135">
                                        <asp:ListItem Value="0" Text="<%$Resources:All%>" />
                                        <asp:ListItem Value="1" Text="<%$Resources:Approved%>" />
                                        <asp:ListItem Value="2" Text="<%$Resources:Pending%>" />
                                        <asp:ListItem Value="3" Text="<%$Resources:Expired%>" />
                                    </asp:DropDownList>
                                </td>
                                <td nowrap>
                                    <asp:Label ID="lblPublishedBetween" runat="server" CssClass="mc_news_label mc_news_lblPublishedBetween"
                                        meta:resourcekey="lblPublishedBetween" />
                                </td>
                                <td width="433px">
                                    <div class="mc_news_divDate">
                                        <asp:TextBox ID="txtDatePostedFrom" runat="server" CssClass="mc_news_input_short"
                                            Width="118" /><br />
                                        <asp:Label runat="server" CssClass="mc_news_comment" Text="<%$Resources:DateFormat%>" /><br />
                                        <asp:RegularExpressionValidator ID="revDatePostedFrom" runat="server" ControlToValidate="txtDatePostedFrom"
                                            ValidationGroup="NewsFilter" ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                            Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revDatePostedFrom" />
                                    </div>
                                    <div style="float: left">
                                        <asp:ImageButton ID="ibtnOpenCalendarFrom" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/calendar.gif") %>' />
                                        <ajaxToolkit:CalendarExtender ID="ceDateFrom" runat="server" Format="MM/dd/yyyy"
                                            TargetControlID="txtDatePostedFrom" PopupButtonID="ibtnOpenCalendarFrom" Enabled="True"
                                            CssClass="ajax__calendar  mc_news_calendar" />
                                        <asp:Label ID="lblAnd" runat="server" CssClass="mc_news_label mc_news_lblAnd" meta:resourcekey="lblAnd" />
                                    </div>
                                    <div class="mc_news_divDate">
                                        <asp:TextBox ID="txtDatePostedTo" runat="server" CssClass="mc_news_input_short" Width="118" /><br />
                                        <asp:Label runat="server" CssClass="mc_news_comment" Text="<%$Resources:DateFormat%>" /><br />
                                        <asp:RegularExpressionValidator ID="revDatePostedTo" runat="server" ControlToValidate="txtDatePostedTo"
                                            ValidationGroup="NewsFilter" ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                            Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revDatePostedTo" />
                                    </div>
                                    <asp:ImageButton ID="ibtnOpenCalendarTo" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/calendar.gif") %>' />
                                    <ajaxToolkit:CalendarExtender ID="ceDateTo" runat="server" Format="MM/dd/yyyy" TargetControlID="txtDatePostedTo"
                                        PopupButtonID="ibtnOpenCalendarTo" Enabled="True" CssClass="ajax__calendar  mc_news_calendar" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="padding-top: 5px;">
                                    <asp:CheckBox ID="chkShowFeaturedOnly" runat="server" meta:resourcekey="chkShowFeaturedOnly"
                                        TextAlign="Left" CssClass="mc_news_label" />
                                    <asp:CheckBox ID="chkShowTranslatedOnly" runat="server" meta:resourcekey="chkShowTranslatedOnly"
                                        TextAlign="Left" CssClass="mc_news_label mc_news_lblShowOnlyTranslated" />
                                </td>
                                <td>
                                    <asp:Label ID="lblNewsId" runat="server" CssClass="mc_news_label mc_news_newsid_lbl"
                                        meta:resourcekey="lblNewsId" />
                                    <asp:TextBox ID="txtNewsId" runat="server" MaxLength="9" Width="50" /><br />
                                    <asp:RegularExpressionValidator ID="revNewsId" runat="server" ControlToValidate="txtNewsId"
                                        ValidationExpression="\d*" ValidationGroup="NewsFilter" SetFocusOnError="true"
                                        Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revNewsId" />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
            </div>
            <!-- Pager for the grid view with news-->
            <melon:Pager ID="TopPager" runat="server" CssClass="mc_news_pager" ShowItemsDetails="false"
                Visible="false" />
            <!-- *** Found news *** -->
            <asp:GridView ID="gvNews" runat="server" AutoGenerateColumns="False" GridLines="None"
                Width="100%" CssClass="mc_news_grid" HeaderStyle-CssClass="mc_news_grid_header"
                EmptyDataRowStyle-BackColor="White" RowStyle-CssClass="mc_news_grid_row" AlternatingRowStyle-CssClass="mc_news_grid_alt_row"
                EmptyDataText="<%$Resources:NoNews%>" AllowPaging="true" PagerSettings-Visible="false"
                AllowSorting="true">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <input id="chkAll" type="checkbox" title="<%#Convert.ToString(GetLocalResourceObject("SelectUnselectAll")) %>"
                                onclick="<%# "ChangeAllCheckBoxStates(this.checked,'" + gvNews.ClientID + "')" %>" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkNews" type="checkbox" runat="server" onclick='<%# "ChangeCheckBoxState(\"chkAll\",\"" + gvNews.ClientID + "\")" %>'
                                value='<%# DataBinder.Eval(Container.DataItem,"Id")%>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col1" />
                        <ItemStyle CssClass="mc_news_newslist_col1" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortById" runat="server" Text="<%$ Resources:Id %>" CommandName="Sort"
                                CommandArgument="Id" CssClass="mc_news_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionId" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                                ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="Id" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col2" />
                        <ItemStyle CssClass="mc_news_newslist_col2" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortByTitle" runat="server" Text="<%$ Resources:Title %>"
                                CommandName="Sort" CommandArgument="Title" CssClass="mc_news_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionTitle" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                                ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="Title" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTitle" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col3" />
                        <ItemStyle CssClass="mc_news_newslist_col3" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortBySubTitle" runat="server" Text="<%$ Resources:SubTitle %>"
                                CommandName="Sort" CommandArgument="SubTitle" CssClass="mc_news_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionSubTitle" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                                ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="SubTitle" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSubTitle" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col4" />
                        <ItemStyle CssClass="mc_news_newslist_col4" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortByCategory" runat="server" Text="<%$ Resources:Category %>"
                                CommandName="Sort" CommandArgument="CategoryName" CssClass="mc_news_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionCategory" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                                ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="CategoryName" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCategory" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col5" />
                        <ItemStyle CssClass="mc_news_newslist_col5" />
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
                            <asp:Label ID="lblAuthor" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col6" />
                        <ItemStyle CssClass="mc_news_newslist_col6" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortByDatePublished" runat="server" Text="<%$ Resources:PublishedOn %>"
                                CommandName="Sort" CommandArgument="DatePosted" CssClass="mc_news_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionDatePublished" runat="server" ImageAlign="AbsMiddle"
                                ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                                ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="DatePosted" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("DatePosted", "{0:MM/dd/yyyy, hh:mm}") %>
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col7" />
                        <ItemStyle CssClass="mc_news_newslist_col7" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortByIsFeatured" runat="server" Text="<%$ Resources:Featured %>"
                                CommandName="Sort" CommandArgument="IsFeatured" CssClass="mc_news_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionIsFeatured" runat="server" ImageAlign="AbsMiddle"
                                ImageUrl='<%# (this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/arrow_down.gif") %>'
                                ToolTip='<%# GetLocalResourceObject(this.SortDirection) %>' Visible='<%# this.SortExpression=="IsFeatured" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblIsFeatured" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col8" />
                        <ItemStyle CssClass="mc_news_newslist_col8" />
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
                            <asp:Label ID="lblIsApproved" runat="server" />
                            <div id="divIsApprovedButton" runat="server">
                                <!--  <span class="mc_news_left_bracket">[</span>-->
                                <asp:LinkButton ID="lbtnApproveDisapprove" runat="server" CssClass="mc_news_link_btn" />
                                <!--<span class="mc_news_right_bracket">]</span>-->
                            </div>
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col9" />
                        <ItemStyle CssClass="mc_news_newslist_col9" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table cellpadding="0" cellspacing="0" class="mc_news_actions">
                                <tr>
                                    <td style="width: 50px;">
                                        <asp:Label ID="lblDisabledLnkPreview" runat="server" meta:resourcekey="lblDisabledLnkPreview"
                                            CssClass="mc_news_disabled_lnk" />
                                        <asp:LinkButton ID="lbtnPreview" runat="server" meta:resourcekey="lbtnPreview" CssClass="mc_news_link_btn" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDisabledLnkEdit" runat="server" meta:resourcekey="lblDisabledLnkEdit"
                                            CssClass="mc_news_disabled_lnk" />
                                        <asp:LinkButton ID="lbtnEdit" runat="server" meta:resourcekey="lbtnEdit" CssClass="mc_news_link_btn" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDisabledLnkDelete" runat="server" meta:resourcekey="lblDisabledLnkDelete"
                                            CssClass="mc_news_disabled_lnk" />
                                        <asp:LinkButton ID="lbtnDelete" runat="server" meta:resourcekey="lbtnDelete" CssClass="mc_news_link_btn"
                                            OnClientClick='<%# "return ConfirmAction(\"" + this.GetLocalResourceObject("DeleteNewsConfirmMessage").ToString() + "\");" %>' />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_newslist_header_col10" />
                        <ItemStyle CssClass="mc_news_newslist_col10" />
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="mc_news_grid_row" />
                <HeaderStyle CssClass="mc_news_grid_header" />
            </asp:GridView>
        </td>
    </tr>
    <!-- Button "Add News" -->
    <tr>
        <td align="right" style="padding-top: 10px;" colspan="2">
            <asp:Button ID="btnLinkNews" runat="server" meta:resourcekey="btnLinkNews" CssClass="mc_news_button mc_news_btn_link_news"
                CausesValidation="false" />
            <asp:Button ID="btnDeleteExpiredNews" runat="server" CssClass="mc_news_button mc_news_btn_delete_expired"
                meta:resourcekey="btnDeleteExpiredNews" CausesValidation="False" OnClientClick='<%# "return ConfirmAction(\"" + this.GetLocalResourceObject("DeleteExpiredNewsConfirmMessage").ToString() + "\");" %>' />
            <asp:Button ID="btnAddNews" runat="server" CssClass="mc_news_button mc_news_btn_add_news"
                meta:resourcekey="btnAddNews" CausesValidation="False" />
        </td>
    </tr>
</table>
<!-- Pop-up Preview -->
<asp:Button ID="btnFireOpenPreview" runat="server" Style="visibility: hidden;" />
<ajaxToolkit:ModalPopupExtender ID="MPEPreviewNews" runat="server" BackgroundCssClass="mc_news_popup_background"
    TargetControlID="btnFireOpenPreview" PopupControlID="divPreviewNews" CancelControlID="btnClosePreviewNews"
    DropShadow="false" Y="40" />
<asp:Panel ID="divPreviewNews" runat="server" Style="display: none;">
    <div class="mc_news_div_preview">
        <div class="mc_news_div_title_preview">
            <asp:Label ID="lblPreviewTitle" runat="server" CssClass="mc_news_title" meta:resourcekey="lblPreviewTitle"  />
        </div>
        <div class="mc_news_div_close_preview">
            <asp:Button ID="btnClosePreviewNews" runat="server" meta:resourcekey="btnClosePreviewNews"
                CssClass="mc_news_button mc_news_btn_61" />
        </div>
        <iframe id="frameNewsPreview" class="mc_news_frame_preview" frameborder="0"></iframe>
    </div>
</asp:Panel>

<script type="text/javascript" language="javascript">
    var hfAdvancedSearchStatusID = '<%=hfAdvancedSearchStatus.ClientID %>';
    
    //Simple search controls
    var txtKeywordsID = '<%= txtKeywords.ClientID %>';
    var ddlCategoryID= '<%= ddlCategory.ClientID %>';
	var txtAuthorID = '<%= txtAuthor.ClientID %>';
   
    //Advanced search controls
	var chkTitleID = '<%= chkTitle.ClientID %>' ; //Title checkbox
	var chkSubTitleID = '<%= chkSubTitle.ClientID %>'; //Subtitle checkbox
	var chkTextID = '<%= chkText.ClientID %>';//Text checkbox

    var ddlStatusID = '<%= ddlStatus.ClientID %>';
    var txtDatePostedFromID = '<%= txtDatePostedFrom.ClientID %>';
    var txtDatePostedToID = '<%= txtDatePostedTo.ClientID %>';
    
    var revDatePostedFromID = '<%= revDatePostedFrom.ClientID %>';
	var revDatePostedToID = '<%= revDatePostedTo.ClientID %>';
	
    var chkFeaturedOnlyID = '<%= chkShowFeaturedOnly.ClientID %>';
    var chkTranslatedOnlyID = '<%= chkShowTranslatedOnly.ClientID%>';
    
    var txtNewsIdID = '<%= txtNewsId.ClientID %>';
    var revNewsIdID = '<%= revNewsId.ClientID %>';
    
    var btnSearch = document.getElementById('<%= btnSearch.ClientID %>');
</script>

