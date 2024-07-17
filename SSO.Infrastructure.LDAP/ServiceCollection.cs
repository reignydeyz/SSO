using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Infrastructure.LDAP.Models;

namespace SSO.Infrastructure.LDAP
{
    public static class ServiceCollection
    {
        public static void ApplyLdapServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuthenticationService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<GroupRepository>();
            services.AddScoped<GroupUserRepository>();

            services.Configure<LDAPSettings>(configuration.GetSection("LDAPSettings"));

            services.AddScoped<SynchronizeService>();
        }
    }
}
