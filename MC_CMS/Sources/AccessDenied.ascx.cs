using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.CMS;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Configuration;


/// <summary>
/// Display message to user for his current rights in the CMS system.
/// </summary>
public partial class AccessDenied :CMSControl
{
    #region Properties

    /// <summary>
    /// User role of user for whom access is denied.
    /// </summary>
    public CMSRole UserRole;

    /// <summary>
    /// Flag whether it is specified role in <see cref="UserRole"/> is the role 
    /// with which the user initially logged to CMS system or it was changed by another user while working with the CMS system. 
    /// </summary>
    public bool IsUserLoggedRole;

    #endregion

    /// <summary>
    /// Attach event handlers to the controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>01/07/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.lbtnLogin.Click += new EventHandler(lbtnLogin_Click);
        base.OnInit(e);
    }

    /// <summary>
    /// Display message to user for his current rights in the CMS system.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            if (String.IsNullOrEmpty(CMSSettings.LoginUrl) || this.ResolveUrl(CMSSettings.LoginUrl) == Request.RawUrl)
            {
                lbtnLogin.Visible = false;
                lblInstruction.Visible = false;
            }
            else
            {
                lbtnLogin.Visible = true;
                lblInstruction.Visible = true;
            }

            if (UserRole == CMSRole.None && IsUserLoggedRole)
            {
                lblMessage.Text = Convert.ToString(GetLocalResourceObject("AccessDenied"));
            }
            else
            {
                if (UserRole == CMSRole.None)
                {
                    lblMessage.Text = Convert.ToString(GetLocalResourceObject("UserRoleChangedToNone"));
                }
                else
                {
                    lblMessage.Text = String.Format(Convert.ToString(GetLocalResourceObject("UserRoleChanged")), Convert.ToString(GetLocalResourceObject(this.UserRole.ToString())));
                }
            }

        }
    }

    /// <summary>
    /// Sign out current user and redirect to login pages.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>01/07/2008</date>
    protected void lbtnLogin_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        this.ParentControl.RedirectToLoginPage();
    }

	/// <summary>
	/// Initializes the control's properties
	/// </summary>
	/// <param name="args">The values with which the properties will be initialized</param>
	public override void Initializer(object[] args)
    {
		this.UserRole = (CMSRole)args[0];
		this.IsUserLoggedRole = (bool)args[1];
    }
}

