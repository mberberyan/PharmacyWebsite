using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Melon.Components.CMS;
using Melon.Components.CMS.ComponentEngine;
using Melon.Components.CMS.Configuration;
using Melon.Components.CMS.Exception;
using System.Web.Security;



/// <summary>
/// Provides interface for listing templates.
/// </summary>
/// <remarks>
/// <para>
///     The TemplateList user control is listing all templates existing in the CMS system.
///     To retrieve them is called method <see cref="Template.List(Template)"/> of class <see cref="Template"/>. 
///     The found templates are displayed in GridView gvTemplates.
/// </para>
///	<para>
///		The GridView gvTemplates has the following columns:
///			<list type="table">
///				<item><term>Name</term><description>Name of the template.</description></item>
///				<item><term>Master Page</term><description>Virtual path of the master page used for template.</description></item>
///				<item><term>Placeholders</term><description>All content placeholders of the template.</description></item>
///				<item><term>Used By</term><description>Number of pages which are currently using the template.</description></item>
///				<item><term>Column with actions</term>
///             <description>Actions edit and delete are available.
///                 <para>When edit is performed event LoadTemplateAddEditEvent is raised of the main user control to load interface for editing template.</para>
///                 <para>When delete is performed method <see cref="Template.Delete(int)"/> of class <see cref="Template"/> is called to perform deletion.</para>
///             </description></item>
///			</list>
///	</para>
/// <para>
///     The results in the GridView could be sorted. When sorting is performed icon appears in the header 
///     of the sorted column which indicate what is the sorting direction: ascending or descending.
/// </para>
/// <para>
///     There is paging in of the results in the GridView. For the purpose is used Pager control at the top of the GridView.
///     The number of the result to be displayed on page is configurable from configuration section &lt;cms /&gt; attribute templatesPageSize in the web.config.
/// </para>
/// <para>
///     All web controls from TemplateList are using local resources.
///     To customize them modify resource file TemplateList.resx located in the MC_CMS folder.
/// </para>
/// </remarks>
/// <seealso cref="Template"/>
public partial class TemplateList : CMSControl
{
    #region Fields & Properties

    /// <summary>
    /// Sort direction of the currently sorted column in the GridView with templates gvTemplates.
    /// It is "ASC" for ascending sorting and "DESC" for descending sorting. 
    /// </summary>
    public string SortDirection
    {
        get
        {
            if (ViewState["__mc_cms_sortDirection"] != null)
            {
                return ViewState["__mc_cms_sortDirection"].ToString();
            }
            else
            {
                return "ASC";
            }
        }
        set
        {
            ViewState["__mc_cms_sortDirection"] = value;
        }
    }

    /// <summary>
    /// Sort expression of the currently sorted column in the GridView with templates gvTemplates.
    /// </summary>
    public string SortExpression
    {
        get
        {
            if (ViewState["__mc_cms_sortExpression"] != null)
            {
                return ViewState["__mc_cms_sortExpression"].ToString();
            }
            else
            {
                return "PagesCount";
            }
        }
        set
        {
            ViewState["__mc_cms_sortExpression"] = value;
        }
    }

    #endregion

    /// <summary>
    /// Attach event handlers for controls' events.
    /// </summary>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>22/02/2008</date>
    protected override void OnInit(EventArgs e)
    {
        this.gvTemplates.RowCreated += new GridViewRowEventHandler(gvTemplates_RowCreated);
        this.gvTemplates.RowDataBound += new GridViewRowEventHandler(gvTemplates_RowDataBound);
        this.gvTemplates.Sorting += new GridViewSortEventHandler(gvTemplates_Sorting);
        this.TopPager.PageChanged += new Pager.PagerEventHandler(Pager_PageChanged);
        this.btnCreateTemplate.Click += new EventHandler(btnCreateTemplate_Click);

        base.OnInit(e);
    }

