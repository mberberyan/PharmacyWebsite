<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Images.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<asp:HiddenField ID="hfMainImageSrc" runat="server" />
<asp:HiddenField ID="hfAddNewImage" runat="server" />
<asp:HiddenField ID="hfImageId" runat="server" />
<table>
    <tr>
        <td>
            <asp:Label ID="lblNoImages" runat="server" meta:resourcekey="lblNoImages" Visible="false" />
            <asp:Repeater ID="repImageList" runat="server">
                <HeaderTemplate>
                    <ul id="my-list">    
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <div class="mc_pc_divHoverImage">
                            <img id="imgItem" runat="server" meta:resourcekey="imgItem" onclick="javascript:centerImagePopup();loadImagePopup(this);" />
                        </div>		        		     
                        <div class="mc_pc_divHoverCheck">
                            <img id="imgCheck" runat="server" meta:resourcekey="imgCheck" src="../Sources/Styles/Images/check.gif" />
                        </div>
                        <div class="left">
                            <asp:Label ID="lblImageName" runat="server" Text="img 1" CssClass="mc_pc_small_label" />               
                        </div>                  
                        <div class="clear"></div>                              
                        <div class="mc_pc_divHoverButton">
                            <asp:Button ID="btnRemoveImage" runat="server" meta:resourcekey="btnRemoveImage" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteImage").ToString() + "\");" %>' />
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
            <input id="btnOpenPopup" runat="server" type="button" meta:resourcekey="btnOpenPopup" class="mc_pc_button mc_pc_btn_106" onclick="javascript:centerImagePopup();loadImagePopup(null);" />
        </td>
    </tr>            
</table>        
<br />
<div id="popupImageContact">
    <a id="popupImageContactClose">x</a>		    
    <p id="contactImageArea">
        <center>
            <span id="litAddImageHeader" class="mc_pc_title_text"><asp:Localize ID="locAddImageHeader" runat="server" meta:resourcekey="locAddImageHeader" /></span>
            <span id="litEditImageHeader" class="mc_pc_title_text"><asp:Localize ID="locEditImageHeader" runat="server" meta:resourcekey="locEditImageHeader" /></span>
        </center>
	    <table class="table_popup">
	        <tr>
	            <td><asp:Localize ID="locAlt" runat="server" meta:resourcekey="locAlt" /></td>
	            <td><asp:TextBox ID="txtPopupImageAlt" runat="server" CssClass="mc_pc_input_short"/></td>
	            <td rowspan="4" valign="top" class="cell_popupImage">	                
	                <img id="imgPopupImage" src="" alt="" />
	            </td>
	        </tr>
	        <tr id="rowImageUpload">
	            <td><asp:Localize ID="locPath" runat="server" meta:resourcekey="locPath" /></td>
	            <td>
	                <asp:FileUpload ID="fuImage" runat="server" />
	                <div id="divImageLimit" runat="server" class="mc_pc_small_label"/>
	                <div>
	                    <asp:RequiredFieldValidator ID="rfvUpload" runat="server" meta:resourcekey="rfvUpload" ControlToValidate="fuImage" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="ImageUpload" />
	                    <asp:RegularExpressionValidator ID="revImage" runat="server" meta:resourcekey="revImage" ControlToValidate="fuImage" Display="Dynamic" 
                            CssClass="mc_cms_validator" ValidationGroup="ImageUpload"
                            ValidationExpression="(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)" />
	                </div>
	            </td>
	        </tr>
	        <tr>
	            <td><asp:Localize ID="locMainImage" runat="server" meta:resourcekey="locMainImage" /></td>
	            <td><asp:CheckBox ID="chkMainImage" runat="server" /></td>
	        </tr>
	        <tr>
	            <td></td>
	            <td>
	                <asp:Button ID="btnEditImage" runat="server" meta:resourcekey="btnEditImage" CssClass="mc_pc_button mc_pc_btn_61"/>
	                <asp:Button ID="btnAddImage" runat="server" meta:resourcekey="btnAddImage" CssClass="mc_pc_button mc_pc_btn_61" ValidationGroup="ImageUpload" />
	            </td>
	        </tr>
	        <tr>
	            <td colspan="2">	                
	                <a href="#" id="lnkPreviewImage"><asp:Localize ID="locPreviewImage" runat="server" meta:resourcekey="locPreviewImage" /></a>
	            </td>
	        </tr>
	    </table>
    </p>
</div>
<div id="backgroundImagePopup"></div>
<script type="text/javascript">    
    var hfMainImageSrc = document.getElementById('<%= hfMainImageSrc.ClientID %>'); 
    var hfAddNewImage = document.getElementById('<%= hfAddNewImage.ClientID %>'); 
    var hfImageId = document.getElementById('<%= hfImageId.ClientID %>');

    var litAddImageHeader = document.getElementById('litAddImageHeader');
    var litEditImageHeader = document.getElementById('litEditImageHeader'); 
    var imgPopupImage = document.getElementById('imgPopupImage');
    var txtPopupImageAlt = document.getElementById('<%= txtPopupImageAlt.ClientID %>');        
    var rowImageUpload = document.getElementById('rowImageUpload');
    var btnAddImage = document.getElementById('<%= btnAddImage.ClientID %>');
    var btnEditImage = document.getElementById('<%= btnEditImage.ClientID %>');    
    var chkMainImage = document.getElementById('<%= chkMainImage.ClientID %>');
    var lnkPreviewImage = document.getElementById('lnkPreviewImage');

    function openImagePopup(targetURL, windowName, windowFeatures) {
        if (!windowName)
            windowName = "someID";

        if (!windowFeatures)
            windowFeatures = "status=0, toolbar=0, location=0, menubar=0, directories=0, resizable=1, scrollbars=0, height=350, width=350";

        var newWindow = window.open('', windowName, windowFeatures);
        newWindow.document.writeln('<img src=\''+targetURL.replace('Thumb','Original')+'\'>');

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
