using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EPS.Resources;

using System.Web.Profile;
using EPS.BusinessLayer;

public partial class Logout : System.Web.UI.Page
{
    //static ProfileCommon profile = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        using (var employee = new EmployeeInfo())
        {
           // profile = (ProfileCommon)HttpContext.Current.Profile;
           var  profile = (ProfileBase)Session["Profile"];
           employee.Logout(((ProfileCommon)(Profile)).UserName);
        }
        Session.Abandon();
        Session.Clear();
        Response.Redirect("~/Account/Login.aspx", true);
    }
}