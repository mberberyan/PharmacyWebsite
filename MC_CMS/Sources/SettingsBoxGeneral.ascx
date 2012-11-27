<%@ Control Language="C#" AutoEventWireup="true" Inherits="SettingsBoxGeneral" CodeFile="SettingsBoxGeneral.ascx.cs" %>
<%@ Import Namespace="Melon.Components.CMS" %>
<%@ Import Namespace="Melon.Components.CMS.Configuration" %>

<div class="mc_cms_first_section">
    <h3>
        <asp:Label ID="lblGeneral" runat="server" CssClass="mc_cms_heading" meta:resourcekey="lblGeneral"/>
    </h3>
    <div class="mc_cms_settings_section">
        <table cellpadding="2" cellspacing="0" width="100%">
            <!-- Location -->
            <tr>
                <td class="mc_cms_settings_label_column">
                    <asp:Label ID="lblLocation" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblLocation"/></td>
                <td colspan="2">
                    <asp:Label ID="lblLocationPath" runat="server" CssClass="mc_cms_static_text"/></td>
            </tr>
            <!-- Code -->
            <tr>
                <td class="mc_cms_settings_label_column">
                    <asp:Label ID="lblCode" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblCode"/>
                    <span class="mc_cms_validator">*</span></td>
                <td colspan="2">
                    <asp:TextBox ID="txtCode" runat="server" CssClass="mc_cms_input_short" MaxLength="50"/>
                    <asp:Label ID="lblCodeUniqueReminder" runat="server" CssClass="mc_cms_comment" meta:resourcekey="lblCodeUniqueReminder"/></td>
            </tr>
            <tr>
                <td class="mc_cms_settings_label_column"></td>
                <td colspan="2">
                    <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="rfvCode" ValidationGroup="NodeSettings"/>
                    <asp:RegularExpressionValidator ID="revCode" runat="server" ControlToValidate="txtCode" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revCode" ValidationGroup="NodeSettings"
                        ValidationExpression='[a-zA-Z0-9_]*' /></td>
            </tr>            
            <!-- Alias -->
            <tr>
                <td class="mc_cms_settings_label_column">
                    <asp:Label ID="lblAlias" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblAlias"/>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtAlias" runat="server" CssClass="mc_cms_input_short" MaxLength="50"/>
                    <asp:Label ID="lblAliasUniqueReminder" runat="server" CssClass="mc_cms_comment" meta:resourcekey="lblAliasUniqueReminder"/></td>
            </tr>
            <tr>
                <td class="mc_cms_settings_label_column"></td>
                <td colspan="2">                    
                    <asp:RegularExpressionValidator ID="revAlias" runat="server" ControlToValidate="txtAlias" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revAlias" ValidationGroup="NodeSettings"
                        ValidationExpression='[a-zA-Z0-9_]*' /></td>
            </tr>
            <!-- Title -->
            <tr>
                <td class="mc_cms_settings_label_column">
                    <asp:Label ID="lblTitle" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblTitle"/>
                    <span class="mc_cms_validator">*</span></td>
                <td colspan="2">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="mc_cms_input_short" MaxLength="256"/></td>
            </tr>
            <tr>
                <td class="mc_cms_settings_label_column"></td>
                <td colspan="2">
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="rfvTitle" ValidationGroup="NodeSettings" />
                    <asp:RegularExpressionValidator ID="revTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revTitle" ValidationGroup="NodeSettings" 
                        ValidationExpression="[a-zA-Zа-яА-Я0-9\'\x22\-_\s]*"/></td>
            </tr>
            <!-- Normal Image -->
            <tr>
                <td valign="top" class="mc_cms_settings_label_column">
                    <asp:Label ID="lblImage" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblImage"/></td>
                <td class="mc_cms_settings_textbox_column">
                    <asp:FileUpload ID="fuImage" runat="server" CssClass="mc_cms_input_file" size="52"/>
                    <div id="divImage" runat="server">
                        <asp:Image ID="imgImage" runat="server"/>
                        <asp:CheckBox ID="chkRemoveImage" runat="server" meta:resourcekey="chkRemoveImage"/>
                    </div>
                </td>
                <td valign="top" rowspan="3">
                    <asp:Label ID="lblImageSettingsReminder" runat="server" CssClass="mc_cms_comment"/>
                </td>
            </tr>
            
            <tr>
                <td class="mc_cms_settings_label_column"></td>
                <td colspan="2">
                    <asp:RegularExpressionValidator ID="revImage" runat="server" ControlToValidate="fuImage" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revImage" ValidationGroup="NodeSettings"
                        ValidationExpression="(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)" />
                    <asp:Label ID="lblImageError" runat="server" CssClass="mc_cms_error_message" Visible="false" /></td>
            </tr>
            
            <!-- Hover Image -->
            <tr>
                <td valign="top" class="mc_cms_settings_label_column">
                    <asp:Label ID="lblHoverImage" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblHoverImage"/></td>
                <td>
                    <asp:FileUpload ID="fuHoverImage" runat="server" CssClass="mc_cms_input_file" size="52"/>
                    <div id="divHoverImage" runat="server">
                        <asp:Image ID="imgHoverImage" runat="server"/>
                        <asp:CheckBox ID="chkRemoveHoverImage" runat="server" meta:resourcekey="chkRemoveImage"/>
                    </div>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="mc_cms_settings_label_column"></td>
                <td colspan="2">
                    <asp:RegularExpressionValidator ID="revHoverImage" runat="server" ControlToValidate="fuHoverImage" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revHoverImage" ValidationGroup="NodeSettings"
                        ValidationExpression="(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)" />
                    <asp:Label ID="lblHoverImageError" runat="server" CssClass="mc_cms_error_message" Visible="false"/></td>
            </tr>
            <!-- Selected Image -->
            <tr id="trSelectedImage" runat="server">
                <td valign="top" class="mc_cms_settings_label_column">
                    <asp:Label ID="lblSelectedImage" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblSelectedImage"/></td>
                <td>
                    <asp:FileUpload ID="fuSelectedImage" runat="server" CssClass="mc_cms_input_file" size="52"/>
                    <div id="divSelectedImage" runat="server">
                        <asp:Image ID="imgSelectedImage" runat="server"/>
                        <asp:CheckBox ID="chkRemoveSelectedImage" runat="server" meta:resourcekey="chkRemoveImage"/>
                    </div>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="mc_cms_settings_label_column"></td>
                <td colspan="2">
                    <asp:RegularExpressionValidator ID="revSelectedImage" runat="server" ControlToValidate="fuSelectedImage" Display="Dynamic" SetFocusOnError="true"
                        CssClass="mc_cms_validator" meta:resourcekey="revSelectedImage" ValidationGroup="NodeSettings"
                        ValidationExpression="(.*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP]|[pP][nN][gG])$)" />
                    <asp:Label ID="lblSelectedImageError" runat="server" CssClass="mc_cms_error_message" Visible="false"/></td>
            </tr>
            <!-- Hide in navigation -->
            <tr>
                <td class="mc_cms_settings_label_column">
                    <asp:Label ID="lblHideInNavigation" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblHideInNavigation"/></td>
                <td colspan="2">
                    <asp:CheckBox ID="chkHideInNavigation" runat="server" CssClass="mc_cms_navigation_checkbox"/></td>
            </tr>
             <!-- Target -->
            <tr id="trTarget" runat="server">
                <td class="mc_cms_settings_label_column" >
                    <asp:Label ID="Label1" runat="server" CssClass="mc_cms_label" meta:resourcekey="lblTarget"/></td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rdolTarget" runat="server" RepeatDirection="Horizontal" CssClass="mc_cms_target_options">
                        <asp:ListItem Value="_self" Text='<%$Resources:TargetSelf %>' Selected="true"/>
                        <asp:ListItem Value="_blank" Text='<%$Resources:TargetBlank %>' />
                        <asp:ListItem Value="frame" Text='<%$Resources:TargetFrame %>' />
                    </asp:RadioButtonList>
                   <span style="vertical-align:top;">
                    <asp:TextBox ID="txtFrameName" runat="server" CssClass="mc_cms_input_short" MaxLength="50"/>
                    <span class="mc_cms_validator">*</span>
                    <asp:RequiredFieldValidator ID="rfvFrameName" runat="server" ControlToValidate="txtFrameName" 
                        Display="Dynamic" ValidationGroup="NodeSettings" CssClass="mc_cms_validator" meta:resourcekey="rfvFrameName"/>
                   </span>
                </td>
            </tr>  
            <tr>
                <td></td>
                <td colspan="2">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td></td>
                            <td>
                            <asp:RegularExpressionValidator ID="revFrameName" runat="server" ControlToValidate ="txtFrameName"
                                Display="Dynamic" ValidationGroup="NodeSettings" CssClass="mc_cms_validator" meta:resourcekey="revFrameName"
                                ValidationExpression='[a-zA-Z0-9_]*'/>    
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                   <asp:Label ID="lblErrorMessage" runat="server" CssClass="mc_cms_error_message" /></td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript" language="javascript">

var rfvFrameName = document.getElementById('<%=rfvFrameName.ClientID%>');
var revFrameName = document.getElementById('<%=revFrameName.ClientID%>');
var txtFrameName = document.getElementById('<%=txtFrameName.ClientID%>');

</script>