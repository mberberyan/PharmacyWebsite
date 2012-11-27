using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Configuration;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page where component object video files are manipulated.
    /// </summary>
    /// <remarks>
    ///  This page allows adding and deleting video files in the system, as well as editing their titles 
    ///  and selecting which video file to be selected as main one for the object 
    /// </remarks>
    public partial class CodeBehind_Video : ProductCatalogControl
    {        

        protected override void OnInit(EventArgs e)
        {
            btnAddVideo.Click += new EventHandler(btnSaveVideo_Click);
            btnEditVideo.Click += new EventHandler(btnSaveVideo_Click);
            repVideoList.ItemCommand += new RepeaterCommandEventHandler(repVideoList_ItemCommand);
            repVideoList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repVideoList_ItemDataBound);
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
                LoadVideoList();
            }
        }

        /// <summary>
        /// Load video files in repeater control
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>10/11/2009</date>
        private void LoadVideoList()
        {
            divVideoLimit.InnerHtml = GetLocalResourceObject("AllowedFileSize").ToString() + " " + Utilities.GetVideoAllowedExtStr() + "<br/>" + GetLocalResourceObject("MaxFileSize").ToString() + " " + ProductCatalogSettings.VideoMaxSize + " KB";
            DataTable videoTab = new DataTable();

            switch (SelectedObjectType)
            {
                case ComponentObjectEnum.Product:
                    if (SelectedProductId != null)
                    {
                        ProductVideo productVideo = new ProductVideo();
                        productVideo.ProductId = SelectedProductId;
                        videoTab = productVideo.List();
                    }
                    break;
                case ComponentObjectEnum.Bundle:
                    if (SelectedObjectId != null)
                    {
                        BundleVideo bundleVideo = new BundleVideo();
                        bundleVideo.BundleId = SelectedObjectId;
                        videoTab = bundleVideo.List();
                    }
                    break;
                case ComponentObjectEnum.Collection:
                    if (SelectedObjectId != null)
                    {
                        CollectionVideo collVideo = new CollectionVideo();
                        collVideo.CollectionId = SelectedObjectId;
                        videoTab = collVideo.List();
                    }
                    break;
            }


            if (videoTab.Rows.Count > 0)
            {
                repVideoList.DataSource = videoTab;
                repVideoList.DataBind();
                repVideoList.Visible = true;
                lblNoVideo.Visible = false;
            }
            else
            {
                repVideoList.Visible = false;
                lblNoVideo.Visible = true;
            }
        }

        /// <summary>
        /// Sets video file parameters in the video list
        /// </summary>
        /// <remarks>
        /// Video physical path, title name and main video flag are set in this repeater data-binding method:        
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repVideoList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlImage videoItem = (HtmlImage)e.Item.FindControl("videoItem");
                HtmlImage videoCheck = (HtmlImage)e.Item.FindControl("videoCheck");
                Label lblVideoName = (Label)e.Item.FindControl("lblVideoName");
                Button btnRemoveVideo = (Button)e.Item.FindControl("btnRemoveVideo");

                videoItem.Src = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.VideoImage);
                videoItem.Attributes.Add("name", ((DataRowView)e.Item.DataItem)["Id"].ToString() + ";" + ((DataRowView)e.Item.DataItem)["VideoPath"].ToString());
                videoItem.Alt = ((DataRowView)e.Item.DataItem)["Title"].ToString();
                videoCheck.Visible = Convert.ToBoolean(((DataRowView)e.Item.DataItem)["IsMain"]);
                lblVideoName.Text = Melon.General.StringUtils.Cut(((DataRowView)e.Item.DataItem)["Title"].ToString(), " ", 20, 5);
                lblVideoName.ToolTip = ((DataRowView)e.Item.DataItem)["Title"].ToString();
                btnRemoveVideo.CommandArgument = ((DataRowView)e.Item.DataItem)["Id"].ToString() + ";" + ((DataRowView)e.Item.DataItem)["VideoPath"].ToString();

                if (videoCheck.Visible)
                {
                    hfMainVideoSrc.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
                }
            }
        }

        /// <summary>
        /// Save new or edit video file details
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/11/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveVideo_Click(object sender, EventArgs e)
        {
            SaveVideoEventArgs args = new SaveVideoEventArgs();
            args.Id = !Convert.ToBoolean(hfAddNewVideo.Value) ? Convert.ToInt32(hfVideoId.Value) : (int?)null;
            args.Title = txtPopupVideoTitle.Text.Trim();
            args.IsMainVideo = chkMainVideo.Checked;
            args.VideoFile = Convert.ToBoolean(hfAddNewVideo.Value) ? fuVideo : null;

            this.BaseParentControl.OnSaveVideo(sender, args);

            LoadVideoList();
        }

        /// <summary>
        /// Remove video file from object video file list
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/11/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repVideoList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DeleteVideoEventArgs args = new DeleteVideoEventArgs();
            args.Id = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[0]);
            args.VideoName = e.CommandArgument.ToString().Split(';')[1];

            this.BaseParentControl.OnDeleteVideo(source, args);

            LoadVideoList();
        }
    }
}
