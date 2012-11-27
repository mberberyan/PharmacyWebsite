using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Data;
using Melon.Components.ProductCatalog.Enumerations;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays catalog details information
    /// </summary>
    public partial class CodeBehind_FECatalogDetails : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Identifier of selected catalog object
        /// </summary>
        public int? CatalogId
        {
            get
            {
                if (ViewState["mc_pc_CatalogId"] == null)
                {
                    return (int?)null;
                }

                return (int?)ViewState["mc_pc_CatalogId"];
            }

            set { ViewState["mc_pc_CatalogId"] = value; }
        }

        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>       
        protected override void OnInit(EventArgs e)
        {
            gvCatalogProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvCatalogProducts_RowDataBound);
            CatalogProductsPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(CatalogProductsPager_PageChanged);            

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes user control information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCatalogDetails();
            }
        }

        /// <summary>
        /// Loads catalog details information on web page and calls method to load catalog product list.
        /// </summary>
        private void LoadCatalogDetails()
        {
            if (CatalogId == null)
            {
                return;
            }

            Catalog catalog = Catalog.Load(CatalogId.Value);

            lblCatalogName.Text = catalog.Name;
            lblLongDescription.Text = catalog.LongDescription;
            
            LoadCatalogProductList();
        }

        /// <summary>
        /// Loads all products for the current catalog object.
        /// </summary>
        /// <remarks>
        /// Loads all products with their names, categories and price values in a table format.
        /// </remarks>
        private void LoadCatalogProductList()
        {
            CatalogProducts catProducts = new CatalogProducts();
            catProducts.CatalogId = CatalogId;
            catProducts.IsActive = true;
            DataTable dtCatalogProductList = catProducts.List();

            //Display details of found products
            DataView dvCatalogProductsList = new DataView(dtCatalogProductList);
            if (dtCatalogProductList.Rows.Count != 0)
            {
                dvCatalogProductsList.Sort = "Name ASC";
            }

            gvCatalogProducts.PageSize = Convert.ToInt16(PageItemNumberEnum.Six);
            gvCatalogProducts.DataSource = dvCatalogProductsList;
            gvCatalogProducts.DataBind();

            //Display paging if there are products found.
            if (dtCatalogProductList.Rows.Count != 0)
            {
                gvCatalogProducts.Visible = true;
                CatalogProductsPager.Visible = true;
                CatalogProductsPager.FillPaging(gvCatalogProducts.PageCount, gvCatalogProducts.PageIndex + 1, 5, gvCatalogProducts.PageSize, dtCatalogProductList.Rows.Count);
            }
            else
            {
                gvCatalogProducts.Visible = false;
                CatalogProductsPager.Visible = false;
            }
        }

        /// <summary>
        /// Initializes catlaog products` details in products listing
        /// </summary>
        /// <remarks>This method sets products` image file paths, common prices and links to product details pages</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCatalogProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                HyperLink hplProduct = (HyperLink)e.Row.FindControl("hplProduct");
                hplProduct.NavigateUrl = "~/ObjectDetails.aspx?objType=ProductList&objId=" + Utilities.StrToHash(drv["ProductId"].ToString());

                HyperLink lbName = (HyperLink)e.Row.FindControl("lbName");
                lbName.NavigateUrl = hplProduct.NavigateUrl;

                Image imgProduct = (Image)e.Row.FindControl("imgProduct");
                string imageUrl = drv["ImagePath"].ToString();

                if (imageUrl == String.Empty)
                {
                    imgProduct.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc); ;
                }
                else
                {
                    imgProduct.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }

                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                if (drv["CommonPrice"] != DBNull.Value)
                {
                    lblPrice.Text = drv["CommonPrice"].ToString() + " " + ProductCatalogSettings.Currency;
                }
            }
        }

        /// <summary>
        /// Performs page changing for catalog products list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CatalogProductsPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            gvCatalogProducts.PageIndex = e.NewPage;
            LoadCatalogProductList();
        }

        /// <summary>
        /// Navigates back to catalog listing page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ObjectList.aspx?objType=CatalogList");
        }
    }
}
