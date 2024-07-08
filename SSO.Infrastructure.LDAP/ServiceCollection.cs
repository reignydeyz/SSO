using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;

namespace SSO.Infrastructure.LDAP
{
    public static class ServiceCollection
    {
        public static void ApplyLdapServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(_ => new RealmService(Realm.LDAP));

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupUserRepository, GroupUserRepository>();

            services.Configure<LDAPSettings>(configuration.GetSection("LDAPSettings"));

            services.AddScoped<SynchronizeService>();
        }
    }
}
