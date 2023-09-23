using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Authentication.Handlers;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure;
using SSO.Infrastructure.Management;
using SSO.Infrastructure.UserManagement;
using AuthenticationService = SSO.Infrastructure.Authentication.AuthenticationService;
using IAuthService = SSO.Domain.Interfaces.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginQueryHandler>());

builder.Services.AddControllers();

builder.Services.AddScoped<IAuthService, AuthenticationService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IUserRoleManagementService, UserRoleManagementService>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();