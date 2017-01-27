using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Profile;

using EPS.DataLayer;


using EPICompliance;

public partial class Account_ReSetPwd : System.Web.UI.Page
{
    private enum enumType
    {
        Register,
        ResetPWD,
        ForgotPWD
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Check QS is empty redirect.
            if (Request.QueryString[Common.enumQueryString.userid.ToString()] == null || Request.QueryString[Common.enumQueryString.userid.ToString()] == string.Empty)
            {
                Response.Redirect("Login.aspx");
            }

            //Check if User available in User Pipeline
            string sMGUID = Request.QueryString[Common.enumQueryString.userid.ToString()];
            DataSet objDS;
            UserManagement objDAL = new UserManagement();
            objDS = objDAL.getUserInPipeline(sMGUID);

            if (objDS == null || objDS.Tables.Count == 0 || objDS.Tables[0].Rows.Count == 0)
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
    protected void chgPwd_Click(object sender, EventArgs e)
    {
        string sMGUID;
        DataSet objDS;
        FailureText.Text = string.Empty;
        try
        {
            if (!recaptcha.IsValid && recaptcha.Enabled)
            {
                FailureText.Text = "Error: Your captcha words did not match.\n\n";
                return;
            }

            Compliance.enumPasswordValidation enu = Compliance.validatePassword(Password.Text.Trim());

            if (enu != Compliance.enumPasswordValidation.OK)
            {
                FailureText.Text = "Password must be 8 characters long, contain at least one number, one special character, and one Upper case letter";
                return;
            }

            ////Check if User available in User Pipeline
            sMGUID = Request.QueryString[Common.enumQueryString.userid.ToString()];

            UserManagement objDAL = new UserManagement();
            
                objDS = objDAL.getUserInPipeline(sMGUID);
            

            if (objDS == null || objDS.Tables.Count == 0 || objDS.Tables[0].Rows.Count == 0)
            {
                Response.Redirect("Login.aspx");
            }

            //if (objDS.Tables[0].Rows[0]["Type"].ToString() == enumType.Register.ToString())
            //{
               // MembershipUser objNewUsrs = Membership.CreateUser(objDS.Tables[0].Rows[0]["UserName"].ToString(), Password.Text.Trim(), objDS.Tables[0].Rows[0]["Email"].ToString());
            //    //Roles.AddUserToRole(objDS.Tables[0].Rows[0]["UserName"].ToString(), objDS.Tables[0].Rows[0]["UserRole"].ToString());
            //}
            //else if ((objDS.Tables[0].Rows[0]["Type"].ToString() == enumType.ResetPWD.ToString()) || (objDS.Tables[0].Rows[0]["Type"].ToString() == enumType.ForgotPWD.ToString()))
            //{
                MembershipUser objNewUsr = Membership.GetUser(objDS.Tables[0].Rows[0]["UserName"].ToString());
                string sRestPwd = objNewUsr.ResetPassword();
                objNewUsr.ChangePassword(sRestPwd, Password.Text);
           // }

                UserManagement objUMDAL = new UserManagement();
           
                objUMDAL.updateEPIUserStatus(sMGUID, objDS.Tables[0].Rows[0]["UserName"].ToString());


                Response.Redirect("Login.aspx?msg=Password Successfully Changed");
        }
        catch (Exception)
        {
            //throw;
            // Logger.logExceptionToDB(objExp);
             FailureText.Text = "Could not update your password.";
        }
    }
}