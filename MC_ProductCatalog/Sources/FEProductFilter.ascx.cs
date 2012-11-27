using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Enumerations;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog;

/// <summary>
/// Web page displays product filter options.
/// </summary>
public partial class FEProductFilter : ProductCatalogControl
{
    #region Fields && Properties
    /// <summary>
    /// Page size to show how many products are listed in Product List/Grid
    /// </summary>        
    /// <remarks>
    /// This property shows how many products should be listed on Listing page.
    /// It can be of type <see cref="Melon.Components.ProductCatalog.Enumerations.PageItemNumberEnum"/>.
    /// </remarks>
    public PageItemNumberEnum ProductsPageSize
    {
        get
        {
            if (ViewState["mc_pc_pageSize"] == null)
            {
                return PageItemNumberEnum.Six;
            }
            return (PageItemNumberEnum)ViewState["mc_pc_pageSize"];
        }
        set { ViewState["mc_pc_pageSize"] = value; }
    }

    /// <summary>
    /// Column to sort products in Product List/Grid
    /// </summary>        
    public SortOrderEnum SortedBy
    {
        get
        {
            if (ViewState["mc_pc_sortedBy"] == null)
            {
                return SortOrderEnum.Name;
            }
            return (SortOrderEnum)ViewState["mc_pc_sortedBy"];
        }
        set { ViewState["mc_pc_sortedBy"] = value; }
    }

    /// <summary>
    /// COlumn to sort products in Product List/Grid
    /// </summary>        
    public ObjectDisplayEnum DisplayType
    {
        get
        {
            if (ViewState["mc_pc_displayType"] == null)
            {
                return ObjectDisplayEnum.List;
            }
            return (ObjectDisplayEnum)ViewState["mc_pc_displayType"];
        }
        set { ViewState["mc_pc_displayType"] = value; }
    }
    #endregion

    /// <summary>
    /// Initializes user control information
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadControls();        
        }
    }    

    /// <summary>
    /// Initializes product filter dropdownlist controls.
    /// </summary>
    /// <remarks>
    /// Product filter controls are used to sort and filter products when listing them in Front End listing user controls.
    /// There are three filter options which can affect the way products are displayed:
    /// <list type="bulltet">
    /// <item>Page size - property of type <see cref="melon.Componentes.ProductCatalog.Enumerations.PageItemNumberEnum"/> which shows how many products should be visible on one page.</item>
    /// <item>Display type - property of type <see cref="melon.Componentes.ProductCatalog.Enumerations.ObjectDisplayEnum"/> which shows whether products should be listed as a List or Grid.</item>
    /// <item>Sorting - property of type <see cref="melon.Componentes.ProductCatalog.Enumerations.SortOrderEnum"/> which denotes how product results are sorted when displayed in Listing controls.</item>
    /// </list>
    /// </remarks>
    private void LoadControls()
    {
        foreach (int idx in Enum.GetValues(typeof(PageItemNumberEnum)))
        { 
            if(idx>=(int)PageItemNumberEnum.Six)
            ddlSize.Items.Add(new ListItem(idx.ToString(),idx.ToString()));
        }
        ddlSize.Items.FindByValue(Convert.ToString((int)ProductsPageSize)).Selected = true;

        foreach (string str in Enum.GetNames(typeof(ObjectDisplayEnum)))
        {
            ddlDisplay.Items.Add(new ListItem(str, "Product"+str));
        }
        if(!String.IsNullOrEmpty(Request.QueryString["objType"]) && Convert.ToString(Request.QueryString["objType"]).Contains("Product"))
        {
            ddlDisplay.Items.FindByValue(Request.QueryString["objType"]).Selected = true;
        }

        foreach (string str in Enum.GetNames(typeof(SortOrderEnum)))
        {
            ddlSort.Items.Add(new ListItem(str, str));
        }
        ddlSort.Items.FindByValue(SortedBy.ToString()).Selected = true;
    }

    /// <summary>
    /// Redirects to product listing control which type depends on "objType" query string parameter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDisplay_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("ObjectList.aspx?objType=" + ddlDisplay.SelectedValue);
    }

    /// <summary>
    /// Calls <see cref="BaseMasterPageControl.OnProductSizeChange"/> method which is attached to product listing controls in order to denote how many products should be listed in product result table.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseMasterPageControl.ProductSizeChangeEventArgs asEventArgs = new BaseMasterPageControl.ProductSizeChangeEventArgs();
        asEventArgs.PageSize = (PageItemNumberEnum)Enum.Parse(typeof(PageItemNumberEnum),ddlSize.SelectedValue);
        this.MasterPageControl.OnProductSizeChange(sender, asEventArgs);
    }

    /// <summary>
    /// Calls <see cref="BaseMasterPageControl.OnProductSizeChange"/> method which is attached to product listing controls in order to sort product result listing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseMasterPageControl.ProductColumnSortEventArgs asEventArgs = new BaseMasterPageControl.ProductColumnSortEventArgs();
        asEventArgs.ColumnName = (SortOrderEnum)Enum.Parse(typeof(SortOrderEnum), ddlSort.SelectedValue);
        this.MasterPageControl.OnProductColumnSort(sender, asEventArgs);
    }
}
