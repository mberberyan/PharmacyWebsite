<%@ Control Language="C#" AutoEventWireup="true" Inherits="Explorer" CodeFile="Explorer.ascx.cs" %>
<%@ Import Namespace="Melon.Components.CMS" %>
<div class="mc_cms_explorer">
    <!-- CMS Explorer Actions -->
    <div class="mc_cms_explorer_actions">
        <div>
            <!-- Create Actions -->
            <div style="float: left;">
                <asp:ImageButton ID="ibtnCreateFolder" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/create_folder.gif")%>'
                    meta:resourcekey="ibtnCreateFolder" CausesValidation="false" />&nbsp;
                <asp:ImageButton ID="ibtnCreateStaticPage" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/create_static_page.gif")%>'
                    meta:resourcekey="ibtnCreateStaticPage" CausesValidation="false" />&nbsp
                <asp:ImageButton ID="ibtnCreateContentPage" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/create_content_page.gif")%>'
                    meta:resourcekey="ibtnCreateContentPage" CausesValidation="false" />&nbsp;
                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/delete.gif")%>'
                    meta:resourcekey="ibtnDelete" CausesValidation="false" OnClientClick='<%# "return OnDeleteNodeClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteNode").ToString() + "\");" %>' />
            </div>
            <!-- Actions Delimiter -->
            <div class="mc_cms_action_delimiter">
            </div>
            <!-- Move Actions, Delete Action -->
            <div class="mc_cms_explorer_actions_move">
                <asp:ImageButton ID="ibtnMoveUp" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/move_up.gif")%>'
                    meta:resourcekey="ibtnMoveUp" CausesValidation="false" />
                <asp:ImageButton ID="ibtnMoveDown" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/move_down.gif")%>'
                    meta:resourcekey="ibtnMoveDown" CausesValidation="false" />&nbsp;
                <asp:ImageButton ID="ibtnMoveLeft" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/move_left.gif")%>'
                    meta:resourcekey="ibtnMoveLeft" CausesValidation="false" />
                <asp:ImageButton ID="ibtnMoveRight" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/move_right.gif")%>'
                    meta:resourcekey="ibtnMoveRight" CausesValidation="false" />
            </div>
            <!-- Actions Delimiter -->
            <div class="mc_cms_action_delimiter">
            </div>
            <!-- Publish Action -->
            <div style="float: left;">
                <asp:ImageButton ID="ibtnRecursivePublish" runat="server" ImageUrl='<%#Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/publish.gif")%>'
                    meta:resourcekey="ibtnPublish" CausesValidation="false" OnClientClick='<%# "return OnRecursivePublishClientClick(\"" + this.GetLocalResourceObject("ConfirmMessagePublishFilteredNodes").ToString() + "\");"%>' />
            </div>
        </div>
        <div class="mc_cms_clear">
        </div>
    </div>
    <!-- Filter by visibility -->
    <div class="mc_cms_explorer_filter">
        <asp:Label ID="lblFilterTitle" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblFilterTitle" />
        <asp:DropDownList ID="ddlFilter" runat="server" DataTextField="Name" DataValueField="Code"
            CssClass="mc_cms_input_short" Width="170" AutoPostBack="true" />
    </div>
    <!-- CMS Explorer -->
    <asp:TreeView ID="tvCMSExplorer" runat="server" ExpandDepth="1" ShowLines="true"
        SkipLinkText="" CssClass="mc_cms_tree" NodeStyle-CssClass="mc_cms_node" SelectedNodeStyle-CssClass="mc_cms_selected_node" />
    <asp:HiddenField ID="hfExpandedNodes" runat="server" />

    <script language="javascript" type="text/javascript">
        var ddlFilter = document.getElementById('<%= ddlFilter.ClientID %>');
        var hfExpandedNodes = document.getElementById('<%= hfExpandedNodes.ClientID %>');

        //Create regular expression pattern for finding link for expand/collapse node.
        var regexExpand = /<%= tvCMSExplorer.ClientID%>n/i;

        var windowLoadEventHandler = window.onload;
        window.onload = function() { if (!!windowLoadEventHandler) windowLoadEventHandler(); InitNodesTreeView(); };
    </script>

</div>
