using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Exception;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing all collections in the system.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     It contains user controls to manage selected collection from the <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.Explorer">Collection Explorer control</see>.
    ///     Following child user controls are used to display and manage collection details:
    ///     <list type="bullet">
    ///     <item>GeneralInformation.ascx - displays collection details information.</item>
    ///     <item>ProductSet.ascx - displays product list and filter functionality to display 
    ///     all selected products for the current collection object.</item>
    ///     <item>Images.ascx - displays all selected image files for current collection object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images listing user control</see></item>
    ///     <item>Audio.ascx - displays all selected audio files for current collection object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio">Audio listing user control</see></item>
    ///     <item>Video.ascx - displays all selected video files for current collection object.For more details see <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video">Video listing user control</see></item>    
    ///     </list>
    /// </para>
    /// </remarks>
    public partial class CodeBehind_Collection : ProductCatalogControl
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

            this.ParentControl.SaveImageEvent += new SaveImageEventHandler(SaveImageEvent);
            this.ParentControl.DeleteImageEvent += new DeleteImageEventHandler(DeleteImageEvent);

            this.ParentControl.SaveAudioEvent += new SaveAudioEventHandler(SaveAudioEvent);
            this.ParentControl.DeleteAudioEvent += new DeleteAudioEventHandler(DeleteAudioEvent);

            this.ParentControl.SaveVideoEvent += new SaveVideoEventHandler(SaveVideoEvent);
            this.ParentControl.DeleteVideoEvent += new DeleteVideoEventHandler(DeleteVideoEvent);

            base.OnInit(e);
        }

        #region Custom Events
        /// <summary>
        /// Saves images added to the current collection object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images">Images user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Images.btnSaveImage_Click"> method to save image is fired.
        /// </remarks>        
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void SaveImageEvent(object sender, SaveImageEventArgs e)
        {
            Collection coll = Melon.Components.ProductCatalog.Collection.Load(SelectedObjectId.Value);
            if (coll == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CollectionNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            CollectionImage collImage = new CollectionImage();
            if (!isNew)
            {
                collImage.Id = e.Id;
                collImage.Load();
            }

            collImage.AltText = e.ImageAlt;
            collImage.CollectionId = SelectedObjectId;
            collImage.IsMain = e.IsMainImage;

            if (isNew)
            {
                collImage.ImageFile = e.ImageFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                collImage.ImagePath = randomCode.ToString();
            }

            try
            {
                CollectionImage.Save(collImage);
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
        /// Deletes images from current collection object.        
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
                CollectionImage.Delete(e.Id, e.ImageName);
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
        /// Saves audio files added to the current collection object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio">Audio user control</see>
        /// when <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Audio.btnSaveAudio_Click"> method to save audio file is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAudioEvent(object sender, SaveAudioEventArgs e)
        {
            Collection coll = Melon.Components.ProductCatalog.Collection.Load(SelectedObjectId.Value);
            if (coll == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CollectionNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            CollectionAudio collAudio = new CollectionAudio();
            if (!isNew)
            {
                collAudio.Id = e.Id;
                collAudio.Load();
            }

            collAudio.Title = e.Title;
            collAudio.CollectionId = SelectedObjectId;
            collAudio.IsMain = e.IsMainAudio;

            if (isNew)
            {
                collAudio.AudioFile = e.AudioFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                collAudio.AudioPath = randomCode.ToString();
            }

            try
            {
                CollectionAudio.Save(collAudio);
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
        /// Deletes audio file from current collection object.        
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
                CollectionAudio.Delete(e.Id, e.AudioName);
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
        /// Saves video file added to the current collection object.        
        /// </summary>
        /// <remarks>This method is called by <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video">Video user control</see>
        /// when save <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Video.btnSaveVideo_Click"> method to save video file is fired.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveVideoEvent(object sender, SaveVideoEventArgs e)
        {
            Collection coll = Melon.Components.ProductCatalog.Collection.Load(SelectedObjectId.Value);
            if (coll == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CollectionNotExistErrorMessage"));
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            bool isNew = e.Id == null;
            CollectionVideo collVideo = new CollectionVideo();
            if (!isNew)
            {
                collVideo.Id = e.Id;
                collVideo.Load();
            }

            collVideo.Title = e.Title;
            collVideo.CollectionId = SelectedObjectId;
            collVideo.IsMain = e.IsMainVideo;

            if (isNew)
            {
                collVideo.VideoFile = e.VideoFile;
                Random RandomClass = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomCode = RandomClass.Next(10000000, Int32.MaxValue);
                collVideo.VideoPath = randomCode.ToString();
            }

            try
            {
                CollectionVideo.Save(collVideo);
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
        /// Deletes video file from current collection object.        
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
                CollectionVideo.Delete(e.Id, e.VideoName);
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
                    divCollectionDetails.Visible = false;
                }
                else
                {
                    divExplorerLayout.Visible = false;
                    divCollectionDetails.Visible = true;
                    SelectedTab = SelectedTab != ProductCatalogTabs.Unknown ? SelectedTab : ProductCatalogTabs.GeneralInformation;

                    if (SelectedObjectId != null && SelectedObjectId != 0)
                    {
                        Collection coll = Melon.Components.ProductCatalog.Collection.Load(SelectedObjectId.Value);
                        if (coll == null)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CollectionNotExistErrorMessage"));
                            this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }

                        LoadCollection(coll);
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
                }
            }
        }

        /// <summary>
        /// Loads collection details information.
        /// </summary>
        /// <param name="bundle"></param>
        private void LoadCollection(Collection coll)
        {
            if (coll != null)
            {
                ucGeneralInformation.Code = coll.Code;
                ucGeneralInformation.Name = coll.Name;
                ucGeneralInformation.ShortDesc = coll.ShortDescription;
                ucGeneralInformation.LongDesc = coll.LongDescription;
                ucGeneralInformation.Tags = coll.Tags;
                ucGeneralInformation.IsActive = coll.IsActive;
            }
        }

        /// <summary>
        /// Saves collection details information contained in <see cref="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_GeneralInformation">GeneralInfornation child user control</see>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bundle"></param>
        private void SaveCollection(object sender, Collection coll)
        {
            coll.Id = coll.Id != null ? coll.Id : (int?)null;
            coll.Code = ucGeneralInformation.Code;
            coll.Name = ucGeneralInformation.Name;
            coll.ShortDescription = ucGeneralInformation.ShortDesc;
            coll.LongDescription = ucGeneralInformation.LongDesc;
            coll.Tags = ucGeneralInformation.Tags;
            coll.IsActive = ucGeneralInformation.IsActive;
            coll.DateCreated = coll.Id != null ? coll.DateCreated : DateTime.Now;
            coll.DateModified = DateTime.Now;
            coll.CreatedBy = coll.Id != null ? coll.CreatedBy : this.ParentControl.CurrentUser.Adapter.Id;
            coll.ModifiedBy = this.ParentControl.CurrentUser.Adapter.Id;

            try
            {
                coll.Save();
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = ex.Message;
                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            LoadCollectionEventArgs e = new LoadCollectionEventArgs();
            e.SelectedCollectionId = coll.Id;
            e.RefreshExplorer = true;
            e.SelectedObjectType = ComponentObjectEnum.Collection;
            e.SelectedTab = SelectedTab;
            e.Message = GetLocalResourceObject("CollectionSaveSuccess").ToString();
            this.ParentControl.OnLoadCollection(sender, e);
        }

        /// <summary>
        /// Save collection information in database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>12/03/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedObjectId != null && SelectedObjectId != 0)
            {
                Collection coll = Melon.Components.ProductCatalog.Collection.Load(SelectedObjectId.Value);
                if (coll == null)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CollectionNotExistErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                SaveCollection(sender, coll);
            }
            else
            {
                SaveCollection(sender, new Collection());
            }
        }
    }
}
