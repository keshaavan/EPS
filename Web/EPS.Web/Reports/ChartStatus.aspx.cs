using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Profile;
using Newtonsoft.Json;

using EPS.Resources;
using EPS.Utilities;

using System.Data;
using System.Web.Security;


public partial class Reports_ChartStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    public static string GetChartInfoByOverallStatus(string chartStatus, string fromDate, string toDate, int chartDateType, string levelNumber, string levelStatusId, string queueId,string UserName)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

                var dt = chartInfoBLObject.GetChartInfoByOverallStatus(int.Parse(queueId), (EPS.Utilities.Helper.ChartDateTypes)chartDateType,
                    Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), chartStatus, string.IsNullOrEmpty(levelNumber) ? new Nullable<int>() : Convert.ToInt32(levelNumber),
                    string.IsNullOrEmpty(levelStatusId) ? new Nullable<int>() : Convert.ToInt32(levelStatusId)).Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> { 
                       "Audit",
                        p.Field<Int32>(DBResources.col_AuditCount).ToString(),
                        p.Field<string>(DBResources.col_ClientMarket),
                        p.Field<string>(DBResources.col_ClientReference),
                        p.Field<string>(DBResources.col_FileName),
                        p.Field<DateTime>(DBResources.col_UploadedDate).ToString("MM/dd/yyyy"),
                        p.Field<DateTime>(DBResources.col_ReceivedDate).ToString("MM/dd/yyyy"),
                        p.Field<string>(DBResources.col_OverallStatus),
                        p.Field<Nullable<Int32>>(DBResources.col_NoOfPages).ToString(),
                        p.Field<Nullable<DateTime>>(DBResources.col_L1StartDate).ToString(),
                        p.Field<Nullable<DateTime>>(DBResources.col_L1EndDate).ToString(),
                        p.Field<Nullable<Int32>>(DBResources.col_L1TimeTaken).ToString(),
                        p.Field<string>(DBResources.col_L1Resource),
                        p.Field<string>(DBResources.col_L1StatusComment),
                        p.Field<Nullable<Int32>>(DBResources.col_L1NumberOfDxCodes).ToString(),
                        p.Field<Nullable<DateTime>>(DBResources.col_L2StartDate).ToString(),
                        p.Field<Nullable<DateTime>>(DBResources.col_L2EndDate).ToString(),
                        p.Field<Nullable<Int32>>(DBResources.col_L2TimeTaken).ToString(),
                        p.Field<string>(DBResources.col_L2Resource),
                        p.Field<string>(DBResources.col_L2StatusComment),
                        p.Field<Nullable<Int32>>(DBResources.col_L2NumberOfDxCodes).ToString(),
                        p.Field<Nullable<DateTime>>(DBResources.col_L3StartDate).ToString(),
                        p.Field<Nullable<DateTime>>(DBResources.col_L3EndDate).ToString(),
                        p.Field<Nullable<Int32>>(DBResources.col_L3TimeTaken).ToString(),
                        p.Field<string>(DBResources.col_L3Resource),
                        p.Field<string>(DBResources.col_L3StatusComment),
                        p.Field<Nullable<Int32>>(DBResources.col_L3NumberOfDxCodes).ToString(),
                        p.Field<Nullable<Int64>>(DBResources.col_ChartId).ToString(),
                        p.Field<Nullable<Int32>>(DBResources.col_L1ProductionTimeCount).ToString(),
                        p.Field<Nullable<Int32>>(DBResources.col_L2ProductionTimeCount).ToString(),
                        p.Field<Nullable<Int32>>(DBResources.col_L3ProductionTimeCount).ToString(),
                        p.Field<Nullable<Int64>>(DBResources.col_L1ChartMoreInfoId).ToString(),
                        p.Field<Nullable<Int64>>(DBResources.col_L2ChartMoreInfoId).ToString(),
                        p.Field<Nullable<Int64>>(DBResources.col_L3ChartMoreInfoId).ToString()
                        
                }).ToList();

                var json = JsonConvert.SerializeObject(lstItem).ToString();
                return json;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }

        return "";
    }

    [WebMethod(EnableSession = true)]
    public static string getChartStatuses(string UserName)
    {
        try
        {
            using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

                int upperLevel = 3;
                var chartStatuses = lookupBLObject.GetLookupByCategory(null, StringMessages.lookup_ChartStatus);

                var tStatuses = new List<ChartLevelStatus>();

                for (int i = 1; i <= upperLevel; i++)
                {
                    foreach (var chartStatus in chartStatuses)
                    {
                        if (!((i == upperLevel || i == 2) && chartStatus.Id == (int)Helper.ChartStatus.Completed))
                        {
                            var tStatus = new ChartLevelStatus();

                            tStatus.id = string.Format("{0}|{1}", i, chartStatus.Id);
                            tStatus.Name = string.Format("L{0} - {1}", i, chartStatus.Name);
                            tStatuses.Add(tStatus);
                        }
                    }
                }

                var statuses = tStatuses.Select(p => new List<string>{
                        p.id,
                        p.Name
                    }).ToList();

                var json = JsonConvert.SerializeObject(statuses).ToString();
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
    public static string GetChartAuditLogsByChartId(Int64 chartId)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var dt = chartInfoBLObject.GetChartAuditByChartId(chartId);

                var lstItem = dt.AsEnumerable().Select(p => new List<string> { 
                      p.LastUpdatedDate.ToString(),
                      p.PageNumbers,
                      p.CorrectedValue,
                      p.Comments,
                      p.ErrorCategory.Name,
                      p.ErrorSubCategory.Name,
                      p.AdditionalComments,
                      (p.L3LevelDisagreeId.HasValue == true) ? p.L3LevelDisagree.Name : ""
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
    public static string GetChartProductionByChartMoreInfoId(long chartMoreInfoId, int levelNumber, int QueueID,string UserName)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

                var productionTimes = chartInfoBLObject.GetChartProductionByChartMoreInfoId(QueueID, chartMoreInfoId, levelNumber);

                var lstItem = productionTimes.Select(p => new List<string>{ 
                    p.StartDate.ToString(),
                    (p.EndDate.HasValue)? p.EndDate.Value.ToString(): "",
                    p.TimeTaken.ToString(),
                    p.LevelStatus.Name,
                    (p.LevelStatusCommentId.HasValue)? p.LevelStatusComment.Description: "" ,
                    string.Format("{0} {1}_{2}", p.Employee.FirstName, p.Employee.LastName, p.Employee.UserName)
                }).ToList();

                var json = JsonConvert.SerializeObject(lstItem).ToString();
                return json;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }

        return "";
    }

    [WebMethod(EnableSession = true)]
    public static string GetQueue(string UserName, string ClientProjectId,string EmployeeId)
    {
        var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);
        DataSet objDS;

        try
        {
            using (EPS.DataLayer.SiteMaster objSM = new EPS.DataLayer.SiteMaster())
            {
                objDS = objSM.getQueues(Convert.ToInt32(ClientProjectId), EmployeeId);
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

public class ChartLevelStatus
{
    public string id;
    public string Name;
}