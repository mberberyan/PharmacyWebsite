<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FECollectionDetails.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FECollectionDetails" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<div id="divCollectionDetails" class="mc_pc_productDetailsBox">
    <div id="divCollectionName" class="div_mc_pc_productDetailsTitle">
        <asp:Label ID="lblCollectionName" runat="server" CssClass="mc_pc_productDetailsTitle" />
    </div>
    <div>
        <div id="divImage" class="mc_pc_productDetailsImage">
            <asp:Image ID="imgCollection" runat="server" />
        </div>
        <div id="divDescription" class="mc_pc_productDetailsLongDesc">
            <asp:Label ID="lblLongDescription" runat="server" />
        </div>
        <div class="clear"></div>
    </div>
    <div id="divOtherImages" runat="server" class="mc_pc_productFiles">
        <asp:Label ID="lblOtherImages" runat="server" meta:resourcekey="lblOtherImages" />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNoImages" runat="server" meta:resourcekey="lblNoImages" Visible="false" />
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

    <div id="divAudio" runat="server" class="mc_pc_productFiles">
        
        <b><asp:Label ID="lblAudio" runat="server" meta:resourcekey="lblAudio" /></b>
        
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNoAudio" runat="server" meta:resourcekey="lblNoAudio" Visible="false" />
                    <asp:Repeater ID="repAudioList" runat="server">
                        <HeaderTemplate>
                            <ul id="audio-list">    
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <div style="padding-left:21px;">
                                    <img id="audioItem" runat="server"  onclick=""/>
                                </div>		        		                             
                                <div style="float:left;">
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

    <div id="divVideo" runat="server" class="mc_pc_productFiles">
    
        <asp:Label ID="lblVideo" runat="server" meta:resourcekey="lblVideo" />
    
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
                                    <img id="videoItem" runat="server"  />
                                </div>		        		                                     
                                <div style="float:left;">
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
    
    <div id="divCollectionProducts" runat="server" class="mc_pc_productList">        
        <div class="mc_pc_title"> 
            <asp:Label ID="lblProductList" runat="server" meta:resourcekey="lblProductList" />
        </div>
        <!-- Pager for the grid view with collection products -->
        <div class="mc_pc_pager">
            <melon:Pager ID="CollectionProductsPager" runat="server" ShowItemsDetails="true" />
        </div>        
        <!-- *** Grid with found collection products *** -->
        <asp:GridView ID="gvCollectionProducts" runat="server"            
                GridLines="None"
                AutoGenerateColumns="False"              
                ShowHeader="false"                 
                CssClass="gv_productList"                            
                RowStyle-CssClass="gv_productList_row" 
                AlternatingRowStyle-CssClass="gv_productList_alt_row"
                AllowPaging="true" 
                PagerSettings-Visible="false"                 
                AllowSorting="true"                                                             
            >
            <Columns>
                <asp:TemplateField ItemStyle-Width="100">                    
                <ItemTemplate>
                    <div class="productsListImage">                        
                    <asp:HyperLink ID="hplProduct" runat="server">
                        <asp:Image id="imgCollection" runat="server" />
                    </asp:HyperLink>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="120" >                    
                <ItemTemplate>
                    <div class="productListTitle">
                    <asp:HyperLink id="lbName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'/>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="120" >                    
                <ItemTemplate>
                <div  class="productListDescription">
                    <asp:Label id="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryList") %>'/>
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="80" >                    
                <ItemTemplate>
                    <div  class="productListDescription">
                    <asp:Label id="lblPrice" runat="server" />
                </div>
                </ItemTemplate>                     
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="200">
                <ItemTemplate>
                    <div  class="productListDescription">
                    <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                    </div>
                </ItemTemplate>                     
            </asp:TemplateField>                
            </Columns>
        </asp:GridView>
    </div>
    <div class="gv_pc_back" class>                    
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="shortButton" />
    </div>
</div>
<script type="text/javascript">
    var imgPopupImage = document.getElementById('imgPopupImage');

    function openAudioPopup(targetURL, windowName, windowFeatures) {
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

    function openVideoPopup(targetURL, windowName, windowFeatures) {
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