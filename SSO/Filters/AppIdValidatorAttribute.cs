using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.Applications;
using SSO.Domain.Management.Interfaces;

namespace SSO.Filters
{
    public class AppIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        readonly string _paramName;

        public AppIdValidatorAttribute(string? paramName = "form")
        {
            _paramName = paramName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var appRepo = context.HttpContext.RequestServices.GetService<IApplicationRepository>();

            var form = context.ActionArguments[_paramName] as ApplicationIdDto;

            if (!(await appRepo.Any(x => x.ApplicationId == form.ApplicationId! && x.DateInactive == null)))
                context.Result = new StatusCodeResult(404);

            if (context.Result is null)
                await next();
        }
    }
}
