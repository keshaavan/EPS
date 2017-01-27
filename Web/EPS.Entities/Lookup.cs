using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class Lookup
    {
        public int Id { get; set; }
        public Nullable<int> ClientProjectId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int DisplayOrder { get; set; }
        public Boolean IsActive { get; set; }

        public ClientProject ClientProject { get; set; }
    }
}
