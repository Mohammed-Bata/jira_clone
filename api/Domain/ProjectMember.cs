using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ProjectMember
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public ProjectRole Role { get; set; }

    }

    public enum ProjectRole
    {
        Owner = 1,
        Member = 2,
        Viewer = 3
    }
}
