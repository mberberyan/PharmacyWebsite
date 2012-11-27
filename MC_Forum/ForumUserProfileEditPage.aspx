<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForumUserProfileEditPage.aspx.cs"  Inherits="Melon.Components.Forum.UI.CodeBehind.ForumUserProfileEditPage"  ValidateRequest="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Update Profile</title>
	<link id="link" runat="server" type="text/css" href="ForumStyles/Styles.css" rel="stylesheet" rev="stylesheet" />
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="mc_forum_message" />
			<table cellpadding="0" cellspacing="0" class="mc_forum_table mc_forum_profile_table">
				<tr>
					<th colspan="2">
						<asp:Label ID="lblProfileDetailsTitle" runat="server" meta:resourcekey="lblProfileDetailsTitle" />
				</tr>
				<tr>
					<td valign="top" class="mc_forum_table_row1_padding">
						<asp:Image ID="imgPhoto" runat="server" meta:resourcekey="imgPhoto" /><br />
					</td>
					<td valign="top" class="mc_forum_table_row1_padding">
						<asp:FileUpload ID="cntrlFileUpload" runat="server" size="46px" CssClass="mc_forum_input_long"
							EnableTheming="false" /> <br />
							<asp:Label ID="lblPhotoInstructions" runat="server"/>
					   
						<br />
						<br />
						<asp:CheckBox ID="chkRemovePhoto" runat="server" meta:resourcekey="chkRemovePhoto" CssClass="mc_forum_checkbox" />
					</td>
				</tr>
				<tr>
					<td style="padding-top:10px;">
						<asp:Label ID="lblFirstNameTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblFirstNameTitle" />
					</td>
					<td style="padding-top:10px;">
						<asp:TextBox ID="txtFirstName" runat="server" CssClass="mc_forum_input_long" /></td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="lblLastNameTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblLastNameTitle" />
					</td>
					<td>
						<asp:TextBox ID="txtLastName" runat="server" CssClass="mc_forum_input_long" /></td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="lblEmailTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="lblEmailTitle" />
					</td>
					<td>
						<asp:TextBox ID="txtEmail" runat="server" CssClass="mc_forum_input_long" /></td>
				</tr>
				<tr>
					<td>
					</td>
					<td>
						<asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" CssClass="mc_forum_validator"
							ValidationExpression="^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" ControlToValidate="txtEmail"
							runat="server" meta:resourcekey="revEmail" /></td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="ICQNumberTitle" runat="server" CssClass="mc_forum_label" meta:resourcekey="ICQNumberTitle" /></td>
					<td>
						<asp:TextBox ID="txtICQNumber" runat="server" MaxLength="20" CssClass="mc_forum_input_short" />
					</td>
				</tr>
				<tr>
				    <td></td>
					<td>
						<asp:CheckBox ID="chkPublishProfileDetails" runat="server" meta:resourcekey="chkPublishProfileDetails" CssClass="mc_forum_checkbox" />
					</td>
				</tr>
				<tr>
					<td colspan="2" style="padding: 10px" align="right">
						<asp:Button ID="btnSave" runat="server" CssClass="mc_forum_button" meta:resourcekey="btnSave"
							OnClick="btnSave_Click" />
					</td>
				</tr>
			</table>
		</div>
	</form>
</body>
</html>
