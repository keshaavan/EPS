using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class ClientProject
    {
        public int Id { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string ClientReferenceLabel { get; set; }

        public string ClientKey { get; set; }
        public string ProjectKey { get; set; }
        public string Queue { get; set; }
        public string QueueKey { get; set; }
        public Boolean IsByDxCode { get; set; }
        public Boolean IsL1Auto { get; set; }
        public Boolean IsL2Auto { get; set; }
        public Boolean IsL3Auto { get; set; }
        public Boolean isActive { get; set; }
        public string HomepageText { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Decimal Factorialization { get; set; }
    }

    public class GetProjects
    {
        public string Project { get; set; }
        public string ProjectKey { get; set; }
    }

    public class GetALLQueues
    {
        public int Id { get; set; }
        public string Queue { get; set; }
    }
   
}
