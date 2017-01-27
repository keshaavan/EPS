<%@ WebHandler Language="C#" Class="PerformanceSummary" %>

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

public class PerformanceSummary : IHttpHandler
{
    public void ProcessRequest(HttpContext objContext)
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        context.Response.ContentType = "application/json";

        const int columnCount = 12;
        ExcelPackage objExlPckg;
        ExcelWorksheet objExlWS;

        ///////////////////////////////////////////// READ PARAM'S STARTS
        string fromDate = (context.Request["fromDatedp"]);
        string toDate = (context.Request["toDatedp"]);
        string levelNumber = (context.Request["levelNumber"]);
        string userName = (context.Request["username"]);
        string queueID = (context.Request["queueID"]);
        ///////////////////////////////////////////// READ PARAM'S ENDS

        try
        {
            var profileName = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var chartStatusBL = new EPS.BusinessLayer.ChartInfo();

            var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

            if (!profile.IsAdmin && string.IsNullOrEmpty(userName))
                userName = profile.UserName;

            //var dtUserPerformance = chartStatusBL.GetInfoForUserPerformance(int.Parse(queueID), Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),
            //    string.IsNullOrEmpty(userName) ? null : userName, Convert.ToInt32(levelNumber), profile.IsByDxCode, profile.IsAdmin).Tables[0];
            //var dtUserPerformance = chartStatusBL.GetInfoForUserPerformance(int.Parse(queueID), Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),
            //    string.IsNullOrEmpty(userName) ? null : userName, Convert.ToInt32(levelNumber), profile.IsByDxCode).Tables[0];

            var dtUserPerformance = chartStatusBL.GetInfoForUserPerformance(queueID, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),
                string.IsNullOrEmpty(userName) ? null : userName, Convert.ToInt32(levelNumber), profile.IsByDxCode).Tables[0];


            dtUserPerformance.Columns.Add("ResourceName", typeof(System.String));
            dtUserPerformance.Columns["ResourceName"].Expression = String.Format("{0}+' '+{1}", DBResources.col_FirstName, DBResources.col_LastName);

            if (Convert.ToInt32(levelNumber) != 1)
            {
                dtUserPerformance.Columns.Remove(DBResources.col_ErrorChartsCount);
                dtUserPerformance.Columns.Remove(DBResources.col_TotalCharts);

                dtUserPerformance.Columns.Add(DBResources.col_ErrorChartsCount, typeof(System.String));
                dtUserPerformance.Columns.Add(DBResources.col_TotalCharts, typeof(System.String));

                foreach (DataRow dr in dtUserPerformance.Rows)
                {
                    dr[DBResources.col_ErrorChartsCount] = "NA";
                    dr[DBResources.col_TotalCharts] = "NA";
                }
            }
            else
            {
                foreach (DataRow dr in dtUserPerformance.Rows)
                {
                    dr[DBResources.col_QualityControl] = Convert.ToDecimal(dr[DBResources.col_QualityControl].ToString()) / 100;
                }
            }

            

            if (profile.IsAdmin)
            {
                dtUserPerformance.Columns["ResourceName"].SetOrdinal(0);
                dtUserPerformance.Columns[DBResources.col_UserName].SetOrdinal(1);
                dtUserPerformance.Columns[DBResources.col_CompletedChartsCount].SetOrdinal(2);
                dtUserPerformance.Columns[DBResources.col_InvalidChartsCount].SetOrdinal(3);
                dtUserPerformance.Columns[DBResources.col_ErrorChartsCount].SetOrdinal(4);
                dtUserPerformance.Columns[DBResources.col_TotalCharts].SetOrdinal(5);
                dtUserPerformance.Columns[DBResources.col_QualityControl].SetOrdinal(6);
                dtUserPerformance.Columns["Queue"].SetOrdinal(7);
                dtUserPerformance.Columns[DBResources.col_chartaudited].SetOrdinal(8);
                dtUserPerformance.Columns[DBResources.col_QC].SetOrdinal(9);


                dtUserPerformance.Columns[DBResources.col_CompletedChartsCount].ColumnName = "#ChartsCompleted";
                dtUserPerformance.Columns[DBResources.col_InvalidChartsCount].ColumnName = "#InvalidCharts";
                dtUserPerformance.Columns[DBResources.col_ErrorChartsCount].ColumnName = "#Errors";
                dtUserPerformance.Columns[DBResources.col_TotalCharts].ColumnName = "DxCodes";
                dtUserPerformance.Columns["ResourceName"].ColumnName = "Resource Name";
                dtUserPerformance.Columns[DBResources.col_QualityControl].ColumnName = "Quality Control";
                dtUserPerformance.Columns["Queue"].ColumnName = "Queue";
                dtUserPerformance.Columns[DBResources.col_chartaudited].ColumnName = "#ChartAudited";
                dtUserPerformance.Columns[DBResources.col_QC].ColumnName = "QC Rate";
                
            }
            else
            {
                dtUserPerformance.Columns["ResourceName"].SetOrdinal(0);
                dtUserPerformance.Columns[DBResources.col_UserName].SetOrdinal(1);
                dtUserPerformance.Columns[DBResources.col_CompletedChartsCount].SetOrdinal(2);
                dtUserPerformance.Columns[DBResources.col_InvalidChartsCount].SetOrdinal(3);
                dtUserPerformance.Columns[DBResources.col_ErrorChartsCount].SetOrdinal(4);
                dtUserPerformance.Columns[DBResources.col_TotalCharts].SetOrdinal(5);
                dtUserPerformance.Columns[DBResources.col_QualityControl].SetOrdinal(6);
                dtUserPerformance.Columns["Queue"].SetOrdinal(7);


                dtUserPerformance.Columns[DBResources.col_CompletedChartsCount].ColumnName = "#Charts Completed";
                dtUserPerformance.Columns[DBResources.col_InvalidChartsCount].ColumnName = "#Invalid Charts";
                dtUserPerformance.Columns[DBResources.col_ErrorChartsCount].ColumnName = "#Errors";
                dtUserPerformance.Columns[DBResources.col_TotalCharts].ColumnName = "#Charts Audited/ DxCodes";
                dtUserPerformance.Columns["ResourceName"].ColumnName = "Resource Name";
                dtUserPerformance.Columns[DBResources.col_QualityControl].ColumnName = "Quality Control";
                dtUserPerformance.Columns["Queue"].ColumnName = "Queue";
                
            }

           

