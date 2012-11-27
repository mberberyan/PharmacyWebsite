using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.Security;
using Melon.Components.CMS.ComponentEngine;


/// <summary>
/// Contains layout for meta-tags settings.
/// </summary>
///     <para>
///     All web controls from SettingsBoxMetaTags are using local resources.
///     To customize them modify resource file SettingsBoxMetaTags.resx located in the MC_CMS folder.
///     </para>
public partial class SettingsBoxMetaTags : SettingsControl
{
    #region Fields & Properties

    private string _Author;
    /// <summary>
    /// Meta tag author of node. 
    /// </summary>
    public string Author
    {
        get
        {
            string author = txtAuthor.Text.Trim();
            if (author != string.Empty)
            {
                return author;
            }
            else
            {
                return null;
            }
        }
        set { _Author = value; }
    }

    private string _Keywords;
    /// <summary>
    /// Meta tag keywords of node.
    /// </summary>
    public string Keywords
    {
        get
        {
            string keywords = txtKeywords.Text.Trim();
            if (keywords != string.Empty)
            {
                return keywords;
            }
            else
            {
                return null;
            }
        }
        set { _Keywords = value; }
    }

    private string _Description;
    /// <summary>
    /// Meta tag description of node.
    /// </summary>
    public string Description
    {
        get
        {
            string description = txtDescription.Text.Trim();
            if (description != string.Empty)
            {
                return description;
            }
            else
            {
                return null;
            }
        }
        set { _Description = value; }
    }

    private string _MetaTitle;
    /// <summary>
    /// Meta tag title of node (title displayed in browser).
    /// </summary>
    public string MetaTitle
    {
        get
        {
            string metaTitle = txtMetaTitle.Text.Trim();
            if (metaTitle != string.Empty)
            {
                return metaTitle;
            }
            else
            {
                return null;
            }
        }
        set { _MetaTitle = value; }
    }

    #endregion

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="DisplayMetaTags"/> to load meta tags in the interface
    ///     if the control is loaded for first time.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <date>26/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            DisplayMetaTags();
        }
    }

    /// <summary>
    /// Event handler for event ReLoadSettings of the user control inherited from 
    /// parent user control <see cref="Melon.Components.CMS.ComponentEngine.SettingsControl"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Calls method <see cref="DisplayMetaTags"/> to reload 
    ///         page meta tags in the interface.</para>
    ///     <para>
    ///         The event is fired from the parent user control Settings.ascx 
    ///         when node settings are restored from live version. In this case are loaded 
    ///         live version meta tags.</para>
    /// </remarks>
    /// <param name="args"></param>
    /// <date>26/03/2008</date>
    protected override void OnReLoadSettings(EventArgs args)
    {
        base.OnReLoadSettings(args);
        DisplayMetaTags();
    }

    /// <summary>
    /// Displays meta-tags of the content-manageable page in the interface.
    /// </summary>
    /// <date>26/03/2008</date>
    private void DisplayMetaTags()
    {
        txtAuthor.Text = this._Author;
        txtKeywords.Text = this._Keywords;
        txtDescription.Text = this._Description;
        txtMetaTitle.Text = this._MetaTitle;
    }
}
