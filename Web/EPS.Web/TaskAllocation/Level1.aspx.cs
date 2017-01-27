using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Web.Security;

using EPS.Entities;
using EPS.BusinessLayer;
using EPS.Resources;
using EPS.Utilities;

public partial class TaskAllocation_Level1 : System.Web.UI.Page
{
    public string ClientKey = EPS.Utilities.ConfigurationHelper.CAPTCHAResponse;


    protected void Page_Load(object sender, EventArgs e)
    {

        string[] values = ClientKey.Split(',');

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = values[i].Trim();
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetL1PendingChartData()
    {
        using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var chartWIPList = chartInfoBLObject.GetChartInfoByEmployee(profile.ClientProjectId, profile.UserName, profile.LevelNumber, false);

            var aaList = chartWIPList.Select(p => new List<string>{
                p.Id.ToString(), 
                p.ClientReference.ToString(),
                p.ReceivedDate.ToString("MM/dd/yyyy"),
                p.ClientMarket.ToString(),
                p.FileName.ToString(),
                p.ClientProject.Client + " - " + p.ClientProject.Project,
                p.ChartMoreInfo.LevelStatus.Name,
                p.ChartMoreInfo.Id.ToString()
            }).ToList();

            var json = JsonConvert.SerializeObject(aaList).ToString();
            return json;
        };
    }

    [WebMethod(EnableSession = true)]
    public static string GetL1ChartStatuses()
    {
        using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
        {
            var chartStatuses = lookupBLObject.GetLookupByCategory(null, StringMessages.lookup_ChartStatus)
                .Where(a => a.Id != (int)Helper.ChartStatus.WorkInProgress && a.Id != (int)Helper.ChartStatus.Pending)
                .Select(p => new List<string>{
                p.Id.ToString(),
                p.Name.ToString(),
            }).ToList();

            var json = JsonConvert.SerializeObject(chartStatuses).ToString();
            return json;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetL1StatusComments(Int32 commentCategoryId)
    {
        using (var commentsObject = new EPS.BusinessLayer.Comments())
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var comments = commentsObject.GetCommentsByCategory(profile.ClientProjectId, commentCategoryId, true).OrderBy(c => c.DisplayOrder)
                .Select(p => new List<string>{
                p.Id.ToString(),
                p.Description.ToString(),
            }).ToList();

            var json = JsonConvert.SerializeObject(comments).ToString();
            return json;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetL1BindCmdStatus(Int32 commentCategoryId)
    {
        using (var commentsObject = new EPS.BusinessLayer.Comments())
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var comments = commentsObject.GetCommentsByCategory(profile.ClientProjectId, commentCategoryId, true).OrderBy(c => c.DisplayOrder)
                .Select(p => new List<string>{
                p.Id.ToString(),
                p.Description.ToString(),
            }).ToList();

            var json = JsonConvert.SerializeObject(comments).ToString();
            return json;
        }
    }


    [WebMethod(EnableSession = true)]
    public static string SaveL1DataChartInfo(Int64 chartId, Int64 chartMoreId, int levelStatusId, int levelStatusCommentId, int noOfPages, int noOfDxCodes, string overallStatus)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                var chartInfoObject = new EPS.Entities.ChartInfo()
                {
                    Id = chartId,
                    NoOfPages = noOfPages,
                    OverallStatus = overallStatus,
                    ChartMoreInfo = new EPS.Entities.ChartMoreInfo()
                    {
                        Id = chartMoreId,
                        LevelStatusId = levelStatusId,
                        LevelStatusCommentId = levelStatusCommentId,
                        LevelUpdatedBy = profile.LoggedOnId,
                        NumberOfDxCode = noOfDxCodes
                    }
                };

                chartInfoBL.UpdateChartInfoStatus(chartInfoObject);
            }
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return "[\" 1 \"]";
    }

    [WebMethod(EnableSession = true)]
    public static string SaveL1DataCommandStatusInfo(string filename, int clientprojectid, int levelnumber, int noofDxCodes, int noofPages)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                var chartInfoObject = new EPS.Entities.ChartInfo()
                {
                    FileName = filename,
                    NoOfPages = noofPages,
                    ClientProjectId = clientprojectid,
                    ChartMoreInfo = new EPS.Entities.ChartMoreInfo()
                    {
                        LevelNumber = levelnumber,
                        NumberOfDxCode = noofDxCodes
                    }
                };

                chartInfoBL.UpdateCommentInfoStatus(chartInfoObject);
            }
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return "[\" 1 \"]";
    }


    [WebMethod(EnableSession = true)]
    public static string RequestL1Chart()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                chartInfoBL.RequestChart(profile.ClientProjectId, profile.LevelNumber, profile.UserName);
            }
        }
        catch (ApplicationException ex)
        {
            return string.Format("[\" {0} \"]", ex.Message);
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }
        return "[\" 1 \"]";
    }

    [WebMethod(EnableSession = true)]
    public static string L1PendingChartCount()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var pendingChartCountList = chartInfoBL.GetChartsPendingCount(profile.ClientProjectId, profile.LevelNumber);
                return pendingChartCountList.ToString();
            }
        }
        catch (ApplicationException ex)
        {
            return string.Format("[\" {0} \"]", ex.Message);
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }
    }

    [WebMethod(EnableSession = true)]
    public static string RequestL1ChartByClientReference(string clientReference)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                chartInfoBL.RequestChartByClientReference(profile.ClientProjectId, profile.LevelNumber, profile.UserName, clientReference);
            }
        }
        catch (ApplicationException ex)
        {
            return string.Format("[\" {0} \"]", ex.Message);
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }
        return "[\" 1 \"]";
    }

    [WebMethod(EnableSession = true)]
    public static string GetL1OtherChartData()
    {
        using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var nonWIPCharts = chartInfoBL.GetChartInfoByEmployee(profile.ClientProjectId, profile.UserName, profile.LevelNumber, true);


            var nonWIPChartsSortedValue = nonWIPCharts.ToList().OrderByDescending(a => a.OverallStatus);


            var charts = nonWIPChartsSortedValue.Select(p => new List<string>{
                p.Id.ToString(), 
                p.ClientReference.ToString(),
                p.ReceivedDate.ToString("MM/dd/yyyy"),
                p.ClientMarket.ToString(),
                p.FileName.ToString(),                
                p.ClientProject.Client + " - " + p.ClientProject.Project,
                p.ChartMoreInfo.LevelStatus.Name,
                p.ChartMoreInfo.LevelStatusComment.Description,
                p.ChartMoreInfo.Id.ToString(),
                p.ChartMoreInfo.NumberOfDxCode.ToString(),                
                p.ChartMoreInfo.LevelStatus.Id.ToString(),                
                p.ChartMoreInfo.LevelStatusCommentId.ToString(),
                p.NoOfPages.ToString(),
                ConfigurationHelper.CAPTCHAResponse.ToString(),
                p.ClientProjectId.ToString(),
                p.ChartMoreInfo.LevelNumber.ToString()               
            }).ToList();
            var json = JsonConvert.SerializeObject(charts).ToString();
            return json;
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
}
