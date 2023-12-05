using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Domain.Management.Interfaces;

namespace SSO.Web.Filters
{
    public class ValidateOriginAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Validate origin header
            var origin = context.HttpContext.Request.Headers["Origin"];

            if (string.IsNullOrEmpty(origin))
                context.Result = new StatusCodeResult(403);
            else
            {
                // Validate app
                var appId = context.HttpContext.Request.Query["appId"];
                var appRepository = context.HttpContext.RequestServices.GetService<IApplicationRepository>();

                var app = string.IsNullOrEmpty(appId)
                        ? await appRepository.GetRoot()
                        : Guid.TryParse(appId, out var parsedGuid)
                            ? await appRepository.FindOne(x => x.ApplicationId == parsedGuid)
                            : null;

                if (app is null)
                    context.Result = new StatusCodeResult(404);
                else
                {
                    // Validate allowed origin
                    var allowedOrigins = await appRepository.GetAllowedOrigins(app.ApplicationId);

                    if (!allowedOrigins.Contains(origin))
                        context.Result = new StatusCodeResult(400);

                    if (context.Result is null)
                        await next();
                }
            }
        }
    }
}
