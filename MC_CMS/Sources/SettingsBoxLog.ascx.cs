using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Melon.Components.CMS.ComponentEngine;


/// <summary>
/// Contains layout for displaying node logs.
/// </summary>
///     <para>
///     All web controls from SettingsBoxLog are using local resources.
///     To customize them modify resource file SettingsBoxLog.resx located in the MC_CMS folder.
///     </para>
public partial class SettingsBoxLog : MelonUserControl
{
    #region Fields & Properties

    /// <summary>
    /// Date when the node was created.
    /// </summary>
    public DateTime? DateCreated;

    /// <summary>
    /// User name of user who created the node.
    /// </summary>
    public string UserWhoCreated;

    /// <summary>
    /// Date when the node was last updated.
    /// </summary>
    public DateTime? DateLastUpdated; 

    /// <summary>
    /// User name of user who last updated the node.
    /// </summary>
    public string UserWhoLastUpdated;

    /// <summary>
    /// Date when the language version  of the node that is displayed was last updated.
    /// </summary>
    public DateTime? LangVersionDateLastUpdated;

    /// <summary>
    /// User name of user who last updated the language version of the node that is displayed.
    /// </summary>
    public string LangVersionUserWhoLastUpdated;

    /// <summary>
    /// Date when language version of the node that is displayed was last published.
    /// </summary>
    public DateTime? LangVersionDateLastPublished;

    /// <summary>
    /// User name of user who last published the language version of node that is displayed.
    /// </summary>
    public string LangVersionUserWhoLastPublished;

    /// <summary>
    /// Name of language that which node version is currently loaded. 
    /// </summary>
    public string LanguageName;

    #endregion

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     Loads in the interface all node settings that are passed to the user control through 
    ///     its public properties.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>14/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            //Set when and who created the node.
            if (this.DateCreated != null)
            {
                lblDateCreated.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblOn")), this.DateCreated.Value.ToString("ddd MMM dd, yyyy, hh:mm"));
                if (String.IsNullOrEmpty(this.UserWhoCreated))
                {
                    lblCreatedBy.Visible = false;
                }
                else
                {
                    lblCreatedBy.Visible = true;
                    lblCreatedBy.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblBy")), this.UserWhoCreated);
                }
            }

            //Set when and who last updated the node.
            if (this.DateLastUpdated !=null)
            {
                lblDateLastUpdated.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblOn")), this.DateLastUpdated.Value.ToString("ddd MMM dd, yyyy, hh:mm"));
                if (String.IsNullOrEmpty(this.UserWhoLastUpdated))
                {
                    lblLastUpdatedBy.Visible = false;
                }
                else
                {
                    lblLastUpdatedBy.Visible = true;
                    lblLastUpdatedBy.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblBy")), this.UserWhoLastUpdated); 
                }
            }

            //Set when and who last updated the language version of the node that is currently displayed.
            if (this.LangVersionDateLastUpdated!= null)
            {
                phLangVersionLastUpdated.Visible = true;
                lblLangVersionLastUpdated.Text = String.Format(Convert.ToString(GetLocalResourceObject("LangVersionLastUpdated")), LanguageName);
                lblLangVersionDateLastUpdated.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblOn")), this.LangVersionDateLastUpdated.Value.ToString("ddd MMM dd, yyyy, hh:mm"));
                if (String.IsNullOrEmpty(this.LangVersionUserWhoLastUpdated))
                {
                    lblLangVersionLastUpdatedBy.Visible = false;
                }
                else
                {
                    lblLangVersionLastUpdatedBy.Visible = true;
                    lblLangVersionLastUpdatedBy.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblBy")), this.LangVersionUserWhoLastUpdated);
                }
            }
            else
            {
                phLangVersionLastUpdated.Visible = false;
            }

            //Set when and who last published the language version of the node that is currently displayed.
            if (this.LangVersionDateLastPublished != null)
            {
                phLangVersionLastPublished.Visible = true;
                lblLangVersionLastPublished.Text = String.Format(Convert.ToString(GetLocalResourceObject("LangVersionLastPublished")), LanguageName);
                lblLangVersionDateLastPublished.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblOn")), this.LangVersionDateLastPublished.Value.ToString("ddd MMM dd, yyyy, hh:mm"));
                if (String.IsNullOrEmpty(this.LangVersionUserWhoLastPublished))
                {
                    lblLangVersionLastPublishedBy.Visible = false;
                }
                else
                {
                    lblLangVersionLastPublishedBy.Visible = true;
                    lblLangVersionLastPublishedBy.Text = String.Format(Convert.ToString(GetLocalResourceObject("lblBy")), this.LangVersionUserWhoLastPublished);
                }
            }
            else
            {
                phLangVersionLastPublished.Visible = false;
            }
        }
    }
}
