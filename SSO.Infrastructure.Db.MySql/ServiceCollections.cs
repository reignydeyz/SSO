using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Db.MySql
{
    public static class ServiceCollections
    {
        public static void ApplyMySqlServiceCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>();

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
                            TablePrefix = "Hangfire" // Optional: prefix for Hangfire tables
                        }
                    )
                )
            );
        }
    }
}
