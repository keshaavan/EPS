using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class ChartProduction
    {
        public Int64 Id { get; set; }
        public Int64 ChartMoreInfoId { get; set; }
        public int LevelStatusId { get; set; }
        public Nullable<int> LevelStatusCommentId { get; set; }
        public Int64 LevelUpdatedBy { get; set; }
        public int LevelNumber { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int TimeTaken { get; set; }

        public Lookup LevelStatus { get; set; }
        public Comments LevelStatusComment { get; set; }
        public Employee Employee { get; set; }
    }
}
