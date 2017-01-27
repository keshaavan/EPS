using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EPICompliance
{
    class ConfigurationHelper
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
    }
}
