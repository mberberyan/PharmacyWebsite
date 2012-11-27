<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Video.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<asp:HiddenField ID="hfMainVideoSrc" runat="server" />
<asp:HiddenField ID="hfAddNewVideo" runat="server" />
<asp:HiddenField ID="hfVideoId" runat="server" />
<input type="hidden" id="hfCurrentVideoTitle" />
<table>
    <tr>
        <td>
            <asp:Label ID="lblNoVideo" runat="server" meta:resourcekey="lblNoVideo" Visible="false" />
            <asp:Repeater ID="repVideoList" runat="server">
                <HeaderTemplate>
                    <ul id="video-list">    
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div class="mc_pc_divHoverImage">
                            <img id="videoItem" runat="server" meta:resourcekey="videoItem" onclick="javascript:centerVideoPopup();loadVideoPopup(this);" />
                        </div>		        		     
                        <div class="mc_pc_divHoverCheck">
                            <img id="videoCheck" runat="server" meta:resourcekey="videoCheck" src="../Sources/Styles/Images/check.gif" />
                        </div>
                        <div class="left">
                            <asp:Label ID="lblVideoName" runat="server" Text="img 1" CssClass="mc_pc_small_label" />               
                        </div>                  
                        <div class="clear"></div>                              
                        <div class="mc_pc_divHoverButton">
                            <asp:Button ID="btnRemoveVideo" runat="server" meta:resourcekey="btnRemoveVideo" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteVideo").ToString() + "\");" %>' />
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>                                                     
        </td>
    </tr>
    <tr>
        <td align="right">
            <input id="btnAddVideoFile" runat="server" meta:resourcekey="btnAddVideoFile" class="mc_pc_button mc_pc_btn_106" type="button" onclick="javascript:centerVideoPopup();loadVideoPopup(null);" />
        </td>
    </tr>            
</table>        
<br />
<div id="popupVideoContact">
    <a id="popupVideoContactClose">x</a>		    
    <p id="contactVideoArea">
        <center>
            <span id="litAddVideoHeader" class="mc_pc_title_text"><asp:Localize ID="locAddVideoHeader" runat="server" meta:resourcekey="locAddVideoHeader" /></span>
            <span id="litEditVideoHeader" class="mc_pc_title_text"><asp:Localize ID="locEditVideoHeader" runat="server" meta:resourcekey="locEditVideoHeader" /></span>
        </center>
	    <table class="table_popup">
	        <tr>
	            <td><asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" /></td>
	            <td><asp:TextBox ID="txtPopupVideoTitle" runat="server" CssClass="mc_pc_input_short"/></td>	            
	        </tr>
	        <tr id="rowVideoUpload">
	            <td><asp:Localize ID="locPath" runat="server" meta:resourcekey="locPath" /></td>
	            <td>
	                <div id="divVideoUpload">
	                    <asp:FileUpload ID="fuVideo" runat="server" />
                        <div id="divVideoLimit" runat="server" class="mc_pc_small_label" />
	                </div>
	                <div>
	                    <asp:RequiredFieldValidator ID="rfvVideoUpload" runat="server" meta:resourcekey="rfvVideoUpload" ControlToValidate="fuVideo" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="VideoUpload" />
	                    <asp:RegularExpressionValidator ID="revVideoUpload" runat="server" meta:resourcekey="revVideoUpload" ControlToValidate="fuVideo" Display="Dynamic" CssClass="mc_pc_validator" ValidationExpression="^.+\.(flv|FLV)$" ValidationGroup="VideoUpload" />
	                </div>	                	                
	            </td>
	        </tr>
	        <tr>
	            <td><asp:Localize ID="locMainVideFile" runat="server" meta:resourcekey="locMainVideFile" /></td>
	            <td><asp:CheckBox ID="chkMainVideo" runat="server" /></td>
	        </tr>
	        <tr>
	            <td></td>
	            <td>
	                <asp:Button ID="btnEditVideo" runat="server" meta:resourcekey="btnEditVideo" CssClass="mc_pc_button mc_pc_btn_61"/>
	                <asp:Button ID="btnAddVideo" runat="server" meta:resourcekey="btnAddVideo" CssClass="mc_pc_button mc_pc_btn_61" ValidationGroup="VideoUpload" />
	            </td>
	        </tr>
	        <tr>
	            <td colspan="2">	                
	                <a href="#" id="lnkPreviewVideo" onclick="javascript:openPopup('<%= ResolveClientUrl(ProductCatalogSettings.BasePath) %>Sources/flashPopupVideo.htm?file=../../Data/Video/'+hfCurrentVideoTitle.value)"><asp:Localize ID="locPreviewVideoFile" runat="server" meta:resourcekey="locPreviewVideoFile" /></a>
	            </td>
	        </tr>
	    </table>
    </p>
</div>
<div id="backgroundVideoPopup"></div>
<script type="text/javascript">
    var hfMainVideoSrc = document.getElementById('<%= hfMainVideoSrc.ClientID %>'); 
    var hfAddNewVideo = document.getElementById('<%= hfAddNewVideo.ClientID %>'); 
    var hfVideoId = document.getElementById('<%= hfVideoId.ClientID %>');
    var hfCurrentVideoTitle = document.getElementById('hfCurrentVideoTitle');
    var lnkPreviewVideo = document.getElementById('lnkPreviewVideo');

    var litAddVideoHeader = document.getElementById('litAddVideoHeader');
    var litEditVideoHeader = document.getElementById('litEditVideoHeader');
    var txtPopupVideoTitle = document.getElementById('<%= txtPopupVideoTitle.ClientID %>');   
    var rfvVideoUpload = document.getElementById('<%= rfvVideoUpload.ClientID %>');   
    var revVideoUpload = document.getElementById('<%= revVideoUpload.ClientID %>');        
    var rowVideoUpload = document.getElementById('rowVideoUpload');
    var btnAddVideo = document.getElementById('<%= btnAddVideo.ClientID %>');
    var btnEditVideo = document.getElementById('<%= btnEditVideo.ClientID %>');
    var chkMainVideo = document.getElementById('<%= chkMainVideo.ClientID %>');

    function openPopup(targetURL, windowName, windowFeatures) {
        if (!windowName)
            windowName = "someID";

        if (!windowFeatures)
            windowFeatures = "status=0, toolbar=0, location=0, menubar=0, directories=0, resizable=0, scrollbars=0, height=350, width=350";

        var newWindow = window.open(targetURL, windowName, windowFeatures);

        var msecs = 1;
        var id = setInterval(watchWindowCreation, msecs);

        function watchWindowCreation() {
            if (newWindow.document.body && newWindow.document.body.offsetWidth) {
                newWindow.moveTo((newWindow.screen.availWidth - newWindow.document.body.offsetWidth) / 2, (newWindow.screen.availHeight - newWindow.document.body.offsetHeight) / 2);
                clearInterval(id);
            }
        }
    }

</script>

