using System;
using System.Web;
using System.Text;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;
using System.IO;
using System.Web.UI;
using System.Web.Script.Serialization;

/// <summary>
/// Generic Class file, with common functionalty to be used across web application (e.g Logging, caching etc).
/// </summary>
public class Common : System.Web.UI.Page
{
    #region Constants
    public static string g_sNotAvailable = "N/A";
    public static string g_sError = "Error";
    public static string g_sGenericMessage = "An unrecoverable error has occurred";
    public static string g_sDB_NULL_DATE = "1900-01-01 00:00:00.000";
    #endregion

    #region Enum

    /// <summary>
    /// Enumeration for Config values in App settings.
    /// </summary>
    public enum enuConfigKey
    {
        /// <summary>
        /// Config Key to enable/disable information log
        /// </summary>
        LogInfo,
        /// <summary>
        /// Tableau scripts files
        /// </summary>
        TableauServerScripts,
        /// <summary>
        /// Tableau host url to communicate for access.
        /// <summary>
        TableauServerHostUrl,
        /// <summary>
        /// Tableau trusted url to generate authentic ticket.
        /// </summary>
        TableauServerTrustedUrl,
        /// <summary>
        /// Tableau access username
        /// </summary>
        TableauServerUserName,
        /// <summary>
        /// Tableau target site to access particular site alone
        /// </summary>
        TableauServerTargetSite,
        /// <summary>
        /// Tableau Site Root to locate the dashboard available path
        /// </summary>
        TableauServerSiteRoot,
        /// <summary>
        /// Is QRAPS based dashboard or Direct
        /// </summary>
        IsQRAPSBase,
        /// <summary>
        /// Default how much recent records need to be display in chatters functionality
        /// </summary>
        ChattersRecordToDisplay
    }


    public enum enumQueryString
    {
        /// <summary>
        /// Used in Password Reset page to get Query String mguid.
        /// </summary>
        userid,
        /// <summary>
        /// Used in Redir.aspx page to pass client ID.
        /// </summary>
        cid,
        /// <summary>
        /// Used in Redir.aspx page to pass client name.
        /// </summary>
        cname,
        /// <summary>
        /// GUID of Navigation
        /// </summary>
        mguid
    }

    public enum enumConfigKey
    {
        /// <summary>
        /// Config Key to enable/disable information log
        /// </summary>
        LogInfo,
        /// <summary>
        /// Tableau Server URL
        /// </summary>
        TableauHostUrl,
        /// <summary>
        /// Tableau Ticket url
        /// </summary>
        TableauTrustedUrl,
        /// <summary>
        /// Tableau Login Username
        /// </summary>
        TableauUserName,
        /// <summary>
        /// Mail Username
        /// </summary>
        SmtpUser,
        /// <summary>
        /// Mail Password
        /// </summary>
        SmtpPwd,
        /// <summary>
        /// Support Email ID
        /// </summary>
        SupportMail,
        /// <summary>
        /// MailID from which mail is sent.
        /// </summary>
        MailFrom,
        /// <summary>
        /// Google CAPTCHA varification URL
        /// </summary>
        CAPTCHAUrl,
        /// <summary>
        /// Google CAPTCHA Response
        /// </summary>
        CAPTCHAResponse,
        /// <summary>
        /// Total Records Count In RDE
        /// </summary>
        TotalRecordsCount,
        /// <summary>
        /// File Preview Type in RDE & OptumRDE
        /// </summary>
        FilePreviewType,
        /// <summary>
        /// Summary File Type RDE
        /// </summary>
        FileSummaryType,
        /// <summary>
        /// Summary File Type EmailHost_India
        /// </summary>
        EmailHost_India,
        /// <summary>
        /// Summary File Type EmailSSL_India
        /// </summary>
        EmailSSL_India,
        /// <summary>
        /// Summary File Type EmailPort_India
        /// </summary>
        EmailPort_India,
        /// <summary>
        /// Summary File Type EmailUserName_India
        /// </summary>
        EmailUserName_India,
        /// <summary>
        /// Summary File Type EmailPassword_India
        /// </summary>
        EmailPassword_India,
        /// <summary>
        /// Summary File Type EmailFrom_India
        /// </summary>
        EmailFrom_India,
        /// <summary>
        /// Summary File Type EmailFromDisplayName_India
        /// </summary>
        EmailFromDisplayName_India,
        /// <summary>
        /// Summary File Type EmailHost
        /// </summary>
        EmailHost,
        /// <summary>
        /// Summary File Type EmailSSL
        /// </summary>
        EmailSSL,
        /// <summary>
        /// Summary File Type EmailPort
        /// </summary>
        EmailPort,
        /// <summary>
        /// Summary File Type EmailUserName
        /// </summary>
        EmailUserName,
        /// <summary>
        /// Summary File Type EmailPassword
        /// </summary>
        EmailPassword,
        /// <summary>
        /// Summary File Type EmailFrom
        /// </summary>
        EmailFrom,
        /// <summary>
        /// Summary File Type EmailFromDisplayName
        /// </summary>
        EmailFromDisplayName
        
    }

   

