using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.Authentication;
using SSO.Domain.Management.Interfaces;

namespace SSO.Web.Filters
{
    public class AppIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var appRepo = context.HttpContext.RequestServices.GetService<IApplicationRepository>();

            var form = context.ActionArguments["form"] as AuthDto;
            
            if (form.AppId.HasValue)
            {
                // TODO: Add Any()
                var app = await appRepo.FindOne(x => x.ApplicationId == form.AppId! && x.DateInactive == null);

                if (app is null)
                    context.Result = new StatusCodeResult(404);
            }

            if (context.Result is null)
                await next();
        }
    }
}
