using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays menu tabs for opening all object details pages
    /// </summary>
    public partial class CodeBehind_MenuTabs : ProductCatalogControl
    {        
        /// <summary>
        /// Initializes user control information
        /// </summary>
        /// <remarks>
        /// If new object is selected then all menu tabs but the first one should be disabled.
        /// For more information see <see cref="DisableMenuTabs()"/> method.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {                
                SetMenuTabsVisibility();

                // this clause checks whether to disable all tabs except 'General Information' into object controls
                // the tab will be enabled if the selected object is open for edit or 'Geneal Information' is already saved
                // in the database                
                if ((SelectedObjectType==ComponentObjectEnum.Category && SelectedObjectId == null)
                 || (SelectedObjectType == ComponentObjectEnum.Product && SelectedProductId == null)
                 || (SelectedObjectId==null || SelectedObjectId==0))
                {
                    DisableMenuTabs();
                }                
            }
        }

        /// <summary>
        /// Make all tabs except 'General Information' disabled
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/17/2009</date>
        public void DisableMenuTabs()
        {
            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Bundle:
                    DisableTab(tabProducts);
                    DisableTab(tabImages);
                    DisableTab(tabAudio);
                    DisableTab(tabVideo);
                    DisableTab(tabDynamicProperties);
                    break;
                case ComponentObjectEnum.Collection:
                    DisableTab(tabProducts);
                    DisableTab(tabImages);
                    DisableTab(tabAudio);
                    DisableTab(tabVideo);
                    break;
                case ComponentObjectEnum.Catalog:
                case ComponentObjectEnum.Discount:
                    DisableTab(tabProducts);
                    break;
                case ComponentObjectEnum.Category:
                    DisableTab(tabImages);
                    DisableTab(tabDynamicProperties);
                    break;                                
                case ComponentObjectEnum.Product:
                    DisableTab(tabImages);
                    DisableTab(tabAudio);
                    DisableTab(tabVideo);
                    DisableTab(tabDynamicProperties);
                    DisableTab(tabRelatedProducts);
                    DisableTab(tabStatistics);                                    
                    break;
            }
        }

        /// <summary>
        /// This method hides tabs that are not used for current object
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/14/2009</date>
        private void SetMenuTabsVisibility()
        {
            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Category:                                        
                    tabAudio.Visible = false;
                    tabProducts.Visible = false;
                    tabRelatedProducts.Visible = false;
                    tabStatistics.Visible = false;
                    tabVideo.Visible = false;                                        
                    break;
                case ComponentObjectEnum.Product:
                    tabProducts.Visible = false;
                    break;
                case ComponentObjectEnum.Bundle:
                    tabRelatedProducts.Visible = false;
                    tabStatistics.Visible = false;
                    break;
                case ComponentObjectEnum.Collection:
                    tabDynamicProperties.Visible = false;
                    tabRelatedProducts.Visible = false;
                    tabStatistics.Visible = false;
                    break;
                case ComponentObjectEnum.Catalog:
                case ComponentObjectEnum.Discount:
                    tabAudio.Visible = false;
                    tabDynamicProperties.Visible = false;
                    tabImages.Visible = false;
                    tabRelatedProducts.Visible = false;
                    tabStatistics.Visible = false;
                    tabVideo.Visible = false;
                    break;                                                                                            
                case ComponentObjectEnum.Export:                    
                case ComponentObjectEnum.MeasurementUnit:                    
                case ComponentObjectEnum.ProductReview:                    
                case ComponentObjectEnum.Search:                    
                case ComponentObjectEnum.Users:
                    ulMenuTabs.Visible = false;
                    break;                                       
            }
        }

        /// <summary>
        /// Disables menu tabs that are not relevant for current object
        /// </summary>
        /// <param name="tab"></param>
        private void DisableTab(HtmlAnchor tab)
        {
            tab.Disabled = true;

            //These attribute change is done in order to enable/disable the tabs in Firefox            
            tab.Attributes.Add("onclick_bak", tab.Attributes["onclick"]);
            tab.Attributes.Remove("onclick");
            tab.Style.Add("color", "gray");            
        }
    }
}
