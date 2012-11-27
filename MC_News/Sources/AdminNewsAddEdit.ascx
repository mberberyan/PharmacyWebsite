<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminNewsAddEdit.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminNewsAddEdit" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="melon" TagName="Languages" Src="AdminLanguages.ascx" %>
<%@ Register TagPrefix="melon" TagName="LinkedNews" Src="AdminNewsList.ascx" %>
<%@ Import Namespace="Melon.Components.News" %>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td align="left">
            <!-- Title -->
            <asp:Label ID="lblNewsAddEditTitle" runat="server" CssClass="mc_news_title" />
        </td>
        <td align="right">
            <!-- Languages -->
            <melon:Languages ID="cntrlLanguages" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height:15px;">
            <asp:Label ID="lblMessage" runat="server" CssClass="mc_news_message" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <!-- Table with News Details -->
            <table cellpadding="0" cellspacing="0" class="mc_news_table mc_news_tbl_addedit">
                <tr>
                    <th colspan="2">
                        <asp:Label ID="lblNewsDetails" runat="server" meta:resourcekey="lblNewsDetails" />
                    </th>
                </tr>
                <tr>
                    <td style="padding: 0px;">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblIdTitle" runat="server" CssClass="mc_news_label" meta:resourcekey="lblIdTitle" />
                                </td>
                                <td>
                                    <asp:Label ID="lblId" runat="server" CssClass="mc_news_label" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" CssClass="mc_news_label" />
                                    <span class="mc_news_validator">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="256" CssClass="mc_news_input_long"
                                        Width="395" /><br />
                                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                                        ValidationGroup="NewsSettings" Display="Dynamic" CssClass="mc_news_validator"
                                        meta:resourcekey="rfvTitle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSubTitle" runat="server" meta:resourcekey="lblSubTitle" CssClass="mc_news_label" /></td>
                                <td>
                                    <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="256" CssClass="mc_news_input_long"
                                        Width="395" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCategory" runat="server" meta:resourcekey="lblCategory" CssClass="mc_news_label" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="Id"
                                        CssClass="mc_news_dropdown" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPhoto" runat="server" meta:resourcekey="lblPhoto" CssClass="mc_news_label" /></td>
                                <td>
                                    <asp:FileUpload ID="fuPhoto" runat="server" CssClass="mc_news_input_file" size="65" /><br />
                                    <asp:Label ID="lblPhotoUploadInstructions" runat="server" CssClass="mc_news_comment" /><br />
                                    <asp:RegularExpressionValidator ID="revPhotoPath" runat="server" ControlToValidate="fuPhoto"
                                        ValidationGroup="NewsSettings" ValidationExpression="(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)"
                                        Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revPhotoPath" />
                                    <div id="divPhoto" runat="server" visible="false">
                                        <asp:Image ID="imgPhoto" runat="server" />
                                        <asp:CheckBox ID="chkRemovePhoto" runat="server" meta:resourcekey="chkRemovePhoto"
                                            CssClass="mc_news_label" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td width="40">
                                    <asp:Label ID="lblPhotoDescription" runat="server" meta:resourcekey="lblPhotoDescription"
                                        CssClass="mc_news_label" /></td>
                                <td>
                                    <asp:TextBox ID="txtPhotoDescription" runat="server" MaxLength="1024" CssClass="mc_news_listbox"
                                        TextMode="MultiLine" Rows="4" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPostedOn" runat="server" meta:resourcekey="lblPostedOn" CssClass="mc_news_label" /></td>
                                <td>
                                    <div class="mc_news_divDate">
                                        <asp:TextBox ID="txtDatePosted" runat="server" MaxLength="10" CssClass="mc_news_input_short"
                                            Width="148" /><br />
                                        <asp:Label ID="Label2" runat="server" CssClass="mc_news_comment" Text="<%$Resources:DateFormat%>" /><br />
                                    </div>
                                    <div style="float: left;">
                                        <asp:ImageButton ID="ibtnOpenCalendarDatePosted" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/calendar.gif") %>' />
                                        <ajaxToolkit:CalendarExtender ID="ceDatePosted" runat="server" Format="MM/dd/yyyy"
                                            TargetControlID="txtDatePosted" PopupButtonID="ibtnOpenCalendarDatePosted" Enabled="True"
                                            CssClass="ajax__calendar  mc_news_calendar" />
                                        <!-- Time Posted -->
                                        <asp:DropDownList ID="ddlHourPosted" runat="server" CssClass="mc_news_dropdown mc_news_ddlHourPosted"
                                            Width="45" />
                                        :
                                        <asp:DropDownList ID="ddlMinutesPosted" runat="server" CssClass="mc_news_dropdown"
                                            Width="45" DataTextFormatString="{0:D2}" />
                                    </div>
                                    <div style="clear: both;">
                                        <asp:RegularExpressionValidator ID="revDatePosted" runat="server" ControlToValidate="txtDatePosted"
                                            ValidationGroup="NewsSettings" ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                            Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revDatePosted" />
                                        <asp:CustomValidator runat="server" ID="cvTimePosted" ClientValidationFunction="ValidateNewsDateTimePosted"
                                            ValidationGroup="NewsSettings" Display="dynamic" CssClass="mc_news_validator"
                                            meta:resourcekey="cvTimePosted" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPostedBy" runat="server" meta:resourcekey="lblPostedBy" CssClass="mc_news_label" /></td>
                                <td>
                                    <asp:TextBox ID="txtAuthor" runat="server" MaxLength="256" CssClass="mc_news_input_short" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSource" runat="server" meta:resourcekey="lblSource" CssClass="mc_news_label" /></td>
                                <td>
                                    <asp:TextBox ID="txtSource" runat="server" MaxLength="256" CssClass="mc_news_input_short" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblValidFrom" runat="server" meta:resourcekey="lblValidFrom" CssClass="mc_news_label" /></td>
                                <td>
                                    <div class="mc_news_divDate">
                                        <asp:TextBox ID="txtDateValidFrom" runat="server" MaxLength="10" CssClass="mc_news_input_short"
                                            Width="148" /><br />
                                        <asp:Label runat="server" CssClass="mc_news_comment" Text="<%$Resources:DateFormat%>" /><br />
                                        <asp:RegularExpressionValidator ID="revDateValidFrom" runat="server" ControlToValidate="txtDateValidFrom"
                                            ValidationGroup="NewsSettings" ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                            Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revDateValidFrom" />
                                    </div>
                                    <div style="float: left">
                                        <asp:ImageButton ID="ibtnOpenCalendarDateValidFrom" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/calendar.gif") %>' />
                                        <ajaxToolkit:CalendarExtender ID="ceDateValidFrom" runat="server" Format="MM/dd/yyyy"
                                            TargetControlID="txtDateValidFrom" PopupButtonID="ibtnOpenCalendarDateValidFrom"
                                            Enabled="True" CssClass="ajax__calendar  mc_news_calendar" />
                                        &nbsp;
                                        <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" CssClass="mc_news_label" />
                                        &nbsp;
                                    </div>
                                    <div class="mc_news_divDate">
                                        <asp:TextBox ID="txtDateValidTo" runat="server" MaxLength="10" CssClass="mc_news_input_short"
                                            Width="148" /><br />
                                        <asp:Label ID="Label1" runat="server" CssClass="mc_news_comment" Text="<%$Resources:DateFormat%>" /><br />
                                        <asp:RegularExpressionValidator ID="revDateValidTo" runat="server" ControlToValidate="txtDateValidTo"
                                            ValidationGroup="NewsSettings" ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                            Display="Dynamic" CssClass="mc_news_validator" meta:resourcekey="revDateValidTo" />
                                    </div>
                                    <asp:ImageButton ID="ibtnOpenCalendarDateValidTo" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"NewsStyles/BackEndImages/calendar.gif") %>' />
                                    <ajaxToolkit:CalendarExtender ID="ceDateValidTo" runat="server" Format="MM/dd/yyyy"
                                        TargetControlID="txtDateValidTo" PopupButtonID="ibtnOpenCalendarDateValidTo"
                                        Enabled="True" CssClass="ajax__calendar  mc_news_calendar" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTags" runat="server" meta:resourcekey="lblTags" CssClass="mc_news_label" /></td>
                                <td>
                                    <asp:TextBox ID="txtTags" runat="server" MaxLength="1024" CssClass="mc_news_listbox"
                                        TextMode="MultiLine" Rows="4" /><br />
                                    <asp:Label ID="lblTagsInstructions" runat="server" CssClass="mc_news_comment" meta:resourcekey="lblTagsInstructions" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkIsFeatured" runat="server" meta:resourcekey="chkIsFeatured"
                                        TextAlign="Left" CssClass="mc_news_label" />
                                    <asp:CheckBox ID="chkIsApproved" runat="server" meta:resourcekey="chkIsApproved"
                                        TextAlign="Left" CssClass="mc_news_label mc_news_chkIsApproved" />
                                </td>
                            </tr>
                            <tr id="trLogDetails" runat="server">
                                <td colspan="2" class="mc_news_label">
                                    <asp:Label ID="lblCreatedOn" runat="server" meta:resourcekey="lblCreatedOn" />
                                    <asp:Label ID="lblDateCreated" runat="server" Text='<%# this.dateCreated.ToString("MM/dd/yyyy, hh:mm") %>' />
                                    <asp:Label ID="lblCreatedBy" runat="server" meta:resourcekey="lblBy" />
                                    <asp:Label ID="lblCreatedByUsername" runat="server" />
                                    <div id="divLogLastUpdated" runat="server" style="padding-top: 5px;">
                                        <asp:Label ID="lblLastUpdatedOn" runat="server" meta:resourcekey="lblLastUpdatedOn" />
                                        <asp:Label ID="lblDateLastUpdated" runat="server" Text='<%# this.dateLastUpdated.ToString("MM/dd/yyyy, hh:mm") %>' />
                                        <asp:Label ID="lblLastUpdatedBy" runat="server" meta:resourcekey="lblBy" />
                                        <asp:Label ID="lblLastUpdatedByUserName" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Label ID="lblText" runat="server" meta:resourcekey="lblText" CssClass="mc_news_label" />
                        <span class="mc_news_validator">*</span>
                        <FCKeditorV2:FCKeditor ID="HTMLEditor" runat="server" Width="540px" Height="480px"
                            HtmlEncodeOutput="false" />
                        <asp:CustomValidator runat="server" ID="cvText" ClientValidationFunction="ValidateNewsText"
                            ValidationGroup="NewsSettings" Display="dynamic" CssClass="mc_news_validator"
                            meta:resourcekey="cvText" />
                        <!-- Actions -->
                        <div class="mc_news_btns_left">
                            <asp:Button ID="btnPreview" runat="server" CssClass="mc_news_button mc_news_btn_61"
                                meta:resourcekey="btnPreview" />&nbsp;
                            <asp:Button ID="btnAddNews" runat="server" CssClass="mc_news_button mc_news_btn_add_news"
                                meta:resourcekey="btnAddNews" />
                        </div>
                        <div class="mc_news_btns_right">
                            <asp:Button ID="btnSave" runat="server" CssClass="mc_news_button mc_news_btn_61"
                                meta:resourcekey="btnSave" ValidationGroup="NewsSettings" />&nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="mc_news_button mc_news_btn_61"
                                meta:resourcekey="btnCancel" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <!-- Linked News -->
            <table id="tblLinkedNews" cellpadding="0" cellspacing="0" class="mc_news_table mc_news_tbl_linked_news"
                runat="server">
                <tr>
                    <th>
                        <div style="float: left;">
                            <asp:Label ID="lblLinkedNews" runat="server" /></div>
                        <div style="float: right;">
                            <asp:LinkButton ID="lbtnAddLinkedNews" runat="server" CssClass="mc_news_header_link"
                                meta:resourcekey="lbtnAddLinkedNews" /></div>
                    </th>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Repeater ID="repLinkedNews" runat="server">
                            <HeaderTemplate>
                                <table width="100%">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblId" runat="server" /></td>
                                    <td>
                                        <asp:Label ID="lblTitle" runat="server" /></td>
                                    <td align="right">
                                        <asp:LinkButton ID="lbtnRemove" runat="server" meta:resourcekey="lbtnRemove" CssClass="mc_news_link_btn" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnUpdateLinkedNewsList" runat="server" Style="visibility: hidden;" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-left: 5px;">
            <!-- Comments -->
            <asp:LinkButton ID="lbtnManageComments" runat="server" CssClass="mc_news_link_btn" />
        </td>
    </tr>
