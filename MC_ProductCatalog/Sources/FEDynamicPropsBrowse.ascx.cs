using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Front-end web page provides functionality to search through object information by setting dynamic property values from DropDownList controls 
    /// with preset dynamic property values.
    /// </summary>
    /// <remarks>
    /// This user control allows setting dynamic property values to search for products in the Front End of the Product Catalog.
    /// After setting desired dynamic property values, <see cref="OnDynamicPropsSearch"/> event is called. 
    /// All methods attached to event`s delegate are fired in order to filter products in Front End product listing controls.
    /// </remarks>
    public partial class CodeBehind_FEDynamicPropsBrowse : ProductCatalogControl
    {
        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        protected override void OnInit(EventArgs e)
        {
            gvBrowseByFeature.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvBrowseByFeature_RowDataBound);
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
                LoadDynamicProperties();
            }
        }

        /// <summary>
        /// Load dynamic property values for all dynamic properties in the database.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="Melon.Components.ProductCatalog.PropertyDefinition.List(PropertyDefinition propDef)"/> method
        /// to table with values.        
        /// </remarks>
        /// <author>Melon Team</author>        
        private void LoadDynamicProperties()
        {
            DataTable dtPropDef = PropertyDefinition.List(new PropertyDefinition());
            gvBrowseByFeature.DataSource = dtPropDef;
            gvBrowseByFeature.DataBind();
        }

        /// <summary>
        /// Load values in DropDownList control for all dynamic properties in the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBrowseByFeature_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlValues = (DropDownList)e.Row.FindControl("ddlValues");
                DataTable dtPropValues = PropertyValue.ListByPropDefId(Convert.ToInt32(((DataRowView)e.Row.DataItem)["Id"]));

                if (dtPropValues.Rows.Count == 0)
                {
                    e.Row.Visible = false;
                    return;
                }

                if (dtPropValues.Rows.Count > 0)
                {
                    foreach (DataRow row in dtPropValues.Rows)
                    {                        
                        ddlValues.Items.Add(new ListItem(row["Value"].ToString()));
                    }

                    ddlValues.Items.Insert(0, new ListItem(GetLocalResourceObject("SelectFeature").ToString(), ""));
                }
            }
        }

        /// <summary>
        /// Selects dynamic property values in order to search products.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method raises event <see cref="BaseMasterPageControl.OnDynamicPropsSearch"/>, which is attached
        /// to all methods that list product information in the Front-end panel.
        /// </para>
        /// <para>
        /// If this control is located in the component`s landing page, then user is redirected to product listing page
        /// and query string parameter is passed to denote selected property values.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<string> valuesList = new List<string>();
            foreach (GridViewRow row in gvBrowseByFeature.Rows)
            {
                string value = ((DropDownList)row.FindControl("ddlValues")).SelectedValue;

                if (value != String.Empty)
                {
                    valuesList.Add(value);
                }
            }

            string url = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf("/") + 1);
            if (url.Contains("Products.aspx") || url.Contains("ObjectDetails.aspx"))
            {
                Response.Redirect("ObjectList.aspx?valueList=" + string.Join(",", valuesList.ToArray()));
            }

            // if ProductGrid or ProductList is active object type passed in the query string, then get current object type.
            // Otherwise if Bundle, Collection or Catalog are selected, then set ProductList as current object type
            this.MasterPageControl.SelectedTab = (!String.IsNullOrEmpty(Request.QueryString["objType"]) && Request.QueryString["objType"].Contains("Product")) ? Request.QueryString["objType"] : "ProductList";

            BaseMasterPageControl.DynamicPropsSearchEventArgs dynPropEventArgs = new BaseMasterPageControl.DynamicPropsSearchEventArgs();            
            dynPropEventArgs.DynamicPropValue = valuesList;
            this.MasterPageControl.OnDynamicPropsSearch(sender, dynPropEventArgs);
        }                
    }
}
