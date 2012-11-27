<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login"
    Theme="Default" MasterPageFile="~/MPBase.master" %>

<asp:Content ID="cBase" ContentPlaceHolderID="cphBase" runat="server">
    <div class="main_login_div">
        <div class="login_div">
            <h1>
                Log In</h1>
            <asp:Login ID="LoginAdmin" runat="server" DestinationPageUrl="~/Default.aspx">
                <LayoutTemplate>
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="UserName" CssClass="input" runat="server" />
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="vgLogin">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Password" CssClass="input" runat="server" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="vgLogin">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="color: Red;">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2" style="padding: 0 20px 0 0;">
                                <asp:Button ID="LoginButton" CssClass="btn_61" runat="server" CommandName="Login"
                                    Text="Log In" ValidationGroup="vgLogin" />
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:Login>            
            <div class="create_account_div">
                <a href="CreateAccount.aspx">Create account</a>        
            </div>            
        </div>
    </div>
</asp:Content>
