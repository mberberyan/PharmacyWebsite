using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.Enumerations;
using Melon.Components.ProductCatalog.ComponentEngine;


namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays collection details information
    /// </summary>
    public partial class CodeBehind_FECollectionDetails : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Identifier of selected collection object
        /// </summary>
        public int? CollectionId
        {
            get
            {
                if (ViewState["mc_pc_CollectionId"] == null)
                {
                    return (int?)null;
                }

                return (int?)ViewState["mc_pc_CollectionId"];
            }

            set { ViewState["mc_pc_CollectionId"] = value; }
        }

        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>        
        protected override void OnInit(EventArgs e)
        {
            repImageList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repImageList_ItemDataBound);
            repAudioList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repAudioList_ItemDataBound);
            repVideoList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repVideoList_ItemDataBound);
            gvCollectionProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvCollectionProducts_RowDataBound);
            CollectionProductsPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(CollectionProductsPager_PageChanged);

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes user control information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCollectionProducts();
            }
        }

        /// <summary>
        /// Loads collection details information on web page and calls methods to load collection`s 
        /// images, audio and video files, as well as collection products listing.
        /// </summary>
        private void LoadCollectionProducts()
        {
            if (CollectionId == null)
            {
                return;
            }

            Collection coll = Collection.Load(CollectionId.Value);

            lblCollectionName.Text = coll.Name;
            lblLongDescription.Text = coll.LongDescription;
            imgCollection.ImageUrl = coll.MainImageSrc != null ? Utilities.GetMediumImage(coll.MainImageSrc) : (Utilities.GetImageUrl(this.Page, ProductCatalogSettings.MediumImageSrc));
            imgCollection.AlternateText = coll.MainImageAltText;
            LoadImageList();
            LoadAudioList();
            LoadVideoList();
            LoadCollectionProductsList();
        }

        /// <summary>
        /// Loads images in repeater control
        /// </summary>
        /// <remarks>
        /// Loads all current catalog`s  images except for the main image, that is already loaded on page
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>10/02/2009</date>
        private void LoadImageList()
        {
            DataTable imagesTab = new DataTable();

            CollectionImage collImage = new CollectionImage();
            collImage.CollectionId = CollectionId;
            imagesTab = collImage.List();

            // remove main image from Other images list, as main image is already listed
            foreach (DataRow row in imagesTab.Rows)
            {
                if (Convert.ToBoolean(row["IsMain"]))
                {
                    imagesTab.Rows.Remove(row);
                    break;
                }
            }

            if (imagesTab.Rows.Count > 0)
            {
                repImageList.DataSource = imagesTab;
                repImageList.DataBind();
                divOtherImages.Visible = true;
                lblNoImages.Visible = false;
            }
            else
            {
                divOtherImages.Visible = false;
                lblNoImages.Visible = true;
            }
        }

        /// <summary>
        /// Load audio files in repeater control
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>10/11/2009</date>
        private void LoadAudioList()
        {
            DataTable audioTab = new DataTable();

            CollectionAudio collAudio = new CollectionAudio();
            collAudio.CollectionId = CollectionId;
            audioTab = collAudio.List();

            if (audioTab.Rows.Count > 0)
            {
                repAudioList.DataSource = audioTab;
                repAudioList.DataBind();
                divAudio.Visible = true;
                lblNoAudio.Visible = false;
            }
            else
            {
                divAudio.Visible = false;
                lblNoAudio.Visible = true;
            }
        }

        /// <summary>
        /// Load video files in repeater control
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>10/11/2009</date>
        private void LoadVideoList()
        {
            DataTable videoTab = new DataTable();

            CollectionVideo collVideo = new CollectionVideo();
            collVideo.CollectionId = CollectionId;
            videoTab = collVideo.List();


            if (videoTab.Rows.Count > 0)
            {
                repVideoList.DataSource = videoTab;
                repVideoList.DataBind();
                divVideo.Visible = true;
                lblNoVideo.Visible = false;
            }
            else
            {
                divVideo.Visible = false;
                lblNoVideo.Visible = true;
            }
        }

        /// <summary>
        /// Sets all images file path, names and preview event caller in repeater control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repImageList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                HtmlImage imgItem = (HtmlImage)e.Item.FindControl("imgItem");
                Label lblImageName = (Label)e.Item.FindControl("lblImageName");

                imgItem.Src = Utilities.GetThumbImage(drv["ImagePath"].ToString());
                imgItem.Alt = drv["AltText"].ToString();
                imgItem.Attributes.Add("onclick", "javascript:centerImagePopup();loadFEImagePopup('" + Page.ResolveUrl(Utilities.GetOriginalImage(drv["ImagePath"].ToString())) + "');");
            }
        }

        /// <summary>
        /// Sets all audio file path, names and preview event caller in repeater control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repAudioList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlImage audioItem = (HtmlImage)e.Item.FindControl("audioItem");
                Label lblAudioName = (Label)e.Item.FindControl("lblAudioName");

                audioItem.Src = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.AudioImage);
                audioItem.Attributes.Add("onclick", "javascript:openAudioPopup('../Sources/flashPopupAudio.htm?file=../Data/Audio/" + ((DataRowView)e.Item.DataItem)["AudioPath"].ToString() + "')");
                lblAudioName.Text = ((DataRowView)e.Item.DataItem)["Title"].ToString();
            }
        }

        /// <summary>
        /// Sets all video file path, names and preview event caller in repeater control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repVideoList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlImage videoItem = (HtmlImage)e.Item.FindControl("videoItem");
                Label lblVideoName = (Label)e.Item.FindControl("lblVideoName");

                videoItem.Src = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.VideoImage);
                videoItem.Attributes.Add("onclick", "javascript:openVideoPopup('../Sources/flashPopupVideo.htm?file=../../Data/Video/" + ((DataRowView)e.Item.DataItem)["VideoPath"].ToString() + "')");

                lblVideoName.Text = ((DataRowView)e.Item.DataItem)["Title"].ToString();
            }
        }

        /// <summary>
        /// Loads all products for the current collection object.
        /// </summary>
        /// <remarks>
        /// Loads all products with their names, categories and price values in a table format.
        /// </remarks>
        private void LoadCollectionProductsList()
        {
            CollectionProducts collProducts = new CollectionProducts();
            collProducts.CollectionId = CollectionId;
            collProducts.IsActive = true;
            DataTable dtCollectionProductList = collProducts.List();

            //Display details of found products
            DataView dvCollectionProductsList = new DataView(dtCollectionProductList);
            if (dtCollectionProductList.Rows.Count != 0)
            {
                dvCollectionProductsList.Sort = "Name ASC";
            }

            gvCollectionProducts.PageSize = Convert.ToInt16(PageItemNumberEnum.Six);
            gvCollectionProducts.DataSource = dvCollectionProductsList;
            gvCollectionProducts.DataBind();

            //Display paging if there are products found.
            if (dtCollectionProductList.Rows.Count != 0)
            {
                gvCollectionProducts.Visible = true;
                CollectionProductsPager.Visible = true;
                CollectionProductsPager.FillPaging(gvCollectionProducts.PageCount, gvCollectionProducts.PageIndex + 1, 5, gvCollectionProducts.PageSize, dtCollectionProductList.Rows.Count);
            }
            else
            {
                gvCollectionProducts.Visible = false;
                CollectionProductsPager.Visible = false;
            }
        }

        /// <summary>
        /// Initializes collection products` details in products listing
        /// </summary>
        /// <remarks>This method sets products` image file paths, common prices and links to product details pages</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCollectionProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                HyperLink hplProduct = (HyperLink)e.Row.FindControl("hplProduct");
                hplProduct.NavigateUrl = "~/ObjectDetails.aspx?objType=ProductList&objId=" + Utilities.StrToHash(drv["ProductId"].ToString());

                HyperLink lbName = (HyperLink)e.Row.FindControl("lbName");
                lbName.NavigateUrl = hplProduct.NavigateUrl;


                Image imgCollection = (Image)e.Row.FindControl("imgCollection");
                string imageUrl = drv["ImagePath"].ToString();

                if (imageUrl == String.Empty)
                {
                    imgCollection.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
                }
                else
                {
                    imgCollection.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }

                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                if (drv["CommonPrice"] != DBNull.Value)
                {
                    lblPrice.Text = drv["CommonPrice"].ToString() + " " + ProductCatalogSettings.Currency;
                }
            }
        }

        /// <summary>
        /// Performs page changing for collection products list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CollectionProductsPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            gvCollectionProducts.PageIndex = e.NewPage;
            LoadCollectionProductsList();
        }

        /// <summary>
        /// Navigates back to collection listing page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ObjectList.aspx?objType=CollectionsList");
        }
    }
}
