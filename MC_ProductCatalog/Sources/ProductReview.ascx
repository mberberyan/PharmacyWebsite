<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductReview.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductReview" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>

<table class="mc_pc_table_listing" onkeydown="SetDefaultButton(document.getElementById(getName('input','btnSearch')), event)">
    <tr>        
        <td><asp:Localize ID="locKeywords" runat="server" meta:resourcekey="locKeywords" /></td>
        <td colspan="2"><asp:TextBox ID="txtKeywords" runat="server" CssClass="mc_pc_input_long" /></td>                
    </tr>
    <tr>
        <td><asp:Localize ID="locSearchInProduct" runat="server" meta:resourcekey="locSearchInProduct" /></td>
        <td>
            <asp:CheckBoxList ID="cbxlSearchCriteria" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                <asp:ListItem Text="<%$ Resources: Code %>" Value="Code" Selected="True" />
                <asp:ListItem Text="<%$ Resources: Name %>" Value="Name" Selected="True"/>
                <asp:ListItem Text="<%$ Resources: Description %>" Value="Description" Selected="True"/>
                <asp:ListItem Text="<%$ Resources: Review %>" Value="Review" Selected="True"/>
            </asp:CheckBoxList>
        </td>
    </tr>                
    <tr>
        <td><asp:Localize ID="locPostedBetween" runat="server" meta:resourcekey="locPostedBetween" /></td>
        <td colspan="2">
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
                                CssClass="mc_pc_validator" Display="Dynamic" meta:resourcekey="revAddedFrom" ValidationGroup="ProductReview"/>                                
                        </div>                            
                    </td>
                    <td  class="mc_pc_calendar_space"><span><asp:Localize ID="locAnd" runat="server" meta:resourcekey="locAnd" /></span></td>
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
                                CssClass="mc_pc_validator" Display="Dynamic" meta:resourcekey="revAddedTo" ValidationGroup="ProductReview"/>                                
                        </div>
                    </td>
                </tr>
                <tr>                        
                    <td colspan="3"><asp:CompareValidator ID="cvValidPeriod" runat="server" meta:resourcekey="cvValidPeriod" Display="Dynamic" ControlToValidate="txtAddedTo" ControlToCompare="txtAddedFrom" Operator="GreaterThanEqual" Type="Date" ValidationGroup="ProductReview"/></td>
                </tr>
            </table>                    
        </td>  
        <td align="right">                
            <asp:Button ID="btnReset" runat="server" meta:resourcekey="btnReset" CssClass="mc_pc_button mc_pc_btn_61" CausesValidation="false" OnClientClick="ResetSearchCriteria();return false;" />                
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSearch" runat="server" meta:resourcekey="btnSearch" CssClass="mc_pc_button mc_pc_btn_61" CausesValidation="true" ValidationGroup="ProductReview" />                                                        
        </td>
    </tr>      
    <tr>            
        
    </tr>
</table>

<table>
    <tr>
        <td>
            <!-- Pager for the grid view with product reviews-->
            <melon:Pager ID="TopPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="false" />
        </td>
    </tr>
    <tr>
        <td>
            <!-- *** Grid with found from product review search  *** -->
            <asp:GridView ID="gvProductReview" runat="server" 
                AutoGenerateColumns="False"
                EmptyDataText='<%$Resources:NoProductReviews %>'     
                GridLines="None"                        
                ShowHeader="true" 
                CssClass="mc_pc_grid"            
                HeaderStyle-CssClass="mc_pc_grid_header"
                RowStyle-CssClass="mc_pc_grid_row" 
                AlternatingRowStyle-CssClass="mc_pc_grid_alt_row"
                AllowPaging="true" 
                PagerSettings-Visible="false" 
                EmptyDataRowStyle-BackColor="#f7f7f7"
                AllowSorting="true"
                >      
            <Columns>
                <asp:TemplateField ItemStyle-Width="120" HeaderStyle-CssClass="mc_pc_grid_header_colFirst">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByProductCode" runat="server" meta:resourcekey="lbtnSortByProductCode" 
                            CommandName="Sort" CommandArgument="Code" />            
                        <asp:Image ID="imgSortDirectionFirstName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Code" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblProductCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code") %>'/>            
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="120" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnProductName" runat="server" meta:resourcekey="lbtnProductName" 
                            CommandName="Sort" CommandArgument="Name" />
                        <asp:Image ID="imgSortDirectionLastName" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Name" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="180" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByReview" runat="server" meta:resourcekey="lbtnSortByReview" 
                            CommandName="Sort" CommandArgument="Text" />
                        <asp:Image ID="imgSortDirectionReview" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Text" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblReview" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="60" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByRating" runat="server" meta:resourcekey="lbtnSortByRating" 
                            CommandName="Sort" CommandArgument="Rating" />
                        <asp:Image ID="imgSortDirectionRating" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Rating" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Image ID="imgRating" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="90" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnSortByDateTime" runat="server" meta:resourcekey="lbtnSortByDateTime" 
                            CommandName="Sort" CommandArgument="DatePosted" />
                        <asp:Image ID="imgSortDirectionDateTime" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                            ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="DatePosted" %>' />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDateTime" runat="server" Text='<%# Eval("DatePosted", "{0:MM/dd/yyyy}") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90" HeaderStyle-CssClass="mc_pc_grid_header_colLast">
                    <HeaderTemplate>
                        <asp:Localize ID="locNavigate" runat="server" meta:resourcekey="locNavigate" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button ID="btnView" runat="server" meta:resourcekey="btnView" CssClass="mc_pc_button mc_pc_btn_26"  CommandName="Navigate" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                        <asp:Button ID="btnRemove" runat="server" meta:resourcekey="btnRemove" CssClass="mc_pc_button mc_pc_btn_26" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteProductReview").ToString() + "\");" %>'/>                    
                    </ItemTemplate>                    
                </asp:TemplateField>
            </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>


