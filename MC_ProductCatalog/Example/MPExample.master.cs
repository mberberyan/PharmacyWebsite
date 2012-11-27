using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    public partial class MPExample : BaseMasterPageControl
    {
        protected override void AddedControl(Control control, int index)
        {
            // Adding this override so that the asp:Menu control renders properly in Safari and Chrome
            if (Request.ServerVariables["http_user_agent"].IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) != -1) 
            { 
                this.Page.ClientTarget = "uplevel"; 
            }

            base.AddedControl(control, index);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
        }
    }
}