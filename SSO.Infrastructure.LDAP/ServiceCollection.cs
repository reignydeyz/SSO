using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<SynchronizeService>();
        }
    }
}
