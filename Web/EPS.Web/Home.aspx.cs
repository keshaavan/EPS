using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.Profile;

using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

using EPS.BusinessLayer;
using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;

public partial class Home : System.Web.UI.Page
{


    //CompletedChartcount
    [WebMethod(EnableSession = true)]
    public static string GetCompletedChartCount(DateTime sfromDate, DateTime stoDate)
    {
        try
        {
            var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            
            using (var clientProjectObject = new EPS.BusinessLayer.ClientProject())
            {
                var clientProject = clientProjectObject.GetCompletedChartCount(profile.EmployeeId, profile.ClientProjectId, profile.LevelNumber, Convert.ToDateTime(sfromDate), Convert.ToDateTime(stoDate));
                var json = JsonConvert.SerializeObject(clientProject).ToString();
                return json;                
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
    public static string GetHomePageText()
    {
        try
        {
            var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

            using (var clientProjectObject = new EPS.BusinessLayer.ClientProject())
            {
                var clientProject = clientProjectObject.GetClientProjectById(profile.ClientProjectId);

                var clientProjectInfo = new List<String>
                {
                    clientProject.HomepageText,
                    clientProject.Factorialization.ToString()
                };

                var json = JsonConvert.SerializeObject(clientProjectInfo).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string UpdatePassword(string oldPassword, string newPassword)
    {
        try
        {
            var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var employeeBL = new EmployeeInfo();
            var result = employeeBL.UpdatePassword(profile.UserName, oldPassword, newPassword, ConfigurationHelper.PerviousPasswordCount);

            if (result)
                return "[\" 1 \"]";
            else
                return "[\" 0 \"]";
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
    public static string IsUserAuthorized(string userName)
    {
        //if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) || HttpContext.Current.User.Identity.Name.ToLower() != userName.ToLower())
        //    return JsonConvert.SerializeObject(false).ToString();

        //var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
        var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(userName);
        MembershipUser User = Membership.GetUser(userName, true);
        if (User.LastPasswordChangedDate < DateTime.Now.AddDays(EPS.Utilities.ConfigurationHelper.PasswordExpiryPeriod))
            return JsonConvert.SerializeObject("ChangePassword");

        return JsonConvert.SerializeObject(true).ToString();
    }

    [WebMethod(EnableSession = true)]
    public static string PasswordExpired(string userName)
    {
        var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(userName);
        return EPI.Compliance.ASPNetMembership.PasswordExpired(userName);
    }





}