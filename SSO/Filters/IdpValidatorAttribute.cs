using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;

namespace SSO.Filters
{
    public class IdpValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public IdentityProvider IdentityProvider { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idpSvc = context.HttpContext.RequestServices.GetService<IdpService>();

            if (idpSvc.IdentityProvider != IdentityProvider) context.Result = new StatusCodeResult(503);
            else await next();
        }
    }
}
