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
using System.Web.Security;

using Newtonsoft.Json;

using EPS.Entities;
using EPS.BusinessLayer;
using EPS.Resources;
using EPS.Utilities;
using EPS.DataLayer;

public partial class TaskAllocation_Level2 : System.Web.UI.Page
{
    static protected string StatusWorkinProgress;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    public static string GetL2PendingChartData()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var chartWIPList = (IEnumerable<EPS.Entities.ChartInfo>)chartInfoBL.GetChartInfoByEmployee(profile.ClientProjectId, profile.UserName, profile.LevelNumber, false);

                var charts = chartWIPList.Select(p => new List<string>{
                    p.Id.ToString(), 
                    p.ClientReference.ToString(),
                    p.ReceivedDate.ToString("MM/dd/yyyy"),
                    p.ClientMarket.ToString(),
                    p.FileName.ToString(),
                    p.ClientProject.Client + " - " + p.ClientProject.Project,
                    p.ChartMoreInfo.LevelStatus.Name,
                    p.ChartMoreInfo.Id.ToString(),
                    p.ChartMoreInfo.LevelStatus.Name,
                    p.ChartMoreInfoId.ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(charts).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetL2ChartAuditData()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var chartWIPList = (IEnumerable<EPS.Entities.ChartInfo>)chartInfoBL.GetChartInfoByEmployee(profile.ClientProjectId, profile.UserName, profile.LevelNumber, false);

                var charts = chartWIPList.Select(p => new List<string>{
                    p.Id.ToString(), 
                    p.ClientReference.ToString(),
                    p.ReceivedDate.ToString("MM/dd/yyyy"),
                    p.ClientMarket.ToString(),
                    p.FileName.ToString(),
                    p.ClientProject.Client + " - " + p.ClientProject.Project,
                    p.ChartMoreInfo.LevelStatus.Name,
                    p.ChartMoreInfo.Id.ToString(),
                    p.ChartMoreInfo.LevelStatus.Name,
                    p.ChartMoreInfoId.ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(charts).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }




    [WebMethod(EnableSession = true)]
    public static string GetChartMoreInfoByChartId(int chartId)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var chartInfoList = chartInfoBL.GetChartMoreInfoByChartId(Convert.ToInt32(profile.ClientProjectId), chartId).Where(c => c.LevelNumber == 1);
                var chartPageNumbers = chartInfoBL.GetChartInfoById(chartId).NoOfPages;

                var charts = chartInfoList.Select(p => new List<string>{
                    chartPageNumbers.ToString(),
                    p.NumberOfDxCode.ToString()
                    
                }).ToList();

                var json = JsonConvert.SerializeObject(charts).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }




    [WebMethod(EnableSession = true)]
    public static string GetL2ChartStatuses()
    {
        try
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
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetL2ChartStatuses1()
    {
        try
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
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetL2StatusComments(int commentCategoryId)
    {
        try
        {
            using (var commentsBLObject = new EPS.BusinessLayer.Comments())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var comments = commentsBLObject.GetCommentsByCategory(profile.ClientProjectId, commentCategoryId, true).OrderBy(c => c.DisplayOrder)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Description.ToString(),
                    }).ToList();

                var json = JsonConvert.SerializeObject(comments).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetErrorCategories()
    {
        try
        {
            using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorCategories = lookupBLObject.GetLookupByCategory(profile.ClientProjectId, StringMessages.lookup_ErrorCategory)
                    .Where(l => l.IsActive == true)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Name.ToString(),
                    }).ToList();

                var json = JsonConvert.SerializeObject(errorCategories).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetErrorCategoriesUpdate()
    {
        try
        {
            using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorCategories = lookupBLObject.GetLookupByCategory(profile.ClientProjectId, StringMessages.lookup_ErrorCategory)
                    .Where(l => l.IsActive == true)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Name.ToString(),
                    }).ToList();

                var json = JsonConvert.SerializeObject(errorCategories).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string L2PendingChartCount()
    {
        try
        {

            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var pendingChartCountList = chartInfoBL.GetChartsPendingCount(profile.ClientProjectId, 2);
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
    public static string GetErrorDescriptions()
    {
        try
        {
            using (var commentsBLObject = new EPS.BusinessLayer.Comments())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorDescriptions = commentsBLObject.GetCommentsByCategory(profile.ClientProjectId, Convert.ToInt32(StringMessages.lookup_ErrorCategoryId), true).OrderBy(c => c.DisplayOrder)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Description.ToString(),
                    }).ToList();

                var json = JsonConvert.SerializeObject(errorDescriptions).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetErrorDescriptionsUpdate()
    {
        try
        {
            using (var commentsBLObject = new EPS.BusinessLayer.Comments())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorDescriptions = commentsBLObject.GetCommentsByCategory(profile.ClientProjectId, Convert.ToInt32(StringMessages.lookup_ErrorCategoryId), true).OrderBy(c => c.DisplayOrder)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Description.ToString(),
                    }).ToList();

                var json = JsonConvert.SerializeObject(errorDescriptions).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    [WebMethod(EnableSession = true)]
    public static string GetErrorSubCategories()
    {
        try
        {
            using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorSubcategories = lookupBLObject.GetLookupByCategory(profile.ClientProjectId, StringMessages.lookup_ErrorSubcategory)
                    .Where(l => l.IsActive == true)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Name.ToString(),
                    }).ToList();

                var json = JsonConvert.SerializeObject(errorSubcategories).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    [WebMethod(EnableSession = true)]
    public static string GetErrorSubCategoriesUpdate()
    {
        try
        {
            using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorSubcategories = lookupBLObject.GetLookupByCategory(profile.ClientProjectId, StringMessages.lookup_ErrorSubcategory)
                    .Where(l => l.IsActive == true)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Name.ToString(),
                    }).ToList();

                var json = JsonConvert.SerializeObject(errorSubcategories).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }




    [WebMethod(EnableSession = true)]
    public static string SaveL2DataChartInfo(Int64 chartId, Int64 chartMoreId, int levelStatusId, int levelStatusCommentId, int noOfPages, int noOfDxCodes, string overallStatus)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                //If the status is completed and no audit logs are present for the chart, then chart is said to be completed.
                if (levelStatusId == (int)Helper.ChartStatus.Completed)
                {
                    /*var chartAudits = chartInfoBL.GetChartAuditByChartId(chartId);
                    if (chartAudits.Count() == 0)*/
                    overallStatus = "Completed";
                }

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
    public static string SaveL2DataChartInfoUpdate(string filename, int clientprojectid, int levelnumber, int noofDxCodes, int noofPages)
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
    public static string SaveL2AuditComments(Int64 chartMoreInfoId, string pageNumbers, string correctedValue, string comments, int errorCategoryId, int errorSubCategoryId, string additionalComments)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var chartAudit = new EPS.Entities.ChartAudit()
                {
                    ChartMoreInfoId = chartMoreInfoId,
                    PageNumbers = pageNumbers,
                    CorrectedValue = correctedValue,
                    Comments = comments,
                    ErrorCategoryId = errorCategoryId,
                    ErrorSubCategoryId = errorSubCategoryId,
                    AdditionalComments = additionalComments
                };

                chartInfoBL.InsertChartAudit(chartAudit);
            }
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return "[\" 1 \"]";
    }

    [WebMethod(EnableSession = true)]
    public static string SaveL2AuditUpdateComments(Int64 ChartMoreInfoId, Int64 Id, string PageNumbers, string CorrectedValue, string Comments, int ErrorCategoryId, int ErrorSubCategoryId, string AdditionalComments)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var chartAuditComments = new EPS.Entities.ChartAuditComments()
                {
                    Id = Id,
                    ChartMoreInfoId = ChartMoreInfoId,
                    PageNumbers = PageNumbers,
                    CorrectedValue = CorrectedValue,
                    Comments = Comments,
                    ErrorCategoryId = ErrorCategoryId,
                    ErrorSubCategoryId = ErrorSubCategoryId,
                    AdditionalComments = AdditionalComments
                };

                chartInfoBL.UpdateChartAudit(chartAuditComments);
            }
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return "[\" 1 \"]";
    }


    [WebMethod(EnableSession = true)]
    public static string DeleteChartAuditUpdateComments(Int64 chartMoreInfoId, Int64 Id)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var chartAuditComments = new EPS.Entities.ChartAuditComments()
                {
                    Id = Id,
                    ChartMoreInfoId = chartMoreInfoId
                };

                chartInfoBL.DeleteChartAudit(chartAuditComments);
            }
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return "[\" 1 \"]";
    }



    [WebMethod(EnableSession = true)]
    public static string RequestL2Chart()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

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
    public static string RequestL2ChartByClientReference(string clientReference)
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
    public static string GetL2OtherChartData()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
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
                    p.ChartMoreInfo.LevelNumber.ToString(),
                    p.ChartMoreInfoId.ToString()                                        
                }).ToList();

                var json = JsonConvert.SerializeObject(charts).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    [WebMethod(EnableSession = true)]
    public static string GetChartAuditComments(string sChartMoreInfoId)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var chartAuditComments = chartInfoBL.GetChartAuditComments(sChartMoreInfoId);

                var charts = chartAuditComments.Select(p => new List<string>{
                    p.Id.ToString(),                    
                    p.PageNumbers.ToString(),
                    p.CorrectedValue.ToString(),
                    p.Comments.ToString(),                    
                    p.ErrorCategoryId.ToString(),
                    p.ErrorSubCategoryId.ToString(),
                    p.AdditionalComments.ToString(),
                    "Edit",
                    "Delete"
                }).ToList();

                var json = JsonConvert.SerializeObject(charts).ToString();
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
}