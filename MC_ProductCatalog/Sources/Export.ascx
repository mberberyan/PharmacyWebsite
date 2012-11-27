<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Export.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Export" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<div id="divExportDetails" runat="server">
    <span class="mc_pc_welcome_page_text_bold"><asp:Localize ID="locExport" runat="server" meta:resourcekey="locExport" /></span>
    <table class="exportTable" cellspacing="0">
        <tr>
            <td><span class="mc_pc_welcome_page_text_bold"><asp:Localize ID="locCategories" runat="server" meta:resourcekey="locCategories" /></span></td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="chkSelectAllCategory" onclick="javascript:SelectAllDetails(this,'chklCategory');" /><asp:Localize ID="locCheck" runat="server" meta:resourcekey="locCheck" />
                <asp:CheckBoxList ID="chklCategory" runat="server" RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" onclick="javascript:CheckSelectAll(this,'chkSelectAllCategory');">
                    <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" />
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" />
                    <asp:ListItem Text="<%$ Resources: ShortDescription %>" Value="ShortDesc" />
                    <asp:ListItem Text="<%$ Resources: LongDescription %>" Value="LongDesc" />
                    <asp:ListItem Text="<%$ Resources: Tags %>" Value="Tags" />
                    <asp:ListItem Text="<%$ Resources: Unit %>" Value="Unit" />
                    <asp:ListItem Text="<%$ Resources: IsActive %>" Value="IsActive" />                    
                    <asp:ListItem Text="<%$ Resources: DynamicProperties %>" Value="DynProp" />                    
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr class="altRow">
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><span class="mc_pc_welcome_page_text_bold"><asp:Localize ID="locProducts" runat="server" meta:resourcekey="locProducts" /></span></td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="chkSelectAllProducts" onclick="javascript:SelectAllDetails(this,'chklProduct');" /><asp:Localize ID="locCheck2" runat="server" meta:resourcekey="locCheck2" />
                <asp:CheckBoxList ID="chklProduct" runat="server" RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" onclick="javascript:CheckSelectAll(this,'chkSelectAllProducts');">
                    <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" />
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" />
                    <asp:ListItem Text="<%$ Resources: ShortDescription %>" Value="ShortDesc" />
                    <asp:ListItem Text="<%$ Resources: LongDescription %>" Value="LongDesc" />
                    <asp:ListItem Text="<%$ Resources: Tags %>" Value="Tags" />
                    <asp:ListItem Text="<%$ Resources: IsActive %>" Value="IsActive" />
                    <asp:ListItem Text="<%$ Resources: IsInStock %>" Value="IsInStock" />
                    <asp:ListItem Text="<%$ Resources: IsFeatured %>" Value="IsFeatured" />
                    <asp:ListItem Text="<%$ Resources: CommonPrice %>" Value="CommonPrice" />                    
                    <asp:ListItem Text="<%$ Resources: Unit %>" Value="Unit" />
                    <asp:ListItem Text="<%$ Resources: Manufacturer %>" Value="Manufacturer" />
                    <asp:ListItem Text="<%$ Resources: CategoryList %>" Value="CategoryList" />
                    <asp:ListItem Text="<%$ Resources: DynamicProperties %>" Value="DynProp" />
                    <asp:ListItem Text="<%$ Resources: AppliedDiscounts %>" Value="AppliedDiscounts" />
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr class="altRow">
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><span class="mc_pc_welcome_page_text_bold"><asp:Localize ID="locBundles" runat="server" meta:resourcekey="locBundles" /></span></td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="chkSelectAllBundles" onclick="javascript:SelectAllDetails(this,'chklBundle');" /><asp:Localize ID="locCheck3" runat="server" meta:resourcekey="locCheck3" />
                <asp:CheckBoxList ID="chklBundle" runat="server" RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" onclick="javascript:CheckSelectAll(this,'chkSelectAllBundles');">
                    <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" />
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" />
                    <asp:ListItem Text="<%$ Resources: CategoryList %>" Value="CategoryList" />
                    <asp:ListItem Text="<%$ Resources: ShortDescription %>" Value="ShortDesc" />
                    <asp:ListItem Text="<%$ Resources: LongDescription %>" Value="LongDesc" />
                    <asp:ListItem Text="<%$ Resources: Tags %>" Value="Tags" />
                    <asp:ListItem Text="<%$ Resources: CommonPrice %>" Value="CommonPrice" />                    
                    <asp:ListItem Text="<%$ Resources: IsActive %>" Value="IsActive" />                                                                                
                    <asp:ListItem Text="<%$ Resources: DynamicProperties %>" Value="DynProp" />                    
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr class="altRow">
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><span class="mc_pc_welcome_page_text_bold"><asp:Localize ID="locCatalogs" runat="server" meta:resourcekey="locCatalogs" /></span></td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="chkSelectAllCatalogs" onclick="javascript:SelectAllDetails(this,'chklCatalog');" /><asp:Localize ID="locCheck4" runat="server" meta:resourcekey="locCheck4" />
                <asp:CheckBoxList ID="chklCatalog" runat="server" RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" onclick="javascript:CheckSelectAll(this,'chkSelectAllCatalogs');">
                    <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" />
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" />                    
                    <asp:ListItem Text="<%$ Resources: ShortDescription %>" Value="ShortDesc" />
                    <asp:ListItem Text="<%$ Resources: LongDescription %>" Value="LongDesc" />
                    <asp:ListItem Text="<%$ Resources: Tags %>" Value="Tags" />                    
                    <asp:ListItem Text="<%$ Resources: IsActive %>" Value="IsActive" />                                                                                                    
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr class="altRow">
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><span class="mc_pc_welcome_page_text_bold"><asp:Localize ID="locCollections" runat="server" meta:resourcekey="locCollections" /></span></td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="chkSelectAllCollections" onclick="javascript:SelectAllDetails(this,'chklCollection');" /><asp:Localize ID="locCheck5" runat="server" meta:resourcekey="locCheck5" />
                <asp:CheckBoxList ID="chklCollection" runat="server" RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" onclick="javascript:CheckSelectAll(this,'chkSelectAllCollections');">
                    <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" />
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" />                    
                    <asp:ListItem Text="<%$ Resources: ShortDescription %>" Value="ShortDesc" />
                    <asp:ListItem Text="<%$ Resources: LongDescription %>" Value="LongDesc" />
                    <asp:ListItem Text="<%$ Resources: Tags %>" Value="Tags" />                    
                    <asp:ListItem Text="<%$ Resources: IsActive %>" Value="IsActive" />                                                                                                    
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr class="altRow">
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><span class="mc_pc_welcome_page_text_bold"><asp:Localize ID="locDiscounts" runat="server" meta:resourcekey="locDiscounts" /></span></td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="chkSelectAllDiscounts" onclick="javascript:SelectAllDetails(this,'chklDiscount');" /><asp:Localize ID="locCheck6" runat="server" meta:resourcekey="locCheck6" />
                <asp:CheckBoxList ID="chklDiscount" runat="server" RepeatColumns="6" RepeatDirection="Vertical" RepeatLayout="Table" onclick="javascript:CheckSelectAll(this,'chkSelectAllDiscounts');">                    
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" />
                    <asp:ListItem Text="<%$ Resources: Description %>" Value="Description" />                                        
                    <asp:ListItem Text="<%$ Resources: DiscountType %>" Value="DiscountType" /> 
                    <asp:ListItem Text="<%$ Resources: DiscountDateFrom %>" Value="DiscountDateFrom" />
                    <asp:ListItem Text="<%$ Resources: DiscountDateTo %>" Value="DiscountDateTo" />
                    <asp:ListItem Text="<%$ Resources: DiscountValue %>" Value="DiscountValue" />
                    <asp:ListItem Text="<%$ Resources: IsActive %>" Value="IsActive" />                                                                                                    
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>
    <div>
        <asp:Button ID="btnExport" runat="server" meta:resourcekey="btnExport" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick='<%# "javascript:return OnExportObjects(\"chklCategory\",\"chklProduct\",\"chklBundle\",\"chklCatalog\",\"chklCollection\",\"chklDiscount\", \"" + this.GetLocalResourceObject("CheckItemAlertMessage").ToString() +"\");" %>' />
    </div>    
</div>

