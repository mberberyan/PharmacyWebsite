using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Enumerations;
using System.Data;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog;
using Melon.Components.ProductCatalog.Exception;
using Melon.Components.ProductCatalog.UI.CodeBehind;

public partial class MC_ProductCatalog_Example_LandingPage : System.Web.UI.Page
{
    #region Fields & Properties    
    /// <summary>
    /// Page size to show how many products are listed in Product List/Grid
    /// </summary>
    private PageItemNumberEnum _ProductsPageSize = PageItemNumberEnum.Twelve;
    public PageItemNumberEnum ProductsPageSize
    {
        get { return _ProductsPageSize; }
        set { _ProductsPageSize = value; }
    }    
    #endregion

    protected override void OnInit(EventArgs e)
    {
        dlProductGrid.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(dlProductGrid_ItemDataBound);        
        TopPager.PageChanged += new CodeBehind_Pager.PagerEventHandler(TopPager_PageChanged);        

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProductGrid(0);
        }
    }

    private void LoadProductGrid(int pageIndex)
    {
        ProductSearchCriteria searchCriteria = new ProductSearchCriteria();
        searchCriteria.ActiveOnly = true;
        searchCriteria.FeaturedOnly = true;        

        DataTable dtProductGrid = new DataTable();

        dlProductGrid.Visible = true;
        TopPager.Visible = true;
        divProductGridError.Visible = false;
        try
        {
            dtProductGrid = Product.Search(searchCriteria, true);
        }
        catch (ProductCatalogException e)
        {
            dlProductGrid.Visible = false;
            TopPager.Visible = false;
            divProductGridError.Visible = true;
            divProductGridError.InnerHtml = e.Message;
            return;
        }

        PagedDataSource pagedDS = new PagedDataSource();

        //Display details of found product reviews
        DataView dvProductGrid = new DataView(dtProductGrid);        

        pagedDS.DataSource = dvProductGrid;
        pagedDS.AllowPaging = true;
        pagedDS.CurrentPageIndex = pageIndex;
        pagedDS.PageSize = (int)_ProductsPageSize;


        //Display paging if there are users found.
        if (dtProductGrid.Rows.Count != 0)
        {
            TopPager.Visible = true;
            TopPager.FillPaging(pagedDS.PageCount, pagedDS.CurrentPageIndex + 1, 5, pagedDS.PageSize, dtProductGrid.Rows.Count);
        }
        else
        {
            TopPager.Visible = false;
        }

        dlProductGrid.DataSource = pagedDS;
        dlProductGrid.DataBind();
    }

    public void TopPager_PageChanged(object sender, CodeBehind_Pager.PagerEventArgs e)
    {
        LoadProductGrid(e.NewPage);
    }

    protected void dlProductGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            HtmlAnchor aName = (HtmlAnchor)e.Item.FindControl("aName");
            aName.HRef = "~/ObjectDetails.aspx?objType=ProductGrid&objId=" + Utilities.StrToHash(drv["Id"].ToString());
            aName.InnerText = drv["Name"].ToString();

            Label lblPrice = (Label)e.Item.FindControl("lblPrice");
            if (drv["StartPrice"] != DBNull.Value)
            {
                lblPrice.Text = drv["StartPrice"].ToString() + " " + ProductCatalogSettings.Currency;
            }

            Image imgProduct = (Image)e.Item.FindControl("imgProduct");            
            string imageUrl = drv["ImagePath"].ToString();

            if (imageUrl == String.Empty)
            {
                imgProduct.ImageUrl = Utilities.GetImageUrl(this.Page, ProductCatalogSettings.ThumbImageSrc);
            }
            else
            {
                imgProduct.ImageUrl = Utilities.GetThumbImage(imageUrl);
            }

            HyperLink lnkImageProduct = (HyperLink)e.Item.FindControl("lnkImageProduct");
            lnkImageProduct.NavigateUrl = aName.HRef;

            PagedDataSource pagedDS=(PagedDataSource)dlProductGrid.DataSource;
            if (((Convert.ToInt16(ProductsPageSize) / dlProductGrid.RepeatColumns) * dlProductGrid.RepeatColumns) < (e.Item.ItemIndex + 1))
            {
                e.Item.CssClass = "";
            }
        }
    }  
}
