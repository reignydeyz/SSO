using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.Applications;
using SSO.Domain.Management.Interfaces;
using System.Security.Claims;

namespace SSO.Filters
{
    public class AppIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
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
            var repo = context.HttpContext.RequestServices.GetService<IApplicationRepository>();
            var form = context.ActionArguments[ParameterName] as ApplicationIdDto;
            var realm = new Guid(context.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.System).Value);

            bool condition = Relevant
                ? await repo.Any(x => x.ApplicationId == form.ApplicationId! && x.DateInactive == null && x.RealmId == realm && x.Realm.DateInactive == null)
                : await repo.Any(x => x.ApplicationId == form.ApplicationId! && x.RealmId == realm && x.Realm.DateInactive == null);

            if (!condition) context.Result = new StatusCodeResult(404);
            else await next();
        }
    }
}
