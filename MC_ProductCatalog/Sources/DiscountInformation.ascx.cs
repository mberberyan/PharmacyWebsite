using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using Melon.Components.ProductCatalog.Enumerations;
using System.Web.UI.WebControls;
using Melon.Components.ProductCatalog.Configuration;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing discount details in the system.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Discounts are applied on products as they alter product`s price for a definite period of time.
    /// Discounts values change product`s price with a precise amount or with a percent decrease of the common price.
    /// For more information on discount type see <see cref="melon.Components.ProductCatalog.Enumerations.DiscountTypeEnum"/>
    /// </para>    
    /// </remarks>
    public partial class CodeBehind_DiscountInformation : ProductCatalogControl
    {        

        #region Fields && Properties
        private string _Name;
        /// <summary>
        /// Name of loaded discount object
        /// </summary>
        public string Name
        {
            get { return txtName.Text.Trim(); }
            set { _Name = value; }
        }        

        private string _Description;
        /// <summary>
        /// Description of loaded discount object
        /// </summary>
        public string Description
        {
            get { return txtDescription.Text.Trim(); }
            set { _Description = value; }
        }

        private DiscountTypeEnum _DiscountType = DiscountTypeEnum.Percent;
        /// <summary>
        /// Discount type of loaded discount object
        /// </summary>
        /// <remarks>
        /// Discount values are measured in one of the two supported types - percent and price value.
        /// These two types are pre-defined in <see cref="Melon.Components.ProductCatalog.Enumerations.DiscountTypeEnum"/>.
        /// </remarks>
        public DiscountTypeEnum DiscountType
        {
            get { return (DiscountTypeEnum)rblDiscountIn.SelectedIndex; }
            set { _DiscountType = value; }
        }

        private DateTime? _DiscountFrom;
        /// <summary>
        /// Start date of applied discount validity
        /// </summary>
        public DateTime? DiscountFrom
        {
            get { return Convert.ToDateTime(txtDiscountFrom.Text.Trim()); }
            set { _DiscountFrom = value; }
        }

        private DateTime? _DiscountTo;
        /// <summary>
        /// End date of applied discount validity
        /// </summary>        
        public DateTime? DiscountTo
        {
            get { return Convert.ToDateTime(txtDiscountTo.Text.Trim()); }
            set { _DiscountTo = value; }
        }

        private decimal? _DiscountValue;
        /// <summary>
        /// The price value or percent value that common product price is decreased with.
        /// </summary>
        public decimal? DiscountValue
        {
            get { return Convert.ToDecimal(txtDiscountValue.Text.Trim()); }
            set { _DiscountValue = value; }
        }

        private bool? _IsActive = false;
        //Flag whether discount should be applied to products
        public bool? IsActive
        {
            get { return chkIsActive.Checked; }
            set { _IsActive = value; }
        }

        private string _Message;
        /// <summary>
        /// Success message displayed after successfull discount save operation
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        #endregion

        /// <summary>
        /// Initializes user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                // if creating new discount, then set focus on first textbox
                if (SelectedObjectId == null || SelectedObjectId == 0)
                {
                    txtName.Focus();
                }

                ibtnOpenCalendarDiscountFrom.ImageUrl = Utilities.GetImageUrl(this.Page, "calendar.gif");
                ibtnOpenCalendarDiscountTo.ImageUrl = Utilities.GetImageUrl(this.Page, "calendar.gif");
                LoadDiscountList();
                LoadObjectDetails();
            }
        }

        /// <summary>
        /// Loads and initializes all discount types defined in the system
        /// </summary>
        /// <remarks>
        /// There are two predefined discount types:
        /// <list type="bullet">
        /// <item>precise value - decreases products common price with precise value amount</item>
        /// <item>percent value - decrease products common price with definite percentage</item>
        /// </list>        
        /// </remarks>
        private void LoadDiscountList()
        { 
            rblDiscountIn.Items.Add(new ListItem(DiscountTypeEnum.Percent.ToString(),"%"));
            rblDiscountIn.Items.Add(new ListItem(DiscountTypeEnum.Value.ToString(), ProductCatalogSettings.Currency));
            rblDiscountIn.Items[0].Attributes.Add("onclick", "javascript:SetDiscountTypeLabel(this)");
            rblDiscountIn.Items[1].Attributes.Add("onclick", "javascript:SetDiscountTypeLabel(this)");

            rblDiscountIn.Items[0].Selected = true;
        }

        /// <summary>
        /// Loads discount details information
        /// </summary>
        private void LoadObjectDetails()
        {
            txtName.Text = _Name;
            txtDescription.Text = _Description;
            rblDiscountIn.SelectedIndex = (int)_DiscountType;
            txtDiscountFrom.Text = _DiscountFrom!=null ? _DiscountFrom.Value.Date.ToString(ceDiscountFrom.Format) : "";            
            txtDiscountTo.Text = _DiscountTo != null ? _DiscountTo.Value.Date.ToString(ceDiscountTo.Format) : "";
            txtDiscountValue.Text = _DiscountValue != null ? _DiscountValue.ToString() : "";
            lblDiscountType.Text = _DiscountType == DiscountTypeEnum.Percent ? "%" : ProductCatalogSettings.Currency;
            chkIsActive.Checked = _IsActive.Value;

            divMessage.Visible = !String.IsNullOrEmpty(Message);
            divMessage.InnerHtml = Message;
        }
    }
}
