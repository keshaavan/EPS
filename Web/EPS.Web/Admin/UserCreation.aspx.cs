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
using System.Web.Security;

using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

using EPS.Entities;
using EPS.BusinessLayer;
using EPS.Resources;
using EPS.Utilities;

public partial class Admin_UserCreation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    public static string getEmployeeData(string userName)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(userName);
                string Action = "Edit";
                string PasswordReset = "Reset";
                var dt = chartInfoBL.GetUserDetails().Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> {                     
                    Action.ToString(),
                    PasswordReset.ToString(),
                    p.Field<int>(DBResources.col_Id).ToString(),
                    p.Field<string>(DBResources.col_UserName).ToString(),
                    p.Field<string>(DBResources.col_FirstName).ToString(),
                    p.Field<string>(DBResources.col_LastName).ToString(),                                       
                    p.Field<string>(DBResources.col_Project).ToString(),
                   // p.Field<Boolean>(DBResources.col_ProjectStatus).ToString(),
                    p.Field<string>(DBResources.col_Queue).ToString(),
                    p.Field<string>(DBResources.col_LocationName).ToString(),
                    p.Field<int>(DBResources.col_LevelNumber).ToString(),
                    p.Field<Boolean>(DBResources.col_IsAdmin).ToString(),
                    p.Field<Boolean>(DBResources.col_IsReportUser).ToString(),
                    p.Field<Boolean>(DBResources.col_Status).ToString(),
                    p.Field<Guid>(DBResources.col_EmployeeId).ToString(),
                    p.Field<int>(DBResources.col_LocationId).ToString(),
                    p.Field<Boolean>(DBResources.col_isLocked).ToString()                                        

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
    public static string getClientProjectData()
    {
        try
        {
            using (var clientProject = new EPS.BusinessLayer.ClientProject())
            {
                var clientProjectList = clientProject.GetAllClientProjects(true).Select(p => new List<string> { p.Id.ToString(), p.Client + " - " + p.Project.ToString() + " - " + p.Queue }).ToList();
                //var clientProjectList = clientProject.GetAllClientProjectsdrp().Select(p => new List<string> { p.Id.ToString(), p.Client + " - " + p.Project.ToString() + " - " + p.Queue }).ToList();

                var json = JsonConvert.SerializeObject(clientProjectList).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string getLocationtData()
    {
        try
        {
            using (var projectLocationsObject = new EPS.BusinessLayer.Lookup())
            {
                var locations = projectLocationsObject.GetLookupByCategory(null, StringMessages.lookup_Location)
                    .Where(l => l.IsActive == true)
                    .Select(p => new List<string> { p.Id.ToString(), p.Name.ToString() }).ToList();

                var json = JsonConvert.SerializeObject(locations).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string UpdateEmployeeinfo(Guid employeeId, string firstName, string lastName, int locationId, int levelNumber, int clientProjectId, int isAdmin, int isReportUser, int isActive)
    {
        try
        {
            using (var employeeBL = new EmployeeInfo())
            {
                var employee = new EPS.Entities.Employee()
                {
                    Id = employeeId,
                    FirstName = firstName.Trim(),
                    LastName = lastName.Trim(),

                    LocationId = locationId,
                    isActive = true,

                    EmployeeLevel = new EPS.Entities.EmployeeLevel()
                    {
                        EmployeeId = employeeId,
                        LevelNumber = levelNumber,
                        ClientProjectId = clientProjectId,
                        isAdmin = Convert.ToBoolean(isAdmin),
                        IsReportUser = Convert.ToBoolean(isReportUser),
                        isActive = Convert.ToBoolean(isActive)
                    }
                };

                employeeBL.InsertUpdateEmployee(employee);
            }
        }
        catch (Exception)
        {
            throw;
        }

        return "[\" 1 \"]";
    }

    [WebMethod(EnableSession = true)]
    public static string ResetPassword(string userName, string newPassword)
    {
        try
        {
            var employeeBL = new EmployeeInfo();
            var result = employeeBL.ResetPassword(userName, newPassword, ConfigurationHelper.PerviousPasswordCount);

            if (result)
            {
                return "[\" 1 \"]";
            }
            else
            {
                return "[\" 0 \"]";
            }
        }
        catch (ApplicationException ex)
        {
            return string.Format("[\" {0} \"]", ex.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string UnlockAccount(string userName)
    {
        try
        {
            EPI.Compliance.ASPNetMembership.UnlockAccount(userName);

            return "[\" 1 \"]";
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