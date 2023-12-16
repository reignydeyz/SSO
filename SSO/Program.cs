using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using SSO.Business.Applications;
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
using System.Reflection;
using System.Security.Claims;
using System.Text;
using VueCliMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAutoMapper(typeof(ApplicationProfile).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginQueryHandler>());

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

builder.Services.AddControllers()
    .AddOData(options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents("odata", GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

string[] prefixes = { "/swagger", "/api", "/odata" };

app.MapWhen(r => !prefixes.Any(p => r.Request.Path.Value.StartsWith(p)), builder =>
{
    builder.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp/";
        spa.UseVueCli(npmScript: "serve");
    });
});

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/Client/swagger.json", "Client");
    options.SwaggerEndpoint("/swagger/System/swagger.json", "System");
});

app.Run();

IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();

    builder.EntitySet<ApplicationDto>("Application")
        .EntityType.HasKey(x => x.ApplicationId);

    builder.EnableLowerCamelCase();

    return builder.GetEdmModel();
}