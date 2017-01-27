<%@ WebHandler Language="C#" Class="ChartStatus" %>

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

public class ChartStatus : IHttpHandler
{

    public void ProcessRequest(HttpContext objContext)
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        context.Response.ContentType = "application/json";

        const int columnCount = 31;
        ExcelPackage objExlPckg;
        ExcelWorksheet objExlWS;

        ///////////////////////////////////////////// READ PARAM'S STARTS
        string levelStatus = (context.Request["levelStatus"]);
        string chartDateType = (context.Request["chartDateType"]);
        string fromDatedp = (context.Request["fromDatedp"]);
        string toDatedp = (context.Request["toDatedp"]);
        string levelNumber = (context.Request["levelNumber"]);
        string levelStatusId = (context.Request["levelStatusId"]);
        string queueID = (context.Request["queueId"]);
        ///////////////////////////////////////////// READ PARAM'S ENDS

        try
        {
            var profileName = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

            var chartStatusBL = new EPS.BusinessLayer.ChartInfo();
            DataTable dtChartStatusList = chartStatusBL.GetChartInfoByOverallStatus(int.Parse(queueID), (Helper.ChartDateTypes)Convert.ToInt32(chartDateType),
                Convert.ToDateTime(fromDatedp), Convert.ToDateTime(toDatedp), levelStatus, string.IsNullOrEmpty(levelNumber) ? new Nullable<int>() : Convert.ToInt32(levelNumber),
                string.IsNullOrEmpty(levelStatusId) ? new Nullable<int>() : Convert.ToInt32(levelStatusId)).Tables[0];

            var clientProject = new EPS.BusinessLayer.ClientProject().GetClientProjectById(profileName.ClientProjectId);
            string clientReferenceLabelHeading = clientProject.ClientReferenceLabel;

            dtChartStatusList.Columns.Remove(DBResources.col_ChartId);
            dtChartStatusList.Columns.Remove(DBResources.col_ClientReferenceLabel);

            dtChartStatusList.Columns.Remove(DBResources.col_L1ChartMoreInfoId);
            dtChartStatusList.Columns.Remove(DBResources.col_L2ChartMoreInfoId);
            dtChartStatusList.Columns.Remove(DBResources.col_L3ChartMoreInfoId);
            dtChartStatusList.Columns.Remove(DBResources.col_L1ProductionTimeCount);
            dtChartStatusList.Columns.Remove(DBResources.col_L2ProductionTimeCount);
            dtChartStatusList.Columns.Remove(DBResources.col_L3ProductionTimeCount);
            
            dtChartStatusList.Columns[DBResources.col_UploadedDate].ColumnName = "Imported date";
            dtChartStatusList.Columns[DBResources.col_NoOfPages].ColumnName = "#Encounters";
            dtChartStatusList.Columns[DBResources.col_ClientKey].ColumnName = "Client Key";
            dtChartStatusList.Columns[DBResources.col_ProjectKey].ColumnName = "Project Key";

            objExlPckg = new ExcelPackage();
            objExlWS = objExlPckg.Workbook.Worksheets.Add("ChartStatus");

            objExlWS.PrinterSettings.FitToPage = true;
            objExlWS.PrinterSettings.FitToWidth = 1;
            objExlWS.PrinterSettings.FitToHeight = 0;
            objExlWS.PrinterSettings.TopMargin = 0.25M;
            objExlWS.PrinterSettings.LeftMargin = 0.25M;

            objExlWS.Cells["A2"].LoadFromDataTable(dtChartStatusList, true);

            //Set Generated Date/time.
            objExlWS.Cells["A1"].Value = string.Format("Generated on: {0}", DateTime.Now);
            objExlWS.Cells["A1"].Style.Font.Bold = true;
            objExlWS.Cells["A1"].Style.Font.Italic = true;
            objExlWS.Cells["A1"].Style.Font.Size = 8;

            objExlWS.Cells["C1"].Value = string.Format("Date Type: {0}", ((Helper.ChartDateTypes)Convert.ToInt32(chartDateType)).ToString());
            objExlWS.Cells["C1"].Style.Font.Bold = true;
            objExlWS.Cells["C1"].Style.Font.Italic = true;
            objExlWS.Cells["C1"].Style.Font.Size = 8;

            if (string.IsNullOrEmpty(levelStatus) && string.IsNullOrEmpty(levelStatusId))
                objExlWS.Cells["F1"].Value = string.Format("Level Status: {0}", "All Statuses");
            else
            {
                int lId = 0;
                Int32.TryParse(levelStatusId, out lId);

                if (lId > 0)
                {
                    using (var lookupBL = new EPS.BusinessLayer.Lookup())
                    {
                        var lookup = lookupBL.GetLookupById(null, lId);
                        objExlWS.Cells["F1"].Value = string.Format("Level Status: {0}", string.Format("L{0} - {1}", levelNumber, lookup.Name));
                    };
                }
                else
                {
                    objExlWS.Cells["F1"].Value = string.Format("Level Status: {0}", levelStatus);
                }
            }

            objExlWS.Cells["F1"].Style.Font.Bold = true;
            objExlWS.Cells["F1"].Style.Font.Italic = true;
            objExlWS.Cells["F1"].Style.Font.Size = 8;

            objExlWS.Cells["H1"].Value = string.Format("From Date: {0}", fromDatedp);
            objExlWS.Cells["H1"].Style.Font.Bold = true;
            objExlWS.Cells["H1"].Style.Font.Italic = true;
            objExlWS.Cells["H1"].Style.Font.Size = 8;

            objExlWS.Cells["J1"].Value = string.Format("To Date: {0}", toDatedp);
            objExlWS.Cells["J1"].Style.Font.Bold = true;
            objExlWS.Cells["J1"].Style.Font.Italic = true;
            objExlWS.Cells["J1"].Style.Font.Size = 8;

            objExlWS.Row(1).Height = 35;
            objExlWS.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

            objExlWS.Row(2).Style.Font.Bold = true;
            objExlWS.Cells[2, 1, 2, columnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            objExlWS.Cells[2, 1, 2, columnCount].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objExlWS.Cells[2, 1, 2, columnCount].Style.Font.Color.SetColor(Color.White);

            objExlWS.Column(9).Style.Numberformat.Format = "mm/dd/yyy"; // Imported Date
            objExlWS.Column(10).Style.Numberformat.Format = "mm/dd/yyy"; // Received Date

            objExlWS.Column(13).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L1 start date
            objExlWS.Column(14).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L1 End date
            //objExlWS.Column(21).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L2 start Date
            //objExlWS.Column(22).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L2  End Date
            //objExlWS.Column(27).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L3 start date
            //objExlWS.Column(28).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss";    //L3 End date

            objExlWS.Column(19).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L2 start Date
            objExlWS.Column(20).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L2  End Date
            objExlWS.Column(25).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss"; // L3 start date
            objExlWS.Column(26).Style.Numberformat.Format = "mm/dd/yyy HH:mm:ss";    //L3 End date

            Boolean flag = true;
            for (int iCtr = 1; iCtr <= columnCount; iCtr++)
            {
                if (flag && iCtr != 5)
                {
                    objExlWS.Cells["G2"].Value = StringMessages.ReferenceLabelHeading_Market;
                    objExlWS.Cells["H2"].Value = clientReferenceLabelHeading;
                    flag = false;
                }

                objExlWS.Column(iCtr).AutoFit();
            }

            objContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            objContext.Response.AddHeader("content-disposition", string.Format("attachment;  filename={1}_{2}_ChartStatus_{0:MMMddyyyy_hhmmss}.xlsx", DateTime.Now, profileName.Client, profileName.Project));
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