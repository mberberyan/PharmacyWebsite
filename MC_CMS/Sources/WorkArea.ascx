<%@Control Language="C#" AutoEventWireup="true" Inherits="WorkArea" CodeFile="WorkArea.ascx.cs" %>
<%@Register TagPrefix="cms" TagName="Languages" Src="Languages.ascx" %>
<%@Register TagPrefix="cms" TagName="Compare" Src="Compare.ascx" %>
<%@Register TagPrefix="settings" TagName="GeneralBox" Src="SettingsBoxGeneral.ascx" %>
<%@Register TagPrefix="settings" TagName="TemplateBox" Src="SettingsBoxTemplate.ascx" %>
<%@Register TagPrefix="settings" TagName="StaticPageBox" Src="SettingsBoxStaticPage.ascx" %>
<%@Register TagPrefix="settings" TagName="MetaTagsBox" Src="SettingsBoxMetaTags.ascx" %>
<%@Register TagPrefix="settings" TagName="PermissionsBox" Src="SettingsBoxPermissions.ascx" %>
<%@Register TagPrefix="settings" TagName="LogBox" Src="SettingsBoxLog.ascx" %>
<%@ Reference Control="Languages.ascx" %>
<%@Import Namespace="Melon.Components.CMS" %>
<%@Import Namespace="Melon.Components.CMS.Configuration" %>

<div class="mc_cms_language_bar">
    <!-- Languages -->
    <cms:Languages ID="cntrlLanguages" runat="server"/>  
 </div> 
 
<!-- This Panel is empty and appears when "Explorer" node is selected in the CMS Explorer tree.-->
<asp:Panel ID="divExplorerLayout" runat="server" CssClass="mc_cms_explorer_layout">
    <div class="rght">
    <div style="padding: 30px 0px 25px 20px;">
        <span class="mc_cms_title_text">Welcome to the administration of your website.</span>
    </div>
    <div style="padding: 0px 0px 15px 20px;">
        <span class="mc_cms_welcome_page_text">To start editing the structure or the contents of your website you may do the following:</span>
    </div>
    <div style="padding: 0 0 10px 22px;">
        <span class="mc_cms_welcome_page_text_bold">In the CMS Explorer Tab:</span>
        <ul class="mc_cms_welcome_page_list">
            <li>
                Create a folder section which is a place holder of various pages on your website.<br />
                To create a folder you may click here, or on the folder button situated on the<br />
                top-left of the navigation panel. <asp:LinkButton ID="lbtnClickHereCMSExplorerCreateFolder" runat="server" meta:resourcekey="lbtnClickHere" CausesValidation="false" CssClass="mc_cms_welcome_page_link" />
            </li>
            <li>                
                Create a static page. <asp:LinkButton ID="lbtnClickHereCMSExplorerCreateStatic" runat="server" meta:resourcekey="lbtnClickHere" CausesValidation="false" CssClass="mc_cms_welcome_page_link" />
            </li>            
            <li>
                Create a content-manageable page. <asp:LinkButton ID="lbtnClickHereCMSExplorerCreateContent" runat="server" meta:resourcekey="lbtnClickHere" CausesValidation="false" CssClass="mc_cms_welcome_page_link" />
            </li>
        </ul>
    </div>
    <div style="padding: 0 0 10px 22px;">
        <span class="mc_cms_welcome_page_text_bold">In the Templates Tab:</span>
        <ul class="mc_cms_welcome_page_list">            
            <li>
                Create special template page. <asp:LinkButton ID="lbtnClickHereTemplatesCreate" runat="server" meta:resourcekey="lbtnClickHere" CausesValidation="false" CssClass="mc_cms_welcome_page_link" />
            </li>
            <li>
                Edit or Delete the existing templates. <asp:LinkButton ID="lbtnClickHereTemplatesEdit" runat="server" meta:resourcekey="lbtnClickHere" CausesValidation="false" CssClass="mc_cms_welcome_page_link" />
            </li>
        </ul>
    </div>
    <div style="padding: 0 0 10px 22px;">
        <span class="mc_cms_welcome_page_text_bold">In the Users Tab:</span>
        <ul class="mc_cms_welcome_page_list">
            <li>
                Define and update the roles and access of the site users (should you<br />
                have the right to do so). <asp:LinkButton ID="lbtnClickHereUsers" runat="server" meta:resourcekey="lbtnClickHere" CausesValidation="false" CssClass="mc_cms_welcome_page_link" />
            </li>            
        </ul>
    </div>
