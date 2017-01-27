using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class ChartInfo
    {
        public Int64 Id { get; set; }
        public int ClientProjectId { get; set; }
        public string ClientMarket { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ClientReference { get; set; }
        public string FileName { get; set; }
        public Nullable<int> NoOfPages { get; set; }
        public string OverallStatus { get; set; }
        public DateTime UploadedDate { get; set; }
        public Int64 UploadedBy { get; set; }
        public Int64 ChartMoreInfoId { get; set; }

        public ClientProject ClientProject { get; set; }
        public ChartMoreInfo ChartMoreInfo { get; set; }
        public ChartMoreInfo PreviousChartMoreInfo { get; set; }
    }
}
