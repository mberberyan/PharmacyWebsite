using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Exception;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing all catalogs in the system.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     It contains user controls to manage selected catalog from the <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.Explorer">Catalog Explorer control</see>.
    ///     Following child user controls are used to display and manage catalog details:
    ///     <list type="bullet">
    ///     <item>GeneralInformation.ascx - displays catalog details information.</item>
    ///     <item>ProductSet.ascx - displays product list and filter functionality to display 
    ///     all selected products for the current catalog object.</item>    
    ///     </list>
    /// </para>
    /// </remarks>
    public partial class CodeBehind_Catalog : ProductCatalogControl
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
            ucGeneralInformation.Message = (string)args[5];
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
                    divCatalogDetails.Visible = false;
                }
                else
                {
                    divExplorerLayout.Visible = false;
                    divCatalogDetails.Visible = true;
                    SelectedTab = SelectedTab != ProductCatalogTabs.Unknown ? SelectedTab : ProductCatalogTabs.GeneralInformation;

                    if (SelectedObjectId != null && SelectedObjectId != 0)
                    {
                        Catalog cat = Melon.Components.ProductCatalog.Catalog.Load(SelectedObjectId);
                        if (cat == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CatalogNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }

                        LoadCatalog(cat);                        
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
        /// Loads catalog details information.
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadCatalog(Catalog cat)
        {
            if (cat != null)
            {
                ucGeneralInformation.Code = cat.Code;
                ucGeneralInformation.Name = cat.Name;
                ucGeneralInformation.ShortDesc = cat.ShortDescription;
                ucGeneralInformation.LongDesc = cat.LongDescription;
                ucGeneralInformation.Tags = cat.Tags;
                ucGeneralInformation.IsActive = cat.IsActive;
            }
        }

        /// <summary>
        /// Saves catalog details information contained in <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_GeneralInformation">GeneralInfornation child user control</see>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bundle"></param>
        private void SaveCatalog(object sender, Catalog cat)
        {
            cat.Id = cat.Id != null ? cat.Id : (int?)null;            
            cat.Code = ucGeneralInformation.Code;
            cat.Name = ucGeneralInformation.Name;
            cat.ShortDescription = ucGeneralInformation.ShortDesc;
            cat.LongDescription = ucGeneralInformation.LongDesc;
            cat.Tags = ucGeneralInformation.Tags;            
            cat.IsActive = ucGeneralInformation.IsActive;
            cat.DateCreated = cat.Id != null ? cat.DateCreated : DateTime.Now;
            cat.DateModified = DateTime.Now;
            cat.CreatedBy = cat.Id != null ? cat.CreatedBy : this.ParentControl.CurrentUser.Adapter.Id;
            cat.ModifiedBy = this.ParentControl.CurrentUser.Adapter.Id;

            try
            {
                cat.Save();
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            LoadCatalogEventArgs e = new LoadCatalogEventArgs();
            e.SelectedCatalogId = cat.Id;
            e.RefreshExplorer = true;
            e.SelectedObjectType = ComponentObjectEnum.Catalog;
            e.SelectedTab = SelectedTab;
            e.Message = GetLocalResourceObject("CatalogSaveSuccess").ToString();
            this.ParentControl.OnLoadCatalog(sender, e);
        }

        /// <summary>
        /// Save catalog information in database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/04/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedObjectId != null && SelectedObjectId != 0)
            {
                Catalog cat = Melon.Components.ProductCatalog.Catalog.Load(SelectedObjectId.Value);
                if (cat == null)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CatalogNotExistErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                SaveCatalog(sender, cat);
            }
            else
            {
                SaveCatalog(sender, new Catalog());
            }
        }
    }
}
