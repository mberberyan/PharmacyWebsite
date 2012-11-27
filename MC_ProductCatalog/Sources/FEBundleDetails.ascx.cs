using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.Enumerations;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays bundle details information
    /// </summary>
    public partial class CodeBehind_FEBundleDetails : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Identifier of selected bundle object
        /// </summary>
        public int? BundleId
        {
            get
            {
                if (ViewState["mc_pc_BundleId"] == null)
                {
                    return (int?)null;
                }

                return (int?)ViewState["mc_pc_BundleId"];
            }

            set { ViewState["mc_pc_BundleId"] = value; }
        }

        /// <summary>
        /// Contains recordset of dynamic property names for selected bundle object
        /// </summary>
        /// <remarks>
        /// Table holds information about all dynamic properties assigned to the current bundle object.
        /// It consists of the following rows:
        /// <list cref="">
        /// <item>propId - identifier of dynamic property</item>
        /// <item>propName - name of dynamic property</item>
        /// </list>
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
        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>        
        protected override void OnInit(EventArgs e)
        {
            lvPropValues.DataBound += new EventHandler(lvPropValues_DataBound);
            repImageList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repImageList_ItemDataBound);
            repAudioList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repAudioList_ItemDataBound);
            repVideoList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repVideoList_ItemDataBound);
            gvBundleProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvBundleProducts_RowDataBound);
            BundleProductsPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(BundleProductsPager_PageChanged);            

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
                LoadBundleDetails();
            }
        }

        /// <summary>
        /// Loads bundle details information on web page and calls methods to load bundle`s dynamic properties,
        /// bundle images, audio and video files, as well as bundle product listing.
        /// </summary>
        private void LoadBundleDetails()
        {
            if (BundleId == null)
            {
                return;
            }

            Bundle bundle = Bundle.Load(BundleId.Value);

            lblBundleName.Text = bundle.Name;
            lblLongDescription.Text = bundle.LongDescription;
            imgBundle.ImageUrl = bundle.MainImageSrc != null ? Utilities.GetMediumImage(bundle.MainImageSrc) : (Utilities.GetImageUrl(this.Page, ProductCatalogSettings.MediumImageSrc));
            imgBundle.AlternateText = bundle.MainImageAltText;
            if (bundle.Price != null)
            {
                lblPrice.Text = GetLocalResourceObject("Price") + " " + bundle.Price + " " + ProductCatalogSettings.Currency;
            }

            LoadBundleFeatures();            
            LoadImageList();
            LoadAudioList();
            LoadVideoList();
            LoadBundleProductsList();
        }

        /// <summary>
        /// Load bundle-specific dynamic properties in a ListView control
        /// </summary>
        /// <remarks>
        /// Sets dynamic property names in temporary table <see cref="tabPropValueNames"/> and 
        /// loads dynamic property values in ListView control
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/12/2010</date>
        private void LoadBundleFeatures()
        {
            BundleProperty propertyList = new BundleProperty();
            propertyList.BundleId = BundleId;
            DataSet ds = propertyList.List();

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
                DataTable dtPropValues = ds.Tables[1].Copy();

                if (dtPropValues.Rows.Count == 0)
                {
                    divDynamicProperties.Visible = false;
                    lvPropValues.Visible = false;
                    return;
                }

                // set '---' to rows that has no values
                foreach (DataRow row in dtPropValues.Rows)
                {
                    if (row["PropertyValue"].ToString() == "")
                    {
                        row["PropertyValue"] = "---";
                    }
                }

                lvPropValues.DataSource = dtPropValues;
                lvPropValues.GroupItemCount = Convert.ToInt32(grpCount);
                lvPropValues.DataBind();
            }
        }

        /// <summary>
        /// Sets dynamic properties ListView control user interface
        /// </summary>
        /// <remarks>
        /// Sets interface css classes to header items of ListView control
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPropValues_DataBound(object sender, EventArgs e)
        {            
            HtmlTable tabHeader = (HtmlTable)lvPropValues.FindControl("tabPropValues");

            int rowIdx = 0;
            foreach (DataRow row in tabPropValueNames.Rows)
            {
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerHtml = row["propName"].ToString();

                if (rowIdx == 0)
                {
                    cell.Attributes.Add("class", "mc_pc_gridDynamicProperties_header mc_pc_grid_header_colFirst");
                }
                else if (rowIdx > 0 && rowIdx < tabPropValueNames.Rows.Count - 1)
                {
                    cell.Attributes.Add("class", "mc_pc_gridDynamicProperties_header mc_pc_grid_header_colMiddle");
                }
                else
                {
                    cell.Attributes.Add("class", "mc_pc_gridDynamicProperties_header mc_pc_grid_header_colLast");
                }

                tabHeader.Rows[0].Cells.Add(cell);
                rowIdx++;
            }
        }        

        /// <summary>
        /// Loads images in repeater control
        /// </summary>
        /// <remarks>
        /// Loads all current bundle`s  images except for the main image, that is already loaded on page
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>10/02/2009</date>
        private void LoadImageList()
        {
            DataTable imagesTab = new DataTable();

            BundleImage bundleImage = new BundleImage();
            bundleImage.BundleId = BundleId;
            imagesTab = bundleImage.List();

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

            BundleAudio bundleAudio = new BundleAudio();
            bundleAudio.BundleId = BundleId;
            audioTab = bundleAudio.List();

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

            BundleVideo bundleVideo = new BundleVideo();
            bundleVideo.BundleId = BundleId;
            videoTab = bundleVideo.List();


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
        /// Loads all products for the current bundle object.
        /// </summary>
        /// <remarks>
        /// Loads all products with their names, categories and price values in a table format.
        /// </remarks>
        private void LoadBundleProductsList()
        {
            BundleProducts bundleProducts = new BundleProducts();
            bundleProducts.BundleId = BundleId;
            bundleProducts.IsActive = true;
            DataTable dtBundleProductList = bundleProducts.List();

            //Display details of found products
            DataView dvBundleProductsList = new DataView(dtBundleProductList);
            if (dtBundleProductList.Rows.Count != 0)
            {
                dvBundleProductsList.Sort = "Name ASC";
            }

            gvBundleProducts.PageSize = Convert.ToInt16(PageItemNumberEnum.Six);
            gvBundleProducts.DataSource = dvBundleProductsList;
            gvBundleProducts.DataBind();

            //Display paging if there are products found.
            if (dtBundleProductList.Rows.Count != 0)
            {
                gvBundleProducts.Visible = true;
                BundleProductsPager.Visible = true;
                BundleProductsPager.FillPaging(gvBundleProducts.PageCount, gvBundleProducts.PageIndex + 1, 5, gvBundleProducts.PageSize, dtBundleProductList.Rows.Count);
            }
            else
            {
                gvBundleProducts.Visible = false;
                BundleProductsPager.Visible = false;
            }
        }

        /// <summary>
        /// Initializes bundle products` details in products listing
        /// </summary>
        /// <remarks>This method sets products` image file paths, common prices and links to product details pages</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBundleProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv=(DataRowView)e.Row.DataItem;
                HyperLink hplProduct = (HyperLink)e.Row.FindControl("hplProduct");
                hplProduct.NavigateUrl = "~/ObjectDetails.aspx?objType=ProductList&objId=" + Utilities.StrToHash(drv["ProductId"].ToString());

                HyperLink lbName = (HyperLink)e.Row.FindControl("lbName");
                lbName.NavigateUrl = hplProduct.NavigateUrl;

                Image imgBundle = (Image)e.Row.FindControl("imgBundle");
                string imageUrl = drv["ImagePath"].ToString();

                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                if (drv["CommonPrice"] != DBNull.Value)
                {
                    lblPrice.Text = drv["CommonPrice"].ToString() + " " + ProductCatalogSettings.Currency;
                }

                if (imageUrl == String.Empty)
                {
                    imgBundle.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
                }
                else
                {
                    imgBundle.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }
            }
        }

        /// <summary>
        /// Performs page changing for bundle products list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BundleProductsPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            gvBundleProducts.PageIndex = e.NewPage;
            LoadBundleProductsList();
        }

        /// <summary>
        /// Navigates back to bundle listing page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ObjectList.aspx?objType=BundleList");
        }

    }
}
