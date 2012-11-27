using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Melon.Components.ProductCatalog.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page lists catalog objects in a table view
    /// </summary>
    public partial class CodeBehind_FECatalogList : BaseMasterPageControl
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
            gvCatalogList.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvCatalogList_RowDataBound);
            this.TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);
            ((BaseMasterPageControl)this.Page.Master).AdvancedSearchEvent += new AdvancedSearchEventHandler(CodeBehind_FEProductList_AdvancedSearchEvent);
            ((BaseMasterPageControl)this.Page.Master).SimpleSearchEvent += new SimpleSearchEventHandler(CodeBehind_FEProductList_SimpleSearchEvent);

            base.OnInit(e);
        }

        /// <summary>
        /// Loads catalog list filtered by entered keyword from Search pane;.
        /// </summary>
        /// This method is attached to <see cref="BaseMasterPage.Control.FESimpoleSearchEvent"/> which is fired when
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

            LoadCatalogList();
        }

        /// <summary>
        /// Loads catalog list filtered by selected search filter criteria when Search event is fired.
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

            LoadCatalogList();
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
                LoadCatalogList();
            }
        }

        /// <summary>
        /// Load catalogs list - show name, short description, common price and link to view full catalog details
        /// </summary>
        /// <remarks>
        /// <para>
        /// Table holds information about active catalogs in the system. Catalog list is filtered by applying 
        /// search criteria. For more information see <see cref="searchCriteria"/> property.
        /// </para>
        /// <para>        
        /// Table rows number to display is set by property <see cref="Melon.Components.ProductCatalog.Configuration.ProductCatalogSettings.TablePageSize"/>
        /// </para>
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/03/2010</date>
        private void LoadCatalogList()
        {
            DataTable dtCatalogList = new DataTable();

            gvCatalogList.Visible = true;
            TopPager.Visible = true;
            divCatalogListError.Visible = false;
            try
            {
                dtCatalogList = Catalog.Search(searchCriteria);
            }
            catch (ProductCatalogException e)
            {
                gvCatalogList.Visible = false;
                TopPager.Visible = false;
                divCatalogListError.Visible = true;
                divCatalogListError.InnerHtml = e.Message;
                return;
            }

            //Display details of found product reviews
            gvCatalogList.PageSize = Convert.ToInt16(ProductCatalogSettings.TablePageSize);
            gvCatalogList.DataSource = dtCatalogList;
            gvCatalogList.DataBind();

            //Display paging if there are bundles found.
            if (dtCatalogList.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvCatalogList.PageCount, gvCatalogList.PageIndex + 1, 5, gvCatalogList.PageSize, dtCatalogList.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }
        }

        /// <summary>
        /// Initializes catalog information in catalog listing.
        /// </summary>
        /// <remarks>
        /// This method sets catalogs` names and links to catalog details pages
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCatalogList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnViewCatalog = (Button)e.Row.FindControl("btnViewCatalog");
                    btnViewCatalog.PostBackUrl = "~/ObjectDetails.aspx?objType=CatalogList&objId=" + Utilities.StrToHash(((DataRowView)e.Row.DataItem)["Id"].ToString());                    
                }
            }
        }

        /// <summary>
        /// Performs page changing for catalog list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            gvCatalogList.PageIndex = e.NewPage;
            LoadCatalogList();
        }
    }
}
