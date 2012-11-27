<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Audio.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<asp:HiddenField ID="hfMainAudioSrc" runat="server" />
<asp:HiddenField ID="hfAddNewAudio" runat="server" />
<asp:HiddenField ID="hfAudioId" runat="server" />
<input type="hidden" id="hfCurrentAudioTitle" />
<table>
    <tr>
        <td>
            <asp:Label ID="lblNoAudio" runat="server" Visible="false" meta:resourcekey="lblNoAudio"/>
            <asp:Repeater ID="repAudioList" runat="server">
                <HeaderTemplate>
                    <ul id="audio-list">    
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div class="mc_pc_divHoverImage">
                            <img id="audioItem" runat="server" title="<%$ Resources: audioItem %>" onclick="javascript:centerAudioPopup();loadAudioPopup(this);" />
                        </div>		        		     
                        <div class="mc_pc_divHoverCheck">
                            <img id="audioCheck" runat="server" src="../Sources/Styles/Images/check.gif" alt="<%$ Resources: audioCheck %>" />
                        </div>
                        <div class="left">
                            <asp:Label ID="lblAudioName" runat="server" CssClass="mc_pc_small_label" />               
                        </div>                  
                        <div class="clear"></div>                              
                        <div class="mc_pc_divHoverButton">
                            <asp:Button ID="btnRemoveAudio" runat="server" CssClass="mc_pc_button mc_pc_btn_61" meta:resourcekey="btnRemoveAudio" OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteAudio").ToString() + "\");" %>' />
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
        <td class="right">
            <asp:Button id="btnAddAudioFile" runat="server" meta:resourcekey="btnAddAudioFile" CssClass="mc_pc_button mc_pc_btn_106" OnClientClick="javascript:centerAudioPopup();loadAudioPopup(null);return false"  />
        </td>
    </tr>            
</table>        
<br />
<div id="popupAudioContact">
    <a id="popupAudioContactClose">x</a>		    
    <p id="contactAudioArea">
        <center>
            <span id="litAddAudioHeader" class="mc_pc_title_text"><asp:Literal ID="locAddAudioHeader" runat="server" meta:resourcekey="locAddAudioHeader" /></span>
            <span id="litEditAudioHeader" class="mc_pc_title_text"><asp:Literal ID="locEditAudioHeader"  runat="server" meta:resourcekey="locEditAudioHeader" /></span>
        </center>
	    <table class="table_popup">
	        <tr>
	            <td><asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" /></td>
	            <td><asp:TextBox ID="txtPopupAudioTitle" runat="server" CssClass="mc_pc_input_short"/></td>	            
	        </tr>
	        <tr id="rowAudioUpload">
	            <td><asp:Localize ID="locPath" runat="server" meta:resourcekey="locPath" /></td>
	            <td>
	                <div id="divAudioUpload">
	                    <asp:FileUpload ID="fuAudio" runat="server" />
	                    <div id="divAudioLimit" runat="server" class="mc_pc_small_label" />
	                </div>
	                <div>
	                    <asp:RequiredFieldValidator ID="rfvAudioUpload" runat="server" ControlToValidate="fuAudio" Display="Dynamic" meta:resourcekey="rfvAudioUpload" CssClass="mc_pc_validator" ValidationGroup="AudioUpload" />
	                    <asp:RegularExpressionValidator ID="revAudioUpload" runat="server" ControlToValidate="fuAudio" Display="Dynamic" ValidationExpression="^.+\.(mp3|MP3)$" CssClass="mc_pc_validator" meta:resourcekey="revAudioUpload" ValidationGroup="AudioUpload" />
	                </div>
	            </td>
	        </tr>
	        <tr>
	            <td><asp:Localize ID="locMainAudio" runat="server" meta:resourcekey="locMainAudio" /></td>
	            <td><asp:CheckBox ID="chkMainAudio" runat="server" /></td>
	        </tr>
	        <tr>
	            <td></td>
	            <td>
	                <asp:Button ID="btnEditAudio" runat="server" meta:resourcekey="btnEditAudio" CssClass="mc_pc_button mc_pc_btn_61"/>
	                <asp:Button ID="btnAddAudio" runat="server" meta:resourcekey="btnAddAudio" ValidationGroup="AudioUpload" CssClass="mc_pc_button mc_pc_btn_61"/>
	            </td>
	        </tr>	        
	        <tr>
	            <td colspan="2">	                
	                <a href="#" id="lnkPreviewAudio" onclick="javascript:openPopup('<%= ResolveClientUrl(ProductCatalogSettings.BasePath) %>Sources/flashPopupAudio.htm?file=../Data/Audio/'+hfCurrentAudioTitle.value)"><asp:Localize ID="locPreviewAudio" runat="server" meta:resourcekey="locPreviewAudio" /></a>
	            </td>
	        </tr>
	    </table>
    </p>
</div>
<div id="backgroundAudioPopup"></div>
<script type="text/javascript">
    var hfMainAudioSrc = document.getElementById('<%= hfMainAudioSrc.ClientID %>'); 
    var hfAddNewAudio = document.getElementById('<%= hfAddNewAudio.ClientID %>');
    var hfAudioId = document.getElementById('<%= hfAudioId.ClientID %>');
    var hfCurrentAudioTitle = document.getElementById('hfCurrentAudioTitle');
    var lnkPreviewAudio = document.getElementById('lnkPreviewAudio');


    var litAddAudioHeader = document.getElementById('litAddAudioHeader');
    var litEditAudioHeader = document.getElementById('litEditAudioHeader');
    var txtPopupAudioTitle = document.getElementById('<%= txtPopupAudioTitle.ClientID %>');        
    var rfvAudioUpload = document.getElementById('<%= rfvAudioUpload.ClientID %>');   
    var revAudioUpload = document.getElementById('<%= revAudioUpload.ClientID %>');   
    var rowAudioUpload = document.getElementById('rowAudioUpload');    
    var btnAddAudio = document.getElementById('<%= btnAddAudio.ClientID %>');
    var btnEditAudio = document.getElementById('<%= btnEditAudio.ClientID %>');
    var chkMainAudio = document.getElementById('<%= chkMainAudio.ClientID %>');

    function openPopup(targetURL, windowName, windowFeatures) {
        if (!windowName)
            windowName = "someID";

        if (!windowFeatures)
            windowFeatures = "status=0, toolbar=0, location=0, menubar=0, directories=0, resizable=0, scrollbars=0, height=80, width=350";

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
