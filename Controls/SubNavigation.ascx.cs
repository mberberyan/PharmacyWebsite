using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;

public partial class Controls_SubNavigation : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SiteMap.CurrentNode != null)
            {
                dsSubMenu.StartingNodeUrl = Utility.GetFirstLevelParentUrl(SiteMap.CurrentNode);
                cntrlSubMenu.Visible = true;
            }
            else
            {
                cntrlSubMenu.Visible = false;
            }
            
        }
    }
}
