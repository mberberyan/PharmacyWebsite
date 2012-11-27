<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscountInformation.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_DiscountInformation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<div id="divMessage" runat="server" class="mc_pc_success_message">
    
</div>
<div>    
    <table class="mc_pc_discountTable" >
        <col width="150px"/>
        <col width="550px"/>
        <tr>
            <td><asp:Localize ID="locName" runat="server" meta:resourcekey="locName" /><span class="mc_pc_star">*</span></td>
            <td>
                <asp:TextBox ID="txtName" runat="server" CssClass="mc_pc_input_short"/>
                <div>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" meta:resourcekey="rfvName" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="Discount" />
                </div>
                <div>
                    <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName" Display="Dynamic"
                        CssClass="mc_pc_validator" meta:resourcekey="revName" ValidationExpression='[^,]+' ValidationGroup="Discount"/>
                </div>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locDescription" runat="server" meta:resourcekey="locDescription" /><span class="mc_pc_star">*</span></td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="mc_pc_input_short"/>
                <div>
                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" Display="Dynamic" meta:resourcekey="rfvDescription" CssClass="mc_pc_validator" ValidationGroup="Discount"/>
                </div>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locDiscountIn" runat="server" meta:resourcekey="locDiscountIn" /></td>
            <td>
                <asp:RadioButtonList ID="rblDiscountIn" runat="server" RepeatDirection="Horizontal" >                                                    
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locDiscountPeriod" runat="server" meta:resourcekey="locDiscountPeriod" /><span class="mc_pc_star">*</span></td>
            <td>
                <table>
                    <tr>
                        <td>
                            <div class="mc_pc_divDate">
                                <asp:TextBox ID="txtDiscountFrom" runat="server" MaxLength="10" CssClass="mc_pc_input_short" onclick="javascript:ibtnOpenCalendarDiscountFrom.click(); return false;" /><br />
                                <asp:Label ID="lblComment" runat="server" CssClass="mc_pc_comment" meta:resourcekey="lblComment" /><br />
                            </div>
                            <div class="left">
                                <asp:ImageButton ID="ibtnOpenCalendarDiscountFrom" runat="server" />
                                <ajaxToolkit:CalendarExtender ID="ceDiscountFrom" runat="server" Format="MM/dd/yyyy"
                                    TargetControlID="txtDiscountFrom" PopupButtonID="ibtnOpenCalendarDiscountFrom" Enabled="True"
                                    CssClass="ajax__calendar mc_pc_calendar" />
                                <!-- Time Posted -->                                
                            </div>
                            <div class="clear">
                                <asp:RequiredFieldValidator ID="rfvDiscountDateFrom" runat="server" ControlToValidate="txtDiscountFrom" meta:resourcekey="rfvDiscountDateFrom" CssClass="mc_pc_validator"  Display="Dynamic" ValidationGroup="Discount" />
                                <asp:RegularExpressionValidator ID="revDiscountFrom" runat="server" ControlToValidate="txtDiscountFrom"
                                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                    CssClass="mc_pc_validator" Display="Dynamic" meta:resourcekey="revDiscountFrom" ValidationGroup="Discount"/>                                
                            </div>                            
                        </td>
                        <td class="mc_pc_calendar_space"><span><asp:Localize ID="locAnd" runat="server" meta:resourcekey="locAnd" /></span></td>
                        <td>
                            <div class="mc_pc_divDate">
                                <asp:TextBox ID="txtDiscountTo" runat="server" MaxLength="10" CssClass="mc_pc_input_short" onclick="javascript:ibtnOpenCalendarDiscountTo.click(); return false;"/><br />
                                <asp:Label ID="lblCommentDiscountTo" runat="server" CssClass="mc_pc_comment" meta:resourcekey="lblCommentDiscountTo" /><br />
                            </div>
                            <div class="left">
                                <asp:ImageButton ID="ibtnOpenCalendarDiscountTo" runat="server" />
                                <ajaxToolkit:CalendarExtender ID="ceDiscountTo" runat="server" Format="MM/dd/yyyy"
                                    TargetControlID="txtDiscountTo" PopupButtonID="ibtnOpenCalendarDiscountTo" Enabled="True"
                                    CssClass="ajax__calendar mc_pc_calendar" />
                                <!-- Time Posted -->                                
                            </div>
                            <div class="clear">
                                <asp:RequiredFieldValidator ID="rfvDiscountTo" runat="server" ControlToValidate="txtDiscountTo" meta:resourcekey="rfvDiscountTo" CssClass="mc_pc_validator"  Display="Dynamic" ValidationGroup="Discount" />
                                <asp:RegularExpressionValidator ID="revDiscountTo" runat="server" ControlToValidate="txtDiscountTo"
                                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/][2-9]\d{3}"
                                    CssClass="mc_pc_validator" Display="Dynamic" meta:resourcekey="revDiscountTo" ValidationGroup="Discount"/>                                
                            </div>
                        </td>
                    </tr>
                    <tr>                        
                        <td colspan="3"><asp:CompareValidator ID="cvValidPeriod" runat="server" Display="Dynamic" meta:resourcekey="cvValidPeriod" ControlToValidate="txtDiscountTo" ControlToCompare="txtDiscountFrom" CssClass="mc_pc_validator" Operator="GreaterThanEqual" Type="Date" ValidationGroup="Discount"/></td>
                    </tr>
                </table>                    
            </td>
        </tr>    
        <tr>
            <td><asp:Localize ID="locDiscountValue" runat="server" meta:resourcekey="locDiscountValue" /><span class="mc_pc_star">*</span></td>
            <td>
                <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="mc_pc_input_extra_short"/> <asp:Label ID="lblDiscountType" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvDiscountValue" runat="server" ControlToValidate="txtDiscountValue" meta:resourcekey="rfvDiscountValue" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="Discount"/>
                    <asp:CompareValidator ID="cvDiscountValue" runat="server" meta:resourcekey="cvDiscountValue" ControlToValidate="txtDiscountValue" Display="Dynamic" CssClass="mc_pc_validator"  Type="Double" Operator="DataTypeCheck" ValidationGroup="Discount"/>
                    <asp:CustomValidator ID="cvDiscountPercent" runat="server" meta:resourcekey="cvDiscountPercent" ClientValidationFunction="CheckDiscountValue" Display="Dynamic" ValidationGroup="Discount" />
                </div>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locIsActive" runat="server" meta:resourcekey="locIsActive" /></td>
            <td><asp:CheckBox ID="chkIsActive" runat="server" /></td>
        </tr>
    </table>    
</div>
<script type="text/javascript" language="javascript">
    var ibtnOpenCalendarDiscountFrom = document.getElementById('<%= ibtnOpenCalendarDiscountFrom.ClientID %>');
    var ibtnOpenCalendarDiscountTo = document.getElementById('<%= ibtnOpenCalendarDiscountTo.ClientID %>');
    var lblDiscountType = document.getElementById('<%= lblDiscountType.ClientID %>');    
</script>