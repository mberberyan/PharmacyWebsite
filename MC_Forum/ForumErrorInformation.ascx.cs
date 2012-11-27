using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.Forum.ComponentEngine;

namespace Melon.Components.Forum.UI.CodeBehind
{
    /// <summary>
    /// Screen with error message.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     The control ForumErrorInformation loads error information in case the forum control is loaded
    ///     with incorrect URL. By incorrect URL is meant the incorrect format 
    ///     of the forum component parts added after the slash in the URL.
    ///     This check is made in the main user control <see cref="Forum"/> when the control is loaded for first time.
    ///     </para>
    ///     <para>
    ///     Property <see cref="Message"/> contains the error information which will be displayed in the interface.
    ///     </para>
    ///</remarks>
    public partial class ForumErrorInformation : ForumControl
    {
        /// <summary>
        /// Error information.
        /// </summary>
        public new string Message;

        /// <summary>
        /// Initializes the control's properties.
        /// </summary>
        /// <param name="args">The values with which the properties will be initialized.</param>
        public override void Initializer(object[] args)
        {
            this.Message = (string)args[0];
        }

        /// <summary>
        /// Display error message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                lblMessage.Text = Message;
            }
        }
    }
}
