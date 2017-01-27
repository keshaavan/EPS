using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Security;

using System.Web.Script.Services;

using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;
using System.Data;

public partial class Admin_UserMapping : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    [WebMethod(EnableSession = true)]
    public static string GetUserDetails(string UserName)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);                
                var dt = chartInfoBL.GetUserDetails().Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> {                     
                    p.Field<int>(DBResources.col_Id).ToString(),
                    p.Field<string>(DBResources.col_UserName).ToString(),
                    p.Field<string>(DBResources.col_Project).ToString()
                    
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
    public static string GetActiveProjectList(string UserName)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(UserName);
                var dt = chartInfoBL.GetActiveProjectList().Tables[0];

                var lstItem = dt.AsEnumerable().Select(p => new List<string> {                     
                    p.Field<int>(DBResources.col_Id).ToString(),                    
                    p.Field<string>(DBResources.col_Project).ToString()
                    
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
    //ActiveInactiveUserMapping


    //[WebMethod(EnableSession = true)]
    //public static string ActiveInactiveUserMapping(int selectedCheckBox, int drpValue, string username)
    //{
    //    try
    //    {
    //        using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
    //        {
    //            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
    //            var UserMappingObject = new EPS.Entities.UserMapping()
    //            {
    //                InactiveProjectId = selectedCheckBox,
    //                ActiveProjectId = drpValue,
    //                Username = username,                   
    //            };

    //            chartInfoBL.ActiveInactiveUserMapping(UserMappingObject);
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        return "[\" 0 \"]";
    //    }

    //    return "[\" 1 \"]";
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