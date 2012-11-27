using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.ProductCatalog.Configuration;
using Melon.Components.ProductCatalog.ComponentEngine;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Display message to user for his current rights in the Product Catalog system.
    /// </summary>
    public partial class CodeBehind_AccessDenied : ProductCatalogControl
    {        
        #region Properties

        public bool IsUserLoggedRole;

        #endregion        

        protected override void OnInit(EventArgs e)
        {
            btnLogin.Click+=new EventHandler(btnLogin_Click);
            base.OnInit(e);
        }

        /// <summary>
        /// Display message to user for his current rights in the Product Catalog system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                if (String.IsNullOrEmpty(ProductCatalogSettings.LoginUrl) || this.ResolveUrl(ProductCatalogSettings.LoginUrl) == Request.RawUrl)
                {
                    btnLogin.Visible = false;
                    lblInstruction.Visible = false;
                }
                else
                {
                    btnLogin.Visible = true;
                    lblInstruction.Visible = true;
                }

                if (IsUserLoggedRole)
                {
                    lblMessage.Text = Convert.ToString(GetLocalResourceObject("AccessDenied"));
                }                
            }
        }

        /// <summary>
        /// Sign out current user and redirect to login pages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Melon Team</author>
        /// <date>09/1/2009</date>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            this.ParentControl.RedirectToLoginPage();
        }

		/// <summary>
		/// Initializes the control's properties
		/// </summary>
		/// <param name="args">The values with which the properties will be initialized</param>
		public override void Initializer(object[] args)
        {			
			this.IsUserLoggedRole = (bool)args[0];
        }
    }
}
