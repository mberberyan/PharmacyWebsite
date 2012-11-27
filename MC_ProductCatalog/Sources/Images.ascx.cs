using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.Exception;
using System.IO;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page where component object image files are manipulated.
    /// </summary>
    /// <remarks>
    ///  This page allows adding and deleting image files in the system, as well as editing their alternative texts
    ///  and selecting which image file to be selected as main one for the object 
    /// </remarks>
    public partial class CodeBehind_Images : ProductCatalogControl
    {
        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        protected override void OnInit(EventArgs e)
        {
            btnAddImage.Click += new EventHandler(btnSaveImage_Click);
            btnEditImage.Click += new EventHandler(btnSaveImage_Click);
            repImageList.ItemCommand += new RepeaterCommandEventHandler(repImageList_ItemCommand);            
            repImageList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repImageList_ItemDataBound);
            base.OnInit(e);
        }

        /// <summary>
        /// Initializes the user control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                LoadImageList();
            }
        }

        /// <summary>
        /// Load images in repeater control
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>10/02/2009</date>
        private void LoadImageList()
        {
            divImageLimit.InnerHtml = GetLocalResourceObject("AllowedFileSize").ToString() + " " + Utilities.GetImageAllowedExtStr() + "<br/>" + GetLocalResourceObject("MaxFileSize").ToString() + " " + ProductCatalogSettings.ImagesMaxSize + " KB";
            DataTable imagesTab = new DataTable();

            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Category:
                    if (SelectedObjectId != null)
                    {
                        CategoryImage catImage = new CategoryImage();
                        catImage.CategoryId = SelectedObjectId;
                        imagesTab = catImage.List();
                    }
                    break;
                case ComponentObjectEnum.Product:
                    if (SelectedProductId != null)
                    {
                        ProductImage productImage = new ProductImage();
                        productImage.ProductId = SelectedProductId;
                        imagesTab = productImage.List();
                    }
                    break;
                case ComponentObjectEnum.Bundle:
                    if (SelectedObjectId != null)
                    {
                        BundleImage bundleImage = new BundleImage();
                        bundleImage.BundleId = SelectedObjectId;
                        imagesTab = bundleImage.List();
                    }
                    break;
                case ComponentObjectEnum.Collection:
                    if (SelectedObjectId != null)
                    {
                        CollectionImage collImage = new CollectionImage();
                        collImage.CollectionId = SelectedObjectId;
                        imagesTab = collImage.List();
                    }
                    break;
            }            


            if (imagesTab.Rows.Count > 0)
            {
                repImageList.DataSource = imagesTab;
                repImageList.DataBind();
                repImageList.Visible = true;
                lblNoImages.Visible = false;
            }
            else
            {
                repImageList.Visible = false;
                lblNoImages.Visible = true;
            }
        }

        /// <summary>
        /// Sets image file parameters in the image list
        /// </summary>
        /// <remarks>
        /// Image file physical path, alternate text and main image flag are set in this repeater data-binding method:        
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repImageList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv=(DataRowView)e.Item.DataItem;

                HtmlImage imgItem = (HtmlImage)e.Item.FindControl("imgItem");
                HtmlImage imgCheck = (HtmlImage)e.Item.FindControl("imgCheck");
                Label lblImageName = (Label)e.Item.FindControl("lblImageName");
                Button btnRemoveImage = (Button)e.Item.FindControl("btnRemoveImage");

                imgItem.Src = Utilities.GetThumbImage(drv["ImagePath"].ToString());
                imgItem.Alt = drv["AltText"].ToString();
                imgItem.Attributes.Add("name", drv["Id"].ToString());
                imgCheck.Visible = Convert.ToBoolean(drv["IsMain"]);
                lblImageName.Text = drv["ImagePath"].ToString();
                btnRemoveImage.CommandArgument = drv["Id"].ToString() + ";" + drv["ImagePath"].ToString();

                if (Convert.ToBoolean(drv["IsMain"]))
                {
                    hfMainImageSrc.Value = drv["Id"].ToString();
                }
            }
        }

        /// <summary>
        /// Save new or edit image details
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/30/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveImage_Click(object sender, EventArgs e)
        {
            SaveImageEventArgs args = new SaveImageEventArgs();
            args.Id = !Convert.ToBoolean(hfAddNewImage.Value) ? Convert.ToInt32(hfImageId.Value) : (int?)null;
            args.ImageAlt = txtPopupImageAlt.Text.Trim();
            args.IsMainImage = chkMainImage.Checked;
            args.ImageFile = Convert.ToBoolean(hfAddNewImage.Value) ? fuImage : null;            

            this.BaseParentControl.OnSaveImage(sender, args);

            LoadImageList();
        }

        /// <summary>
        /// Remove image from object image list
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>10/05/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repImageList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DeleteImageEventArgs args = new DeleteImageEventArgs();
            args.Id = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[0]);
            args.ImageName = e.CommandArgument.ToString().Split(';')[1];

            this.BaseParentControl.OnDeleteImage(source, args);

            LoadImageList();
        }
    }
}
