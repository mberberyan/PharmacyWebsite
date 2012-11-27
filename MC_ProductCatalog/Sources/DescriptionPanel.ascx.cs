using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    public partial class CodeBehind_DescriptionPanel : ProductCatalogControl
    {        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                // show current category`s name and current product`s name and code in Description panel
                LoadDescriptionPanel();
            }
        }

        /// <summary>
        /// Loads desciprtion panel under Category Explorer
        /// Product Category: [Name]
        /// Product Name: [Name]
        /// Product Code: [Name]
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/8/2009</date>
        private void LoadDescriptionPanel()
        {
            if (SelectedObjectId == null || SelectedObjectId == 0)
            {
                divDescriptionPanel.Visible = false;
                return;
            }

            divDescriptionPanel.Visible = true;

            switch (SelectedObjectType)
            {
                case ComponentObjectEnum.Category:
                    Category category = Category.Load(SelectedObjectId);
                    if (category == null)
                    {
                        DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                        args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CategoryNotExistErrorMessage"));
                        this.ParentControl.OnDisplayErrorMessageEvent(null, args);

                        return;
                    }
                    locObjectDescName.Text = GetLocalResourceObject("CategoryName").ToString();
                    locObjectDescCode.Text = GetLocalResourceObject("CategoryCode").ToString();
                    lblObjectDescName.Text = " <b>" + category.Name + "</b>";
                    lblObjectDescCode.Text = " <b>" + category.Code + "</b>";

                    break;
                case ComponentObjectEnum.Product:
                    if (SelectedProductId != null)
                    {
                        Product product = Product.Load(SelectedProductId.Value);

                        if (product == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("ProductNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(null, args);

                            return;
                        }

                        locObjectDescName.Text = GetLocalResourceObject("ProductName").ToString();
                        locObjectDescCode.Text = GetLocalResourceObject("ProductCode").ToString();
                        lblObjectDescName.Text = " <b>" + product.Name + "</b>";
                        lblObjectDescCode.Text = " <b>" + product.Code + "</b>";
                    }
                    break;
                case ComponentObjectEnum.Catalog:
                    if (SelectedObjectId != null)
                    {
                        Catalog catalog = Catalog.Load(SelectedObjectId.Value);

                        if (catalog == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CatalogNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(null, args);

                            return;
                        }

                        locObjectDescName.Text = GetLocalResourceObject("CatalogName").ToString();
                        locObjectDescCode.Text = GetLocalResourceObject("CatalogCode").ToString();
                        lblObjectDescName.Text = " <b>" + catalog.Name + "</b>";
                        lblObjectDescCode.Text = " <b>" + catalog.Code + "</b>";
                    }
                    break;
                case ComponentObjectEnum.Bundle:
                    if (SelectedObjectId != null)
                    {
                        Bundle bundle = Bundle.Load(SelectedObjectId.Value);

                        if (bundle == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("BundleNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(null, args);

                            return;
                        }

                        locObjectDescName.Text = GetLocalResourceObject("BundleName").ToString();
                        locObjectDescCode.Text = GetLocalResourceObject("BundleCode").ToString(); ;
                        lblObjectDescName.Text = " <b>" + bundle.Name + "</b>";
                        lblObjectDescCode.Text = " <b>" + bundle.Code + "</b>";
                    }
                    break;
                case ComponentObjectEnum.Collection:
                    if (SelectedObjectId != null)
                    {
                        Collection collection = Collection.Load(SelectedObjectId.Value);

                        if (collection == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CollectionNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(null, args);

                            return;
                        }

                        locObjectDescName.Text = GetLocalResourceObject("CollectionName").ToString();
                        locObjectDescCode.Text = GetLocalResourceObject("CollectionCode").ToString();
                        lblObjectDescName.Text = " <b>" + collection.Name + "</b>";
                        lblObjectDescCode.Text = " <b>" + collection.Code + "</b>";
                    }
                    break;
                case ComponentObjectEnum.Discount:
                    if (SelectedObjectId != null)
                    {
                        Discount discount = Discount.Load(SelectedObjectId.Value);

                        if (discount == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("DiscountNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(null, args);

                            return;
                        }

                        locObjectDescName.Text = GetLocalResourceObject("DiscountName").ToString();
                        locObjectDescCode.Text = "";
                        lblObjectDescName.Text = " <b>" + discount.Name + "</b>";
                        lblObjectDescCode.Text = "";
                    }
                    break;
            }
        }
    }
}
