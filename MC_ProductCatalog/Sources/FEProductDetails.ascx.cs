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
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays product details information
    /// </summary>
    public partial class CodeBehind_FEProductDetails : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Identifier of selected product object
        /// </summary>
        public int? ProductId
        {
            get 
            {
                if (ViewState["mc_pc_ProductId"] == null)
                {
                    return (int?)null;
                }

                return (int?)ViewState["mc_pc_ProductId"];
            }

            set { ViewState["mc_pc_ProductId"] = value; }
        }

        /// <summary>
        /// Contains recordset of dynamic property names for selected product object
        /// </summary>
        /// <remarks>
        /// Table holds information about all dynamic properties assigned to the current product object.
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

        /// <summary>
        /// Object of type <see cref="Melon.Components.ProductCatalog.Product"/> that holds product details information.
        /// </summary>
        private Product _product = null;
        
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
            gvProductReview.RowDataBound += new GridViewRowEventHandler(gvProductReview_RowDataBound);
            dlRelatedProducts.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(dlRelatedProducts_ItemDataBound);
            TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);
            ProductReviewPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(ProductReviewPager_PageChanged);
            btnSubmitReview.Click += new EventHandler(btnSubmitReview_Click);                

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
                LoadProductDetails();               
            }
        }

        /// <summary>
        /// Saves product visit date and time
        /// </summary>
        private void SaveProductStatistics()
        {
            Statistics stats = new Statistics();
            stats.ProductId = ProductId;
            stats.DateViewed = DateTime.Now;

            stats.Save();
        }

        /// <summary>
        /// Loads product details information on web page and calls methods to load product`s dynamic properties,
        /// product images, audio and video files, as well as related product listing.
        /// </summary>
        private void LoadProductDetails()
        {
            if (ProductId == null)
            {
                return;
            }            

            _product = Product.Load(ProductId.Value);

            lblProductName.Text = _product.Name;
            lblLongDescription.Text = !String.IsNullOrEmpty(_product.LongDescription) ? _product.LongDescription : (!String.IsNullOrEmpty(_product.ShortDescription) ? _product.ShortDescription : "");
            divDescription.Visible=lblLongDescription.Text!=String.Empty;
            imgProduct.ImageUrl = _product.MainImageSrc != null ? Utilities.GetMediumImage(_product.MainImageSrc) : (Utilities.GetImageUrl(this.Page, ProductCatalogSettings.MediumImageSrc));
            imgProduct.AlternateText = _product.MainImageAltText;
            imgProduct.Attributes.Add("onclick", "javascript:centerImagePopup();loadFEImagePopup('" + Page.ResolveUrl(Utilities.GetOriginalImage(_product.MainImageSrc) + "');"));
            LoadProductFeatures();
            LoadProductPrice();
            LoadImageList();
            LoadAudioList();
            LoadVideoList();
            LoadRelatedProducts(0);
            LoadProductReviews();

            SaveProductStatistics();
        }

        /// <summary>
        /// Load product-specific dynamic properties in a ListView control
        /// </summary>
        /// <remarks>
        /// Sets dynamic property names in temporary table <see cref="tabPropValueNames"/> and 
        /// loads dynamic property values in ListView control
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/12/2010</date>
        private void LoadProductFeatures()
        {
            ProductProperty propertyList = new ProductProperty();
            propertyList.ProductId = ProductId;            
            DataSet ds = propertyList.List();

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
            else
            {
                divDynamicProperties.Visible = false;
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
        /// Load product price listing including common price and all active product discounts
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>02/12/2010</date>
        private void LoadProductPrice()
        {
            DiscountProducts discountProducts = new DiscountProducts();
            discountProducts.ProductId = ProductId;
            
            DataTable tabDiscountProducts = DiscountProducts.ListActive(discountProducts);

            double productPrice = Convert.ToDouble(_product.CommonPrice);
            divProductPrice.InnerHtml = "<table cellpadding='0' cellspacing='0' class='mc_pc_gridPrice'>";           
            divProductPrice.InnerHtml += "<tr><td class='mc_pc_gridPrice_commonPrice'>" + GetLocalResourceObject("CommonPrice").ToString() + ": </td><td class='mc_pc_gridPrice_commonPricePrice'> &nbsp; " + _product.CommonPrice.ToString() + " " + ProductCatalogSettings.Currency + "</td></tr>";            
            foreach (DataRow row in tabDiscountProducts.Rows)
            {
                divProductPrice.InnerHtml += "<tr><td class='mc_pc_gridPrice_discountName'>" + row["DiscountName"] + ": </td><td class='mc_pc_gridPrice_discount'> - " + row["DiscountValue"] + (Convert.ToInt16(row["DiscountType"]) == (int)DiscountTypeEnum.Value ? ProductCatalogSettings.Currency : "%") + "</td></tr>";
                
                if((Convert.ToInt16(row["DiscountType"]) == (int)DiscountTypeEnum.Value))
                {
                    productPrice-=Convert.ToDouble(row["DiscountValue"]);
                }
                else
                {
                    productPrice-=productPrice*Convert.ToDouble(row["DiscountValue"])/100;
                }                
            }

            if (tabDiscountProducts.Rows.Count > 0)
            {
                divProductPrice.InnerHtml += "<tr><td class='mc_pc_gridPrice_total' colspan='2'>" + GetLocalResourceObject("Total").ToString() + ": "+productPrice.ToString("0.00") + " " + ProductCatalogSettings.Currency + "</td></tr>";
            }

            if (!_product.IsInStock.Value)
            {
                divProductPrice.InnerHtml += "<tr><td class='mc_pc_gridPrice_outOfStock' colspan='2' align='left'><b><span class='mc_pc_short_error_message'>" + GetLocalResourceObject("ProductOutOfStock").ToString() + "</b></td></tr>";
            }
            divProductPrice.InnerHtml += "</table>";
        }

        /// <summary>
        /// Load images in repeater control
        /// </summary>
        /// <remarks>
        /// Loads all current product`s  images except for the main image, that is already loaded on page
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>10/02/2009</date>
        private void LoadImageList()
        {
            DataTable imagesTab = new DataTable();
            
            ProductImage productImage = new ProductImage();
            productImage.ProductId = ProductId;
            imagesTab = productImage.List();

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

            ProductAudio productAudio = new ProductAudio();
            productAudio.ProductId = ProductId;
            audioTab = productAudio.List();

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
            
            ProductVideo productVideo = new ProductVideo();
            productVideo.ProductId = ProductId;
            videoTab = productVideo.List();    
    

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
                DataRowView drv=(DataRowView)e.Item.DataItem;
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
        /// Loads all related products for the current product object.
        /// </summary>
        /// <remarks>
        /// Loads all products with their names, categories and price values in a table format.
        /// </remarks>
        private void LoadRelatedProducts(int pageIndex)
        {
            RelatedProducts relatedProducts=new RelatedProducts();
            relatedProducts.ProductId=ProductId;
            DataTable dtRelatedProducts = RelatedProducts.List(relatedProducts);
            PagedDataSource pagedDS = new PagedDataSource();

            //Display details of found product reviews
            DataView dvRelatedProducts = new DataView(dtRelatedProducts);
            if (dtRelatedProducts.Rows.Count != 0)
            {
                dvRelatedProducts.Sort = "Name ASC";
            }

            pagedDS.DataSource = dvRelatedProducts;
            pagedDS.AllowPaging = true;
            pagedDS.CurrentPageIndex = pageIndex;
            pagedDS.PageSize = (int)PageItemNumberEnum.Six;


            //Display paging if there are users found.
            if (dtRelatedProducts.Rows.Count != 0)
            {
                divRelatedProducts.Visible = true;
                TopPager.Visible = true;

                TopPager.FillPaging(pagedDS.PageCount, pagedDS.CurrentPageIndex + 1, 5, pagedDS.PageSize, dtRelatedProducts.Rows.Count);
            }
            else
            {
                divRelatedProducts.Visible = false;
                TopPager.Visible = false;
            }

            dlRelatedProducts.DataSource = pagedDS;
            dlRelatedProducts.DataBind();
        }

        /// <summary>
        /// Load all reviews for current product object.
        /// </summary>
        private void LoadProductReviews()
        {
            ProductReview review = new ProductReview();
            review.ProductId = ProductId;            
            DataTable dtProductReviewList = review.List();

            //Display details of found products
            DataView dvProductReviewList = new DataView(dtProductReviewList);
            if (dtProductReviewList.Rows.Count != 0)
            {
                dvProductReviewList.Sort = "DatePosted DESC";
            }

            gvProductReview.PageSize = Convert.ToInt16(PageItemNumberEnum.Six);
            gvProductReview.DataSource = dvProductReviewList;
            gvProductReview.DataBind();

            //Display paging if there are products found.
            if (dtProductReviewList.Rows.Count != 0)
            {
                gvProductReview.Visible = true;   
                ProductReviewPager.Visible = true;
                ProductReviewPager.FillPaging(gvProductReview.PageCount, gvProductReview.PageIndex + 1, 5, gvProductReview.PageSize, dtProductReviewList.Rows.Count);
            }
            else
            {
                gvProductReview.Visible = false;
                ProductReviewPager.Visible = false;
            }
        }

        /// <summary>
        /// Submit new product review for current product.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            ProductReview review = new ProductReview();
            review.ProductId = ProductId;
            review.PostedBy = txtName.Text.Trim();
            review.Text = txtReview.Text.Trim();
            review.Rating = Convert.ToInt16(txtRating.Text);
            review.DatePosted = DateTime.Now;

            try
            {
                review.Save();
                LoadProductReviews();

                txtName.Text = String.Empty;
                txtReview.Text = String.Empty;
                txtRating.Text = String.Empty;
            }
            catch(ProductCatalogException args)
            {
                divProductReviewSaveError.Visible = true;
                divProductReviewSaveError.InnerText = args.Message;
            }
        }

        /// <summary>
        /// Loads product review`s author name, message text and rating.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgRating = (Image)e.Row.FindControl("imgRating");
                imgRating.ImageUrl = Utilities.GetImageUrl(this.Page, "rating_" + ((DataRowView)e.Row.DataItem)["Rating"].ToString() + ".gif");
            }
        }

        /// <summary>
        /// Performs page changing for related products list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            LoadRelatedProducts(e.NewPage);
        }

        /// <summary>
        /// Performs page changing for product reviews list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProductReviewPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            gvProductReview.PageIndex = e.NewPage;
            LoadProductReviews();
        }

        /// <summary>
        /// Loads all related products for the current product object.
        /// </summary>
        /// <remarks>
        /// Loads all related products for the current one with their names, categories and price values in a table format.
        /// </remarks>
        protected void dlRelatedProducts_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv=(DataRowView)e.Item.DataItem;

                HtmlAnchor aName = (HtmlAnchor)e.Item.FindControl("aName");
                aName.HRef = "~/ObjectDetails.aspx?objType=ProductList&objId=" + Utilities.StrToHash(drv["ProductId"].ToString());
                aName.InnerText = drv["Name"].ToString();

                Image imgProduct = (Image)e.Item.FindControl("imgProduct");
                string imageUrl = drv["ImagePath"].ToString();

                if (imageUrl == String.Empty)
                {
                    imgProduct.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
                }
                else
                {
                    imgProduct.ImageUrl = Utilities.GetThumbImage(imageUrl);
                }

                HyperLink hplRelatedProduct = (HyperLink)e.Item.FindControl("hplRelatedProduct");
                hplRelatedProduct.NavigateUrl = aName.HRef;
            }
        }

        /// <summary>
        /// Navigates back to product listing page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ObjectList.aspx?objType=ProductList");
        }
    }
}
