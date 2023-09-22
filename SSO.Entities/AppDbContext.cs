﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSO.Entities.Configs;

namespace SSO.Entities
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ApplicationConfig());
            builder.ApplyConfiguration(new ApplicationRoleConfig());

            base.OnModelCreating(builder);
        }
    }
}
