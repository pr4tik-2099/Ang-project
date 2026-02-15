using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ang_app.DTO
{
    public class UserDetailDTO
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; } 
        public String[]? Roles { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public bool? PhnumberConfirm { get; set; }
        public int AccessFailedCount { get; set; }      
        public string? password { get; set; }  
    }
}