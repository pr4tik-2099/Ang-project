using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ang_app.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ang_app.Data
{
    public class DbContext : IdentityDbContext<AppUser>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
            
        }
    }
}