    /// <summary>
    /// Initialize the user control.
    /// </summary>
    /// <remarks>
    ///     If control is loaded for first time calls method <see cref="ListTemplates"/> 
    ///     to retrieve all templates and display them in GridView gvTemplates.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>22/02/2008</date>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsControlPostBack)
        {
            ListTemplates();
        }
    }


    /// <summary>
    /// Event handler for event RowCreated of GridView gvTemplates.
    /// </summary>
    /// <remarks>
    ///     Attach event handlers to the buttons btnEditTemplate, btnRefreshTemplate, btnDeleteTemplate 
    ///     and repeater repPlaceholders in GridView gvTemplates.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>20/02/2008</date>
    protected void gvTemplates_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Repeater repPlaceholders = (Repeater)e.Row.FindControl("repPlaceholders");
            repPlaceholders.ItemDataBound += new RepeaterItemEventHandler(repPlaceholders_ItemDataBound);

            LinkButton lbtnEditTemplate = (LinkButton)e.Row.FindControl("lbtnEditTemplate");
            lbtnEditTemplate.Command += new CommandEventHandler(lbtnEditTemplate_Command);

            LinkButton lbtnDeleteTemplate = (LinkButton)e.Row.FindControl("lbtnDeleteTemplate");
            lbtnDeleteTemplate.Command += new CommandEventHandler(lbtnDeleteTemplate_Command);
        }
    }

    /// <summary>
    /// Event handler for event RowDataBound of GridView gvTemplates.
    /// </summary>
    /// <remarks>
    ///     Used to set template details in the columns of GridView gvTemplates.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>19/02/2008</date>
    protected void gvTemplates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            //Set name.
            Label lblName = (Label)e.Row.FindControl("lblName");
            lblName.Text = Server.HtmlEncode(Convert.ToString(drv["Name"]));

            //Set master page.
            Label lblMasterPage = (Label)e.Row.FindControl("lblMasterPage");
            lblMasterPage.Text = Convert.ToString(drv["MasterPage"]);

            //Set placeholders.
            Repeater repPlaceholders = (Repeater)e.Row.FindControl("repPlaceholders");
            repPlaceholders.DataSource = Template.GetMasterPagePlaceholders(Server.MapPath(Convert.ToString(drv["MasterPage"])));
            repPlaceholders.DataBind();

            //Set number of pages using the template.
            Label lblPagesCount = (Label)e.Row.FindControl("lblPagesCount");
            lblPagesCount.Text = String.Format(Convert.ToString(GetLocalResourceObject("PagesCount")), Convert.ToString(drv["PagesCount"]));

            //Set buttons command arguments.
            LinkButton lbtnEditTemplate = (LinkButton)e.Row.FindControl("lbtnEditTemplate");
            lbtnEditTemplate.CommandArgument = Convert.ToString(drv["Id"]);

            LinkButton lbtnDeleteTemplate = (LinkButton)e.Row.FindControl("lbtnDeleteTemplate");
            lbtnDeleteTemplate.CommandArgument = Convert.ToString(drv["Id"]) + ";" + Convert.ToString(drv["Name"]);
        }
    }

    /// <summary>
    /// Event handler for event Sorting of GridView gvTemplates.
    /// </summary>
    /// <remarks>
    ///     Save in view state the new sorting direction and expression 
    ///     and calls method <see cref="ListTemplates"/> to perform sorting of the templates in GridView gvTemplates.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>22/02/2008</date>
    protected void gvTemplates_Sorting(object sender, GridViewSortEventArgs e)
    {
        string newSortExpression = e.SortExpression;
        if (this.SortExpression == newSortExpression)
        {
            //If the old sort expression is the same as the new sort expression, we invert the sort direction.
            this.SortDirection = (this.SortDirection == "ASC") ? "DESC" : "ASC";
        }
        else
        {
            //We sort by new column, so set the sorting direction to be acsending.
            this.SortExpression = newSortExpression;
            this.SortDirection = "ASC";
        }

        ListTemplates();
    }

    /// <summary>
    /// Event handler for event PageChange of user control Pages.ascx.
    /// </summary>
    /// <remarks>
    ///     Set property PageIndex of GridView gvTemplates to the number of the new page and
    ///     calls method <see cref="ListTemplates"/> to perform paging.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>22/02/2008</date>
    protected void Pager_PageChanged(object sender, Pager.PagerEventArgs e)
    {
        gvTemplates.PageIndex = e.NewPage;
        ListTemplates();
    }


    /// <summary>
    /// Event handler for event ItemDataBound of Repeater repPlaceholders.
    /// </summary>
    /// <remarks>
    ///     Used to set content placeholder id in Repeater repPlaceholders.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>20/02/2008</date>
    protected void repPlaceholders_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || ((e.Item.ItemType == ListItemType.AlternatingItem)))
        {
            Label lblPlaceholderName = (Label)e.Item.FindControl("lblPlaceholderName");
            lblPlaceholderName.Text = Convert.ToString(e.Item.DataItem);
        }
    }


    /// <summary>
    /// Event handler for event Click of Button btnCreateTemplate.
    /// </summary>
    /// <remarks>
    ///     Raises event LoadTemplateAddEditEvent of parent user control for loading CMSTemplateAddEdit.ascx 
    ///     in the context of creating template.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void btnCreateTemplate_Click(object sender, EventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator)
            {
                LoadTemplateAddEditEventArgs args = new LoadTemplateAddEditEventArgs();
                this.ParentControl.OnLoadTemplateAddEditEvent(sender, args);
            }
            else
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole = false;
                args.UserRole = currentUserRole;
                this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Event handler for event Click of LinkButton lbtnEditTemplate.
    /// </summary>
    /// <remarks>
    ///     Raises event LoadTemplateAddEditEvent of parent user control for loading CMSTemplateAddEdit.ascx 
    ///     in the context of edit template.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void lbtnEditTemplate_Command(object sender, CommandEventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator)
            {
                int templateId = Convert.ToInt32(e.CommandArgument);
                LoadTemplateAddEditEventArgs args = new LoadTemplateAddEditEventArgs(templateId);
                this.ParentControl.OnLoadTemplateAddEditEvent(sender, args);
            }
            else
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole = false;
                args.UserRole = currentUserRole;
                this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }

    /// <summary>
    /// Event handler for event Click of all linnk buttons with name lbtnDeleteTemplate in GridView gvTemplates.
    /// </summary>
    /// <remarks>
    ///     Calls method <see cref="Melon.Components.CMS.Template.Delete(int)"/> to delete the template
    ///     If the deletion is successful then event LoadTemplateListEvent of parent user controls is raised
    ///     to reload data in the GridView gvTemplates. 
    ///     <para>
    ///     In case of error,event for diplaying error message of the parent user control is raised.
    ///     </para>
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <author></author>
    /// <date>25/02/2008</date>
    protected void lbtnDeleteTemplate_Command(object sender, CommandEventArgs e)
    {
        if (this.ParentControl.CurrentUser != null)
        {
            CMSRole currentUserRole = User.GetCMSRole(this.ParentControl.CurrentUser.UserName);
            if (currentUserRole == CMSRole.SuperAdministrator)
            {
                string[] args = (Convert.ToString(e.CommandArgument)).Split(';');
                int templateId = Convert.ToInt32(args[0]);
                string templateName = args[1];

                try
                {
                    Template.Delete(templateId);
                }
                catch (CMSException ex)
                {
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();

                    if (ex.Code == CMSExceptionCode.TemplateIsUsedByPage)
                    {
                        errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("TemplateIsUsedByPageErrorMessage")), templateName);
                    }
                    else
                    {
                        errorArgs.ErrorMessage = ex.Message;
                    }

                    ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    ParentControl.OnLoadTemplateListEvent(sender, new LoadTemplateListEventArgs());
                    return;
                }
                catch
                {
                    //Error during deletion. Display error message.
                    DisplayErrorMessageEventArgs errorArgs = new DisplayErrorMessageEventArgs();
                    errorArgs.ErrorMessage = String.Format(Convert.ToString(GetLocalResourceObject("DeleteTemplateErrorMessage")), templateName);
                    ParentControl.OnDisplayErrorMessageEvent(sender, errorArgs);

                    ParentControl.OnLoadTemplateListEvent(sender, new LoadTemplateListEventArgs());
                    return;
                }

                //Successful delete. Bind the grid again bacause the deleted tempplate should be removed from it.
                ParentControl.OnLoadTemplateListEvent(sender, new LoadTemplateListEventArgs());
            }
            else
            {
                LoadAccessDeniedEventArgs args = new LoadAccessDeniedEventArgs();
                args.IsUserLoggedRole = false;
                args.UserRole = currentUserRole;
                this.ParentControl.OnLoadAccessDeniedEvent(sender, args);
            }
        }
        else
        {
            this.ParentControl.RedirectToLoginPage();
        }
    }


    /// <summary>
    /// Retrieves all existing templates and display them in GridView gvTemplates.
    /// </summary>
    /// <remarks>
    ///     The method calls static method List of class <see cref="Melon.Components.CMS.Template"/> 
    ///     to retrieve the templates. After that GridView gvTemplates is databound with the returned templates.
    /// </remarks>
    /// <author></author>
    /// <date>22/02/2008</date>
    private void ListTemplates()
    {
        //Retrive templates.
        DataTable dtTemplates = Template.List(new Template());

        //Display details of temapltes in GridView gvTemplates.
        DataView dvTemplates = new DataView(dtTemplates);
        dvTemplates.Sort = this.SortExpression + " " + this.SortDirection;

        gvTemplates.PageSize = CMSSettings.TemplatesPageSize;
        gvTemplates.DataSource = dvTemplates;
        gvTemplates.DataBind();

        //Display paging if there are users found.
        if (dtTemplates.Rows.Count != 0)
        {
            tblEmptyDataTemplate.Visible = false;
            TopPager.FillPaging(gvTemplates.PageCount, gvTemplates.PageIndex + 1, 5, gvTemplates.PageSize, dtTemplates.Rows.Count);
        }
        else
        {
            tblEmptyDataTemplate.Visible = true;
            TopPager.Visible = false;
        }
    }
}
