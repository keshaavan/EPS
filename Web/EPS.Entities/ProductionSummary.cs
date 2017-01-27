using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class ProductionSummary
    {
        public DateTime ProcessedDate { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public int TotalAvailLogins { get; set; }
        public int LoginsUsed { get; set; }
        public int NoOfLevel1Resources { get; set; }
        public int NoOfLevel2Resources { get; set; }
        public int ChartsReceived { get; set; }
        public int InvalidCharts { get; set; }
        public int ChartsCompletedLevel1 { get; set; }
        public int ChartsCompletedLevel2 { get; set; }
        public int BacklogLevel1Begin { get; set; }
        public int BacklogLevel1End { get; set; }
        public int BacklogLevel2Begin { get; set; }
        public int BacklogLevel2End { get; set; }
        public int Level2Greater2Days { get; set; }
        public Decimal WellmedErrorRate { get; set; }
        public Decimal FirstPassErrorRate { get; set; }
    }
}
