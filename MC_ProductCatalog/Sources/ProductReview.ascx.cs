using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using Melon.Components.ProductCatalog.Configuration;
using System.Web.UI.WebControls;
using AjaxControlToolkit;


namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays all product reviews entered by users in the front-end
    /// </summary>
    public partial class CodeBehind_ProductReview : ProductCatalogControl
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
                    return "DESC";
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
                    return "DatePosted";
                }
            }
            set
            {
                ViewState["__mc_pc_sortExpression"] = value;
            }
        }

        /// <summary>
        /// Identifier of loaded product review object
        /// </summary>
        public int? ProductReviewId
        {
            get 
            {
                if (ViewState["ProductReviewId"] == null)
                {
                    return null;
                }

                return (int?)ViewState["ProductReviewId"];
            }

            set { ViewState["ProductReviewId"] = value; }
        }

        #endregion

        /// <summary>
        /// Attach event handlers for controls' events.
        /// </summary>
        /// <param name="e"></param> 
        protected override void OnInit(EventArgs e)
        {
            gvProductReview.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(gvProductReview_RowCommand);
            gvProductReview.RowDataBound += new GridViewRowEventHandler(gvProductReview_RowDataBound);
            gvProductReview.Sorting += new GridViewSortEventHandler(gvProductReview_Sorting);
            TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(Pager_PageChanged);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnCloseProductReview.Click+=new EventHandler(btnCloseProductReview_Click);
            btnSaveReview.Click += new EventHandler(btnSaveReview_Click);

            base.OnInit(e);
        }
        
        /// <summary>
        /// Initializes user control information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                ibtnOpenCalendarAddedFrom.ImageUrl = Utilities.GetImageUrl(this.Page, "calendar.gif");
                ibtnOpenCalendarAddedTo.ImageUrl = Utilities.GetImageUrl(this.Page, "calendar.gif");                

                TopPager.Visible = false;

                ProductSearchCriteria searchCriteria = CollectSearchCriteria();
                ListProductReviews(searchCriteria);
            }
        }

        /// <summary>
        /// Event handler for event Click of Button btnSearch.
        /// </summary>
        /// <remarks>
        ///     The methods calls method <see cref="CollectSearchCriteria"/> to get the search criteria
        ///     and then method <see cref="ListUsers"/> is called to 
        ///     search for product rebiews corresponding to the search critera and display them in GridView gvProductReview.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/07/2010</date>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ProductSearchCriteria searchCriteria = CollectSearchCriteria();
            ListProductReviews(searchCriteria);
        }

        /// <summary>
        /// Load product reviews corresponding to the selected search criteria
        /// </summary>
        /// <remarks>
        ///     The method get search criteria from properties <see cref="searchCriteria"/> and passed to 
        ///     static method Search of class <see cref="Melon.Components.ProductCatalog.ProductReview"/>.
        ///     After that GridView gvUsers is databound with the found users.
        /// </remarks>        
        /// <author>Melon Team</author>
        /// <date>01/11/2010</date>
        private void ListProductReviews(ProductSearchCriteria searchCriteria)
        {
            DataTable dtProductReview = ProductReview.Search(searchCriteria);
            
            //Display details of found product reviews
            DataView dvProductReview = new DataView(dtProductReview);
            if (dtProductReview.Rows.Count != 0)
            {
                dvProductReview.Sort = this.SortExpression + " " + this.SortDirection;
            }

            gvProductReview.PageSize = ProductCatalogSettings.TablePageSize;
            gvProductReview.DataSource = dvProductReview;
            gvProductReview.DataBind();

            //Display paging if there are users found.
            if (dtProductReview.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvProductReview.PageCount, gvProductReview.PageIndex + 1, 5, gvProductReview.PageSize, dtProductReview.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;                
            }            
        }        

        /// <summary>
        /// Returns the currently entered search criteria to filter for product reviews
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>01/11/2010</date>
        /// <returns></returns>
        private ProductSearchCriteria CollectSearchCriteria()
        {
            ProductSearchCriteria searchCriteria = new ProductSearchCriteria();           

            if (txtKeywords.Text.Trim() != String.Empty)
            {
                searchCriteria.keywords = txtKeywords.Text.Trim();
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

            if (cbxlSearchCriteria.Items.FindByValue("Review").Selected)
            {
                fields.Add(ProductSearchFields.Review);
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
            
            return searchCriteria;
        }

        /// <summary>
        /// Fire events for selected object on selected row
        /// </summary>
        /// <remarks>        
        /// X - deletes product review
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>01/11/2010</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductReview_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                ProductReview.Delete(Convert.ToInt32(e.CommandArgument));
                btnSearch_Click(sender, e);
            }
            else if (e.CommandName == "Navigate")
            {
                lblNoProductReview.Visible = false;             
                ProductReview pr = new ProductReview();
                ProductReviewId=Convert.ToInt32(e.CommandArgument.ToString());
                pr.Id = ProductReviewId;
                DataTable dtProductReview = ProductReview.List(pr);

                if (dtProductReview.Rows.Count == 0)
                {
                    lblNoProductReview.Visible = true;
                    return;
                }

                DataRow rowProductReview = dtProductReview.Rows[0];
                lblProductCode.Text = rowProductReview["Code"].ToString();
                lblProductName.Text = rowProductReview["Name"].ToString();
                lblUserName.Text = rowProductReview["PostedBy"].ToString();
                ddlRating.SelectedIndex = Convert.ToInt16(rowProductReview["Rating"].ToString()) - 1;
                txtReview.Text = rowProductReview["Text"].ToString(); 

                MPEProductReview.Show();
            }
        }

        /// <summary>
        /// Event handler for event RowDataBound of GridView gvProductReview.
        /// </summary>
        /// <remarks>
        ///     Used to set product review details in the columns of GridView gvProductReview.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>19/02/2008</date>
        protected void gvProductReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblReview = (Label)e.Row.FindControl("lblReview");
                lblReview.Text = Melon.General.StringUtils.Cut(((DataRowView)e.Row.DataItem)["Text"].ToString(), " ", 50, 10);

                Image imgRating = (Image)e.Row.FindControl("imgRating");
                imgRating.ImageUrl=Utilities.GetImageUrl(this.Page, "rating_"+((DataRowView)e.Row.DataItem)["Rating"].ToString()+".gif");
            }
        }

        /// <summary>
        /// Event handler for event Sorting of GridView gvProductReview.
        /// </summary>
        /// <remarks>
        ///     Save in view state the new sorting direction and expression 
        ///     and then calls method <see cref="ListProductReviews"/> to perform the sorting.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/11/2010</date>
        protected void gvProductReview_Sorting(object sender, GridViewSortEventArgs e)
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
                ListProductReviews(searchCriteria);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Event handler for event PageChange for user control Pager.ascx.
        /// </summary>
        /// <remarks>
        ///     Set property PageIndex of GridView gvProductReview to the new page number and then 
        ///     calls method <see cref="ListProductReview"/> to perform the paging.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>01/11/2010</date>
        protected void Pager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                gvProductReview.PageIndex = e.NewPage;

                ProductSearchCriteria searchCriteria = CollectSearchCriteria();
                ListProductReviews(searchCriteria);
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Close popup control where product review details are opened in edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCloseProductReview_Click(object sender, EventArgs e)
        {
            ProductSearchCriteria searchCriteria = CollectSearchCriteria();
            ListProductReviews(searchCriteria);
        }

        /// <summary>
        /// Event handler for event Click of button btnSaveReview
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="Load"/> method of class <see cref="Melon.Components.ProductCatalog.ProductReview"/>.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveReview_Click(object sender, EventArgs e)
        {
            if (ProductReviewId != null)
            {
                ProductReview review = ProductReview.Load(ProductReviewId.Value);
                if (review == null)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = Convert.ToString(GetLocalResourceObject("ProductReviewSaveNotExistErrorMessage"));
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }
                review.Rating = Convert.ToInt16(ddlRating.SelectedValue);
                review.Text = txtReview.Text;
                review.Save();

                ProductSearchCriteria searchCriteria = CollectSearchCriteria();
                ListProductReviews(searchCriteria);
            }
        }
    }    
}
