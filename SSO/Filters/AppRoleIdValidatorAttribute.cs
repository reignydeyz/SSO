using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.ApplicationRoles;
using SSO.Domain.Management.Interfaces;

namespace SSO.Filters
{
    public class AppRoleIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        readonly string _paramName;

        public AppRoleIdValidatorAttribute(string? paramName = "form")
        {
            _paramName = paramName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var repo = context.HttpContext.RequestServices.GetService<IApplicationRoleRepository>();

            var form = context.ActionArguments[_paramName] as AppRoleIdDto;

            if (!(await repo.Any(x => x.ApplicationId == form.ApplicationId && x.Id == form.RoleId.ToString() && x.Application.DateInactive == null)))
                context.Result = new StatusCodeResult(404);

            if (context.Result is null)
                await next();
        }
    }
}
