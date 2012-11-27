using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Exception;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing all bundles in the system.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     It contains user controls to manage selected bundle from the <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.Explorer">Bundle Explorer control</see>.
    ///     Following child user controls are used to display and manage bundle details:
    ///     <list type="bullet">
    ///     <item>GeneralInformation.ascx - displays bundle details information.</item>
    ///     <item>ProductSet.ascx - displays product list and filter functionality to display 
    ///     all selected products for the current bundle object.</item>
    ///     <item>Images.ascx - displays all selected image files for current bundle object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images listing user control</see></item>
    ///     <item>Audio.ascx - displays all selected audio files for current bundle object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio">Audio listing user control</see></item>
    ///     <item>Video.ascx - displays all selected video files for current bundle object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video">Video listing user control</see></item>
    ///     <item>DynamicPropDefinition.ascx - This control allows adding dynamic property definitions for the current bundle object and provides list control to select dynamic properties inherited by selected categories for the bundle.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_DynamicPropDefinition">Dynamic properties listing user control</see></item>    
    ///     </list>
    /// </para>
    /// </remarks>
    public partial class CodeBehind_Bundle : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Table contains all added dynamic properties information for current bundle.
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
        /// Table contains values for all added dynamic properties for current bundle.
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
                if (ViewState["IsInsert"] == null)
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
        /// Saves images added to the current bundle object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images.btnSaveImage_Click"> method to save image is fired.
        /// </remarks>        
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void SaveImageEvent(object sender, SaveImageEventArgs e)
        {
            Bundle bundle = Melon.Components.ProductCatalog.Bundle.Load(SelectedObjectId.Value);
            if (bundle == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("BundleNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            BundleImage bundleImage = new BundleImage();
            if (!isNew)
            {
                bundleImage.Id = e.Id;
                bundleImage.Load();
            }

            bundleImage.AltText = e.ImageAlt;
            bundleImage.BundleId = SelectedObjectId;
            bundleImage.IsMain = e.IsMainImage;

            if (isNew)
            {
                bundleImage.ImageFile = e.ImageFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                bundleImage.ImagePath = randomCode.ToString();
            }

            try
            {
                BundleImage.Save(bundleImage);
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
        /// Deletes images from current bundle object.        
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
                BundleImage.Delete(e.Id, e.ImageName);
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
        /// Saves audio files added to the current bundle object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio">Audio user control</see>
        /// when save <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio.btnSaveAudio_Click"> method to save audio file is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAudioEvent(object sender, SaveAudioEventArgs e)
        {
            Bundle bundle = Melon.Components.ProductCatalog.Bundle.Load(SelectedObjectId.Value);
            if (bundle == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("BundleNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            BundleAudio bundleAudio = new BundleAudio();
            if (!isNew)
            {
                bundleAudio.Id = e.Id;
                bundleAudio.Load();
            }

            bundleAudio.Title = e.Title;
            bundleAudio.BundleId = SelectedObjectId;
            bundleAudio.IsMain = e.IsMainAudio;

            if (isNew)
            {
                bundleAudio.AudioFile = e.AudioFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                bundleAudio.AudioPath = randomCode.ToString();
            }

            try
            {
                BundleAudio.Save(bundleAudio);
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
        /// Deletes audio file from current bundle object.        
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
                BundleAudio.Delete(e.Id, e.AudioName);
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
        /// Saves video file added to the current bundle object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video">Video user control</see>
        /// when save <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video.btnSaveVideo_Click"> method to save video file is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveVideoEvent(object sender, SaveVideoEventArgs e)
        {
            Bundle bundle = Melon.Components.ProductCatalog.Bundle.Load(SelectedObjectId.Value);
            if (bundle == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("BundleNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            BundleVideo bundleVideo = new BundleVideo();
            if (!isNew)
            {
                bundleVideo.Id = e.Id;
                bundleVideo.Load();
            }

            bundleVideo.Title = e.Title;
            bundleVideo.BundleId = SelectedObjectId;
            bundleVideo.IsMain = e.IsMainVideo;

            if (isNew)
            {
                bundleVideo.VideoFile = e.VideoFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                bundleVideo.VideoPath = randomCode.ToString();
            }

            try
            {
                BundleVideo.Save(bundleVideo);
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
        /// Deletes video file from current bundle object.        
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
                BundleVideo.Delete(e.Id, e.VideoName);
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
                if (SelectedObjectId == (int?)null)
                {
                    divExplorerLayout.Visible = true;
                    divBundleDetails.Visible = false;
                }
                else
                {
                    divExplorerLayout.Visible = false;
                    divBundleDetails.Visible = true;
                    SelectedTab = SelectedTab != ProductCatalogTabs.Unknown ? SelectedTab : ProductCatalogTabs.GeneralInformation;

                    if (SelectedObjectId != null && SelectedObjectId != 0)
                    {
                        Bundle bundle = Melon.Components.ProductCatalog.Bundle.Load(SelectedObjectId);
                        if (bundle == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("BundleNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }

                        LoadBundle(bundle);
                        LoadPropertyValues();
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
                    case "divImages":
                        SelectedTab = ProductCatalogTabs.Images;
                        break;
                    case "divAudio":
                        SelectedTab = ProductCatalogTabs.Audio;
                        break;
                    case "divVideo":
                        SelectedTab = ProductCatalogTabs.Video;
                        break;                    
                    case "divDynamicProperties":
                        SelectedTab = ProductCatalogTabs.DynamicProperties;
                        break;
                }
            }

            // should be after LoadPropertyValues() 
            // as temporary table with prop definitions is loaded there
            LoadPropertyDefinitions();
            divNoPropValues.Visible = false;
        }        

        /// <summary>
        /// Loads bundle details information.
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadBundle(Bundle bundle)
        {
            if (bundle != null)
            {
                ucGeneralInformation.Code = bundle.Code;
                ucGeneralInformation.Name = bundle.Name;
                ucGeneralInformation.ShortDesc = bundle.ShortDescription;
                ucGeneralInformation.LongDesc = bundle.LongDescription;
                ucGeneralInformation.Tags = bundle.Tags;
                ucGeneralInformation.CommonPrice = bundle.Price;
                ucGeneralInformation.IsActive = bundle.IsActive;

                List<int?> catIdList = new List<int?>();
                List<string> catNameList = new List<string>();
                foreach (DataRow row in bundle.ParentCatStrTable.Rows)
                {
                    catIdList.Add(Convert.ToInt32(row["Id"]));
                    catNameList.Add(row["CategoryFullName"].ToString());
                }

                FirstCategoryId = catIdList[0];

                ucGeneralInformation.CategoryIdList = catIdList;
                ucGeneralInformation.CategoryNameList = catNameList;
                ucGeneralInformation.CategoryNameList = catNameList;                
            }
        }

        /// <summary>
        /// Load property values table with all property variations for current bundle
        /// If no variations are found, then load property names from current bundle`s category
        /// <author>Melon Team</author>
        /// <date>11/19/2009</date>
        /// </summary>
        private void LoadPropertyValues()
        {
            BundleProperty propertyList = new BundleProperty();
            propertyList.BundleId = SelectedObjectId;            
            DataSet ds = propertyList.List();

            if (ds.Tables[0].Rows.Count == 0)
            {
                divNoPropValues.Visible = true;
                return;
            }

            if (ds.Tables[0].Rows.Count > 0) // add property values for current bundle
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

                if (dtPropValues.Rows.Count > tabPropValueNames.Rows.Count || IsInsert == true)
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
        /// Loads bundle dynamic properties information.
        /// </summary>        
        private void LoadPropertyDefinitions()
        {
            ucDynamicPropDef.DynPropDefTable = tabPropValueNames.Copy();            
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
                    ((RequiredFieldValidator)e.Item.FindControl("cellUpdate").FindControl("rfvInsertItem")).ValidationGroup = IsInsert ? "InsertProperty" : "NoValidation";
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
                    ((RequiredFieldValidator)e.Item.FindControl("cellUpdate").FindControl("rfvInsertItem")).ValidationGroup = "NoValidation";
                }

                // show Edit button on the last cell of each row
                if (dataItem.DisplayIndex != lvPropValues.EditIndex)
                {
                    ((HtmlTableCell)e.Item.FindControl("cellEdit")).Style["display"] = ((dataItem.DisplayIndex + 1) % tabPropValueNames.Rows.Count) == 0 ? "" : "none";
                }


                if (lvPropValues.EditIndex > -1 &&
                    dataItem.DisplayIndex != lvPropValues.EditIndex &&
                    (dataItem.DisplayIndex) < lvPropValues.EditIndex &&
                    (dataItem.DisplayIndex + 1) > (lvPropValues.EditIndex - tabPropValueNames.Rows.Count + 1)
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
                    ((RequiredFieldValidator)e.Item.FindControl("cellUpdate").FindControl("rfvInsertItem")).ValidationGroup = "NoValidation";
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
            HtmlTable tabHeader = (HtmlTable)lvPropValues.FindControl("tabPropValues");

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
        /// Update dynamic property values for the selected bundle.
        /// </summary>
        /// <remarks>
        /// This method updates dynamic property values by passing xml-formated string to the dynamic property value save method.
        /// For more information see <see cref="Melon.Components.ProductCatalog.BundleProperty.SaveByXml"/>
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
                    (item.DisplayIndex) >= (lvPropValues.EditIndex - (tabPropValueNames.Rows.Count - 1 - 1))
                   )
                {
                    // get Product Property identifier from temp table 
                    string bundlePropertyId = dtPropValues.Rows[item.DisplayIndex]["Id"].ToString();

                    // get property Name from textbox control in edit mode
                    string propName = ((TextBox)item.FindControl("txtEdit")).Text;

                    // get product property id from hidden field in edit mode
                    string PropValueId = ((HiddenField)item.FindControl("hfPropertyValueId")).Value;

                    // add xml items to be saved in database
                    xmlStr += "<prop ProductPropId='' BundlePropId='" + bundlePropertyId + "' PropValueId='" + PropValueId + "' Name='" + propName + "'/>";
                }
            }
            xmlStr += "</root>";

            try
            {
                BundleProperty.SaveByXml(SelectedObjectId.Value, true, xmlStr);
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
        /// Saves or cancel saving dynamic property values for the selected bundle.
        /// </summary>
        /// <remarks>
        /// This method saves or cancel saving dynamic property values.
        /// CommandEventArgs argument parameter denotes whether to save or cancel saving added dynamic property values.
        /// The parameter contains one of the following values:
        /// <list type="bullet">
        /// <item>InsertAction - saves newly-added dynamic property values</item>
        /// <item>InsertCancel - cancels saving new dynamic property values</item>
        /// </list>        
        /// For more information see <see cref="Melon.Components.ProductCatalog.BundleProperty.SaveByXml"/>
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

                        // get Bundle Property identifier from temp table dtPropValues
                        string bundlePropertyId = tabPropValueNames.Rows[colNum]["PropId"].ToString();

                        // get property Name from textbox control in edit mode
                        string propValueName = ((TextBox)item.FindControl("txtEdit")).Text;

                        // add xml items to be saved in database
                        xmlStr += "<prop ProductPropId='' BundlePropId='"+bundlePropertyId+"' PropValueId='' Name='" + propValueName + "'/>";
                    }
                }

                xmlStr += "</root>";

                try
                {
                    BundleProperty.SaveByXml(SelectedObjectId.Value, false, xmlStr);
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
        /// Deletes dynamic property values for the selected bundle.
        /// </summary>
        /// <remarks>
        /// This method deletes dynamic property values by passing xml-formated string of dynamic property value object identifier.
        /// For more information see <see cref="Melon.Components.ProductCatalog.BundleProperty.DeleteByXml"/>
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
                    // get bundle property id from hidden field in edit mode
                    string propValueId = ((HiddenField)item.FindControl("hfPropertyValueId")).Value;

                    // add xml items to be saved in database
                    xmlStr += "<prop PropValueId='" + propValueId + "' />";
                }
            }
            xmlStr += "</root>";

            try
            {
                BundleProperty.DeleteByXml(xmlStr);
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
        /// Saves bundle details information contained in <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_GeneralInformation">GeneralInfornation child user control</see>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bundle"></param>
        private void SaveBundle(object sender, Bundle bundle)
        {
            bundle.Id = bundle.Id != null ? bundle.Id : (int?)null;            
            bundle.Code = ucGeneralInformation.Code;
            bundle.Name = ucGeneralInformation.Name;
            bundle.ShortDescription = ucGeneralInformation.ShortDesc;
            bundle.LongDescription = ucGeneralInformation.LongDesc;
            bundle.Tags = ucGeneralInformation.Tags;
            bundle.Price = ucGeneralInformation.CommonPrice;
            bundle.IsActive = ucGeneralInformation.IsActive;
            bundle.DateCreated = bundle.Id != null ? bundle.DateCreated : DateTime.Now;
            bundle.DateModified = DateTime.Now;
            bundle.CreatedBy = bundle.Id != null ? bundle.CreatedBy : this.ParentControl.CurrentUser.Adapter.Id;
            bundle.ModifiedBy = this.ParentControl.CurrentUser.Adapter.Id;

            string catIdList = "";
            foreach (int? item in ucGeneralInformation.CategoryIdList.ToArray())
            {
                catIdList += item.ToString() + ",";
            }

            if (catIdList != "")
            {
                bundle.CategoryIdList = catIdList.Substring(0, catIdList.Length - 1);
            }

            if (String.IsNullOrEmpty(bundle.CategoryIdList))
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("BundleCategoriesNotSelectedErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            try
            {
                bundle.Save();
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            LoadBundleEventArgs e = new LoadBundleEventArgs();
            e.SelectedBundleId = bundle.Id;
            e.RefreshExplorer = true;
            e.SelectedObjectType = ComponentObjectEnum.Bundle;
            e.SelectedTab = SelectedTab;
            e.Message = GetLocalResourceObject("BundleSaveSuccess").ToString();
            this.ParentControl.OnLoadBundle(sender, e);
        }

        /// <summary>
        /// Save bundle information in database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/19/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedObjectId != null && SelectedObjectId != 0)
            {
                Bundle bundle = Melon.Components.ProductCatalog.Bundle.Load(SelectedObjectId.Value);
                if (bundle == null)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = Convert.ToString(GetLocalResourceObject("BundleNotExistErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                SaveBundle(sender, bundle);
            }
            else
            {
                SaveBundle(sender, new Bundle());
            }
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
