using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Net;

/// <summary>
/// Summary description for Mailer
/// </summary>
public class Mailer
{
    private enum enuMailType
    {
        RegistrationMail,
        ResetPasswordMail
    }

    #region "SendMail"

    public static void sendMail(string toMailAddress, string sSubject, StringBuilder sbText)
    {
        try
        {
            SmtpClient SmtpServer = new SmtpClient();
            MailMessage mail = new MailMessage();
            string smtpUser = "", smtpPassword = "";
            smtpUser = Common.GetConfigSetting(Common.enumConfigKey.SmtpUser.ToString());
            smtpPassword = Common.GetConfigSetting(Common.enumConfigKey.SmtpPwd.ToString());

            SmtpServer.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
            SmtpServer.Port = 587;
            SmtpServer.Host = "smtp.gmail.com";

            mail = new MailMessage();
            mail.From = new MailAddress(Common.GetConfigSetting(Common.enumConfigKey.MailFrom.ToString()));
            mail.To.Add(toMailAddress);

            mail.Subject = sSubject;
            mail.IsBodyHtml = true;
            SmtpServer.EnableSsl = true;
            mail.Body = sbText.ToString();
            SmtpServer.Send(mail);
        }
        catch (Exception)
        {
            throw;
            // Logger.logExceptionToDB(objExp);
        }
    }




    private static void sendMailEpiIndia(string sToEmail, string sSubject, StringBuilder sbHTMLData)
    {
        SmtpClient objMailer = new SmtpClient();
        MailMessage objMessage = new MailMessage();
        NetworkCredential objNetwrk = new NetworkCredential();

        try
        {
            objNetwrk.UserName = Common.getConfigSetting(Common.enumConfigKey.EmailUserName_India.ToString());
            objNetwrk.Password = Common.getConfigSetting(Common.enumConfigKey.EmailPassword_India.ToString());

            objMailer.DeliveryMethod = SmtpDeliveryMethod.Network;
            objMailer.EnableSsl = bool.Parse(Common.getConfigSetting(Common.enumConfigKey.EmailSSL_India.ToString()));
            objMailer.Host = Common.getConfigSetting(Common.enumConfigKey.EmailHost_India.ToString());
            objMailer.Port = int.Parse(Common.getConfigSetting(Common.enumConfigKey.EmailPort_India.ToString()));
            objMailer.Credentials = objNetwrk;

            objMessage.From = new MailAddress(Common.getConfigSetting(Common.enumConfigKey.EmailFrom_India.ToString()), Common.getConfigSetting(Common.enumConfigKey.EmailFromDisplayName_India.ToString()));
            objMessage.To.Add(sToEmail);

            //if (ccEmailIds != null && ccEmailIds != string.Empty)
            //    objMessage.CC.Add(ccEmailIds);

            objMessage.Subject = sSubject;
            objMessage.IsBodyHtml = true;

            objMessage.Body = sbHTMLData.ToString();

            objMailer.Send(objMessage);

        }
        catch (Exception objExp)
        {
            //Common.logToDB(objExp, Common.logType.Exception);
            throw new Exception("Error While Send Email", objExp);
        }
        finally
        {
            objMailer.Dispose();
            objMessage.Dispose();
            objNetwrk = null;
        }
    }

    public static void sendRegistrationMail(string sToEmail, string sURL, string sUserName, string sDisplayName)
    {
        try
        {
            StringBuilder sbText = new StringBuilder();
            string sSubject = string.Format("Welcome to EPS - {0}", sDisplayName);
            sbText.AppendFormat(readStaticFile("RegistrationMail"), sURL, string.Format("{0}{1}", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpRuntime.AppDomainAppVirtualPath), sDisplayName, sUserName);
            if (sToEmail.Substring((sToEmail.Length - 18)).ToLower() == "episourceindia.com")
            {
                sendMailEpiIndia(sToEmail, sSubject, sbText);
            }
            else
            {
                sendMail(sToEmail, sSubject, sbText);
            }
            //sendMailEpiIndia(sToEmail, sSubject, sbText);
        }
        catch (Exception)
        {
            throw;
            //Logger.logExceptionToDB(objExp);
        }
    }
     
    public static void sendForgotPasswordMail(string sToEmail, string sURL, string sUserName)
    {
        try
        {
            StringBuilder sbText = new StringBuilder();
            string sSubject = string.Format("EPS Forgot Password - {0}", sUserName);
            sbText.AppendFormat(readStaticFile("ForgotPwdMail"), sURL, string.Format("{0}{1}", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpRuntime.AppDomainAppVirtualPath), sUserName);

            if (sToEmail.Substring((sToEmail.Length - 18)).ToLower() == "episourceindia.com")
            {
                sendMailEpiIndia(sToEmail, sSubject, sbText);
            }
            else
            {
                sendMail(sToEmail, sSubject, sbText);
            }
           
        }
        catch (Exception)
        {
            throw;
            //Logger.logExceptionToDB(objExp);
        }
    }
    
    public static string readStaticFile(string enuMailType)
    {
        try
        {
            string sFileName = "";

            switch (enuMailType)
            {
                case "RegistrationMail":
                    sFileName = @"../Static/RegistrationMail.html";
                    break;
                case "ResetPasswordMail":
                    sFileName = @"../Static/ResetPasswordEmail.html";
                    break;
                case "ForgotPwdMail":
                    sFileName = @"../Static/FPwdMail.html";
                    break;
            }

            return System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(sFileName));
        }
        catch (Exception)
        {
           // Logger.logExceptionToDB(objExp);
            throw;
            //return null;
        }
    }
   
    #endregion
}