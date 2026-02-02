using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Invitation
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Email { get; set; }
        public ProjectRole Role { get; set; } = ProjectRole.Member;
        public Guid Token { get; set; }
        public bool IsAccepted { get; set; } = false;
        public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.UtcNow.AddDays(7);

    }
}
