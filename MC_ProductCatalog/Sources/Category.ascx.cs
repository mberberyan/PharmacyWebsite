using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using Melon.Components.ProductCatalog.Exception;
using System.Web.UI.WebControls;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing all categories in the system.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     It contains user controls to manage selected category from the <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CategoryExplorer">Category Explorer user control</see>.
    ///     Following child user controls are used to display and manage category details:
    ///     <list type="bullet">
    ///     <item>GeneralInformation.ascx - displays category details information.</item>
    ///     <item>ProductList.ascx - displays product list and filter functionality to display 
    ///     all selected products for the current category object.</item>
    ///     <item>Images.ascx - displays all selected image files for current category object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images listing user control</see></item>
    ///     <item>DynamicPropDefinition.ascx - This control allows adding dynamic property definitions for the current category object and provides list control to select dynamic properties inherited by selected categories for the category.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_DynamicPropDefinition">Dynamic properties listing user control</see></item>
    ///     </list>
    /// </para>
    /// </remarks>
    public partial class CodeBehind_Category : ProductCatalogControl
    {        
        #region Fields && Properties
        /// <summary>
        /// Id of parent category
        /// </summary>
        public int? ParentCategoryId
        {
            get { return (int?)ViewState["__mc_productcatalog_ParentCategoryId"]; }
            set { ViewState["__mc_productcatalog_ParentCategoryId"] = value; }
        }
               
        #endregion

        /// <summary>
        /// Set user control property values passed from control`s caller page
        /// </summary>
        /// <param name="args"></param>
        public override void Initializer(object[] args)
        {
            this.ParentCategoryId = (int?)args[0];
            SelectedObjectId = (int?)args[1];
            SelectedObjectType = (ComponentObjectEnum)args[2];
            SelectedTab = (ProductCatalogTabs)args[3];
            ucGeneralInformation.Message = (string)args[4];
        }

        /// <summary>
        /// Attach event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            
            this.ParentControl.SaveImageEvent += new SaveImageEventHandler(SaveImageEvent);
            this.ParentControl.DeleteImageEvent += new DeleteImageEventHandler(DeleteImageEvent);

            base.OnInit(e);
        }        

        #region Custom Events
        /// <summary>
        /// Saves images added to the current category object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images.btnSaveImage_Click"> method to save image is fired.
        /// </remarks>        
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveImageEvent(object sender, SaveImageEventArgs e)
        {
            Category cat = Melon.Components.ProductCatalog.Category.Load(SelectedObjectId.Value);
            if (cat == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CategoryNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            CategoryImage catImage = new CategoryImage();
            if (!isNew)
            {
                catImage.Id = e.Id;
                catImage.Load();
            }

            catImage.AltText = e.ImageAlt;
            catImage.CategoryId = SelectedObjectId;            
            catImage.IsMain = e.IsMainImage;

            if (isNew)
            {
                catImage.ImageFile = e.ImageFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                catImage.ImagePath = randomCode.ToString();
            }

            try
            {
                CategoryImage.Save(catImage);
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                switch (ex.Code)
                { 
                    case ProductCatalogExceptionCode.UnauthorizedAccessException:
                        args.ErrorMessage = String.Format(ex.Message, ResolveUrl(ex.AdditionalInfo));
                        break;
                    case ProductCatalogExceptionCode.ImageUploadFailure:
                        args.ErrorMessage = ex.Message;
                        break;
                    case ProductCatalogExceptionCode.ImageNotAllowedSize:
                        args.ErrorMessage = ex.Message;
                        break;
                    default:
                        args.ErrorMessage = ex.Message;
                        break;
                    
                }
                
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }
        }

        /// <summary>
        /// Deletes images from current category object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio.repImageList_ItemCommand"> method for image deletion is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteImageEvent(object sender, DeleteImageEventArgs e)
        {
            try
            {
                CategoryImage.Delete(e.Id, e.ImageName);                
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }
        }

        #endregion Custom Events

        /// <summary>
        /// Initializes user control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {                
                if (SelectedObjectType == ComponentObjectEnum.Unknown)
                {
                    divExplorerLayout.Visible = true;
                    divCategoryDetails.Visible = false;
                }
                else
                {
                    divExplorerLayout.Visible = false;
                    divCategoryDetails.Visible = true;

                    SelectedTab = SelectedTab!=ProductCatalogTabs.Unknown ? SelectedTab : ProductCatalogTabs.GeneralInformation;

                    Category cat = new Category();
                    if (SelectedObjectId != null)
                    {
                        cat = Melon.Components.ProductCatalog.Category.Load(SelectedObjectId);
                        if (cat == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CategoryNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }

                        LoadCategory(cat);
                        LoadDynPropDefinition(cat);
                    }

                    LoadMeasurementUnit(cat);
                }
            }
            else
            {
                switch (hfSelectedTab.Value)
                { 
                    case "divGeneralInformation":
                        SelectedTab = ProductCatalogTabs.GeneralInformation;
                        break;
                    case "divImages":
                        SelectedTab = ProductCatalogTabs.Images;
                        break;
                    case "divAudio":
                        SelectedTab = ProductCatalogTabs.Audio;
                        break;
                    case "divVideo":
                        SelectedTab = ProductCatalogTabs.Video;
                        break;
                    case "divRelatedProducts":
                        SelectedTab = ProductCatalogTabs.RelatedProducts;
                        break;
                    case "divStatistics":
                        SelectedTab = ProductCatalogTabs.Statistics;
                        break;
                    case "divDynamicProperties":
                        SelectedTab = ProductCatalogTabs.DynamicProperties;
                        break;
                    case "divProducts":
                        SelectedTab = ProductCatalogTabs.Products;
                        break;
                }
            }
        }

        /// <summary>
        /// Loads category dynamic properties information.
        /// </summary>
        private void LoadDynPropDefinition(Category cat)
        {
            ucDynamicPropDef.DynPropDefTable = cat.PropDefTable.Copy();
        }

        /// <summary>
        /// Loads category details information.
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadCategory(Category cat)
        {
            ucGeneralInformation.Code = cat.Code;
            ucGeneralInformation.Name = cat.Name;
            ucGeneralInformation.ShortDesc = cat.ShortDescription;
            ucGeneralInformation.LongDesc = cat.LongDescription;
            ucGeneralInformation.Tags = cat.Tags;            
            ucGeneralInformation.IsActive = cat.IsActive;            
        }

        /// <summary>
        /// Load and initizalize measurement unit listing control
        /// </summary>
        /// <param name="cat"></param>
        private void LoadMeasurementUnit(Category cat)
        {
            if (cat.MeasurementUnitId != null)
            {
                ucGeneralInformation.Unit = cat.MeasurementUnitId;
            }
            else if(ParentCategoryId!=null)
            {
                Category parentCat = Category.Load(ParentCategoryId);
                ucGeneralInformation.Unit = parentCat.MeasurementUnitId;
            }
        }

        /// <summary>
        /// Saves category details information contained in <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_GeneralInformation">GeneralInfornation child user control</see>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bundle"></param>
        private void SaveCategory(object sender, Category cat)
        {
            cat.Id = cat.Id != null ? cat.Id : (int?)null;
            cat.ParentCategoryId = cat.Id != null ? cat.ParentCategoryId : ParentCategoryId;
            cat.Code = ucGeneralInformation.Code;
            cat.Name = ucGeneralInformation.Name;
            cat.ShortDescription = ucGeneralInformation.ShortDesc;
            cat.LongDescription = ucGeneralInformation.LongDesc;
            cat.Tags = ucGeneralInformation.Tags;
            cat.MeasurementUnitId = ucGeneralInformation.Unit;
            cat.IsActive = ucGeneralInformation.IsActive;
            cat.DateCreated = cat.Id != null ? cat.DateCreated : DateTime.Now;
            cat.DateModified = DateTime.Now;
            cat.CreatedBy = cat.Id != null ? cat.CreatedBy : this.ParentControl.CurrentUser.Adapter.Id;
            cat.ModifiedBy = this.ParentControl.CurrentUser.Adapter.Id;

            try
            {
                cat.Save();
            }
            catch(ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }
            
            LoadCategoryEventArgs e = new LoadCategoryEventArgs();
            e.SelectedCategoryId = cat.Id;
            e.RefreshExplorer = true;
            e.SelectedObjectType = ComponentObjectEnum.Category;
            e.SelectedTab = SelectedTab;
            e.Message = GetLocalResourceObject("CategorySaveSuccess").ToString();
            this.ParentControl.OnLoadCategory(sender, e);                        
        }
        /// <summary>
        /// Save category information in database
        /// </summary>
        /// <author>Melon Team</author>        
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedObjectId != null)
            {
                Category cat = Melon.Components.ProductCatalog.Category.Load(SelectedObjectId.Value);
                if (cat == null)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CategoryNotExistErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                SaveCategory(sender, cat);
            }
            else
            {
                SaveCategory(sender, new Category());
            }
        }

        
    }
}
