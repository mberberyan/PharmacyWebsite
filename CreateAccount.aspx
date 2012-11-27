<%@ Page Title="" Language="C#" MasterPageFile="~/MPBase.master" AutoEventWireup="true" CodeFile="CreateAccount.aspx.cs" Inherits="CreateAccount" Theme="Default" %>

<asp:Content ID="cBase" ContentPlaceHolderID="cphBase" Runat="Server">
    <div class="main_login_div">
        <div class="login_div">
            <h1>
                Create New Account
            </h1>            
            <table>
                <tr>
                    <td>
                        UserName:<span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txbUserName" runat="server" CssClass="input"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txbUserName"
                            ErrorMessage="Enter username." ValidationGroup="AccountDetails" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Password:<span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txbPassword" runat="server" CssClass="input" TextMode="Password"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txbPassword"
                            ErrorMessage="Enter password." ValidationGroup="AccountDetails" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Confirm Password:<span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txbConfirmPassword" runat="server" CssClass="input" TextMode="Password"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txbConfirmPassword"
                            ErrorMessage="Re-Enter password." ValidationGroup="AccountDetails" Display="Dynamic" />
                        <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txbConfirmPassword"
                            ControlToCompare="txbPassword" ErrorMessage="The password and confirm password fields do not match."
                            ValidationGroup="AccountDetails" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td>
                        First Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txbFirstName" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Last Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txbLastName" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email:<span style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txbEmail" runat="server" CssClass="input"></asp:TextBox><br />
                         <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txbEmail"
                            ErrorMessage="Enter email." ValidationGroup="AccountDetails" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ValidationExpression="^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" ControlToValidate="txbEmail"
                            ErrorMessage="Invalid email address." ValidationGroup="AccountDetails"  Display="Dynamic"/>
                    </td>
                </tr>
                
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn_61"/>
                        <asp:Button ID="btnCreate" runat="server" Text="Create" ValidationGroup="AccountDetails" CssClass="btn_61"
                            CausesValidation="true" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="error_message"/>
        </div>        
    </div>
</asp:Content>

