using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Db.MySql
{
    public static class ServiceCollection
    {
        public static void ApplyMySqlServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), serverVersion));

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

#if !DEBUG
using (var scope = services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}
#endif
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

            services.AddHangfire(x =>
                x.UseStorage(
                    new MySqlStorage(
                        configuration.GetConnectionString("DefaultConnection"),
                        new MySqlStorageOptions
                        {
                            // Configuration options for MySQL storage
                            TablesPrefix = "Hangfire" // Optional: prefix for Hangfire tables
                        }
                    )
                )
            );
        }
    }
}
