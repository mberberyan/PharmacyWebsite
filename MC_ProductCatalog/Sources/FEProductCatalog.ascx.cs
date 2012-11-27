using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Represents the main control for the Front End of the Product Catalog component, which is to be included in the desired page on the web site where demo webiste should be integrated.
    /// The control inherits most of its functionality from <see cref="Melon.Components.ProductCatalog.ComponentEngine.ProductCatalogControl"/>.
    /// In short this control manages all the logic flow and by using a system of events decides how to proceed on every step.
    /// </summary>
    public partial class CodeBehind_FEProductCatalog : ProductCatalogControl
    {
        /// <summary>
        /// Sets visibility of front end user controls based on query string parameters.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            LoadObjectDetailsControl();

            base.OnPreRender(e);
        }

        /// <summary>
        /// Initializes query string parameters and fires events in order to filter object listing information.
        /// </summary>
        /// <remarks>
        /// This method uses query string parameters in order to navigate users to desired page and fires events
        /// to filter objects in Product Catalog front-end listing controls.
        /// <para>
        /// Following query string parameters are declared:
        /// <list type="bullet">
        /// <item>objType - holds comma-separated string of type names. These names denote 
        /// the user controls to be opened. This parameter can be of type: ProductList, ProductGrid, BundleList, CatalogList, CollectionList, AdvancedSearch
        /// </item>
        /// <item>keyword - entered search keyword, selected from SimpleSearch.ascx control to filter products with</item>
        /// <item>catId = selected category identifier from Category menu</item>
        /// <item>valueList - comma-separated string of selected dynamic property values</item>
        /// </list>
        /// </para>
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductList"/>
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductGrid"/>
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_BundleList"/>
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_CatalogList"/>
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_CollectionList"/>
        /// <seealso cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_AdvancedSearch"/>        
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["objType"]))
                {
                    this.MasterPageControl.SelectedTab = Request.QueryString["objType"];                    
                }                

                // if Simple Search button is pressed on Landing Page, then keyword should be loaded and SimpleSearchEvent fired
                if (!String.IsNullOrEmpty(Request.QueryString["keyword"]))
                {
                    BaseMasterPageControl.SimpleSearchEventArgs asEventArgs = new BaseMasterPageControl.SimpleSearchEventArgs();
                    asEventArgs.Keywords = Request.QueryString["keyword"];

                    this.MasterPageControl.OnSimpleSearch(sender, asEventArgs);
                    return;
                }

                // if Category item is selected on Landing Page, then category identifier should be loaded and FEDynamicCategoryExplorer fired
                int idx;
                if (!String.IsNullOrEmpty(Request.QueryString["catId"]) && Int32.TryParse(Request.QueryString["catId"], out idx))
                {
                    BaseMasterPageControl.FEDynamicCategoryExplorerEventArgs asCatEventArgs = new BaseMasterPageControl.FEDynamicCategoryExplorerEventArgs();
                    asCatEventArgs.SelectedCategoryId = idx;

                    this.MasterPageControl.OnFEDynamicCategoryExplorer(sender, asCatEventArgs);
                    return;
                }

                // if Dynamic Property Search button is being pressed on Landing Page, then props value list should be loaded and FEDynamicPropBrowseEvent fired
                if (!String.IsNullOrEmpty(Request.QueryString["valueList"]))
                {
                    BaseMasterPageControl.DynamicPropsSearchEventArgs asPropsEventArgs = new BaseMasterPageControl.DynamicPropsSearchEventArgs();
                    asPropsEventArgs.DynamicPropValue = Request.QueryString["valueList"].ToString().Split(',').ToList<string>();

                    this.MasterPageControl.OnDynamicPropsSearch(sender, asPropsEventArgs);
                    return;
                }
            }            
        }

        /// <summary>
        /// Loads user controls depending on <see cref="BaseMasterPageControl.SelectedTab"/> parameter.
        /// </summary>
        private void LoadObjectDetailsControl()
        {
            ucProductList.Visible = this.MasterPageControl.SelectedTab.Contains("ProductList");
            ucProductGrid.Visible = this.MasterPageControl.SelectedTab.Contains("ProductGrid");
            ucCatalogList.Visible = this.MasterPageControl.SelectedTab.Contains("CatalogList");
            ucBundleList.Visible = this.MasterPageControl.SelectedTab.Contains("BundleList");
            ucCollectionList.Visible = this.MasterPageControl.SelectedTab.Contains("CollectionList");
            ucAdvancedSearch.Visible = this.MasterPageControl.SelectedTab.Contains("AdvancedSearch");
        }
    }
}
