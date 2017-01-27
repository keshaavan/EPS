<%@ WebHandler Language="C#" Class="ChartStatusComments" %>
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Reflection;
using System.Drawing;

using OfficeOpenXml.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using EPS.Entities;
using EPS.BusinessLayer;
using EPS.Resources;

public class ChartStatusComments : IHttpHandler {

    public void ProcessRequest(HttpContext objContext)
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        context.Response.ContentType = "application/json";

        const int columnCount = 22;
        ExcelPackage objExlPckg;
        ExcelWorksheet objExlWS;

        ///////////////////////////////////////////// READ PARAM'S STARTS
        string fromDatedp = (context.Request["fromDatedp"]);
        string toDatedp = (context.Request["toDatedp"]);
        string queueDrp = (context.Request["queueDrp"]);
     
        ///////////////////////////////////////////// READ PARAM'S ENDS
        
        try
        {
            var profileName = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var chartStatusBL = new EPS.BusinessLayer.ChartInfo();
            DataTable dtChartStatusComments = chartStatusBL.GetChartAudits(int.Parse(queueDrp), String.IsNullOrEmpty(fromDatedp) ? new Nullable<DateTime>() : Convert.ToDateTime(fromDatedp), string.IsNullOrEmpty(toDatedp) ? new Nullable<DateTime>() : Convert.ToDateTime(toDatedp)).Tables[0];

            var clientProject = new EPS.BusinessLayer.ClientProject().GetClientProjectById(profileName.ClientProjectId);
            string clientReferenceLabelHeading = clientProject.ClientReferenceLabel;// StringMessages.ReferenceLabelHeading_ClientReference;

            dtChartStatusComments.Columns.Remove(DBResources.col_ClientReferenceLabel);
            dtChartStatusComments.Columns.Remove(DBResources.col_LevelNumber);

            dtChartStatusComments.Columns[DBResources.col_ClientKey].ColumnName = "Client Key";
            dtChartStatusComments.Columns[DBResources.col_ProjectKey].ColumnName = "Project Key";
            
            objExlPckg = new ExcelPackage();
            objExlWS = objExlPckg.Workbook.Worksheets.Add("ChartAuditLog");

            objExlWS.PrinterSettings.FitToPage = true;
            objExlWS.PrinterSettings.FitToWidth = 1;
            objExlWS.PrinterSettings.FitToHeight = 0;
            objExlWS.PrinterSettings.TopMargin = 0.25M;
            objExlWS.PrinterSettings.LeftMargin = 0.25M;

            objExlWS.Cells["A2"].LoadFromDataTable(dtChartStatusComments, true);

            //Set Generated Date/time.
            objExlWS.Cells["A1"].Value = string.Format("Generated on: {0}", DateTime.Now);
            objExlWS.Cells["A1"].Style.Font.Bold = true;
            objExlWS.Cells["A1"].Style.Font.Italic = true;
            objExlWS.Cells["A1"].Style.Font.Size = 8;
            objExlWS.Row(1).Height = 35;
            objExlWS.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

            objExlWS.Cells["C1"].Value = string.Format("From Date: {0}", fromDatedp);
            objExlWS.Cells["C1"].Style.Font.Bold = true;
            objExlWS.Cells["C1"].Style.Font.Italic = true;
            objExlWS.Cells["C1"].Style.Font.Size = 8;
            objExlWS.Row(1).Height = 35;
            objExlWS.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;


            objExlWS.Cells["E1"].Value = string.Format("To Date: {0}", toDatedp);
            objExlWS.Cells["E1"].Style.Font.Bold = true;
            objExlWS.Cells["E1"].Style.Font.Italic = true;
            objExlWS.Cells["E1"].Style.Font.Size = 8;
            objExlWS.Row(1).Height = 35;
            objExlWS.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

            objExlWS.Row(2).Style.Font.Bold = true;
            objExlWS.Cells[2, 1, 2, columnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            objExlWS.Cells[2, 1, 2, columnCount].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objExlWS.Cells[2, 1, 2, columnCount].Style.Font.Color.SetColor(Color.White);

            objExlWS.Column(9).Style.Numberformat.Format = "MM/dd/yyyy"; // Received date

            objExlWS.Column(10).Style.Numberformat.Format = "MM/dd/yyy"; // Reviewed Date
            objExlWS.Column(12).Style.Numberformat.Format = "MM/dd/yyy"; // Audited Date
            objExlWS.Column(14).Style.Numberformat.Format = "MM/dd/yyy"; // Master Audited Date

            Boolean flag = true;
            for (int iCtr = 1; iCtr <= columnCount; iCtr++)
            {
                if (flag && iCtr != 5)
                {
                    objExlWS.Cells["F2"].Value = StringMessages.ReferenceLabelHeading_Market;
                    objExlWS.Cells["G2"].Value = clientReferenceLabelHeading;
                    flag = false;
                } 
                
                objExlWS.Column(iCtr).AutoFit();
            }

            objContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            objContext.Response.AddHeader("content-disposition", string.Format("attachment;  filename={1}_{2}_ChartAuditLog_{0:MMMddyyyy_hhmmss}.xlsx", DateTime.Now, profileName.Client, profileName.Project));
            objContext.Response.BinaryWrite(objExlPckg.GetAsByteArray());
        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}