</div><!-- .rght -->
</asp:Panel>

<!-- This Panel contains all settings and apears when node is selected in the CMS Explorer tree 
except in the case when "Explorer" is selected.-->
<asp:Panel ID="divWorkArea" runat="server" CssClass="mc_cms_panels_inner_section_explorer_padding">
    
    <!-- Tabs -->
    <div class="mc_cms_tp_lft">
    <div class="mc_cms_tp_rght">
    <div class="mc_cms_bttm_lft">
    <div class="mc_cms_bttm_rght">

    <div style="overflow:hidden;" class="mc_cms_tab_container">
        <ul class="mc_cms_tabs">
            <li class="tabSettings"><a id ="tabSettings" class="mc_cms_tab_selected" onclick='SelectSettingsTab();'><%=Convert.ToString(GetLocalResourceObject("Settings"))%></a></li>
            <li class="tabContent"><a id ="tabContent" class="mc_cms_tab_unselected" onclick='SelectContentTab();'><%=Convert.ToString(GetLocalResourceObject("Content"))%></a></li>
        </ul>
         <div class="mc_cms_preview_btn">
            <!-- Preview Links-->
            <asp:LinkButton ID="lbtnPreviewLive" runat="server" CssClass="mc_cms_preview_live"
                meta:resourcekey="lbtnPreviewLive"  />
            <asp:LinkButton ID="lbtnPreviewDraft" runat="server" CssClass="mc_cms_preview_draft"
                meta:resourcekey="lbtnPreviewDraft" />
        </div>    
    </div>
    
        
    <!-- <div class="mc_cms_clear">&nbsp;</div> -->

    <div class="mc_cms_panels_inner_section_content">
        <asp:Label ID="lblDefaultSettingsLoaded" runat="server" Visible="false" CssClass="mc_cms_default_settings_loaded mc_cms_warning_message" meta:resourcekey="lblDefaultSettingsLoaded"/>    
        <!-- Content of tab "Settings" -->
        <div id="divSettings" class="mc_cms_settings">
            <settings:GeneralBox ID="cntrlGeneralSettings" runat="server"/>
            <settings:TemplateBox ID="cntrlTemplateSettings" runat="server"/>
            <settings:MetaTagsBox ID="cntrlMetaTagsSettings" runat="server"/>
            <settings:StaticPageBox ID="cntrlStaticPageSettings" runat="server"/>
            <settings:PermissionsBox ID="cntrlPermissionsSettings" runat="server"/>
            <settings:LogBox ID="cntrlLog" runat="server"/>
        </div>            
        
        <!-- Content of tab "Content" -->
        <div id="divContent" class="mc_cms_content">
            <div style="padding:5px 5px 5px 10px"><asp:Label ID="lblLoginLogoutWarning" runat="server" CssClass="mc_cms_warning_message" meta:resourcekey="lblLoginLogoutWarning"/></div>
            <iframe id="iFramePageContent" name="iFramePageContent" src="" class="mc_cms_contentFrame" frameborder="0"></iframe>
        </div>
        
        <!-- Buttons -->
        <div class="mc_cms_Buttons">
             <asp:Button ID="btnCompare" runat="server" CssClass="mc_cms_button mc_cms_btn_156 mc_cms_button_green_color" meta:resourcekey="btnCompare" 
                CausesValidation="true" ValidationGroup="NodeSettings" />
             &nbsp;
             <asp:Button ID="btnLoadFromLive" runat="server" CssClass="mc_cms_button mc_cms_btn_156" meta:resourcekey="btnLoadFromLive"
                CausesValidation="false"/>
             &nbsp;
             <asp:Button ID="btnSaveAndPublish" runat="server" CssClass="mc_cms_button mc_cms_btn_106" meta:resourcekey="btnSaveAndPublish" 
                ValidationGroup="NodeSettings"/>
             &nbsp;
             <asp:Button ID="btnSaveDraft" runat="server" CssClass="mc_cms_button mc_cms_btn_106" meta:resourcekey="btnSaveDraft" 
                ValidationGroup="NodeSettings" />
             <asp:Button ID="btnLoadPage" runat="server" Style="visibility:hidden;" />   
        </div>
    </div>   
 <div class="mc_cms_panels_inner_section_footer">&nbsp;</div>


    </div> <!-- .bttm_rght -->
    </div> <!-- .bttm_lft -->
    </div> <!-- .tp_rght -->
    </div> <!-- .mc_cms_tp_lft -->


 </asp:Panel>

