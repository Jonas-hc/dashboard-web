using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using dashboard_web.Models;

namespace dashboard_web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<dashboard_web.Models.Credentials> Credentials { get; set; }
        public DbSet<dashboard_web.Models.Dashboard> Dashboard { get; set; }
    }
}
