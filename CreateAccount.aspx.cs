using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Profile;

public partial class CreateAccount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnInit(EventArgs e)
    {
        this.btnCreate.Click += new EventHandler(btnCreate_Click);
        base.OnInit(e);
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            MembershipUser user = Membership.CreateUser(txbUserName.Text.Trim(), txbPassword.Text, txbEmail.Text.Trim());

            ProfileBase profile = ProfileCommon.Create(user.UserName);
            profile.SetPropertyValue("MC_FirstName", txbFirstName.Text.Trim());
            profile.SetPropertyValue("MC_LastName", txbLastName.Text.Trim());
            profile.Save();
        }
        catch (MembershipCreateUserException ex)
        {
            lblErrorMessage.Text = GetErrorMessage(ex.StatusCode);
            lblErrorMessage.Visible = true;
            return;
        }
        catch
        {
            lblErrorMessage.Text = "Error occured while trying create account.";
            lblErrorMessage.Visible = true;
            return;
        }

        Response.Redirect("RegisterSuccess.aspx");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Login.aspx");
    }

    private string GetErrorMessage(MembershipCreateStatus status)
    {
        switch (status)
        {
            case MembershipCreateStatus.DuplicateUserName:
                return "Username already exists. Please enter a different user name.";

            case MembershipCreateStatus.DuplicateEmail:
                return "A username for that e-mail address already exists. Please enter a different e-mail address.";

            case MembershipCreateStatus.InvalidPassword:
                return "The password provided is invalid. Please enter a valid password value.";

            case MembershipCreateStatus.InvalidEmail:
                return "The e-mail address provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.InvalidUserName:
                return "The user name provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.ProviderError:
                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

            case MembershipCreateStatus.UserRejected:
                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

            default:
                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        }
    }
}
