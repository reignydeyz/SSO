using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure
{
    public static class ServiceCollection
    {
        public static void ApplySqlServerServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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

            services.AddHangfire(x => x.UseSqlServerStorage((configuration.GetConnectionString("DefaultConnection"))));
        }
    }
}
