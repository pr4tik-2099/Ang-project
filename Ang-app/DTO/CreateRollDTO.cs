using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Tasks;

namespace Ang_app.DTO
{
    public class CreateRollDTO
    {
        [Required(ErrorMessage ="Role Name is required")]
        public string RoleName { get; set; } = null!;
    }
}