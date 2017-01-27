using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class ChartAudit
    {
        public Int64 Id { get; set; }
        public Int64 ChartMoreInfoId { get; set; }
        public string PageNumbers { get; set; }
        public string CorrectedValue { get; set; }
        public string Comments { get; set; }
        public int ErrorCategoryId { get; set; }
        public int ErrorSubCategoryId { get; set; }
        public string AdditionalComments { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Nullable<int> L3LevelDisagreeId { get; set; }

        public Lookup ErrorCategory { get; set; }
        public Lookup ErrorSubCategory { get; set; }

        public ChartMoreInfo ChartMoreInfo { get; set; }
        public Lookup L3LevelDisagree { get; set; }
        public int Flag { get; set; }
    }

    public class ChartAuditComments
    {
        public Int64 Id { get; set; }
        public Int64 ChartMoreInfoId { get; set; }
        public string PageNumbers { get; set; }
        public string CorrectedValue { get; set; }
        public string Comments { get; set; }
        public string AdditionalComments { get; set; }
        public int ErrorCategoryId { get; set; }
        public int ErrorSubCategoryId { get; set; }
        
    }
    
   
}
