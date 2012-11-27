using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.UI.CodeBehind;
using System.Data;
using Melon.Components.ProductCatalog.Configuration;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Enumerations;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Exception;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page lists product objects in a list view
    /// </summary>
    public partial class CodeBehind_FEProductList : BaseMasterPageControl
    {        
        #region Fields & Properties

        /// <summary>
        /// Sort direction of the currently sorted column in the GridView with products.
        /// It is "ASC" for ascending sorting and "DESC" for descending sorting. 
        /// </summary>        
        public string SortDirection
        {
            get
            {
                if (ViewState["mc_pc_sortDirection"] == null)
                {
                    return "ASC";
                }
                return (string)ViewState["mc_pc_sortDirection"];
            }
            set
            {
                ViewState["mc_pc_sortDirection"] = value;
            }
        }      

        /// <summary>
        /// Page size to show how many products are listed in Product List/Grid
        /// </summary>        
        public PageItemNumberEnum ProductsPageSize
        {
            get 
            {
                if (ViewState["mc_pc_pageSize"] == null)
                {
                    return PageItemNumberEnum.Six;
                }
                return (PageItemNumberEnum)ViewState["mc_pc_pageSize"]; 
            }
            set { ViewState["mc_pc_pageSize"] = value; }
        }

        /// <summary>
        /// COlumn to sort products in Product List/Grid
        /// </summary>        
        public SortOrderEnum SortedBy
        {
            get 
            {
                if (ViewState["mc_pc_sortedBy"] == null)
                {
                    return SortOrderEnum.Name;
                }
                return (SortOrderEnum)ViewState["mc_pc_sortedBy"]; 
            }
            set { ViewState["mc_pc_sortedBy"] = value; }
        }

        /// <summary>
        /// Search criteria filter to list found products.
        /// </summary>                
        public ProductSearchCriteria searchCriteria
        {
            get
            {
                if (ViewState["SearchCriteria"] == null)
                {
                    ProductSearchCriteria _searchCriteria = new ProductSearchCriteria();
                    _searchCriteria.ActiveOnly = true;

                    return _searchCriteria;
                }

                return (ProductSearchCriteria)ViewState["SearchCriteria"];
            }
            set
            {
                ViewState["SearchCriteria"] = value;
            }
        }   
        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author> 
        protected override void OnInit(EventArgs e)
        {                        
            gvProductList.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvProductList_RowDataBound);
            this.TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);
            ((BaseMasterPageControl)this.Page.Master).SimpleSearchEvent += new SimpleSearchEventHandler(CodeBehind_FEProductList_SimpleSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).AdvancedSearchEvent+=new AdvancedSearchEventHandler(CodeBehind_FEProductList_AdvancedSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).FEDynamicCategoryExplorerEvent+=new FEDynamicCategoryExplorerEventHandler(CodeBehind_FEProductList_FEDynamicCategoryExplorerEvent);
            ((BaseMasterPageControl)this.Page.Master).DynamicPropsSearchEvent+=new DynamicPropsSearchEventHandler(CodeBehind_FEProductList_DynamicPropsSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).ProductSizeChangeEvent += new ProductSizeChangeEventHandler(CodeBehind_FEProductList_ProductSizeChangeEvent);
            ((BaseMasterPageControl)this.Page.Master).ProductColumnSortEvent += new ProductColumnSortEventHandler(CodeBehind_FEProductList_ProductColumnSortEvent);


            base.OnInit(e);
        }

        /// <summary>
        /// Loads product list filtered by entered keyword from Search panel.
        /// </summary>
        /// This method is attached to <see cref="BaseMasterPage.Control.FESimpleSearchEvent"/> which is fired when
        /// keyword is submited from search panel in Front End user interface.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBehind_FEProductList_SimpleSearchEvent(object sender, BaseMasterPageControl.SimpleSearchEventArgs e)
        {
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            criteria.keywords = e.Keywords;
            criteria.ActiveOnly = true;

            // fields to search by keywords
            List<ProductSearchFields> fields = new List<ProductSearchFields>();
            fields.Add(ProductSearchFields.Code);
            fields.Add(ProductSearchFields.Name);
            fields.Add(ProductSearchFields.Description);
            fields.Add(ProductSearchFields.Tags);
            criteria.keywordsPlaceholders = fields;

            searchCriteria = criteria; 

            LoadProductList();
        }

        /// <summary>
        /// Loads product list filtered by selected category identifier.
        /// </summary>
        /// This method is attached to <see cref="BaseMasterPage.Control.FEDynamicCategoryExplorerEvent"/> which is fired when
        /// category item is selected in category menu.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBehind_FEProductList_FEDynamicCategoryExplorerEvent(object sender, BaseMasterPageControl.FEDynamicCategoryExplorerEventArgs e)
        {
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            criteria.categoryId = e.SelectedCategoryId;
            criteria.ActiveOnly = true;

            searchCriteria = criteria;

            LoadProductList();
        }

        /// <summary>
        /// Loads product list filtered by selected search filter criteria when Search event is fired.
        /// </summary>
        /// This method is attached to <see cref="BaseMasterPage.Control.FEAdancedSearchEvent"/> which is fired when
        /// search button is pressed from AdvancedSearch user control. Search criteria are stored in <see cref="Melon.Components.ProductCatalog.ProductSearchFields"/> property
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_AdvancedSearch"/>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBehind_FEProductList_AdvancedSearchEvent(object sender, BaseMasterPageControl.AdvancedSearchEventArgs e)
        {
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            criteria.keywords = e.Keywords;
            criteria.categoryIdList = e.CategoryIdList;
            criteria.PriceFrom = e.PriceFrom;
            criteria.PriceTo = e.PriceTo;
            criteria.InStockOnly = e.IsInStock;
            criteria.ActiveOnly = true;

            // fields to search by keywords
            List<ProductSearchFields> fields = new List<ProductSearchFields>();
            fields.Add(ProductSearchFields.Code);
            fields.Add(ProductSearchFields.Name);
            fields.Add(ProductSearchFields.Description);
            fields.Add(ProductSearchFields.Tags);
            criteria.keywordsPlaceholders = fields;


            searchCriteria = criteria;

            LoadProductList();
        }

        /// <summary>
        /// Loads product list filtered by selected dynamic property values.
        /// </summary>
        /// <remarks>
        /// This method is attached to <see cref="BaseMasterPage.Control.FEDynamicPropsSearchEvent"/> which is fired when
        /// search button is pressed from DynamicProps user control.
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEDynamicPropsBrowse"/>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBehind_FEProductList_DynamicPropsSearchEvent(object sender, BaseMasterPageControl.DynamicPropsSearchEventArgs e)
        {
            if (e.DynamicPropValue.Count == 0)
            {
                return;
            }

            ProductSearchCriteria criteria = new ProductSearchCriteria();
            string valList = "";
            foreach (string val in e.DynamicPropValue.ToArray())
            {
                valList += val + ",";
            }

            valList = valList.Substring(0, valList.Length - 1);

            criteria.propValueList = valList;
            criteria.ActiveOnly = true;

            searchCriteria = criteria;

            LoadProductList();
        }

        /// <summary>
        /// Fires <see cref="LoadProductGrid(int idx)"/> method to load product list.
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="ProductPageSize"/> property to select number of products to be visible on product result grid.
        /// <seealso cref="Melon.Components.ProductCatalog.Enumerations.PageItemNumberEnum"/>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBehind_FEProductList_ProductSizeChangeEvent(object sender, BaseMasterPageControl.ProductSizeChangeEventArgs e)
        {
            ProductsPageSize = e.PageSize;

            LoadProductList();
        }

        /// <summary>
        /// Fires <see cref="LoadProductGrid(int idx)"/> method to load product list.
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="SortedBy"/> property to sort products to be visible on product result grid.
        /// <seealso cref="Melon.Components.ProductCatalog.Enumerations.SortOrderEnum"/>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeBehind_FEProductList_ProductColumnSortEvent(object sender, BaseMasterPageControl.ProductColumnSortEventArgs e)
        {
            SortedBy = e.ColumnName;

            LoadProductList();
        }

        /// <summary>
        /// Initializes user control information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProductList();
            }
        }

        /// <summary>
        /// Load products for selected search criteria: category list, price range, in stock availability and keywords
        /// </summary>
        /// <remarks>
        /// <para>
        /// Table holds information about active products in the system. Product list is filtered by several attributes:
        /// search criteria, category identifier and dynamic property values.
        /// </para>
        /// <para>        
        /// Table rows number to display is set by property <see cref="Melon.Components.ProductCatalog.Configuration.ProductCatalogSettings.TablePageSize"/>
        /// </para>
        /// <author>Melon Team</author>
        /// <date>02/02/2010</date>
        private void LoadProductList()
        {
            DataTable dtProductList = new DataTable();

            gvProductList.Visible = true;
            TopPager.Visible = true;
            divProductListError.Visible = false;

            try
            {
                dtProductList = Product.Search(searchCriteria, true);
            }
            catch (ProductCatalogException e)
            {
                gvProductList.Visible = false;
                TopPager.Visible = false;
                divProductListError.Visible = true;
                divProductListError.InnerHtml = e.Message;
                return;
            }

            //Display details of found products
            DataView dvProductList = new DataView(dtProductList);
            if (dtProductList.Rows.Count != 0)
            {
                dvProductList.Sort = Utilities.GetColumnName(this.SortedBy) + " " + this.SortDirection;
            }

            gvProductList.PageSize = Convert.ToInt16(ProductsPageSize);
            gvProductList.DataSource = dvProductList;
            gvProductList.DataBind();

            //Display paging if there are products found.
            if (dtProductList.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvProductList.PageCount, gvProductList.PageIndex + 1, 5, gvProductList.PageSize, dtProductList.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }  
        }

        /// <summary>
        /// Initializes product information in product list.
        /// </summary>
        /// <remarks>
        /// This method sets products` image file paths, main images, common prices and links to product details pages.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv=(DataRowView)e.Row.DataItem;
                HtmlAnchor aName = (HtmlAnchor)e.Row.FindControl("aName");
                aName.HRef = "~/ObjectDetails.aspx?objType=ProductList&objId=" + Utilities.StrToHash(drv["Id"].ToString());
                aName.InnerText = drv["Name"].ToString();

                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                if(drv["StartPrice"]!=DBNull.Value)
                {
                    lblPrice.Text=drv["StartPrice"].ToString()+ " " + ProductCatalogSettings.Currency;
                }

                Label lblShortDesc = (Label)e.Row.FindControl("lblDescription");
                if (drv["ShortDescription"] != DBNull.Value)
                {
                    lblShortDesc.Text = Melon.General.StringUtils.Cut(drv["ShortDescription"].ToString(), " ", 60, 20);
                }

                Image imgProduct = (Image)e.Row.FindControl("imgProduct");                
                string imageUrl=drv["ImagePath"].ToString();
                
                if(imageUrl==String.Empty)
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

        /// <summary>
        /// Performs page changing for product list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        protected void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {            
            gvProductList.PageIndex = e.NewPage;                
            LoadProductList();            
        }        
    }
}
