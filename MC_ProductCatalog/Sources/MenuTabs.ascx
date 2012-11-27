<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuTabs.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_MenuTabs" %>
<%@Import Namespace="Melon.Components.ProductCatalog" %>

<ul id="ulMenuTabs" runat="server" class="mc_pc_tabs">
    <li class="liGeneral"><a runat="server" id="tabGeneralInformation" onclick="javascript:SelectMenuTab('GeneralInformation');" class="mc_pc_tab_selected" ><asp:Localize ID="locGeneralInfo" runat="server" meta:resourcekey="locGeneralInfo" /></a></li>
    <li class="liProducts"><a runat="server" id="tabProducts" onclick="javascript:SelectMenuTab('Products');" class="mc_pc_tab_unselected"><asp:Localize ID="locProducts" runat="server" meta:resourcekey="locProducts" /></a></li>        
    <li class="liImages"><a runat="server" id="tabImages" onclick="javascript:SelectMenuTab('Images');" class="mc_pc_tab_unselected" ><asp:Localize ID="locImages" runat="server" meta:resourcekey="locImages" /></a></li>
    <li class="liAudio"><a runat="server" id="tabAudio" onclick="javascript:SelectMenuTab('Audio');" class="mc_pc_tab_unselected" ><asp:Localize ID="locAudio" runat="server" meta:resourcekey="locAudio" /></a></li>
    <li class="liVideo"><a runat="server" id="tabVideo" onclick="javascript:SelectMenuTab('Video');" class="mc_pc_tab_unselected" ><asp:Localize ID="locVideo" runat="server" meta:resourcekey="locVideo" /></a></li>
    <li class="liDynamicProperties"><a  runat="server" id="tabDynamicProperties" onclick="javascript:SelectMenuTab('DynamicProperties');" class="mc_pc_tab_unselected" ><asp:Localize ID="locDynProp" runat="server" meta:resourcekey="locDynProp" /></a></li>
    <li class="liRelatedProducts"><a  runat="server" id="tabRelatedProducts" onclick="javascript:SelectMenuTab('RelatedProducts');" class="mc_pc_tab_unselected" ><asp:Localize ID="locRelated" runat="server" meta:resourcekey="locRelated" /></a></li>
    <li class="liStatistics"><a  runat="server" id="tabStatistics" onclick="javascript:SelectMenuTab('Statistics');" class="mc_pc_tab_unselected" ><asp:Localize ID="locStatistics" runat="server" meta:resourcekey="locStatistics" /></a></li>        
</ul>
<div class="mc_pc_clear">&nbsp;</div>
<script type="text/javascript" language="javascript">
    var tabGeneralInformation = document.getElementById('<%= tabGeneralInformation.ClientID %>');
    var tabImages = document.getElementById('<%= tabImages.ClientID %>');
    var tabAudio = document.getElementById('<%= tabAudio.ClientID %>');
    var tabVideo = document.getElementById('<%= tabVideo.ClientID %>');
    var tabDynamicProperties = document.getElementById('<%= tabDynamicProperties.ClientID %>');
    var tabRelatedProducts = document.getElementById('<%= tabRelatedProducts.ClientID %>');
    var tabStatistics = document.getElementById('<%= tabStatistics.ClientID %>');
    var tabProducts = document.getElementById('<%= tabProducts.ClientID %>');
</script>