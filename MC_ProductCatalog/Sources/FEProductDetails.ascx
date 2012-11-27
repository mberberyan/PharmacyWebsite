<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEProductDetails.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEProductDetails" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<script src='<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Example/JavaScript/rating.js"%>'></script>
<div id="divProductDetails" class="mc_pc_productDetailsBox">
    <div id="divProductName" class="div_mc_pc_productDetailsTitle">
        <asp:Label ID="lblProductName" runat="server" CssClass="mc_pc_productDetailsTitle" />
    </div>
    <div>
        <div id="divImage" class="mc_pc_productDetailsImage">
            <asp:Image ID="imgProduct" runat="server" />
        </div>
        <div id="divProductPrice" runat="server" class="mc_pc_productDetailsPricing">      
        </div>
        <div id="divDescription" runat="server" class="mc_pc_productDetailsLongDesc">
            <asp:Label ID="lblLongDescription" runat="server" />
        </div>
    </div>
    <br />
     <!---------- Dynamic properties - size, color ... ----------->
    <div id="divDynamicProperties" runat="server" class="mc_pc_productDetailsDynamicProperties">
        <asp:Label ID="lblDynamicProperties" CssClass="mc_pc_productDetailsFeatures" runat="server" meta:resourcekey="lblDynamicProperties" />
        <asp:ListView ID="lvPropValues" runat="server" > 
            <LayoutTemplate>                                               
                <table id="tabPropValues" runat="server" class="mc_pc_gridDynamicProperties" cellpadding="0" cellspacing="1">                        
                <!-- Header row will be filled with property names -->                                                                
                    <tr>                                
                    </tr>                        
                <!-- End Header row -->
                <tr ID="groupPlaceholder" runat="server" />
                </table>                            
            </LayoutTemplate>
            <GroupTemplate>                                   
                <tr id="Tr1" runat="server" class="mc_pc_gridDynamicProperties_alt_row">
                    <td id="itemPlaceholder" runat="server">                        
                        &nbsp;
                    </td>                    
                </tr>
            </GroupTemplate>
            <ItemTemplate>                                                                
                <td id="cellView" runat="server" >
                    <asp:Label ID="lblPropValue" runat="server" Text='<%# Eval("PropertyValue") %>' />                
                </td>                
            </ItemTemplate>                        
        </asp:ListView>
    </div>
    <div class="clear"></div>
     <!---------- Other Images List ----------->
    <div id="divOtherImages" runat="server" class="mc_pc_productFiles">
        
        <asp:Label ID="lblOtherImages" runat="server" meta:resourcekey="lblOtherImages" />
        
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNoImages" runat="server" meta:resourcekey="lblNoImages" Visible="false"  />
                    <asp:Repeater ID="repImageList" runat="server">
                        <HeaderTemplate>
                            <ul id="my-list" class="mc_pc_otherImages_list">    
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <div style="padding-left:21px;">
                                    <img id="imgItem" runat="server" meta:resourcekey="imgItem" style="vertical-align:middle;cursor:pointer;" />
                                </div>		        		                                                                     
                                <div class="clear"></div>                                                          
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>                                                     
                </td>
            </tr>
        </table>
    </div>
    <div id="popupImageContact">
        <a id="popupImageContactClose">x</a>		    
        <p id="contactImageArea">        
	        <img id="imgPopupImage" src="" alt="" style="padding-top:5px;" />
        </p>
    </div>
    <div id="backgroundImagePopup"></div>
    <!---------- Audio List ----------->
    <div id="divAudio" runat="server" class="mc_pc_productFiles">
        
        <b><asp:Label ID="lblAudio" runat="server" meta:resourcekey="lblAudio" /></b>
        
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNoAudio" runat="server" meta:resourcekey="lblNoAudio" Visible="false" />
                    <asp:Repeater ID="repAudioList" runat="server">
                        <HeaderTemplate>
                        <ul id="audio-list" class="mc_pc_audio_list">    
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>     		                             
                                 <div class="audioImage">
                                    <img id="audioItem" runat="server" />
                                </div>      
                                <div class="audioTitle">
                                    <asp:Label ID="lblAudioName" runat="server" Text="img 1" />               
                                </div>                                  
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>                                                     
                </td>
            </tr>
        </table>
    </div>
    <!---------- Video List ----------->
    <div id="divVideo" runat="server" class="mc_pc_productFiles">
        
        <b><asp:Label ID="lblVideo" runat="server" meta:resourcekey="lblVideo" /></b>
        
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNoVideo" runat="server" meta:resourcekey="lblNoVideo" Visible="false" />
                    <asp:Repeater ID="repVideoList" runat="server">
                        <HeaderTemplate>
                            <ul id="video-list" class="mc_pc_video_list">    
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <div class="videoImage">
                                    <img id="videoItem" runat="server" />
                                </div>   
                                <div class="videoTitle">
                                    <asp:Label ID="lblVideoName" runat="server" Text="img 1" />               
                                </div>                                     
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>                                                     
                </td>
            </tr>
        </table>
    </div>
    <!---------- Related products ----------->
    <div id="divRelatedProducts" runat="server" class="mc_pc_productDetailsRelatedProducts">                
        <!-- *** Grid with found from product search *** -->
        <asp:DataList ID="dlRelatedProducts" runat="server"
            RepeatColumns="6" 
            GridLines="None" 
            RepeatDirection="Horizontal" 
            RepeatLayout="Table" 
            CssClass="mc_pc_grid"
            ItemStyle-CssClass="mc_pc_grid_row"
            ItemStyle-Width="100"
            AlternatingItemStyle-CssClass="mc_pc_grid_row_alt"                    
            ItemStyle-HorizontalAlign="Center"                    
            >                           
            <HeaderTemplate>
                <span class="mc_pc_title"><asp:Localize ID="locRelatedProducts" runat="server" meta:resourcekey="locRelatedProducts" /></span>
            </HeaderTemplate>
            <ItemTemplate>                            
                <div class="mc_pc_grid_row_name">
                    <a id="aName" runat="server" />
                </div>           
                <div class="productsListImage">
                    <asp:HyperLink ID="hplRelatedProduct" runat="server" >
                        <asp:Image id="imgProduct" runat="server" />
                    </asp:HyperLink>
                </div>
                <div class="mc_pc_grid_row_startprice">
                    <asp:Label id="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CommonPrice") %>' />
                </div>
                <div class="mc_pc_grid_row_description">
                    <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                </div>
            </ItemTemplate>
        </asp:DataList>
        <!-- Pager for the grid view with product reviews-->
        <melon:Pager ID="TopPager" runat="server" CssClass="mc_pc_pager_related" ShowItemsDetails="true" />
    </div>
    <!---------- Review ----------->
    <div id="divProductReviews" runat="server">                   
        <table class="mc_pc_productReview">
            <tr>
                <td>                                
                    <!-- *** Grid with found product reviews *** -->
                    <asp:GridView ID="gvProductReview" runat="server"            
                            GridLines="None"
                            AutoGenerateColumns="False"              
                            ShowHeader="true" 
                            CssClass="mc_pc_gridReview"            
                            HeaderStyle-CssClass="mc_pc_grid_headerReview" 
                            RowStyle-CssClass="mc_pc_gridReview_row" 
                            AlternatingRowStyle-CssClass="mc_pc_gridReview_alt_row"
                            AllowPaging="true" 
                            PagerSettings-Visible="false" 
                            EmptyDataRowStyle-BackColor="#f7f7f7"
                            AllowSorting="true"                       
                            Width="100%"                                      
                        >
                        <Columns>
                            <asp:TemplateField>                    
                                <HeaderTemplate>                        
                                    <asp:Localize ID="locReviews" runat="server" meta:resourcekey="locReviews" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <b><asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PostedBy") %>' /></b> 
                                            </td> 
                                            <td align="right">
                                                <asp:Localize ID="locRating" runat="server" meta:resourcekey="locRating" /> : <asp:Image ID="imgRating" runat="server"/>
                                            </td>                               
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblReviewText" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Text") %>'/> 
                                            </td>
                                        </tr>
                                    </table>                        
                                </ItemTemplate>                     
                            </asp:TemplateField>            
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <!-- Pager for the grid view with product reviews-->
                    <div class="pagerReview">
                        <melon:Pager ID="ProductReviewPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="true" />
                    </div>
                </td>
            </tr>
        </table>
                
        <div id="divProductReviewSaveError" runat="server" visible="false" class="mc_pc_error_message" />
        <br />
        <b><asp:Label ID="lblWriteReview" runat="server" meta:resourcekey="lblWriteReview" /></b>
        <br /><br />
        <table>
            <tr>
                <td><asp:Localize ID="locName" runat="server" meta:resourcekey="locName" /></td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="mc_pc_input_short" MaxLength="64"/>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" meta:resourcekey="rfvName" 
                            ControlToValidate="txtName" Display="Dynamic" CssClass="mc_pc_validator" 
                            ValidationGroup="ProductReview" />
                        <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_pc_validator" meta:resourcekey="revName" ValidationGroup="ProductReview"
                        ValidationExpression="[a-zA-Zа-яА-Я0-9\'\x22\-_\s,.]*"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td><asp:Localize ID="locReview" runat="server" meta:resourcekey="locReview" /></td>
                <td>
                    <asp:TextBox ID="txtReview" runat="server" Rows="3" Columns="40" TextMode="MultiLine" onkeypress="return imposeMaxLength(this, 500);" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvReview" runat="server" meta:resourcekey="rfvReview" ControlToValidate="txtReview" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="ProductReview" />
                        <asp:RegularExpressionValidator ID="revReview" runat="server" ControlToValidate="txtReview" Display="Dynamic" SetFocusOnError="true"
                            CssClass="mc_pc_validator" meta:resourcekey="revReview" ValidationGroup="ProductReview"
                            ValidationExpression="[a-zA-Zа-яА-Я0-9\'\x22\-_\s,.:;!?()/]*"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td><asp:Localize ID="locYourRating" runat="server" meta:resourcekey="locYourRating" /></td>
                <td>
                    <div id="reviewRating" >
                        <a onclick="rateIt(this)" id="_1" onmouseover="rating(this)" onmouseout="off(this)"></a>
                        <a onclick="rateIt(this)" id="_2" onmouseover="rating(this)" onmouseout="off(this)"></a>
                        <a onclick="rateIt(this)" id="_3" onmouseover="rating(this)" onmouseout="off(this)"></a>
                        <a onclick="rateIt(this)" id="_4" onmouseover="rating(this)" onmouseout="off(this)"></a>
                        <a onclick="rateIt(this)" id="_5" onmouseover="rating(this)" onmouseout="off(this)"></a>
                    </div>
                    <asp:TextBox ID="txtRating" runat="server" style="display:none;" />
                    <asp:RequiredFieldValidator ID="rfvRating" runat="server" meta:resourcekey="rfvRating" ControlToValidate="txtRating" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="ProductReview" />                    
                </td>
            </tr>
            <tr>
                <td>                    
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="shortButton" />
                </td>
                <td align="right">
                    <asp:Button ID="btnSubmitReview" runat="server" meta:resourcekey="btnSubmitReview" CssClass="shortButton" ValidationGroup="ProductReview" />
                </td>
            </tr>                
        </table>                    
    </div>
