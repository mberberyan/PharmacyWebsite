using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Melon.Components.ProductCatalog.Enumerations;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page lists most-viewed products in the component.
    /// </summary>
    public partial class CodeBehind_FETopProducts : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Page size to show how many products are listed Top Products
        /// </summary>
        private PageItemNumberEnum _TopProductsCount = PageItemNumberEnum.Two;
        public PageItemNumberEnum ProductsCount
        {
            get { return _TopProductsCount; }
            set { _TopProductsCount = value; }
        }

        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        protected override void OnInit(EventArgs e)
        {
            gvTopProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvTopProducts_RowDataBound);
        }

        /// <summary>
        /// Initializes user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTopProductsList();
            }
        }

        /// <summary>
        /// Load top products
        /// </summary>
        /// <remarks>
        /// This method loads most-viewed products and shows first <see cref="_TopProductsCount"/> products.
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/17/2010</date>
        private void LoadTopProductsList()
        {
            DataTable dtTopProductsList = new DataTable();
            divTopProductsError.Visible = false;
            gvTopProducts.Visible = true;
            try
            {
                dtTopProductsList = Product.ListTopProducts(Convert.ToInt16(_TopProductsCount), true);
            }
            catch (ProductCatalogException e)
            {
                gvTopProducts.Visible = false;
                divTopProductsError.Visible = true;
                divTopProductsError.InnerHtml = e.Message;
                return;
            }

            //Display details of top products                        
            gvTopProducts.DataSource = dtTopProductsList;
            gvTopProducts.DataBind();
        }

        /// <summary>
        /// Loads top product details.
        /// </summary>
        /// <remarks>
        /// This method loads product information - product name, main image in thumbnail format, 
        /// product`s lowest price (based on applied active discounts) and link for navigation to product details page.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTopProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                HtmlAnchor aName = (HtmlAnchor)e.Row.FindControl("aName");
                aName.HRef = "~/ObjectDetails.aspx?objType=ProductList&objId=" + Utilities.StrToHash(drv["Id"].ToString());
                aName.InnerText = drv["Name"].ToString();

                Label lblLowestPrice = (Label)e.Row.FindControl("lblLowestPrice");

                if (drv["StartPrice"] != DBNull.Value)
                {
                    lblLowestPrice.Visible = true;
                    lblLowestPrice.Text = GetLocalResourceObject("StartingFrom").ToString() + " " + drv["StartPrice"].ToString() + " " + ProductCatalogSettings.Currency;
                }
                else
                {
                    lblLowestPrice.Visible = false;
                }

                Image imgProduct = (Image)e.Row.FindControl("imgProduct");
                string imageUrl = drv["ImagePath"].ToString();

                if (imageUrl == String.Empty)
                {
                    imgProduct.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
                }
                else
                {
                    imgProduct.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }

                HyperLink hplTopProduct = (HyperLink)e.Row.FindControl("hplTopProduct");
                hplTopProduct.NavigateUrl = aName.HRef;
            }
        }
    }
}
