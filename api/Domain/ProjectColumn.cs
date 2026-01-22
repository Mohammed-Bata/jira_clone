using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ProjectColumn
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }  
        public Project Project { get; set; }
    }
}
