<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ForumSearchResults.ascx.cs"  Inherits="Melon.Components.Forum.UI.CodeBehind.ForumSearchResults" %>
<%@ Register Assembly="Melon.Components.Forum" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.Forum" %>

<!-- Search Results -->
<melon:Pager ID="TopPager" runat="server" CssClass="mc_forum_pager" ShowItemsDetails="false"/>
<asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="false" GridLines="None" meta:resourcekey="gvResults" 
     CssClass="mc_forum_grid mc_forum_search_results" 
     HeaderStyle-CssClass="mc_forum_grid_header" 
     RowStyle-CssClass="mc_forum_grid_col" 
     AlternatingRowStyle-CssClass="mc_forum_grid_col_alternative"
     AllowPaging="true" PagerSettings-Visible="false" >
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lblSearchResultsTitle" runat="server"  meta:resourcekey="lblSearchResultsTitle" />
            </HeaderTemplate>
            <ItemTemplate>
                <!-- Forum and Topic of the found post-->
                <asp:Label ID="lblForumTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblForumTitle"/>
                <melon:MelonLinkButton ID="lbtnOpenForum" runat="server" CssClass="mc_forum_link" meta:resourcekey="lbtnOpenForum"
                    CausesValidation="false" />
                &nbsp;
                <asp:Label ID="lblTopicTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblTopicTitle"/>
                <melon:MelonLinkButton ID="lbtnOpenTopic" runat="server" CssClass="mc_forum_link" meta:resourcekey="lbtnOpenTopic"
                    CausesValidation="false"/>
                <br /><br />
                <!-- Post Details -->
                <div class="mc_forum_divPostDate">
                     <asp:Label ID="lblPostedOnTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblPostedOnTitle"/>
                     <%# Eval("DateCreated", "{0:ddd MMM dd, yyyy, hh:mm}") %>
                     &nbsp;&nbsp;
                     
                     <asp:Label ID="lblAuthorTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblAuthorTitle"/> 
                     <!--Author Nickname: It is displayed in label or linkbutton - This depends on whether the profile of the user is visible -->
                     <melon:ForumUserNicknameDisplay ID="cntrlForumUserNickname" runat="server"
                        AnonymousUserText='<%$ Resources:Anonymous %>' 
                        TextCssClass="mc_forum_text"
                        LinkCssClass="mc_forum_link"
                        LinkTooltip='<%$ Resources:lbtnProfileDetails.Tooltip %>' />
                </div>
                <div>
                    <asp:Label ID="lblPostText" runat="server"/>
                </div>
           </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<melon:Pager ID="BottomPager" runat="server" CssClass="mc_forum_pager" ShowItemsDetails="false"/>

