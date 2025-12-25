using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ang_app.DTO
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public bool isSuccessfull { get; set; } 
        public string? Message { get; set; } 
    }
}