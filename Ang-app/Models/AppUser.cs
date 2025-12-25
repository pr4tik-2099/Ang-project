using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ang_app.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }
        
    }
}