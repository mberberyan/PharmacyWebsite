using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MPProducts : BaseMasterPageControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();

    }

    protected override void AddedControl(Control control, int index)
    {
        // Adding this override so that the asp:Menu control renders properly in Safari and Chrome
        if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1)
        {
            this.Page.ClientTarget = "uplevel";
        }

        base.AddedControl(control, index);
    }

}