</table>
<!-- Pop-up Preview -->
<ajaxToolkit:ModalPopupExtender ID="MPEPreviewNews" runat="server" BackgroundCssClass="mc_news_popup_background"
    TargetControlID="btnPreview" PopupControlID="divPreviewNews" CancelControlID="btnClosePreviewNews"
    DropShadow="false" Y="40" />
<asp:Panel ID="divPreviewNews" runat="server" Style="display: none;">
    <div class="mc_news_div_preview">
        <div class="mc_news_div_title_preview">
            <asp:Label ID="lblPreviewTitle" runat="server" CssClass="mc_news_title" meta:resourcekey="lblPreviewTitle" />
        </div>
        <div class="mc_news_div_close_preview">
            <asp:Button ID="btnClosePreviewNews" runat="server" meta:resourcekey="btnClosePreviewNews"
                CssClass="mc_news_button mc_news_btn_61" />
        </div>
        <iframe id="frameNewsPreview" class="mc_news_frame_preview" frameborder="0"></iframe>
    </div>
</asp:Panel>
<!-- Pop-up Link News -->
<ajaxToolkit:ModalPopupExtender ID="MPELinkedNews" runat="server" BackgroundCssClass="mc_news_popup_background"
    TargetControlID="lbtnAddLinkedNews" PopupControlID="divLinkedNews" CancelControlID="btnCloseLinkedNews"
    DropShadow="false" Y="40" />
<asp:Panel ID="divLinkedNews" runat="server" Style="display: none;" CssClass="mc_news_linked_news_popup">
    <div class="mc_news_div_close_search_news">
        <asp:Button ID="btnCloseLinkedNews" runat="server" meta:resourcekey="btnCloseLinkedNews"
            CssClass="mc_news_button mc_news_btn_61" />
    </div>
    <div style="clear:both;"></div>
    <asp:UpdatePanel ID="upLinkedNews" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <melon:LinkedNews ID="cntrlLinkedNews" runat="server" IsLoadedAsLinkNewsPopUp="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<script type="text/javascript" language="javascript">
    var txtDatePostedID = '<%= txtDatePosted.ClientID %>';
    var ddlHourPostedID = '<%= ddlHourPosted.ClientID %>';
    var ddlMinutesPostedID = '<%= ddlMinutesPosted.ClientID %>';
    var editorID = '<%= HTMLEditor.ClientID %>';
</script>

