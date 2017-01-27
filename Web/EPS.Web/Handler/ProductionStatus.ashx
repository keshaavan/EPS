<%@ WebHandler Language="C#" Class="ProductionStatus" %>

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
using EPS.Utilities;

public class ProductionStatus : IHttpHandler
{

    public void ProcessRequest(HttpContext objContext)
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        context.Response.ContentType = "application/json";

        const int columnCount = 20;
        ExcelPackage objExlPckg;
        ExcelWorksheet objExlWS;

        ///////////////////////////////////////////// READ PARAM'S STARTS
        string fromDate = (context.Request["fromDatedp"]);
        string toDate = (context.Request["toDatedp"]);
        string queueID = (context.Request["queueID"]);
        ///////////////////////////////////////////// READ PARAM'S ENDS
        
        try
        {
            var profileName = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);            
            var clientProjectBLObject = new EPS.BusinessLayer.ClientProject();
            var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            
            var dtProductionStatus = clientProjectBLObject.GetProductionStatistics(queueID, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).Tables[0];
            dtProductionStatus.Columns[DBResources.col_ProcessedDate].ColumnName = "Date";
            
            objExlPckg = new ExcelPackage();
            objExlWS = objExlPckg.Workbook.Worksheets.Add("Current Production Status");

            objExlWS.PrinterSettings.FitToPage = true;
            objExlWS.PrinterSettings.FitToWidth = 1;
            objExlWS.PrinterSettings.FitToHeight = 0;
            objExlWS.PrinterSettings.TopMargin = 0.25M;
            objExlWS.PrinterSettings.LeftMargin = 0.25M;

            objExlWS.Cells["A2"].LoadFromDataTable(dtProductionStatus, true);

            //Set Generated Date/time.
            objExlWS.Cells["A1"].Value = string.Format("Generated on: {0}", DateTime.Now);
            objExlWS.Cells["A1"].Style.Font.Bold = true;
            objExlWS.Cells["A1"].Style.Font.Italic = true;
            objExlWS.Cells["A1"].Style.Font.Size = 8;

            objExlWS.Cells["B1"].Value = string.Format("From Date: {0}", fromDate);
            objExlWS.Cells["B1"].Style.Font.Bold = true;
            objExlWS.Cells["B1"].Style.Font.Italic = true;
            objExlWS.Cells["B1"].Style.Font.Size = 8;

            objExlWS.Cells["C1"].Value = string.Format("To Date: {0}", toDate);
            objExlWS.Cells["C1"].Style.Font.Bold = true;
            objExlWS.Cells["C1"].Style.Font.Italic = true;
            objExlWS.Cells["C1"].Style.Font.Size = 8;

            objExlWS.Row(1).Height = 35;
            objExlWS.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

            objExlWS.Row(2).Style.Font.Bold = true;
            objExlWS.Cells[2, 1, 2, columnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            objExlWS.Cells[2, 1, 2, columnCount].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objExlWS.Cells[2, 1, 2, columnCount].Style.Font.Color.SetColor(Color.White);

            objExlWS.Column(1).Style.Numberformat.Format = "mm/dd/yyy"; // Processed Date

            for (int iCtr = 1; iCtr <= columnCount; iCtr++)
            {
                objExlWS.Column(iCtr).AutoFit();
            }

            objContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            objContext.Response.AddHeader("content-disposition", string.Format("attachment;  filename={1}_{2}_CurrentProductionStatus_{0:MMMddyyyy_hhmmss}.xlsx", DateTime.Now, profileName.Client, profileName.Project));
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