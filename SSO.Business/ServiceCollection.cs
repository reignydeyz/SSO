using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Authentication.Interfaces;
using SSO.Infrastructure.Authentication;
using SSO.Infrastructure.Management;

namespace SSO.Business
{
    public static class ServiceCollection
    {
        public static void ApplyBusinessServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuthenticationService>();

            services.AddScoped<UserRepository>();
            services.AddScoped<GroupRepository>();
            services.AddScoped<GroupUserRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOtpService, OtpService>();

            services.AddScoped<Authentication.ServiceFactory>();

            services.AddScoped<Users.RepositoryFactory>();
            services.AddScoped<Groups.RepositoryFactory>();
            services.AddScoped<GroupUsers.RepositoryFactory>();
        }
    }
}
