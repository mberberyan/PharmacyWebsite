<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Forum.ascx.cs" Inherits="Melon.Components.Forum.UI.CodeBehind.Forum" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="melon" TagName="Breadcrumbs" Src="Breadcrumb.ascx" %>
<%@ Register Assembly="Melon.Components.ForumCore" Namespace="Melon.Components.Forum.UI.Controls" TagPrefix="melon" %>
<%@ Import Namespace="Melon.Components.Forum" %>

<asp:ScriptManager ID="cntrlScriptManager" runat="server">
  <Scripts>
    <asp:ScriptReference  Path="Scripts/forum.js"/>
  </Scripts> 
</asp:ScriptManager>

<nStuff:UpdateHistory runat="server" ID="updateHistory" HistoryPagePath="History.htm" OnNavigate="updateHistory_Navigate" />

<asp:UpdatePanel runat="server" ID="upMain" UpdateMode="Conditional"  >
<ContentTemplate>  
<ajaxToolkit:ModalPopupExtender ID="MPE" runat="server"
    BackgroundCssClass="mc_forum_popup_error_background"
    TargetControlID="btnShowError"
    PopupControlID="PanelError"
    OkControlID="btnOkError"
    DropShadow="false"  />
    
<asp:Button ID="btnShowError" Enabled="false" runat="server" style="display:none;" />
<asp:Panel ID="PanelError" runat="server" CssClass="mc_forum_popup_error" style="display:none;">
    <asp:Label ID="lblErrorTitle" runat="server" CssClass="mc_forum_popup_error_title" meta:resourcekey="lblErrorTitle"/><br />
    <asp:Label ID="lblError" runat="server" /><br /><br />
    <div align="center">
        <asp:Button ID="btnOkError" runat="server" Text="Ok" CssClass="mc_forum_button" Width="45"/></div>
 </asp:Panel>
 
<table cellpadding="0" cellspacing="0" class="mc_forum_maintable">
    <tr>
        <td>
            <div class="mc_forum_navigation">
                <!-- Breadcrumbs -->
                <div style="float: left;">
                    <melon:Breadcrumbs ID="cntrlBreadcrumbs" runat="server" Separator=">>" />
                </div>
                <!-- Forum Menu Actions -->
                <div style="float: right;">
                    <!-- Update Profile Link -->
                    <asp:LinkButton ID="lbtnUpdateProfile" runat="server" CssClass="mc_forum_link" meta:resourcekey="lbtnUpdateProfile"
                        CausesValidation="false" OnClick="lbtnUpdateProfile_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <!-- Search Link-->
                    <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="mc_forum_link" meta:resourcekey="lbtnSearch"
                        CausesValidation="false" OnClick="lbtnSearch_Click" /></div>
            </div>
        </td>
    </tr>
     <!-- Panels for including forum user controls-->
    <tr>
        <td>
           
            <melon:ForumPanel runat="server" ID="panelFirst"/>
        </td>
    </tr>
    <tr>
        <td>
            <melon:ForumPanel runat="server" ID="panelSecond" Style="padding-top: 10px;" />
        </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
