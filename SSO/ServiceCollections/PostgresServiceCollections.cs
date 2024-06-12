using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Db.Postgres;

namespace SSO.ServiceCollections
{
    public static class PostgresServiceCollections
    {
        public static void ApplyPostgresServiceCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

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

            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(c =>
                    c.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))));
        }
    }
}
