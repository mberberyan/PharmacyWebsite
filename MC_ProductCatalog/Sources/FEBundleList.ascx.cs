using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.Enumerations;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Melon.Components.ProductCatalog.Exception;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page lists bundle objects in a table view
    /// </summary>
    public partial class CodeBehind_FEBundleList : BaseMasterPageControl
    {        

        #region Fields && Pproperties
        /// <summary>
        /// Search criteria filter to list found collections.
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
            gvBundleList.RowDataBound +=new GridViewRowEventHandler(gvBundleList_RowDataBound);
            this.TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);
            ((BaseMasterPageControl)this.Page.Master).FECategoryExplorerEvent += new FECategoryExplorerEventHandler(CodeBehind_FEBundleList_FECategoryExplorerEvent);
            ((BaseMasterPageControl)this.Page.Master).FEDynamicCategoryExplorerEvent += new FEDynamicCategoryExplorerEventHandler(CodeBehind_FEBundleList_FEDynamicCategoryExplorerEvent);
            ((BaseMasterPageControl)this.Page.Master).AdvancedSearchEvent += new AdvancedSearchEventHandler(CodeBehind_FEProductList_AdvancedSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).SimpleSearchEvent += new SimpleSearchEventHandler(CodeBehind_FEProductList_SimpleSearchEvent);

            base.OnInit(e);
        }

        /// <summary>
        /// Loads bundle list filtered by selected category identifier.
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

            LoadBundleList();
        }

        /// <summary>
        /// Loads bundle list filtered by selected category identifier.
        /// </summary>
        /// This method is attached to <see cref="BaseMasterPage.Control.FEDynamicCategoryExplorerEvent"/> which is fired when
        /// category item is selected in category menu.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CodeBehind_FEBundleList_FEDynamicCategoryExplorerEvent(object sender, BaseMasterPageControl.FEDynamicCategoryExplorerEventArgs e)
        {
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            criteria.categoryId = e.SelectedCategoryId;
            criteria.ActiveOnly = true;
            criteria.InStockOnly = true;
            criteria.FeaturedOnly = false;

            searchCriteria = criteria;

            LoadBundleList();
        }

        /// <summary>
        /// Loads bundle list filtered by entered keyword from Search panel.
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

            LoadBundleList();
        }

        /// <summary>
        /// Loads bundle list filtered by selected search filter criteria when Search event is fired.
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
            criteria.ActiveOnly = true;

            // fields to search by keywords
            List<ProductSearchFields> fields = new List<ProductSearchFields>();
            fields.Add(ProductSearchFields.Code);
            fields.Add(ProductSearchFields.Name);
            fields.Add(ProductSearchFields.Description);
            fields.Add(ProductSearchFields.Tags);
            criteria.keywordsPlaceholders = fields;

            searchCriteria = criteria;

            LoadBundleList();
        }

        /// <summary>
        /// Inizitalizes user control information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBundleList();
            }
        }

        /// <summary>
        /// Load bundles list - show name, short description, common price and link to view full bundle details
        /// </summary>
        /// <remarks>
        /// <para>
        /// Table holds information about active bundles in the system. Bundle list is filtered by applying 
        /// search criteria. For more information see <see cref="searchCriteria"/> property.
        /// </para>
        /// <para>        
        /// Table rows number to display is set by property <see cref="Melon.Components.ProductCatalog.Configuration.ProductCatalogSettings.TablePageSize"/>
        /// </para>
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/03/2010</date>
        private void LoadBundleList()
        {                        
            DataTable dtBundleList = new DataTable();

            gvBundleList.Visible = true;
            TopPager.Visible = true;
            divBundleListError.Visible = false;
            try
            {
                dtBundleList = Bundle.Search(searchCriteria);
            }
            catch (ProductCatalogException e)
            {
                gvBundleList.Visible = false;
                TopPager.Visible = false;
                divBundleListError.Visible = true;
                divBundleListError.InnerHtml = e.Message;
                return;
            }

            //Display details of found product reviews
            gvBundleList.PageSize = Convert.ToInt16(ProductCatalogSettings.TablePageSize);
            gvBundleList.DataSource = dtBundleList;
            gvBundleList.DataBind();

            //Display paging if there are bundles found.
            if (dtBundleList.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvBundleList.PageCount, gvBundleList.PageIndex + 1, 5, gvBundleList.PageSize, dtBundleList.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }
        }

        /// <summary>
        /// Initializes bundle information in bundle listing.
        /// </summary>
        /// <remarks>
        /// This method sets bundles` image file paths, common prices and links to bundle details pages
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBundleList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv=(DataRowView)e.Row.DataItem;
                Button btnViewBundle = (Button)e.Row.FindControl("btnViewBundle");
                btnViewBundle.PostBackUrl = "~/ObjectDetails.aspx?objType=BundleList&objId=" + Utilities.StrToHash(drv["Id"].ToString());

                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                if (drv["Price"] != DBNull.Value)
                {
                    lblPrice.Text = drv["Price"].ToString() + " " + ProductCatalogSettings.Currency;
                }

                Image imgBundle = (Image)e.Row.FindControl("imgBundle");
                string imageUrl = drv["ImagePath"].ToString();

                if (imageUrl == String.Empty)
                {
                    imgBundle.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
                }
                else
                {
                    imgBundle.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }                
            }
        }

        /// <summary>
        /// Performs page changing for bundles list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            gvBundleList.PageIndex = e.NewPage;
            LoadBundleList();
        }

    }
}
