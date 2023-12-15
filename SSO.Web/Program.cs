using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SSO.Business.Authentication.Handlers;
using SSO.Business.Mappings;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure;
using SSO.Infrastructure.Authentication;
using SSO.Infrastructure.Management;
using SSO.Infrastructure.Settings.Constants;
using SSO.Infrastructure.Settings.Services;
using SSO.Web.AuthenticationHandlers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using VueCliMiddleware;
using AuthenticationService = SSO.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = SSO.Domain.Authentication.Interfaces.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Apply Migrations
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

builder.Services.AddAutoMapper(typeof(ApplicationProfile).Assembly);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginQueryHandler>());

builder.Services.AddControllers();

builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

builder.Services.AddMemoryCache();

var jwtSecret = Guid.NewGuid().ToString();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = TokenValidationParamConstants.Audience,
        ValidIssuer = TokenValidationParamConstants.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("RootPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.System);
    });
});

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

// Register the JwtSecret as a singleton service
builder.Services.AddSingleton(_ => new JwtSecretService(jwtSecret));

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserClaimRepository, UserClaimRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("Client", new OpenApiInfo { Title = "Client", Version = $"v{typeof(Program).Assembly.GetName().Version}" });
    x.SwaggerDoc("System", new OpenApiInfo { Title = "System", Version = $"v{typeof(Program).Assembly.GetName().Version}" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

#if DEBUG
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
#endif

app.UseRouting();
app.UseSpaStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapWhen(r => !(r.Request.Path.Value.StartsWith("/swagger")
        || r.Request.Path.Value.StartsWith("/api")), builder =>
{
    builder.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp/";
        spa.UseVueCli(npmScript: "serve");
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/Client/swagger.json", "Client");
    options.SwaggerEndpoint("/swagger/System/swagger.json", "System");
});

app.Run();