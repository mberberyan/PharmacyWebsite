<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumTopicList.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.ForumTopicList" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.Forum" %>

<div class="mc_forum_message_box">
    <asp:Label ID="lblMessage" runat="server" CssClass="mc_forum_message" Visible="false" />
</div>
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <melon:Pager ID="TopPager" runat="server" CssClass="mc_forum_pager" ShowItemsDetails="false"/>
            <asp:GridView ID="gvForumTopics" runat="server" AutoGenerateColumns="False" GridLines="None"  ShowHeader="true"
                CssClass="mc_forum_grid" HeaderStyle-CssClass="mc_forum_grid_header" RowStyle-CssClass="mc_forum_grid_col"
                AllowPaging="true" PagerSettings-Visible="false">
                <Columns>
                    <asp:TemplateField  HeaderStyle-CssClass="mc_forum_topiclist_header_col1" ItemStyle-CssClass="mc_forum_topiclist_col1">
                        <ItemTemplate>
                            <asp:Image ID="imgTopicTypeIcon" runat="server" BorderWidth="0" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:TopicHeader %>" HeaderStyle-CssClass="mc_forum_topiclist_header_col2"
                        ItemStyle-CssClass="mc_forum_topiclist_col2">
                        <ItemTemplate>
                            <!-- Topic Details -->
                            <melon:MelonLinkButton ID="lbtnOpenForumTopic" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnOpenForumTopic" 
                                CausesValidation="false" />
                            <asp:Image ID="imgClosed" runat="server" meta:resourcekey="imgClosed"
                                ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/closed.gif") %>' ImageAlign="AbsMiddle" />  
                            <asp:Image ID="imgInActive" runat="server" meta:resourcekey="imgInActive"
                                ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/inactive.gif") %>' ImageAlign="AbsMiddle" />    
                            <div>
                                <asp:Label ID="lblTopicDescription" runat="server"/>
                            </div>
                            
                              <!-- Button Area (Edit/Delete)-->
                            <div style="float:right;">
                                <asp:LinkButton ID="lbtnEditForumTopic" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnEditForumTopic"
                                    CausesValidation="false"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnDeleteForumTopic" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnDeleteForumTopic"
                                    OnClientClick='<%# "return confirm(\"" + this.GetLocalResourceObject("ConfirmMessageForumTopicDelete").ToString() + "\")"%>'
                                    CausesValidation="false"/>
                            </div>
                        
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:AuthorHeader %>" HeaderStyle-CssClass="mc_forum_topiclist_header_col3"
                        ItemStyle-CssClass="mc_forum_grid_col_alternative mc_forum_topiclist_col3">
                        <ItemTemplate>
        
                            <!--Topic Author Nickname: It is displayed in label or linkbutton - This depends on whether the profile of the user is visible -->
                            <melon:ForumUserNicknameDisplay ID="cntrlTopicAuthorNickname" runat="server"
                            AnonymousUserText='<%$ Resources:Anonymous %>' 
                            TextCssClass="mc_forum_text"
                            LinkCssClass="mc_forum_link"
                            LinkTooltip='<%$ Resources:lbtnProfileDetails.Tooltip %>' />
                            <br />
                                
                            <!-- Topic Author Top Role -->
                            <asp:Label ID="lblTopicAuthorRole" runat="server"/>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:ViewsHeader %>" HeaderStyle-CssClass="mc_forum_topiclist_header_col4"
                        ItemStyle-CssClass="mc_forum_grid_col_alternative mc_forum_topiclist_col4">
                        <ItemTemplate>
                            <asp:Label ID="lblTopicViewsCount" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:PostsHeader %>" HeaderStyle-CssClass="mc_forum_topiclist_header_col5"
                        ItemStyle-CssClass="mc_forum_grid_col_alternative mc_forum_topiclist_col5">
                        <ItemTemplate>
                            <asp:Label ID="lblTopicPostsCount" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:LastPostHeader %>" HeaderStyle-CssClass="mc_forum_topiclist_header_col6"
                        ItemStyle-CssClass="mc_forum_grid_col_alternative mc_forum_topiclist_col6">
                        <ItemTemplate>
                            <!-- Last Post Details-->
                            <div id="divLastPost" runat="server">
                                <!-- Last Post Date -->
                                <%# (Eval("LastPostDateCreated") != DBNull.Value) ? Eval("LastPostDateCreated", "{0:ddd MMM dd, yyyy, hh:mm}") : ""%>
                                  
                                 <!-- Button: Go to last post-->
                                <asp:ImageButton ID="ibtnOpenPost" runat="server" meta:resourcekey="ibtnOpenPost"
                                    ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/icon_latest_post.gif") %>' ImageAlign="Right" 
                                    CausesValidation="false"/>
                                <br />
                                
                                <!-- Last Post Author-->                            
                                <asp:Label ID="lblAuthorTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblAuthorTitle" />&nbsp;&nbsp;
                         
                                <!--Last Post Author Nickname: It is displayed in label or linkbutton - This depends on whether the profile of the user is visible -->
                                <melon:ForumUserNicknameDisplay ID="cntrlLastPostAuthorNickname" runat="server"
                                    AnonymousUserText='<%$ Resources:Anonymous %>' 
                                    TextCssClass="mc_forum_text"
                                    LinkCssClass="mc_forum_link"
                                    LinkTooltip='<%$ Resources:lbtnProfileDetails.Tooltip %>' />
                                    <br />
                                
                                <!-- Last Post Author Top Role-->
                                <asp:Label ID="lblLastPostAuthorRole" runat="server"/>
                              
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table id="tblEmptyDataTemplate" runat="server" cellpadding="0" cellspacing="0" class="mc_forum_grid mc_forum_grid_header">
                <tr>
                    <th class="mc_forum_topiclist_header_col1"></th>
                    <th class="mc_forum_topiclist_header_col2"><%=GetLocalResourceObject("TopicHeader").ToString()%></th>
                    <th class="mc_forum_topiclist_header_col3"><%=GetLocalResourceObject("AuthorHeader")%></th>
                    <th class="mc_forum_topiclist_header_col4"><%=GetLocalResourceObject("ViewsHeader")%></th>
                    <th class="mc_forum_topiclist_header_col5"><%=GetLocalResourceObject("PostsHeader")%></th>
                    <th class="mc_forum_topiclist_header_col6"><%=GetLocalResourceObject("LastPostHeader")%></th>
                </tr>
            </table>
            <melon:Pager ID="BottomPager" runat="server" CssClass="mc_forum_pager" ShowItemsDetails="false"/>
        </td>
    </tr>
    <tr>
        <td class="mc_forum_tdCreateTopicButton">
            <asp:Button ID="btnCreateForumTopic" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnCreateForumTopic" 
                CausesValidation="false"/>
        </td>
    </tr>
</table>
