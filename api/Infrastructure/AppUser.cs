using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string? AvatarUrl { get; set; }
        public string? AvatarLocalPath { get; set; }
    }
}
