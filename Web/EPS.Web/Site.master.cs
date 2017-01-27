using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Data;

using EPS.Entities;
using EPS.Resources;
using EPS.DataLayer;
using System.Web.Profile;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    public string EmployeeId = string.Empty;
    public string FirstName = string.Empty;
    public string LastName = string.Empty;
    public string UserName = string.Empty;
    public string LocationId = string.Empty;
    public string LoggedOnId = string.Empty;
    public string ClientProjectId = string.Empty;
    public string LevelNumber = string.Empty;
    public string IsAdmin = string.Empty;
    public string IsReportUser = string.Empty;
    public string Client = string.Empty;
    public string Project = string.Empty;
    public string Queue = string.Empty;
    public string QueueKey = string.Empty;
    public string ClientKey = string.Empty;
    public string ProjectKey = string.Empty;
    public string DisplayName = string.Empty;
    public string IsByDxCode = string.Empty;
    public string ClientReferenceLabel = string.Empty;
    public string IsL1Auto = string.Empty;
    public string IsL2Auto = string.Empty;
    public string IsL3Auto = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet objDS;

        var Profile = (ProfileBase)Session["Profile"];


        //ClientReferenceLabel = ((ProfileCommon)(Profile)).ClientReferenceLabel;
        //IsAdmin = Convert.ToString(((ProfileCommon)(Profile)).IsAdmin);
        //IsReportUser = Convert.ToString(((ProfileCommon)(Profile)).IsReportUser);
        //UserName = Convert.ToString(((ProfileCommon)(Profile)).UserName);
        //LevelNumber = Convert.ToString(((ProfileCommon)(Profile)).LevelNumber);
        //ClientProjectId = Convert.ToString(((ProfileCommon)(Profile)).ClientProjectId);
        //IsL1Auto = Convert.ToString(((ProfileCommon)(Profile)).IsL1Auto);
        //IsL2Auto = Convert.ToString(((ProfileCommon)(Profile)).IsL2Auto);
        //IsL3Auto = Convert.ToString(((ProfileCommon)(Profile)).IsL3Auto);
        //ClientKey = Convert.ToString(((ProfileCommon)(Profile)).ClientKey);

        EmployeeId = Convert.ToString(((ProfileCommon)(Profile)).EmployeeId);
        FirstName = ((ProfileCommon)(Profile)).FirstName;
        LastName = ((ProfileCommon)(Profile)).LastName;
        UserName = ((ProfileCommon)(Profile)).UserName;
        LocationId =Convert.ToString(((ProfileCommon)(Profile)).LocationId);
        LoggedOnId = Convert.ToString( ((ProfileCommon)(Profile)).LoggedOnId);
        ClientProjectId=Convert.ToString(((ProfileCommon)(Profile)).ClientProjectId);
        LevelNumber =Convert.ToString( ((ProfileCommon)(Profile)).LevelNumber);
        IsAdmin =Convert.ToString( ((ProfileCommon)(Profile)).IsAdmin);
        IsReportUser =Convert.ToString( ((ProfileCommon)(Profile)).IsReportUser);
        Client = ((ProfileCommon)(Profile)).Client;
        Project =((ProfileCommon)(Profile)).Project;
        Queue = ((ProfileCommon)(Profile)).Queue;
        QueueKey =((ProfileCommon)(Profile)).QueueKey;
        ClientKey =((ProfileCommon)(Profile)).ClientKey;
        ProjectKey =((ProfileCommon)(Profile)).ProjectKey;
        DisplayName =((ProfileCommon)(Profile)).DisplayName;
        IsByDxCode =Convert.ToString( ((ProfileCommon)(Profile)).IsByDxCode);
        ClientReferenceLabel=((ProfileCommon)(Profile)).ClientReferenceLabel;
        IsL1Auto =Convert.ToString( ((ProfileCommon)(Profile)).IsL1Auto);
        IsL2Auto =Convert.ToString( ((ProfileCommon)(Profile)).IsL2Auto);
        IsL3Auto =Convert.ToString( ((ProfileCommon)(Profile)).IsL3Auto);

        using (EPS.DataLayer.SiteMaster objSM = new EPS.DataLayer.SiteMaster())
        {
            objDS = objSM.getQueues(((ProfileCommon)(Profile)).ClientProjectId, ((ProfileCommon)(Profile)).EmployeeId);
        }

        repQueues.DataSource = objDS.Tables[0];
        repQueues.DataBind();
        // HttpContext.Current.User.Identity.IsAuthenticated = true;
    }
}

