using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using Melon.Components.ProductCatalog.Configuration;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    public partial class CodeBehind_Export : ProductCatalogControl
    {
        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/03/2009</date>
        protected override void OnInit(EventArgs e)
        {
            btnExport.Click+=new EventHandler(btnExport_Click);
            btnExport.OnClientClick = "javascript:return OnExportObjects('chklCategory','chklProduct','chklBundle','chklCatalog','chklCollection','chklDiscount', '" + this.GetLocalResourceObject("CheckItemAlertMessage").ToString() + "');";

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes user control information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {

            }
        }

        /// <summary>
        /// Exports component`s object information in .xls format file.
        /// </summary>
        /// <remarks>
        /// Exports details for all component objects - categories, products, bundles, catalogs, collections and discounts.
        /// This web page user interface provides options to select which objects details to export.
        /// Objects` details are exported by calling List method of type <see cref="melon.Components.ProductCatalog.Export"/>.
        /// Export information is rendered on another web page where xls file-generation method is called. 
        /// For more information see <see cref="Melon.Components.ProductCatalog.Utility.GenerateExportFile()"/> method.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportObjectParams exportObjParams=new ExportObjectParams();
            exportObjParams.colCategoryCode = chklCategory.Items.FindByValue("Code").Selected;
            exportObjParams.colCategoryName = chklCategory.Items.FindByValue("Name").Selected;
            exportObjParams.colCategoryShortDesc = chklCategory.Items.FindByValue("ShortDesc").Selected;
            exportObjParams.colCategoryLongDesc = chklCategory.Items.FindByValue("LongDesc").Selected;
            exportObjParams.colCategoryTags = chklCategory.Items.FindByValue("Tags").Selected;
            exportObjParams.colCategoryUnit = chklCategory.Items.FindByValue("Unit").Selected;
            exportObjParams.colCategoryIsActive = chklCategory.Items.FindByValue("IsActive").Selected;
            exportObjParams.colCategoryDynProp = chklCategory.Items.FindByValue("DynProp").Selected;

            exportObjParams.colProductCode = chklProduct.Items.FindByValue("Code").Selected;
            exportObjParams.colProductName = chklProduct.Items.FindByValue("Name").Selected;
            exportObjParams.colProductShortDesc = chklProduct.Items.FindByValue("ShortDesc").Selected;
            exportObjParams.colProductLongDesc = chklProduct.Items.FindByValue("LongDesc").Selected;
            exportObjParams.colProductTags = chklProduct.Items.FindByValue("Tags").Selected;
            exportObjParams.colProductIsActive = chklProduct.Items.FindByValue("IsActive").Selected;
            exportObjParams.colProductIsInStock = chklProduct.Items.FindByValue("IsInStock").Selected;
            exportObjParams.colProductIsFeatured = chklProduct.Items.FindByValue("IsFeatured").Selected;
            exportObjParams.colProductCommonPrice = chklProduct.Items.FindByValue("CommonPrice").Selected;
            exportObjParams.colProductUnit = chklProduct.Items.FindByValue("Unit").Selected;
            exportObjParams.colProductManufacturer = chklProduct.Items.FindByValue("Manufacturer").Selected;
            exportObjParams.colProductCategoryList = chklProduct.Items.FindByValue("CategoryList").Selected;
            exportObjParams.colProductDynProp = chklProduct.Items.FindByValue("DynProp").Selected;
            exportObjParams.colProductAppliedAccounts = chklProduct.Items.FindByValue("AppliedDiscounts").Selected;

            exportObjParams.colBundleCode = chklBundle.Items.FindByValue("Code").Selected;
            exportObjParams.colBundleName = chklBundle.Items.FindByValue("Name").Selected;
            exportObjParams.colBundleCategoryList = chklBundle.Items.FindByValue("CategoryList").Selected;
            exportObjParams.colBundleShortDesc = chklBundle.Items.FindByValue("ShortDesc").Selected;
            exportObjParams.colBundleLongDesc = chklBundle.Items.FindByValue("LongDesc").Selected;
            exportObjParams.colBundleTags = chklBundle.Items.FindByValue("Tags").Selected;
            exportObjParams.colBundleCommonPrice = chklBundle.Items.FindByValue("CommonPrice").Selected;
            exportObjParams.colBundleIsActive = chklBundle.Items.FindByValue("IsActive").Selected;
            exportObjParams.colBundleDynProp = chklBundle.Items.FindByValue("DynProp").Selected;

            exportObjParams.colCatalogCode = chklCatalog.Items.FindByValue("Code").Selected;
            exportObjParams.colCatalogName = chklCatalog.Items.FindByValue("Name").Selected;            
            exportObjParams.colCatalogShortDesc = chklCatalog.Items.FindByValue("ShortDesc").Selected;
            exportObjParams.colCatalogLongDesc = chklCatalog.Items.FindByValue("LongDesc").Selected;
            exportObjParams.colCatalogTags = chklCatalog.Items.FindByValue("Tags").Selected;            
            exportObjParams.colCatalogIsActive = chklCatalog.Items.FindByValue("IsActive").Selected;

            exportObjParams.colCollectionCode = chklCollection.Items.FindByValue("Code").Selected;
            exportObjParams.colCollectionName = chklCollection.Items.FindByValue("Name").Selected;
            exportObjParams.colCollectionShortDesc = chklCollection.Items.FindByValue("ShortDesc").Selected;
            exportObjParams.colCollectionLongDesc = chklCollection.Items.FindByValue("LongDesc").Selected;
            exportObjParams.colCollectionTags = chklCollection.Items.FindByValue("Tags").Selected;
            exportObjParams.colCollectionIsActive = chklCollection.Items.FindByValue("IsActive").Selected;

            
            exportObjParams.colDiscountName = chklDiscount.Items.FindByValue("Name").Selected;
            exportObjParams.colDiscountDescription = chklDiscount.Items.FindByValue("Description").Selected;
            exportObjParams.colDiscountDiscountType = chklDiscount.Items.FindByValue("DiscountType").Selected;
            exportObjParams.colDiscountFrom = chklDiscount.Items.FindByValue("DiscountDateFrom").Selected;
            exportObjParams.colDiscountTo = chklDiscount.Items.FindByValue("DiscountDateTo").Selected;
            exportObjParams.colDiscountValue = chklDiscount.Items.FindByValue("DiscountValue").Selected;
            exportObjParams.discountCurrencyStr = ProductCatalogSettings.Currency;
            exportObjParams.colDiscountIsActive = chklDiscount.Items.FindByValue("IsActive").Selected;

            DataSet dsExport = Export.List(exportObjParams);

            Session["ExportCriteria"] = dsExport;
            Response.Redirect(ProductCatalogSettings.BasePath + "Sources/ExportToExcel.aspx");
        }
    }
}
