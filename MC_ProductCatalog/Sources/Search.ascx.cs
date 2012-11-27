using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using Melon.Components.ProductCatalog.Configuration;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides functionality to search through object information with search filter options.
    /// </summary>
    /// <remarks>
    /// This user control allows setting search filter attributes to search for component objects.
    /// After all search attributes are set, <see cref="ListObjectDetails"/> method is fired.     
    /// </remarks>
    public partial class CodeBehind_Search : ProductCatalogControl
    {
        #region Fields & Properties

        /// <summary>
        /// Sort direction of the currently sorted column in the GridView with users gvProductReview.
        /// It is "ASC" for ascending sorting and "DESC" for descending sorting. 
        /// </summary>
        public string SortDirection
        {
            get
            {
                if (ViewState["__mc_pc_sortDirection"] != null)
                {
                    return ViewState["__mc_pc_sortDirection"].ToString();
                }
                else
                {
                    return "ASC";
                }
            }
            set
            {
                ViewState["__mc_pc_sortDirection"] = value;
            }
        }

        /// <summary>
        /// Sort expression of the currently sorted column in the GridView with users gvProductReview.
        /// </summary>
        public string SortExpression
        {
            get
            {
                if (ViewState["__mc_pc_sortExpression"] != null)
                {
                    return ViewState["__mc_pc_sortExpression"].ToString();
                }
                else
                {
                    return "ObjectType";
                }
            }
            set
            {
                ViewState["__mc_pc_sortExpression"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>11/05/2009</date>
        protected override void OnInit(EventArgs e)
        {
            gvObjectDetailsList.RowCommand += new GridViewCommandEventHandler(gvObjectDetailsList_RowCommand);
            gvObjectDetailsList.Sorting += new GridViewSortEventHandler(gvObjectDetailsList_Sorting);
            this.TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(Pager_PageChanged);
            btnSearch.Click += new EventHandler(btnSearch_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialises user control information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                ibtnOpenCalendarAddedFrom.ImageUrl = Utilities.GetImageUrl(this.Page, "calendar.gif");
                ibtnOpenCalendarAddedTo.ImageUrl = Utilities.GetImageUrl(this.Page, "calendar.gif");

                LoadCategoryList();
                TopPager.Visible = false;
            }
        }

        /// <summary>
        /// Load category hierarchical listwith categories from database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>01/04/2010</date>
        private void LoadCategoryList()
        {
            DataTable dtCategoryList = Category.GetHierarchicalList((bool?)null);
            if (dtCategoryList.Rows.Count > 0)
            {
                String str = "---- ";
                foreach (DataRow row in dtCategoryList.Rows)
                {
                    String s = "--- ";
                    for (int i = 0; i < Convert.ToInt32(row["CatLevel"]); i++)
                    {
                        s += str;
                    }

                    ListItem item = new ListItem(s.Replace(" ", HttpUtility.HtmlDecode("&nbsp;")) + row["Name"].ToString(), row["Id"].ToString() + ";" + row["CategoryFullName"].ToString());
                    item.Attributes.Add("visible", "false");
                    ddlCategoryList.Items.Add(item);                    
                }

                ddlCategoryList.Items.Insert(0, new ListItem("All", ""));
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ProductSearchCriteria searchCriteria = CollectSearchCriteria();
            ListObjectDetails(searchCriteria);
        }

        /// <summary>
        /// Load objects for selected search criteria
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>01/04/2010</date>
        private void ListObjectDetails(ProductSearchCriteria searchCriteria)
        {
            DataTable dtObjectDetails = Utilities.ListObjectDetails(searchCriteria);
            DataView dvObjectDetails = new DataView(dtObjectDetails);
            if (dtObjectDetails.Rows.Count != 0)
            {
                dvObjectDetails.Sort = this.SortExpression + " " + this.SortDirection;
            }
            gvObjectDetailsList.DataSource = dvObjectDetails;
            gvObjectDetailsList.PageSize = ProductCatalogSettings.TablePageSize;
            gvObjectDetailsList.DataBind();

            //Display paging if there are users found.
            if (dtObjectDetails.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvObjectDetailsList.PageCount, gvObjectDetailsList.PageIndex + 1, 5, gvObjectDetailsList.PageSize, dtObjectDetails.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }
        }

        /// <summary>
        /// Returns the currently entered search criteria to filter for products
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>01/04/2010</date>
        /// <returns></returns>
        private ProductSearchCriteria CollectSearchCriteria()
        {
            ProductSearchCriteria searchCriteria = new ProductSearchCriteria();

            if (ddlCategoryList.SelectedValue != "")
            {
                searchCriteria.categoryId = Convert.ToInt32(ddlCategoryList.SelectedValue.Split(';')[0]);
            }

            searchCriteria.isCategoryRecursive = chkRecursiveCategory.Checked;

            if (txtKeywords.Text.Trim() != String.Empty)
            {
                searchCriteria.keywords = txtKeywords.Text.Trim();
            }

            List<ComponentObjectEnum> objectTypeList = new List<ComponentObjectEnum>();
            if (cbxlObjectType.Items.FindByValue("Category").Selected)
            {
                objectTypeList.Add(ComponentObjectEnum.Category);
            }
            
            if (cbxlObjectType.Items.FindByValue("Product").Selected)
            {
                objectTypeList.Add(ComponentObjectEnum.Product);
            }

            if (cbxlObjectType.Items.FindByValue("Catalog").Selected)
            {
                objectTypeList.Add(ComponentObjectEnum.Catalog);
            }

            if (cbxlObjectType.Items.FindByValue("Bundle").Selected)
            {
                objectTypeList.Add(ComponentObjectEnum.Bundle);
            }

            if (cbxlObjectType.Items.FindByValue("Collection").Selected)
            {
                objectTypeList.Add(ComponentObjectEnum.Collection);
            }

            if (cbxlObjectType.Items.FindByValue("Discount").Selected)
            {
                objectTypeList.Add(ComponentObjectEnum.Discount);
            }

            if (objectTypeList.Count > 0)
            {
                searchCriteria.objectTypes = objectTypeList;
            }

            List<ProductSearchFields> fields = new List<ProductSearchFields>();
            if (cbxlSearchCriteria.Items.FindByValue("Code").Selected)
            {
                fields.Add(ProductSearchFields.Code);
            }

            if (cbxlSearchCriteria.Items.FindByValue("Name").Selected)
            {
                fields.Add(ProductSearchFields.Name);
            }

            if (cbxlSearchCriteria.Items.FindByValue("Description").Selected)
            {
                fields.Add(ProductSearchFields.Description);
            }

            if (cbxlSearchCriteria.Items.FindByValue("Tags").Selected)
            {
                fields.Add(ProductSearchFields.Tags);
            }

            if (fields.Count > 0)
            {
                searchCriteria.keywordsPlaceholders = fields;
            }

            if (txtAddedFrom.Text.Trim() != String.Empty)
            {
                searchCriteria.StartDate = Convert.ToDateTime(txtAddedFrom.Text.Trim());
            }

            if (txtAddedTo.Text.Trim() != String.Empty)
            {
                searchCriteria.EndDate = Convert.ToDateTime(txtAddedTo.Text.Trim());
            }

            if (txtPriceFrom.Text.Trim() != String.Empty)
            {
                searchCriteria.PriceFrom = Convert.ToDouble(txtPriceFrom.Text.Trim());
            }

            if (txtPriceTo.Text.Trim() != String.Empty)
            {
                searchCriteria.PriceTo = Convert.ToDouble(txtPriceTo.Text.Trim());
            }

            if (ddlActive.SelectedValue != "")
            {
                searchCriteria.ActiveOnly = ddlActive.SelectedValue == "0";
            }

            if (ddlInStock.SelectedValue != "")
            {
                searchCriteria.InStockOnly = ddlInStock.SelectedValue == "0";
            }

            if (ddlFeatured.SelectedValue != "")
            {
                searchCriteria.FeaturedOnly = ddlFeatured.SelectedValue == "0";
            }

            return searchCriteria;
        }

        /// <summary>
        /// Fire events for selected object on selected row
        /// </summary>
        /// <remarks>
        /// N - opens product details page        
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>01/04/2010</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvObjectDetailsList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Navigate")
            {
                switch (e.CommandArgument.ToString().Split(';')[2])
                { 
                    case "Category":
                        //((CodeBehind_ProductCatalog)ParentControl).SetTabStyles(ComponentObjectEnum.Category);

                        LoadCategoryEventArgs args = new LoadCategoryEventArgs();
                        args.SelectedCategoryId = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[1]);
                        args.RefreshExplorer = true;
                        args.SelectedObjectType = ComponentObjectEnum.Category;
                        args.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadCategory(sender, args);
                        break;
                    case "Product":
                        //((CodeBehind_ProductCatalog)ParentControl).SetTabStyles(ComponentObjectEnum.Product);

                        LoadProductEventArgs productArgs = new LoadProductEventArgs();
                        productArgs.SelectedCategoryId = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[0]);
                        productArgs.SelectedProductId = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[1]);
                        productArgs.SelectedObjectType = ComponentObjectEnum.Product;
                        productArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadProduct(sender, productArgs);
                        break;
                    case "Catalog":
                        //((CodeBehind_ProductCatalog)ParentControl).SetTabStyles(ComponentObjectEnum.Catalog);

                        LoadCatalogEventArgs catArgs = new LoadCatalogEventArgs();
                        catArgs.SelectedCatalogId = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[1]);
                        catArgs.RefreshExplorer = true;
                        catArgs.SelectedObjectType = ComponentObjectEnum.Catalog;
                        catArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadCatalog(sender, catArgs);
                        break;
                    case "Bundle":
                        //((CodeBehind_ProductCatalog)ParentControl).SetTabStyles(ComponentObjectEnum.Bundle);

                        LoadBundleEventArgs argsBundle = new LoadBundleEventArgs();
                        argsBundle.SelectedBundleId = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[1]);
                        argsBundle.RefreshExplorer = true;
                        argsBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                        argsBundle.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadBundle(sender, argsBundle);
                        break;
                    case "Collection":
                        //((CodeBehind_ProductCatalog)ParentControl).SetTabStyles(ComponentObjectEnum.Collection);

                        LoadCollectionEventArgs collArgs = new LoadCollectionEventArgs();
                        collArgs.SelectedCollectionId = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[1]);
                        collArgs.RefreshExplorer = true;
                        collArgs.SelectedObjectType = ComponentObjectEnum.Collection;
                        collArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadCollection(sender, collArgs);
                        break;
                    case "Discount":
                        //((CodeBehind_ProductCatalog)ParentControl).SetTabStyles(ComponentObjectEnum.Discount);

                        LoadDiscountEventArgs discountArgs = new LoadDiscountEventArgs();
                        discountArgs.SelectedDiscountId = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[1]);
                        discountArgs.RefreshExplorer = true;
                        discountArgs.SelectedObjectType = ComponentObjectEnum.Discount;
                        discountArgs.SelectedTab = ProductCatalogTabs.GeneralInformation;
                        this.ParentControl.OnLoadDiscount(sender, discountArgs);
                        break;
                }                                
            }         
        }

        /// <summary>
        /// Event handler for event PageChange for user control Pager.ascx.
        /// </summary>
        /// <remarks>
        ///     Set property PageIndex of GridView gvUsers to the new page number and then 
        ///     calls method <see cref="ListUsers"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        protected void Pager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                gvObjectDetailsList.PageIndex = e.NewPage;
                ProductSearchCriteria criteria = CollectSearchCriteria();
                ListObjectDetails(criteria);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event Sorting of GridView gvObjectDetails.
        /// </summary>
        /// <remarks>
        ///     Save in view state the new sorting direction and expression 
        ///     and then calls method <see cref="ListObjectDetails"/> to perform the sorting.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>03/16/2010</date>
        protected void gvObjectDetailsList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                string newSortExpression = e.SortExpression;
                if (this.SortExpression == newSortExpression)
                {
                    //If the old sort expression is the same as the new sort expression, we invert the sort direction.
                    this.SortDirection = (this.SortDirection == "ASC") ? "DESC" : "ASC";
                }
                else
                {
                    //We sort by new column, so set the sorting direction to be acsending.
                    this.SortExpression = newSortExpression;
                    this.SortDirection = "ASC";
                }

                ProductSearchCriteria searchCriteria = CollectSearchCriteria();
                ListObjectDetails(searchCriteria);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }
    }
}
