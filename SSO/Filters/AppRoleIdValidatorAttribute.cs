using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.ApplicationRoles;
using SSO.Domain.Management.Interfaces;

namespace SSO.Filters
{
    public class AppRoleIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        /// <summary>
        /// The name(parameter) used in the Action.
        /// </summary>
        public string ParameterName { get; set; } = "form";

        /// <summary>
        /// When set to true, validations will be applied, and errors relevant to the status may be triggered.
        /// Default is true.
        /// </summary>
        public bool Relevant { get; set; } = true;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var repo = context.HttpContext.RequestServices.GetService<IApplicationRoleRepository>();
            var form = context.ActionArguments[ParameterName] as AppRoleIdDto;

            bool condition = Relevant
                ? await repo.Any(x => x.ApplicationId == form.ApplicationId! && x.Id == form.RoleId.ToString() && x.Application.DateInactive == null)
                : await repo.Any(x => x.ApplicationId == form.ApplicationId! && x.Id == form.RoleId.ToString());

            if (!condition) context.Result = new StatusCodeResult(404);
            else await next();
        }
    }
}
