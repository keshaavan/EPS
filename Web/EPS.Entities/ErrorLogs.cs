using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class ErrorLogs
    {
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }
        public string FromIP { get; set; }
        public string LogType { get; set; }
    }
}
