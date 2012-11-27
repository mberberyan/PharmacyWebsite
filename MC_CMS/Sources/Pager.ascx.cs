using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.CMS.ComponentEngine;


/// <summary>
/// Contains the layout and functionality for paging various lists within the CMS component.
/// </summary>
/// <remarks>
///     <para>
///		The pages are arranged in so called page ranges, which represent groups of consecutive number of pages, i.e. (1-10,11-20, etc.). 
///		All page ranges have same size (number of pages) with the obvious possible exception of the last range.
///		Given that the control represents the list of pages in the following format:
///		<list type="bullet">
///			<item>Link to/text of first page.</item>
///			<item>Link to previous page range (if any).</item>
///			<item>Links of all pages from the current page range, with page index less than the currently selected page.</item>
///			<item>Text of the index of the current page.</item>
///			<item>Links of all pages from the current page range, with page index greater than the currently selected page.</item>
///			<item>Link to next page range (if any).</item>
///			<item>Link to/text of last page.</item>
///		</list>
///		</para>
///		<para> </para>
/// 	<para>
/// 	The following web controls build this control:
///			<list type="bullet">
///				<item><term>Div divPager</term><description> Main container, which can have a css class associated with it (set using CssClass property).</description></item>
///				<item><term>HtmlTable tblItemsDetails</term><description>Used to display some paging info (found items, currently showed items, etc.).</description></item>
///				<item><term>HtmlTable tblPageDetails</term><description>Used to display the actual page numbers/titles.</description></item>
///			</list>
/// 	</para>
///		<para> </para>
///		<para>
///		Remarks on the table tblItemsDetails:
///			<list type="table">
///				<item><term>Label lblTotalTitle</term><description>Displays the text from associated resource file, key lblTotalTitle.</description></item>
///				<item><term>Label lblItemFound</term><description>Displays the number of items found.</description></item>
///				<item><term>Label lblShowingItem</term><description>Displays the index range of the currently showed items.</description></item>
///			</list>
///		</para>
///		<para> </para>
///		<para>
///		Remarks on the table tblPageDetails:
///		<list type="table">
///			<item><term>LinkButton lbtnFirstPage</term><description>Used to navigate to the first page.</description></item>
///			<item><term>Label lblPage</term><description>Displays the text from the associated resource file, key lblPage.</description></item>
///			<item><term>LinkButton lbtnPrevPages</term><description>Used to navigate to the previous range of pages.</description></item>
///			<item><term>Repeater rptPagesLeft</term><description>Used to display links to pages within the current page range, but with index lower than the current page selected. Contains LinkButton lbtnPageNumber.</description></item>
///			<item><term>Label lblCurrentPage</term><description>Used to display the currently selected page index.</description></item>
///			<item><term>Repeater rptPagesRight</term><description>Used to display links to pages within the current page range, but with index greater than the current page selected. Contains LinkButton lbtnPageNumber.</description></item>
///			<item><term>LinkButton lbtnNextPages</term><description>Used to navigate to the next range of pages.</description></item>
///			<item><term>LinkButton lbtnLastPage</term><description>Used to navigate to the last page.</description></item>
///			<item><term>LinkButton lbtnPreviousPage</term><description>Used to navigate to the previous page index.</description></item>
///			<item><term>LinkButton lbtnNextPage</term><description>Used to navigate to the next page index.</description></item>
///		</list>
///		</para>
///		<para>
///			<list type="table">
///				<listheader><description>Note</description></listheader>
///				<item><description>Unless explicitly mentioned all resources and css classes are used according as in the initial source provided.</description></item>
///			</list>
///		</para>
/// </remarks>
public partial class Pager:CMSControl
{
    #region Events

    /// <summary>
    /// Event being fired when the page in the pager was changed.
    /// </summary>
    public event PagerEventHandler PageChanged;

    #endregion

    #region Delegates

    /// <summary>
    /// Represents the method that will handle the PageChanged event. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void PagerEventHandler(object sender, PagerEventArgs e);

    #endregion

    #region Inner Classes

    /// <summary>
    /// Class for event arguments for event PageChanged.
    /// </summary>
    public class PagerEventArgs : EventArgs
    {
        private int newPage;
        /// <summary>
        /// Gets or sets the new page.
        /// </summary>
        public int NewPage
        {
            get
            {
                return newPage;
            }
        }

