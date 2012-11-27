using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MPDefault : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cntrlFeaturedNews.Language = System.Threading.Thread.CurrentThread.CurrentCulture;
            cntrlFeaturedNews.DataBind();
        }
    }
}
