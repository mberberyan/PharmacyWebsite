<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsDetails.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.NewsDetails" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="AdminPager.ascx" %>
<div class="mc_news_fe_news_details">
    <asp:Label ID="lblError" runat="server" />
    <asp:PlaceHolder ID="phNewsDetails" runat="server">
        <div class="posted">
            <asp:Label ID="lblPublishedOnTitle" runat="server" meta:resourcekey="lblPublishedOnTitle" />&nbsp;
            <asp:Label ID="lblDatePublished" runat="server" Text='<%# this.DatePosted.ToString("MM/dd/yyyy, hh:mm") %>'
                CssClass="mc_news_padding3" />&nbsp;
            <asp:Label ID="lblBy" runat="server" meta:resourcekey="lblBy" CssClass="mc_news_padding3" />&nbsp;
            <asp:Label ID="lblAuthor" runat="server" CssClass="mc_news_padding3" />
        </div>
        <div class="category">
            <asp:Label ID="lblCategoryTitle" runat="server" meta:resourcekey="lblCategoryTitle" />&nbsp;
            <asp:Label ID="lblCategoryName" runat="server" CssClass="mc_news_padding3" />
        </div>
        <div class="source">
            <asp:Label ID="lblSourceTitle" runat="server" meta:resourcekey="lblSourceTitle" />&nbsp;
            <asp:Label ID="lblSource" runat="server" CssClass="mc_news_padding3" />
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" />
        </div>
        <div>
            <div class="photo">
                <asp:Image ID="imgPhoto" runat="server" /><br />
                <asp:Label ID="lblPhotoDescription" runat="server" />
            </div>
            <asp:Label ID="lblSubTitle" runat="server" CssClass="subtitle" />
            <div id="divText" runat="server" style="display: inline;" />
        </div>
        <div class="tags">
            <asp:Label ID="lblTagsTitle" runat="server" meta:resourcekey="lblTagsTitle" />&nbsp;
            <asp:Label ID="lblTags" runat="server" CssClass="mc_news_padding3" />
        </div>
        <div class="views">
            <asp:Label ID="lblViewsTitle" runat="server" meta:resourcekey="lblViewsTitle" />&nbsp;
            <asp:Label ID="lblViewsCount" runat="server" CssClass="mc_news_padding3" />
        </div>
    </asp:PlaceHolder>
    <div class="back">
        <asp:HyperLink ID="lnkBack" runat="server" meta:resourcekey="lnkBack" />
    </div>
    <asp:PlaceHolder ID="phLinkedNews" runat="server">
        <div class="linked_news">
            <asp:Repeater ID="repLinkedNews" runat="server">
                <HeaderTemplate>
                    <div class="read_also">
                        <asp:Label ID="lblLinkedNews" runat="server" meta:resourcekey="lblLinkedNews" />
                    </div>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <asp:HyperLink ID="lnkLinkedNews" runat="server" /></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phComments" runat="server">
        <div class="comments">
            <div class="comments_header">
                <div class="title">
                    <asp:Label ID="lblCommentsTitle" runat="server" />
                </div>
                <div class="post">
                    <asp:LinkButton ID="lbtnPostComment" runat="server" meta:resourcekey="lbtnPostComment" />
                </div>
            </div>
            <div id="divCommentForm" runat="server">
                <div class="comment_form">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <th colspan="2">
                                <asp:Label ID="lblCommentFormTitle" runat="server" meta:resourcekey="lblCommentFormTitle" />
                            </th>
                        </tr>
                        <tr id="trCommentAuthorDetails" runat="server">
                            <td valign="top">
                                <asp:Label ID="lblCommentAuthor" runat="server" meta:resourcekey="lblCommentAuthor" />
                                <span class="mc_news_validator">*</span></td>
                            <td>
                                <asp:TextBox ID="txtCommentAuthor" runat="server" MaxLength="256" CssClass="input"
                                    Width="360" /><br />
                                <asp:RequiredFieldValidator ID="rfvCommentAuthor" runat="server" ControlToValidate="txtCommentAuthor"
                                    ValidationGroup="CommentPost" Display="Dynamic" CssClass="mc_news_validator"
                                    meta:resourcekey="rfvCommentAuthor" /></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblCommentText" runat="server" meta:resourcekey="lblCommentText" />
                                <span class="mc_news_validator">*</span></td>
                            <td>
                                <asp:TextBox ID="txtCommmentText" runat="server" CssClass="input" TextMode="MultiLine"
                                    Rows="5" Width="360" /><br />
                                <asp:Label ID="lblMaxCommentSize" runat="server" meta:resourcekey="lblMaxCommentSize"
                                    CssClass="mc_news_comment" /><br />
                                <asp:RequiredFieldValidator ID="rfvCommentText" runat="server" ControlToValidate="txtCommmentText"
                                    ValidationGroup="CommentPost" Display="Dynamic" CssClass="mc_news_validator"
                                    meta:resourcekey="rfvCommentText" />
                                <asp:RegularExpressionValidator ID="revCommentText" runat="server" ControlToValidate="txtCommmentText"
                                    ValidationExpression="(.|\n){0,350}" ValidationGroup="CommentPost" Display="Dynamic"
                                    CssClass="mc_news_validator" meta:resourcekey="revCommentText" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSaveComment" runat="server" meta:resourcekey="btnSaveComment"
                                    CausesValidation="true" ValidationGroup="CommentPost" CssClass="btn" />&nbsp;
                                <asp:Button ID="btnCancelComment" runat="server" meta:resourcekey="btnCancelComment"
                                    CssClass="btn" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblPostCommentError" runat="server" CssClass="mc_news_error_message" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:Label ID="lblPostCommentNeedApprovement" runat="server" CssClass="mc_news_message"
                meta:resourcekey="lblPostCommentNeedApprovement" />
            <melon:Pager ID="topPager" runat="server" ShowItemsDetails="false" CssClass="mc_news_pager" />
            <asp:GridView ID="gvComment" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                AllowPaging="true" PagerSettings-Visible="false" GridLines="None" Width="100%"
                CssClass="comments_grid">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Image ID="imgAuthorPhoto" runat="server" />
                            <br />
                            <asp:Label ID="lblAuthorNickname" runat="server" />
                            <br />
                            <asp:Label ID="lblRegisteredOnTitle" runat="server" meta:resourcekey="lblRegisteredOnTitle" />
                            <asp:Label ID="lblRegisteredOn" runat="server" Text='<%#Eval("AuthorRegistrationDate","{0:MM/dd/yyyy, hh:mm}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="posted_on">
                                <asp:Label ID="lblCommentPostedOnTitle" runat="server" meta:resourcekey="lblCommentPostedOnTitle" />
                                <asp:Label ID="lblDatePosted" runat="server" Text='<%#Eval("DatePosted","{0:MM/dd/yyyy, hh:mm}") %>' />
                            </div>
                            <div id="divCommentText" runat="server" class="comment_text" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:PlaceHolder>
</div>
