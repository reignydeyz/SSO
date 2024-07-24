using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;
using System.Security.Claims;

namespace SSO.Filters
{
    public class IdpValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public IdentityProvider IdentityProvider { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var realmRepo = context.HttpContext.RequestServices.GetService<IRealmRepository>();
            var realmId = new Guid(context.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.System).Value);

            bool condition = await realmRepo.Any(x => x.RealmId == realmId && x.IdpSettingsCollection.Any(y => y.IdentityProvider == IdentityProvider));

            if (!condition) context.Result = new StatusCodeResult(503);
            else await next();
        }
    }
}
