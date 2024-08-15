using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.ApplicationUserRoles;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Management;

namespace SSO.Filters
{
    public class AppUserIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
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
            var appRepo = context.HttpContext.RequestServices.GetService<IApplicationRepository>();
            var userRepo = context.HttpContext.RequestServices.GetService<UserRepository>();

            var form = context.ActionArguments[ParameterName] as AppUserIdDto;

            bool condition = Relevant
                ? await appRepo.Any(x => x.ApplicationId == form.ApplicationId! && x.DateInactive == null)
                : await appRepo.Any(x => x.ApplicationId == form.ApplicationId!);

            if (condition)
            {
                condition = Relevant
                ? await userRepo.Any(x => x.Id == form.UserId.ToString() && x.DateInactive == null)
                : await userRepo.Any(x => x.Id == form.UserId.ToString());
            }

            if (!condition) context.Result = new StatusCodeResult(404);
            else await next();
        }
    }
}
