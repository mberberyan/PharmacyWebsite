<%@ Control Language="C#" AutoEventWireup="true" Inherits="TemplateList" CodeFile="TemplateList.ascx.cs" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.CMS" %>

<asp:Label ID="lblManageTemplates" runat="server" meta:resourcekey="lblManageTemplates"
    CssClass="mc_cms_title" />
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>
            <!-- Pager for the grid view with templates-->
            <melon:Pager ID="TopPager" runat="server" CssClass="mc_cms_pager" ShowItemsDetails="false" />
            <!-- *** Grid with templates *** -->
            <asp:GridView ID="gvTemplates" runat="server" AutoGenerateColumns="False" GridLines="None"
                ShowHeader="true" CssClass="mc_cms_grid" HeaderStyle-CssClass="mc_cms_grid_header"
                RowStyle-CssClass="mc_cms_grid_row" AlternatingRowStyle-CssClass="mc_cms_grid_alt_row"
                AllowPaging="true" PagerSettings-Visible="false" AllowSorting="true">
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="mc_cms_templatelist_header_col1" ItemStyle-CssClass="mc_cms_templatelist_col1">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortByName" runat="server" Text="<%$ Resources:Name %>" CommandName="Sort"
                                CommandArgument="Name" CssClass="mc_cms_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Name" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="mc_cms_templatelist_header_col2" ItemStyle-CssClass="mc_cms_templatelist_col2">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortByMasterPage" runat="server" Text="<%$ Resources:MasterPage %>"
                                CommandName="Sort" CommandArgument="MasterPage" CssClass="mc_cms_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionMasterPage" runat="server" ImageAlign="AbsMiddle"
                                ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="MasterPage" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMasterPage" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$Resources:Placeholders %>' HeaderStyle-CssClass="mc_cms_templatelist_header_col3"
                        ItemStyle-CssClass="mc_cms_templatelist_col3">
                        <ItemTemplate>
                            <asp:Repeater ID="repPlaceholders" runat="server">
                                <HeaderTemplate>
                                    <ul style="margin: 0px; padding: 0 0 0 20px;">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <asp:Label ID="lblPlaceholderName" runat="server" />
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="mc_cms_templatelist_header_col4" ItemStyle-CssClass="mc_cms_templatelist_col4">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lbtnSortByPagesCount" runat="server" Text="<%$ Resources:UsedBy %>"
                                CommandName="Sort" CommandArgument="PagesCount" CssClass="mc_cms_header_link" />
                            <!-- Image which is visible only in the column currently sorted. It is arrow down or arrow up and display the current sorting order. -->
                            <asp:Image ID="imgSortDirectionPagesCount" runat="server" ImageAlign="AbsMiddle"
                                ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_up.gif"):Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/arrow_down.gif") %>'
                                ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="PagesCount" %>' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPagesCount" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="mc_cms_templatelist_header_col5" ItemStyle-CssClass="mc_cms_templatelist_col5">
                        <ItemTemplate>
                            <span style="padding-left: 10px; padding-right: 10px;">
                                <asp:LinkButton ID="lbtnEditTemplate" runat="server" CssClass="mc_cms_link_btn" meta:resourcekey="lbtnEdit" />
                            </span>
                            <span>
                                <asp:LinkButton ID="lbtnDeleteTemplate" runat="server" CssClass="mc_cms_link_btn"
                                    meta:resourcekey="lbtnDelete" OnClientClick='<%# "return OnTemplateDeleteClientClick(\"" + this.GetLocalResourceObject("DeleteTemplateConfirmMessage").ToString() + "\");" %>' />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Table that appears when grid is empty. -->
            <table id="tblEmptyDataTemplate" runat="server" cellpadding="0" cellspacing="0" class="mc_cms_grid mc_cms_grid_header"
                style="margin-top: 10px;">
                <tr>
                    <th class="mc_cms_templatelist_header_col1">
                        <%=GetLocalResourceObject("Name")%>
                    </th>
                    <th class="mc_cms_templatelist_header_col2">
                        <%=GetLocalResourceObject("MasterPage")%>
                    </th>
                    <th class="mc_cms_templatelist_header_col3">
                        <%=GetLocalResourceObject("Placeholders")%>
                    </th>
                    <th class="mc_cms_templatelist_header_col4">
                        <%=GetLocalResourceObject("UsedBy")%>
                    </th>
                    <th class="mc_cms_templatelist_header_col5">
                    </th>
                </tr>
            </table>
        </td>
    </tr>
    <!-- Button "Create Template" -->
    <tr>
        <td align="left" style="padding-top: 10px;">
            <asp:Button ID="btnCreateTemplate" runat="server" CssClass="mc_cms_button mc_cms_btn_create_template"
                meta:resourcekey="btnCreateTemplate" CausesValidation="false" />
        </td>
    </tr>
</table>
