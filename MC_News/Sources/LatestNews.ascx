<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LatestNews.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.LatestNews" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="AdminPager.ascx" %>
<%@ Import Namespace="Melon.Components.News.Configuration" %>

<div class="mc_news_fe_latest_news" >
    <melon:Pager ID="topPager" runat="server" ShowItemsDetails="false" Visible="false" CssClass="mc_news_pager"/>
    <asp:GridView ID="gvNews" runat="server" AutoGenerateColumns="false" GridLines="None"
        ShowHeader="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <span class="date_posted"><%#Eval("DatePosted","{0:MM/dd/yyyy, hh:mm}") %></span><br />
                    <asp:HyperLink ID="hplReadMore" runat="server" CssClass="title" Text='<%#Server.HtmlEncode(Eval("Title").ToString()) %>'
                        NavigateUrl='<%# NewsSettings.NewsDetailsPagePath + "?cat_id=" + ((Eval("CategoryId")==DBNull.Value)?"-1" : Eval("CategoryId") )+ "&news_id=" + Eval("Id") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <melon:Pager ID="bottomPager" runat="server" ShowItemsDetails="false" Visible="false" CssClass="mc_news_pager"/>
</div>