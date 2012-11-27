<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Categories.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.Categories" %>
<%@ Import Namespace="Melon.Components.News.Configuration" %>
<%@ Import Namespace="System.Data" %>
<div class="mc_news_fe_categories">
    <ul>
        <asp:Repeater ID="repCategories" runat="server">
            <HeaderTemplate>
                <li>
                    <asp:HyperLink ID="hplAllNews" runat="server" meta:resourcekey="hplAllNews"  
                        NavigateUrl='<%# NewsSettings.NewsListPagePath %>'/>
                </li>
            </HeaderTemplate>
            <ItemTemplate>
                <li><asp:HyperLink ID="hplCategory" runat="server"/></li>
            </ItemTemplate> 
          
        </asp:Repeater>
       
     </ul>
</div>