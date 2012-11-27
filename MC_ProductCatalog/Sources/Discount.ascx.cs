using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.Enumerations;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing all discount in the system.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     It contains user controls to manage selected discounts from the <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.Explorer">Discount Explorer control</see>.
    ///     Following child user controls are used to display and manage discount details:
    ///     <list type="bullet">
    ///     <item>GeneralInformation.ascx - displays discount details information.</item>
    ///     <item>ProductSet.ascx - displays product list and filter functionality to display 
    ///     all selected products for the current discount object.</item>    
    ///     </list>
    /// </para>
    /// </remarks>
    public partial class CodeBehind_Discount : ProductCatalogControl
    {
        /// <summary>
        /// Set user control property values passed from control`s caller page
        /// </summary>
        /// <param name="args"></param>
        public override void Initializer(object[] args)
        {
            ProductCatalogControl.SelectedObjectId = (int?)args[0];
            ProductCatalogControl.SelectedObjectType = (ComponentObjectEnum)args[1];
            ProductCatalogControl.SelectedTab = (ProductCatalogTabs)args[2];
            ucProductSet.interfaceSearchCriteria = (ProductSearchCriteria)args[3];
            ucProductSet.isFirstLoad = (bool)args[4];
            ucDiscountInformation.Message = (string)args[5];
        }

        /// <summary>
        /// Attach event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>                
        protected override void OnInit(EventArgs e)
        {
            btnSave.Click += new EventHandler(btnSave_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes user control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                if (SelectedObjectId == (int?)null)
                {
                    divExplorerLayout.Visible = true;
                    divDiscountDetails.Visible = false;
                }
                else
                {
                    divExplorerLayout.Visible = false;
                    divDiscountDetails.Visible = true;
                    SelectedTab = SelectedTab != ProductCatalogTabs.Unknown ? SelectedTab : ProductCatalogTabs.GeneralInformation;

                    if (SelectedObjectId != null && SelectedObjectId != 0)
                    {
                        Discount coll = Melon.Components.ProductCatalog.Discount.Load(SelectedObjectId.Value);
                        if (coll == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("DiscountNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }

                        LoadDiscount(coll);
                    }
                }
            }
            else
            {
                switch (hfSelectedTab.Value)
                {
                    case "divGeneralInformation":
                        SelectedTab = ProductCatalogTabs.GeneralInformation;
                        break;
                    case "divProducts":
                        SelectedTab = ProductCatalogTabs.Products;
                        break;                    
                }
            }
        }

        /// <summary>
        /// Loads discount details information.
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadDiscount(Discount discount)
        {
            if (discount != null)
            {
                ucDiscountInformation.Name = discount.Name;
                ucDiscountInformation.Description = discount.Description;
                ucDiscountInformation.DiscountType = (DiscountTypeEnum)discount.DiscountTypeId;
                ucDiscountInformation.DiscountFrom = discount.StartDate;
                ucDiscountInformation.DiscountTo = discount.EndDate;
                ucDiscountInformation.DiscountValue = discount.DiscountValue;
                ucDiscountInformation.IsActive = discount.IsActive;
            }
        }

        /// <summary>
        /// Saves discount details information contained in <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_GeneralInformation">GeneralInfornation child user control</see>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bundle"></param>
        private void SaveDiscount(object sender, Discount discount)
        {
            discount.Id = discount.Id != null ? discount.Id : (int?)null;            
            discount.Name = ucDiscountInformation.Name;
            discount.Description = ucDiscountInformation.Description;
            discount.IsActive = ucDiscountInformation.IsActive;
            discount.DiscountTypeId = (int)ucDiscountInformation.DiscountType;
            discount.StartDate = Convert.ToDateTime(ucDiscountInformation.DiscountFrom);
            discount.EndDate = Convert.ToDateTime(ucDiscountInformation.DiscountTo);
            discount.DiscountValue = ucDiscountInformation.DiscountValue;
            discount.DateCreated = discount.Id != null ? discount.DateCreated : DateTime.Now;
            discount.DateModified = DateTime.Now;
            discount.CreatedBy = discount.Id != null ? discount.CreatedBy : this.ParentControl.CurrentUser.Adapter.Id;
            discount.ModifiedBy = this.ParentControl.CurrentUser.Adapter.Id;

            try
            {
                discount.Save();
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            LoadDiscountEventArgs e = new LoadDiscountEventArgs();
            e.SelectedDiscountId = discount.Id;
            e.RefreshExplorer = true;
            e.SelectedObjectType = ComponentObjectEnum.Discount;
            e.SelectedTab = SelectedTab;
            e.Message = GetLocalResourceObject("DiscountSaveSuccess").ToString();
            this.ParentControl.OnLoadDiscount(sender, e);
        }

        /// <summary>
        /// Save discount information in database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>12/03/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedObjectId != null && SelectedObjectId != 0)
            {
                Discount discount = Melon.Components.ProductCatalog.Discount.Load(SelectedObjectId.Value);
                if (discount == null)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = Convert.ToString(GetLocalResourceObject("DiscountNotExistErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                SaveDiscount(sender, discount);
            }
            else
            {
                SaveDiscount(sender, new Discount());
            }
        }

    }
}
