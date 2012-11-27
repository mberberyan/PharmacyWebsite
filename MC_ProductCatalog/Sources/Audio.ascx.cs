using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.UI.CodeBehind;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page where component object audio files are manipulated.
    /// </summary>
    /// <remarks>
    ///  This page allows adding and deleting audio files in the system, as well as editing their titles 
    ///  and selecting which audio file to be selected as main one for the object 
    /// </remarks>
    public partial class CodeBehind_Audio : ProductCatalogControl
    {                
        protected override void OnInit(EventArgs e)
        {
            btnAddAudio.Click += new EventHandler(btnSaveAudio_Click);
            btnEditAudio.Click += new EventHandler(btnSaveAudio_Click);
            repAudioList.ItemCommand += new RepeaterCommandEventHandler(repAudioList_ItemCommand);
            repAudioList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repAudioList_ItemDataBound);
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
                LoadAudioList();
            }
        }

        /// <summary>
        /// Load audio files in repeater control
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>10/11/2009</date>
        private void LoadAudioList()
        {
            divAudioLimit.InnerHtml = GetLocalResourceObject("AllowedFileSize").ToString() + " " + Utilities.GetAudioAllowedExtStr() + "<br/>" + GetLocalResourceObject("MaxFileSize").ToString() + " " + ProductCatalogSettings.AudioMaxSize + " KB";
            DataTable audioTab = new DataTable();

            switch (SelectedObjectType)
            {                
                case ComponentObjectEnum.Product:
                    if (SelectedProductId != null)
                    {
                        ProductAudio productAudio = new ProductAudio();
                        productAudio.ProductId = SelectedProductId;
                        audioTab = productAudio.List();
                    }
                    break;
                case ComponentObjectEnum.Bundle:
                    if (SelectedObjectId != null)
                    {
                        BundleAudio bundleAudio = new BundleAudio();
                        bundleAudio.BundleId = SelectedObjectId;
                        audioTab = bundleAudio.List();
                    }
                    break;
                case ComponentObjectEnum.Collection:
                    if (SelectedObjectId != null)
                    {
                        CollectionAudio collAudio = new CollectionAudio();
                        collAudio.CollectionId = SelectedObjectId;
                        audioTab = collAudio.List();
                    }
                    break;
            }


            if (audioTab.Rows.Count > 0)
            {
                repAudioList.DataSource = audioTab;
                repAudioList.DataBind();
                repAudioList.Visible = true;
                lblNoAudio.Visible = false;
            }
            else
            {
                repAudioList.Visible = false;
                lblNoAudio.Visible = true;
            }
        }

        /// <summary>
        /// Sets audio file parameters in the audio list
        /// </summary>
        /// <remarks>
        /// Audio physical path, title name and main audio flag are set in this repeater data-binding method:        
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repAudioList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlImage audioItem = (HtmlImage)e.Item.FindControl("audioItem");
                HtmlImage audioCheck = (HtmlImage)e.Item.FindControl("audioCheck");
                Label lblAudioName = (Label)e.Item.FindControl("lblAudioName");
                Button btnRemoveAudio = (Button)e.Item.FindControl("btnRemoveAudio");

                audioItem.Src = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.AudioImage);
                audioItem.Attributes.Add("name", ((DataRowView)e.Item.DataItem)["Id"].ToString() + ";" + ((DataRowView)e.Item.DataItem)["AudioPath"].ToString());
                audioItem.Alt = ((DataRowView)e.Item.DataItem)["Title"].ToString();
                audioCheck.Visible = Convert.ToBoolean(((DataRowView)e.Item.DataItem)["IsMain"]);
                lblAudioName.Text = Melon.General.StringUtils.Cut(((DataRowView)e.Item.DataItem)["Title"].ToString(), " ", 15, 5);
                lblAudioName.ToolTip = ((DataRowView)e.Item.DataItem)["Title"].ToString();
                btnRemoveAudio.CommandArgument = ((DataRowView)e.Item.DataItem)["Id"].ToString() + ";" + ((DataRowView)e.Item.DataItem)["AudioPath"].ToString();

                if (audioCheck.Visible)
                {
                    hfMainAudioSrc.Value = ((DataRowView)e.Item.DataItem)["Id"].ToString();
                }
            }
        }

        /// <summary>
        /// Save new or edit audio file details
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/11/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveAudio_Click(object sender, EventArgs e)
        {
            SaveAudioEventArgs args = new SaveAudioEventArgs();
            args.Id = !Convert.ToBoolean(hfAddNewAudio.Value) ? Convert.ToInt32(hfAudioId.Value) : (int?)null;
            args.Title = txtPopupAudioTitle.Text.Trim();
            args.IsMainAudio = chkMainAudio.Checked;
            args.AudioFile = Convert.ToBoolean(hfAddNewAudio.Value) ? fuAudio : null;

            this.BaseParentControl.OnSaveAudio(sender, args);

            LoadAudioList();
        }

        /// <summary>
        /// Remove audio file from object audio file list
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>11/11/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repAudioList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            DeleteAudioEventArgs args = new DeleteAudioEventArgs();
            args.Id = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[0]);
            args.AudioName = e.CommandArgument.ToString().Split(';')[1];

            this.BaseParentControl.OnDeleteAudio(source, args);

            LoadAudioList();
        }
    }
}

