using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.Forum.SearchEngine;
using Melon.Components.Forum.ComponentEngine;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Breadcrumb is a form of navigation where the current location within a forum is indicated by a list of links. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// The breadcrumb elements from collection <see cref="ContentSource"/> will be displayed as list of links which 
    /// are separated with sign specified in property <see cref="Separator"/>. 
    /// Repeater control <see cref="repBreadCrumb"/> is data bound with the breadcrumbs collection.</para>
    /// <para>
    /// Every breadcrumb element <see cref="Melon.Components.Forum.ComponentEngine.BreadCrumbElement"/> from collection <see cref="ContentSource"/>
    /// will be rendered as LinkButton with text, command name and command argument specified in its properties.
    /// When breadcrumb link is activated then all following breadcrumb elements in the collection are removed 
    /// and event <see cref="BreadCrumbChanged"/> is raised. 
    /// The event argument <see cref="Melon.Components.Forum.ComponentEngine.BreadCrumbEventArgs"/> 
    /// contains information for the breadcrumb element that was activated.
    /// In the component's main user control <see cref="Forum"/> the event is handled
    /// and depending of the activated breadcrumb element are loaded list of forums, list of forum topics or list of topic posts.
    /// </para>
    /// </remarks>
    public partial class Breadcrumb : ForumControl
    {
        #region Fields & Properties

        private string _separator = "-";
        /// <summary>
        /// Gets or sets the separator between the breadcrumb elements.
        /// </summary>
        public string Separator
        {
            set { _separator = value; }
        }

        /// <summary>
        /// Collection of BreadCrumbElement objects.
        /// </summary>
        public List<BreadCrumbElement> ContentSource;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the breadcrumb link is clicked.
        /// </summary>
        public event BreadCrumbChangedHandler BreadCrumbChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Register that the user control will use control state.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
            this.ContentSource = new List<BreadCrumbElement>();
            this.repBreadCrumb.ItemDataBound += new RepeaterItemEventHandler(rptBreadCrumb_ItemDataBound);
        }

        /// <summary>
        /// Saves property <see cref="ContentSource"/> in the control state. 
        /// </summary>
        /// <returns></returns>
        protected override object SaveControlState()
        {
            object obj = base.SaveControlState();
            return new Pair(obj, this.ContentSource);
        }

        /// <summary>
        /// Feed property <see cref="ContentSource"/> from the control state.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadControlState(object savedState)
        {
            if (savedState != null)
            {
                Pair p = savedState as Pair;
                if (p.First != null)
                {
                    base.LoadControlState(p.First);
                }
                this.ContentSource = p.Second as List<BreadCrumbElement>;
            }

        }


        /// <summary>
        /// Binds the repeater repBreadCrumb with the breadcrumb elements from collection <see cref="ContentSource"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.repBreadCrumb.DataSource = this.ContentSource;
            this.DataBind();
        }

        /// <summary>
        /// Creates LinkButton control for the current bound breadcrumb element and
        /// set properties (text, command name, command argument) corresponding to the breadcrumb element properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptBreadCrumb_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                BreadCrumbElement el = (BreadCrumbElement)e.Item.DataItem;
                LinkButton lbtnBreadCrumb = (LinkButton)e.Item.FindControl("lbtnBreadCrumb");

                lbtnBreadCrumb.Text = Server.HtmlEncode(el.BreadCrumbText);
                lbtnBreadCrumb.Command += new CommandEventHandler(lbtnBreadCrumb_Command);
                lbtnBreadCrumb.CommandName = Convert.ToString(Convert.ToInt32(el.CommandName));
                lbtnBreadCrumb.CommandArgument = Convert.ToString(el.CommandArgument);
                lbtnBreadCrumb.CausesValidation = false;
            }
            else if (e.Item.ItemType == ListItemType.Separator)
            {
                Label lblSeparator = (Label)e.Item.FindControl("lblSeparator");
                lblSeparator.Text = this._separator;
            }
        }

        /// <summary>
        /// Event handler for Command event of every breadcrumb LinkButton.
        /// Find the position of the breadcrumb element which fired the event and remove all breadcrumb elements after it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnBreadCrumb_Command(object sender, CommandEventArgs e)
        {
            BreadCrumbCommandType cmdType = (BreadCrumbCommandType)int.Parse(e.CommandName);

            int? arg = null;
            arg = (Convert.ToString(e.CommandArgument) == "") ? (int?)null : arg.GetValueOrDefault(Convert.ToInt32(e.CommandArgument));

            //Add history entry.
			if ((cmdType == BreadCrumbCommandType.ListForums) || (cmdType == BreadCrumbCommandType.Search))
			{
				nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("forumId:all");
			}
			else if (cmdType == BreadCrumbCommandType.ListTopics)
			{
				string newquery = "forumId:" + arg.Value.ToString();
				nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry(newquery);
			}
			else
			{
				string newquery = "forumId:" + this.ContentSource[1].CommandArgument.Value.ToString() + ";topicId:" + this.ContentSource[2].CommandArgument.Value.ToString();
				nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry(newquery);
			}

            // Find the position from which we should remove
            int selectedPosition = 0;

            BreadCrumbElement element = new BreadCrumbElement(((LinkButton)sender).Text, cmdType, arg);
            Predicate<BreadCrumbElement> predicate = new Predicate<BreadCrumbElement>(element.Match);
            selectedPosition = this.ContentSource.FindIndex(predicate);

            string bText = this.ContentSource[selectedPosition].BreadCrumbText;
            this.ContentSource.RemoveRange(selectedPosition, ContentSource.Count - selectedPosition);

            this.OnBreadCrumbChanged(this, new BreadCrumbEventArgs(bText, cmdType, arg));
        }


        /// <summary>
        /// Raises event BreadCrumbChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnBreadCrumbChanged(object sender, BreadCrumbEventArgs e)
        {
            if (BreadCrumbChanged != null)
            {
                BreadCrumbChanged(sender, e);
            }
        }

        /// <summary>
        /// Add a breadcrumb element to the collection of breadcrumb elements (ContentSource) if it is not already in the collection.
        /// </summary>
        /// <param name="breadCrumbText"></param>
        /// <param name="commandName"></param>
        /// <param name="commandArgument"></param>
        public void Add(string breadCrumbText, BreadCrumbCommandType commandName, int? commandArgument)
        {
            BreadCrumbElement newEl = new BreadCrumbElement(breadCrumbText, commandName, commandArgument);

            if (this.ContentSource == null)
            {
                this.ContentSource = new List<BreadCrumbElement>();
            }

            Predicate<BreadCrumbElement> predicate = new Predicate<BreadCrumbElement>(newEl.Match);
            if (!this.ContentSource.Exists(predicate))
            {
                this.ContentSource.Add(newEl);

                this.repBreadCrumb.DataSource = this.ContentSource;
                this.DataBind();
            }
        }

        /// <summary>
        /// Clear all collection ContentSource.
        /// </summary>
        public void Reset()
        {
            this.ContentSource = null;
        }


		/// <summary>
		/// Prepares a standard query string based on the ContentSource.
		/// </summary>
		/// <returns>The prepared query.</returns>
		public string PrepareNormalQuery()
		{
			string _default = "forumId=all";
			if (this.ContentSource.Count <= 1)
			{
				return _default;
			}
			else if (this.ContentSource.Count == 2)
			{
				if (this.ContentSource[1].CommandArgument.HasValue)
				{
					return "forumId=" + this.ContentSource[1].CommandArgument.Value;
				}
				else
				{
					return _default;
				}
			}
			else if (this.ContentSource.Count == 3)
			{
				if (this.ContentSource[1].CommandArgument.HasValue && this.ContentSource[2].CommandArgument.HasValue)
				{
					return "forumId=" + this.ContentSource[1].CommandArgument.Value + "&topicId=" + this.ContentSource[2].CommandArgument.Value;
				}
				else
				{
					return _default;
				}
			}
			else
			{
				return _default;
			}
			
		}

        #endregion Methods
    }
}