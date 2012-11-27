<%@ Control Language="C#" AutoEventWireup="true"  CodeFile="ForumList.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.ForumList" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Import Namespace="Melon.Components.Forum" %>

<div class="mc_forum_message_box">
    <asp:Label ID="lblMessage" runat="server" CssClass="mc_forum_message" Visible="false" />
</div>
<asp:Repeater ID="repForumGroups" runat="server">
    <HeaderTemplate>
        <table cellpadding="0" cellspacing="0" class="mc_forum_grid">
            <tr class="mc_forum_grid_header">
                <!--Header -->
                <th class="mc_forum_forumlist_header_col1">
                    <asp:Label ID="lblForumTitle" runat="server" meta:resourcekey="lblForumTitle" /></th>
                <th class="mc_forum_forumlist_header_col2">
                    <asp:Label ID="lblTopicsTitle" runat="server" meta:resourcekey="lblTopicsTitle" /></th>
                <th class="mc_forum_forumlist_header_col3">
                    <asp:Label ID="lblPostsTitle" runat="server" meta:resourcekey="lblPostsTitle" /></th>
                <th class="mc_forum_forumlist_header_col4">
                    <asp:Label ID="lblLastPostTitle" runat="server" meta:resourcekey="lblLastPostTitle" /></th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="mc_forum_grid_sub_header">
            <th colspan="4">
                <!--Forum Group Details-->
                <div class="mc_forum_divForumGroupTitle">
                    <asp:Label ID="lblForumGroupName" runat="server"/>
                    <asp:Image ID="imgIsActiveGroup" runat="server" meta:resourcekey="imgIsActiveGroup"
                        ImageUrl='<%# Utilities.GetImageUrl(this.Page, "ForumStyles/Images/inactive.gif") %>' ImageAlign="AbsMiddle"  />         
                    
                </div>
                <!--Action Buttons for Forum Group -->
                <div id="divForumGroupAdminButtons" class="mc_forum_divForumGroupAdminButtons" runat="server">
                    <asp:ImageButton ID="ibtnEditForumGroup" runat="server" meta:resourcekey="ibtnEditForumGroup"
                       ImageUrl='<%# Utilities.GetImageUrl(this.Page, "ForumStyles/Images/edit.gif") %>' ImageAlign="Middle" 
                       CausesValidation="false"/>
                       
                    <asp:ImageButton ID="ibtnDeleteForumGroup" runat="server" meta:resourcekey="ibtnDeleteForumGroup" 
                       ImageUrl='<%# Utilities.GetImageUrl(this.Page, "ForumStyles/Images/delete.gif") %>' ImageAlign="Middle"  
                       OnClientClick='<%# "return confirm(\"" + this.GetLocalResourceObject("ConfirmMessageForumGroupDelete").ToString() + "\")"%>'
                       CausesValidation="false"/>
                       
                    <asp:ImageButton ID="ibtnMoveUpForumGroup" runat="server" meta:resourcekey="ibtnMoveUpForumGroup"
                       ImageUrl='<%# Utilities.GetImageUrl(this.Page, "ForumStyles/Images/move_up.gif") %>' ImageAlign="Middle"   
                       CausesValidation="false"/>
                       
                    <asp:ImageButton ID="ibtnMoveDownForumGroup" runat="server" meta:resourcekey="ibtnMoveDownForumGroup" 
                       ImageUrl='<%# Utilities.GetImageUrl(this.Page, "ForumStyles/Images/move_down.gif") %>' ImageAlign="Middle"   
                       CausesValidation="false"/>
                       
                    &nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnCreateForum" runat="server" meta:resourcekey="ibtnCreateForum"
                        ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/add.gif") %>' ImageAlign="Middle"   
                        CausesValidation="false"/>
                   
                </div>
            </th>
        </tr>
        <asp:Repeater ID="repForums" runat="server">
            <ItemTemplate>
                <tr id="trForumDetails" runat="server">
                    <td class="mc_forum_grid_col mc_forum_forumlist_col1">
                        <!-- Forum Details (Name, Description)-->
                        
                        <melon:MelonLinkButton ID="lbtnOpenForum" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnOpenForum"/>
                        <asp:Image ID="imgClosed" runat="server" meta:resourcekey="imgClosed"
                            ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/closed.gif") %>' ImageAlign="AbsMiddle" />
                        <asp:Image ID="imgIsActiveForum" runat="server" meta:resourcekey="imgIsActiveForum" 
                            ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/inactive.gif") %>' ImageAlign="AbsMiddle" />              
                        
                        <div>
                            <asp:Label ID="lblForumDescription" runat="server"/>
                        </div>
                        
                        <!-- Button Area (Edit/Delete Forum)-->
                        <div id="divForumButtons" runat="server" style="float:right" >
                            <asp:LinkButton ID="lbtnEditForum" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnEditForum" 
                                CausesValidation="false" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lbtnDeleteForum" runat="server" CssClass="mc_forum_bold_link" meta:resourcekey="lbtnDeleteForum" 
                                OnClientClick='<%# "return confirm(\"" + this.GetLocalResourceObject("ConfirmMessageForumDelete").ToString() + "\")"%>'
                                CausesValidation="false"/>
                        </div>
                    </td>
                    <td class="mc_forum_grid_col_alternative mc_forum_forumlist_col2">
                        <asp:Label ID="lblTopicsCount" runat="server"/>
                    </td>
                    <td class="mc_forum_grid_col_alternative mc_forum_forumlist_col3">
                        <asp:Label ID="lblPostsCount" runat="server"/>
                    </td>
                    <td class="mc_forum_grid_col_alternative mc_forum_forumlist_col4">
                        <!-- Last Post Details-->
                        <div id="divLastPost" runat="server">

                            <!-- Last Post Date -->
                            <%# (Eval("LastPostDateCreated") != null) ? Eval("LastPostDateCreated", "{0:ddd MMM dd, yyyy, hh:mm}") : ""%>
                             <!-- Button: Go to last post-->
                            <asp:ImageButton ID="ibtnOpenPost" runat="server" meta:resourcekey="ibtnOpenPost"
                                ImageUrl='<%# Utilities.GetImageUrl(this.Page,"ForumStyles/Images/icon_latest_post.gif") %>' ImageAlign="Right"
                                CausesValidation="false"/><br />   
                            <!-- Last Post Author-->                         
                            <asp:Label ID="lblAuthorTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblAuthorTitle" />&nbsp;&nbsp;
                            
                            <!-- Author Nickname: It is displayed in label or linkbutton - This depends on whether the profile of the user is visible -->
                            <melon:ForumUserNicknameDisplay ID="cntrlForumUserNickname" runat="server"
                                AnonymousUserText='<%$ Resources:Anonymous %>' 
                                TextCssClass="mc_forum_text"
                                LinkCssClass="mc_forum_link"
                                LinkTooltip='<%$ Resources:lbtnProfileDetails.Tooltip %>' />
                            
                            <!-- Author Role -->
                            <asp:Label ID="lblAuthorRole" runat="server"/>

                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </ItemTemplate>
    <FooterTemplate>
        <tr>
            <td colspan="4" class="mc_forum_tdCreateForumGroupButton">
                <!--Button Add Forum Group -->
                <asp:Button ID="btnCreateForumGroup" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnCreateForumGroup" />
            </td>
        </tr>
        </table>
    </FooterTemplate>
</asp:Repeater>

