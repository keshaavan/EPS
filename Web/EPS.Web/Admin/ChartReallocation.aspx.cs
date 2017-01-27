using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using EPS.Entities;
using EPS.Resources;

using Newtonsoft.Json;
using System.Web.Script.Services;

using System.Web.Security;

public partial class Admin_ChartReallocation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    public static string getWIPChartData(string UserName, string ClientProjectId)
    {
        try
        {
            var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var chartWIPList = chartInfoBLObject.GetChartInfoForReallocation(Convert.ToInt32(ClientProjectId));
                var charts = chartWIPList.Select(p => new List<string>{
                    "Edit",
                    p.Id.ToString(), 
                    p.ClientReference.ToString(),
                    p.ReceivedDate.ToString("MM/dd/yyyy"),
                    p.ClientMarket.ToString(),
                    p.FileName.ToString(),
                    p.ClientProject.Client + " - " + p.ClientProject.Project +" - " + p.ClientProject.Queue,
                    p.ChartMoreInfo.LevelStatus.Name,
                    p.ChartMoreInfo.Id.ToString(),
                    p.ChartMoreInfo.Employee.UserName,
                    p.ChartMoreInfo.Employee.FirstName + " " + p.ChartMoreInfo.Employee.LastName + "_" + p.ChartMoreInfo.Employee.UserName,
                    p.ChartMoreInfo.Employee.Id.ToString(),
                    p.ClientProjectId.ToString(),
                    p.ChartMoreInfo.LevelNumber.ToString(),
                    ((p.PreviousChartMoreInfo != null)? p.PreviousChartMoreInfo.Employee.UserName : "")
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
    public static string getEmployeeData(int clientProjectId, int levelNumber, string chartUserName, string prevUserName, string UserName)
    {
        try
        {
            using (var employeeBL = new EPS.BusinessLayer.EmployeeInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);

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
                        LocationId = employee.LocationId,
                        isActive = employee.isActive,
                        Location = employee.Location,
                        UserName = employee.UserName
                    };

                    empList.Add(emp);

                    foreach (var employeeLevel in employee.AdminEmployeeLevels)
                    {
                        var empLevel = new EmployeeLevel()
                        {
                            Id = employeeLevel.Id,
                            EmployeeId = emp.Id,
                            ClientProjectId = employeeLevel.ClientProjectId,
                            ClientProject = employeeLevel.ClientProject,
                            LevelNumber = employeeLevel.LevelNumber,
                            isActive = employeeLevel.isActive,
                            isAdmin = employeeLevel.isAdmin
                        };

                        empLevelList.Add(empLevel);
                    }
                }

                var jsonList = (from e in empList
                                join el in empLevelList on e.Id equals el.EmployeeId
                                where el.ClientProjectId == clientProjectId && el.LevelNumber == levelNumber && el.isActive == true && e.UserName != chartUserName
                                    && e.UserName != prevUserName
                                select new List<string>
                                {
                                    el.Id.ToString(),
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
    public static string AssignChartWithEmployee(Int64 chartId, Int64 assignToEmployeeId, int clientProjectId, string levelStatus, string previousUserName, string UserName)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);
               
                var chartWIPList = chartInfoBL.GetChartInfoForReallocation(profile.ClientProjectId);
                if (chartWIPList == null)
                    return "[\" 0 \"]";

                var chartinfo = chartWIPList.Where(p => p.Id == chartId).FirstOrDefault();
                var chartInfoObject = new EPS.Entities.ChartInfo()
                {
                    Id = chartinfo.Id,
                    ClientProjectId = chartinfo.ClientProjectId,
                    NoOfPages = chartinfo.NoOfPages == null ? 0 : chartinfo.NoOfPages,
                    OverallStatus = levelStatus,
                    ChartMoreInfo = new EPS.Entities.ChartMoreInfo()
                    {
                        Id = chartinfo.ChartMoreInfo.Id,
                        ChartId = chartId,
                        LevelStatusId = chartinfo.ChartMoreInfo.LevelStatusId,
                        LevelStatusCommentId = chartinfo.ChartMoreInfo.LevelStatusCommentId,
                        LevelNumber = chartinfo.ChartMoreInfo.LevelNumber,
                        LevelUpdatedBy = assignToEmployeeId,
                        NumberOfDxCode = chartinfo.ChartMoreInfo.NumberOfDxCode
                    }
                };

                chartInfoBL.UpdateChartInfoStatus(chartInfoObject, chartinfo.ChartMoreInfo.Employee.EmployeeLevel.Id);

                return "[\" 1 \"]";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //[WebMethod(EnableSession = true)]
    //public static string IsUserAuthorized(string userName)
    //{
    //    if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) || HttpContext.Current.User.Identity.Name.ToLower() != userName.ToLower())
    //        return JsonConvert.SerializeObject(false).ToString();

    //    ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
    //    if (profile != null && !profile.IsAdmin)
    //        return JsonConvert.SerializeObject(false).ToString();

    //    MembershipUser User = Membership.GetUser(profile.UserName, true);
    //    if (User.LastPasswordChangedDate < DateTime.Now.AddDays(EPS.Utilities.ConfigurationHelper.PasswordExpiryPeriod))
    //        return JsonConvert.SerializeObject("ChangePassword");

    //    return JsonConvert.SerializeObject(true).ToString();
    //}

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
