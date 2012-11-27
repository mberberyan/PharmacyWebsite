using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using Melon.Components.ProductCatalog.Exception;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing all products in the system.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     It contains user controls to manage selected product.
    ///     Following child user controls are used to display and manage product details:
    ///     <list type="bullet">
    ///     <item>GeneralInformation.ascx - displays product details information.</item>
    ///     <item>ProductSet.ascx - displays related product list and filter functionality to display 
    ///     all selected related products for the current product object.</item>
    ///     <item>Images.ascx - displays all selected image files for current product object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images listing user control</see></item>
    ///     <item>Audio.ascx - displays all selected audio files for current product object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio">Audio listing user control</see></item>
    ///     <item>Video.ascx - displays all selected video files for current product object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video">Video listing user control</see></item>
    ///     <item>DynamicPropDefinition.ascx - This control allows adding dynamic property definitions for the current product object and provides list control to select dynamic properties inherited by selected categories for the product. For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_DynamicPropDefinition">Dynamic properties listing user control</see></item>    
    ///     <item>Statistics.ascx - displays statistics information about product object. For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_ProductStatistics">Product Statistics user control</see></item>
    ///     </list>
    /// </para>
    /// </remarks>
    public partial class CodeBehind_Product : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Identifier of selected product
        /// </summary>
        public int? SelectedProductId
        {
            get { return (int?)ViewState["__mc_productcatalog_SelectedProductId"]; }
            set { ViewState["__mc_productcatalog_SelectedProductId"] = value; }
        }

        /// <summary>
        /// Table contains all added dynamic properties information for current product.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Table consists of the following columns:
        /// <list type="bullet">
        /// <item>propId - identifier of dynamic property</item>
        /// <item>propName - Name of dynamic property</item>
        /// </list>
        /// </para>        
        /// </remarks>
        private DataTable tabPropValueNames
        {
            get 
            {
                if (ViewState["tabPropValueNames"] == null)
                {
                    DataTable tempTable = new DataTable();
                    tempTable.Columns.Add("propId");
                    tempTable.Columns.Add("propName");
                    return tempTable;
                }

                return (DataTable)ViewState["tabPropValueNames"];
            }

            set { ViewState["tabPropValueNames"] = value; }
        }

        /// <summary>
        /// Table contains values for all added dynamic properties for current product.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Table consists of the following columns:
        /// <list type="bullet">
        /// <item>Id - identifier of dynamic property value in the database.</item>
        /// <item>Name - Already added value for dynamic property object.</item>
        /// </list>
        /// </para>        
        /// </remarks>
        private DataTable dtPropValues
        {
            get
            {
                if (ViewState["dtPropValues"] == null)
                {
                    return new DataTable();
                }

                return (DataTable)ViewState["dtPropValues"];
            }

            set { ViewState["dtPropValues"] = value; }
        }

        /// <summary>
        /// Flag whether dynamic property value is added for the first time
        /// </summary>
        private bool IsInsert
        {
            get 
            {
                if(ViewState["IsInsert"]==null)
                {
                    return false;
                }

                return (bool)ViewState["IsInsert"];
            }

            set { ViewState["IsInsert"] = value; }
        }
        #endregion

        /// <summary>
        /// Set user control property values passed from control`s caller page
        /// </summary>
        /// <param name="args"></param>
        public override void Initializer(object[] args)
        {
            this.SelectedProductId = (int?)args[1];
            ProductCatalogControl.SelectedObjectId = (int?)args[0];            
            ProductCatalogControl.SelectedProductId = (int?)args[1];
            SelectedObjectType = (ComponentObjectEnum)args[2];
            SelectedTab = (ProductCatalogTabs)args[3];
            ucProductSet.interfaceSearchCriteria = (ProductSearchCriteria)args[4];
            ucProductSet.isFirstLoad = (bool)args[5];
            ucGeneralInformation.Message = (string)args[6];
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

            this.ParentControl.SaveAudioEvent += new SaveAudioEventHandler(SaveAudioEvent);
            this.ParentControl.DeleteAudioEvent += new DeleteAudioEventHandler(DeleteAudioEvent);

            this.ParentControl.SaveVideoEvent += new SaveVideoEventHandler(SaveVideoEvent);
            this.ParentControl.DeleteVideoEvent += new DeleteVideoEventHandler(DeleteVideoEvent);

            lvPropValues.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvPropValues_ItemDataBound);
            lvPropValues.DataBound += new EventHandler(lvPropValues_DataBound);
            lvPropValues.ItemEditing += new EventHandler<ListViewEditEventArgs>(lvPropValues_ItemEditing);
            lvPropValues.ItemUpdating += new EventHandler<ListViewUpdateEventArgs>(lvPropValues_ItemUpdating);
            lvPropValues.ItemCanceling += new EventHandler<ListViewCancelEventArgs>(lvPropValues_ItemCanceling);            
            lvPropValues.ItemCommand += new EventHandler<ListViewCommandEventArgs>(lvPropValues_ItemCommand);
            lvPropValues.ItemDeleting += new EventHandler<ListViewDeleteEventArgs>(lvPropValues_ItemDeleting);

            btnAddPropValue.Click += new EventHandler(btnAddPropValue_Click);
            base.OnInit(e);
        }

        #region Custom Events
        /// <summary>
        /// Saves images added to the current product object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images.btnSaveImage_Click"> method to save image is fired.
        /// </remarks>        
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void SaveImageEvent(object sender, SaveImageEventArgs e)
        {
            Product product = Melon.Components.ProductCatalog.Product.Load(SelectedProductId.Value);
            if (product == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("ProductNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            ProductImage productImage = new ProductImage();
            if (!isNew)
            {
                productImage.Id = e.Id;
                productImage.Load();
            }

            productImage.AltText = e.ImageAlt;
            productImage.ProductId = SelectedProductId;
            productImage.IsMain = e.IsMainImage;

            if (isNew)
            {
                productImage.ImageFile = e.ImageFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                productImage.ImagePath = randomCode.ToString();
            }

            try
            {
                ProductImage.Save(productImage);
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
        /// Deletes images from current product object.        
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
                ProductImage.Delete(e.Id, e.ImageName);
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }
        }

        /// <summary>
        /// Saves audio files added to the current product object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio">Audio user control</see>
        /// when save <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio.btnSaveAudio_Click"> method to save audio file is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAudioEvent(object sender, SaveAudioEventArgs e)
        {
            Product product = Melon.Components.ProductCatalog.Product.Load(SelectedProductId.Value);
            if (product == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("ProductNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            ProductAudio productAudio = new ProductAudio();
            if (!isNew)
            {
                productAudio.Id = e.Id;
                productAudio.Load();
            }

            productAudio.Title = e.Title;
            productAudio.ProductId = SelectedProductId;
            productAudio.IsMain = e.IsMainAudio;

            if (isNew)
            {
                productAudio.AudioFile = e.AudioFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                productAudio.AudioPath = randomCode.ToString();
            }

            try
            {
                ProductAudio.Save(productAudio);
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                switch (ex.Code)
                {
                    case ProductCatalogExceptionCode.UnauthorizedAccessException:
                        args.ErrorMessage = String.Format(ex.Message, ResolveUrl(ex.AdditionalInfo));
                        break;
                    case ProductCatalogExceptionCode.AudioUploadFailure:
                        args.ErrorMessage = ex.Message;
                        break;
                    case ProductCatalogExceptionCode.AudioNotAllowedSize:
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
        /// Deletes audio file from current product object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio">Audio user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio.repAudioList_ItemCommand"> method for audio file deletion is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAudioEvent(object sender, DeleteAudioEventArgs e)
        {
            try
            {
                ProductAudio.Delete(e.Id, e.AudioName);
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }
        }

        /// <summary>
        /// Saves video file added to the current product object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video">Video user control</see>
        /// when save <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video.btnSaveVideo_Click"> method to save video file is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveVideoEvent(object sender, SaveVideoEventArgs e)
        {
            Product product = Melon.Components.ProductCatalog.Product.Load(SelectedProductId.Value);
            if (product == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("ProductNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            ProductVideo productVideo = new ProductVideo();
            if (!isNew)
            {
                productVideo.Id = e.Id;
                productVideo.Load();
            }

            productVideo.Title = e.Title;
            productVideo.ProductId = SelectedProductId;
            productVideo.IsMain = e.IsMainVideo;

            if (isNew)
            {
                productVideo.VideoFile = e.VideoFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                productVideo.VideoPath = randomCode.ToString();
            }

            try
            {
                ProductVideo.Save(productVideo);
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                switch (ex.Code)
                {
                    case ProductCatalogExceptionCode.UnauthorizedAccessException:
                        args.ErrorMessage = String.Format(ex.Message, ResolveUrl(ex.AdditionalInfo));
                        break;
                    case ProductCatalogExceptionCode.VideoUploadFailure:
                        args.ErrorMessage = ex.Message;
                        break;
                    case ProductCatalogExceptionCode.VideoNotAllowedSize:
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
        /// Deletes video file from current product object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video">Video user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video.repVideoList_ItemCommand"> method for video file deletion is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteVideoEvent(object sender, DeleteVideoEventArgs e)
        {
            try
            {
                ProductVideo.Delete(e.Id, e.VideoName);
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
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {                
                SelectedTab = SelectedTab != ProductCatalogTabs.Unknown ? SelectedTab : ProductCatalogTabs.GeneralInformation;

                LoadProduct();

                if (SelectedProductId != null)
                {                    
                    LoadPropertyValues();
                }
                else if(SelectedObjectId!=null && SelectedObjectId!=0)
                {
                    SetMeasurementUnit();
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

            // should be after LoadPropertyValues() 
            // as temporary table with prop definitions is loaded there
            LoadPropertyDefinitions();
            divNoPropValues.Visible = false;
        }

        /// <summary>
        /// Loads product details information.
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadProduct()
        {
            Product product = new Product();

            if (SelectedProductId != null)
            {
                product = Product.Load(SelectedProductId.Value);
                ucGeneralInformation.Code = product.Code;
                ucGeneralInformation.Name = product.Name;
                ucGeneralInformation.ShortDesc = product.ShortDescription;
                ucGeneralInformation.LongDesc = product.LongDescription;
                ucGeneralInformation.Tags = product.Tags;
                ucGeneralInformation.IsActive = product.IsActive;
                ucGeneralInformation.IsInStock = product.IsInStock;
                ucGeneralInformation.IsFeatured = product.IsFeatured;
                ucGeneralInformation.CommonPrice = product.CommonPrice;
                ucGeneralInformation.Unit = product.MeasurementUnitId;
                ucGeneralInformation.Manufacturer = product.Manufacturer;
                ucGeneralInformation.AppliedDiscounts = product.AppliedDiscountsList;  
            }
            
            List<int?> catIdList = new List<int?>();
            List<string> catNameList = new List<string>();

            if (product.ParentCatStrTable.Rows.Count > 0)
            {
                foreach (DataRow row in product.ParentCatStrTable.Rows)
                {
                    catIdList.Add(Convert.ToInt32(row["Id"]));
                    catNameList.Add(row["CategoryFullName"].ToString());
                }
            }
            else if(SelectedObjectId!=null)
            {
                catIdList.Add(SelectedObjectId);
                catNameList.Add(Category.GetHierarchicalList((bool?)null).Select("Id="+SelectedObjectId)[0]["CategoryFullName"].ToString());
            }
            
            ucGeneralInformation.CategoryIdList = catIdList;            
            ucGeneralInformation.CategoryNameList = catNameList;            
        }

        /// <summary>
        /// Load and initizalize measurement unit listing control
        /// </summary>
        /// <param name="cat"></param>
        private void SetMeasurementUnit()
        { 
            Category cat=Category.Load(SelectedObjectId);
            ucGeneralInformation.Unit = cat.MeasurementUnitId;
        }

        /// <summary>
        /// Load property values table with all property variations for current product
        /// If no variations are found, then load property names from current product`s category
        /// <author>Melon Team</author>
        /// <date>10/13/2009</date>
        /// </summary>
        private void LoadPropertyValues()
        {
            ProductProperty propertyList = new ProductProperty();
            propertyList.ProductId = SelectedProductId;
            propertyList.CategoryId = SelectedObjectId;
            DataSet ds = propertyList.List();            
            
            if (ds.Tables[0].Rows.Count == 0)
            {
                divNoPropValues.Visible = true;
                return;
            }

            if (ds.Tables[0].Rows.Count > 0) // add property values for current product
            {
                int grpCount = 0;                
                
                // temporary "tempTable" table is defined for copying information and adding a new line
                // that`s because of "This row already belongs to another table." error
                DataTable tempTable = new DataTable();
                tempTable.Columns.Add("propId");
                tempTable.Columns.Add("propName");                
                foreach (DataRow row in ds.Tables[0].Rows)
                {                                                          
                    grpCount++;
                
                    DataRow tabRow = tempTable.NewRow();                        
                    tabRow[0] = row["Id"].ToString();
                    tabRow[1] = row["Name"].ToString();
                    tempTable.Rows.Add(tabRow);                    
                }

                tabPropValueNames = tempTable.Copy();
                dtPropValues = ds.Tables[1].Copy();

                // insert rows in table(respectively columns in ListView) to be visible when inserting new property values
                for (int idx = 0; idx < tabPropValueNames.Rows.Count; idx++)
                {
                    dtPropValues.Rows.Add(dtPropValues.NewRow());
                }
                
                lvPropValues.GroupItemCount = Convert.ToInt32(grpCount);

                if (dtPropValues.Rows.Count > tabPropValueNames.Rows.Count || IsInsert==true)
                {
                    lvPropValues.Visible = true;
                    lvPropValues.DataSource = dtPropValues;
                    lvPropValues.DataBind();
                }
                else
                {
                    lvPropValues.Visible = false;
                }
            }            
        }

        /// <summary>
        /// Loads product dynamic properties information.
        /// </summary>        
        private void LoadPropertyDefinitions()
        {
            // remove last('actions') row from 'Dynamic Properties' table only if there are property values
            DataTable catDynPropDefTable = tabPropValueNames.Copy();
            if (dtPropValues.Rows.Count > tabPropValueNames.Rows.Count)
            {
                catDynPropDefTable.Rows.RemoveAt(catDynPropDefTable.Rows.Count - 1);
            }

            ucDynamicPropDef.DynPropDefTable = catDynPropDefTable.Copy();
        }

        /// <summary>
        /// Renders dynamic property table for adding, editing or deleting dynamic property values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPropValues_ItemDataBound(object sender, ListViewItemEventArgs e)
        {            
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;

                // show controls when in Insert mode
                if (dataItem.DisplayIndex > dtPropValues.Rows.Count - tabPropValueNames.Rows.Count - 1)
                {
                    ((HtmlTableCell)e.Item.FindControl("cellView")).Style["display"] = "none";
                    ((HtmlTableCell)e.Item.FindControl("cellUpdate")).Style["display"] = IsInsert ? "" : "none";                    
                    ((CustomValidator)e.Item.FindControl("cellUpdate").FindControl("cvInsertItem")).ValidationGroup = IsInsert ? "InsertProperty" : "NoValidation";
                    ((HtmlTableCell)e.Item.FindControl("cellEdit")).Style["display"] = IsInsert ? (((dataItem.DisplayIndex + 1) % tabPropValueNames.Rows.Count) == 0 ? "" : "none") : "none";
                    ((Button)((HtmlTableCell)e.Item.FindControl("cellEdit")).FindControl("btnInsert")).Visible = true;
                    ((Button)((HtmlTableCell)e.Item.FindControl("cellEdit")).FindControl("btnCancel")).Visible = true;
                    ((Button)((HtmlTableCell)e.Item.FindControl("cellEdit")).FindControl("btnEdit")).Visible = false;
                    ((Button)((HtmlTableCell)e.Item.FindControl("cellEdit")).FindControl("btnDelete")).Visible = false;
                    return;
                }

                //show Labels with data if not in Edit mode
                if (lvPropValues.EditIndex == -1)
                {                    
                    ((Label)e.Item.FindControl("lblPropvalue")).Text = drv["PropertyValue"].ToString();
                    ((HtmlTableCell)e.Item.FindControl("cellView")).Style["display"] = "";
                    ((HtmlTableCell)e.Item.FindControl("cellUpdate")).Style["display"] = "none";                    
                    ((CustomValidator)e.Item.FindControl("cellUpdate").FindControl("cvInsertItem")).ValidationGroup = "NoValidation";
                }

                // show Edit button on the last cell of each row
                if (dataItem.DisplayIndex != lvPropValues.EditIndex)
                {                    
                    ((HtmlTableCell)e.Item.FindControl("cellEdit")).Style["display"] = ((dataItem.DisplayIndex + 1) % tabPropValueNames.Rows.Count) == 0 ? "" : "none";                     
                }
                                
            
                if (lvPropValues.EditIndex > -1 &&
                    dataItem.DisplayIndex != lvPropValues.EditIndex &&
                    (dataItem.DisplayIndex) < lvPropValues.EditIndex &&
                    (dataItem.DisplayIndex+1) > (lvPropValues.EditIndex - tabPropValueNames.Rows.Count + 1)
                   )
                {
                    // show controls for add/edit mode
                    ((HtmlTableCell)e.Item.FindControl("cellView")).Style["display"] = "none";
                    ((HtmlTableCell)e.Item.FindControl("cellUpdate")).Style["display"] = "";
                }
                else if (dataItem.DisplayIndex != lvPropValues.EditIndex)
                {
                    // show controls in view mode
                    ((HtmlTableCell)e.Item.FindControl("cellView")).Style["display"] = "";
                    ((HtmlTableCell)e.Item.FindControl("cellUpdate")).Style["display"] = "none";                    
                    ((CustomValidator)e.Item.FindControl("cellUpdate").FindControl("cvInsertItem")).ValidationGroup = "NoValidation";
                }
            
            }            
        }

        /// <summary>
        /// Initializes dynamic property value table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPropValues_DataBound(object sender, EventArgs e)
        {
            HtmlTable tabHeader=(HtmlTable)lvPropValues.FindControl("tabPropValues");

            // add rightmost empty row for actions column            
            tabPropValueNames.Rows.Add(tabPropValueNames.NewRow());
            foreach (DataRow row in tabPropValueNames.Rows)
            {
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerHtml = row["propName"].ToString();
                tabHeader.Rows[0].Cells.Add(cell);            
            }            
        }

        /// <summary>
        /// Opens dynamic property value table for editing property values for selected table row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPropValues_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvPropValues.EditIndex = e.NewEditIndex;
            IsInsert = false;
            LoadPropertyValues();                            
        }

        /// <summary>
        /// Cancels dynamic property value editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPropValues_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvPropValues.EditIndex = -1;
            lvPropValues.InsertItemPosition = InsertItemPosition.None;
            LoadPropertyValues();
        }

        /// <summary>
        /// Update dynamic property values for the selected product.
        /// </summary>
        /// <remarks>
        /// This method updates dynamic property values by passing xml-formated string to the dynamic property value save method.
        /// For more information see <see cref="Melon.Components.ProductCatalog.ProductProperty.SaveByXml"/>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPropValues_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            string xmlStr = "<root>";
            foreach (ListViewDataItem item in lvPropValues.Items)
            {
                if (lvPropValues.EditIndex > -1 &&                    
                    (item.DisplayIndex) <= lvPropValues.EditIndex &&
                    (item.DisplayIndex) >= (lvPropValues.EditIndex - (tabPropValueNames.Rows.Count-1-1))
                   )
                {                    
                    // get Product Property identifier from temp table 
                    string productPropertyId = dtPropValues.Rows[item.DisplayIndex]["Id"].ToString();

                    // get property Name from textbox control in edit mode
                    string propName=((TextBox)item.FindControl("txtEdit")).Text;

                    // get product property id from hidden field in edit mode
                    string PropValueId = ((HiddenField)item.FindControl("hfPropertyValueId")).Value;

                    // add xml items to be saved in database
                    xmlStr += "<prop ProductPropId='" + productPropertyId + "' BundlePropId='' PropValueId='" + PropValueId + "' Name='" + propName + "'/>";
                }
            }
            xmlStr += "</root>";

            try
            {
                ProductProperty.SaveByXml(SelectedProductId.Value, true, xmlStr);
            }
            catch (ProductCatalogException args)
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = args.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                return;
            }

            lvPropValues.EditIndex = -1;
            LoadPropertyValues();
        }

        /// <summary>
        /// Saves or cancels saving dynamic property values for the selected product.
        /// </summary>
        /// <remarks>
        /// This method saves or cancel saving dynamic property values.
        /// CommandEventArgs argument parameter denotes whether to save or cancel saving added dynamic property values.
        /// The parameter contains one of the following values:
        /// <list type="bullet">
        /// <item>InsertAction - saves newly-added dynamic property values</item>
        /// <item>InsertCancel - cancels saving new dynamic property values</item>
        /// </list>        
        /// For more information see <see cref="Melon.Components.ProductCatalog.ProductProperty.SaveByXml"/>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lvPropValues_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "InsertCancel")
            {
                IsInsert = false;
                LoadPropertyValues();
            }
            else if (e.CommandName == "InsertAction")
            {
                string xmlStr = "<root>";

                foreach (ListViewDataItem item in lvPropValues.Items)
                {
                    if (item.DisplayIndex > dtPropValues.Rows.Count - (tabPropValueNames.Rows.Count - 1) - 1)
                    {                        
                        // find the column Index
                        // in expression below: if last row -> get temporary table`s last meaning row( -1 -> for empty cell; -1 -> for zero index)
                        int colNum = (item.DisplayIndex + 1) % (tabPropValueNames.Rows.Count - 1) != 0 ? ((item.DisplayIndex + 1) % (tabPropValueNames.Rows.Count - 1) - 1) : (tabPropValueNames.Rows.Count - 1 - 1);

                        // get Product Property identifier from temp table dtPropValues
                        string productPropertyId = tabPropValueNames.Rows[colNum]["PropId"].ToString();

                        // get property Name from textbox control in edit mode
                        string propValueName = ((TextBox)item.FindControl("txtEdit")).Text;
                        
                        // add xml items to be saved in database
                        xmlStr += "<prop ProductPropId='" + productPropertyId + "' BundlePropId='' PropValueId='' Name='" + propValueName + "'/>";                        
                    }
                }

                xmlStr += "</root>";

                try
                {
                    ProductProperty.SaveByXml(SelectedProductId.Value, false, xmlStr);
                }
                catch (ProductCatalogException args)
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = args.Message;
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    return;
                }

                IsInsert = false;
                LoadPropertyValues();
            }            
        }

        /// <summary>
        /// Deletes dynamic property values for the selected product.
        /// </summary>
        /// <remarks>
        /// This method deletes dynamic property values by passing xml-formated string of dynamic property value object identifier.
        /// For more information see <see cref="Melon.Components.ProductCatalog.ProductProperty.DeleteByXml"/>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPropValues_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string xmlStr = "<root>";
            foreach (ListViewDataItem item in lvPropValues.Items)
            {
                if (item.DisplayIndex <= e.ItemIndex &&
                    item.DisplayIndex >= (e.ItemIndex - (tabPropValueNames.Rows.Count - 1 - 1))
                   )
                {                    
                    // get product property id from hidden field in edit mode
                    string propValueId = ((HiddenField)item.FindControl("hfPropertyValueId")).Value;

                    // add xml items to be saved in database
                    xmlStr += "<prop PropValueId='" + propValueId + "' />";
                }
            }
            xmlStr += "</root>";

            try
            {
                ProductProperty.DeleteByXml(xmlStr);
            }
            catch (ProductCatalogException args)
            {
                DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                errorArgs.ErrorMessage = args.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                return;
            }
            
            LoadPropertyValues();
        }

        /// <summary>
        /// Save product information in database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/21/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedProductId != null)
            {
                Product product = Melon.Components.ProductCatalog.Product.Load(SelectedProductId.Value);
                if (product == null)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = Convert.ToString(GetLocalResourceObject("ProductNotExistErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                SaveProduct(sender, product);
            }
            else
            {
                SaveProduct(sender, new Product());
            }
        }

        /// <summary>
        /// Saves product details information contained in <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_GeneralInformation">GeneralInfornation child user control</see>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bundle"></param>
        private void SaveProduct(object sender, Product product)
        {
            product.Code = ucGeneralInformation.Code;
            product.Name = ucGeneralInformation.Name;                        
            product.ShortDescription = ucGeneralInformation.ShortDesc;
            product.LongDescription = ucGeneralInformation.LongDesc;
            product.Tags = ucGeneralInformation.Tags;
            product.IsActive = ucGeneralInformation.IsActive;
            product.IsInStock = ucGeneralInformation.IsInStock;
            product.IsFeatured = ucGeneralInformation.IsFeatured;
            product.CommonPrice = ucGeneralInformation.CommonPrice;
            product.MeasurementUnitId = ucGeneralInformation.Unit;
            product.Manufacturer = ucGeneralInformation.Manufacturer;           
            product.CreatedBy = ParentControl.CurrentUser.AdapterId;

            if (product.Id == null)
            {
                product.DateCreated = DateTime.Now;
            }

            if (product.Id != null)
            {
                product.DateModified = DateTime.Now;
                product.ModifiedBy = ParentControl.CurrentUser.AdapterId;
            }

            string catIdList="";
            foreach(int? item in ucGeneralInformation.CategoryIdList.ToArray())
            {
                catIdList += item.ToString() + ",";
            }
            
            if (catIdList != "")
            {
                product.CategoryIdList = catIdList.Substring(0, catIdList.Length - 1);
            }

            if (String.IsNullOrEmpty(product.CategoryIdList))
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("ProductCategoriesNotSelectedErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            try
            {
                product.Save();
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);
                return;
            }

            SelectedObjectId = Convert.ToInt32(product.CategoryIdList.Split(',')[0]);

            LoadProductEventArgs e = new LoadProductEventArgs();
            e.SelectedCategoryId = SelectedObjectId;
            e.SelectedProductId = product.Id;
            e.SelectedObjectType = ComponentObjectEnum.Product;
            e.SelectedTab = ProductCatalogTabs.GeneralInformation;
            e.RefreshExplorer = true;
            e.Message = GetLocalResourceObject("ProductSaveSuccess").ToString();
            this.ParentControl.OnLoadProduct(sender, e);
        }

        /// <summary>
        /// Open dynamic property values table in Insert mode to enter new values for dynamic property set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddPropValue_Click(object sender, EventArgs e)
        {
            IsInsert = true;
            lvPropValues.EditIndex = -1;
            LoadPropertyValues();
        }                                                

    }
}
