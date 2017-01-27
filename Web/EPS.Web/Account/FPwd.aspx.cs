

#region "Header Files
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Security;
using System.Net.Mail;
using System.Data;
using EPICompliance;
using EPS.DataLayer;


#endregion

public partial class Account_FPwd : System.Web.UI.Page
{
    string sUser = string.Empty;
    string sUserId = string.Empty;
    string sUserDispName = string.Empty;

    #region Members
    private string c_sConStr;
    private string c_sEPICConStr = "EPSConnectionString";

    protected string sClient = string.Empty;
    protected string sUserName = string.Empty;
    #endregion


    #region "PageLoad"
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        { }
        lblError.Text = "";
    }
    #endregion

    #region "Send Password"
    protected void SendPwdButton_Click(object sender, EventArgs e)
    {


        try
        {

            if (!recaptcha.IsValid && recaptcha.Enabled)
            {
                lblError.Text = "Error: Your captcha words did not match.\n\n";
                return;
            }

            sUser = Membership.GetUserNameByEmail(EmailAddress.Text.Trim());

            if (!string.IsNullOrEmpty(sUser))
            {
                string sAuthKey = Guid.NewGuid().ToString();
                DataSet dsUserList = new DataSet();
                Userpipeline loginDAL = new Userpipeline();
                dsUserList = loginDAL.getUser(sUser);
                if (dsUserList != null && dsUserList.Tables.Count > 0 && dsUserList.Tables[0].Rows.Count > 0)
                {
                    sUserId = dsUserList.Tables[0].Rows[0]["UserName"].ToString();                    
                    UserManagement usermgt = new UserManagement();
                    usermgt.insertUserPipeline(string.Empty, sUser, EmailAddress.Text.Trim(), string.Empty, sAuthKey, string.Empty, string.Empty, string.Empty);
                }
                StringBuilder sbText = new StringBuilder();
                string url = string.Format("{0}/Account/ReSetPwd.aspx?userid={1}", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath, sAuthKey);                
                sbText.AppendFormat(Mailer.readStaticFile("ForgotPwdMail"), url, HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath, sUser);
                Mailer.sendForgotPasswordMail(EmailAddress.Text.Trim(), url, sUser);
                Response.Redirect("~/Account/ResendPwd.aspx", false);
            }
            else
            {
                lblError.Text = " Invalid registered email address.";
            }

        }
        catch (Exception)
        {
            throw;            
        }



    }
    #endregion
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Account/Login.aspx", false);
    }
}