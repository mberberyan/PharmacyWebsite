using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page provides user interface for displaying and managing object details in the system.
    /// <remarks>
    /// This is the main user control where object details are displayed. 
    /// When creating new object, first object details on this page should be saved, 
    /// in order to be able to submit further object information - images, audio and video files, dynamic properties.
    /// </remarks>
    /// </summary>    
    public partial class CodeBehind_GeneralInformation : ProductCatalogControl
    {        

        #region Fields && Properties
        private string _Code;
        /// <summary>
        /// Code of loaded object
        /// </summary>
        public string Code
        {
            get { return txtCode.Text.Trim(); }
            set { _Code = value; }
        }

        private string _Name;
        /// <summary>
        /// Name of loaded object.
        /// </summary>
        public string Name
        {
            get { return txtName.Text.Trim(); }
            set { _Name = value; }
        }

        private string _ShortDesc;
        /// <summary>
        /// Short description of loaded object
        /// </summary>
        public string ShortDesc
        {
            get { return txtShortDesc.Text.Trim(); }
            set { _ShortDesc = value; }
        }

        private string _LongDesc;
        /// <summary>
        /// Long description of loaded object
        /// </summary>
        public string LongDesc
        {
            get { return txtLongDesc.Text.Trim(); }
            set { _LongDesc = value; }
        }

        private string _Tags;
        /// <summary>
        /// Tags of loaded object
        /// </summary>
        public string Tags
        {
            get { return txtTags.Text.Trim(); }
            set { _Tags = value; }
        }

        private int? _Unit;
        /// <summary>
        /// Measurement unit of loaded object
        /// </summary>
        public int? Unit
        {
            get { return String.IsNullOrEmpty(ddlMeasurementUnit.SelectedValue) ? (int?)null : Convert.ToInt32(ddlMeasurementUnit.SelectedValue); }
            set { _Unit = value; }
        }

        private bool? _IsActive=false;
        /// <summary>
        /// Flag whether loaded object should be visible in the front-end
        /// </summary>
        public bool? IsActive
        {
            get { return chkIsActive.Checked; }
            set { _IsActive = value; }
        }

        private bool? _IsInStock=false;
        /// <summary>
        /// Flag whether product is in stock.
        /// </summary>
        public bool? IsInStock
        {
            get { return chkIsInStock.Checked; }
            set { _IsInStock = value; }
        }

        private bool? _IsFeatured=false;
        /// <summary>
        /// Flag whether product is marked as featured and visible in Featured product user control in the front-end.
        /// </summary>
        public bool? IsFeatured
        {
            get { return chkIsFeatured.Checked; }
            set { _IsFeatured = value; }
        }

        private decimal? _CommonPrice;
        /// <summary>
        /// Common price of loaded product object
        /// </summary>
        public decimal? CommonPrice
        {
            get { return txtCommonPrice.Text.Trim()!="" ? Convert.ToDecimal(txtCommonPrice.Text.Trim()) : (decimal?)null; }
            set { _CommonPrice = value; }
        }

        private string _Manufacturer;
        /// <summary>
        /// Manufacturer of loaded object.
        /// </summary>
        public string Manufacturer
        {
            get { return txtManufacturer.Text.Trim(); }
            set { _Manufacturer = value; }
        }

        private List<int?> _CategoryIdList = new List<int?>();
        /// <summary>
        /// List of integeres with category identifiers to which the loaded object belong.
        /// </summary>
        public List<int?> CategoryIdList
        {
            get 
            {
                List<int?> list = new List<int?>();
                if (hfCategoryList.Value != String.Empty)
                {
                    foreach (string str in hfCategoryList.Value.Split(','))
                    {
                        list.Add(Convert.ToInt32(str));
                    }
                }

                return list.Count>0 ? list : new List<int?>();
            }
            set { _CategoryIdList = value; }
        }

        private List<string> _CategoryNameList = new List<string>();
        /// <summary>
        /// List of category names to which the loaded object belong.
        /// </summary>
        public List<string> CategoryNameList
        {
            get
            {
                List<string> list = new List<string>();
                if (hfCategoryName.Value != String.Empty)
                {
                    foreach (string str in hfCategoryName.Value.Split(','))
                    {
                        list.Add(str);
                    }
                }

                return list.Count > 0 ? list : new List<string>();
            }
            set { _CategoryNameList = value; }
        }

        private string _AppliedDiscounts;
        /// <summary>
        /// Comma-separated string of all discounts applied to the loaded product object.
        /// </summary>
        public string AppliedDiscounts
        {
            get { return lblAppliedDiscounts.Text; }
            set { _AppliedDiscounts = value; }
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
        /// <remarks>
        /// Different object details are displayed depending on objects type.
        /// </remarks>
        /// <seealso cref="Melon.Components.ProductCatalog.Enumerations.ComponentObjectEnum"/>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {           
            if (!IsControlPostBack)
            {
                if (SelectedObjectType != ComponentObjectEnum.Unknown)
                {
                    if (SelectedObjectType == ComponentObjectEnum.Category ||
                        SelectedObjectType == ComponentObjectEnum.Product)
                    {                        
                        LoadMeasurementUnits();
                        divMeasurementUnit.Visible = true;                        
                    }
                 
                    // This clause checks whether to load data into object controls.
                    // The object details will be loaded if the selected object is of any type except PRODUCT
                    // or if the selected object is of type PRODUCT and opened for edit
                    if (SelectedObjectId != null && SelectedObjectId != 0
                    && !(SelectedObjectType == ComponentObjectEnum.Product && SelectedProductId == null))
                    {
                        LoadObjectDetails();
                    }
                    else
                    {
                        // if General Informtion tab is opened for new object, then set cursor focus on first textbox
                        txtCode.Focus();
                    }

                    if (SelectedObjectType == ComponentObjectEnum.Bundle || SelectedObjectType == ComponentObjectEnum.Product)
                    {
                        divCategoryControl.Visible = true;
                        LoadCategoryList();
                        LoadCategoryDetails();
                    }

                    if (SelectedObjectType == ComponentObjectEnum.Product)
                    {
                        divProduct.Visible = true;                                                                       

                        if (SelectedProductId != null)
                        {
                            LoadProductDetails();
                        }                        
                    }

                    if (SelectedObjectType == ComponentObjectEnum.Bundle || SelectedObjectType==ComponentObjectEnum.Product)
                    {
                        divPrice.Visible = true;
                        txtCommonPrice.Text = this._CommonPrice.ToString();
                    }

                    if ((SelectedObjectType == ComponentObjectEnum.Bundle && SelectedObjectId != null && SelectedObjectId!=0) ||
                       (SelectedObjectType == ComponentObjectEnum.Product && SelectedProductId != null)
                       )
                    {
                        int? SelectedId = (int?)null;
                        if (SelectedObjectType == ComponentObjectEnum.Product)
                        {                            
                            SelectedId = SelectedObjectId;
                        }
                        else
                        {                            
                            SelectedId = FirstCategoryId;
                        }                                              
                    }
                }
            }            
        }

        /// <summary>
        /// Loads object details.
        /// </summary>        
        private void LoadObjectDetails()
        {
            // Load Category 'General Information' tab
            txtCode.Text = this._Code;
            txtName.Text = this._Name;
            txtShortDesc.Text = this._ShortDesc;
            txtLongDesc.Text = this._LongDesc;
            txtTags.Text = this._Tags;            
            chkIsActive.Checked = this._IsActive.Value;
            
            divMessage.Visible = !String.IsNullOrEmpty(Message);
            divMessage.InnerHtml = Message;            
        }

        /// <summary>
        /// Load object details for objects of type <value>ComponentObjectEnum.Product</value>.
        /// </summary>
        private void LoadProductDetails()
        {            
            chkIsInStock.Checked = this._IsInStock.Value;
            chkIsFeatured.Checked = this._IsFeatured.Value;            
            txtManufacturer.Text =(this._Manufacturer!=null) ? this._Manufacturer.ToString() : "";            
            lblAppliedDiscounts.Text = this._AppliedDiscounts;                       
        }

        /// <summary>
        /// Load object details for objects of type <value>ComponentObjectEnum.Category</value>.
        /// </summary>
        private void LoadCategoryDetails()
        {
            // load selected category ids in hfCategoryList hidden field control
            if (_CategoryIdList.Count > 0)
            {
                hfCategoryList.Value = "";
                foreach (int? item in _CategoryIdList.ToArray())
                {
                    hfCategoryList.Value += item.ToString() + ",";
                }

                hfCategoryList.Value = hfCategoryList.Value.Substring(0, hfCategoryList.Value.Length - 1);
            }

            // load selected category names in hfCategoryName hidden field control
            if (_CategoryNameList.Count > 0)
            {
                hfCategoryName.Value = "";
                foreach (string item in _CategoryNameList.ToArray())
                {
                    hfCategoryName.Value += item.ToString() + ",";
                }

                hfCategoryName.Value = hfCategoryName.Value.Substring(0, hfCategoryName.Value.Length - 1);
            }
        }

        /// <summary>
        /// Load measurement unit dropdownlist with units from database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/16/2009</date>
        private void LoadMeasurementUnits()
        {
            DataTable dt = MeasurementUnit.List(new Melon.Components.ProductCatalog.MeasurementUnit());                                    
            ddlMeasurementUnit.DataSource = dt;            
            ddlMeasurementUnit.DataTextField = "Name";
            ddlMeasurementUnit.DataValueField = "Id";
            ddlMeasurementUnit.DataBind();

            ddlMeasurementUnit.Items.Insert(0, new ListItem("--Select Unit--", ""));

            if (this._Unit != null)
            {
                ddlMeasurementUnit.SelectedIndex = ddlMeasurementUnit.Items.IndexOf(ddlMeasurementUnit.Items.FindByValue(Convert.ToString(this._Unit.Value)));
            }
        }

        /// <summary>
        /// Load category hierarchical listwith categories from database
        /// </summary>
        /// <author>Melon Team</author>
        /// <date>09/17/2009</date>
        private void LoadCategoryList()
        {
            DataTable dtCategoryList = Category.GetHierarchicalList(((bool?)null));
            if (dtCategoryList.Rows.Count > 0)
            {
                String str = "---- ";
                foreach(DataRow row in dtCategoryList.Rows)
                {
                    String s = "";
                    for (int i = 0;  i < Convert.ToInt32(row["CatLevel"]);i++)
                    {
                        s += str;
                    }

                    ListItem item = new ListItem(s.Replace(" ", HttpUtility.HtmlDecode("&nbsp;")) + row["Name"].ToString(), row["Id"].ToString() + ";" + row["CategoryFullName"].ToString());
                    item.Attributes.Add("visible", "false");
                    lbCategoryList.Items.Add(item);
                }
            }                                   
        }
    }
}
