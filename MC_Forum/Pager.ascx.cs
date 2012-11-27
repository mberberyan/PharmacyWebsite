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
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.UI.Controls;

namespace Melon.Components.Forum.UI.CodeBehind
{
	/// <summary>
	/// Contains the layout and functionality for paging various lists within the forum component.
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
	///				<item><term>Div divPager:</term><description> Main container, which can have a css class associated with it (set using CssClass property).</description></item>
	///				<item><term>HtmlTable tblItemsDetails: </term><description>Used to display some paging info (found items, currently showed items, etc.).</description></item>
	///				<item><term>HtmlTable tblPageDetails: </term><description>Used to display the actual page numbers/titles.</description></item>
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
	///			<item><term>Repeater rptPageLeft</term><description>Used to display links to pages within the current page range, but with index lower than the current page selected. Contains LinkButton lbtnPageNumber.</description></item>
	///			<item><term>Label lblCurrentPage</term><description>Used to display the currently selected page index.</description></item>
	///			<item><term>Repeater rptPageRight</term><description>Used to display links to pages within the current page range, but with index greater than the current page selected. Contains LinkButton lbtnPageNumber.</description></item>
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
	/// <seealso cref="Melon.Components.Forum.UI.CodeBehind.Pager"/>
	/// <seealso cref="Melon.Components.Forum.UI.Controls.ForumUserNicknameDisplay"/>
    public partial class Pager : ForumControl
    {
        #region Events
		/// <summary>
		/// The event being fired when the page of the pager was changed
		/// </summary>
        public event PagerEventHandler PageChanged;
        #endregion

        #region Delegates
		/// <summary>
		/// Defines the event handler for the PageChanged event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        public delegate void PagerEventHandler(object sender, PagerEventArgs e);
        #endregion

        #region Inner Classes
		/// <summary>
		/// The event arguments needed for PageChanged event
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
		/// Defines if the details like total found items and current displayed item indexes are shown.
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
		/// Defines if the link to the first page should be shown.
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
		/// Defines if the link to the last page should be shown.
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
        /// Attach event handlers to the controls' events/
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.lbtnFirstPage.Command += new CommandEventHandler(ChangePage_Command);
            this.lbtnLastPage.Command += new CommandEventHandler(ChangePage_Command);
            this.lbtnPrevPages.Command += new CommandEventHandler(ChangePage_Command);
            this.lbtnNextPages.Command += new CommandEventHandler(ChangePage_Command);
            this.lbtnPreviousPage.Command += new CommandEventHandler(ChangePage_Command);
            this.lbtnNextPage.Command += new CommandEventHandler(ChangePage_Command);

            this.rptPageLeft.ItemCreated += new RepeaterItemEventHandler(rptPage_ItemCreated);
            this.rptPageLeft.ItemDataBound += new RepeaterItemEventHandler(rptPage_ItemDataBound);

            this.rptPageRight.ItemCreated += new RepeaterItemEventHandler(rptPage_ItemCreated);
            this.rptPageRight.ItemDataBound += new RepeaterItemEventHandler(rptPage_ItemDataBound);

            base.OnInit(e);
        }


