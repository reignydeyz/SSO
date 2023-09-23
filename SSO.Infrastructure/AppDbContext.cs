using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Models;
using SSO.Infrastructure.Configs;

namespace SSO.Infrastructure
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
