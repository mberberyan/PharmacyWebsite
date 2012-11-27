<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsList.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.NewsList" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="AdminPager.ascx" %>
<%@ Import Namespace="Melon.Components.News.Configuration" %>

<div class="mc_news_fe_newslist" >
    <melon:Pager ID="topPager" runat="server" ShowItemsDetails="false" Visible="false" CssClass="mc_news_pager"/>
    <asp:GridView ID="gvNews" runat="server" AutoGenerateColumns="false" GridLines="None"
        ShowHeader="false" CssClass="news_list" CellPadding="0" CellSpacing="0">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class='<%# Convert.ToBoolean(Eval("IsFeatured"))?"featured_news":"not_featured_news" %>'>
                        <div class="posted">
                            <asp:Label ID="lblPublishedOnTitle" runat="server" meta:resourcekey="lblPublishedOnTitle"/>
                            <%#Eval("DatePosted","{0:MM/dd/yyyy, hh:mm}") %>
                            <asp:Label ID="lblBy" runat="server" meta:resourcekey="lblBy"/>
                            <%#Server.HtmlEncode(Eval("Author").ToString())%>
                        </div>
                        <div class="title">
                            <asp:HyperLink ID="hplTitle" runat="server" Text='<%#Server.HtmlEncode(Eval("Title").ToString()) %>' 
                            NavigateUrl='<%# NewsSettings.NewsDetailsPagePath + ((this.CategoryId == null) ? ("?news_id=" + Eval("Id")) : ("?cat_id=" + Convert.ToString(this.CategoryId)+ "&news_id=" + Eval("Id")) ) %>'/>
                             
                        </div>
                        <div class="summary">
                             <%#Eval("Summary") %>
                        </div>
                        <asp:HyperLink ID="hplReadMore" runat="server" meta:resourcekey="hplReadMore" CssClass="more"
                            NavigateUrl='<%# NewsSettings.NewsDetailsPagePath + ((this.CategoryId == null) ? ("?news_id=" + Eval("Id")) : ("?cat_id=" + Convert.ToString(this.CategoryId)+ "&news_id=" + Eval("Id")) ) %>'/>
                        <div class="clear"></div>    
                    </div>
                </ItemTemplate>
                <ItemStyle CssClass="news"/>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <melon:Pager ID="bottomPager" runat="server" ShowItemsDetails="false" Visible="false" CssClass="mc_news_pager"/>
</div>