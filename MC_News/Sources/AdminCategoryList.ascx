<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminCategoryList.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminCategoryList" %>
<%@ Register TagPrefix="melon" TagName="Languages" Src="AdminLanguages.ascx" %>
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="left">
            <asp:Label ID="lblManageCategories" runat="server" meta:resourcekey="lblManageCategories"
                CssClass="mc_news_title" /></td>
        <td align="right">
            <melon:Languages ID="cntrlLanguages" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2" class="mc_news_categorylist_td">
            <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" GridLines="None"
                CssClass="mc_news_grid" HeaderStyle-CssClass="mc_news_grid_header" RowStyle-CssClass="mc_news_grid_row"
                AlternatingRowStyle-CssClass="mc_news_grid_alt_row">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Id %>">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_categorylist_header_col1" />
                        <ItemStyle CssClass="mc_news_categorylist_col1" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:CategoryName %>">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_categorylist_header_col2" />
                        <ItemStyle CssClass="mc_news_categorylist_col2" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:IsVisible %>">
                        <ItemTemplate>
                            <asp:Label ID="lblIsVisible" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_categorylist_header_col3" />
                        <ItemStyle CssClass="mc_news_categorylist_col3" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:NewsCount %>">
                        <ItemTemplate>
                            <asp:Label ID="lblNewsCount" runat="server" />
                            /
                            <asp:Label ID="lblTotalNewsCount" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_categorylist_header_col4" />
                        <ItemStyle CssClass="mc_news_categorylist_col4" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table cellpadding="0" cellspacing="0" class="mc_news_actions">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDisabledLnkMoveUp" runat="server" meta:resourcekey="lblDisabledLnkMoveUp"
                                            CssClass="mc_news_disabled_lnk" />
                                        <asp:LinkButton ID="lbtnMoveUp" runat="server" meta:resourcekey="lbtnMoveUp" CssClass="mc_news_link_btn"
                                            CausesValidation="false" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDisabledLnkMoveDown" runat="server" meta:resourcekey="lblDisabledLnkMoveDown"
                                            CssClass="mc_news_disabled_lnk" />
                                        <asp:LinkButton ID="lbtnMoveDown" runat="server" meta:resourcekey="lbtnMoveDown"
                                            CssClass="mc_news_link_btn" CausesValidation="false" /></td>
                                    <td>
                                        <asp:LinkButton ID="lbtnEdit" runat="server" meta:resourcekey="lbtnEdit" CssClass="mc_news_link_btn"
                                            CausesValidation="false" /></td>
                                    <td>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" meta:resourcekey="lbtnDelete" CssClass="mc_news_link_btn"
                                            CausesValidation="false" OnClientClick='<%# "return ConfirmAction(\"" + this.GetLocalResourceObject("DeleteCategoryConfirmMessage").ToString() + "\");" %>' /></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <HeaderStyle CssClass="mc_news_categorylist_header_col5" />
                        <ItemStyle CssClass="mc_news_categorylist_header_col5" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Table that appears when grid is empty. -->
            <table id="tblEmptyDataTemplate" runat="server" cellpadding="0" cellspacing="0" class="mc_news_grid_header"
                style="margin-top: 10px;">
                <tr>
                    <th class="mc_news_categorylist_header_col1">
                        <%=GetLocalResourceObject("Id")%>
                    </th>
                    <th class="mc_news_categorylist_header_col2">
                        <%=GetLocalResourceObject("CategoryName")%>
                    </th>
                    <th class="mc_news_categorylist_header_col3">
                        <%=GetLocalResourceObject("IsVisible")%>
                    </th>
                    <th class="mc_news_categorylist_header_col4">
                        <%=GetLocalResourceObject("NewsCount")%>
                    </th>
                    <th class="mc_news_categorylist_header_col5">
                        &nbsp;
                    </th>
                </tr>
            </table>
        </td>
    </tr>
    <!-- Button "Add Category" -->
    <tr>
        <td align="right" style="padding-top: 10px;" colspan="2">
            <asp:Button ID="btnAddCategory" runat="server" CssClass="mc_news_button mc_news_btn_category_add" meta:resourcekey="btnAddCategory"
                CausesValidation="false" />
        </td>
    </tr>
</table>
