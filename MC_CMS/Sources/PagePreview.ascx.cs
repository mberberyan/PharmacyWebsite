using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS.ComponentEngine;
using System.Web.UI;
using Melon.Components.CMS;


/// <summary>
/// Provides interface for preview live or draft version of page.
/// </summary>
/// <remarks>
/// <para>
///     The PagePreview user control displays preview of version specified in <see cref="Version"/> of page with id <see cref="PageId"/>.
/// </para>     
/// <para>
/// The user control is build by the following controls:
///     <list type="bullet">
///         <item>
///             <term>Label lblLoginLogoutWarning</term>
///             <description>
///                 Warning label that login and logout are recommended from the iframe where the preview is displayed.
///             </description>
///         </item>
///         <item>
///             <term>iframe iFramePreview</term>
///             <description>
///                 Here is displayed the preview of the page.As src attribute of this iframe is set the router pages from 
///                 CMS system Router.aspx which first sets some browsing settings defined in <see cref="CMS"/> class 
///                 and then redirect to one of the dynamic page for displaying cms page corresponding to the node type (it is specified in property <see cref="Url"/>).
///             </description>
///         </item>
///     </list>
/// </para>
/// </remarks>
public partial class PagePreview : CMSControl
{
    #region Fields & Properties 

    /// <summary>
    /// Version of page which is previewed.
    /// </summary>
    public NodeVersion Version;

    /// <summary>
    /// Id of page which is previewed.
    /// </summary>
    public int PageId;

    /// <summary>
    /// Url of the page which is previewed.
    /// </summary>
    public string Url;

    #endregion

    /// <summary>
    /// Set attribute src of iframe <see cref="iFramePreview"/> to the url of the previewed page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            string versionTitle = "";
            if (Version == NodeVersion.Draft)
            {
                versionTitle = Convert.ToString(GetLocalResourceObject("DraftVersion"));
            }
            else if (Version == NodeVersion.Live)
            {
                versionTitle = Convert.ToString(GetLocalResourceObject("LiveVersion"));
            }
            lblPreviewTitle.Text = String.Format(Convert.ToString(GetLocalResourceObject("PreviewTitle")),versionTitle);


            string iframeURL = "about:blank";
            if (Version == NodeVersion.Draft)
            {
                iframeURL = CMSSettings.RouterPagePath + "?version=draft&mode=preview&lang=" + this.ParentControl.CurrentLanguage.Name + "&url=" + Server.UrlEncode(Url);
            }
            else if (Version == NodeVersion.Live)
            {
                iframeURL = CMSSettings.RouterPagePath + "?version=live&mode=preview&lang=" + this.ParentControl.CurrentLanguage.Name + "&url=" + Server.UrlEncode(Url);   
            }
            iFramePreview.Attributes.Add("src", Page.ResolveUrl(iframeURL));
        }
    }

	/// <summary>
	/// Initializes the control's properties
	/// </summary>
	/// <param name="args">The values with which the properties will be initialized</param>
	public override void Initializer(object[] args)
    {
		this.Version = (NodeVersion)args[0];
		this.PageId = (int)args[1];
		this.Url = (string)args[2];
    }
}

