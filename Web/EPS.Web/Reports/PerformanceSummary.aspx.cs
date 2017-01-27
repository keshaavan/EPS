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
using System.Data;
using System.Web.Security;

public partial class Reports_PerformanceSummary_ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string GetPerformanceSummaryReport(int levelNumber, DateTime fromDate, DateTime toDate, string userName, string queueID)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                if (!profile.IsAdmin && string.IsNullOrEmpty(userName))
                    userName = profile.UserName;
                
                var dt = chartInfoBLObject.GetInfoForUserPerformance(queueID, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), string.IsNullOrEmpty(userName) ? null : userName, levelNumber, profile.IsByDxCode).Tables[0];


                var lstItem = dt.AsEnumerable().Select(p => new List<string> { 
                    p.Field<string>(DBResources.col_FirstName) + ' ' + p.Field<string>(DBResources.col_LastName) + '_' + p.Field<string>(DBResources.col_UserName),
                    p.Field<int>(DBResources.col_LevelNumber).ToString(),
                    p.Field<int>(DBResources.col_CompletedChartsCount).ToString(),
                    p.Field<int>(DBResources.col_InvalidChartsCount).ToString(),
                    (levelNumber == 2) ? "NA": p.Field<int>(DBResources.col_ErrorChartsCount).ToString(),
                    (levelNumber == 2) ? "NA": p.Field<int>(DBResources.col_TotalCharts).ToString(),
                    (levelNumber == 2) ? "NA": p.Field<Decimal>(DBResources.col_QualityControl).ToString("0.00"),
                    p.Field<string>(DBResources.col_Queue),                                        
                    p.Field<Int32>(DBResources.col_chartaudited).ToString(),
                    p.Field<Double>(DBResources.col_QC).ToString()
                   
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
    public static string getEmployeeInfo()
    {
        try
        {
            using (var employeeBL = new EPS.BusinessLayer.EmployeeInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var employeelist = employeeBL.GetEmployees(profile.ClientProjectId).ToList();

                var empList = new List<Employee>();
                var empLevelList = new List<EmployeeLevel>();

                foreach (var employee in employeelist)
                {
                    var emp = new Employee()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        UserName = employee.UserName
                    };

                    empList.Add(emp);
                }

                var jsonList = (from e in empList
                                select new List<string>
                                {
                                    e.UserName,
                                    e.FirstName + " " + e.LastName + "_" + e.UserName
                                });

                var json = JsonConvert.SerializeObject(jsonList).ToString();
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
    public static string GetQueue()
    {
        var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
        DataSet objDS;

        try
        {
            using (EPS.DataLayer.SiteMaster objSM = new EPS.DataLayer.SiteMaster())
            {
                objDS = objSM.getQueues(profile.ClientProjectId, profile.EmployeeId);
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