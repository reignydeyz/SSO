using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.Users;
using SSO.Domain.Management.Interfaces;

namespace SSO.Filters
{
    public class UserIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
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
            var repo = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var form = context.ActionArguments[ParameterName] as UserIdDto;

            bool condition = Relevant
                ? await repo.Any(x => x.Id == form.UserId.ToString() && x.DateInactive == null)
                : await repo.Any(x => x.Id == form.UserId.ToString());

            if (!condition) context.Result = new StatusCodeResult(404);
            else await next();
        }
    }
}
