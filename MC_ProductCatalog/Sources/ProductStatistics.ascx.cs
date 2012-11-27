using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Globalization;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    /// <summary>
    /// Web page displays statistics for current product 
    /// </summary>
    /// <remarks>
    /// User control displays information on product views in the front end - product creation date, last modification date,
    /// how many times the product is viewed per day or per month. 
    /// </remarks>
    public partial class CodeBehind_ProductStatistics : ProductCatalogControl
    {        
        /// <summary>
        /// Initializes user control information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsControlPostBack)
            {
                if (SelectedProductId != null)
                {
                    LoadProductStatistics();
                }
            }
        }

        /// <summary>
        /// Loads product statistics.
        /// </summary>
        private void LoadProductStatistics()
        {
            Statistics stats = new Statistics();
            stats.ProductId = SelectedProductId;
            DataTable dtStats = stats.List();

            tabStatistics.Visible = true;
            lblNoStatistics.Visible = false;            
            if (dtStats.Rows.Count == 0)
            {
                tabStatistics.Visible = false;
                lblNoStatistics.Visible = true;
                return;
            }

            

            // get total views for current product            
            lblTotalViews.Text = dtStats.Rows.Count.ToString();

            // get today views for current product
            lblViewsToday.Text = dtStats.Select(String.Format("DateViewed >#{0}# AND DateViewed <#{1}#", DateTime.Now.AddDays(-1).ToString(CultureInfo.InvariantCulture), DateTime.Now.AddDays(1).ToString(CultureInfo.InvariantCulture))).Count().ToString();            

            // get current month views for current product
            lblViewsMonth.Text = dtStats.Select(String.Format("DateViewed >#{0}# AND DateViewed <#{1}#", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(CultureInfo.InvariantCulture), new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).ToString(CultureInfo.InvariantCulture))).Count().ToString();

            // get views per day for current product
            int days = DateTime.Now.Subtract((DateTime)dtStats.Rows[0]["DateCreated"]).Days;
            double viewsPerDay=Convert.ToDouble(dtStats.Rows.Count) / Convert.ToDouble(days==0 ? 1 : days);
            lblViewsPerDay.Text = Math.Round(viewsPerDay,2).ToString();

            // get views per month for current product
            DateTime dateCreated = Convert.ToDateTime(dtStats.Rows[0]["DateCreated"]);
            double monthCnt = 12*(DateTime.Now.Year - dateCreated.Year)+DateTime.Now.Month-dateCreated.Month+1;
            double viewsPerMonth = dtStats.Rows.Count / monthCnt;
            lblViewsPerMonth.Text = Math.Round(viewsPerMonth, 2).ToString();
            
            
            lblAddedOn.Text = (dtStats.Rows[0]["DateCreated"]!=DBNull.Value) ? Convert.ToDateTime(dtStats.Rows[0]["DateCreated"]).ToString("d") : "N/A";
            lblUpdatedOn.Text = (dtStats.Rows[0]["DateModified"] != DBNull.Value) ? Convert.ToDateTime(dtStats.Rows[0]["DateModified"]).ToString("d") : "N/A";
            lblAddedBy.Text = (dtStats.Rows[0]["CreatedBy"]!=DBNull.Value) ? dtStats.Rows[0]["CreatedBy"].ToString() : "N/A";
            lblUpdatedBy.Text = (dtStats.Rows[0]["ModifiedBy"]!=DBNull.Value) ? dtStats.Rows[0]["ModifiedBy"].ToString() : "N/A";
            
        }
    }
}
