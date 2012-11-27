using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Exception;
using System.Web;
using Melon.Components.CMS;


/// <summary>
/// Contains layout for displaying node's template and its placeholders.
/// </summary>
///     <para>
///     All web controls from SettingsBoxTemplate are using local resources.
///     To customize them modify resource file SettingsBoxTemplate.resx located in the MC_CMS folder.
///     </para>
public partial class SettingsBoxTemplate : SettingsControl
{
    #region Fields & Properties

    private int? _TemplateId;
    /// <summary>
    /// Identifier of the content-manageable page's template that is opened for update.
    /// </summary>
    public int? TemplateId
    {
        get { return (String.IsNullOrEmpty(ddlTemplates.SelectedValue))? (int?)null :Convert.ToInt32(ddlTemplates.SelectedValue); }
        set { this._TemplateId = value; }
    }

    /// <summary>
    /// All content placeholders of the selected in DropDown ddlTemplates template.
    /// </summary>
    protected List<string> lstPlaceholders
    {
        get
        {
            if (ViewState["__mc_cms_Placehodlers"] != null)
            {
                return (List<string>)ViewState["__mc_cms_Placehodlers"];
            }
            else
            {
                return null;
            }
        }
        set
        {
            ViewState["__mc_cms_Placehodlers"] = value;
        }
    }

    #endregion

    /// <summary>
    /// Attach event handlers to the controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.ddlTemplates.SelectedIndexChanged += new EventHandler(ddlTemplates_SelectedIndexChanged);
        this.repPlaceholders.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(repPlaceholders_ItemDataBound);
        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     If user control is loaded for first time, method <see cref="LoadTemplates"/> is called to 
    ///     load all existing cms templates in DropDown ddlTemplates.</para>
    ///     <para>
    ///         If the user control is loaded in context of updating content-manageable page 
    ///         then its template is selected in DropDown ddlTemplates and method <see cref="LoadSelectedTemplatePlaceholders"/>
    ///         is called to display template's placeholders in Repeater repPlaceholders.</para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>10/03/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            if (this.CurrentNodeType == NodeType.ContentManageablePage)
            {
                LoadTemplates();
                if (this._TemplateId != null)
                {
                    ddlTemplates.SelectedIndex = ddlTemplates.Items.IndexOf(ddlTemplates.Items.FindByValue(Convert.ToString(this._TemplateId.Value)));
                }
                lstPlaceholders = LoadSelectedTemplatePlaceholders();
            }
        }
    }


    /// <summary>
    /// Event handler for event ReLoadSettings of the user control inherited from 
    /// parent user control <see cref="Melon.Components.CMS.ComponentEngine.SettingsControl"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///        Selection of the template in the DropDown ddlTemplates is remade.
    ///        Method <see cref="LoadSelectedTemplatePlaceholders"/> is called to load the placeholders of 
    ///        the currently selected template.</para>
    ///     <para>
    ///         The event is fired from the parent user control Settings.ascx 
    ///         when node settings are restored from live version. In this case is loaded 
    ///         the template of the content-manageable page from its live version.</para>
    /// </remarks>
    /// <param name="args"></param>
    /// <author></author>
    /// <date>26/03/2008</date>
    protected override void OnReLoadSettings(EventArgs args)
    {
        base.OnReLoadSettings(args);
        if (this._TemplateId != null)
        {
            ddlTemplates.SelectedIndex = ddlTemplates.Items.IndexOf(ddlTemplates.Items.FindByValue(Convert.ToString(this._TemplateId.Value)));
        }
        LoadSelectedTemplatePlaceholders();
    }

    /// <summary>
    /// Event handler for event SelectedIndexChanged of DropDown ddlTemplates.
    /// </summary> 
    /// <remarks>
    ///     Calls method <see cref="LoadSelectedTemplatePlaceholders"/> to display in Repeater repPlaceholders 
    ///     the names (identifiers) of the content placeholders of the selected template.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/03/2008</date>
    protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstPlaceholders = LoadSelectedTemplatePlaceholders();
    }

    /// <summary>
    /// Event handler for event ItemDataBound of Repeater repPlaceholders.
    /// </summary>
    /// <remarks>
    ///     Used to set placeholders details in the Repeater repPlaceholders.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>18/03/2008</date>
    protected void repPlaceholders_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            Label lblPlaceholderName = (Label)e.Item.FindControl("lblPlaceholderName");
            lblPlaceholderName.Text = Convert.ToString(e.Item.DataItem);
        }
    }

    /// <summary>
    /// Load all existing CMS templates in DropDown ddlTemplates.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="Melon.Components.CMS.Template.List"/> to retrieve all cms templates
    ///     and databind DropDown ddlTemplates with the returned templates.
    /// </remarks>
    /// <author></author>
    /// <date>11/03/2008</date>
    private void LoadTemplates()
    {
        DataTable dtTemplates = Template.List(new Template());

        ddlTemplates.DataSource = dtTemplates;
        ddlTemplates.DataBind();
    }

    /// <summary>
    /// Loads the placeholders of the selected template in Repeater repPlaceholders.    
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="Melon.Components.CMS.Template.Load"/> to retrieve details for the selected template
    ///     and then databind Repeater repPlaceholders with its placeholders. 
    /// </remarks>
    /// <returns></returns>
    /// <author></author>
    /// <date>18/03/2008</date>
    private List<string> LoadSelectedTemplatePlaceholders()
    {
        List<string> placeholders = new List<string>();
        if (ddlTemplates.Items.Count != 0 && ddlTemplates.SelectedIndex != -1)
        {
            int templateId = Convert.ToInt32(ddlTemplates.SelectedValue);
            Template objTemplate = Template.Load(templateId);

            
            string masterPagePhysicalPath = Server.MapPath(objTemplate.MasterPage);
            placeholders = Template.GetMasterPagePlaceholders(masterPagePhysicalPath);

            repPlaceholders.DataSource = placeholders;
            repPlaceholders.DataBind();
        }
        return placeholders;
    }
}
