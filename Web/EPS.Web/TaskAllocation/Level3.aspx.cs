using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;

using Newtonsoft.Json;

using EPS.Entities;
using EPS.BusinessLayer;
using EPS.Resources;
using EPS.Utilities;

public partial class TaskAllocation_Level3 : System.Web.UI.Page
{

    static protected string StatusWorkinProgress;
    protected void Page_Load(object sender, EventArgs e)
    {

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
    public static string GetL3PendingChartData()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var chartWIPList = (IEnumerable<EPS.Entities.ChartInfo>)chartInfoBL.GetChartInfoByEmployee(profile.ClientProjectId, profile.UserName, profile.LevelNumber, false);
                var clientProject = new EPS.BusinessLayer.ClientProject().GetClientProjectById(profile.ClientProjectId);

                var l1ChartMoreInfoes = new List<EPS.Entities.ChartMoreInfo>();
                var l2ChartMoreInfoes = new List<EPS.Entities.ChartMoreInfo>();

                foreach (var chartInfo in chartWIPList)
                {
                    var l1ChartMoreInfo = new EPS.BusinessLayer.ChartInfo().GetChartMoreInfoByChartIdAndLevel(clientProject, chartInfo.Id, 1);
                    if (l1ChartMoreInfo != null)
                        l1ChartMoreInfoes.Add(l1ChartMoreInfo);

                    var l2ChartMoreInfo = new EPS.BusinessLayer.ChartInfo().GetChartMoreInfoByChartIdAndLevel(clientProject, chartInfo.Id, 2);
                    if (l2ChartMoreInfo != null)
                        l2ChartMoreInfoes.Add(l2ChartMoreInfo);
                }

                var charts = (from cwl in chartWIPList
                              join l1 in l1ChartMoreInfoes on cwl.Id equals l1.ChartId
                              join l2 in l2ChartMoreInfoes on cwl.Id equals l2.ChartId
                              select new List<string>{
                                cwl.Id.ToString(), 
                                cwl.ChartMoreInfo.LevelStatus.Name,
                                cwl.ChartMoreInfo.LevelStatus.Name,
                                l2.Id.ToString(),
                                cwl.ClientReference.ToString(),
                                cwl.ReceivedDate.ToString("MM/dd/yyyy"),
                                cwl.ClientMarket.ToString(),
                                cwl.FileName.ToString(),
                                cwl.ClientProject.Client + " - " + cwl.ClientProject.Project,
                                l2.LevelStatus.Name,
                                l2.LevelStatusComment.Description,
                                l2.NumberOfDxCode.ToString(),
                                l1.LevelStatus.Name,
                                l1.LevelStatusComment.Description,
                                l1.NumberOfDxCode.ToString(),
                                cwl.ChartMoreInfo.Id.ToString(),
                                cwl.ChartMoreInfo.LevelStatus.Name,
                                (string.IsNullOrEmpty(l2.Employee.UserName)==true)? "" : string.Format("{0} {1}_{2}", l2.Employee.FirstName, l2.Employee.LastName, l2.Employee.UserName)
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
    public static string RequestL3Chart()
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
    public static string L3PendingChartCount()
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
    public static string RequestL3ChartByClientReference(string clientReference)
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
    public static string GetL3ChartStatuses()
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
    public static string GetL3StatusComments(int commentCategoryId)
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
    public static string SaveL3DataChartInfo(Int64 chartId, Int64 chartMoreId, int levelStatusId, int levelStatusCommentId, int noOfPages, int noOfDxCodes, string overallStatus)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                //If the status is completed, then chart is said to be completed.
                if (levelStatusId == (int)Helper.ChartStatus.Completed)
                {
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
    public static string GetL3ErrorCategories()
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
    public static string GetL3ErrorDescriptions()
    {
        try
        {
            using (var commentsBLObject = new EPS.BusinessLayer.Comments())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorDescriptions = commentsBLObject.GetCommentsByCategory(profile.ClientProjectId, Convert.ToInt32(StringMessages.lookup_ErrorCategoryId), true)
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
    public static string GetL3ErrorSubCategories()
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
    public static string SaveL3AuditComments(Int64 chartMoreInfoId, string pageNumbers, string correctedValue, string comments, int errorCategoryId, int errorSubCategoryId, string additionalComments)
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
                    AdditionalComments = additionalComments,
                    L3LevelDisagreeId = new Nullable<int>()
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
    public static string GetL3OtherChartData()
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var chartWIPList = (IEnumerable<EPS.Entities.ChartInfo>)chartInfoBL.GetChartInfoByEmployee(profile.ClientProjectId, profile.UserName, profile.LevelNumber, true);
                var clientProject = new EPS.BusinessLayer.ClientProject().GetClientProjectById(profile.ClientProjectId);

                var l1ChartMoreInfoes = new List<EPS.Entities.ChartMoreInfo>();
                var l2ChartMoreInfoes = new List<EPS.Entities.ChartMoreInfo>();

                foreach (var chartInfo in chartWIPList)
                {
                    var l1ChartMoreInfo = new EPS.BusinessLayer.ChartInfo().GetChartMoreInfoByChartIdAndLevel(clientProject, chartInfo.Id, 1);
                    if (l1ChartMoreInfo != null)
                        l1ChartMoreInfoes.Add(l1ChartMoreInfo);

                    var l2ChartMoreInfo = new EPS.BusinessLayer.ChartInfo().GetChartMoreInfoByChartIdAndLevel(clientProject, chartInfo.Id, 2);
                    if (l2ChartMoreInfo != null)
                        l2ChartMoreInfoes.Add(l2ChartMoreInfo);
                }

                var charts = (from cwl in chartWIPList
                              join l1 in l1ChartMoreInfoes on cwl.Id equals l1.ChartId
                              join l2 in l2ChartMoreInfoes on cwl.Id equals l2.ChartId
                              select new List<string>{
                                cwl.Id.ToString(), 
                                cwl.ChartMoreInfo.LevelStatus.Name,
                                cwl.ChartMoreInfo.LevelStatus.Name,
                                l2.Id.ToString(),
                                cwl.ClientReference.ToString(),
                                cwl.ReceivedDate.ToString("MM/dd/yyyy"),
                                cwl.ClientMarket.ToString(),
                                cwl.FileName.ToString(),
                                cwl.ClientProject.Client + " - " + cwl.ClientProject.Project,
                                l2.LevelStatus.Name,
                                l2.LevelStatusComment.Description,
                                l2.NumberOfDxCode.ToString(),
                                l1.LevelStatus.Name,
                                l1.LevelStatusComment.Description,
                                l1.NumberOfDxCode.ToString(),
                                cwl.ChartMoreInfo.Id.ToString(),
                                 (string.IsNullOrEmpty(l2.Employee.UserName)==true)? "" : string.Format("{0} {1}_{2}", l2.Employee.FirstName, l2.Employee.LastName, l2.Employee.UserName)
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
    public static string GetChartAuditLogsByChartId(Int64 chartId)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var dt = chartInfoBLObject.GetChartAuditByChartId(chartId);

                var lstItem = dt.AsEnumerable().Select(p => new List<string> { 
                      "",
                      (p.L3LevelDisagreeId.HasValue == true) ? p.L3LevelDisagreeId.Value.ToString() : "0",
                      p.PageNumbers,
                      p.CorrectedValue,
                      p.Comments,
                      p.ErrorCategory.Name,
                      p.ErrorSubCategory.Name,
                      p.AdditionalComments,
                      p.LastUpdatedDate.ToString(),
                      p.ChartMoreInfo.LevelNumber.ToString(),
                      (p.L3LevelDisagreeId.HasValue == true) ? p.L3LevelDisagree.Name : "",
                      (p.L3LevelDisagreeId.HasValue == true) ? p.L3LevelDisagreeId.Value.ToString() : "0",
                      p.Id.ToString(),
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
    public static string GetLevelDisagreeOptions()
    {
        try
        {
            using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var errorCategories = lookupBLObject.GetLookupByCategory(null, StringMessages.lookup_L3LevelDisagree)
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
    public static string SaveChartAuditList(string chartAuditIds)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                List<string> chartAuditIdList = new List<string>(chartAuditIds.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));

                chartInfoBL.UpdateL3ChartAuditLevelDisagreement(profile.ClientProjectId, chartAuditIdList);

                return "[\" 1 \"]";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    [WebMethod(EnableSession = true)]
    public static string GetL1andL2ChartMoreInfoByChartId(int chartId)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var chartInfoList = chartInfoBL.GetChartMoreInfoByChartId(Convert.ToInt32(profile.ClientProjectId), chartId).Where(c => c.LevelNumber != 3 );
                    //.Where(c => c.LevelNumber == 2 && c.LevelNumber == 1);
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
}