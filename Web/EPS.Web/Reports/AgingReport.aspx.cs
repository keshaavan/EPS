using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;

using System.Data;
using System.Web.Security;

public partial class Reports_Aging : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string GetAgingSummaryReport(DateTime fromDate, DateTime toDate,string queueID, string UserName)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

                var dt = chartInfoBLObject.GetSummaryInfoForAging(int.Parse(queueID), Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> { 
                    p.Field<string>(DBResources.col_Status),
                    p.Field<int>(DBResources.col_Range1).ToString(),
                    p.Field<int>(DBResources.col_Range2).ToString(),
                    p.Field<int>(DBResources.col_Range3).ToString(),
                    p.Field<int>(DBResources.col_Range4).ToString(),
                    p.Field<int>(DBResources.col_Range5).ToString(),
                    p.Field<Boolean>(DBResources.col_isCompleted).ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(lstItem).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetAgingReportForCompleted(DateTime fromDate, DateTime toDate, string dateRangeType,string queueID , string UserName)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

                var dt = chartInfoBLObject.GetAgingReportDetails(int.Parse(queueID), Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), (Helper.DateRangeType)Convert.ToInt32(dateRangeType), true).Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> { 
                    p.Field<DateTime>(DBResources.col_ReceivedDate).ToString("MM/dd/yyyy"),
                    p.Field<int>(DBResources.col_L1Invalid).ToString(),
                    p.Field<int>(DBResources.col_L2Invalid).ToString(),
                    p.Field<int>(DBResources.col_L3Invalid).ToString(),
                    p.Field<int>(DBResources.col_CompletedChartsCount).ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(lstItem).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetAgingReportForIncomplete(DateTime fromDate, DateTime toDate, string dateRangeType, string queueID, string UserName)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

                var dt = chartInfoBLObject.GetAgingReportDetails(int.Parse(queueID), Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), (Helper.DateRangeType)Convert.ToInt32(dateRangeType), false).Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> { 
                    p.Field<DateTime>(DBResources.col_ReceivedDate).ToString("MM/dd/yyyy"),
                    p.Field<int>(DBResources.col_UnassignedCharts).ToString(),
                    p.Field<int>(DBResources.col_L1WorkInProgress).ToString(),
                    p.Field<int>(DBResources.col_L1Pending).ToString(),
                    p.Field<int>(DBResources.col_L1Hold).ToString(),
                    p.Field<int>(DBResources.col_L1Completed).ToString(),
                    p.Field<int>(DBResources.col_L2WorkInProgress).ToString(),
                    p.Field<int>(DBResources.col_L2Pending).ToString(),
                    p.Field<int>(DBResources.col_L2Hold).ToString(),
                    p.Field<int>(DBResources.col_L3WorkInProgress).ToString(),
                    p.Field<int>(DBResources.col_L3Pending).ToString(),
                    p.Field<int>(DBResources.col_L3Hold).ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(lstItem).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string IsUserAuthorized(string userName)
    {
        //if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) || HttpContext.Current.User.Identity.Name.ToLower() != userName.ToLower())
        //    return JsonConvert.SerializeObject(false).ToString();

        ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(userName);
        if (profile != null && !profile.IsAdmin)
            return JsonConvert.SerializeObject(false).ToString();

        MembershipUser User = Membership.GetUser(userName, true);
        if (User.LastPasswordChangedDate < DateTime.Now.AddDays(EPS.Utilities.ConfigurationHelper.PasswordExpiryPeriod))
            return JsonConvert.SerializeObject("ChangePassword");

        return JsonConvert.SerializeObject(true).ToString();
    }

    [WebMethod(EnableSession = true)]
    public static string GetQueue(string UserName)
    {
        var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);
        DataSet objDS;

        try
        {
            using (EPS.DataLayer.SiteMaster objSM = new EPS.DataLayer.SiteMaster())
            {
                objDS = objSM.getQueues(profile.ClientProjectId,profile.EmployeeId);
            }

            var lstItem = objDS.Tables[0].AsEnumerable().Select(p => new List<string> {                        
                        p.Field<Int32>("ID").ToString(),
                        p.Field<string>("Queue").ToString()
                }).ToList();

            var json = JsonConvert.SerializeObject(lstItem).ToString();
            return json;
        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }

        return "";
    }
}