using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

using EPS.BusinessLayer;
using EPS.Entities;
using EPS.Utilities;

/// <summary>
/// Summary description for Exception
/// </summary>
public class ExceptionHandler
{
    #region Declarations

    private const string LogInfo = "LogInfo";

    #endregion

    #region Enumeration

    /// <summary>
    /// MessageType Enumeration
    /// </summary>
    public enum MessageType
    {
        Exception,
        InformationCritical,
        Information
    }

    /// <summary>
    /// LogType Enumeration
    /// </summary>
    public enum LogType
    {
        Web,
        WebService
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandler" /> class.
    /// </summary>
    public ExceptionHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #endregion

    #region Methods

    /// <summary>
    /// Common Function to be used in the pages for logging purpose.
    /// </summary>
    /// <param name="objExp">Exception Object.</param>
    /// <param name="enumLogType">Log type as specified in Enum</param>
    public static void Logger(Exception ex, MessageType enumMessageType, LogType enumLogType)
    {
        var profileBase = HttpContext.Current.Profile;

        if (profileBase != null)
        {
            var profile = ((ProfileCommon)profileBase).GetProfile(HttpContext.Current.User.Identity.Name);

            //Create the instance for StackFrame for getting the exception details
            StackFrame stackFrame = new StackFrame(1, true);

            //Create the instance of ErrorLog DAL
            var errorBL = new ErrorLog();
            var errorLog = new EPS.Entities.ErrorLogs()
            {
                UserName = profile.UserName,
                TimeStamp = DateTime.Now,
                MessageType = enumMessageType.ToString(),
                Message = ex.StackTrace,
                Module = string.Format("Page Name :- {0} --- Method Name :- {1} --- Line Number :- {2} --- Error Msg :- {3}", stackFrame.GetFileName(), stackFrame.GetMethod(), stackFrame.GetFileLineNumber().ToString(), ex.Message),
                FromIP = GetIPAddress(),
                LogType = enumLogType.ToString()
            };

            //Add exception details in the database log
            errorBL.InsertErrorLog(errorLog);
        }
    }

    /// <summary>
    /// Common Function to be used in the pages for logging purpose.
    /// </summary>
    /// <param name="sMessage">Message.</param>
    /// <param name="enumLogType">Log type as specified in Enum</param>
    //public static void Logger(string sMessage, MessageType EnumMessageType, LogType EnumLogType)
    //{
    //    //If configuration is set as active or information is critical then it will allow for logging
    //    if (Common.getConfigSetting(LogInfo).Equals("true") || EnumMessageType == MessageType.InformationCritical)
    //    {
    //        //Create the instance for StackFrame for getting the exception details
    //        StackFrame stackFrame = new StackFrame(1, true);

    //      //  Create the instance for CommonDAL
    //        using (CommonDAL commonDAL = new CommonDAL(HttpContext.Current.User.Identity.Name.Trim()))
    //        {
    //            //Add exception details in the database log
    //            commonDAL.AddLog(EnumMessageType.ToString(), sMessage,
    //                                 string.Format("Page Name :- {0} --- Method Name :- {1} --- Line Number :- {2}", stackFrame.GetFileName(), stackFrame.GetMethod(), stackFrame.GetFileLineNumber().ToString()),
    //                                 HttpContext.Current.Request.UserHostAddress, EnumLogType.ToString());
    //        }
    //    }
    //}

    private static string GetIPAddress()
    {
        string retString;
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        string sIPAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(sIPAddress))
        {
            retString = context.Request.ServerVariables["REMOTE_ADDR"];
        }
        else
        {
            string[] ipArray = sIPAddress.Split(new Char[] { ',' });
            retString = ipArray[0];
        }

        return retString.ToLower();
    }
    #endregion
}