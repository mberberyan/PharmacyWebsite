using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Exception;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    public partial class CodeBehind_DynamicPropDefinition : ProductCatalogControl
    {        

        #region Fields && Properties
        /// <summary>
        /// Contains table with dynamic property definitions
        /// </summary>
        /// <remarks>
        /// Table contains all dynamic property definitions in the system. It has the following rows:
        /// <list type="bullet">
        /// <item>propId - identifier of property definition</item>
        /// <itemm>propName - name of property identifier</itemm>
        /// </list>
        /// </remarks>
        public DataTable DynPropDefTable
        {
            get { return (DataTable)ViewState["__mc_productcatalog_dynPropDefTable"]; }
            set { ViewState["__mc_productcatalog_dynPropDefTable"] = value; }
        }
        
        #endregion

        /// <summary>
        /// Set user control property values passed from control`s caller page
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInit(EventArgs e)
        {
            btnAddDynamicProp.Click += new EventHandler(btnAddDynamicProp_Click);
            gvDynamicProps.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(gvDynamicProps_RowCommand);
            gvDynamicProps.RowEditing += new GridViewEditEventHandler(gvDynamicProps_RowEditing);
            gvDynamicProps.RowDataBound += new GridViewRowEventHandler(gvDynamicProps_RowDataBound);            

            base.OnInit(e);
        }        

        /// <summary>
        /// Initializes user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                // TODO -> check when to load dynamic properties
                // because it is not needed to load them on every postback that happens
                if (SelectedObjectId != null && SelectedObjectId != 0)
                {
                    LoadDynamicProps();
                }
                
            }
        }

        /// <summary>
        /// Load dynamic property definitions in DropDownList control.
        /// </summary>
        /// <remarks>
        /// This method gets all dynamic property definitions and loads DropDownList control 
        /// for listing dynamic property definitions.After loading all property names, those that are already added
        /// to the current object are exluded from DropDownList control. Exluded dynamic properties are located in
        /// <see cref="DynPropDefTable"/> table.
        /// </remarks>
        private void LoadDynamicProps()
        {
            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Category:
                    // Load 'Dynamic Properties' table from current category
                    DataView dvPropDefTable = new DataView(DynPropDefTable);
                    dvPropDefTable.Sort = "propName ASC";
                    gvDynamicProps.DataSource = dvPropDefTable;
                    gvDynamicProps.DataBind();
    
                    // Load dropdown control with all dynamic properties` definitions
                    // except those already added for current category
                    DataTable dtPropDef = PropertyDefinition.List(new PropertyDefinition());
                    DataView dvPropDef = new DataView(dtPropDef);
                    dvPropDef.Sort = "propName ASC";
                    ddlPropDef.DataSource = dvPropDef;
                    ddlPropDef.DataBind();
                    ddlPropDef.Items.Insert(0, new ListItem(GetLocalResourceObject("SelectItem").ToString(), ""));

                    // exclude already added dynamic properties from property definition list
                    foreach (DataRow row in DynPropDefTable.Rows)
                    {
                        if (ddlPropDef.Items.Contains(new ListItem(row["propName"].ToString())))
                        {
                            ddlPropDef.Items.Remove(row["propName"].ToString());
                        }
                    }

                    rfvDdlDynamicProp.Enabled = false;

                    break;
                case ComponentObjectEnum.Product:
                case ComponentObjectEnum.Bundle:                    
                    

                    DataView dvProductPropDefTable = new DataView(DynPropDefTable);
                    dvProductPropDefTable.Sort = "propName ASC";
                    gvDynamicProps.DataSource = dvProductPropDefTable;
                    gvDynamicProps.DataBind();

                    // Load dropdown control with all dynamic properties` definitions of current object`s categories                    
                    DataTable dtPropDefList = new DataTable();


                    if (SelectedObjectType == ComponentObjectEnum.Product && SelectedProductId != null)
                    {
                        dtPropDefList = Product.GetPropertyList(SelectedProductId.Value);
                    }
                    else if (SelectedObjectType == ComponentObjectEnum.Bundle && SelectedObjectId != null)
                    {
                        dtPropDefList = Bundle.GetPropertyList(SelectedObjectId.Value);
                    }

                    if (dtPropDefList.Rows.Count > 0)
                    {
                        DataView dvProductPropDef = new DataView(dtPropDefList);
                        dvProductPropDef.Sort = "propName ASC";
                        ddlPropDef.DataSource = dvProductPropDef;
                        ddlPropDef.DataBind();
                        ddlPropDef.Items.Insert(0, new ListItem(GetLocalResourceObject("SelectItem").ToString(), ""));
                    }

                    // exclude already added dynamic properties from property definition list
                    foreach (DataRow row in DynPropDefTable.Rows)
                    {
                        if (ddlPropDef.Items.Contains(new ListItem(row["propName"].ToString())))
                        {
                            ddlPropDef.Items.Remove(row["propName"].ToString());
                        }
                    }
                    
                    // hide textbox and the respective required field validator for adding dynamic properties
                    // when user control is opened for Products or Bundles
                    txtDynamicProp.Style.Remove("display");
                    txtDynamicProp.Style.Add("display", "none");
                    rfvDynamicProp.Visible = false;
                    rfvDdlDynamicProp.Enabled = true;

                    break;
                default:
                    break;
            }            
        }

        /// <summary>
        /// Add dynamic property definition to the current object
        /// </summary>
        /// <remarks>
        /// Adds dynamic property definition for the selected object.
        /// After saving property name to the object`s dynamic properties collection,
        /// <see cref="Melon.Components.ProductCatalog.ComponentEngine.OnLoadCategory" /> method is called.
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>09/10/2009</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddDynamicProp_Click(object sender, EventArgs e)
        {
            switch (SelectedObjectType)
            { 
                case ComponentObjectEnum.Category:
                    CategoryProperty catProp = new CategoryProperty();
                    catProp.CategoryId = SelectedObjectId;
                    catProp.PropertyName = txtDynamicProp.Text.Trim();

                    try
                    {
                        catProp.Save();
                    }
                    catch (ProductCatalogException ex)
                    {
                        DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                        args.ErrorMessage = ex.Message;
                        this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);
                    }

                    LoadCategoryEventArgs catArgs = new LoadCategoryEventArgs();
                    catArgs.RefreshExplorer = false;
                    catArgs.SelectedCategoryId = SelectedObjectId;
                    catArgs.SelectedTab = SelectedTab;
                    catArgs.SelectedObjectType = ComponentObjectEnum.Category;
                    this.BaseParentControl.OnLoadCategory(sender, catArgs);

                    break;
                case ComponentObjectEnum.Product:                                        
                    try
                    {
                        ProductProperty.Save(SelectedProductId, ddlPropDef.SelectedValue);
                    }
                    catch (ProductCatalogException ex)
                    {
                        DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                        if (ex.Code == ProductCatalogExceptionCode.ProductPropertyNotFound)
                        {
                            args.ErrorMessage = String.Format((new ProductCatalogException(ProductCatalogExceptionCode.ProductPropertyNotFound)).Message, ex.AdditionalInfo);
                        }
                        else
                        {
                            args.ErrorMessage = ex.Message;
                        }

                        this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);                        

                        break;
                    }

                    LoadProductEventArgs arg = new LoadProductEventArgs();
                    arg.SelectedCategoryId = SelectedObjectId;
                    arg.SelectedProductId = SelectedProductId;
                    arg.SelectedObjectType = ComponentObjectEnum.Product;
                    arg.SelectedTab = SelectedTab;
                    arg.RefreshExplorer = true;
                    this.BaseParentControl.OnLoadProduct(sender, arg);

                    break;
                case ComponentObjectEnum.Bundle:
                    try
                    {
                        BundleProperty.Save(SelectedObjectId, ddlPropDef.SelectedValue);
                    }
                    catch (ProductCatalogException ex)
                    {
                        DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                        if (ex.Code == ProductCatalogExceptionCode.ProductPropertyNotFound)
                        {
                            args.ErrorMessage = String.Format((new ProductCatalogException(ProductCatalogExceptionCode.BundlePropertyNotFound)).Message, ex.AdditionalInfo);
                        }
                        else
                        {
                            args.ErrorMessage = ex.Message;
                        }

                        this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);

                        break;
                    }

                    LoadBundleEventArgs argBundle = new LoadBundleEventArgs();
                    argBundle.SelectedBundleId = SelectedObjectId;                    
                    argBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                    argBundle.SelectedTab = SelectedTab;
                    argBundle.RefreshExplorer = true;
                    this.BaseParentControl.OnLoadBundle(sender, argBundle);

                    break;
            }            
        }

        /// <summary>
        ///  This method inserts, updates or deletes dynamic property definition name
        /// </summary>
        /// <remarks>
        /// Selected dynamic property definitions are manipulated depending on <see creaf="System.Web.UI.WebControl.GridViewCommandEventArgs"/> argument.
        /// It has four possible values:
        /// <list type="bullet">
        /// <item>Remove - remove dynamic property name from dynamic property list for selected object</item>
        /// <item>Edit - provides stub method when opening table in edit mode</item>
        /// <item>UpdatePropDef - updates dynamic property definition name</item>
        /// <item>CancelPropDef - cancels insert/update operation on dynamic property</item>
        /// </list>
        /// </remarks>
        /// <author>Melon Team</author>
        /// <date>09/10/2009</date>
        /// <param name="sender"></param>
        /// <param name="e">Contains the id of the dynamic property to be deleted</param>
        protected void gvDynamicProps_RowCommand(object sender, GridViewCommandEventArgs e)
        {            
            // Delete dynamic property definition from GridView
            if (e.CommandName == "Remove")
            {
                switch (SelectedObjectType)
                {
                    case ComponentObjectEnum.Category:
                        try
                        {
                            CategoryProperty.Delete(Convert.ToInt32(e.CommandArgument.ToString()));
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                        finally
                        {
                            LoadCategoryEventArgs catArgs = new LoadCategoryEventArgs();
                            catArgs.RefreshExplorer = false;
                            catArgs.SelectedCategoryId = SelectedObjectId;
                            catArgs.SelectedTab = SelectedTab;
                            catArgs.SelectedObjectType = ComponentObjectEnum.Category;
                            this.BaseParentControl.OnLoadCategory(sender, catArgs);
                        }

                        break;
                    case ComponentObjectEnum.Product:
                        try
                        {
                            ProductProperty.Delete(Convert.ToInt32(e.CommandArgument.ToString()));
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                        finally
                        {
                            LoadProductEventArgs arg = new LoadProductEventArgs();
                            arg.SelectedCategoryId = SelectedObjectId;
                            arg.SelectedProductId = SelectedProductId;
                            arg.SelectedObjectType = ComponentObjectEnum.Product;
                            arg.SelectedTab = SelectedTab;
                            arg.RefreshExplorer = true;
                            this.BaseParentControl.OnLoadProduct(sender, arg);
                        }

                        break;
                    case ComponentObjectEnum.Bundle:
                        try
                        {
                            BundleProperty.Delete(Convert.ToInt32(e.CommandArgument.ToString()));
                        }
                        catch (ProductCatalogException ex)
                        {
                            DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                            args.ErrorMessage = ex.Message;
                            this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);

                            return;
                        }
                        finally
                        {
                            LoadBundleEventArgs argBundle = new LoadBundleEventArgs();
                            argBundle.SelectedBundleId = SelectedObjectId;                            
                            argBundle.SelectedObjectType = ComponentObjectEnum.Bundle;
                            argBundle.SelectedTab = SelectedTab;
                            argBundle.RefreshExplorer = true;
                            this.BaseParentControl.OnLoadBundle(sender, argBundle);
                        }

                        break;
                }
            }
            else if (e.CommandName == "Edit")
            {

            }
            else if (e.CommandName == "UpdatePropDef") // Update dynamic property definition name
            {
                CategoryProperty catProp = new CategoryProperty();
                catProp.Id = Convert.ToInt32(e.CommandArgument);

                try
                {
                    catProp.Load();
                }
                catch (ProductCatalogException ex)
                {
                    DisplayErrorMessageEventArgs argsError = new DisplayErrorMessageEventArgs();
                    argsError.ErrorMessage = ex.Message;
                    this.BaseParentControl.OnDisplayErrorMessageEvent(sender, argsError);
                    return;
                }
                catch (System.Exception exception)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = exception.Message;
                    this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);
                    return;
                }

                catProp.PropertyName = ((TextBox)gvDynamicProps.Rows[gvDynamicProps.EditIndex].FindControl("txtDynamicPropEdit")).Text;

                try
                {
                    catProp.Save();
                }
                catch (ProductCatalogException ex)
                {
                    DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                    args.ErrorMessage = ex.Message;
                    this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);

                    return;
                }

                gvDynamicProps.EditIndex = -1;

                LoadCategoryEventArgs catArgs = new LoadCategoryEventArgs();
                catArgs.RefreshExplorer = false;
                catArgs.SelectedCategoryId = SelectedObjectId;
                catArgs.SelectedTab = SelectedTab;
                catArgs.SelectedObjectType = ComponentObjectEnum.Category;
                this.BaseParentControl.OnLoadCategory(sender, catArgs);
            }
            else if (e.CommandName == "CancelPropDef") // Cancel editing dynamic property definition name
            {
                gvDynamicProps.EditIndex = -1;
                LoadDynamicProps();
            }
        }

        /// <summary>
        /// Open dynamic property definitions table in edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDynamicProps_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Category cat = Melon.Components.ProductCatalog.Category.Load(SelectedObjectId);
            if (cat == null)
            {
                DisplayErrorMessageEventArgs args = new DisplayErrorMessageEventArgs();
                args.ErrorMessage = Convert.ToString(GetLocalResourceObject("CategoryNotExistErrorMessage"));
                this.BaseParentControl.OnDisplayErrorMessageEvent(sender, args);

                return;
            }

            gvDynamicProps.EditIndex = Convert.ToInt32(e.NewEditIndex);
            LoadDynamicProps();

        }

        /// <summary>
        /// Sets accessibility of action buttons in table where dynamic property definitions are listed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Set visiblity of action buttons in dynamic property definition table:
        /// <list type="bullet">
        /// <item>btnRemove  - calls event to remove selected dynamic property definition from property list for selected object</item>
        /// <item>btnEdit - opens table in edit mode so as to change dynamic property name</item>        
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDynamicProps_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (SelectedObjectType == ComponentObjectEnum.Category)
                {                    
                    ((Label)e.Row.FindControl("lblInheritedCategory")).Text = drv["catName"].ToString();

                    if (Convert.ToInt32((drv)["catId"]) != SelectedObjectId)
                    {
                        ((Button)e.Row.FindControl("btnRemove")).Enabled = false;
                        ((Button)e.Row.FindControl("btnEdit")).Enabled = false;
                        ((Button)e.Row.FindControl("btnRemove")).CssClass = "mc_pc_button mc_pc_btn_61 mc_pc_disabled_button";
                        ((Button)e.Row.FindControl("btnEdit")).CssClass = "mc_pc_button mc_pc_btn_61 mc_pc_disabled_button";
                    }
                    else
                    {
                        ((Button)e.Row.FindControl("btnRemove")).CssClass = "mc_pc_button mc_pc_btn_61";
                        ((Button)e.Row.FindControl("btnEdit")).CssClass = "mc_pc_button mc_pc_btn_61";
                    }
                }
                else
                {
                    ((Button)e.Row.FindControl("btnEdit")).Visible = false;
                }

                
            }
        }        
    }
}