    /// <summary>
    /// Enumeration for Date formatting.
    /// </summary>
    public enum enumDateFormatType
    {
        /// <summary>
        /// MMM. dd, yyyy (e.g May. 17, 2012)
        /// </summary>
        MMM_dd_yyyy,
        /// <summary>
        /// HH:mm tt (e.g 10:30 AM)
        /// </summary>
        HH_mm_tt,
        /// <summary>
        /// MM-dd-yyyy (e.g 12-17-2012)
        /// </summary>
        M_d_yy,
        /// <summary>
        /// MM/dd/yyyy (e.g 12/17/2012)
        /// </summary>
        MM_dd_yyyy,
        /// <summary>
        /// MM/dd/yyyy (e.g 12/17/12)
        /// </summary>
        MM_dd_yy
    }

    #endregion

    #region Common Utility Properties

    /// <summary>
    /// Gets the get assembly build time.
    /// </summary>
    /// <value>
    /// The get assembly build time.
    /// </value>
    public static string getAssemblyBuildTime
    {
        get
        {
            string sAsblyTime = System.IO.File.GetCreationTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            sAsblyTime = sAsblyTime.Replace(" ", "").Replace("/", "").Replace(":", "");
            return sAsblyTime;
        }
    }

    public static string GetConfigSetting(string key)
    {
        if (ConfigurationManager.AppSettings[key] != null)
            return ConfigurationManager.AppSettings[key].Trim();
        else
            return string.Empty;
    }

    #endregion

    #region Common Utility Functions

    /// <summary>
    /// Gets the config setting.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static string getConfigSetting(string key)
    {
        if (ConfigurationManager.AppSettings[key] != null)
            return ConfigurationManager.AppSettings[key].Trim();
        else
            return string.Empty;
    }

    public static string httpPostRequest(string sURL, Dictionary<string, string> dictParams)
    {

        string postData = string.Empty;

        foreach (string key in dictParams.Keys)
            postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(dictParams[key]) + "&";

        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(sURL);
        myHttpWebRequest.Method = "POST";

        byte[] data = Encoding.ASCII.GetBytes(postData);

        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        myHttpWebRequest.ContentLength = data.Length;

        Stream requestStream = myHttpWebRequest.GetRequestStream();
        requestStream.Write(data, 0, data.Length);
        requestStream.Close();

        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

        Stream responseStream = myHttpWebResponse.GetResponseStream();

        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

        string pageContent = myStreamReader.ReadToEnd();

        myStreamReader.Close();
        responseStream.Close();

        myHttpWebResponse.Close();

        return pageContent;
    }

    public static string getTableauTicket(object Page)
    {

        Dictionary<string, string> paramsList = new Dictionary<string, string>();
        paramsList.Clear();
        paramsList.Add("username", getConfigSetting("TableauServerUserName"));
        paramsList.Add("client_ip", ((Page)Page).Request.ServerVariables["REMOTE_ADDR"]);
        paramsList.Add("target_site", getConfigSetting("TableauServerTargetSite"));

        return httpPostRequest(getConfigSetting("TableauServerTrustedUrl"), paramsList);
    }

    public static string getName(string Dashboard)
    {
        string Name = string.Empty;

        switch (Dashboard)
        {
            case "Attendance":
                break;
            case "OptumDailyReport":
                break;
        }

        return Name;
    }

    public static string formatToDate(object oValue, enumDateFormatType objDateFormat = enumDateFormatType.MMM_dd_yyyy)
    {
        if (checkNull(oValue).Equals(g_sNotAvailable) || oValue.Equals(g_sDB_NULL_DATE))
            return g_sNotAvailable;

        switch (objDateFormat)
        {
            case enumDateFormatType.MMM_dd_yyyy:
                return string.Format("{0:MMM dd, yyyy}", oValue);

            case enumDateFormatType.HH_mm_tt:
                return string.Format("{0:HH:mm tt}", oValue);

            case enumDateFormatType.M_d_yy:
                return string.Format("{0:M-d-yy}", oValue);

            case enumDateFormatType.MM_dd_yyyy:
                return string.Format("{0:MM/dd/yyyy}", oValue);

            case enumDateFormatType.MM_dd_yy:
                return ((DateTime)oValue).ToString("MM/dd/yy");

            default:
                return string.Format("{0:MMM. dd, yyyy}", oValue);
        }
    }

    public static object checkNull(object oObject)
    {
        if (oObject.Equals(null) || oObject.Equals(DBNull.Value) || oObject.ToString().Equals(string.Empty) || oObject.ToString().Equals(g_sNotAvailable))
        {
            return g_sNotAvailable;
        }

        return oObject;
    }

    #endregion
}