<asp:Button ID="btnProductReview" runat="server" meta:resourcekey="btnProductReview" class="hidden"/>                                
<!-- Pop-up Preview -->
<ajaxToolkit:ModalPopupExtender ID="MPEProductReview" runat="server" BackgroundCssClass="mc_pc_popup_background"
    TargetControlID="btnProductReview" PopupControlID="divProductReview" BehaviorID="modalPopup" OkControlID="btnCloseProductReview" CancelControlID="btnCloseProductReview"
    DropShadow="false" Y="40" />
<asp:Panel ID="divProductReview" runat="server" style="display:none;">
    <div class="mc_pc_div_preview">
        <div class="mc_pc_div_border">
            <div>
                <center>
                        <span id="litAddAudioHeader" class="mc_pc_title_text"><asp:Localize ID="locProductReviewHeader" runat="server" meta:resourcekey="locProductReviewHeader" /></span>            
                </center>
                <table class="mc_pc_tabReview">
                    <col width="80px" />
                    <col width="520px" />
                    <tr>
                        <td><b><asp:Localize ID="locProductCode" runat="server" meta:resourcekey="locProductCode" /></b></td>
                        <td><asp:Label ID="lblProductCode" runat="server" /></td>	            
                    </tr>
                    <tr>
                        <td><b><asp:Localize ID="locProductName" runat="server" meta:resourcekey="locProductName" /></b></td>
                        <td><asp:Label ID="lblProductName" runat="server" /></td>	            
                    </tr>
                    <tr>
                        <td><b><asp:Localize ID="locName" runat="server" meta:resourcekey="locName" /></b></td>
                        <td><asp:Label ID="lblUserName" runat="server" /></td>	            
                    </tr>
                    <tr>
                        <td><b><asp:Localize ID="locRating" runat="server" meta:resourcekey="locRating" /></b></td>
                        <td>
                            <asp:DropDownList ID="ddlRating" runat="server">
                                <asp:ListItem Text="1" Value="1" />
                                <asp:ListItem Text="2" Value="2" />
                                <asp:ListItem Text="3" Value="3" />
                                <asp:ListItem Text="4" Value="4" />
                                <asp:ListItem Text="5" Value="5" />
                            </asp:DropDownList>            
                        </td>	            
                    </tr>
                    <tr>
                        <td><b><asp:Localize ID="locReview" runat="server" meta:resourcekey="locReview" /></b></td>
                        <td>                    
                            <asp:TextBox ID="txtReview" runat="server" TextMode="MultiLine" Columns="50" Rows="5" />                
                        </td>	            
                    </tr>	        
                    <tr>
                        <td colspan="2" align="right">                            
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Label ID="lblNoProductReview" runat="server" meta:resourcekey="lblNoProductReview" />
            <div class="mc_pc_div_close_preview">
            <asp:Button ID="btnSaveReview" runat="server" meta:resourcekey="btnSaveReview" CssClass="mc_pc_button mc_pc_btn_61" />
            <asp:Button ID="btnCloseProductReview" runat="server" meta:resourcekey="btnCloseProductReview" CssClass="mc_pc_button mc_pc_btn_61" />                
            </div>                
        </div>        
    </div>
</asp:Panel>


<script language="javascript" type="text/javascript">
    var txtKeywords = document.getElementById('<%= txtKeywords.ClientID %>');

    var chkCode = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_0');
    var chkName = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_1');
    var chkDescription = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_2');
    var chkReview = document.getElementById('<%= cbxlSearchCriteria.ClientID %>' + '_3');

    var txtAddedFrom = document.getElementById('<%= txtAddedFrom.ClientID %>');    
    var txtAddedTo = document.getElementById('<%= txtAddedTo.ClientID %>');
    var revAddedFrom = document.getElementById('<%= revAddedFrom.ClientID %>');
    var revAddedTo = document.getElementById('<%= revAddedTo.ClientID %>');
    var cvValidPeriod = document.getElementById('<%= cvValidPeriod.ClientID %>');

    var ibtnOpenCalendarAddedFrom = document.getElementById('<%= ibtnOpenCalendarAddedFrom.ClientID %>');
    var ibtnOpenCalendarAddedTo = document.getElementById('<%= ibtnOpenCalendarAddedTo.ClientID %>');
</script>