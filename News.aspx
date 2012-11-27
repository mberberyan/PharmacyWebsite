<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MPNews.master" CodeFile="News.aspx.cs" Inherits="News" meta:resourcekey="Page" Theme="Default"%>
<%@ Reference VirtualPath="~/Controls/Languages.ascx" %>
<%@ Register TagPrefix="melon" TagName="NewsList" Src="~/MC_News/Sources/NewsList.ascx" %>
<%@ Register TagPrefix="melon" TagName="FeaturedNews" Src="~/MC_News/Sources/FeaturedNews.ascx" %>
<%@ Register TagPrefix="melon" TagName="LatestNews" Src="~/MC_News/Sources/LatestNews.ascx" %>
<%@ Register TagPrefix="melon" TagName="PopularNews" Src="~/MC_News/Sources/PopularNews.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphNews">
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top">
                <melon:NewsList ID="cntrlNewsList" runat="server" AllowPaging="true" PageSize="10"/></td>
            <td class="mc_news_fe_news_boxes">
               
                <div class="latest_news">
                    <div class="header">
                        <asp:Label ID="lblLatestNewsTitle" runat="server" meta:resourcekey="lblLatestNewsTitle"/>
                    </div>
                    <melon:LatestNews ID="cntrlLatestNews" runat="server" NewsCount="5" />
                </div>
                <div class="popular_news">
                    <div class="header">
                        <asp:Label ID="lblPopularNewsTitle" runat="server" meta:resourcekey="lblPopularNewsTitle"/>
                    </div>
                    <melon:PopularNews ID="cntrlPopularNews" runat="server" NewsCount="5"/>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>