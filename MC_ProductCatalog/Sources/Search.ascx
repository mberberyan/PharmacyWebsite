<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Search" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>

<div id="divSearch" runat="server">
    <table class="mc_pc_table_listing" onkeydown="SetDefaultButton(document.getElementById(getName('input','btnSearch')), event)">
        <tr>
            <td><asp:Localize ID="locKeywords" runat="server" meta:resourcekey="locKeywords" /></td>
            <td colspan="4"><asp:TextBox ID="txtKeywords" runat="server" CssClass="mc_pc_input_long" /></td>        
        </tr>        
        <tr>
            <td><asp:Localize ID="locObjectType" runat="server" meta:resourcekey="locObjectType" /></td>
            <td colspan="4">
                <asp:CheckBoxList ID="cbxlObjectType" runat="server" RepeatDirection="Horizontal"  CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text="<%$ Resources: Category %>" Value="Category" Selected="True" />
                    <asp:ListItem Text="<%$ Resources: Product %>" Value="Product" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Catalog %>" Value="Catalog" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Bundle %>" Value="Bundle" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Collection %>" Value="Collection" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Discount %>" Value="Discount" Selected="True"/>                    
                </asp:CheckBoxList>
            </td>
        </tr>                
        <tr>
            <td><asp:Localize ID="locSearchIn" runat="server" meta:resourcekey="locSearchIn" /></td>
            <td colspan="4">
                <asp:CheckBoxList ID="cbxlSearchCriteria" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" Selected="True" />
                    <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Description %>" Value="Description" Selected="True"/>
                    <asp:ListItem Text="<%$ Resources: Tags %>" Value="Tags" Selected="True"/>
                </asp:CheckBoxList>
            </td>
        </tr>          
        <tr>
            <td><asp:Localize ID="locCategory" runat="server" meta:resourcekey="locCategory" /></td>
            <td colspan="4">
                <asp:DropDownList ID="ddlCategoryList" runat="server" Width="450px" /><asp:CheckBox ID="chkRecursiveCategory" runat="server" meta:resourcekey="chkRecursiveCategory" />
            </td>        
        </tr>      
        <tr>
            <td valign="bottom"><asp:Localize ID="locAddedBetween" runat="server" meta:resourcekey="locAddedBetween" /></td>
            <td>
                <table>
                    <tr>
                        <td>
                            <div class="mc_pc_divDate">
                                <asp:TextBox ID="txtAddedFrom" runat="server" MaxLength="10" CssClass="mc_pc_input_extra_short" onclick="javascript:ibtnOpenCalendarAddedFrom.click(); return false;"/><br />
                                <asp:Label ID="lblComment" runat="server" meta:resourcekey="lblComment" CssClass="mc_pc_comment" /><br />
                            </div>
                            <div class="left">
                                <asp:ImageButton ID="ibtnOpenCalendarAddedFrom" runat="server" />
                                <ajaxToolkit:CalendarExtender ID="ceAddedFrom" runat="server" Format="MM/dd/yyyy"
                                    TargetControlID="txtAddedFrom" PopupButtonID="ibtnOpenCalendarAddedFrom" Enabled="True"
                                    CssClass="ajax__calendar mc_pc_calendar" />
                                <!-- Time Posted -->                                
                            </div>
                            <div class="clear">
                                <asp:RegularExpressionValidator ID="revAddedFrom" runat="server" ControlToValidate="txtAddedFrom"
                                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                    CssClass="mc_pc_validator" Display="Dynamic" meta:resourcekey="revAddedFrom" ValidationGroup="Search"/>                                
                            </div>                            
                        </td>
                        <td class="mc_pc_calendar_space"><span><asp:Localize ID="locAnd" runat="server" meta:resourcekey="locAnd" /></span></td>
                        <td>
                            <div class="mc_pc_divDate">
                                <asp:TextBox ID="txtAddedTo" runat="server" MaxLength="10" CssClass="mc_pc_input_extra_short" onclick="javascript:ibtnOpenCalendarAddedTo.click(); return false;"/><br />
                                <asp:Label ID="lvlCommentAddedTo" runat="server" meta:resourcekey="lvlCommentAddedTo" CssClass="mc_pc_comment" /><br />
                            </div>
                            <div class="left">
                                <asp:ImageButton ID="ibtnOpenCalendarAddedTo" runat="server" />
                                <ajaxToolkit:CalendarExtender ID="ceAddedTo" runat="server" Format="MM/dd/yyyy"
                                    TargetControlID="txtAddedTo" PopupButtonID="ibtnOpenCalendarAddedTo" Enabled="True"
                                    CssClass="ajax__calendar mc_pc_calendar" />
                                <!-- Time Posted -->                                
                            </div>
                            <div class="clear">
                                <asp:RegularExpressionValidator ID="revAddedTo" runat="server" ControlToValidate="txtAddedTo"
                                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                    CssClass="mc_pc_validator" Display="Dynamic" meta:resourcekey="revAddedTo" ValidationGroup="Search"/>                                
                            </div>
                        </td>
                    </tr>
                    <tr>                        
                        <td colspan="3"><asp:CompareValidator ID="cvValidPeriod" runat="server" meta:resourcekey="cvValidPeriod" Display="Dynamic" ControlToValidate="txtAddedTo" ControlToCompare="txtAddedFrom" Operator="GreaterThanEqual" Type="Date"/></td>
                    </tr>
                </table>                    
            </td>
            <td rowspan="2">
                <table class="mc_pc_statusTable">
                    <tr>
                        <td><asp:Localize ID="locActive" runat="server" meta:resourcekey="locActive"/></td>
                        <td>
                            <asp:DropDownList ID="ddlActive" runat="server">
                                <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                                <asp:ListItem Text="<%$ Resources: Yes %>" Value="0" />
                                <asp:ListItem Text="<%$ Resources: No %>" Value="1" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Localize ID="locInStock" runat="server" meta:resourcekey="locInStock"/></td>
                        <td>
                            <asp:DropDownList ID="ddlInStock" runat="server">
                                <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                                <asp:ListItem Text="<%$ Resources: Yes %>" Value="0" />
                                <asp:ListItem Text="<%$ Resources: No %>" Value="1" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Localize ID="locFeatured" runat="server" meta:resourcekey="locFeatured" /></td>
                        <td>
                            <asp:DropDownList ID="ddlFeatured" runat="server">
                                <asp:ListItem Text="<%$ Resources: Any %>" Value="" />
                                <asp:ListItem Text="<%$ Resources: Yes %>" Value="0" />
                                <asp:ListItem Text="<%$ Resources: No %>" Value="1" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>            
            </td>          
            <td colspan="2"></td>  
        </tr>    
        <tr>
            <td valign="bottom"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locPriceBetween" /></td>
            <td>
                <asp:TextBox ID="txtPriceFrom" runat="server" CssClass="mc_pc_input_extra_short" /><span class="mc_pc_space_bottom"><asp:Localize ID="locAnd2" runat="server" meta:resourcekey="locAnd2" /></span><asp:TextBox ID="txtPriceTo" runat="server" CssClass="mc_pc_input_extra_short"/>
                <div>
                    <asp:CompareValidator ID="cvPriceFrom" runat="server" meta:resourcekey="cvPriceFrom" ControlToValidate="txtPriceFrom" Display="Dynamic" Type="Double" Operator="DataTypeCheck" ValidationGroup="Search"/>
                </div>
                <div>
                    <asp:CompareValidator ID="cvPriceTo" runat="server" meta:resourcekey="cvPriceTo" ControlToValidate="txtPriceTo" Display="Dynamic" Type="Double" Operator="DataTypeCheck" ValidationGroup="Search"/>
                </div>
                <div>
                    <asp:CustomValidator ID="cvComparePrices" runat="server" meta:resourcekey="cvComparePrices" ClientValidationFunction="CheckPriceRange" Display="Dynamic" ValidationGroup="Search" />
                </div>
            </td>
            <td></td>            
        </tr>        
        <tr>
            <td colspan="4"></td>            
            <td align="right">
                <asp:Button ID="btnClear" runat="server" meta:resourcekey="btnClear" CssClass="mc_pc_button mc_pc_btn_61" OnClientClick="javascript:ResetSearchCriteria(); return false;" />&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearch" CssClass="mc_pc_button mc_pc_btn_61" ValidationGroup="Search" />
            </td>
        </tr>
    </table>
               
    <table>
        <tr>
            <td>
                <!-- Pager for the grid view with users-->
                <melon:Pager ID="TopPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvObjectDetailsList" runat="server"
                    AutoGenerateColumns="false"   
                    GridLines="None"       
                    EmptyDataText="<%$ Resources: NoObjectsErrorMessage %>"
                    ShowHeader="true" 
                    CssClass="mc_pc_grid"
                    HeaderStyle-CssClass="mc_pc_grid_header" 
                    RowStyle-CssClass="mc_pc_grid_row" 
                    AlternatingRowStyle-CssClass="mc_pc_grid_alt_row"
                    AllowPaging="true" 
                    AllowSorting="true"
                    PagerSettings-Visible="false" 
                    EmptyDataRowStyle-BackColor="#f7f7f7"                
                >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="150" HeaderStyle-CssClass="mc_pc_grid_header_colFirst">
                            <HeaderTemplate>                                                        
                                <asp:LinkButton ID="lbtnSortByCode" runat="server" meta:resourcekey="lbtnSortByCode" 
                                    CommandName="Sort" CommandArgument="Code" />            
                                <asp:Image ID="imgSortDirectionCode" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Code" %>' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="150" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>                                
                                <asp:LinkButton ID="lbtnSortByName" runat="server" meta:resourcekey="lbtnSortByName" 
                                    CommandName="Sort" CommandArgument="Name" />            
                                <asp:Image ID="imgSortDirectionName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Name" %>' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="280" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>                                
                                <asp:LinkButton ID="lbtnSortByDescription" runat="server" meta:resourcekey="lbtnSortByDescription" 
                                    CommandName="Sort" CommandArgument="ShortDescription" />            
                                <asp:Image ID="imgSortDirectionDescription" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="ShortDescription" %>' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ShortDescription") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:Localize ID="locType" runat="server" meta:resourcekey="locType" />
                                <asp:LinkButton ID="lbtnSortByType" runat="server" meta:resourcekey="lbtnSortByType" 
                                    CommandName="Sort" CommandArgument="ObjectType" />            
                                <asp:Image ID="imgSortDirectionType" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="ObjectType" %>' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label id="lblObjectType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ObjectType") %>' />
                            </ItemTemplate>                     
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90" HeaderStyle-CssClass="mc_pc_grid_header_colLast">
                            <HeaderTemplate>
                                <asp:Localize ID="locNavigate" runat="server" meta:resourcekey="locNavigate" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btnNavigate" runat="server" meta:resourcekey="btnNavigate" CssClass="mc_pc_button mc_pc_btn_61" CommandName="Navigate" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CategoryId") +";"+DataBinder.Eval(Container.DataItem, "Id")+";"+DataBinder.Eval(Container.DataItem, "ObjectType") %>' />                            
                            </ItemTemplate>                    
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>    
            </td>
        </tr>
    </table>           
