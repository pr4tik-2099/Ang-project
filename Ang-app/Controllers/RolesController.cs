using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ang_app.DTO;
using Ang_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ang_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole([FromBody] CreateRollDTO createRollDTO)
        {
            if(string.IsNullOrEmpty(createRollDTO.RoleName))
            {
                return BadRequest("Role name required");
            }

            var roleExist = await roleManager.RoleExistsAsync(createRollDTO.RoleName);
            if(roleExist)
            {
                return BadRequest("Role already exists");
            }

            var result = await roleManager.CreateAsync(new IdentityRole(createRollDTO.RoleName));
            if(result.Succeeded)
            {
                return Ok("Role created successfully");
            }
            else
            {
                return BadRequest("Error while creating role");
            }
        }
    
        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> GetRoles()
        {
            var roles = await roleManager.Roles.Select(r => new RoleResponseDTO
            {
                Id = r.Id,
                Name = r.Name,
                TotalUsers = userManager.GetUsersInRoleAsync(r.Name!).Result.Count
            }).ToListAsync();

            return Ok(roles);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignDTO roleAssignDTO)
        {
            var user = await userManager.FindByIdAsync(roleAssignDTO.userId);
            if(user == null)
            {
                return NotFound("User not found");
            }

            var roleExists = await roleManager.FindByIdAsync(roleAssignDTO.RoleId);
            if(roleExists == null)
            {
                return NotFound("Role not found");
            }
            var result = await userManager.AddToRoleAsync(user, roleExists.Name!);
            if(result.Succeeded == true)
            {
                return Ok("Role assigned successfully");
            }
            else
            {
                return BadRequest("Error while assigning role");
            }
            
        }
    }
}