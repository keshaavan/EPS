using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EPS.Entities
{
    public class Queue
    {
        public int ClientProjectId { get; set; }
        public int QueueId { get; set; }
        public string QueueName { get; set; }
        public Boolean isActive { get; set; }
        public string Flag { get; set; }
        public string ClientKey { get; set; }
        public string ProjectKey { get; set; }
    }
}
