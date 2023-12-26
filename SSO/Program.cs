using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using SSO;
using VueCliMiddleware;

var builder = WebApplication.CreateBuilder(args);

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntityType<WeatherForecast>();
modelBuilder.EntitySet<WeatherForecast>("WeatherForecast");

builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

string[] prefixes = { "/swagger", "/api", "/odata" };

app.MapWhen(r => !prefixes.Any(p => r.Request.Path.Value.StartsWith(p)), builder =>
{
    builder.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp/";
        spa.UseVueCli(npmScript: "serve");
    });
});

app.Run();
