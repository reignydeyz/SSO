using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Authentication.Handlers;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Domain.UserManegement.Interfaces;
using SSO.Infrastructure;
using SSO.Infrastructure.Management;
using SSO.Infrastructure.UserManagement;
using SSO.Web.AuthenticationHandlers;
using VueCliMiddleware;
using AuthenticationService = SSO.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = SSO.Domain.Authentication.Interfaces.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginQueryHandler>());

builder.Services.AddControllers();

builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseRouting();
app.UseSpaStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapWhen(r => !(r.Request.Path.Value.StartsWith("/swagger") || r.Request.Path.Value.StartsWith("/api")), builder => {
    builder.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp/";
        spa.UseVueCli(npmScript: "serve");
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();