        /// <summary>
        /// Constructor with page provided.
        /// </summary>
        /// <param name="newPage"></param>
        public PagerEventArgs(int newPage)
        {
            this.newPage = newPage;
        }
    }

    #endregion

    #region Interface Properties

    private bool _ShowItemsDetails = true;
    /// <summary>
    /// Defines whether the details like total found items and current displayed item indexes are displayed.
    /// </summary>
    public bool ShowItemsDetails
    {
        get
        {
            return this._ShowItemsDetails;
        }
        set
        {
            this._ShowItemsDetails = value;
        }
    }

    private bool _ShowFirstPageLink = false;
    /// <summary>
    /// Defines whether the link to the first page should be displayed.
    /// </summary>
    public bool ShowFirstPageLink
    {
        get
        {
            return this._ShowFirstPageLink;
        }
        set
        {
            this._ShowFirstPageLink = value;
        }
    }

    private bool _ShowLastPageLink = false;
    /// <summary>
    /// Defines whether the link to the last page should be displayed.
    /// </summary>
    public bool ShowLastPageLink
    {
        get
        {
            return this._ShowLastPageLink;
        }
        set
        {
            this._ShowLastPageLink = value;
        }
    }

    private string _CssClass = null;
    /// <summary>
    /// Defines the css class to be used for the main div.
    /// </summary>
    public string CssClass
    {
        get
        {
            return this._CssClass;
        }
        set
        {
            this._CssClass = value;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Attach event handlers to the controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>06/12/2007</date>
    protected override void OnInit(EventArgs e)
    {
        this.lbtnFirstPage.Command += new CommandEventHandler(ChangePage_Command);
        this.lbtnLastPage.Command += new CommandEventHandler(ChangePage_Command);
        this.lbtnPrevPages.Command += new CommandEventHandler(ChangePage_Command);
        this.lbtnNextPages.Command += new CommandEventHandler(ChangePage_Command);
        this.lbtnPreviousPage.Command += new CommandEventHandler(ChangePage_Command);
        this.lbtnNextPage.Command += new CommandEventHandler(ChangePage_Command);

        this.rptPagesLeft.ItemCreated += new RepeaterItemEventHandler(rptPages_ItemCreated);
        this.rptPagesLeft.ItemDataBound += new RepeaterItemEventHandler(rptPages_ItemDataBound);

        this.rptPagesRight.ItemCreated += new RepeaterItemEventHandler(rptPages_ItemCreated);
        this.rptPagesRight.ItemDataBound += new RepeaterItemEventHandler(rptPages_ItemDataBound);

        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Apply css styles to the pager control
        if (this._CssClass != null)
        {
            divPager.Attributes.Add("class", this._CssClass);
        }

        if (!this._ShowItemsDetails)
        {
            tblItemsDetails.Visible = false;
        }

        //Set visibility of links First and Last Page 
        lbtnFirstPage.Visible = this._ShowFirstPageLink;
        lbtnLastPage.Visible = this._ShowLastPageLink;

    }

    /// <summary>
    /// Event handler for event ItemCreated of both Repeater rptPagesLeft and Repeater rptPagesRight.
    /// </summary>
    /// <remarks>
    ///     Attached event handler <see cref="ChangePage_Command"/> for event Command of link buttons 
    ///     with name lbtnPageNumber in the repeaters.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>06/12/2007</date>
    protected void rptPages_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            LinkButton lbtnPageNumber = (LinkButton)e.Item.FindControl("lbtnPageNumber");
            lbtnPageNumber.Command += new CommandEventHandler(ChangePage_Command);
        }
    }

