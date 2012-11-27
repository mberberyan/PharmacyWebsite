<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumPostList.ascx.cs"  Inherits="Melon.Components.Forum.UI.CodeBehind.ForumPostList" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.Forum" %>

<div class="mc_forum_message_box"></div>       
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <melon:Pager ID="TopPager" runat="server" CssClass="mc_forum_pager" ShowItemsDetails="false"/>
            <asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="False" GridLines="None" 
                CssClass="mc_forum_grid" ShowHeader="false" AllowPaging="true" PagerSettings-Visible="false">
                <Columns>
                    <asp:TemplateField ItemStyle-CssClass="mc_forum_grid_col mc_forum_postlist_col1">
                        <ItemTemplate>
                            
                            <asp:Label ID="lblAnonymousUser" runat="server" meta:resourcekey="lblAnonymousUser" />
                            
                            <!-- Author Details (displayed in case the post is not from anonymous user)-->
                            <div id="divAuthorDetails" runat="server">
                                <asp:Image ID="imgPostAuthor" runat="server" meta:resourcekey="imgPostAuthor"
                                ImageUrl='<%#((Eval("AuthorPhotoPath") != DBNull.Value) && (Eval("AuthorPhotoPath").ToString() != ""))? Eval("AuthorPhotoPath").ToString(): Utilities.GetImageUrl(this.Page,"ForumStyles/Images/snimka.gif") %>' /><br />
                                
                                <!--Author Nickname: It is displayed in label or linkbutton - This depends on whether the profile of the user is visible -->
                                <melon:ForumUserNicknameDisplay ID="cntrlForumUserNickname" runat="server"
                                AnonymousUserText='<%$ Resources:Anonymous %>' 
                                TextCssClass="mc_forum_text"
                                LinkCssClass="mc_forum_link"
                                LinkTooltip='<%$ Resources:lbtnProfileDetails.Tooltip %>' />
                              
                                <!-- Author Top Role-->
                                <asp:Label ID="lblPostAuthorRole" runat="server"/>
                                <br /><br />
                                <!-- Author Posts-->
                                <asp:Label ID="lblPostsTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblPostsTitle"/>
                                <asp:Label ID="lblAuthorPostsCount" runat="server"/>
                                <br />
                                <!-- Author Registration Date -->
                                <asp:Label ID="lblRegistratedOnTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblRegistratedOnTitle"/>
                                <%# (Eval("AuthorCreationDate") != DBNull.Value) ? Eval("AuthorCreationDate", "{0:ddd MMM dd, yyyy, hh:mm}") : ""%> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField ItemStyle-CssClass="mc_forum_grid_col mc_forum_postlist_col2">
                        <ItemTemplate>
                             <!-- Post Details -->
                            <div class="mc_forum_divPostDate">
                                <asp:Label ID="lblPostedOn" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblPostedOn"/>
                                <a id="aPostPosition" runat="server" class="mc_forum_link_nounderline">&nbsp;</a>
                                &nbsp;
                                <%# Eval("DateCreated", "{0:ddd MMM dd, yyyy, hh:mm}") %>
                            </div>
                            <div class="mc_forum_divPost">
                                <asp:Label ID="lblPostText" runat="server"/>
                            </div>
                            <div style="float:right">
                                <asp:LinkButton ID="lbtnEditForumPost" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnEditForumPost"
                                    CausesValidation="false"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnDeleteForumPost" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnDeleteForumPost"
                                    OnClientClick='<%# "return confirm(\"" + this.GetLocalResourceObject("ConfirmMessageForumPostDelete").ToString() + "\")"%>'
                                    CausesValidation="false"/>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
					<%# this.GetLocalResourceObject("NoPostsEntered").ToString()%>
                </EmptyDataTemplate>
            </asp:GridView>
            <melon:Pager ID="BottomPager" runat="server" CssClass="mc_forum_pager" ShowItemsDetails="false"/>
        </td>
    </tr>
    <tr>
        <td class="mc_forum_tdCreateForumPostButton">
            <asp:Button ID="btnCreateForumPost" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnCreateForumPost" 
                CausesValidation="false"/>
        </td>
    </tr>
</table>

