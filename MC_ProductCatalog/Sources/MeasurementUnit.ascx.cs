using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.Exception;
using System.Web.UI.WebControls;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays all measurement unit in the system
    /// </summary>
    public partial class CodeBehind_MeasurementUnit : ProductCatalogControl
    {        

        #region Fields & Properties

        /// <summary>
        /// Sort direction of the currently sorted column in the GridView with users gvMeasurementUnit.
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
        /// Sort expression of the currently sorted column in the GridView with users gvMeasurementUnit.
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
                    return "Name";
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
        protected override void OnInit(EventArgs e)
        {
            gvMeasurementUnit.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(gvMeasurementUnit_RowCommand);
            gvMeasurementUnit.Sorting += new System.Web.UI.WebControls.GridViewSortEventHandler(gvMeasurementUnit_Sorting);
            gvMeasurementUnit.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(gvMeasurementUnit_RowDataBound);
            TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);

            btnAddMeasurementUnit.Click += new EventHandler(btnSaveMeasurementUnit_Click);
            btnEditMeasurementUnit.Click += new EventHandler(btnSaveMeasurementUnit_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Initializes user control information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                ListMeasurementUnits();
            }
        }

        /// <summary>
        /// Lists all measurement units defined in the system.
        /// </summary>        
        private void ListMeasurementUnits()
        {
            DataTable dtMeasurementUnits = MeasurementUnit.List(new MeasurementUnit());

            //Display details of measurement units
            DataView dvMeasurementUnits = new DataView(dtMeasurementUnits);
            if (dtMeasurementUnits.Rows.Count != 0)
            {
                dvMeasurementUnits.Sort = this.SortExpression + " " + this.SortDirection;
            }

            gvMeasurementUnit.PageSize = ProductCatalogSettings.TablePageSize;
            gvMeasurementUnit.DataSource = dvMeasurementUnits;
            gvMeasurementUnit.DataBind();

            //Display paging if there are users found.
            if (dtMeasurementUnits.Rows.Count != 0)
            {
                TopPager.Visible = true;
                TopPager.FillPaging(gvMeasurementUnit.PageCount, gvMeasurementUnit.PageIndex + 1, 5, gvMeasurementUnit.PageSize, dtMeasurementUnits.Rows.Count);
            }
            else
            {
                TopPager.Visible = false;
            }
        }

        /// <summary>
        /// Performs page changing for measurement unit listing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
        {
            if (this.ParentControl.CurrentUser != null)
            {
                gvMeasurementUnit.PageIndex = e.NewPage;                
                ListMeasurementUnits();
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Performs sorting on measurement unit list.
        /// </summary>
        /// <remarks>Table is sorted by applying <see cref="SortExpression"/> parameter.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMeasurementUnit_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
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
                
                ListMeasurementUnits();
            }
            else
            {
                this.ParentControl.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// Removes measurement unit from system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMeasurementUnit_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                try
                {
                    MeasurementUnit.Delete(Convert.ToInt32(e.CommandArgument));
                }
                catch (ProductCatalogException ex)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = ex.Message;
                    this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                ListMeasurementUnits();
            }            
        }

        /// <summary>
        /// Adds javascript method which opens measurement unit details in edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMeasurementUnit_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv=(DataRowView)e.Row.DataItem;
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                btnEdit.OnClientClick = "javascript:centerMeasurementUnitPopup();loadMeasurementUnitPopup('" + drv["Id"] + "','" +drv["Name"]+"','"+drv["Description"]+"'); return false;";                    
            }
        }

        /// <summary>
        /// Save new or edit existing measurement unit
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>01/15/2010</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveMeasurementUnit_Click(object sender, EventArgs e)
        {            
            bool isNew = hfMeasurementUnitId.Value == "";
            MeasurementUnit mu = new MeasurementUnit();
            if (!isNew)
            {
                mu.Id = Convert.ToInt32(hfMeasurementUnitId.Value);
                mu.Load();
            }

            mu.Name = txtPopupMeasurementUnitName.Text;
            mu.Description = txtPopupMeasurementUnitDescription.Text;

            try
            {
                MeasurementUnit.Save(mu);
            }
            catch (ProductCatalogException ex)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                switch (ex.Code)
                {
                    case ProductCatalogExceptionCode.MeasurementUnitDuplicateNameException:
                        args.ErrorMessage = String.Format(ex.Message, ex.AdditionalInfo);
                        break;                    
                    default:
                        args.ErrorMessage = ex.Message;
                        break;

                }

                this.ParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            ListMeasurementUnits();
        }
    }
}
