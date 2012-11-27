using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Front-end web page provides functionality to search through object information with keyword.
    /// </summary>
    /// <remarks>
    /// This user control allows setting keyord to search for component objects in the Front End of the Product Catalog.
    /// After all search attributes are set, <see cref="OnSimpleSearch"/> event is called. 
    /// All methods attached to event`s delegate are fired in order to filter object listing in Front End object listing controls.
    /// </remarks>
    public partial class CodeBehind_FESimpleSearch : ProductCatalogControl
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
         
            }
        }

        /// <summary>
        /// Fires <see cref="OnSimpleSearch"/> event that is attached to other object search methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string url = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf("/") + 1);
            if (url.Contains("Products.aspx") || url.Contains("ObjectDetails.aspx"))
            {
                Response.Redirect("ObjectList.aspx?keyword=" + txtSimpleSearchKeywords.Text.Trim());
            }

            // if ProductGrid or ProductList is active object type passed in the query string, then get current object type.
            // Otherwise if Bundle, Collection or Catalog are selected, then set ProductList as current object type
            this.MasterPageControl.SelectedTab = (!String.IsNullOrEmpty(Request.QueryString["objType"]) && Request.QueryString["objType"].Contains("Product")) ? Request.QueryString["objType"] : "ProductList";

            BaseMasterPageControl.SimpleSearchEventArgs asEventArgs = new BaseMasterPageControl.SimpleSearchEventArgs();
            asEventArgs.Keywords = txtSimpleSearchKeywords.Text.Trim();            
            this.MasterPageControl.OnSimpleSearch(sender, asEventArgs);
        }
    }
}
