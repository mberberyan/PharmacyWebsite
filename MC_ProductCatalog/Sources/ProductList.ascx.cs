using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page allows adding products to current loaded object.
    /// </summary>
    /// <remarks>
    /// It contains user controls to filter products by applying search criteria and managing products list
    /// for the loaded object except products or managing related products for the selected product object.
    /// </remarks>    
    public partial class CodeBehind_ProductList : ProductCatalogControl
    {
        /// <summary>
        /// Attach event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>  
        protected override void OnInit(EventArgs e)
        {            
            btnSearch.Click += new EventHandler(btnSearch_Click);
            TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);
            gvProductList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(gvProductList_RowCommand);
        }

        /// <summary>
        /// Set user control property values passed from control`s caller page
        /// </summary>
        /// <param name="args"></param>
        public override void Initializer(object[] args)
        {            
            SelectedObjectId = (int?)args[0];
            SelectedObjectType = (ComponentObjectEnum)args[1];
            SelectedTab = (ProductCatalogTabs)args[2];            
        }

        /// <summary>
        /// Initalizes user control information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                TopPager.Visible = false;
            }            
        }

        /// <summary>
        /// Load products for current category selected from Category Explorer
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/14/2009</date>
        private void LoadProductList(ProductSearchCriteria searchCriteria)
        {
            DataTable dtProducts = Product.Search(searchCriteria, null);
            gvProductList.DataSource = dtProducts; 
            gvProductList.DataBind();

            if (dtProducts.Rows.Count != 0)
            {             
                TopPager.Visible = true;                               
                TopPager.FillPaging(gvProductList.PageCount, gvProductList.PageIndex + 1, 5, gvProductList.PageSize, dtProducts.Rows.Count);
            }
            else
            {                                
                TopPager.Visible = false;
            }            
        }

        /// <summary>
        /// Performs page changing for product result list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                gvProductList.PageIndex = e.NewPage;
                ProductSearchCriteria searchCriteria = CollectSearchCriteria();
                LoadProductList(searchCriteria);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Fire events for selected product on selected row
        /// </summary>
        /// <remarks>
        /// N - opens product details page
        /// X - remove product from system
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>09/14/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Navigate")
            {
                LoadProductEventArgs args = new LoadProductEventArgs();
                args.SelectedCategoryId = SelectedObjectId;
                args.SelectedProductId = Convert.ToInt32(e.CommandArgument);
                args.SelectedObjectType = ComponentObjectEnum.Product;
                args.SelectedTab = ProductCatalogTabs.GeneralInformation;
                this.ParentControl.OnLoadProduct(sender, args);
            }
            else if(e.CommandName == "Remove")
            {
                Product.Delete(Convert.ToInt32(e.CommandArgument));
                btnSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// Event handler for event Click of Button btnSearch.
        /// </summary>
        /// <remarks>
        ///     The methods calls method <see cref="CollectSearchCriteria"/> to collect the entered search criteria
        ///     and then method <see cref="LoadProductList"/> is called to 
        ///     search for products corresponding to the search critera and display them in GridView gvProductList.
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>09/14/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ProductSearchCriteria searchCriteria = CollectSearchCriteria();
            LoadProductList(searchCriteria);
        }

        /// <summary>
        /// Returns the currently entered search criteria to filter for products
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/14/2009</date>
        /// <returns></returns>
        private ProductSearchCriteria CollectSearchCriteria()
        {
            ProductSearchCriteria searchCriteria = new ProductSearchCriteria();
            searchCriteria.categoryId = SelectedObjectId;
            if (txtKeywords.Text.Trim() != String.Empty)
            {
                searchCriteria.keywords = txtKeywords.Text.Trim();
            }

            if (txtPriceFrom.Text.Trim() != String.Empty)
            {
                searchCriteria.PriceFrom = Convert.ToDouble(txtPriceFrom.Text.Trim());
            }

            if (txtPriceTo.Text.Trim() != String.Empty)
            {
                searchCriteria.PriceTo = Convert.ToDouble(txtPriceTo.Text.Trim());
            }

            List<ProductSearchFields> fields = new List<ProductSearchFields>();
            if (cbxlSearchCriteria.Items.FindByValue("Code").Selected)
            {
                fields.Add(ProductSearchFields.Code);
            }
            
            if (cbxlSearchCriteria.Items.FindByValue("Name").Selected)
            {
                fields.Add(ProductSearchFields.Name);
            }
            
            if (cbxlSearchCriteria.Items.FindByValue("Description").Selected)
            {
                fields.Add(ProductSearchFields.Description);
            }
            
            if (cbxlSearchCriteria.Items.FindByValue("Tags").Selected)
            {
                fields.Add(ProductSearchFields.Tags);
            }

            if (fields.Count > 0)
            {
                searchCriteria.keywordsPlaceholders = fields;
            }
            
            if(ddlActive.SelectedValue!="")
            {
                searchCriteria.ActiveOnly = ddlActive.SelectedValue == "0";
            }

            if (ddlInStock.SelectedValue != "")
            {
                searchCriteria.InStockOnly = ddlInStock.SelectedValue == "0";
            }

            if (ddlFeatured.SelectedValue != "")
            {
                searchCriteria.FeaturedOnly = ddlFeatured.SelectedValue == "0";
            }

            return searchCriteria;
        }

    }
}