</div>
<script type="text/javascript">
    var imgPopupImage = document.getElementById('imgPopupImage');
    var txtRating=document.getElementById('<%= txtRating.ClientID %>');

    function openAudioPopup(targetURL, windowAudioName, windowAudioFeatures) {
        if (!windowAudioName)
            windowAudioName = "audioID";

        if (!windowAudioFeatures)
            windowAudioFeatures = "status=0, toolbar=0, location=0, menubar=0, directories=0, resizable=0, scrollbars=0, height=80, width=350";

        var newAudioWindow = window.open(targetURL, windowAudioName, windowAudioFeatures);

        var msecs = 1;
        var id = setInterval(watchAudioWindowCreation, msecs);

        function watchAudioWindowCreation() {
            if (newAudioWindow.document.body && newAudioWindow.document.body.offsetWidth) {
                newAudioWindow.moveTo((newAudioWindow.screen.availWidth - newAudioWindow.document.body.offsetWidth) / 2, (newAudioWindow.screen.availHeight - newAudioWindow.document.body.offsetHeight) / 2);
                clearInterval(id);
            }
        }
    }

    function openVideoPopup(targetURL, windowVideoName, windowVideoFeatures) {
        if (!windowVideoName)
            windowVideoName = "videoID";

        if (!windowVideoFeatures)
            windowVideoFeatures = "status=0, toolbar=0, location=0, menubar=0, directories=0, resizable=0, scrollbars=0, height=350, width=350";

        var newVideoWindow = window.open(targetURL, windowVideoName, windowVideoFeatures);

        var msecs = 1;
        var id = setInterval(watchVideoWindowCreation, msecs);

        function watchVideoWindowCreation() {
            if (newVideoWindow.document.body && newVideoWindow.document.body.offsetWidth) {
                newVideoWindow.moveTo((newVideoWindow.screen.availWidth - newVideoWindow.document.body.offsetWidth) / 2, (newVideoWindow.screen.availHeight - newVideoWindow.document.body.offsetHeight) / 2);
                clearInterval(id);
            }
        }
    }
</script>