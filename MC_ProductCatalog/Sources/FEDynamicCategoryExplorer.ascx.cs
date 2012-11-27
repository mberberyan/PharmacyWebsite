using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Configuration;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Front-end web page provides functionality to search through object information by selecting category from category menu.
    /// </summary>
    /// <remarks>
    /// This user control allows selecting category from category menu to search for component objects in the Front End of the Product Catalog.
    /// After selecting category item, <see cref="OnDynamicCategoryExplorer"/> event is called. 
    /// All methods attached to event`s delegate are fired in order to filter object listing in Front End object listing controls.
    /// </remarks>
    public partial class CodeBehind_FEDynamicCategoryExplorer : ProductCatalogControl
    {
        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        protected override void OnInit(EventArgs e)
        {         
            mCategoryMenu.MenuItemDataBound += new MenuEventHandler(mCategoryMenu_MenuItemDataBound);
            mCategoryMenu.MenuItemClick += new MenuEventHandler(mCategoryMenu_MenuItemClick);

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategoryExplorer();
            }
        }

        /// <summary>
        /// Load category menu with categories from database.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="Melon.Components.ProductCatalog.Category.ListCategoryExplorerNodes"/> method
        /// to extract xml-formatted category hierarchical information. Xml data is passed as data source to Menu control
        /// where categories hierarchy is displayed.
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>02/02/2010</date>
        private void LoadCategoryExplorer()
        {
            XmlDataSource xml = new XmlDataSource();
            xml.EnableCaching = false;
            xml.Data = Category.ListCategoryExplorerNodes(null, true).DocumentElement.OuterXml;
            xml.XPath = "root/cat";

            mCategoryMenu.DataSource = xml;
            mCategoryMenu.MaximumDynamicDisplayLevels = ProductCatalogSettings.CategoryLevel;
            mCategoryMenu.DataBind();            
        }

        /// <summary>
        /// Load Category Menu Item information
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>02/15/2010</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void mCategoryMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            XmlNode node = (XmlNode)e.Item.DataItem;
            
            e.Item.Text = "<span>"+node.Attributes["Name"].Value+"</span>";
            e.Item.Value = node.Attributes["Id"].Value;
            
        }

        /// <summary>
        /// Selects category to filter product list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method raises event <see cref="BaseMasterPageControl.OnFEDynamicCategoryExplorer"/>, which is attached
        /// to all methods that list product information in the Front-end panel.
        /// </para>
        /// <para>
        /// If this control is located in the component`s landing page, then user is redirected to product listing page
        /// and query string parameter is passed to denote selected category item.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void mCategoryMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            string url=Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf("/") + 1);
            if (url.Contains("Products.aspx") || url.Contains("ObjectDetails.aspx"))
            {
                Response.Redirect("ObjectList.aspx?catId=" + e.Item.Value);
            }

            // if ProductGrid or ProductList is active object type passed in the query string, then get current object type.
            // Otherwise if Bundle, Collection or Catalog are selected, then set ProductList as current object type
            this.MasterPageControl.SelectedTab = (!String.IsNullOrEmpty(Request.QueryString["objType"]) && Request.QueryString["objType"].Contains("Product")) ? Request.QueryString["objType"] : "ProductList";

            BaseMasterPageControl.FEDynamicCategoryExplorerEventArgs dceEventArgs = new BaseMasterPageControl.FEDynamicCategoryExplorerEventArgs();
            dceEventArgs.SelectedCategoryId = Convert.ToInt32(e.Item.Value);
            this.MasterPageControl.OnFEDynamicCategoryExplorer(sender, dceEventArgs);
        }
    }
}
