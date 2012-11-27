using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Front-end web page provides functionality to search through object information with search filter options.
    /// </summary>
    /// <remarks>
    /// This user control allows setting search filter attributes to search for component objects in the Front End of the Product Catalog.
    /// After all search attributes are set, <see cref="OnAdvancedSearch"/> event is called. 
    /// All methods attached to event`s delegate are fired in order to filter object listing in Front End object listing controls.
    /// </remarks>
    public partial class CodeBehind_FEAdvancedSearch : ProductCatalogControl
    {
        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        protected override void OnInit(EventArgs e)
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            
            base.OnInit(e);
        }

        /// <summary>
        /// Initializes user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                LoadCategoryList();
            }
        }

        /// <summary>
        /// Load category hierarchical list with categories from database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>02/02/2010</date>
        private void LoadCategoryList()
        {            
            DataTable dtCategoryList = Category.GetHierarchicalList(true);
            if (dtCategoryList.Rows.Count > 0)
            {
                String str = "---- ";
                foreach (DataRow row in dtCategoryList.Rows)
                {
                    String s = "";
                    for (int i = 0; i < Convert.ToInt32(row["CatLevel"]); i++)
                    {
                        s += str;
                    }

                    ListItem item = new ListItem(s.Replace(" ", HttpUtility.HtmlDecode("&nbsp;")) + row["Name"].ToString(), row["Id"].ToString() + ";" + row["CategoryFullName"].ToString());
                    item.Attributes.Add("visible", "false");
                    lbCategoryList.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Collects search criteria and fires <see cref="OnAdvancedSearch"/> event that is attached to other object search methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.MasterPageControl.SelectedTab = "AdvancedSearch";
            foreach (ListItem item in cbxObjectType.Items)
            {
                if(item.Selected)
                {
                    this.MasterPageControl.SelectedTab += item.Value + ",";   
                }
            }

            if (this.MasterPageControl.SelectedTab.Length > 0)
            {
                this.MasterPageControl.SelectedTab = this.MasterPageControl.SelectedTab.Substring(0,this.MasterPageControl.SelectedTab.Length - 1);
            }

            BaseMasterPageControl.AdvancedSearchEventArgs asEventArgs = new BaseMasterPageControl.AdvancedSearchEventArgs();
            asEventArgs.Keywords = txtAdvancedSearchKeywords.Text.Trim();
            asEventArgs.CategoryIdList=hfAdvancedSearchCategoryList.Value;
            asEventArgs.PriceFrom=txtPriceFrom.Text!=String.Empty ? Convert.ToDouble(txtPriceFrom.Text.Trim()) : (double?)null;
            asEventArgs.PriceTo=txtPriceTo.Text!=String.Empty ? Convert.ToDouble(txtPriceTo.Text.Trim()): (double?)null;
            if (ddlInStock.SelectedValue != "")
            {
                asEventArgs.IsInStock = ddlInStock.SelectedValue == "0";
            }
            this.MasterPageControl.OnAdvancedSearch(sender, asEventArgs);
        }                
    }
}
