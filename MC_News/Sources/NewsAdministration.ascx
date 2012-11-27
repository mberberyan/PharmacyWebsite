<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsAdministration.ascx.cs" Inherits="Melon.Components.News.UI.CodeBehind.NewsAdministration" %>
<%@ Register Assembly="Melon.Components.NewsCore" Namespace="Melon.Components.News.UI.Controls"
    TagPrefix="melon" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="Melon.Components.News.Configuration" %>

<asp:ScriptManager runat="server">
    <Scripts>
        <asp:ScriptReference Path="JavaScript/news.js"/>
    </Scripts>
</asp:ScriptManager>

<ajaxToolkit:ModalPopupExtender ID="MPE" runat="server"
    BackgroundCssClass="mc_news_popup_error_background"
    TargetControlID="btnShowError"
    PopupControlID="PanelError"
    OkControlID="btnOkError"
    DropShadow="false"  />
    
<asp:Button ID="btnShowError" runat="server" Enabled="false" style="display:none;" />
<asp:Panel ID="PanelError" runat="server" CssClass="mc_news_popup_error" style="display:none;">
    <asp:Label ID="lblErrorTitle" runat="server" CssClass="mc_news_popup_error_title" meta:resourcekey="lblErrorTitle"/><br />
    <asp:Label ID="lblError" runat="server" /><br /><br />
    <div align="center">
        <asp:Button ID="btnOkError" runat="server" CssClass="mc_news_button" meta:resourcekey="btnOkError"/></div>
 </asp:Panel>

<div class="mc_news_main_panel">
    <div class="mc_news_menu_back">
        <div class="mc_news_div_menu">
            <!-- MENU -->
            <div class="mc_news_menu" id="divNavigation" runat="server">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td id="tdOpenNews" class="mc_news_menu_item_first" runat="server">
                            <asp:LinkButton ID="lbtnOpenNews" runat="server" meta:resourcekey="lbtnOpenNews"
                                CausesValidation="False" />
                        </td>
                        <td id="tdOpenCategories" class="mc_news_menu_item_middle" runat="server">
                            <asp:LinkButton ID="lbtnOpenCategories" runat="server" meta:resourcekey="lbtnOpenCategories"
                                CausesValidation="False" />
                        </td>
                        <td id="tdOpenComments" class="mc_news_menu_item_middle" runat="server">
                            <asp:LinkButton ID="lbtnOpenComments" runat="server" meta:resourcekey="lbtnOpenComments"
                                CausesValidation="False" />
                        </td>
                        <td id="tdOpenUsers" class="mc_news_menu_item_last" runat="server">
                            <asp:LinkButton ID="lbtnOpenUsers" runat="server" meta:resourcekey="lbtnOpenUsers"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </div>
            <!-- User name of logged user -->
            <div class="mc_news_username">
            <asp:LoginName ID="LoginName" runat="server" />
        </div>
        </div>
    </div>    
   
    <!-- NEWS PANELS -->
    <div class="mc_news_panel_content mc_news_panels_boder_top">
            <div class="mc_news_panels_border_middle">
                <table cellpadding="0" cellspacing="0" width="100%" class="mc_news_main_table">
                    <tr>
                        <td>
                            <div>
                                <melon:NewsPanel ID="panelFirst" runat="server" />
                                <melon:NewsPanel ID="panelSecond" runat="server" Style="padding-top:20px;" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="mc_news_panels_border_footer">&nbsp;</div>
        </div>
</div>
