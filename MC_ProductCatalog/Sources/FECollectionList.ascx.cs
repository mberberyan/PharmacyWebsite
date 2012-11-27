using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using Melon.Components.ProductCatalog.Configuration;
using System.Web.UI;
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page lists collection objects in a table view
    /// </summary>
    public partial class CodeBehind_FECollectionList : BaseMasterPageControl
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
            gvCollectionList.RowDataBound += new GridViewRowEventHandler(gvCollectionList_RowDataBound);
            this.TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);
            ((BaseMasterPageControl)this.Page.Master).AdvancedSearchEvent += new AdvancedSearchEventHandler(CodeBehind_FEProductList_AdvancedSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).SimpleSearchEvent += new SimpleSearchEventHandler(CodeBehind_FEProductList_SimpleSearchEvent);

            base.OnInit(e);
        }

        /// <summary>
        /// Loads collection list filtered by entered keyword from Search panel.
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

            LoadCollectionList();
        }

        /// <summary>
        /// Loads collection list filtered by selected search filter criteria when Search event is fired.
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

            LoadCollectionList();
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
                LoadCollectionList();
            }
        }

        /// <summary>
        /// Load collection list - show name, short description, common price and link to view full collection details
        /// </summary>
        /// <remarks>
        /// <para>
        /// Table holds information about active collections in the system. Collection list is filtered by applying 
        /// search criteria. For more information see <see cref="searchCriteria"/> property.
        /// </para>
        /// <para>        
        /// Table rows number to display is set by property <see cref="Melon.Components.ProductCatalog.Configuration.ProductCatalogSettings.TablePageSize"/>
        /// </para>
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/03/2010</date>
        private void LoadCollectionList()
        {
            DataTable dtCollectionList = new DataTable();

            gvCollectionList.Visible = true;
            TopPager.Visible = true;
            divCollectionListError.Visible = false;
            try
            {
                dtCollectionList = Collection.Search(searchCriteria);
            }
            catch (ProductCatalogException e)
            {
                gvCollectionList.Visible = false;
                TopPager.Visible = false;
                divCollectionListError.Visible = true;
                divCollectionListError.InnerHtml = e.Message;
                return;
            }

            //Display details of found product reviews
            gvCollectionList.PageSize = Convert.ToInt16(ProductCatalogSettings.TablePageSize);
            gvCollectionList.DataSource = dtCollectionList;
            gvCollectionList.DataBind();

            //Display paging if there are bundles found.
            if (dtCollectionList.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvCollectionList.PageCount, gvCollectionList.PageIndex + 1, 5, gvCollectionList.PageSize, dtCollectionList.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }
        }

        /// <summary>
        /// Initializes collection information in collection listing.
        /// </summary>
        /// <remarks>
        /// This method sets collections` image file paths and links to collection details pages
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCollectionList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnViewCollection = (Button)e.Row.FindControl("btnViewCollection");
                btnViewCollection.PostBackUrl = "~/ObjectDetails.aspx?objType=CollectionList&objId=" + Utilities.StrToHash(((DataRowView)e.Row.DataItem)["Id"].ToString());                    

                Image imgCollection = (Image)e.Row.FindControl("imgCollection");
                string imageUrl = ((DataRowView)e.Row.DataItem)["ImagePath"].ToString();

                if (imageUrl == String.Empty)
                {
                    imgCollection.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
                }
                else
                {
                    imgCollection.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }
            }
        }

        /// <summary>
        /// Performs page changing for collection list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            gvCollectionList.PageIndex = e.NewPage;
            LoadCollectionList();
        }
    }
}
