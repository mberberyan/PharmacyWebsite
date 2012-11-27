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
using Melon.Components.Forum.ComponentEngine;
using Melon.Components.Forum.Exception;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Represents the main control of the Forum component, used to be included in the desired page or web control.
    /// The control inherits most of its functionality from <see cref="Melon.Components.Forum.ComponentEngine.InnerBaseForumControl">BaseForumControl</see> 
    /// and is a critical part of the Forum component engine.
    /// In short this control manages all the logic flow and by using a system of events decides how to proceed on every step. 
    /// This is deeply connected with the usage of dynamic control load, which is implemented here.
    /// </summary>
    /// <remarks>
    ///		<para> 
    ///			Elements of this control:
    ///			<list type="bullet">
    ///				<item>ScriptManager: Key element of ajax engine, dealing mainly with client scripts for ajax.</item>
    ///				<item>nStuff.UpdateHistory: Key element of ajax navigation. Component developed by Nikhil Kothari <see ref="www.nikhilk.net">www.nikhilk.net</see></item>
    ///				<item>UpdatePanel: Key element of ajax engine, defining ajax updatable area.</item>
    ///				<item><see cref="Melon.Components.Forum.UI.CodeBehind.Breadcrumb">Breadcrumbs</see>: Displays the navigation history in a breadcrumb format.</item>
    ///				<item>LinkButton lbtnUpdateProfile: Used to navigate (load control) for editing current user's profile.</item>
    ///				<item>LinkButton lbtnSearch: Used to navigate (load control) for searching.</item>
    ///				<item>ForumPanel panelFirst and panelSecond: Used to define placeholders for the child controls to load.</item>
    ///			</list>
    ///		</para>
    ///		<para> </para>
    ///		<para>
    ///			Basic workflow of the forum component engine:
    ///			<list type="bullet">
    ///				<listheader>
    ///					As a general rule in order events and data to be maintained between page recycling the state of the controls (dynamic or not) should be preserved. 
    ///					While in general ViewState helps with the static controls, for dynamic a little additional effort is needed – they must be recreated at proper time so the ASP.NET engine can match their ViewState from the previous page iteration.
    ///					In the suggested algorithm the following system was chosen:
    ///				</listheader>
    ///				<item>Before load of the ViewState of this a special refresh function is called which recreates the controls.</item>
    ///				<item>Since the refresh function should somehow know which controls to recreate and the ViewState is obviously not the place that information, 
    ///				the control state is used instead. This is simply because the control state is loaded before the viewstate.</item>
    ///				<item>Moreover a special structure <see cref="Melon.Components.Forum.ComponentEngine.ControlInitInfo"/> is needed to store the info about the controls in the control state. 
    ///				That structure contains the control file name (in case of web control) or the control class (in case of custom control). 
    ///				The structure contains also values of control's proeprties with which it should be initialized to restore control's state from the previous page request.</item>
    ///				<item>The above structure is stored in the control state and being read between cycles by the <see cref="Melon.Components.Forum.ComponentEngine.InnerBaseForumControl.Refresh">Refresh</see> function. This function on the other hand calls the function <see cref="Melon.Components.Forum.ComponentEngine.InnerBaseForumControl.LoadCustomControl(Melon.Components.Forum.ComponentEngine.ControlInitInfo[])">LoadCustomControl</see> which actually cares to load the controls and call the init blocks <see cref="Melon.Components.Forum.ComponentEngine.InnerForumControl.Initializer"/> with their argument values.</item>
    ///				<item>The main control(Forum) inherits <see cref="Melon.Components.Forum.ComponentEngine.BaseForumControl">BaseForumControl</see> which defines a set of delegates, events and methods which provide the communication system between the controls. Every event describes a specific action within the component. This action can happen in each of the child controls. Moreover every event has a public fire method, which allows the child controls to initiate the execution of attached event handler(s). All the handling of the events is done in this via handling its base’s events.</item>
    ///				<item>Parent class <see cref="Melon.Components.Forum.ComponentEngine.BaseForumControl">BasForumControl</see> class inherits <see cref="Melon.Components.Forum.ComponentEngine.InnerBaseForumControl">InnerBaseForumControl</see> class which contains all state functionality methods that are implemented</item>
    ///				<item>On the other hand all child controls inherit <see cref="Melon.Components.Forum.ComponentEngine.ForumControl">ForumControl</see> class, which defines the following set of features:
    ///					<list type="bullet">
    ///						<item>Property defining the parent control, which because of the above and the run-time creation of web controls should be the BaseForumControl (this explains why this control handles its base events).</item>
    ///						<item>Implementation of IsControlPostBack feature, indicating that the control was loaded before and if the postback occurred by it.</item>
    ///						<item>Paging consistency for controls needing it.</item>
    ///					</list>
    ///				</item>
    ///				<item>Finally to allow easier management and possible implementation of webparts, set of panels are defined in this control in which the child controls can reside. The loading is always serial and which panels should be left empty is decided by the parameters of the method loading the controls.</item>
    ///				<item>To allow including ajax update panels or any other controls, a new control <see cref="Melon.Components.Forum.UI.Controls.ForumPanel">ForumPanel</see> is created, which simply inherits the standard panel control.</item>
    ///			</list>
    ///		</para>
    ///		<para> </para>
    ///		<para>
    ///			<list type="table">
    ///				<listheader><description>Note</description></listheader>
    ///				<item><description>Unless explicitly mentioned all resources and css classes are used according as in the initial source provided.</description></item>
    ///			</list>
    ///		</para>
    /// </remarks>
    /// <seealso cref="Melon.Components.Forum.UI.Controls.ForumPanel"/>
    /// <seealso cref="Melon.Components.Forum.ComponentEngine.BaseForumControl"/>
    /// <seealso cref="Melon.Components.Forum.ComponentEngine.InnerBaseForumControl"/>
    /// <seealso cref="Melon.Components.Forum.ComponentEngine.ForumControl"/>
    /// <seealso cref="Melon.Components.Forum.UI.CodeBehind.Breadcrumb"/>
    public partial class Forum : BaseForumControl
    {
        /// <summary>
        /// Attach event handlers for the forum events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
           base.LoadForumGroupAddEditEvent += new LoadForumGroupAddEditEventHandler(LoadForumGroupAddEdit);
           base.LoadForumListEvent += new LoadForumListEventHandler(LoadForumList);
           base.LoadForumAddEditEvent += new LoadForumAddEditEventHandler(LoadForumAddEdit);
           base.LoadForumTopicListEvent += new LoadForumTopicListEventHandler(LoadForumTopicList);
           base.LoadForumPostListEvent += new LoadForumPostListEventHandler(LoadForumPostList);
           base.LoadForumTopicAddEditEvent += new LoadForumTopicAddEditEventHandler(LoadForumTopicAddEdit);
           base.LoadForumPostAddEditEvent += new LoadForumPostAddEditEventHandler(LoadForumPostAddEdit);
           base.LoadForumUserDetailsEvent += new LoadForumUserDetailsEventHandler(LoadForumUserDetails);
           base.LoadForumSearchResultsEvent += new LoadForumSearchResultsEventHandler(LoadForumSearchResults);
           base.LoadFoundForumItemEvent += new LoadFoundForumItemEventHandler(LoadFoundForumItem);
           base.LoadForumErrorInformationEvent += new LoadForumErrorInformationEventHandler(LoadForumErrorInformation);
           base.LoadErrorPopupEvent += new LoadErrorPopupEventHandler(LoadErrorPopup);
           base.RemoveForumControlEvent += new RemoveForumControlEventHandler(RemoveForumControl);

            this.lbtnUpdateProfile.Click += new EventHandler(lbtnUpdateProfile_Click);
            this.lbtnSearch.Click += new EventHandler(lbtnSearch_Click);
            this.cntrlBreadcrumbs.BreadCrumbChanged += new BreadCrumbChangedHandler(cntrlBreadcrumbs_BreadCrumbChanged);

            base.OnInit(e);
        }

        /// <summary>
        /// Initialization of the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
            //If there is currently loggd user display link for update user profile
            CurrentUser = ForumUserInfo.Load();

            if (!Page.IsPostBack)
            {
              
                if (CurrentUser != null)
                {
                    lbtnUpdateProfile.Visible = true;
                }
                else
                {
                    lbtnUpdateProfile.Visible = false;
                }

				if (Request.Url.PathAndQuery.Contains("init=yes"))
				{
					LoadForumList(this, new LoadForumListEventArgs());
				}
				else
				{
                    if (String.IsNullOrEmpty(Request.Url.Query))
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MelonForumComponentInitNoScript",
                                @"<noscript>
							<a href=""" + Request.Url.ToString() + @"?init=yes"">follow</a>
						</noscript>", false);

                        //When the page with the forum control is opened for first time 
                        //we check the query string for query parameters added by the forum control mechanism
                        //If there are such parameters we have to display the forum user control which added these parameters to the query string
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MelonForumComponentInitScript",
                        @"<script language=""javascript"" type=""text/javascript"">
						if(window.location.href.indexOf('?') > -1)
						{
							window.location.replace(window.location.href.replace(/\?/g,'#').replace(/\=/g,':').replace(/\&/g,';')); 
						}
						else
						{
							if(window.location.href.indexOf('#') == -1) 
							  {
									window.location.replace(window.location.href + '#forumId:all');
							  }
                            else
                            {
                                if(window.location.href.indexOf('#') == (window.location.href.length - 1))
                                {
                                    window.location.href += 'forumId:all';
                                }
                            }
						}
						</script>
						", false);
                    }
					else 
					{

                        ParseURL(new char[] { '&' }, new char[] { '=' }, (Request.Url.Query.Contains("?") ? Request.Url.Query.Substring(1) : Request.Url.Query));
					}
				}
            }
        }

        /// <summary>
        /// Load user control ForumGroupAddEdit.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumGroupAddEdit(object sender, LoadForumGroupAddEditEventArgs e)
        {
            ControlInitInfo cntrlForumListInitInfo = new ControlInitInfo("ForumList.ascx", false, null);
            ControlInitInfo cntrlForumGroupAddEditInitInfo = new ControlInitInfo("ForumGroupAddEdit.ascx", true, new object[] { e.ForumGroupId });
            LoadCustomControl(new ControlInitInfo[] { cntrlForumListInitInfo, cntrlForumGroupAddEditInitInfo });
        }

        /// <summary>
        /// Load user control ForumList.ascx
        /// </summary>
        protected void LoadForumList(object sender, LoadForumListEventArgs e)
        {
            ControlInitInfo cntrlForumListInitInfo = new ControlInitInfo("ForumList.ascx", true, new object[] { e.Message });
            LoadCustomControl(new ControlInitInfo[] { cntrlForumListInitInfo });

            this.cntrlBreadcrumbs.Reset();
            this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);
        }

        /// <summary>
        /// Load user controls: ForumList.ascx and ForumAddEdit.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumAddEdit(object sender, LoadForumAddEditEventArgs e)
        {
            ControlInitInfo cntrlForumListInitInfo = new ControlInitInfo("ForumList.ascx", false, null);
            ControlInitInfo cntrlForumAddEditInitInfo = new ControlInitInfo("ForumAddEdit.ascx", true, new object[] { e.ForumGroupId, e.ForumId });
            LoadCustomControl(new ControlInitInfo[] { cntrlForumListInitInfo, cntrlForumAddEditInitInfo });
        }

        /// <summary>
        /// Load user control ForumTopicList.ascx 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumTopicList(object sender, LoadForumTopicListEventArgs e)
        {
            ControlInitInfo cntrlForumTopicListInitInfo = new ControlInitInfo("ForumTopicList.ascx", true, new object[] { e.ForumId,e.ForumName, e.CurrentPage, e.Message });
            LoadCustomControl(new ControlInitInfo[] { cntrlForumTopicListInitInfo });

            updateHistory.AddEntry("forumId:" + e.ForumId.ToString());

            this.cntrlBreadcrumbs.Reset();
            this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);
            cntrlBreadcrumbs.Add(e.ForumName, BreadCrumbCommandType.ListTopics, e.ForumId);
        }

        /// <summary>
        /// Load user control ForumPostList.ascx 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumPostList(object sender, LoadForumPostListEventArgs e)
        {
			ControlInitInfo cntrlForumPostListInitInfo = new ControlInitInfo("ForumPostList.ascx", true, new object[] { e.ForumId, e.ForumName, e.ForumTopicId, e.ForumTopicName, e.PostId, e.CurrentPage }); 
			LoadCustomControl(new ControlInitInfo[] { cntrlForumPostListInitInfo });

            //After post new topic and posts list is open there was no topicId added in the query string. The next line fix this.
            updateHistory.AddEntry("forumId:" + e.ForumId.ToString() + ";topicId:" + e.ForumTopicId.ToString());

            this.cntrlBreadcrumbs.Reset();
            this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);
            cntrlBreadcrumbs.Add(e.ForumName, BreadCrumbCommandType.ListTopics, e.ForumId);
            cntrlBreadcrumbs.Add(e.ForumTopicName, BreadCrumbCommandType.ListPosts, e.ForumTopicId);
        }

        /// <summary>
        /// Load user controls: ForumTopicList.ascx and ForumTopicAddEdit.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumTopicAddEdit(object sender, LoadForumTopicAddEditEventArgs e)
        {
            BreadCrumbInfo bci = BreadCrumbInfo.Load(e.ForumId, null);

            ControlInitInfo cntrlForumTopicListInitInfo = new ControlInitInfo("ForumTopicList.ascx", false, new object[] { e.ForumId,bci.ForumName, e.CurrentPage, null});
			ControlInitInfo cntrlForumTopicAddEditInitInfo = new ControlInitInfo("ForumTopicAddEdit.ascx", true, new object[] { e.ForumId, e.ForumTopicId, e.CurrentPage });

            LoadCustomControl(new ControlInitInfo[] { cntrlForumTopicListInitInfo, cntrlForumTopicAddEditInitInfo });
        }

        /// <summary>
        /// Load user controls: ForumPostList.ascx and ForumPostAddEdit.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumPostAddEdit(object sender, LoadForumPostAddEditEventArgs e)
        {
            BreadCrumbInfo bci = BreadCrumbInfo.Load(e.ForumId, e.ForumTopicId);
			ControlInitInfo cntrlForumPostListInitInfo = new ControlInitInfo("ForumPostList.ascx", false, new object[] { e.ForumId, bci.ForumName,e.ForumTopicId, bci.TopicName ,e.ForumPostId,e.CurrentPage });
			ControlInitInfo cntrlForumPostAddEditInitInfo = new ControlInitInfo("ForumPostAddEdit.ascx", true, new object[] { e.ForumId, e.ForumTopicId, e.ForumPostId, e.CurrentPage });

            LoadCustomControl(new ControlInitInfo[] { cntrlForumPostListInitInfo, cntrlForumPostAddEditInitInfo });
        }

        /// <summary>
        /// Load user control ForumUserProfileDetails.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumUserDetails(object sender, LoadForumUserDetailEventArgs e)
        {
            ControlInitInfo cntrlForumUserDetailsInitInfo = new ControlInitInfo("ForumUserProfileDetails.ascx", true, new object[] { e.UserName });
            LoadCustomControl(new ControlInitInfo[] { cntrlForumUserDetailsInitInfo });
        }

        /// <summary>
        /// Load user control ForumSearchResults.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumSearchResults(object sender, LoadForumSearchResultsEventArgs e)
        {
            ControlInitInfo cntrlForumSearchCriteria = new ControlInitInfo("ForumSearchCriteria.ascx", false, null);
            ControlInitInfo cntrlForumSearchResults = new ControlInitInfo("ForumSearchResults.ascx", true, new object[] { e.SearchEngine });

            LoadCustomControl(new ControlInitInfo[] { cntrlForumSearchCriteria, cntrlForumSearchResults });
        }

        /// <summary>
        /// Load user controls 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadFoundForumItem(object sender, LoadFoundForumItemEventArgs e)
        {
            if (e.TopicId != null)
            {
                //Load topic posts
                cntrlBreadcrumbs.Add(e.ForumName, BreadCrumbCommandType.ListTopics, e.ForumId);
				LoadForumPostList(sender, new LoadForumPostListEventArgs(e.ForumId, e.ForumName, e.TopicId, e.TopicName, e.PostId));
            }
            else
            {
                //Load forum topics
                LoadForumTopicList(sender, new LoadForumTopicListEventArgs(e.ForumId, e.ForumName));
            }
        }

        /// <summary>
        /// Load user control ForumErrorInformation.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadForumErrorInformation(object sender, LoadForumErrorInformationEventArgs e)
        {
            ControlInitInfo cntrlForumErrorInformation = new ControlInitInfo("ForumErrorInformation.ascx", false, new object[] { e.Message });
            LoadCustomControl(new ControlInitInfo[] { cntrlForumErrorInformation });

            this.cntrlBreadcrumbs.Reset();
            this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);
        }

        /// <summary>
        /// Displays popup modal window with error message in it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoadErrorPopup(object sender, LoadErrorPopupEventArgs e)
        {
            lblError.Text = Server.HtmlEncode(e.ErrorMessage);
            if (e.RefreshScreen)
            {
                MPE.OnOkScript = "document.location.reload(true);";
                upMain.Update();
            }
            MPE.Show();
        }


        /// <summary>
        /// Remove forum control from its panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemoveForumControl(object sender, RemoveForumControlEventArgs e)
        {
            RemoveCustomControl(e.ControlFile);
        }

      

        /// <summary>
        /// Load user control: ForumUserProfileUpdate.ascx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnUpdateProfile_Click(object sender, EventArgs e)
        {
			if (CurrentUser == null)
			{
				string url = Request.Url.PathAndQuery + "?" + this.cntrlBreadcrumbs.PrepareNormalQuery();
				Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + Server.UrlEncode(url), true);
			}
			nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("mode:profile;username:" + CurrentUser.UserName);
			
            //ControlInitializer cntrlForumUserProfileEditInit = delegate(Control cntrl, object[] arg)
            //{
            //    Type t = cntrl.GetType();
            //    t.GetField("UserName").SetValue(cntrl, arg[0]);
            //};

			ControlInitInfo cntrlForumUserProfileEdit = new ControlInitInfo("ForumUserProfileEdit.ascx", true, new object[] { CurrentUser.UserName });
			
			
			LoadCustomControl(new ControlInitInfo[] { cntrlForumUserProfileEdit });
        }

        /// <summary>
        /// Load user control: ForumSearch.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
			nStuff.UpdateControls.UpdateHistory.GetCurrent(this.Page).AddEntry("mode:search");

			ControlInitInfo cntrlForumSearchCriteria = new ControlInitInfo("ForumSearchCriteria.ascx", true, null);
            LoadCustomControl(new ControlInitInfo[] { cntrlForumSearchCriteria });

            this.cntrlBreadcrumbs.Reset();
            this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);
        }

        /// <summary>
        /// Loads forum user control that corresponds to the breadcrumb element clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cntrlBreadcrumbs_BreadCrumbChanged(object sender, BreadCrumbEventArgs e)
        {
            switch (e.CommandName)
            {
                case BreadCrumbCommandType.ListForums:
                    this.LoadForumList(sender, new LoadForumListEventArgs());
                    break;
                case BreadCrumbCommandType.ListTopics:
                    LoadForumTopicListEventArgs argsListTopics = new LoadForumTopicListEventArgs();
                    argsListTopics.ForumId = e.CommandArgument;
                    argsListTopics.ForumName = e.BreadCrumbText;
                    this.LoadForumTopicList(sender, argsListTopics);
                    break;
                case BreadCrumbCommandType.ListPosts:
                    LoadForumPostListEventArgs argsListPosts = new LoadForumPostListEventArgs();
                    argsListPosts.ForumId = cntrlBreadcrumbs.ContentSource[1].CommandArgument;
                    argsListPosts.ForumName = cntrlBreadcrumbs.ContentSource[1].BreadCrumbText;
                    argsListPosts.ForumTopicId = e.CommandArgument;
                    argsListPosts.ForumTopicName = e.BreadCrumbText;
                    this.LoadForumPostList(sender, argsListPosts);
                    break;
                case BreadCrumbCommandType.Search:
                    this.lbtnSearch_Click(sender, new EventArgs());
                    break;
            }
        }

		/// <summary>
		/// Loads the controls needed based on the ajax navigation url
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void updateHistory_Navigate(object sender, nStuff.UpdateControls.HistoryEventArgs e)
		{
			ParseURL(new char[] { ';' }, new char[] { ':' }, e.EntryName);
		}

		/// <summary>
		/// Loads controls based on ajax/non-ajax url
		/// </summary>
		/// <param name="keySeparator">The separator between key-value pairs.</param>
		/// <param name="valueSeparator">The separator between a key and its value.</param>
		/// <param name="query">The query part of the url.</param>
		private void ParseURL(char[] keySeparator, char[] valueSeparator, string query)
		{
            string mode, username;
			int? forumId, topicId, postId, gotoPage;
         
			string[] args = query.Split(keySeparator);
			Dictionary<string, string> dictArgs = new Dictionary<string, string>();
			foreach (string arg in args)
			{
				string[] keyvalue = arg.Split(valueSeparator);
				if (keyvalue.Length != 2)
				{
					// Don't do nothing for hacked urls
					continue;
				}
				string key = keyvalue[0];
				string value = keyvalue[1];
				if (dictArgs.ContainsKey(key))
				{
					dictArgs[key] = value;
				}
				else
				{
					dictArgs.Add(key, value);
				}
			}

			//Get all query parameters for the forum
			try
			{
				mode = dictArgs.ContainsKey("mode") ? dictArgs["mode"] : (string)null;
                username = dictArgs.ContainsKey("username") ? dictArgs["username"] : (string)null;
				forumId = dictArgs.ContainsKey("forumId") ? ((dictArgs["forumId"] == "all") ? (int?)null : int.Parse(dictArgs["forumId"])) : (int?)null;
				topicId = dictArgs.ContainsKey("topicId") ? int.Parse(dictArgs["topicId"]) : (int?)null;
				postId = dictArgs.ContainsKey("postId") ? int.Parse(dictArgs["postId"]) : (int?)null;
				gotoPage = dictArgs.ContainsKey("gotoPage") ? int.Parse(dictArgs["gotoPage"]) : (int?)null;
			}
			catch (FormatException)
			{
				//Query parameter with incorrect format was found so load ForumErrorInformation.ascx.
				LoadForumErrorInformation(this, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("IncorrectURL").ToString()));
				upMain.Update();
				return;
			}

			// Check what forum query parameters are found and load the related forum user control
			if (mode == "search")
			{
				//ForumSearchCriteria.ascx should be loaded.
				lbtnSearch_Click(this.Page, new EventArgs());
			}
			else if (mode == "profile")
			{
				ForumUser objForumUser = ForumUserInfo.Load(username);
				if (objForumUser != null)
				{
					//ForumUserProfileDetails.ascx should be loaded.
					LoadForumUserDetails(this.Page, new LoadForumUserDetailEventArgs(username));
				}
				else
				{
					//The user for which profile details were requested doesn't exist so load ForumErrorInformation.ascx.
					LoadForumErrorInformation(this.Page, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("UnexistingForumUser").ToString()));
					upMain.Update();
					return;
				}

				this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);
				if (forumId != null)
				{
					BreadCrumbInfo bci;
					try
					{
						bci = BreadCrumbInfo.Load(forumId, topicId);
					}
					catch (ForumException ex)
					{
						//Id of unexisting forum or topic was entered as query parameter so load ForumErrorInformation.ascx.
						switch (ex.Code)
						{
							case ForumExceptionCode.UnexistingForum:
								LoadForumErrorInformation(this, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("UnexistingForum").ToString()));
								break;
							case ForumExceptionCode.UnexistingTopic:
								LoadForumErrorInformation(this, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("UnexistingTopic").ToString()));
								break; ;
						}
						upMain.Update();
						return;
					}

					this.cntrlBreadcrumbs.ContentSource.Clear();
					this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);
					this.cntrlBreadcrumbs.Add(bci.ForumName, BreadCrumbCommandType.ListTopics, forumId);
					if (topicId != null)
					{
						this.cntrlBreadcrumbs.Add(bci.TopicName, BreadCrumbCommandType.ListPosts, topicId);
					}
				}
			}
			else
			{
				if (forumId != null)
				{
					BreadCrumbInfo bci;
					try
					{
						bci = BreadCrumbInfo.Load(forumId, topicId);
					}
					catch (ForumException ex)
					{
						//Id of unexisting forum or topic was entered as query parameter so load ForumErrorInformation.ascx.
						switch (ex.Code)
						{
							case ForumExceptionCode.UnexistingForum:
								LoadForumErrorInformation(this, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("UnexistingForum").ToString()));
								break;
							case ForumExceptionCode.UnexistingTopic:
								LoadForumErrorInformation(this, new LoadForumErrorInformationEventArgs(GetLocalResourceObject("UnexistingTopic").ToString()));
								break;
						}
						upMain.Update();
						return;
					}

					this.cntrlBreadcrumbs.ContentSource.Clear();
					this.cntrlBreadcrumbs.Add(GetLocalResourceObject("ForumsBreadcrumbElement").ToString(), BreadCrumbCommandType.ListForums, null);

					this.cntrlBreadcrumbs.Add(bci.ForumName, BreadCrumbCommandType.ListTopics, forumId);
					if (topicId != null)
					{
						this.cntrlBreadcrumbs.Add(bci.TopicName, BreadCrumbCommandType.ListPosts, topicId);
                        if ((CurrentUser != null) && (Request.Cookies["mc_forum_mode_create_post"] != null) && (Request.Cookies["mc_forum_mode_create_post"].Value == "true"))
						{
							//ForumPostAddEdit.ascx should be loaded.
							LoadForumPostAddEditEventArgs postAEArgs = new LoadForumPostAddEditEventArgs(topicId);
                            postAEArgs.ForumId = forumId;
							postAEArgs.CurrentPage = gotoPage;
							LoadForumPostAddEdit(this, postAEArgs);
						}
						else
						{
							//ForumPostList.ascx should be loaded.
							LoadForumPostListEventArgs postArgs = new LoadForumPostListEventArgs(forumId, bci.ForumName, topicId, bci.TopicName, postId);
							postArgs.CurrentPage = gotoPage;
							LoadForumPostList(this, postArgs);
						}
					}
					else
					{
						if ((CurrentUser != null)&&(Request.Cookies["mc_forum_mode_create_topic"] != null) && (Request.Cookies["mc_forum_mode_create_topic"].Value == "true"))
						{
							//ForumTopicAddEdit.ascx should be loaded.
							LoadForumTopicAddEditEventArgs topicAEArgs = new LoadForumTopicAddEditEventArgs(forumId);
							topicAEArgs.CurrentPage = gotoPage;
							LoadForumTopicAddEdit(this, topicAEArgs);
						}
						else
						{
							//ForumTopicList.ascx should be loaded.
							LoadForumTopicList(this, new LoadForumTopicListEventArgs(forumId, bci.ForumName, gotoPage));
						}
					}
				}
				else
				{
					//ForumList.ascx should be loaded.               
					LoadForumList(this, new LoadForumListEventArgs());
				}
			}

			this.upMain.Update();
		}


       

    }
}
