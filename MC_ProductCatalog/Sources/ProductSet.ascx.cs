using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.Configuration;
using System.Web.UI;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page allows adding products to current loaded object.
    /// </summary>
    /// <remarks>
    /// It contains user controls to filter products by applying search criteria and managing products list
    /// for the loaded object except products.
    /// </remarks>    
    public partial class CodeBehind_ProductSet : ProductCatalogControl
    {        

        #region Properties && Fields
        private List<int?> _CategoryIdList;
        /// <summary>
        /// List of category identifiers used in search products criteria
        /// </summary>
        public List<int?> CategoryIdList
        {
            get
            {
                List<int?> list = new List<int?>();
                if (hfProductSetCategoryList.Value != String.Empty)
                {
                    foreach (string str in hfProductSetCategoryList.Value.Split(','))
                    {
                        list.Add(Convert.ToInt32(str));
                    }
                }

                return list.Count > 0 ? list : new List<int?>();
            }
            set { _CategoryIdList = value; }
        }

        private List<string> _CategoryNameList;
        /// <summary>
        /// List of category names used in search products criteria
        /// </summary>
        public List<string> CategoryNameList
        {
            get
            {
                List<string> list = new List<string>();
                if (hfProductSetCategoryName.Value != String.Empty)
                {
                    foreach (string str in hfProductSetCategoryName.Value.Split(','))
                    {
                        list.Add(str);
                    }
                }

                return list.Count > 0 ? list : new List<string>();
            }
            set { _CategoryNameList = value; }
        }

        /// <summary>
        /// Table with product results from applied search filter criteria
        /// </summary>
        private DataTable tabResultProducts
        {
            get
            {
                if (ViewState["mc_pc_resultproducts"] == null)
                {
                    return new DataTable();
                }

                return (DataTable)ViewState["mc_pc_resultproducts"];
            }

            set { ViewState["mc_pc_resultproducts"] = value; }
        }

        /// <summary>
        /// Table with already added products for current object
        /// </summary>
        private DataTable tabObjectProducts
        {
            get 
            {
                if (ViewState["mc_pc_objectproducts"] == null)
                {
                    return new DataTable();
                }

                return (DataTable)ViewState["mc_pc_objectproducts"];
            }

            set { ViewState["mc_pc_objectproducts"] = value; }
        }

        /// <summary>
        /// Save search criteria values when searching/adding/removing items
        /// </summary>
        public ProductSearchCriteria interfaceSearchCriteria
        {
            get 
            {
                if (ViewState["mc_pc_interfaceSearchCriteria"] == null)
                {
                    return new ProductSearchCriteria();
                }

                return (ProductSearchCriteria)ViewState["mc_pc_interfaceSearchCriteria"]; 
            }
            set { ViewState["mc_pc_interfaceSearchCriteria"] = value; }
        }

        /// <summary>
        /// Flag whether user control is opened for the first time
        /// </summary>
        public bool isFirstLoad
        {
            get
            {
                if (ViewState["mc_pc_isFirstLoad"] == null)
                {
                    return true;
                }

                return (bool)ViewState["mc_pc_isFirstLoad"];
            }
            set { ViewState["mc_pc_isFirstLoad"] = value; }
        }
        #endregion

        /// <summary>
        /// Attach event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>  
        protected override void OnInit(EventArgs e)
        {
            gvProductList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(gvProductList_RowCommand);
            gvObjectProductsList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(gvObjectProductsList_RowCommand);
            gvObjectProductsList.RowDataBound += new GridViewRowEventHandler(gvObjectProductsList_RowDataBound);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            this.ProductSetPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(ProductSetPager_PageChanged);
            this.ObjectProductListPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(ObjectProductListPager_PageChanged);            

            btnRemoveAll.OnClientClick = "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageRemoveProductList").ToString() + "\");";
            btnAddAll.Click += new EventHandler(btnAddAll_Click);
            btnAddAllFromPage.Click += new EventHandler(btnAddAllFromPage_Click);
            btnRemoveAll.Command += new CommandEventHandler(btnRemoveAll_Command);
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
        /// Initializes search filter criteria and loads product result table.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!isFirstLoad)
            {
                SetSearchPanelFilter();

                // load search result table with saved search criteria
                LoadProductList(interfaceSearchCriteria);
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Initalizes user control information
        /// </summary>
        /// <remarks>
        /// This method register startup javascript that expands or collapses product result table.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            //Regsiter script for initial collase/expand of product set search results.
            string script = @"<script language='javascript' type='text/javascript'>
                if (typeof(hfProductSetSearchStatus)!= 'undefined')
                {
                    if (hfProductSetSearchStatus.value == 'collapsed')
                    {
                        document.getElementById('lnkProductSetSearch').className = 'mc_pc_lnk_expand';
                        document.getElementById('tabProductSetSearchResult').style.display = 'none';
                    }
                    else
                    {
                        document.getElementById('lnkProductSetSearch').className = 'mc_pc_lnk_collapse';
                        document.getElementById('tabProductSetSearchResult').style.display = '';
                    }
                }
                </script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CollaspseExpand", script, false);

            if (!IsControlPostBack)
            {
                LoadCategoryList();
                ProductSetPager.Visible = false;
                ObjectProductListPager.Visible = false;
                LoadObjectProductsList();

                hfProductSetSearchStatus.Value = "collapsed";
            }            
        }

        /// <summary>
        /// Load all products meeting criteria in order to add to object`s product set
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/06/2009</date>
        private void LoadProductList(ProductSearchCriteria searchCriteria)
        {
            DataTable dtProducts = Product.Search(searchCriteria, null);
            DataColumn[] keys = new DataColumn[1];
            keys[0] = dtProducts.Columns["Id"];
            dtProducts.PrimaryKey = keys;
            //foreach (DataRow row in ((DataTable)gvObjectProductsList.DataSource).Rows)
            foreach (DataRow row in tabObjectProducts.Rows)
            {
                if (dtProducts.Rows.Contains(row["ProductId"].ToString()))
                {
                    dtProducts.Rows.Remove(dtProducts.Rows.Find(row["ProductId"].ToString()));
                }
            }

            // if showing related products, then remove current product from product result list
            if (SelectedObjectType == ComponentObjectEnum.Product)
            { 
                if(dtProducts.Rows.Contains(SelectedProductId))
                {
                    dtProducts.Rows.Remove(dtProducts.Rows.Find(SelectedProductId));
                }
            }

            gvProductList.DataSource = dtProducts;
            gvProductList.DataBind();

            // set result products in 'tabResultProducts' property            
            tabResultProducts = dtProducts;
            gvProductList.PageSize = ProductCatalogSettings.TablePageSize;

            //Display paging if there are users found.
            if (dtProducts.Rows.Count != 0)
            {
                hfProductSetSearchStatus.Value = "expanded";
                ProductSetPager.Visible = true;
                btnAddAll.Visible = true;
                btnAddAllFromPage.Visible = true;
                ProductSetPager.FillPaging(gvProductList.PageCount, gvProductList.PageIndex + 1, 5, gvProductList.PageSize, dtProducts.Rows.Count);
            }
            else
            {
                ProductSetPager.Visible = false;
                btnAddAll.Visible = false;
                btnAddAllFromPage.Visible = false;
            }
        }

        /// <summary>
        /// Load all object products for current object
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/10/2009</date>
        private void LoadObjectProductsList()
        {
            DataTable dtObjectProducts = new DataTable();
            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Catalog:
                    CatalogProducts catProducts = new CatalogProducts();
                    catProducts.CatalogId = SelectedObjectId;
                    dtObjectProducts = CatalogProducts.List(catProducts);
                    break;
                case ComponentObjectEnum.Product:
                    RelatedProducts relProducts = new RelatedProducts();
                    relProducts.ProductId = SelectedProductId;
                    dtObjectProducts = RelatedProducts.List(relProducts);
                    break;
                case ComponentObjectEnum.Bundle:
                    BundleProducts bundleProducts = new BundleProducts();
                    bundleProducts.BundleId = SelectedObjectId;
                    dtObjectProducts = BundleProducts.List(bundleProducts);
                    break;
                case ComponentObjectEnum.Collection:
                    CollectionProducts collProducts = new CollectionProducts();
                    collProducts.CollectionId = SelectedObjectId;
                    dtObjectProducts = CollectionProducts.List(collProducts);
                    break;
                case ComponentObjectEnum.Discount:
                    DiscountProducts discountProducts = new DiscountProducts();
                    discountProducts.DiscountId = SelectedObjectId;
                    dtObjectProducts = DiscountProducts.List(discountProducts);
                    break;
            }
            
            gvObjectProductsList.DataSource = dtObjectProducts;
            gvObjectProductsList.DataBind();

            // set catalog products in 'tabCatalogProducts' property
            // which is used when filtering product search results table
            tabObjectProducts = dtObjectProducts;
            gvObjectProductsList.PageSize = ProductCatalogSettings.TablePageSize;

            //Display paging if there are users found.
            if (dtObjectProducts.Rows.Count != 0)
            {
                btnRemoveAll.Visible = true;
                ObjectProductListPager.Visible = true;
                ObjectProductListPager.FillPaging(gvObjectProductsList.PageCount, gvObjectProductsList.PageIndex + 1, 5, gvObjectProductsList.PageSize, dtObjectProducts.Rows.Count);
            }
            else
            {
                switch (SelectedObjectType)
                {
                    case ComponentObjectEnum.Product:
                        gvObjectProductsList.EmptyDataText = GetLocalResourceObject("NoRelatedProductsErrorMessage").ToString();
                        break;
                    case ComponentObjectEnum.Bundle:
                        gvObjectProductsList.EmptyDataText = GetLocalResourceObject("NoBundleProductsErrorMessage").ToString();
                        break;
                    case ComponentObjectEnum.Catalog:
                        gvObjectProductsList.EmptyDataText = GetLocalResourceObject("NoCatalogProductsErrorMessage").ToString();
                        break;
                    case ComponentObjectEnum.Collection:
                        gvObjectProductsList.EmptyDataText = GetLocalResourceObject("NoCollectionProductsErrorMessage").ToString();
                        break;
                    case ComponentObjectEnum.Discount:
                        gvObjectProductsList.EmptyDataText = GetLocalResourceObject("NoDiscountProductsErrorMessage").ToString();
                        break;
                }
                btnRemoveAll.Visible = false;
                ObjectProductListPager.Visible = false;                
            }
        }

        /// <summary>
        /// Fire events for adding product to the product list of current object
        /// </summary>
        /// <remarks>
        /// The following parameter denotes what operation takes place:
        /// <list type="bullet">
        /// <item>Add - adds product to the product set of current object.</item>
        /// </list>        
        /// After adding operation finishes, method <see cref="RefreshExplorerPanel()"/> is called in order to 
        /// research for products and refresh product result table.
        /// </remarks>
        /// <author>Melon Team</author>                
        protected void gvProductList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                switch (SelectedObjectType)
                {
                    case ComponentObjectEnum.Product:
                        RelatedProducts relatedProducts = new RelatedProducts();
                        relatedProducts.ProductId = SelectedProductId;
                        relatedProducts.RelatedProductId = Convert.ToInt32(e.CommandArgument);

                        try
                        {
                            relatedProducts.Save();
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                        break;
                    case ComponentObjectEnum.Catalog:
                        CatalogProducts catProducts = new CatalogProducts();
                        catProducts.CatalogId = SelectedObjectId;
                        catProducts.ProductId = Convert.ToInt32(e.CommandArgument);

                        try
                        {
                            catProducts.Save();
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                        
                        break;
                    case ComponentObjectEnum.Bundle:                        
                        BundleProducts bundleProducts = new BundleProducts();
                        bundleProducts.BundleId = SelectedObjectId;
                        bundleProducts.ProductId = Convert.ToInt32(e.CommandArgument);

                        try
                        {
                            bundleProducts.Save();
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                       
                        break;
                    case ComponentObjectEnum.Collection:
                        CollectionProducts collProducts = new CollectionProducts();
                        collProducts.CollectionId = SelectedObjectId;
                        collProducts.ProductId = Convert.ToInt32(e.CommandArgument);

                        try
                        {
                            collProducts.Save();
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                        
                        break;
                    case ComponentObjectEnum.Discount:
                        DiscountProducts discountProducts = new DiscountProducts();                        
                        discountProducts.DiscountId = SelectedObjectId;
                        discountProducts.ProductId = Convert.ToInt32(e.CommandArgument);

                        try
                        {
                            discountProducts.Save();
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                        
                        break;
                }

                RefreshExplorerPanel(sender);
            }
        }

        /// <summary>
        /// Fire event for removing product from the product list of current object
        /// </summary>
        /// <remarks>
        /// The following parameter denotes what operation takes place:
        /// <list type="bullet">
        /// <item>Remove - opens product details page</item>
        /// </list>        
        /// After remove operation finishes, method <see cref="RefreshExplorerPanel()"/> is called in order to 
        /// research for products and refresh product result table.
        /// </remarks>
        /// <author>Melon Team</author>                
        protected void gvObjectProductsList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                try
                {
                    switch (SelectedObjectType)
                    { 
                        case ComponentObjectEnum.Product:
                            RelatedProducts.Delete(Convert.ToInt32(e.CommandArgument));
                            break;
                        case ComponentObjectEnum.Bundle:
                            BundleProducts.Delete(Convert.ToInt32(e.CommandArgument));
                            break;
                        case ComponentObjectEnum.Catalog:
                            CatalogProducts.Delete(Convert.ToInt32(e.CommandArgument));
                            break;
                        case ComponentObjectEnum.Collection:
                            CollectionProducts.Delete(Convert.ToInt32(e.CommandArgument));
                            break;
                        case ComponentObjectEnum.Discount:
                            DiscountProducts.Delete(Convert.ToInt32(e.CommandArgument));
                            break;
                    }                    
                }
                catch (ProductCatalogException ex)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = ex.Message;
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                RefreshExplorerPanel(sender);
            }
        }

        /// <summary>
        /// Event handler for event RowDataBound of GridView gvObjectProductsList.
        /// </summary>
        /// <remarks>
        /// This method sets already added product details.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvObjectProductsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (SelectedObjectType != ComponentObjectEnum.Discount)
                {
                    e.Row.Cells[4].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                if (SelectedObjectType == ComponentObjectEnum.Discount)
                {
                    Label lblDiscountStr = (Label)e.Row.FindControl("lblDiscountStr");
                    lblDiscountStr.Text = ((DataRowView)e.Row.DataItem)["DiscountList"].ToString();
                }
                else
                {
                    e.Row.Cells[4].Visible = false;
                }
         
            }
        }   

        /// <summary>
        /// This method adds all products from result table current page to the product set of current selected object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAllFromPage_Click(object sender, EventArgs e)
        {
            string ObjectIdList="";

            if (gvProductList.Rows.Count > 0)
            {
                foreach (GridViewRow row in gvProductList.Rows)
                {
                    ObjectIdList += ((HiddenField)row.Cells[0].FindControl("hfProductId")).Value + ",";
                }
            }

            if (ObjectIdList == String.Empty)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();

                switch (SelectedObjectType)
                {
                    case ComponentObjectEnum.Product:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotRelatedProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Bundle:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotBundleProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Catalog:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotCatalogProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Collection:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotCollectionProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Discount:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotDiscountProductsFoundToSaveErrorMessage"));
                        break;
                }
                
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);
                return;
            }

            ObjectIdList = ObjectIdList.Substring(0, ObjectIdList.Length - 1);
            try
            {
                switch (SelectedObjectType)
                {
                    case ComponentObjectEnum.Product:
                        RelatedProducts.Save(SelectedProductId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Bundle:
                        BundleProducts.Save(SelectedObjectId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Catalog:
                        CatalogProducts.Save(SelectedObjectId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Collection:
                        CollectionProducts.Save(SelectedObjectId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Discount:
                        DiscountProducts.Save(SelectedObjectId.Value, ObjectIdList);
                        break;
                }                
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs errArgs = new DisplayErrorMessageEventArgs();
                errArgs.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, errArgs);

                return;
            }

            RefreshExplorerPanel(sender);
        }

        /// <summary>
        /// This method adds all products from result table to the product set of current selected object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAll_Click(object sender, EventArgs e)
        {
            string ObjectIdList = "";

            if (tabResultProducts.Rows.Count > 0)
            {
                foreach (DataRow row in tabResultProducts.Rows)
                {
                    ObjectIdList += row["Id"].ToString() + ",";
                }
            }

            if (ObjectIdList == String.Empty)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();

                switch (SelectedObjectType)
                {
                    case ComponentObjectEnum.Product:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotRelatedProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Bundle:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotBundleProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Catalog:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotCatalogProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Collection:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotCollectionProductsFoundToSaveErrorMessage"));
                        break;
                    case ComponentObjectEnum.Discount:
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotDiscountProductsFoundToSaveErrorMessage"));
                        break;
                }
                
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);
                return;
            }

            ObjectIdList = ObjectIdList.Substring(0, ObjectIdList.Length - 1);
            try
            {
                switch (SelectedObjectType)
                {
                    case ComponentObjectEnum.Product:
                        RelatedProducts.Save(SelectedProductId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Bundle:
                        BundleProducts.Save(SelectedObjectId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Catalog:
                        CatalogProducts.Save(SelectedObjectId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Collection:
                        CollectionProducts.Save(SelectedObjectId.Value, ObjectIdList);
                        break;
                    case ComponentObjectEnum.Discount:
                        DiscountProducts.Save(SelectedObjectId.Value, ObjectIdList);                        
                        break;
                }                
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            RefreshExplorerPanel(sender);
        }

        /// <summary>
        /// This method removes all products from result table to the product set of current selected object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveAll_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "RemoveAll")
            {
                string ObjectIdList = "";

                if (tabObjectProducts.Rows.Count > 0)
                {
                    foreach (DataRow row in tabObjectProducts.Rows)
                    {
                        ObjectIdList += row["Id"].ToString() + ",";
                    }
                }

                if (ObjectIdList == String.Empty)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();

                    switch (SelectedObjectType)
                    {
                        case ComponentObjectEnum.Product:
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotRelatedProductsFoundToDeleteErrorMessage"));
                            break;
                        case ComponentObjectEnum.Bundle:
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotBundleProductsFoundToDeleteErrorMessage"));
                            break;
                        case ComponentObjectEnum.Catalog:
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotCatalogProductsFoundToDeleteErrorMessage"));
                            break;
                        case ComponentObjectEnum.Collection:
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotCollectionProductsFoundToDeleteErrorMessage"));
                            break;
                        case ComponentObjectEnum.Discount:
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("NotDiscountProductsFoundToDeleteErrorMessage"));
                            break;
                    }

                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);
                    return;
                }

                ObjectIdList = ObjectIdList.Substring(0, ObjectIdList.Length - 1);
                try
                {
                    switch (SelectedObjectType)
                    {
                        case ComponentObjectEnum.Product:
                            RelatedProducts.DeleteByRelatedProductList(ObjectIdList);
                            break;
                        case ComponentObjectEnum.Bundle:
                            BundleProducts.DeleteByBundleProductList(ObjectIdList);                            
                            break;
                        case ComponentObjectEnum.Catalog:
                            CatalogProducts.DeleteByCatalogProductList(ObjectIdList);                            
                            break;
                        case ComponentObjectEnum.Collection:
                            CollectionProducts.DeleteByCollectionProductList(ObjectIdList);                            
                            break;
                        case ComponentObjectEnum.Discount:
                            DiscountProducts.DeleteByDiscountProductList(ObjectIdList);                           
                            break;
                    }
                }
                catch (ProductCatalogException ex)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = ex.Message;
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                RefreshExplorerPanel(sender);
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
        /// <date>11/06/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ProductSearchCriteria searchCriteria = CollectSearchCriteria();
            LoadProductList(searchCriteria);

            interfaceSearchCriteria = searchCriteria;
            hfProductSetSearchStatus.Value = "expanded";
        }

        /// <summary>
        /// Returns the currently entered search criteria to filter for products
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/06/2009</date>
        /// <returns></returns>
        private ProductSearchCriteria CollectSearchCriteria()
        {
            ProductSearchCriteria searchCriteria = new ProductSearchCriteria();

            if (hfProductSetCategoryList.Value != String.Empty)
            {
                searchCriteria.categoryIdList = hfProductSetCategoryList.Value;
            }
            if (hfProductSetCategoryName.Value != String.Empty)
            {
                searchCriteria.categoryNameList = hfProductSetCategoryName.Value;
            }
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

            if (ddlActive.SelectedValue != "")
            {
                searchCriteria.ActiveOnly = ddlActive.SelectedValue == "1";
            }

            if (ddlInStock.SelectedValue != "")
            {
                searchCriteria.InStockOnly = ddlInStock.SelectedValue == "1";
            }

            if (ddlFeatured.SelectedValue != "")
            {
                searchCriteria.FeaturedOnly = ddlFeatured.SelectedValue == "1";
            }

            return searchCriteria;
        }

        /// <summary>
        /// Load category hierarchical list with categories from database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        private void LoadCategoryList()
        {
            DataTable dtCategoryList = Category.GetHierarchicalList((bool?)null);
            if (dtCategoryList.Rows.Count > 0)
            {
                String str = "---- ";
                foreach (DataRow row in dtCategoryList.Rows)
                {
                    String s = "";
                    for (int i = 0; i < Convert.ToInt32(row["CatLevel"]); i++)
                    {
                        s += str;
                    }

                    ListItem item = new ListItem(s.Replace(" ", HttpUtility.HtmlDecode("&nbsp;")) + row["Name"].ToString(), row["Id"].ToString() + ";" + row["CategoryFullName"].ToString());
                    item.Attributes.Add("visible", "false");
                    lbCategoryList.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Event handler for event PageChange for ProductSet table in user control Pager.ascx.
        /// </summary>
        /// <remarks>
        ///     Set property PageIndex of GridView gvProductList to the new page number and then 
        ///     calls method <see cref="LoadProductList"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>11/10/2009</date>
        protected void ProductSetPager_PageChanged(object sender, Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Pager.PagerEventArgs e)
        {
            if (this.BaseParentControl.CurrentUser != null)
            {
                gvProductList.PageIndex = e.NewPage;
                ProductSearchCriteria searchCriteria = CollectSearchCriteria();
                LoadProductList(searchCriteria);
            }
            else
            {
                this.BaseParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event PageChange for gvObjectProducts table in user control Pager.ascx.
        /// </summary>
        /// <remarks>
        ///     Set property PageIndex of GridView gvObjectProducts to the new page number and then 
        ///     calls method <see cref="LoadObjectProductList"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>11/10/2009</date>
        protected void ObjectProductListPager_PageChanged(object sender, Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Pager.PagerEventArgs e)
        {
            if (this.BaseParentControl.CurrentUser != null)
            {
                gvObjectProductsList.PageIndex = e.NewPage;

                LoadObjectProductsList();
            }
            else
            {
                this.BaseParentControl.RedirectToLoginPage();
            }
        }

        private void RefreshExplorerPanel(object sender)
        {
            switch (SelectedObjectType)
            {
                case ComponentObjectEnum.Product:
                    LoadProductEventArgs productArgs = new LoadProductEventArgs();
                    productArgs.SelectedCategoryId = SelectedObjectId;
                    productArgs.SelectedProductId = SelectedProductId;
                    productArgs.SelectedObjectType = ComponentObjectEnum.Product;
                    productArgs.SelectedTab = ProductCatalogTabs.RelatedProducts;
                    productArgs.RefreshExplorer = false;
                    productArgs.SearchCriteria = interfaceSearchCriteria;
                    productArgs.IsFirstLoad = false;
                    this.BaseParentControl.OnLoadProduct(sender, productArgs);                    
                    break;
                case ComponentObjectEnum.Catalog:
                    LoadCatalogEventArgs catArgs = new LoadCatalogEventArgs();
                    catArgs.SelectedCatalogId = SelectedObjectId;
                    catArgs.RefreshExplorer = true;
                    catArgs.SelectedObjectType = ComponentObjectEnum.Catalog;
                    catArgs.SelectedTab = SelectedTab;
                    catArgs.SearchCriteria = interfaceSearchCriteria;
                    catArgs.IsFirstLoad = false;
                    this.BaseParentControl.OnLoadCatalog(sender, catArgs);
                    break;
                case ComponentObjectEnum.Collection:
                    LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                    collArgs.SelectedCollectionId = SelectedObjectId;
                    collArgs.RefreshExplorer = true;
                    collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                    collArgs.SelectedTab = SelectedTab;
                    collArgs.SearchCriteria = interfaceSearchCriteria;
                    collArgs.IsFirstLoad = false;
                    this.BaseParentControl.OnLoadCollection(sender, collArgs);
                    break;
                case ComponentObjectEnum.Bundle:
                    LoadBundleEventArgs bundleArgs = new LoadBundleEventArgs();
                    bundleArgs.SelectedBundleId = SelectedObjectId;
                    bundleArgs.RefreshExplorer = true;
                    bundleArgs.SelectedObjectType = ComponentObjectEnum.Bundle;
                    bundleArgs.SelectedTab = SelectedTab;
                    bundleArgs.SearchCriteria = interfaceSearchCriteria;
                    bundleArgs.IsFirstLoad = false;
                    this.BaseParentControl.OnLoadBundle(sender, bundleArgs);
                    break;
                case ComponentObjectEnum.Discount:
                    LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                    discountArgs.SelectedDiscountId = SelectedObjectId;
                    discountArgs.RefreshExplorer = true;
                    discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                    discountArgs.SelectedTab = SelectedTab;
                    discountArgs.SearchCriteria = interfaceSearchCriteria;
                    discountArgs.IsFirstLoad = false;
                    this.BaseParentControl.OnLoadDiscount(sender, discountArgs);
                    break;
            }            
        }

        /// <summary>
        /// Initializes search fields information
        /// </summary>
        private void SetSearchPanelFilter()
        {
            hfProductSetCategoryList.Value = interfaceSearchCriteria.categoryIdList;
            hfProductSetCategoryName.Value = interfaceSearchCriteria.categoryNameList;
            txtKeywords.Text = interfaceSearchCriteria.keywords;
            txtPriceFrom.Text = interfaceSearchCriteria.PriceFrom.ToString();
            txtPriceTo.Text = interfaceSearchCriteria.PriceTo.ToString();

            if (interfaceSearchCriteria.keywordsPlaceholders != null)
            {
                cbxlSearchCriteria.Items[0].Selected = interfaceSearchCriteria.keywordsPlaceholders.Exists(field => field == ProductSearchFields.Code);
                cbxlSearchCriteria.Items[1].Selected = interfaceSearchCriteria.keywordsPlaceholders.Exists(field => field == ProductSearchFields.Name);
                cbxlSearchCriteria.Items[2].Selected = interfaceSearchCriteria.keywordsPlaceholders.Exists(field => field == ProductSearchFields.Description);
                cbxlSearchCriteria.Items[3].Selected = interfaceSearchCriteria.keywordsPlaceholders.Exists(field => field == ProductSearchFields.Tags);
            }

            if (interfaceSearchCriteria.ActiveOnly != null)
            {
                ddlActive.SelectedValue = Convert.ToInt16(interfaceSearchCriteria.ActiveOnly).ToString();
            }

            if (interfaceSearchCriteria.InStockOnly != null)
            {
                ddlInStock.SelectedValue = Convert.ToInt16(interfaceSearchCriteria.InStockOnly).ToString();
            }

            if (interfaceSearchCriteria.FeaturedOnly != null)
            {
                ddlFeatured.SelectedValue = Convert.ToInt16(interfaceSearchCriteria.FeaturedOnly).ToString();
            }
        }
    }
}
