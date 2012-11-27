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
using System.Globalization;
using Melon.Components.News.Configuration;

public partial class Languages : System.Web.UI.UserControl
{
     
        #region Events

        /// <summary>
        /// Event raised when language is selected. 
        /// </summary>
        public event ChangeLanguageEventHandler ChangeLanguage;

        #endregion

        #region Fields & Properties

        /// <summary>
        /// The currently selected language in the News Administration Panel.
        /// </summary>
        public CultureInfo SelectedLanguage;

        #endregion

        /// <summary>
        /// Attach event handlers to the controls' events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.repLanguages.ItemCreated += new RepeaterItemEventHandler(repLanguages_ItemCreated);
            this.repLanguages.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repLanguages_ItemDataBound);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialize the user control.
        /// </summary>
        /// <remarks>
        ///     When user control is loaded for first time method <see cref="LoadNewsLanguages"/> is called 
        ///     to display in Repeater repLanguages all languages specified in news configuration element in web.config.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="LoadNewsLanguages"/>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLanguages();
            }
        }

        /// <summary>
        /// Event handler for event ItemCreated of Repeater repLanguages.
        /// </summary>
        /// <remarks>
        ///     Attached event handler <see cref="lbtnLanguage_Command"/> for event Command of link buttons 
        ///     with name lbtnLanguage in the repeater.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repLanguages_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                LinkButton lbtnLanguage = (LinkButton)e.Item.FindControl("lbtnLanguage");
                lbtnLanguage.Command += new CommandEventHandler(lbtnLanguage_Command);
            }
        }

        /// <summary>
        /// Event handler for event ItemDataBound of Repeater repLanguages.
        /// </summary>
        /// <remarks>
        ///     The method is used to set properties Text, CommandArgument and CssClass of link buttons lbtnLanguage in the repeater.
        ///     <list type="bullet">
        ///         <item>Text - it is set equal to the property Name of CultureInfo object that represents the currently bound language.</item>
        ///         <item>CommandArgument - it is set equal to property Name of CultureInfo object that represents the currently bound language.</item>
        ///         <item>CssClass - it is set equal to "mc_news_lang_link" or "mc_news_lang_link_selected" (if the language is the currently selected one).
        ///             These css styles are defined in stylesheet file NewsStyles/Styles.css. 
        ///             The currently selected language is set in user control property <see cref="SelectedLanguage"/></item>
        ///    </list>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repLanguages_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CultureInfo language = (CultureInfo)e.Item.DataItem;
                LinkButton lbtnLanguage = (LinkButton)e.Item.FindControl("lbtnLanguage");
                lbtnLanguage.Text = language.Name;
                lbtnLanguage.CommandArgument = language.Name;
                if (language.Name == Melon.Components.News.NewsLanguage.CurrentLanguage.Name)
                {
                    lbtnLanguage.CssClass = "selected";
                }
               
            }
        }

        /// <summary>
        /// Event handler for Command event of link buttons with name lbtnLanguage in Repeater repLanguages.
        /// </summary>
        /// <remarks>
        ///     Raises event ChangeLanguage.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnLanguage_Command(object sender, CommandEventArgs e)
        {
            string cultureName = Convert.ToString(e.CommandArgument);
            CultureInfo newLanguage = CultureInfo.GetCultureInfo(cultureName);

            ChangeLanguageArgs args = new ChangeLanguageArgs();
            args.NewLanguage = newLanguage;
            OnChangeLanguage(sender, args);
        }

        /// <summary>
        /// DataBinds Repeater repLanguages with the languages from <see cref="Melon.Components.News.Configuration.NewsSettings.Languages"/>
        /// </summary>
        /// <remarks>
        ///     If there is only one languages in collection <see cref="Melon.Components.News.Configuration.NewsSettings.Languages"/>
        ///     it is not displayed in the repeater because there is no need to switch between languages.
        /// </remarks>
        /// <seealso cref="Melon.Components.News.Configuration.NewsSettings"/>
        private void LoadLanguages()
        {
            repLanguages.DataSource = NewsSettings.Languages;
            repLanguages.DataBind();
        }

        /// <summary>
        /// Event handler for event ChangeLanguage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public virtual void OnChangeLanguage(object sender, ChangeLanguageArgs args)
        {
            Melon.Components.News.NewsLanguage.CurrentLanguage = args.NewLanguage;
            LoadLanguages();

            if (ChangeLanguage != null)
            {
                ChangeLanguage(sender, args);
            }
        }
}

    /// <summary>
    /// Class for event arguments of event ChangeLanguage.
    /// </summary>
    public class ChangeLanguageArgs : EventArgs
    {
        private CultureInfo _NewLanguage;
        /// <summary>
        /// Gets or sets new selected language.
        /// </summary>
        public CultureInfo NewLanguage
        {
            get { return _NewLanguage; }
            set { _NewLanguage = value; }
        }
    }
    /// <summary>
    /// Represents the method that will handle the ChangeLanguage event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void ChangeLanguageEventHandler(object sender, ChangeLanguageArgs args);

