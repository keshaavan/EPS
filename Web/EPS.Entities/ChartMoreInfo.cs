using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class ChartMoreInfo
    {
        public Int64 Id { get; set; }
        public Int64 ChartId { get; set; }
        public int LevelStatusId { get; set; }
        public int LevelStatusCommentId { get; set; }
        public Int64 LevelUpdatedBy { get; set; }
        public int LevelNumber { get; set; }
        public DateTime LevelStartDate { get; set; }
        public Nullable<DateTime> LevelEndDate { get; set; }
        public int NumberOfDxCode { get; set; }
        public Nullable<DateTime> CompletionDate { get; set; }
        public Nullable<DateTime> InvalidDate { get; set; }
        public Nullable<DateTime> HoldDate { get; set; }        
        public Lookup LevelStatus { get; set; }
        public Comments LevelStatusComment { get; set; }
        //public IEnumerable<ChartAudit> ChartAudits { get; set; }
        public Employee Employee { get; set; }
        public Boolean isComments { get; set; }
       // public Int64 ChartMoreInfoId { get; set; }
    }
}
