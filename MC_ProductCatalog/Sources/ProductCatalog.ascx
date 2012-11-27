<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductCatalog.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductCatalog" %>
<%@ Register Assembly="Melon.Components.ProductCatalogCore" Namespace="Melon.Components.ProductCatalog.UI.Controls" TagPrefix="melon" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>

<script src="<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/ProductCatalog.js"%>"></script>
<script src="<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/swfobject.js"%>" type="text/javascript"></script>
<link href="<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/styles/jquery.hoverscroll-0.2.2.css"%>" type="text/css" rel="stylesheet" />
<link href="<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/styles/popup.css" %>" type="text/css" rel="stylesheet" />

<style type="text/css">
.hoverscroll {
	border: #000 solid 1px;
}	
    
#my-list li {
	width: 130px;
	height: 56px;		
	background: #fff;
	padding: 4px 5px;				
}

#audio-list li {
	width: 130px;
	height: 56px;		
	background: #fff;
	padding: 4px 5px;				
}

#video-list li {
	width: 130px;
	height: 56px;		
	background: #fff;
	padding: 4px 5px;				
}		
</style>

<script type="text/javascript" src="<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/jquery-1.3.2.min.js" %>"></script>
<script type="text/javascript" src="<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/jquery.hoverscroll-0.2.2.js" %>"></script>
<script type="text/javascript" src="<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/popup.js" %>"></script>

<script type="text/javascript">
    /**
    * HoverScroll default parameters
    */
    $.fn.hoverscroll.params = {
        vertical: false,
        width: 600,
        height: 140,
        arrows: true,
        arrowsOpacity: 0.7,
        debug: false
    };

</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div id="divMain" class="mc_pc_main">
    <!-- NAVIGATION -->
    <div id="divNavigation" runat="server" class="mc_pc_menu_back">                        
        <!-- MENU -->
        <div id="divMenu" class="mc_pc_menu_section">
            <div class="mc_pc_menu_section_left">
                <table runat="server" cellpadding="0" cellspacing="0">
                    <tr>
                        <td id="tdOpenCategoriesAndProducts" runat="server" class="mc_pc_menu_item_first">                    
                            <asp:LinkButton ID="lbOpenCategoriesAndProducts" runat="server" meta:resourcekey="lbOpenCategoriesAndProducts" CausesValidation="false" />
                        </td>
                        <td id="tdOpenCatalogs" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbOpenCatalogs" runat="server" meta:resourcekey="lbOpenCatalogs" CausesValidation="false" />
                        </td>
                        <td id="tdOpenBundles" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbOpenBundles" runat="server" meta:resourcekey="lbOpenBundles" CausesValidation="false" />
                        </td>
                        <td id="tdOpenCollections" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbOpenCollections" runat="server" meta:resourcekey="lbOpenCollections" CausesValidation="false" />
                        </td>
                        <td id="tdOpenDiscounts" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbOpenDiscounts" runat="server" meta:resourcekey="lbOpenDiscounts" CausesValidation="false" />
                        </td>
                        <td id="tdExport" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbExport" runat="server" meta:resourcekey="lbExport" CausesValidation="false" />
                        </td>
                        <td id="tdSearch" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbSearch" runat="server" meta:resourcekey="lbSearch" CausesValidation="false" />
                        </td>
                        <td id="tdOpenUsers" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbOpenUsers" runat="server" meta:resourcekey="lbOpenUsers" CausesValidation="false" />
                        </td>
                        <td id="tdOpenReviews" runat="server" class="mc_pc_menu_item_middle">
                            <asp:LinkButton ID="lbOpenReviews" runat="server" meta:resourcekey="lbOpenReviews" CausesValidation="false" />
                        </td>
                        <td id="tdOpenUnits" runat="server" class="mc_pc_menu_item_last">
                            <asp:LinkButton ID="lbOpenUnits" runat="server" meta:resourcekey="lbOpenUnits" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>                    
    </div>        
    
    <!-- Error Message -->
    <div class="mc_pc_error_message">
        <asp:Label ID="lblErrorMessage" runat="server"/>
    </div>        
    <div class="mc_pc_clear">&nbsp;</div>
    
    <div id="divPanels" class="mc_pc_panels mc_pc_panels_boder_top">
        <!-- User name of Product Catalog user -->
        <div id="divUser" class="mc_pc_username">
            <asp:Localize ID="locCurrentUser" runat="server" meta:resourcekey="locCurrentUser" /><b><asp:LoginName ID="LoginName" runat="server" /></b> | <asp:LoginStatus ID="LoginStatus1" runat="server" LogoutAction="Redirect" LogoutPageUrl="~/Login.aspx"/>
        </div>                
        <div class="mc_pc_panels_border_middle">            
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mc_pc_panels_table">
                <tr>
                    <td>                    
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td style="vertical-align:top;">
                                    <melon:ProductCatalogPanel ID="panelExplorer" runat="server" />
                                </td>                            
                                <td class="mc_pc_horizontalDelimiter">&nbsp;</td>
                                <td>
                                    <asp:Panel>
                                        <melon:ProductCatalogPanel ID="panelMain" runat="server" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="mc_pc_panels_border_footer">&nbsp;</div>
    </div>    
</div>
