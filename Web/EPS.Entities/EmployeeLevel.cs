using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class EmployeeLevel
    {
        public Int64 Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int ClientProjectId { get; set; }
        public int LevelNumber { get; set; }
        public Boolean isAdmin { get; set; }
        public Boolean IsReportUser { get; set; }
        public Boolean isActive { get; set; }

        public ClientProject ClientProject { get; set; }
    }
}
