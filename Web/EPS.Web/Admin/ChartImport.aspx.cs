using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI.HtmlControls;

using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

public partial class Admin_ChartImport : System.Web.UI.Page
{
    public static Page page;
    protected void Page_Load(object sender, EventArgs e)
    {
        page = this.Page;
    }

    [WebMethod(EnableSession = true)]
    public static string UploadFile(object FileUpload)
    {
        return "";
    }

    [WebMethod(EnableSession = true)]
    public static string IsUserAuthorized(string userName)
    {
        //if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) || HttpContext.Current.User.Identity.Name.ToLower() != userName.ToLower())
        //    return JsonConvert.SerializeObject(false).ToString();

        ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(userName);
        if (profile != null && !profile.IsAdmin)
            return JsonConvert.SerializeObject(false).ToString();

        MembershipUser User = Membership.GetUser(profile.UserName, true);
        if (User.LastPasswordChangedDate < DateTime.Now.AddDays(EPS.Utilities.ConfigurationHelper.PasswordExpiryPeriod))
            return JsonConvert.SerializeObject("ChangePassword");

        return JsonConvert.SerializeObject(true).ToString();
    }
}
