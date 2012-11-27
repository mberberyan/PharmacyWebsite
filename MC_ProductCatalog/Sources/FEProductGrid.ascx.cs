using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.UI.CodeBehind;
using Melon.Components.ProductCatalog.Enumerations;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Exception;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page lists product objects in a grid view
    /// </summary>
    public partial class CodeBehind_FEProductGrid : BaseMasterPageControl
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
        
        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author> 
        protected override void OnInit(EventArgs e)
        {
            dlProductGrid.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(dlProductGrid_ItemDataBound);
            this.TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);
            ((BaseMasterPageControl)this.Page.Master).FECategoryExplorerEvent += new FECategoryExplorerEventHandler(CodeBehind_FEBundleList_FECategoryExplorerEvent);
            ((BaseMasterPageControl)this.Page.Master).FEDynamicCategoryExplorerEvent += new FEDynamicCategoryExplorerEventHandler(CodeBehind_FEProductGrid_FEDynamicCategoryExplorerEvent);
            ((BaseMasterPageControl)this.Page.Master).AdvancedSearchEvent += new AdvancedSearchEventHandler(CodeBehind_FEProductList_AdvancedSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).SimpleSearchEvent += new SimpleSearchEventHandler(CodeBehind_FEProductList_SimpleSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).DynamicPropsSearchEvent += new DynamicPropsSearchEventHandler(CodeBehind_FEProductList_DynamicPropsSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).ProductSizeChangeEvent += new ProductSizeChangeEventHandler(CodeBehind_FEProductList_ProductSizeChangeEvent);
            ((BaseMasterPageControl)this.Page.Master).ProductColumnSortEvent += new ProductColumnSortEventHandler(CodeBehind_FEProductList_ProductColumnSortEvent);

            base.OnInit(e);
        }

        /// <summary>
        /// Loads product grid filtered by selected category identifier.
        /// </summary>
        /// This method is attached to <see cref="BaseMasterPage.Control.FECategoryExplorerEvent"/> which is fired when
        /// category item is selected in category menu.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CodeBehind_FEBundleList_FECategoryExplorerEvent(object sender, BaseMasterPageControl.FECategoryExplorerEventArgs e)
        {
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            criteria.categoryId = e.SelectedCategoryId;
            criteria.ActiveOnly = true;
            criteria.InStockOnly = true;
            criteria.FeaturedOnly = false;

            searchCriteria = criteria;

            LoadProductGrid(0);
        }

        /// <summary>
        /// Loads product grid filtered by selected category identifier.
        /// </summary>
        /// This method is attached to <see cref="BaseMasterPage.Control.FEDynamicCategoryExplorerEvent"/> which is fired when
        /// category item is selected in category menu.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CodeBehind_FEProductGrid_FEDynamicCategoryExplorerEvent(object sender, BaseMasterPageControl.FEDynamicCategoryExplorerEventArgs e)
        {
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            criteria.categoryId = e.SelectedCategoryId;
            criteria.ActiveOnly = true;
            criteria.InStockOnly = true;
            criteria.FeaturedOnly = false;

            searchCriteria = criteria;

            LoadProductGrid(0);
        }

        /// <summary>
        /// Loads product grid filtered by entered keyword from Search panel.
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
            criteria.InStockOnly = true;
            criteria.FeaturedOnly = false;

            // fields to search by keywords
            List<ProductSearchFields> fields = new List<ProductSearchFields>();
            fields.Add(ProductSearchFields.Code);
            fields.Add(ProductSearchFields.Name);
            fields.Add(ProductSearchFields.Description);
            fields.Add(ProductSearchFields.Tags);
            criteria.keywordsPlaceholders = fields;

            searchCriteria = criteria;

            LoadProductGrid(0);
        }

        /// <summary>
        /// Loads product grid filtered by selected search filter criteria when Search event is fired.
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

            LoadProductGrid(0);
        }

        /// <summary>
        /// Loads product grid filtered by selected dynamic property values.
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

            LoadProductGrid(0);
        }

        /// <summary>
        /// Fires <see cref="LoadProductGrid(int idx)"/> method to load product grid.
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

            LoadProductGrid(0);
        }

        /// <summary>
        /// Fires <see cref="LoadProductGrid(int idx)"/> method to load product grid.
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

            LoadProductGrid(0);
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
                LoadProductGrid(0);
            }
        }

        /// <summary>
        /// Load product grid - show name, short description, main image, common price and link to view full product details
        /// </summary>
        /// <remarks>
        /// <para>
        /// Table holds information about active products in the system. Product grid is filtered by several attributes:
        /// search criteria, category identifier and dynamic property values.
        /// </para>
        /// <para>        
        /// Table rows number to display is set by property <see cref="Melon.Components.ProductCatalog.Configuration.ProductCatalogSettings.TablePageSize"/>
        /// </para>
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/03/2010</date>
        /// <param name="pageIndex"></param>
        private void LoadProductGrid(int pageIndex)
        {            
            DataTable dtProductGrid = new DataTable();

            dlProductGrid.Visible = true;
            TopPager.Visible = true;
            divProductGridError.Visible = false;
            try
            {
                dtProductGrid = Product.Search(searchCriteria, true);
            }
            catch (ProductCatalogException e)
            {
                dlProductGrid.Visible = false;
                TopPager.Visible = false;
                divProductGridError.Visible = true;
                divProductGridError.InnerHtml = e.Message;
                return;
            }

            PagedDataSource pagedDS = new PagedDataSource();
            
            //Display details of found product reviews
            DataView dvProductGrid = new DataView(dtProductGrid);
            if (dtProductGrid.Rows.Count != 0)
            {
                dvProductGrid.Sort = Utilities.GetColumnName(this.SortedBy) + " " + this.SortDirection;
            }

            pagedDS.DataSource = dvProductGrid;
            pagedDS.AllowPaging = true;
            pagedDS.CurrentPageIndex = pageIndex;
            pagedDS.PageSize = (int)ProductsPageSize;
            

            //Display paging if there are users found.
            if (dtProductGrid.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(pagedDS.PageCount, pagedDS.CurrentPageIndex + 1, 5, pagedDS.PageSize, dtProductGrid.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }

            dlProductGrid.DataSource = pagedDS;
            dlProductGrid.DataBind();
        }

        /// <summary>
        /// Performs page changing for product grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        public void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {            
            LoadProductGrid(e.NewPage);   
        }

        /// <summary>
        /// Initializes product information in product grid.
        /// </summary>
        /// <remarks>
        /// This method sets products` image file paths, main images, common prices and links to product details pages.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlProductGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv=(DataRowView)e.Item.DataItem;
                HtmlAnchor aName = (HtmlAnchor)e.Item.FindControl("aName");
                aName.HRef = "~/ObjectDetails.aspx?objType=ProductGrid&objId=" + Utilities.StrToHash(drv["Id"].ToString());
                aName.InnerText = drv["Name"].ToString();

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                if (drv["StartPrice"] != DBNull.Value)
                {
                    lblPrice.Text = drv["StartPrice"].ToString() + " " + ProductCatalogSettings.Currency;
                }

                Image imgProduct = (Image)e.Item.FindControl("imgProduct");
                string imageUrl = drv["ImagePath"].ToString();

                if (imageUrl == String.Empty)
                {
                    imgProduct.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
                }
                else
                {
                    imgProduct.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }

                HyperLink hplProductImage = (HyperLink)e.Item.FindControl("hplProductImage");
                hplProductImage.NavigateUrl = aName.HRef;

                PagedDataSource pagedDS = (PagedDataSource)dlProductGrid.DataSource;
                if (((Convert.ToInt16(ProductsPageSize) / dlProductGrid.RepeatColumns) * dlProductGrid.RepeatColumns) < (e.Item.ItemIndex + 1))
                {
                    e.Item.CssClass = "";
                }                
            }
        }                
    }
}