<div id="popupContact">
    <cms:Compare ID="ucCompare" runat="server" />
</div>
<div id="backgroundPopup"></div>

<asp:HiddenField ID="hfSelectedTab" runat="server"/>
<asp:HiddenField ID="hfPageContent" runat="server"/>
<asp:HiddenField ID="hfNewFrameUrl" runat="server" />

<script type="text/javascript" language="javascript">

var tabContent = document.getElementById('tabContent');
var tabSettings = document.getElementById('tabSettings');
var divContent = document.getElementById('divContent');
var divSettings = document.getElementById('divSettings');
var hfSelectedTab = document.getElementById('<%= hfSelectedTab.ClientID %>');
var iFramePageContent = document.getElementById('iFramePageContent');

var hfNewFrameUrl = document.getElementById('<%= hfNewFrameUrl.ClientID %>');
var btnLoadPage = document.getElementById('<%= btnLoadPage.ClientID %>');
var timer_CheckFrameUrl;//Timer to check whether the url in the content iframe is not changed.

var hfPageContent = document.getElementById('<%= hfPageContent.ClientID %>');

var txtCode = document.getElementById('<%= cntrlGeneralSettings.FindControl("txtCode").ClientID %>');
var txtTitle = document.getElementById('<%= cntrlGeneralSettings.FindControl("txtTitle").ClientID %>');
var txtFrameTitle = document.getElementById('<%= cntrlGeneralSettings.FindControl("txtFrameName").ClientID %>');
var txtExternalPage = document.getElementById('<%= cntrlStaticPageSettings.FindControl("txtExternalPage").ClientID %>');
var txtAuthor = document.getElementById('<%= cntrlMetaTagsSettings.FindControl("txtAuthor").ClientID %>');
var txtKeywords = document.getElementById('<%= cntrlMetaTagsSettings.FindControl("txtKeywords").ClientID %>');
var txtDescription = document.getElementById('<%= cntrlMetaTagsSettings.FindControl("txtDescription").ClientID %>');
var revImage = document.getElementById('<%= cntrlGeneralSettings.FindControl("revImage").ClientID %>');
var fuImage = document.getElementById('<%= cntrlGeneralSettings.FindControl("fuImage").ClientID %>');
var revHoverImage = document.getElementById('<%= cntrlGeneralSettings.FindControl("revHoverImage").ClientID %>');
var fuHoverImage = document.getElementById('<%= cntrlGeneralSettings.FindControl("fuHoverImage").ClientID %>');
var revSelectedImage = document.getElementById('<%= cntrlGeneralSettings.FindControl("revSelectedImage").ClientID %>');
var fuSelectedImage = document.getElementById('<%= cntrlGeneralSettings.FindControl("fuSelectedImage").ClientID %>');
 

//Script that make the initialization of the tabs.
if(<%=(this.NodeType == NodeType.ContentManageablePage) ? "true" : "false"%>)
{     
    tabContent.style.visibility='visible'; 
     
    if (<%=(this.SelectedTab == WorkAreaTabs.Content)? "true" : "false" %>)
    {
        SelectContentTab();
    }
    else
    {
        SelectSettingsTab();
    }        

    <% 
        bool hasSelectedTemplate = (cntrlTemplateSettings.TemplateId != null);
        string pageParams = "";
        string url="";
        if (hasSelectedTemplate)
        {         
           string pageDraftUrl = CMSSettings.DynamicContentManageablePagePath;
           pageParams = "?templateId=" + Convert.ToString(cntrlTemplateSettings.TemplateId);
           if (this.CurrentNode != null)
           {
                pageParams += "&pageId=" + Convert.ToString(this.CurrentNode.Id);
                if (this.LiveVersionLoaded)
                {
                    pageParams += "&restore=yes";
                }
           }
           pageDraftUrl += pageParams;
           url = ResolveUrl(CMSSettings.RouterPagePath +"?version=draft&mode=edit&lang=" + this.ParentControl.CurrentLanguage.Name + "&url=" + Server.UrlEncode(pageDraftUrl));
        } 
    %>
    
    if ("<%=hasSelectedTemplate %>")
    {
        iFramePageContent.src = '<%= url%>';
        CheckFrameUrl('<%= pageParams %>');  
    } 
}
else
{
    if (!!document.getElementById('tabContent'))
    {
        tabContent.style.visibility='hidden';
        divContent.style.display='none';
        tabSettings.className='mc_cms_tab_selected';
        divSettings.style.display='block';
    }
}
</script>