            dtUserPerformance.Columns.Remove(DBResources.col_LevelNumber);

            objExlPckg = new ExcelPackage();
            objExlWS = objExlPckg.Workbook.Worksheets.Add("Performance Summary");

            objExlWS.PrinterSettings.FitToPage = true;
            objExlWS.PrinterSettings.FitToWidth = 1;
            objExlWS.PrinterSettings.FitToHeight = 0;
            objExlWS.PrinterSettings.TopMargin = 0.25M;
            objExlWS.PrinterSettings.LeftMargin = 0.25M;

            objExlWS.Cells["A2"].LoadFromDataTable(dtUserPerformance, true);

            //Set Generated Date/time.
            objExlWS.Cells["A1"].Value = string.Format("Generated on: {0}", DateTime.Now);
            objExlWS.Cells["A1"].Style.Font.Bold = true;
            objExlWS.Cells["A1"].Style.Font.Italic = true;
            objExlWS.Cells["A1"].Style.Font.Size = 8;

            objExlWS.Cells["B1"].Value = string.Format("Level: {0}", levelNumber);
            objExlWS.Cells["B1"].Style.Font.Bold = true;
            objExlWS.Cells["B1"].Style.Font.Italic = true;
            objExlWS.Cells["B1"].Style.Font.Size = 8;

            if (string.IsNullOrEmpty(userName))
                objExlWS.Cells["C1"].Value = string.Format("Username: {0}", "All Employees");
            else
                objExlWS.Cells["C1"].Value = string.Format("Username: {0}", userName);

            objExlWS.Cells["C1"].Style.Font.Bold = true;
            objExlWS.Cells["C1"].Style.Font.Italic = true;
            objExlWS.Cells["C1"].Style.Font.Size = 8;

            objExlWS.Cells["D1"].Value = string.Format("From Date: {0}", fromDate);
            objExlWS.Cells["D1"].Style.Font.Bold = true;
            objExlWS.Cells["D1"].Style.Font.Italic = true;
            objExlWS.Cells["D1"].Style.Font.Size = 8;

            objExlWS.Cells["E1"].Value = string.Format("To Date: {0}", toDate);
            objExlWS.Cells["E1"].Style.Font.Bold = true;
            objExlWS.Cells["E1"].Style.Font.Italic = true;
            objExlWS.Cells["E1"].Style.Font.Size = 8;

            objExlWS.Row(1).Height = 35;
            objExlWS.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

            objExlWS.Row(2).Style.Font.Bold = true;
            objExlWS.Cells[2, 1, 2, columnCount-2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            objExlWS.Cells[2, 1, 2, columnCount-2].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objExlWS.Cells[2, 1, 2, columnCount-2].Style.Font.Color.SetColor(Color.White);

            if (profile.IsAdmin)
            {
                objExlWS.Cells[2, 11, (2 + dtUserPerformance.Rows.Count), columnCount].Clear();
            }
            else
            {
                objExlWS.Cells[2, 9, (2 + dtUserPerformance.Rows.Count), columnCount].Clear();
            }

            if (Convert.ToInt32(levelNumber) == 1)
            {
                objExlWS.Column(7).Style.Numberformat.Format = "0.00%"; // Quality control
            }

            for (int iCtr = 1; iCtr <= columnCount; iCtr++)
            {
                objExlWS.Column(iCtr).AutoFit();
            }

            objContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            objContext.Response.AddHeader("content-disposition", string.Format("attachment;  filename={1}_{2}_PerformanceSummary_{0:MMMddyyyy_hhmmss}.xlsx", DateTime.Now, profileName.Client, profileName.Project));
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

