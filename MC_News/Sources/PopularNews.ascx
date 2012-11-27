<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PopularNews.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.PopularNews" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="AdminPager.ascx" %>
<%@ Import Namespace="Melon.Components.News.Configuration" %>

<div class="mc_news_fe_popular_news" >
    <melon:Pager ID="topPager" runat="server" ShowItemsDetails="false" Visible="false" CssClass="mc_news_pager"/>
    <asp:GridView ID="gvNews" runat="server" AutoGenerateColumns="false" GridLines="None"
        ShowHeader="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <span class="views">
                        <asp:Label ID="lblViews" runat="server" meta:resourcekey="lblViews"/>:
                        <%#Eval("ViewsCount") %>
                    </span><br />
                    <asp:HyperLink ID="hplReadMore" runat="server" CssClass="title" Text='<%#Server.HtmlEncode(Eval("Title").ToString()) %>'
                        NavigateUrl='<%# NewsSettings.NewsDetailsPagePath + "?cat_id=" + ((Eval("CategoryId")==DBNull.Value)?"-1" : Eval("CategoryId") )+ "&news_id=" + Eval("Id") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <melon:Pager ID="bottomPager" runat="server" ShowItemsDetails="false" Visible="false" CssClass="mc_news_pager"/>
</div>