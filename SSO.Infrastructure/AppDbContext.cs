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
        public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationAllowedOrigin> ApplicationAllowedOrigins { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ApplicationConfig());
            builder.ApplyConfiguration(new ApplicationPermissionConfig());
            builder.ApplyConfiguration(new ApplicationRoleConfig());
            builder.ApplyConfiguration(new ApplicationAllowedOriginConfig());
            builder.ApplyConfiguration(new ApplicationUserConfig());

            base.OnModelCreating(builder);
        }
    }
}