</div>
<script language="javascript" type="text/javascript">
    var txtKeywords = document.getElementById('<%= txtKeywords.ClientID %>');
    var ddlCategoryList = document.getElementById('<%= ddlCategoryList.ClientID %>');
    var txtAddedFrom = document.getElementById('<%= txtAddedFrom.ClientID %>');
    var chkRecursiveCategory = document.getElementById('<%= chkRecursiveCategory.ClientID %>');
    var txtAddedTo = document.getElementById('<%= txtAddedTo.ClientID %>');
    var revAddedFrom = document.getElementById('<%= revAddedFrom.ClientID %>');
    var revAddedTo = document.getElementById('<%= revAddedTo.ClientID %>');
    var cvValidPeriod = document.getElementById('<%= cvValidPeriod.ClientID %>');
    var txtPriceFrom = document.getElementById('<%= txtPriceFrom.ClientID %>');
    var txtPriceTo = document.getElementById('<%= txtPriceTo.ClientID %>');
    var cvPriceFrom = document.getElementById('<%= cvPriceFrom.ClientID %>');
    var cvPriceTo = document.getElementById('<%= cvPriceTo.ClientID %>');
    var cvComparePrices = document.getElementById('<%= cvComparePrices.ClientID %>');


    var chkCategory = document.getElementById('<%= cbxlObjectType.ClientID %>' + '_0');
    var chkProduct = document.getElementById('<%= cbxlObjectType.ClientID %>' + '_1');
    var chkCatalog = document.getElementById('<%= cbxlObjectType.ClientID %>' + '_2');
    var chkBundle = document.getElementById('<%= cbxlObjectType.ClientID %>' + '_3');
    var chkCollection = document.getElementById('<%= cbxlObjectType.ClientID %>' + '_4');
    var chkDiscount = document.getElementById('<%= cbxlObjectType.ClientID %>' + '_5'); 
    
    var chkCode = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_0'); 
	var chkName = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_1'); 
	var chkDescription = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_2');
	var chkTags = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_3');

	var ddlActive = document.getElementById('<%= ddlActive.ClientID %>');
	var ddlInStock = document.getElementById('<%= ddlInStock.ClientID %>');
	var ddlFeatured = document.getElementById('<%= ddlFeatured.ClientID %>');

	var ibtnOpenCalendarAddedFrom = document.getElementById('<%= ibtnOpenCalendarAddedFrom.ClientID %>');
	var ibtnOpenCalendarAddedTo = document.getElementById('<%= ibtnOpenCalendarAddedTo.ClientID %>');
</script>