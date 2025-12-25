using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ang_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ang_app.DTO;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Ang_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(RegisterDTO register)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var user = new AppUser
            {
                Email = register.Email,
                FullName = register.FullName,
                UserName = register.Email
            };

            var result= await userManager.CreateAsync(user, register.password);
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);

            }
            if(register.Roles is null)
            {
                await userManager.AddToRoleAsync(user,"User");

            }else
            {
                foreach(var role in register.Roles)
                {
                   await userManager.AddToRoleAsync(user, role);
                }
            }

            return Ok(new AuthResponseDTO
            {
                isSuccessfull = true,
                Message = "Account Created",

            });

            
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO Login)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(Login.Email);
            if(user is null)
            {
                return Unauthorized(new AuthResponseDTO
                {
                    isSuccessfull = false,
                    Message = "Invalid User"
                });

            }

            var result = await userManager.CheckPasswordAsync(user, Login.Password);
            if(!result)
            {
                return Unauthorized(new AuthResponseDTO
                {
                    isSuccessfull = false,
                    Message = "Invalid Password"
                });
            }
            var token = GenerateJWTToken(user);
            return Ok(new AuthResponseDTO
            {
                isSuccessfull = true,
                Message = "Login Successfull",
                Token = token
            });
        }    

        private string GenerateJWTToken(AppUser user)
        {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("JWTSettings").GetSection("Key").Value!);
            var roles = userManager.GetRolesAsync(user).Result;
            List<Claim> claim =
            [
                new (JwtRegisteredClaimNames.Email, user.Email??""),
                new (JwtRegisteredClaimNames.Name, user.FullName??""),
                new (JwtRegisteredClaimNames.NameId, user.Id ??""),
                new (JwtRegisteredClaimNames.Aud, configuration.GetSection("JWTSettings").GetSection("Audience").Value!),
                new (JwtRegisteredClaimNames.Iss, configuration.GetSection("JWTSettings").GetSection("Issuer").Value!) 
            ];

            foreach(var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )

            };
            var token = JwtTokenHandler.CreateToken(tokenDescripter);

            return JwtTokenHandler.WriteToken(token);

        }

        [Authorize]
        [HttpGet("Details")]
        public async Task<ActionResult<UserDetailDTO>> GetUserDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId!);

            if(user is null)
            {
                return NotFound(new AuthResponseDTO
                {
                    isSuccessfull = false,
                    Message = "User Not Found"
                });
            }

            return Ok(new UserDetailDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = [..await userManager.GetRolesAsync(user)],
                PhnumberConfirm = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                AccessFailedCount = user.AccessFailedCount
            });
        } 

        [HttpGet("AllUsers")]
        public async Task<ActionResult<IEnumerable<UserDetailDTO>>> GetAllUser()
        {
            var users = await userManager.Users.Select(u => new UserDetailDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Roles = userManager.GetRolesAsync(u).Result.ToArray()
            }).ToListAsync();

            return Ok(users);
        }
    }



}