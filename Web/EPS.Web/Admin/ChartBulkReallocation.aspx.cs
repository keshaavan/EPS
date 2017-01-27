using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;


using EPS.Utilities;

using Newtonsoft.Json;
using EPS.Entities;
using EPS.BusinessLayer;
using EPS.Resources;
using EPS.Utilities;


public partial class Admin_ChartBulkReallocation : System.Web.UI.Page
{
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
    public static string LoadChartData( int clientprojectid, int datatype, DateTime  fromDate, DateTime  toDate, string username, string fromstatusId)        
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var dt = chartInfoBLObject.GetChartData(clientprojectid,(EPS.Utilities.Helper.ChartDateTypes)datatype, fromDate, toDate, username, fromstatusId).Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> {                        
                        p.Field<int>(DBResources.col_Id).ToString(),
                        p.Field<int>(DBResources.col_ClientProjectId).ToString(),
                        p.Field<string>(DBResources.col_ClientReference).ToString(),
                        p.Field<string>(DBResources.col_ReceivedDate).ToString(),
                        p.Field<string>(DBResources.col_ClientMarket).ToString(),
                        p.Field<string>(DBResources.col_FileName).ToString(),
                        p.Field<string>(DBResources.col_OverallStatus).ToString(),
                        p.Field<string>(DBResources.col_UserName).ToString()
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
    public static string getCharttoStatuses(int id)
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);


                var dt = chartInfoBLObject.GetCharttostatus(id).Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> {   
                        p.Field<string>(DBResources.col_FromStatus).ToString()                        
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
    public static string getChartStatuses()
    {
        try
        {
            using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);


                var dt = chartInfoBLObject.GetChartFromstatus().Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> {   
                        p.Field<int>(DBResources.col_FromStatusId).ToString(),
                        p.Field<string>(DBResources.col_FromStatus).ToString()
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


}
public class FromStatus
{
    public string id;
    public string Name;
}