<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMS" CodeFile="CMS.ascx.cs" %>
<%@ Register Assembly="Melon.Components.CMSCore" Namespace="Melon.Components.CMS.UI.Controls" TagPrefix="melon" %>
<%@ Import Namespace="Melon.Components.CMS" %>
<%@ Import Namespace="Melon.Components.CMS.UI.Controls" %>
<%@ Import Namespace="Melon.Components.CMS.Configuration" %>

<script src='<%=ResolveUrl(CMSSettings.BasePath)+ "Sources/JavaScript/cms.js"%>'></script>
<script src='<%=ResolveUrl(CMSSettings.BasePath)+ "Sources/JavaScript/jquery-1.3.2.min.js"%>'></script>
<script src='<%=ResolveUrl(CMSSettings.BasePath)+ "Sources/JavaScript/easing.js"%>'></script>
<script src='<%=ResolveUrl(CMSSettings.BasePath)+ "Sources/JavaScript/popup.js"%>'></script>
<script type="text/javascript">    
    $(document).ready(function() {
        if ('<%= !IsVertical %>' == 'True') {
            $("#divLeftPanel").width(300);
            $("#divFirstPanel").width(270);
            $("#divLeftPanel").css('float', 'left');
            $("#divFirstPanel").css('float', 'left');
            $("#divRightPanel").css('margin-left', '300px');

            if ($("input[id$=hfCollapsed]").val() == 'true') {
                $("a#controlbtn").addClass('mc_cms_close').html('');
                $("div#divFirstPanel").addClass('mc_cms_left_border');     
                $("div#divPanels").animate({
                    marginLeft: -($("div#divLeftPanel").width() - 30)
                }, {
                    duration: 0,
                    easing: 'easeOutQuint'
                });

                if ($.browser.msie && (parseInt(jQuery.browser.version) == 6 || parseInt(jQuery.browser.version) == 7)) {
                    $("div#divLeftPanel").animate({
                        marginLeft: -($("div#divLeftPanel").width() - 30)
                    }, {
                        duration: 0,
                        easing: 'easeOutQuint'
                    });
                }
            }
        }
        else {
            $("#divLeftPanel").width('100%');
            $("#divFirstPanel").width('100%');
        }

        $("a#controlbtn").click(function(e) {

            e.preventDefault();

            var slidepx = $("div#divLeftPanel").width() - 30;

            if (!$("div#divPanels").is(':animated')) {

                if (parseInt($("div#divPanels").css('marginLeft'), 10) < 0) {
                    $(this).removeClass('mc_cms_close').html('');
                    $("div#divFirstPanel").removeClass('mc_cms_left_border');
                    margin = "+=" + slidepx;
                    $("input[id$=hfCollapsed]").val('false');                    
                } else {
                $(this).addClass('mc_cms_close').html('');
                    $("div#divFirstPanel").addClass('mc_cms_left_border');
                    margin = "-=" + slidepx;
                    $("input[id$=hfCollapsed]").val('true');                    
                }

                $("div#divPanels").animate({
                    marginLeft: margin
                }, {
                    duration: 'slow',
                    easing: 'easeOutQuint'
                });

                if ($.browser.msie && (parseInt(jQuery.browser.version) == 6 || parseInt(jQuery.browser.version) == 7)) {
                    $("div#divLeftPanel").animate({
                        marginLeft: margin
                    }, {
                        duration: 'slow',
                        easing: 'easeOutQuint'
                    });
                }
            }
        });
    });
</script>
<asp:HiddenField ID="hfCollapsed" runat="server" />
<div class="mc_cms_main">
    <!-- NAVIGATION -->
    <div id="divNavigation" runat="server" class="mc_cms_menu_back">
        <!-- Menu -->
        <div class="mc_cms_menu_section">
            <div class="mc_cms_menu_section_left">
                <table cellpadding="0" cellspacing="0" width="100">
                    <tr>
                        <td id="tdOpenExplorer" class="mc_cms_menu_item_explorer" runat="server">
                            <asp:LinkButton ID="lbtnOpenExplorer" runat="server" meta:resourcekey="lbtnOpenExplorer"
                                CausesValidation="false" />
                        </td>
                        <td id="tdOpenTemplates" class="mc_cms_menu_item_template" runat="server">
                            <asp:LinkButton ID="lbtnOpenTemplates" runat="server" meta:resourcekey="lbtnOpenTemplates"
                                CausesValidation="false" />
                        </td>
                        <td id="tdOpenUsers" class="mc_cms_menu_item_users" runat="server">
                            <asp:LinkButton ID="lbtnOpenUsers" runat="server" meta:resourcekey="lbtnOpenUsers"
                                CausesValidation="false" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lbtnReturnFromPreview" runat="server" CssClass="mc_cms_button mc_cms_btn_back"
                                meta:resourcekey="lbtnReturnFromPreview" />
                        </td>
                    </tr>
                </table>
            </div>
            <!-- User name of CMS user -->
            <div class="mc_cms_username">
                <asp:LoginName ID="LoginName1" runat="server" />
            </div>
        </div>
    </div>
    <!-- Error Message -->
    <div class="mc_cms_div_error_message">
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="mc_cms_error_message" />
    </div>



    <!-- CMS PANELS -->
    <div class="mc_cms_panels mc_cms_panels_boder_top  mc_cms_tp_lft">
        <div class="mc_cms_tp_rght">
            <div class="mc_cms_bttm_lft">
                <div class="mc_cms_panels_border_middle mc_cms_bttm_rght">

                <table cellpadding="0" cellspacing="0" width="100%" class="mc_cms_min_height">
                    <tr>
                        <td valign="top">
                            <asp:Image ID="imgTransparent" runat="server" ImageUrl='<%# Utilities.GetImageUrl(this.Page,"CMS_Styles/Images/transparent_px.gif")%>'
                                CssClass="mc_cms_img_transparent" /></td>
                        <td valign="top" style="overflow:hidden;">
                            <div id="divPanels" style="border:0px; ">                            
                                <div id="divLeftPanel">
                                    <div id="divFirstPanel" style="vertical-align:top;">
                                        <melon:CMSPanel ID="panelFirst" runat="server" />                                    
                                    </div>                
                                    <% if (!IsVertical) Response.Write("<div id=\"control\"><a id=\"controlbtn\" class=\"mc_cms_open\" href=\"\" alt=\"Expand/collapse explorer\"></a></div>");%>
                                </div>
                                <!--<% if (IsVertical) Response.Write("</tr><tr>"); else Response.Write("<td id=\"control\"><a id=\"controlbtn\" class=\"mc_cms_open\" href=\"\" alt=\"Expand/collapse explorer\"></a></td><td class='mc_cms_horizontalDelimiter'>&nbsp;</td>");%>  -->                                
                                <div id="divRightPanel" style="vertical-align:top;">                                        
                                    <melon:CMSPanel ID="panelSecond" runat="server" />
                                </div>                                                                    
                                <% if (!IsVertical) Response.Write("<div style=\"clear:both;\"></div>");%>
                                
                            </div>
                        </td>
                    </tr>
                </table>

            </div>
       </div>
  </div>
</div>



</div>