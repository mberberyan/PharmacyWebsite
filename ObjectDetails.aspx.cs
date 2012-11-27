using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.UI.CodeBehind;
using Melon.Components.ProductCatalog;

public partial class MC_ProductCatalog_Example_ObjectDetails : System.Web.UI.Page
{
    #region Fields && Properties
    public string ObjectType
    {
        get 
        {
            if (ViewState["mc_pc_objType"] == null)
            {
                return String.Empty;
            }

            return (string)ViewState["mc_pc_objType"];
        }

        set { ViewState["mc_pc_objType"] = value; }
    }

    public int? ObjectId
    {
        get
        {
            if (ViewState["mc_pc_objId"] == null)
            {
                return (int?)null;
            }

            return (int?)ViewState["mc_pc_objId"];
        }

        set { ViewState["mc_pc_objId"] = value; }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadObjectDetailsControl();
        }
    }

    private void LoadObjectDetailsControl()
    {
        ObjectType = Request.QueryString["objType"];
        ObjectId = Convert.ToInt32(Utilities.HashToStr(Request.QueryString["objId"]));

        if (ObjectType==String.Empty || ObjectId==(int?)null)
        {
            divNoObject.Visible = true;
            return;
        }

        ucProductDetails.Visible = ObjectType == "ProductList" || ObjectType == "ProductGrid";
        ucCatalogDetails.Visible = ObjectType == "CatalogList";
        ucBundleDetails.Visible = ObjectType == "BundleList";
        ucCollectionDetails.Visible = ObjectType == "CollectionList";

        // load object details control
        switch (ObjectType)
        { 
            case "ProductList":
            case "ProductGrid":
                ucProductDetails.ProductId = ObjectId;
                break;
            case "CatalogList":
                ucCatalogDetails.CatalogId = ObjectId;
                break;
            case "BundleList":
                ucBundleDetails.BundleId = ObjectId;
                break;
            case "CollectionList":
                ucCollectionDetails.CollectionId = ObjectId;
                break;
        }
    }    
}
