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

public partial class Admin_Clientconfiguration : System.Web.UI.Page
{
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
    public static string GetClientConfiguration(int clientProjectId)
    {
        try
        {
            using (var clientProjectObject = new EPS.BusinessLayer.ClientProject())
            {
                var clientProject = clientProjectObject.GetClientProjectById(clientProjectId);

                var clientProjectInfo = new List<String>
                {
                    string.Format("{0} - {1} - {2}", clientProject.Client, clientProject.Project, clientProject.Queue),
                    clientProject.IsL1Auto.ToString(),
                    clientProject.IsL2Auto.ToString(),
                    clientProject.IsL3Auto.ToString(),
                    HttpUtility.UrlDecode(clientProject.HomepageText),
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
    public static string UpdateClientProjectInfo(int IsL1Auto, int IsL2Auto, int IsL3Auto, string homePageContent, decimal factorialization)
    {
        try
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            using (var clientProjectObject = new EPS.BusinessLayer.ClientProject())
            {
                var clientprojectinfo = new EPS.Entities.ClientProject()
                {

                    Id = profile.ClientProjectId,
                    IsL1Auto = Convert.ToBoolean(IsL1Auto),
                    IsL2Auto = Convert.ToBoolean(IsL2Auto),
                    IsL3Auto = Convert.ToBoolean(IsL3Auto),
                    HomepageText = homePageContent,
                    UpdatedBy = profile.LoggedOnId,
                    Factorialization = factorialization                    

                };
                clientProjectObject.UpdateClientProject(clientprojectinfo);
            }
        }
        catch (Exception)
        {
            throw;
        }

        return "[\" 1 \"]";
    }
}