    /// <summary>
    /// Event handler for event ItemDataBound of both Repeater rptPagesLeft and Repeater rptPagesRight.
    /// </summary>
    /// <remarks>
    ///     The method is used to set properties Text and CommandArgument of link buttons lbtnPageNumber in the repeater.
    ///     <list type="bullet">
    ///         <item>Text - it is set equal to the number of the currently bound page.</item>
    ///         <item>CommandArgument - it is set equal to the number of the currently bound page.</item>
    ///     </list>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>06/12/2007</date>
    protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            string strPageId = e.Item.DataItem.ToString();
            LinkButton lbtnPageNumber = (LinkButton)e.Item.FindControl("lbtnPageNumber");
            lbtnPageNumber.Text = strPageId;
            lbtnPageNumber.CommandArgument = strPageId;
        }
    }

    /// <summary>
    /// Event handler for event Command of link buttons with name "lbtnPageNumber" in both repeaters repPagesLeft and repPagesRight. 
    /// </summary>
    /// <remarks>
    ///     Raises event PageChanged.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/01/2008</date>
    protected void ChangePage_Command(Object sender, CommandEventArgs e)
    {
        PagerEventArgs pe = new PagerEventArgs(Convert.ToInt32(e.CommandArgument) - 1);
        PageChanged(this, pe);
    }

    /// <summary>
    /// Performs the population of the links of pages.
    /// </summary>
    /// <param name="pageCount">The total number of pages.</param>
    /// <param name="currentPage">The index of the current page.</param>
    /// <param name="pageSize">The size of the page range.</param>
    /// <param name="pageItemSize">The number of items being displayed at a page.</param>
    /// <param name="itemsInGrid">The total number of items in all pages.</param>
    /// <author></author>
    /// <date>18/01/2008</date>
    public void FillPaging(int pageCount, int currentPage, int pageSize, int pageItemSize, int itemsInGrid)
    {
        lblCurrentPage.Text = currentPage.ToString();
        lblCurrentPage.Attributes.Add("style", "font-weight: bold ! important;");
        currentPage--;
        lbtnFirstPage.CommandArgument = "1";
        lbtnFirstPage.Visible = true;

        lbtnLastPage.CommandArgument = pageCount.ToString();
        lbtnLastPage.Visible = true;

        int previousPage = currentPage;
        lbtnPreviousPage.CommandArgument = previousPage.ToString();
        lbtnPreviousPage.Visible = true;

        int nextPage = currentPage + 2;
        lbtnNextPage.CommandArgument = nextPage.ToString();
        lbtnNextPage.Visible = true;

        //Set Item Found Label
        lblItemFound.Text = String.Format(GetLocalResourceObject("lblItemFound.Text").ToString(), itemsInGrid);
        //Set the Shown Items Label
        int lastIndex = (currentPage + 1) * pageItemSize;
        int firstIndex = (currentPage) * pageItemSize + 1;
        //check if we have items at all
        if (itemsInGrid == 0)
        {
            firstIndex = 0;
        }
        if (lastIndex > itemsInGrid)
        {
            lastIndex = itemsInGrid;
        }
        lblShowingItem.Text = String.Format(GetLocalResourceObject("lblShowingItem.Text").ToString(), firstIndex, lastIndex);
        if (currentPage == 0)
        {
            lbtnPreviousPage.Visible = false;
        }
        //check if we have items at all and if we have next page
        if ((currentPage == pageCount - 1) || (pageCount == 0))
        {
            lbtnNextPage.Visible = false;
        }
        if (currentPage < pageSize)
        {
            lbtnPrevPages.Visible = false;
        }
        else
        {
            lbtnPrevPages.Visible = true;
            int iPageLeft;
            iPageLeft = currentPage - (currentPage % pageSize);
            lbtnPrevPages.Text = "<<";
            lbtnPrevPages.CommandArgument = iPageLeft.ToString();
        }
        int iPageRight;
        if ((currentPage / pageSize) == ((pageCount - 1) / pageSize))
        {
            lbtnNextPages.Visible = false;
        }
        else
        {
            lbtnNextPages.Visible = true;
            iPageRight = currentPage - (currentPage % pageSize) + pageSize + 1;
            lbtnNextPages.Text = ">>";
            lbtnNextPages.CommandArgument = iPageRight.ToString();
        }
        ArrayList arrLeft = new ArrayList();
        ArrayList arrRight = new ArrayList();
        int startPage = (currentPage / pageSize) * pageSize;
        for (int i = startPage; i < currentPage; i++)
        {
            arrLeft.Add((i + 1).ToString());
        }
        int lastPage;
        int spacePages = pageCount - 1 - startPage;
        if ((pageSize - 1) > spacePages)
        {
            lastPage = pageCount - 1;
        }
        else
        {
            lastPage = startPage + Math.Min(spacePages, (pageSize - 1));
        }
        for (int j = currentPage + 1; j <= lastPage; j++)
        {
            arrRight.Add((j + 1).ToString());
        }
        rptPagesLeft.DataSource = arrLeft;
        rptPagesLeft.DataBind();

        rptPagesRight.DataSource = arrRight;
        rptPagesRight.DataBind();


        //Set visibility of links First and Last Page 
        lbtnFirstPage.Visible = this._ShowFirstPageLink;
        lbtnLastPage.Visible = this._ShowLastPageLink;
    }

    #endregion
}
