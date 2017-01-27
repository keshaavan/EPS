using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.UI;
using System.Web.UI.WebControls;

using EPS.BusinessLayer;
using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;

public partial class QueueRedirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            string sNewClientProject = Request.QueryString["id"];

            var employeeBLObject = new EmployeeInfo();
            var employee = employeeBLObject.GetEmployeeinProject(int.Parse(sNewClientProject), HttpContext.Current.User.Identity.Name);

            var profileBase = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            //var profileBase = ProfileBase.Create(LoginUser.UserName);

            profileBase[DBResources.col_EmployeeId] = employee.Id.ToString();
            profileBase[DBResources.col_FirstName] = employee.FirstName;
            profileBase[DBResources.col_LastName] = employee.LastName;
            profileBase[DBResources.col_UserName] = employee.UserName;
            profileBase[DBResources.col_LocationId] = employee.LocationId;

            profileBase[DBResources.col_LoggedOnId] = employee.EmployeeLevel.Id;
            profileBase[DBResources.col_ClientProjectId] = employee.EmployeeLevel.ClientProjectId;
            profileBase[DBResources.col_LevelNumber] = employee.EmployeeLevel.LevelNumber;
            profileBase[DBResources.col_IsAdmin] = employee.EmployeeLevel.isAdmin;
            profileBase[DBResources.col_IsReportUser] = employee.EmployeeLevel.IsReportUser;

            profileBase[DBResources.col_Client] = employee.EmployeeLevel.ClientProject.Client;
            profileBase[DBResources.col_Project] = employee.EmployeeLevel.ClientProject.Project;
            profileBase[DBResources.col_Queue] = employee.EmployeeLevel.ClientProject.Queue;
            profileBase[DBResources.col_QueueKey] = employee.EmployeeLevel.ClientProject.QueueKey;
            profileBase[DBResources.col_ClientReferenceLabel] = employee.EmployeeLevel.ClientProject.ClientReferenceLabel;
            profileBase[DBResources.col_IsByDxCode] = employee.EmployeeLevel.ClientProject.IsByDxCode;

            profileBase[DBResources.col_IsL1Auto] = employee.EmployeeLevel.ClientProject.IsL1Auto;
            profileBase[DBResources.col_IsL2Auto] = employee.EmployeeLevel.ClientProject.IsL2Auto;
            profileBase[DBResources.col_IsL3Auto] = employee.EmployeeLevel.ClientProject.IsL3Auto;

            profileBase.Save();

            Response.Redirect("~/Default.aspx");
        }
    }
}