using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Db.MySql;

namespace SSO.ServiceCollections
{
    public static class MySqlServiceCollections
    {
        public static void ApplyMySqlServiceColletions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
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
                            TablePrefix = "Hangfire" // Optional: prefix for Hangfire tables
                        }
                    )
                )
            );
        }
    }
}
