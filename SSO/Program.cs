using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using SSO;
using SSO.Business.Applications;
using SSO.Business.Authentication.Handlers;
using SSO.Business.Groups;
using SSO.Business.Mappings;
using SSO.Business.Users;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure;
using SSO.Infrastructure.Authentication;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Management;
using SSO.Infrastructure.Settings.Constants;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;
using System.Reflection;
using System.Security.Claims;
using VueCliMiddleware;
using LDAP = SSO.Infrastructure.LDAP;

var builder = WebApplication.CreateBuilder(args);

var modelBuilder = new ODataConventionModelBuilder();

modelBuilder.EntityType<ApplicationDto>().HasKey(x => x.ApplicationId);
modelBuilder.EntitySet<ApplicationDto>("Application");

modelBuilder.EntityType<UserDto>().HasKey(x => x.UserId);
modelBuilder.EntitySet<UserDto>("User");

modelBuilder.EntityType<UserDto>().HasKey(x => x.UserId);
modelBuilder.EntitySet<UserDto>("ApplicationUser");

modelBuilder.EntityType<GroupDto>().HasKey(x => x.GroupId);
modelBuilder.EntitySet<GroupDto>("Group");

modelBuilder.EntityType<UserDto>().HasKey(x => x.UserId);
modelBuilder.EntitySet<UserDto>("GroupUser");

modelBuilder.EntityType<GroupDto>().HasKey(x => x.GroupId);
modelBuilder.EntitySet<GroupDto>("ApplicationGroup");

modelBuilder.EnableLowerCamelCase();

builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel()));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

#if !DEBUG
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}
#endif

builder.Services.AddAutoMapper(typeof(ApplicationProfile).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginQueryHandler>());

var rsaPrivateKeyService = new RsaPrivateKeyService();
var pemFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private_key.pem");
var privateKey = rsaPrivateKeyService.CreatePrivateKey(pemFilePath);

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
        IssuerSigningKey = new RsaSecurityKey(privateKey)
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

builder.Services.AddSingleton(_ => new JwtSecretService(privateKey));
builder.Services.AddSingleton(_ => new RealmService(Realm.Default));

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IApplicationCallbackRepository, ApplicationCallbackRepository>();
builder.Services.AddScoped<IApplicationPermissionRepository, ApplicationPermissionRepository>();
builder.Services.AddScoped<IApplicationRoleClaimRepository, ApplicationRoleClaimRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupUserRepository, GroupUserRepository>();
builder.Services.AddScoped<IGroupRoleRepository, GroupRoleRepository>();

var ldapSettings = builder.Configuration.GetSection("LDAPSettings").Get<LDAPSettings>();

if (ldapSettings != null)
{
    builder.Services.AddSingleton(_ => new RealmService(Realm.LDAP));

    builder.Services.AddScoped<IAuthenticationService, LDAP.AuthenticationService>();
    builder.Services.AddScoped<IUserRepository, LDAP.UserRepository>();

    builder.Services.Configure<LDAPSettings>(builder.Configuration.GetSection("LDAPSettings"));

    builder.Services.AddScoped<LDAP.SynchronizeUsersService>();
}

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

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

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAnyOrigin", policy => {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddHangfire(x => x.UseSqlServerStorage((builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddHealthChecks().AddCheck<HealthCheckHandler>(nameof(HealthCheckHandler));

var app = builder.Build();

// Obtain an instance of IWebHostEnvironment through dependency injection
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

// Configure the HTTP request pipeline.

#if DEBUG
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
#else
app.UseCors();
#endif

app.UseSpaStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

string[] prefixes = { "/swagger", "/api", "/odata", "/hangfire" };

app.MapWhen(r => !prefixes.Any(p => r.Request.Path.Value.StartsWith(p)), builder =>
{
    builder.UseSpa(spa =>
    {
        if (env.IsDevelopment())
        {
            spa.Options.SourcePath = "ClientApp/";
            spa.UseVueCli(npmScript: "serve");
        }
    });
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/Client/swagger.json", "Client");
    options.SwaggerEndpoint("/swagger/System/swagger.json", "System");
});

#pragma warning disable CS0618 // Type or member is obsolete
app.UseHangfireServer();
#pragma warning restore CS0618 // Type or member is obsolete
app.UseHangfireDashboard();

app.Run();
