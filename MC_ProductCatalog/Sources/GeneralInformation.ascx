<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GeneralInformation.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_GeneralInformation" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<asp:HiddenField ID="hfCategoryList" runat="server" />
<asp:HiddenField ID="hfCategoryName" runat="server" />
<div id="divMessage" runat="server" class="mc_pc_success_message">
    
</div>
<div>
    <table cellpadding="0" cellspacing="5" class="mc_pc_table" >
        <col width="100px"/>
        <col width="350px"/>
        <tr>
            <td><asp:Localize ID="locCode" runat="server" meta:resourcekey="locCode" /><span class="mc_pc_star">*</span></td>
            <td>
                <asp:TextBox ID="txtCode" runat="server" CssClass="mc_pc_input_short"/>
                <div>
                    <asp:RequiredFieldValidator ID="rfvCode" runat="server" meta:resourcekey="rfvCode" ControlToValidate="txtCode" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="GeneralInformation"/>                    
                </div>
                <div>
                    <asp:RegularExpressionValidator ID="revCode" runat="server" ControlToValidate="txtCode" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_pc_validator" meta:resourcekey="revCode" ValidationExpression='[a-zA-Zа-яА-Я0-9_%./\!?*#@]*' ValidationGroup="GeneralInformation"/>
                </div>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locName" runat="server" meta:resourcekey="locName" /><span class="mc_pc_star">*</span></td>
            <td>
                <asp:TextBox ID="txtName" runat="server" CssClass="mc_pc_input_short"/>
                <div>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" meta:resourcekey="rfvName" ControlToValidate="txtName" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="GeneralInformation"/>
                </div>
                <div>
                    <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_pc_validator" meta:resourcekey="revName" ValidationExpression='[^,]+' ValidationGroup="GeneralInformation"/>
                </div>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locShortDesc" runat="server" meta:resourcekey="locShortDesc" /></td>
            <td><asp:TextBox ID="txtShortDesc" runat="server" CssClass="mc_pc_input_short"/></td>
        </tr>
        <tr>
            <td ><asp:Localize ID="locLongDesc" runat="server" meta:resourcekey="locLongDesc" /></td>
            <td><asp:TextBox ID="txtLongDesc" runat="server" TextMode="MultiLine" Columns="40" Rows="5" /></td>
        </tr>
        <tr>
            <td ><asp:Localize ID="locTags" runat="server" meta:resourcekey="locTags" /></td>
            <td><asp:TextBox ID="txtTags" runat="server" TextMode="MultiLine" Columns="40" Rows="5"/></td>
        </tr>        
        <tr>
            <td><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locIsActive" /></td>
            <td><asp:CheckBox ID="chkIsActive" runat="server" /></td>
        </tr>    
    </table>    
    <div id="divPrice" runat="server" visible="false">
        <table class="mc_pc_table">
            <col width="100px" />
            <col width="350px" />            
            <tr>
                <td><asp:Localize ID="locCommonPrice" runat="server" meta:resourcekey="locCommonPrice" /><span class="mc_pc_star">*</span></td>
                <td>
                    <asp:TextBox ID="txtCommonPrice" runat="server" CssClass="mc_pc_input_extra_short"/>&nbsp;&nbsp;<%= ProductCatalogSettings.Currency %>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" meta:resourcekey="rfvPrice" ControlToValidate="txtCommonPrice" Display="Dynamic" ValidationGroup="GeneralInformation" CssClass="mc_pc_validator" />
                        <asp:RegularExpressionValidator ID="revPrice" runat="server" meta:resourcekey="revPrice"  ControlToValidate="txtCommonPrice" ValidationExpression="([0-9]){1,26}([.]([0-9]{2}))?" Display="Dynamic" ValidationGroup="GeneralInformation" CssClass="mc_pc_validator"/>
                    </div>
                </td>
            </tr>            
        </table>
    </div>
    <div id="divCategoryControl" runat="server" visible="false" width="450px;" class="mc_pc_table">
        <table class="mc_pc_table">
            <col width="100px" />
            <col width="350px" />
            <tr>
                <td><asp:Localize ID="locCategory" runat="server" meta:resourcekey="locCategory" /><span class="mc_pc_star">*</span></td>
                <td>
                    <div id="divCategoryList" class="mc_pc_table_category_list hidden">                    
                    </div>
                    <div class="mc_pc_table_category_wrapper">
                        <asp:ListBox ID="lbCategoryList" runat="server" 
                            Width="200px"                             
                            SelectionMode="Multiple"                                                                               
                        >                        
                        </asp:ListBox>
                    </div>                    
                    <asp:Button ID="btnAddCategory" runat="server" meta:resourcekey="btnAddCategory" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick="javascript:AddProductCategories();return false;" />
                </td>
            </tr>
        </table>
    </div>    
    <div id="divProduct" runat="server" visible="false">
        <table class="mc_pc_table">
            <col width="100px" />
            <col width="350px" />            
            <tr>
                <td><asp:Localize ID="locInStock" runat="server" meta:resourcekey="locInStock" /></td>
                <td><asp:CheckBox ID="chkIsInStock" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Localize ID="locIsFeatured" runat="server" meta:resourcekey="locIsFeatured" /></td>
                <td><asp:CheckBox ID="chkIsFeatured" runat="server" /></td>
            </tr>                        
            <tr>
                <td><asp:Localize ID="locManufacturer" runat="server" meta:resourcekey="locManufacturer" /></td>
                <td><asp:TextBox ID="txtManufacturer" runat="server" CssClass="mc_pc_input_short"/></td>
            </tr>            
            <tr>
                <td><asp:Localize ID="locAppliedDiscounts" runat="server" meta:resourcekey="locAppliedDiscounts" /></td>
                <td class="padding"><asp:Label ID="lblAppliedDiscounts" runat="server" /></td>
            </tr>
        </table>
    </div>       
    <div id="divMeasurementUnit" runat="server" visible="false">
        <table class="mc_pc_table">
            <col width="100px" />
            <col width="350px" />            
            <tr>
                <td><asp:Localize ID="locUnit" runat="server" meta:resourcekey="locUnit" /><span class="mc_pc_star">*</span></td>
                <td>
                    <asp:DropDownList ID="ddlMeasurementUnit" runat="server" />                
                    <div>
                        <asp:RequiredFieldValidator ID="rfvMeasurementUnit" runat="server" meta:resourcekey="rfvMeasurementUnit" ControlToValidate="ddlMeasurementUnit" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="GeneralInformation"/>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript" language="javascript">
    var lbCategoryList = document.getElementById('<%= lbCategoryList.ClientID %>');
    var hfCategoryList = document.getElementById('<%= hfCategoryList.ClientID %>');
    var hfCategoryName = document.getElementById('<%= hfCategoryName.ClientID %>');    
    
    addLoadEvent(
        RenderSelectedCategoryList, 
        document.getElementById('<%= hfCategoryList.ClientID %>'), 
        document.getElementById('<%= hfCategoryName.ClientID %>'),
        document.getElementById('divCategoryList'),
        '<%= "http://"+(Request.Url.AbsoluteUri.Contains("localhost") ? "localhost": Request.Url.Host) + Page.ResolveUrl(ProductCatalogSettings.BasePath) %>'
        );
    
        function AddProductCategories()
        {
            AddCategoryToList(
                document.getElementById('<%= hfCategoryList.ClientID %>'), 
                document.getElementById('<%= hfCategoryName.ClientID %>'), 
                document.getElementById('<%= lbCategoryList.ClientID %>'), 
                document.getElementById('divCategoryList'),
                'ucGeneralInformation',
                '<%= "http://"+(Request.Url.AbsoluteUri.Contains("localhost") ? "localhost": Request.Url.Host) + Page.ResolveUrl(ProductCatalogSettings.BasePath) %>'
            );
        }
</script>