using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;

namespace SSO.Filters
{
    public class RealmValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public Realm Realm { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var realmSvc = context.HttpContext.RequestServices.GetService<RealmService>();

            if (realmSvc.Realm != Realm) context.Result = new StatusCodeResult(503);
            else await next();
        }
    }
}