        /// <summary>
        /// Initialize the control
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
        /// Attach event handlers to the events of the controls in the repeater.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPage_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
				MelonLinkButton lbtnPageNumber = (MelonLinkButton)e.Item.FindControl("lbtnPageNumber");
                lbtnPageNumber.Command += new CommandEventHandler(ChangePage_Command);
            }
        }


        /// <summary>
        /// Called to for additional processing of the page numbers to enable the ajax navigation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPage_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string strPageId = e.Item.DataItem.ToString();
				MelonLinkButton lbtnPageNumber = (MelonLinkButton)e.Item.FindControl("lbtnPageNumber");
				lbtnPageNumber.Text = strPageId.Split(new char[] { '*' })[0];
                lbtnPageNumber.CommandArgument = strPageId;
				string[] args = strPageId.ToString().Split(new char[] { '*' });
				if (args[1].EndsWith(";"))
				{
					args[1] = args[1].TrimEnd(new char[] { ';' });
				}
				lbtnPageNumber.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?" + args[1].Replace(";", "&").Replace(":", "=") + "&gotoPage=" + args[0];
            }
        }

        /// <summary>
        /// Event handler of every LinkButton Command event. Fires the event of page being changed. This event is handled in the parent control. Also ensures ajax navigation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage_Command(Object sender, CommandEventArgs e)
        {
			string[] args = e.CommandArgument.ToString().Split(new char[] { '*' });
			if (args[1].EndsWith(";"))
			{
				args[1] = args[1].TrimEnd(new char[] { ';' });
			}
			PagerEventArgs pe = new PagerEventArgs(Convert.ToInt32(args[0]) - 1);
			nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry(args[1] + ";gotoPage:" + args[0]);

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
		/// <param name="addHashQuery">Additional query suffix that should be used to maintain the ajax navigation.</param>
		public void FillPaging(int pageCount, int currentPage, int pageSize, int pageItemSize, int itemsInGrid, string addHashQuery)
		{
			lblCurrentPage.Text = currentPage.ToString();
			lblCurrentPage.Attributes.Add("style", "font-weight: bold ! important;");
			currentPage--;
			lbtnFirstPage.CommandArgument = "1*" + addHashQuery;
			lbtnFirstPage.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?" + addHashQuery.Replace(";", "&").Replace(":", "=") + "&gotoPage=1";
			lbtnFirstPage.Visible = true;

			lbtnLastPage.CommandArgument = pageCount.ToString() + "*" + addHashQuery;
			lbtnLastPage.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?" + addHashQuery.Replace(";", "&").Replace(":", "=") + "&gotoPage=" + pageCount.ToString();
			lbtnLastPage.Visible = true;

			int previousPage = currentPage;
			lbtnPreviousPage.CommandArgument = previousPage.ToString() + "*" + addHashQuery;
			lbtnPreviousPage.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?" + addHashQuery.Replace(";", "&").Replace(":", "=") + "&gotoPage=" + previousPage.ToString();
			lbtnPreviousPage.Visible = true;

			int nextPage = currentPage + 2;
			lbtnNextPage.CommandArgument = nextPage.ToString() + "*" + addHashQuery;
			lbtnNextPage.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?" + addHashQuery.Replace(";", "&").Replace(":", "=") + "&gotoPage=" + nextPage.ToString();
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
				lbtnPrevPages.CommandArgument = iPageLeft.ToString() + "*" + addHashQuery;
				lbtnPrevPages.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?" + addHashQuery.Replace(";", "&").Replace(":", "=") + "&gotoPage=" + iPageLeft.ToString();
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
				lbtnNextPages.CommandArgument = iPageRight.ToString() + "*" + addHashQuery;
				lbtnNextPages.Href = Request.Url.AbsoluteUri.Split(new char[] { '?' })[0] + "?" + addHashQuery.Replace(";", "&").Replace(":", "=") + "&gotoPage=" + iPageRight.ToString();
			}
			ArrayList arrLeft = new ArrayList();
			ArrayList arrRight = new ArrayList();
			int startPage = (currentPage / pageSize) * pageSize;
			for (int i = startPage; i < currentPage; i++)
			{
				arrLeft.Add((i + 1).ToString() + "*" + addHashQuery);
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
				arrRight.Add((j + 1).ToString() + "*" + addHashQuery);
			}
			rptPageLeft.DataSource = arrLeft;
			rptPageLeft.DataBind();

			rptPageRight.DataSource = arrRight;
			rptPageRight.DataBind();


			//Set visibility of links First and Last Page 
			lbtnFirstPage.Visible = this._ShowFirstPageLink;
			lbtnLastPage.Visible = this._ShowLastPageLink;
		}

        #endregion
    }
}