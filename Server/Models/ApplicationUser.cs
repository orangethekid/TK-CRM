﻿using Microsoft.AspNetCore.Identity;

namespace TakraonlineCRM.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
