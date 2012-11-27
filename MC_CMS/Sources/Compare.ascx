<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compare" CodeFile="Compare.ascx.cs" %>

<!-- Begin Top Corners -->
<div class="mc_cms_top_left_corner">
    <div class="mc_cms_top_right_corner"></div>
</div>
<!-- End Top Corners -->

<!-- Begin Content Borders -->
<div class="mc_cms_content_border mc_cms_body" style="z-index:2;">
      
      <div class="mc_cms_compare_screen_title" style="z-index:2;">
        <asp:Label ID="lblCompareScreenTitle" runat="server"/>
      </div>
      <div class="mc_cms_compare_close" style="z-index:2;">                                        
         <asp:Button ID="btnCloseComparePage" runat="server" CssClass="mc_cms_button mc_cms_btn_61" OnClientClick="disablePopup(); return false;" meta:resourcekey="btnCloseComparePage" 
                CausesValidation="false" />
      </div>
      <div class="mc_cms_clear" style="z-index:2;"></div>
    
      <!-- Begin Settings -->
      <div class="mc_cms_compare_left_div" style="z-index:2;">
        <div class="mc_cms_compare_right_div" style="z-index:2;">
          <asp:Table ID="tblSettings" runat="server" CellPadding="5" BorderColor="1" CellSpacing="0" Width="100%" CssClass="mc_cms_compare_table">
             <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="20%">
                    <asp:Label ID="lblSetting" runat="server" Text="Setting" meta:resourcekey="lblSetting"/></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="40%">
                    <asp:Label ID="lblCurrent" runat="server" Text="Current" meta:resourcekey="lblCurrent"/></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="40%">
                    <asp:Label ID="lblLive" runat="server" Text="Live" meta:resourcekey="lblLive"/></asp:TableHeaderCell>
             </asp:TableHeaderRow>
             
             <asp:TableRow>
                <asp:TableCell ColumnSpan="3" Height="30px">
                    <asp:Label ID="lblGeneral" runat="server" CssClass="mc_cms_header" meta:resourcekey="lblGeneral"/></asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trAlias">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblAlias" runat="server" Text="Alias" meta:resourcekey="lblAlias"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentAlias" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveAlias" runat="server"/></asp:TableCell>
             </asp:TableRow>
              <asp:TableRow ID="trTitle">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblTitle" runat="server" Text="Title" meta:resourcekey="lblTitle"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentTitle" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveTitle" runat="server"/></asp:TableCell>
             </asp:TableRow>
              <asp:TableRow ID="trImage">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblImage" runat="server" Text="Image" meta:resourcekey="lblImage"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Image ID="imgCurrentImage" runat="server" />&nbsp;</asp:TableCell>
                <asp:TableCell>
                    <asp:Image ID="imgLiveImage" runat="server"/>&nbsp;</asp:TableCell>
             </asp:TableRow>
              <asp:TableRow ID="trHoverImage">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblHoverImage" runat="server" Text="Hover Image" meta:resourcekey="lblHoverImage"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Image ID="imgCurrentHoverImage" runat="server" />&nbsp;</asp:TableCell>
                <asp:TableCell>
                    <asp:Image ID="imgLiveHoverImage" runat="server" />&nbsp;</asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trSelectedImage">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="imgSelectedImage" runat="server" Text="Selected Image" meta:resourcekey="imgSelectedImage"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Image ID="imgCurrentSelectedImage" runat="server" />&nbsp;</asp:TableCell>
                <asp:TableCell>
                    <asp:Image ID="imgLiveSelectedImage" runat="server" />&nbsp;</asp:TableCell>
             </asp:TableRow>
              <asp:TableRow ID="trHideInNavigation">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblHideInNavigation" runat="server" Text="Hide in navigation" meta:resourcekey="lblHideInNavigation"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentHideInNavigation" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveHideInNavigation" runat="server"/></asp:TableCell>
             </asp:TableRow>
              <asp:TableRow ID="trTarget">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblTarget" runat="server" Text="Target" meta:resourcekey="lblTarget"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentTarget" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveTarget" runat="server"/></asp:TableCell>
             </asp:TableRow>
             
             <asp:TableRow ID="trStaticPage" Height="30px">
                <asp:TableCell ColumnSpan="3">
                    <asp:Label ID="lblStaticPage" runat="server" CssClass="mc_cms_header" meta:resourcekey="lblStaticPage"/></asp:TableCell>
             </asp:TableRow>        
             <asp:TableRow ID="trStaticPageUrl">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblUrl" runat="server"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentUrl" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveUrl" runat="server"/></asp:TableCell>
             </asp:TableRow>
             
             <asp:TableRow ID="trTemplate" Height="30px">
                <asp:TableCell ColumnSpan="3">
                    <asp:Label ID="lblTemplate" runat="server" CssClass="mc_cms_header" meta:resourcekey="lblTemplate"/></asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trTemplateName">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblTemplateName" runat="server" meta:resourcekey="lblTemplateName"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentTemplateName" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveTemplateName" runat="server"/></asp:TableCell>
             </asp:TableRow>
             
             <asp:TableRow ID="trMetaTags" Height="30px">
                <asp:TableCell ColumnSpan="3">
                    <asp:Label ID="lblMetaTags" runat="server" CssClass="mc_cms_header" meta:resourcekey="lblMetaTags"/></asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trMetaTagAuthor">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblMetaTagAuthor" runat="server" meta:resourcekey="lblMetaTagAuthor"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentMetaTagAuthor" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveMetaTagAuthor" runat="server"/></asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trMetaTagKeywords">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblMetaTagKeywords" runat="server" meta:resourcekey="lblMetaTagKeywords"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentMetaTagKeywords" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveMetaTagKeywords" runat="server"/></asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trMetaTagDescription">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblMetaTagDescription" runat="server" meta:resourcekey="lblMetaTagDescription"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentMetaTagDescription" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveMetaTagDescription" runat="server"/></asp:TableCell>
             </asp:TableRow>
             
             <asp:TableRow ID="trPermissionsTitle" Height="30px">
                <asp:TableCell ColumnSpan="3">
                    <asp:Label ID="lblPermissions" runat="server" CssClass="mc_cms_header" meta:resourcekey="lblPermissions"/></asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trVisibilityPermissions">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblVisibleFor" runat="server" Text="Visible For" meta:resourcekey="lblVisibleFor"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentVisibleFor" runat="server"/></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveVisibleFor" runat="server"/></asp:TableCell>
             </asp:TableRow>
             <asp:TableRow ID="trAccessibilityPermissions">
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblAccessibleFor" runat="server" Text="Accessible For" meta:resourcekey="lblAccessibleFor"/></asp:TableCell>
                <asp:TableCell CssClass="mc_cms_td_delimiter">
                    <asp:Label ID="lblCurrentAccessibleFor" runat="server" /></asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLiveAccessibleFor" runat="server" /></asp:TableCell>
             </asp:TableRow>
          </asp:Table>
        </div>
      </div>
      <!-- End Settings -->
      
      <!-- Begin Content -->
      <div class="mc_cms_compare_left_div" style="z-index:2;">
        <div class="mc_cms_compare_right_div" style="z-index:2;">
          <asp:Table ID="tblContent" Width="100%" CellPadding="0" CellSpacing="0" runat="server" CssClass="mc_cms_compare_table" >
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>
                    <asp:Label ID="lblContentPlaceholder" runat="server" meta:resourcekey="lblContentPlaceholder"/></asp:TableHeaderCell>
               <asp:TableHeaderCell Width="40%">
                    <asp:Label ID="lblCurrentContent" runat="server" meta:resourcekey="lblCurrent"/></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="40%">
                    <asp:Label ID="lblLiveContent" runat="server" meta:resourcekey="lblLive"/></asp:TableHeaderCell>
             </asp:TableHeaderRow>
          </asp:Table>
        </div>
     </div>
     <!-- End Content -->
 </div>
 <!-- End Content Borders -->
 
 <!-- Begin Bottom Corners -->
 <div class="mc_cms_bottom_left_corner" style="z-index:2;">
    <div class="mc_cms_bottom_right_corner" style="z-index:2;"></div>
</div>
<!-- End Bottom Corners -->
