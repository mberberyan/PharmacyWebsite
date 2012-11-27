using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melon.Components.ProductCatalog.ComponentEngine;
using System.Data;
using System.Web.UI;

namespace Melon.Components.ProductCatalog.UI.CodeBehind
{
    public partial class CodeBehind_ExportToExcel : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSet dsExport = Session["ExportCriteria"]!=null ? (DataSet)Session["ExportCriteria"] : null;

                if (dsExport != null)
                {
                    Utility peh = new Utility(dsExport);
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=ExportObjectInfo.xls");

                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    System.Web.HttpContext.Current.Response.Charset = "utf-8";
                    Response.Write(peh.GenerateExportFile(true, "print_table"));
                }
            }
        }
    }
}
