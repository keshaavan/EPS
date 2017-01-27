using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LocationId { get; set; }
        public Boolean isActive { get; set; }

        public EmployeeLevel EmployeeLevel { get; set; }
        public IEnumerable<EmployeeLevel> AdminEmployeeLevels { get; set; }
        public Lookup Location { get; set; }
    }
}
