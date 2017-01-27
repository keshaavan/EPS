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

public partial class Reports_ProductionStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod(EnableSession = true)]
    public static string GetProductionStatisticsData(DateTime fromDate, DateTime toDate, string queueId)
    {
        try
        {
            using (var clientProjectBLObject = new EPS.BusinessLayer.ClientProject())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                
                var dt = clientProjectBLObject.GetProductionStatistics(queueId, fromDate, toDate).Tables[0];

                var productionStatus = dt.AsEnumerable().Select(p=>new List<string> { 
                    p.Field<DateTime>(DBResources.col_ProcessedDate).ToString("MM/dd/yyyy"),
                    p.Field<int>(DBResources.col_ImportedCharts).ToString(),
                    p.Field<int>(DBResources.col_UnassignedCharts).ToString(),
                    p.Field<int>(DBResources.col_CompletedCharts).ToString(),
                    p.Field<int>(DBResources.col_L1WorkInProgress).ToString(),
                    p.Field<int>(DBResources.col_L1Pending).ToString(),
                    p.Field<int>(DBResources.col_L1Completed).ToString(),
                    p.Field<int>(DBResources.col_L1Hold).ToString(),
                    p.Field<int>(DBResources.col_L1Invalid).ToString(),
                    p.Field<int>(DBResources.col_L2WorkInProgress).ToString(),
                    p.Field<int>(DBResources.col_L2Pending).ToString(),
                    p.Field<int>(DBResources.col_L2Completed).ToString(),
                    p.Field<int>(DBResources.col_L2Hold).ToString(),
                    p.Field<int>(DBResources.col_L2Invalid).ToString(),
                    p.Field<int>(DBResources.col_L3WorkInProgress).ToString(),
                    p.Field<int>(DBResources.col_L3Pending).ToString(),
                    p.Field<int>(DBResources.col_L3Completed).ToString(),
                    p.Field<int>(DBResources.col_L3Hold).ToString(),
                    p.Field<int>(DBResources.col_L3Invalid).ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(productionStatus).ToString();
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
                objDS = objSM.getQueues(profile.ClientProjectId,profile.EmployeeId);
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