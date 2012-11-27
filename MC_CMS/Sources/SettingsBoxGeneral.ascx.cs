using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS;


/// <summary>
/// Contains layout for displaying general node settings.
/// </summary>
///     <para>
///     All web controls from SettingsBoxGeneral are using local resources.
///     To customize them modify resource file SettingsBoxGeneral.resx located in the MC_CMS folder.
///     </para>
public partial class SettingsBoxGeneral : SettingsControl
{
    #region Fields & Properties

    /// <summary>
    /// Location of node.
    /// </summary>
    public string Location;

    private string _Code;
    /// <summary>
    /// Code of node.
    /// </summary>
    public string Code
    {
        get { return txtCode.Text.Trim(); }
        set { this._Code = value; }
    }

    private string _Alias;
    /// <summary>
    /// Alias of node.
    /// </summary>
    public string Alias
    {
        get 
        { 
            string alias = txtAlias.Text.Trim();
            if (alias != String.Empty)
            {
                return alias;
            }
            else
            {
                return null;
            }
        }
        set { this._Alias = value; }
    }

    private string _Title;
    /// <summary>
    /// Title of node.
    /// </summary>
    public string Title
    {
        get { return txtTitle.Text.Trim(); }
        set { this._Title = value; }
    }

    /// <summary>
    /// Image of node.
    /// </summary>
    public Melon.Components.CMS.NodeImage Image
    {
        get
        {
            if (!chkRemoveImage.Checked)
            {
                if (fuImage.HasFile)
                {
                    //New image will be uploaded.
                    NodeImage image = new NodeImage();
                    image.BinaryInfo = fuImage.FileBytes;
                    image.FileName = fuImage.FileName;
                    return image;
                }
                else
                {
                    if (this.IsLiveVersionLoaded)
                    {
                        //Image should be restored from live version.
                        NodeImage image = new NodeImage();
                        image.RestoreFromLiveVersion = true;
                        return image;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                //Current image should be deleted.
                NodeImage image = new NodeImage();
                image.RemovePreviousImage = true;
                return image;
            }
        }
    }
    /// <summary>
    /// Virtual path of image. 
    /// </summary>
    public string ImagePath;
   
    /// <summary>
    /// Hover image of node.
    /// </summary>
    public Melon.Components.CMS.NodeImage HoverImage
    {
        get
        {
            if (!chkRemoveHoverImage.Checked)
            {
                if (fuHoverImage.HasFile)
                {
                    //New image will be uploaded.
                    NodeImage hoverImage = new NodeImage();
                    hoverImage.BinaryInfo = fuHoverImage.FileBytes;
                    hoverImage.FileName = fuHoverImage.FileName;
                    return hoverImage;
                }
                else
                {
                    if (this.IsLiveVersionLoaded)
                    {
                        //Image should be restored from live version.
                        NodeImage hoverImage = new NodeImage();
                        hoverImage.RestoreFromLiveVersion = true;
                        return hoverImage;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                //Current image should be deleted.
                NodeImage hoverImage = new NodeImage();
                hoverImage.RemovePreviousImage = true;
                return hoverImage;
            }
        }
    }
    /// <summary>
    /// Virtual path of hover image.
    /// </summary>
    public string HoverImagePath;
    
    /// <summary>
    /// Selected image of node.
    /// </summary>
    public Melon.Components.CMS.NodeImage SelectedImage
    {
        get
        {
            if (!chkRemoveSelectedImage.Checked)
            {
                if (fuSelectedImage.HasFile)
                {
                    //New image will be uploaded.
                    NodeImage selectedImage = new NodeImage();
                    selectedImage.BinaryInfo = fuSelectedImage.FileBytes;
                    selectedImage.FileName = fuSelectedImage.FileName;
                    return selectedImage;
                }
                else
                {
                    if (this.IsLiveVersionLoaded)
                    {
                        //Image should be restored from live version.
                        NodeImage selected = new NodeImage();
                        selected.RestoreFromLiveVersion = true;
                        return selected;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                //Current image should be deleted.
                NodeImage selected = new NodeImage();
                selected.RemovePreviousImage = true;
                return selected;
            }
        }
    }
    /// <summary>
    /// Virtual path of selected image.
    /// </summary>
    public string SelectedImagePath;

    private string _Target;
    /// <summary>
    /// Target of node.
    /// </summary>
    public string Target
     {
        get 
        {
            string _target = "";
            if (rdolTarget.SelectedValue == "frame")
            {
                _target = txtFrameName.Text.Trim();
            }
            else
            {
                _target = rdolTarget.SelectedValue;
            }
       
           return _target;
        }
        set 
        {
            this._Target = value;  
        }
    }

    private bool _IsHiddenInNavigation;
    /// <summary>
    /// Flag whether node is visible in the menu.
    /// </summary>
    public bool IsHiddenInNavigation
    {
        get { return chkHideInNavigation.Checked; }
        set { _IsHiddenInNavigation = value; }
    }

    /// <summary>
    /// Flag whether the live version of node settings is loaded.
    /// </summary>
    private bool IsLiveVersionLoaded
    {
        get
        {
            if (ViewState["__mc_cms_IsLiveVersionLoaded"] != null)
            {
                return Convert.ToBoolean(ViewState["__mc_cms_IsLiveVersionLoaded"]);
            }
            else
            { 
                return false; 
            }
        }
        set { ViewState["__mc_cms_IsLiveVersionLoaded"] = value; }
    }

    /// <summary>
    /// Url of image node displayed currently in the interface.
    /// </summary>
    public string DisplayedImageUrl
    {
        get { return imgImage.ImageUrl; }
    }

    #endregion

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     If the control is loaded for first time  method <see cref="DisplayGeneralSettings"/>
    ///     is called to load node general settings in the interface.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        lblImageError.Visible = false;
        lblHoverImageError.Visible = false;
        lblSelectedImageError.Visible = false;

        //*** Attach javascript ****
        Type csType = this.GetType();
        ClientScriptManager cs = Page.ClientScript;

        if (!cs.IsStartupScriptRegistered(csType, "__mc_cms_target"))
        {
            cs.RegisterStartupScript(csType, "__mc_cms_target",
               @" if(!!document.getElementById('" + rdolTarget.ClientID + @"'))
                  {
                     SelectTargetOption('" + rdolTarget.ClientID + @"');
                  }", true);
        }
        rdolTarget.Items[0].Attributes.Add("onclick", "SelectTargetOption('" + rdolTarget.ClientID + @"')");
        rdolTarget.Items[1].Attributes.Add("onclick", "SelectTargetOption('" + rdolTarget.ClientID + @"')");
        rdolTarget.Items[2].Attributes.Add("onclick", "SelectTargetOption('" + rdolTarget.ClientID + @"')");

        if (!IsControlPostBack)
        {
            txtCode.Focus();
            lblImageSettingsReminder.Text = String.Format(
                Convert.ToString(GetLocalResourceObject("lblImageSettingsReminder.Text")),
                String.Concat(CMSSettings.ImagesAllowedExtensions.ToArray()),
                CMSSettings.ImagesMaxSize);

            this.IsLiveVersionLoaded = false;
            DisplayGeneralSettings();
        }
    }


    /// <summary>
    /// Event handler for event ReLoadSettings of the user control inherited from 
    /// parent user control <see cref="Melon.Components.CMS.ComponentEngine.SettingsControl"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Calls method <see cref="DisplayGeneralSettings"/> to reload 
    ///         node general settings in the interface.</para>
    ///     <para>
    ///         The event is fired from the parent user control Settings.ascx 
    ///         when node settings are restored from live version. In this case are loaded 
    ///         live version general settings.</para>
    /// </remarks>
    /// <param name="args"></param>
    /// <author></author>
    /// <date>26/03/2008</date>
    protected override void OnReLoadSettings(EventArgs args)
    {
        base.OnReLoadSettings(args);
        this.IsLiveVersionLoaded = true;
        DisplayGeneralSettings();
    }

    /// <summary>
    /// Displays node general settings in the interface.
    /// </summary>
    /// <author></author>
    /// <date>26/03/2008</date>
    private void DisplayGeneralSettings()
    {
        if (String.IsNullOrEmpty(this.Location))
        {
            lblLocationPath.Text = Convert.ToString(GetLocalResourceObject("Explorer"));
        }
        else
        {
            string location = Convert.ToString(GetLocalResourceObject("Explorer")) + "/" + this.Location;
            lblLocationPath.Text = location.Replace("/", " / ");
        }
        txtCode.Text = this._Code;
        txtAlias.Text = this._Alias;
        txtTitle.Text = this._Title;
        if (!String.IsNullOrEmpty(this.ImagePath))
        {
            divImage.Visible = true;
            imgImage.ImageUrl = this.ImagePath;
            chkRemoveImage.Checked = false;
        }
        else
        {
            divImage.Visible = false;
        }
        if (!String.IsNullOrEmpty(this.HoverImagePath))
        {
            divHoverImage.Visible = true;
            imgHoverImage.ImageUrl = this.HoverImagePath;
            chkRemoveHoverImage.Checked = false;
        }
        else
        {
            divHoverImage.Visible = false;
        }
        if (this.CurrentNodeType == NodeType.Folder)
        {
            trSelectedImage.Visible = false;
            trTarget.Visible = false;
        }
        else
        {
            trSelectedImage.Visible = true;
            if (!String.IsNullOrEmpty(this.SelectedImagePath))
            {
                divSelectedImage.Visible = true;
                imgSelectedImage.ImageUrl = this.SelectedImagePath;
                chkRemoveSelectedImage.Checked = false;
            }
            else
            {
                divSelectedImage.Visible = false;
            }

            trTarget.Visible = true;
            if (!String.IsNullOrEmpty(this._Target))
            {
                //Targets:
                //"_blank": Load the linked document into a new blank window. This window is not named. 
                //"_self": Default. Load the linked document into the window in which the link was clicked (the active window). 
                //"frame": In this case name of frame should be specified where the document to be loaded.
                if ((this._Target == "_self") || (this._Target == "_blank"))
                {
                    rdolTarget.SelectedIndex = rdolTarget.Items.IndexOf(rdolTarget.Items.FindByValue(this._Target));
                }
                else
                {
                    rdolTarget.SelectedIndex = rdolTarget.Items.IndexOf(rdolTarget.Items.FindByValue("frame"));
                    txtFrameName.Text = this._Target;
                }
            }
        }
        chkHideInNavigation.Checked = this._IsHiddenInNavigation;
    }
}
