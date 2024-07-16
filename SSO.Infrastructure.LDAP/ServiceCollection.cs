using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;

namespace SSO.Infrastructure.LDAP
{
    public static class ServiceCollection
    {
        public static void ApplyLdapServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(_ => new IdpService(IdentityProvider.LDAP));

            services.AddScoped<AuthenticationService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<GroupRepository>();
            services.AddScoped<GroupUserRepository>();

            services.Configure<LDAPSettings>(configuration.GetSection("LDAPSettings"));

            services.AddScoped<SynchronizeService>();
        }
    }
}
