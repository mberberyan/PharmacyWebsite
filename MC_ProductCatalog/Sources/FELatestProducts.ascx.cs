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
    /// Web page lists latest added products in the component.
    /// </summary>
    public partial class CodeBehind_FELatestProducts : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Page size to show how many products are listed in Product List/Grid
        /// </summary>
        private PageItemNumberEnum _LatestProductsCount = PageItemNumberEnum.Two;
        public PageItemNumberEnum ProductsCount
        {
            get { return _LatestProductsCount; }
            set { _LatestProductsCount = value; }
        }

        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        protected override void OnInit(EventArgs e)
        {
            gvLatestProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvLatestProducts_RowDataBound);            
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
                LoadLatestProductsList();
            }
        }

        /// <summary>
        /// Load latest products
        /// </summary>
        /// <remarks>
        /// This method loads latest products and shows first <see cref="Melon.Components.ProductCatalog.Enumerations.PageItemNumberEnum"/> products.
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/02/2010</date>
        private void LoadLatestProductsList()
        {            
            DataTable dtLatestProductsList = new DataTable();
            divLatestProductsError.Visible = false;
            gvLatestProducts.Visible = true;
            try
            {
                dtLatestProductsList = Product.ListLatestProducts(Convert.ToInt16(_LatestProductsCount), true);
            }
            catch (ProductCatalogException e)
            {
                gvLatestProducts.Visible = false;
                divLatestProductsError.Visible = true;
                divLatestProductsError.InnerHtml = e.Message;
                return;
            }

            //Display details of latest products                        
            gvLatestProducts.DataSource = dtLatestProductsList;
            gvLatestProducts.DataBind();
        }

        /// <summary>
        /// Loads latest product details.
        /// </summary>
        /// <remarks>
        /// This method loads product information - product name, main image in thumbnail format, 
        /// product`s lowest price (based on applied active discounts) and link for navigation to product details page.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLatestProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

                HyperLink hplLatestProduct = (HyperLink)e.Row.FindControl("hplLatestProduct");
                hplLatestProduct.NavigateUrl = aName.HRef;
            }
        }
    }
}
