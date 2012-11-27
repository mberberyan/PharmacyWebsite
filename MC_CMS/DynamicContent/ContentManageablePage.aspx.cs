using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS.Exception;
using System.Reflection;

namespace Melon.Components.CMS.UI.CodeBehind
{
    /// <summary>
    /// Web page to which redirects the router page to preview or edit content-manageable page.
    /// </summary>
    public partial class ContentManageablePage : System.Web.UI.Page
    {
        #region Fields & Properties

        /// <summary>
        /// Identifier of template used by the content-manageable page.
        /// </summary>
        static int templateId;

        /// <summary>
        /// Virtual path of master page used by template.
        /// </summary>
        static string masterPage;

        #endregion

        /// <summary>
        /// Set master page of content-manageable page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["templateId"] != null)
                {
                    templateId = Convert.ToInt32(Request["templateId"]);
                    Template objTemplate = Template.Load(templateId);
                    masterPage = objTemplate.MasterPage;
                    this.MasterPageFile = masterPage;
                }

            }
            else
            {
                this.MasterPageFile = masterPage;
            }
        }

        /// <summary>
        /// Attach event handler for change language event (if any).
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            //Attach method to be executed when language is changed.
            MethodInfo method = typeof(ContentManageablePage).GetMethod("DisplayContent");
            Utilities.ChangeLanguageEventAttach(this.Page, method);

            base.OnInit(e);
        }

        /// <summary>
        /// Calls method <see cref="DisplayContent()"/> to load content of draft content-manageable page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Request.QueryString["error"]!= null && Request.QueryString["error"] != "pageNotFound"))
            {
                DisplayContent();
            }  
        }

        /// <summary>
        /// Loads draft content of content-manageable page in the placeholders of its template.
        /// </summary>
        /// <remarks>
        /// If page is opened in edit mode then content placeholders will be replaced with FCKEdtors and their content 
        /// will be set as their html.
        /// </remarks>
        public static void DisplayContent()
        {
            System.Web.UI.Page page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;
            if (!String.IsNullOrEmpty(masterPage))
            {
                //Get all content placeholders.
                List<string> placeholders = new List<string>();
                string masterPagePhysicalPath = page.Server.MapPath(masterPage);
                try
                {
                    placeholders = Template.GetMasterPagePlaceholders(masterPagePhysicalPath);
                }
                catch (CMSException ex)
                {
                    if (ex.Code == CMSExceptionCode.MasterPageNotFound)
                    {
                        HtmlHead head = (HtmlHead)page.Page.Header;

                        HtmlLink cmsStyles = new HtmlLink();
                        cmsStyles.Href = CMSSettings.BasePath + "Sources/CMS_Styles/Styles.css";
                        cmsStyles.Attributes.Add("rel", "stylesheet");
                        cmsStyles.Attributes.Add("type", "text/css");
                        head.Controls.Add(cmsStyles);

                        page.Response.Write("<span class=\"mc_cms_error_message\">" + String.Format(Convert.ToString(((ContentManageablePage)page).GetLocalResourceObject("MasterPageNotFoundErrorMessage")), masterPagePhysicalPath) + "</span>");
                        return;
                    }
                }

                //Retrieve from database data for the draft.
                DataSet dsPage = new DataSet();
                //pageId - query parameter which contains identifier of the content-manageable page which draft we display.
                int? id = (page.Request.QueryString["pageId"] != null) ? Convert.ToInt32(page.Request.QueryString["pageId"]) : (int?)null;
                if (id.HasValue)
                {
                    bool restoreFromLiveVersion = (page.Request.QueryString["restore"] != null && Convert.ToString(page.Request.QueryString["restore"]) == "yes") ? true : false;
                    bool previewLiveVersion = (Melon.Components.CMS.CMS.BrowseVersion == NodeVersion.Live);

                    string languageName;
                    if (CMSSettings.WebSiteLanguageProperty != null)
                    {
                        languageName = ((CultureInfo)CMSSettings.WebSiteLanguageProperty.GetValue(null, null)).Name;
                    }
                    else
                    {
                        languageName = CMSSettings.DefaultLanguage.Name;
                    }

                    ArrayList Params = new ArrayList();
                    Params.Add(id);
                    Params.Add(languageName);
                    Params.Add(CMSSettings.DefaultLanguage.Name);
                    Params.Add(restoreFromLiveVersion || previewLiveVersion);

                    dsPage = DataAccess.ExecuteDataset("MC_CMS_DynamicContentManageablePageGet", Params);

                    if (dsPage.Tables[0].Rows.Count > 0)
                    {
                        //Page exists.
                        DataRow drPageDetails = dsPage.Tables[0].Rows[0];

                        #region Set meta tags

                        // *** Set title ***
                        if (!drPageDetails.IsNull("Title"))
                        {
                            page.Page.Title = Convert.ToString(drPageDetails["Title"]);
                        }

                        HtmlHead head = (HtmlHead)page.Page.Header;

                        // *** Set meta tags ***
                        if (!drPageDetails.IsNull("MetaTagKeywords"))
                        {
                            HtmlMeta keywords = new HtmlMeta();
                            keywords.Name = "keywords";
                            keywords.Content = Convert.ToString(drPageDetails["MetaTagKeywords"]);
                            head.Controls.Add(keywords);
                        }

                        if (!drPageDetails.IsNull("MetaTagDescription"))
                        {
                            HtmlMeta description = new HtmlMeta();
                            description.Name = "description";
                            description.Content = Convert.ToString(drPageDetails["MetaTagDescription"]);
                            head.Controls.Add(description);
                        }

                        if (!drPageDetails.IsNull("MetaTagAuthor"))
                        {
                            HtmlMeta author = new HtmlMeta();
                            author.Name = "author";
                            author.Content = Convert.ToString(drPageDetails["MetaTagAuthor"]);
                            head.Controls.Add(author);
                        }

                        #endregion
                    }
                    else
                    {
                        //Page doesn't exist.
                        if (Melon.Components.CMS.CMS.BrowseMode == PageMode.Edit)
                        {
                            page.Response.Redirect(page.Request.RawUrl + "&error=pageNotFound",true);
                        }
                        else
                        {
                            HtmlHead head = (HtmlHead)page.Page.Header;

                            HtmlLink cmsStyles = new HtmlLink();
                            cmsStyles.Href = CMSSettings.BasePath + "Sources/CMS_Styles/Styles.css";
                            cmsStyles.Attributes.Add("rel", "stylesheet");
                            cmsStyles.Attributes.Add("type", "text/css");
                            head.Controls.Add(cmsStyles);

                            page.Response.Write("<span class=\"mc_cms_error_message\">" + Convert.ToString(((ContentManageablePage)page).GetLocalResourceObject("PageNotFound")) + "</span>");
                            return;
                        }
                    }
                }

                #region Set content placeholders

                foreach (string contentPlaceholderID in placeholders)
                {
                    string content = "";
                    if ((id != null) && (templateId == Convert.ToInt32(dsPage.Tables[0].Rows[0]["TemplateId"])))
                    {
                        DataRow[] drs = dsPage.Tables[1].Select("ContentPlaceholderID = '" + contentPlaceholderID + "'");
                        if (drs.Length != 0)
                        {
                            content = Convert.ToString(drs[0]["Content"]);
                        }
                    }

                    ContentPlaceHolder ph = Utilities.FindContentPlaceholder(contentPlaceholderID, page.Page);
                    if (ph != null)
                    {
                        if (Melon.Components.CMS.CMS.BrowseMode == PageMode.Edit)
                        {
                            page.Page.ClientScript.RegisterClientScriptInclude(page.GetType(), "mc_cms_fckeditor_js", page.ResolveUrl(CMSSettings.BasePath + "Editors/fckeditor/" + "fckeditor.js"));

                            string editorID = "editorID_" + contentPlaceholderID;
                            string editorVar = "editor_" + contentPlaceholderID;

                            HtmlGenericControl spanEditor = new HtmlGenericControl("span");
                            //                            spanEditor.InnerHtml = @"<script type=""text/javascript"" language=""javascript"">
                            //												var " + editorVar + @" = new FCKeditor('" + editorID + @"');
                            //												" + editorVar + @".BasePath = '" + page.ResolveUrl(CMSSettings.BasePath + "Editors/fckeditor/") + @"';
                            //												
                            //                                                " + editorVar + @".Value = """ + content.Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;","&").Replace ("\"","\\\"") +@""";
                            //												" + editorVar + @".Create();
                            //											</scr" + "ipt>";
                            spanEditor.InnerHtml = @"<script type=""text/javascript"" language=""javascript"">
												    var " + editorVar + @" = new FCKeditor('" + editorID + @"');
												    " + editorVar + @".BasePath = '" + page.ResolveUrl(CMSSettings.BasePath + "Editors/fckeditor/") + @"';
    												
                                                    " + editorVar + @".Value = """ + content.Replace("\"", "\\\"") + @""";
												    " + editorVar + @".Create();
											    </scr" + "ipt>";
                            ph.Controls.Clear();
                            ph.Controls.Add(spanEditor);
                        }
                        else
                        {
                            //Preview
                            HtmlGenericControl spanContent = new HtmlGenericControl("span");
                            //spanContent.InnerHtml = content.Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&").Replace("\\\"", "\"");
                            spanContent.InnerHtml = content;
                            ph.Controls.Clear();
                            ph.Controls.Add(spanContent);
                        }
                    }
                    else
                    {
                        HtmlHead head1 = (HtmlHead)page.Page.Header;

                        HtmlLink cmsStyles = new HtmlLink();
                        cmsStyles.Href = CMSSettings.BasePath + "Sources/CMS_Styles/Styles.css";
                        cmsStyles.Attributes.Add("rel", "stylesheet");
                        cmsStyles.Attributes.Add("type", "text/css");
                        head1.Controls.Add(cmsStyles);

                        page.Response.Write("<span class=\"mc_cms_error_message\">" + String.Format(Convert.ToString(((ContentManageablePage)page).GetLocalResourceObject("PlaceholderNotFoundErrorMessage")), contentPlaceholderID) + "</span>");
                    }
                }

                #endregion

            }
        }
    }
}
