using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for ReCAPTCHAhelper
/// </summary>
public class ReCAPTCHAHelper
{
    //As we have - in this keyword not able to use them in Common Enum.
    private const string C_CAPTCHAResponse = "g-recaptcha-response";
    public string CAPTCHAUrl = EPS.Utilities.ConfigurationHelper.CAPTCHAUrl;
    public string CAPTCHAResponse = EPS.Utilities.ConfigurationHelper.CAPTCHAResponse;


    public static bool validate()
    {
        string sRes = HttpContext.Current.Request[C_CAPTCHAResponse];

        bool Valid = false;

        HttpWebRequest objReq = (HttpWebRequest)WebRequest.Create(string.Format("{0}&{1}={2}", "CAPTCHAUrl", "CAPTCHAResponse", sRes));

        try
        {
            using (WebResponse wResponse = objReq.GetResponse())
            {

                using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                {
                    string jsonResponse = readStream.ReadToEnd();

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    CAPTCHAValid data = js.Deserialize<CAPTCHAValid>(jsonResponse);

                    Valid = Convert.ToBoolean(data.success);
                }
            }



        }
        catch (WebException objExp)
        {
            //  Logger.logExceptionToDB(objExp);
        }

        return Valid;
    }
}

class CAPTCHAValid
{
    public string success { get; set; }
}