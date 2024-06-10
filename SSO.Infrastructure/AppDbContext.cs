using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Configs;

namespace SSO.Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<ApplicationUserClaim> UserClaims { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
        public DbSet<ApplicationRoleClaim> RoleClaims { get; set; }
        public DbSet<IdentityUserToken<string>> UserTokens { get; set; }

        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
        public DbSet<ApplicationCallback> ApplicationCallbacks { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ApplicationConfig());
            builder.ApplyConfiguration(new ApplicationPermissionConfig());
            builder.ApplyConfiguration(new ApplicationRoleConfig());
            builder.ApplyConfiguration(new ApplicationUserConfig());
            builder.ApplyConfiguration(new ApplicationRoleClaimConfig());
            builder.ApplyConfiguration(new ApplicationCallbackConfig());
            builder.ApplyConfiguration(new GroupConfig());
            builder.ApplyConfiguration(new GroupUserConfig());
            builder.ApplyConfiguration(new GroupRoleConfig());

            base.OnModelCreating(builder);
        }
    }
}
