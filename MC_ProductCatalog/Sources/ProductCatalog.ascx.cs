using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.Providers;
using System.Web.UI.HtmlControls;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Represents the main control of the Product Catalog component, used to be included in the desired page on the web site where component should be integrated.
    /// The control inherits most of its functionality from <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl"/> and is a critical part of the Product Catalog component engine.
    /// In short this control manages all the logic flow and by using a system of events decides how to proceed on every step. This is deeply connected with the usage of dynamic control load, which is implemented here.
    /// </summary>
    /// <remarks>
    ///		<para>
    ///			Basic workflow of the Product Catalog component engine:
    ///			<list type="bullet">
    ///				<listheader>
    ///					As a general rule in order events and data to be maintained between page recycling the state of the controls (dynamic or not) should be preserved. 
    ///					While in general ViewState helps with the static controls, for dynamic controls a little additional effort is needed – they must be recreated at proper time so the ASP.NET engine can match their ViewState from the previous page iteration.
    ///					In the suggested algorithm the following system was chosen:
    ///				</listheader>
    ///				<item>Before load of the ViewState a special refresh function is called which recreates the controls.</item>
    ///				<item>Since the refresh function should somehow know which controls to recreate 
    ///                 and the ViewState is obviously not the place for that information, the control state is used instead. 
    ///                 This is simply because the control state is loaded before the viewstate.</item>
    ///				<item>Moreover a special structure is needed to store the info about the controls in the control state. 
    ///             That structure contains the control file name (in case of web control) or the control class (in case of custom control). 
    ///             Because these child controls hold some data which depends on the other controls, 
    ///             a special init blocks are needed so the control will match its state from the previous page request. 
    ///             These init blocks are implemented as instance methods Initializer of the user controls. 
    ///             Finally Initializer method has arguments with real values. These values are the second part of this <see cref="Melon.Components.ProductCatalog.ComponentEngine.ControlInitInfo">ControlInitInfo</see> class.</item>
    ///				<item>The above structure is stored in the control state and being read between cycles by the <see cref="Melon.Components.ProductCatalog.ComponentEngine.InnerBaseProductCatalogControl.Refresh">Refresh</see> function. 
    ///				This function on the other hand calls the function <see cref="Melon.Components.ProductCatalog.ComponentEngine.InnerBaseProductCatalogControl.LoadCustomControl(Melon.Components.ProductCatalog.ComponentEngine.ControlInitInfo[])">LoadCustomControl</see> which actually cares to load the controls and call the init blocks with their argument values.</item>
    ///				<item>The main control(Product Catalog Admiistration page) inherits <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl"/> which defines a set of delegates, events and methods which provide the communication system between the controls. 
    ///				Every event describes a specific action within the component. This action can happen in each of the child controls. 
    ///				Moreover every event has a public fire method, which allows the child controls to initiate the execution of attached event handler(s). 
    ///				All the handling of the events is done in this via handling its base’s events.</item>
    ///				<item>This parent class <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl">BaseProductCatalog</see> inherits <see cref="Melon.Components.ProductCatalog.ComponentEngine.InnerBaseProductCatalogControl">InnerBaseProductCatalog</see> class where control state functionality is implemented.</item>
    ///				<item>On the other hand all child controls inherit <see cref="Melon.Components.ProductCatalog.ComponentEngine.ProductCatalogControl"/> class, which defines the following set of features:
    ///					<list type="bullet">
    ///						<item>Property defining the parent control, which because of the above and the run-time creation of web controls should be the BaseProductCatalogControl (this explains why this control handles its base events).</item>
    ///						<item>Implementation of IsControlPostBack feature, indicating that the control was loaded before and if the postback occurred by it.</item>
    ///					</list>
    ///				</item>
    ///			</list>
    ///		</para>
    /// </remarks>
    /// <seealso cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl"/>
    /// <seealso cref="Melon.Components.ProductCatalog.ComponentEngine.InnerBaseProductCatalogControl"/>
    /// <seealso cref="Melon.Components.ProductCatalog.ComponentEngine.ProductCatalogControl"/>
    /// <seealso cref="Melon.Components.ProductCatalog.ComponentEngine.ControlInitInfo"/>
    public partial class CodeBehind_ProductCatalog : BaseProductCatalogControl
    {        

        /// <summary>
        /// Attach event handlers to the controls' events and Product Catalog events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>31/08/2009</date>
        protected override void OnInit(EventArgs e)
        {
            this.lbOpenCategoriesAndProducts.Click+=new EventHandler(lbOpenCategoriesAndProducts_Click);
            this.lbOpenCatalogs.Click+=new EventHandler(lbOpenCatalogs_Click);
            this.lbOpenBundles.Click+=new EventHandler(lbOpenBundles_Click);
            this.lbOpenCollections.Click+=new EventHandler(lbOpenCollections_Click);
            this.lbOpenDiscounts.Click+=new EventHandler(lbOpenDiscounts_Click);
            this.lbExport.Click+=new EventHandler(lbExport_Click);
            this.lbSearch.Click+=new EventHandler(lbSearch_Click);
            this.lbOpenUsers.Click+=new EventHandler(lbOpenUsers_Click);
            this.lbOpenReviews.Click+=new EventHandler(lbOpenReviews_Click);
            this.lbOpenUnits.Click+=new EventHandler(lbOpenUnits_Click);

            this.LoadAccessDeniedEvent += new LoadAccessDeniedEventHandler(LoadAccessDenied);
            this.LoadCategoryEvent += new LoadCategoryEventHandler(LoadCategory);
            this.LoadProductEvent+=new LoadProductEventHandler(LoadProduct);
            this.LoadProductListEvent += new LoadProductListEventHandler(LoadProductList);
            this.LoadBundleEvent += new LoadBundleEventHandler(ProductCatalog_LoadBundleEvent);
            this.LoadCatalogEvent += new LoadCatalogEventHandler(ProductCatalog_LoadCatalogEvent);
            this.LoadCollectionEvent += new LoadCollectionEventHandler(ProductCatalog_LoadCollectionEvent);
            this.LoadDiscountEvent += new LoadDiscountEventHandler(ProductCatalog_LoadDiscountEvent);
            this.LoadExportEvent += new LoadExportEventHandler(ProductCatalog_LoadExportEvent);
            this.LoadMeasurementUnitEvent += new LoadMeasurementUnitEventHandler(ProductCatalog_LoadMeasurementUnitEvent);
            this.LoadProductReviewEvent += new LoadProductReviewEventHandler(ProductCatalog_LoadProductReviewEvent);
            this.LoadSearchEvent += new LoadSearchEventHandler(ProductCatalog_LoadSearchEvent);
            this.LoadUsersEvent += new LoadUserEventHandler(ProductCatalog_LoadUsersEvent);

            this.DisplayErrorMessageEvent += new DisplayErrorMessageEventHandler(DisplayErrorMessage);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.   
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Check the current logged user. If there is no such user then redirect to login page.
        ///         If there is logged user but it is not Product Catalog user "Access is denied" error message is displayed.
        ///     </para>
        ///     <para>
        ///         After user is authenticated and authoriozed in the system Category and Products user control 
        ///         is visualized in the administration panel. 
        ///     </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>09/01/2010</date>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                CurrentUser = User.Load();

                if (CurrentUser == null || CurrentUser.Adapter==null)
                {
                    RedirectToLoginPage();
                }
                else if (CurrentUser.Adapter.IsAdmin)
                {
                    //The currently logged user is Product Catalog user. Display Product Catalog screen.
                    lbOpenCategoriesAndProducts_Click(sender, e);
                }
                else
                {
                    //The currently logged user is not Product Catalog user. Display screen Access Denied and hide menu.
                    LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                    args.IsUserLoggedRole = true;                    
                    LoadAccessDenied(sender, args);
                }                
            }
            else
            {
                //Check whether there is currently logged user because Product Catalog component is accessible only for logged user.
                if (CurrentUser == null)
                {
                    RedirectToLoginPage();
                }
            }            
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenCategoriesAndProducts.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Products" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadCategoryEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenCategoriesAndProducts_Click(object sender, EventArgs e)
        {            
            LoadCategoryEventArgs args = new LoadCategoryEventArgs();
            args.SelectedCategoryId = (int?)null;
            args.RefreshExplorer = false;
            args.SelectedObjectType = ComponentObjectEnum.Unknown;
            OnLoadCategory(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenCatalogs.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Catalogs" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadCatalogEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenCatalogs_Click(object sender, EventArgs e)
        {            
            LoadCatalogEventArgs args = new LoadCatalogEventArgs();
            args.SelectedCatalogId = (int?)null;
            args.RefreshExplorer = true;
            args.SelectedObjectType = ComponentObjectEnum.Catalog;
            OnLoadCatalog(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenBundles.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Bundles" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadBundleEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenBundles_Click(object sender, EventArgs e)
        {            
            LoadBundleEventArgs args = new LoadBundleEventArgs();
            args.SelectedBundleId = (int?)null;
            args.RefreshExplorer = true;
            args.SelectedObjectType = ComponentObjectEnum.Bundle;
            OnLoadBundle(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenCollections.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Collections" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadCollectionEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenCollections_Click(object sender, EventArgs e)
        {            
            LoadCollectionEventArgs args = new LoadCollectionEventArgs();
            args.SelectedCollectionId = (int?)null;
            args.RefreshExplorer = true;
            args.SelectedObjectType = ComponentObjectEnum.Collection;
            OnLoadCollection(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenDiscounts.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Discounts" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadDiscountEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenDiscounts_Click(object sender, EventArgs e)
        {            
            LoadDiscountEventArgs args = new LoadDiscountEventArgs();
            args.SelectedDiscountId = (int?)null;            
            args.RefreshExplorer = true;
            args.SelectedObjectType = ComponentObjectEnum.Discount;
            OnLoadDiscount(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbExport.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Export" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadExportEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbExport_Click(object sender, EventArgs e)
        {            
            LoadExportEventArgs args = new LoadExportEventArgs();            
            OnLoadExport(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbSeach.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Search" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadSearchEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbSearch_Click(object sender, EventArgs e)
        {            
            LoadSearchEventArgs args = new LoadSearchEventArgs();            
            OnLoadSearch(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenUsers.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Users" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadUserEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenUsers_Click(object sender, EventArgs e)
        {            
            LoadUserEventArgs args = new LoadUserEventArgs();
            OnLoadUser(sender, args);            
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenReviews.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Reviews" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadProductReviewEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenReviews_Click(object sender, EventArgs e)
        {            
            LoadProductReviewEventArgs args = new LoadProductReviewEventArgs();
            args.SelectedProductReviewId = 1;
            OnLoadProductReview(sender, args);
        }

        /// <summary>
        ///Event handler for event Click on button lbOpenUnits.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It selects menu item "Units" in the Product Catalog menu and calls 
        ///         method <see cref="Melon.Components.ProductCatalog.ComponentEngine.BaseProductCatalogControl.OnLoadMeasurementUnitEvent"/>.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author></author>
        /// <date>10/09/2010</date>
        protected void lbOpenUnits_Click(object sender, EventArgs e)
        {            
            LoadMeasurementUnitEventArgs args = new LoadMeasurementUnitEventArgs();            
            OnLoadMeasurementUnit(sender, args);
        }


        /// <summary>
        /// Redirects to LoginUrl page if specified otherwise load user control for access denied.
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        public override void RedirectToLoginPage()
        {
            if (String.IsNullOrEmpty(ProductCatalogSettings.LoginUrl) || this.ResolveUrl(ProductCatalogSettings.LoginUrl) == Request.RawUrl)
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();                
                args.IsUserLoggedRole = true;
                LoadAccessDenied(this, args);
            }
            else
            {
                Response.Redirect(ProductCatalogSettings.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(Request.RawUrl), true);
            }
        }

        /// <summary>
        /// Loads user control AccessDenied.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        protected void LoadAccessDenied(object sender, LoadAccessDeniedEventArgs e)
        {
            divNavigation.Visible = false;

            ControlInitInfo cntrlAccessDenied = new ControlInitInfo("AccessDenied.ascx", false, new object[] { e.IsUserLoggedRole });
            LoadCustomControl(new ControlInitInfo[] { cntrlAccessDenied });
        }

        /// <summary>
        /// Loads user control Category.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        protected void LoadCategory(object sender, LoadCategoryEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Category);

            ControlInitInfo cntrlExplorer = new ControlInitInfo("CategoryExplorer.ascx", e.RefreshExplorer, new object[] { e.SelectedCategoryId!=null ? e.SelectedCategoryId : e.ParentCategoryId });                       
            ControlInitInfo cntrlCategory = new ControlInitInfo("Category.ascx", true, new object[] { e.ParentCategoryId, e.SelectedCategoryId, e.SelectedObjectType, e.SelectedTab, e.Message});
            LoadCustomControl(new ControlInitInfo[] { cntrlExplorer, cntrlCategory });
        }

        /// <summary>
        /// Loads user control Product.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        protected void LoadProduct(object sender, LoadProductEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Product);

            ControlInitInfo cntrlExplorer = new ControlInitInfo("CategoryExplorer.ascx", e.RefreshExplorer, new object[] { e.SelectedCategoryId });
            ControlInitInfo cntrlProduct     = new ControlInitInfo("Product.ascx", true, new object[] { e.SelectedCategoryId, e.SelectedProductId, e.SelectedObjectType,e.SelectedTab, e.SearchCriteria,e.IsFirstLoad, e.Message});
            LoadCustomControl(new ControlInitInfo[] { cntrlExplorer, cntrlProduct });
        }

        /// <summary>
        /// Loads user control ProductList.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        protected void LoadProductList(object sender, LoadProductListEventArgs e)
        {
            ControlInitInfo cntrlExplorer = new ControlInitInfo("CategoryExplorer.ascx", false, new object[] { e.SelectedCategoryId });
            ControlInitInfo cntrlProductList = new ControlInitInfo("ProductList.ascx", true, new object[] { e.SelectedCategoryId, e.SelectedObjectType, e.SelectedTab, e.RefreshExplorer});
            LoadCustomControl(new ControlInitInfo[] { cntrlExplorer, cntrlProductList });
        }

        /// <summary>
        /// Loads user control Users.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadUsersEvent(object sender, LoadUserEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Users);

            ControlInitInfo cntrlUsers = new ControlInitInfo("Users.ascx", true, new object[] { e.SelectedUserId });
            LoadCustomControl(new ControlInitInfo[] {cntrlUsers});
        }

        /// <summary>
        /// Loads user control Search.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadSearchEvent(object sender, LoadSearchEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Search);

            ControlInitInfo cntrlSearch = new ControlInitInfo("Search.ascx", true, new object[] { });
            LoadCustomControl(new ControlInitInfo[] {cntrlSearch});
        }

        /// <summary>
        /// Loads user control ProductReview.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadProductReviewEvent(object sender, LoadProductReviewEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.ProductReview);

            ControlInitInfo cntrlProductReview = new ControlInitInfo("ProductReview.ascx", true, new object[] {e.SelectedProductReviewId });
            LoadCustomControl(new ControlInitInfo[] { cntrlProductReview });
        }

        /// <summary>
        /// Loads user control MeasurementUnit.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadMeasurementUnitEvent(object sender, LoadMeasurementUnitEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.MeasurementUnit);

            ControlInitInfo cntrlMeasurementUnit = new ControlInitInfo("MeasurementUnit.ascx", true, new object[] { });
            LoadCustomControl(new ControlInitInfo[] { cntrlMeasurementUnit });
        }

        /// <summary>
        /// Loads user control Export.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadExportEvent(object sender, LoadExportEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Export);

            ControlInitInfo cntrlExport = new ControlInitInfo("Export.ascx", true, new object[] { });
            LoadCustomControl(new ControlInitInfo[] { cntrlExport });
        }

        /// <summary>
        /// Loads user control Discount.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadDiscountEvent(object sender, LoadDiscountEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Discount);

            ControlInitInfo cntrlExplorer = new ControlInitInfo("Explorer.ascx", e.RefreshExplorer, new object[] { e.SelectedDiscountId, e.SelectedObjectType });
            ControlInitInfo cntrlDiscount = new ControlInitInfo("Discount.ascx", true, new object[] { e.SelectedDiscountId, e.SelectedObjectType, e.SelectedTab, e.SearchCriteria, e.IsFirstLoad, e.Message });
            LoadCustomControl(new ControlInitInfo[] { cntrlExplorer, cntrlDiscount });
        }

        /// <summary>
        /// Loads user control Collection.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadCollectionEvent(object sender, LoadCollectionEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Collection);

            ControlInitInfo cntrlExplorer = new ControlInitInfo("Explorer.ascx", e.RefreshExplorer, new object[] { e.SelectedCollectionId, e.SelectedObjectType });
            ControlInitInfo cntrlCollection = new ControlInitInfo("Collection.ascx", true, new object[] { e.SelectedCollectionId, e.SelectedObjectType, e.SelectedTab, e.SearchCriteria, e.IsFirstLoad, e.Message });
            LoadCustomControl(new ControlInitInfo[] { cntrlExplorer, cntrlCollection });
        }

        /// <summary>
        /// Loads user control Catalog.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadCatalogEvent(object sender, LoadCatalogEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Catalog);

            ControlInitInfo cntrlExplorer = new ControlInitInfo("Explorer.ascx", e.RefreshExplorer, new object[] { e.SelectedCatalogId, e.SelectedObjectType });
            ControlInitInfo cntrlCatalog = new ControlInitInfo("Catalog.ascx", true, new object[] { e.SelectedCatalogId, e.SelectedObjectType, e.SelectedTab, e.SearchCriteria, e.IsFirstLoad, e.Message });
            LoadCustomControl(new ControlInitInfo[] { cntrlExplorer, cntrlCatalog });
        }

        /// <summary>
        /// Loads user control Bundle.ascx.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        void ProductCatalog_LoadBundleEvent(object sender, LoadBundleEventArgs e)
        {
            SetTabStyles(ComponentObjectEnum.Bundle);
            
            ControlInitInfo cntrlExplorer = new ControlInitInfo("Explorer.ascx", e.RefreshExplorer, new object[] { e.SelectedBundleId, e.SelectedObjectType });
            ControlInitInfo cntrlBundle = new ControlInitInfo("Bundle.ascx", true, new object[] { e.SelectedBundleId, e.SelectedObjectType, e.SelectedTab, e.SearchCriteria, e.IsFirstLoad, e.Message });
            LoadCustomControl(new ControlInitInfo[] { cntrlExplorer, cntrlBundle });
        }

        /// <summary>
        /// Display Error message occured in Product Catalog Component
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/10/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DisplayErrorMessage(object sender, DisplayErrorMessageEventArgs e)
        {
            lblErrorMessage.Text = e.ErrorMessage;
        }

        /// <summary>
        /// Set css classes to menu items in Administration panel to visualize selected and unselected menu tabs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        public void SetTabStyles(ComponentObjectEnum objectType)
        {
            switch (objectType)
            { 
                case ComponentObjectEnum.Bundle:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Catalog:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Category:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first_selected");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Collection:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Discount:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Export:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.MeasurementUnit:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last_selected");
                    break;
                case ComponentObjectEnum.Product:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first_selected");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.ProductReview:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Search:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Unknown:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
                case ComponentObjectEnum.Users:
                    tdOpenCategoriesAndProducts.Attributes.Add("class", "mc_pc_menu_item_first");
                    tdOpenCatalogs.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenBundles.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenCollections.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenDiscounts.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdExport.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdSearch.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUsers.Attributes.Add("class", "mc_pc_menu_item_middle_selected");
                    tdOpenReviews.Attributes.Add("class", "mc_pc_menu_item_middle");
                    tdOpenUnits.Attributes.Add("class", "mc_pc_menu_item_last");
                    break;
            }
        }
    }
}
