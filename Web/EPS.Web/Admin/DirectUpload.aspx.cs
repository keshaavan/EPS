using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;

using Newtonsoft.Json;
using System.Web.Script.Services;

using System.Web.Security;

public partial class Admin_DirectUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string GetChartsForDirectUpload(string fromDate, string toDate)
    {
        using (var chartInfoBLObject = new EPS.BusinessLayer.ChartInfo())
        {
            var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

            var chartsDirectUpload = (IEnumerable<EPS.Entities.ChartInfo>)chartInfoBLObject.GetChartsForDirectUpload(profile.ClientProjectId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
            
            var aaList = chartsDirectUpload.Select(p => new List<string>{
                    "",
                    p.Id.ToString(), 
                    p.ReceivedDate.ToString("MM/dd/yyyy"),
                    p.ClientReference.ToString(),
                    p.ClientMarket.ToString(),
                    p.FileName.ToString(),
                    (string.IsNullOrEmpty(p.ChartMoreInfo.Employee.UserName) == true) ? "" : string.Format("{0} {1}_{2}", p.ChartMoreInfo.Employee.FirstName, p.ChartMoreInfo.Employee.LastName, p.ChartMoreInfo.Employee.UserName)
                }).ToList();

            var json = JsonConvert.SerializeObject(aaList).ToString();
            return json;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string DirectUploadCharts(string chartIds)
    {
        try
        {
            using (var chartInfoBL = new EPS.BusinessLayer.ChartInfo())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                List<string> chartIdList = new List<string>(chartIds.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));

                chartInfoBL.DirectUploadCharts(profile.ClientProjectId, ConfigurationHelper.EPSSysUserName, chartIdList);

                return "[\" 1 \"]";
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