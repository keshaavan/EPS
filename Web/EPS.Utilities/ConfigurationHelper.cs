using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EPS.Utilities
{
    public class ConfigurationHelper
    {
        public static int PasswordExpiryPeriod
        {
            get
            {
                return -(Convert.ToInt32(ConfigurationManager.AppSettings["passwordExpiryPeriod"]));
            }
        }

        public static int PerviousPasswordCount
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["perviousPasswordCount"]);
            }
        }

        public static string SecurityKey
        {
            get
            {
                return ConfigurationManager.AppSettings["SecurityKey"];
            }
        }

        public static int ConnectionTimeout
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["ConnectionTimeout"]);
            }
        }

        public static string EPSSysUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["EPSSysUserName"];
            }
        }

        public static int BulkUploadForDeallocationLimit
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["BulkUploadForDeallocationLimit"]);
            }
        }
        public static string ClientKey
        {
            get
            {
                return ConfigurationManager.AppSettings["ClientKey"];
            }
        }

        public static string CAPTCHAUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CAPTCHAUrl"];
            }
        }

        public static string CAPTCHAResponse
        {
            get
            {
                return ConfigurationManager.AppSettings["CAPTCHAResponse"];
            }
        }

    }
}
