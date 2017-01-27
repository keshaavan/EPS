using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Entities
{
    public class Comments
    {
        public int Id { get; set; }
        public int ClientProjectId { get; set; }
        public string Description { get; set; }
        public int CommentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public Boolean isActive { get; set; }

        public ClientProject ClientProject { get; set; }
        public Lookup CommentCategory { get; set; }
    }
}
