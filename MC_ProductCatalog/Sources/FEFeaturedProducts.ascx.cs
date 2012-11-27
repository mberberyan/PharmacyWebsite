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
    /// Web page lists all products marked as featured in the component.
    /// </summary>
    public partial class CodeBehind_FEFeaturedProducts : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Page size to show how many products are listed in Product List/Grid
        /// </summary>
        private PageItemNumberEnum _ProductsPageSize = PageItemNumberEnum.Two;
        public PageItemNumberEnum ProductsPageSize
        {
            get { return _ProductsPageSize; }
            set { _ProductsPageSize = value; }
        }

        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        protected override void OnInit(EventArgs e)
        {
            gvFeaturedProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvFeaturedProducts_RowDataBound);            
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
                LoadFeaturedProductsList();
            }
        }

        /// <summary>
        /// Loads featured products.
        /// </summary>
        /// <remarks>
        /// This method loads all products marked as featured and shows first <see cref="Melon.Components.ProductCatalog.Enumerations.PageItemNumberEnum"/> products.
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/02/2010</date>
        private void LoadFeaturedProductsList()
        {
            ProductSearchCriteria searchCriteria = new ProductSearchCriteria();
            searchCriteria.ActiveOnly = true;
            searchCriteria.FeaturedOnly = true;
            searchCriteria.InStockOnly = true;

            DataTable dtFeaturedProductsList = new DataTable();
            divFeaturedProductsError.Visible = false;
            gvFeaturedProducts.Visible = true;            
            try
            {
                dtFeaturedProductsList = Product.Search(searchCriteria, true);
            }
            catch (ProductCatalogException e)
            {
                gvFeaturedProducts.Visible = false;                
                divFeaturedProductsError.Visible = true;
                divFeaturedProductsError.InnerHtml = GetLocalResourceObject("FeaturedProductsListingErrorMessage").ToString(); ;
                return;
            }

            //Display details of featured products
            DataView dvFeaturedProductsList = new DataView(dtFeaturedProductsList);            
            if (dtFeaturedProductsList.Rows.Count != 0)
            {
                dvFeaturedProductsList.Sort = "DateModified DESC";
            }

            gvFeaturedProducts.PageSize = Convert.ToInt16(_ProductsPageSize);
            gvFeaturedProducts.DataSource = dvFeaturedProductsList;
            gvFeaturedProducts.DataBind();            
        }        

        /// <summary>
        /// Loads featured product details.
        /// </summary>
        /// <remarks>
        /// This method loads product information - product name, main image in thumbnail format, 
        /// product`s lowest price (based on applied active discounts) and link for navigation to product details page.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFeaturedProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv=(DataRowView)e.Row.DataItem;

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

                HyperLink hplProductImage = (HyperLink)e.Row.FindControl("hplProductImage");
                hplProductImage.NavigateUrl = aName.HRef;
            }
        }
    }
}
