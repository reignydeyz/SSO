using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Domain.Management.Interfaces;
using System.Security.Claims;

namespace SSO.Filters
{
    public class AppIdValidatorAttribute<T> : ActionFilterAttribute, IAsyncActionFilter where T : class
    {
        /// <summary>
        /// The name(parameter) used in the Action.
        /// </summary>
        public string ParameterName { get; set; } = "form";

        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; set; } = "ApplicationId";

        /// <summary>
        /// When set to true, validations will be applied, and errors relevant to the status may be triggered.
        /// Default is true.
        /// </summary>
        public bool Relevant { get; set; } = true;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var repo = context.HttpContext.RequestServices.GetService<IApplicationRepository>();
            var realmRepo = context.HttpContext.RequestServices.GetService<IRealmRepository>();
            var param = context.ActionArguments[ParameterName] as T;
            var id = (Guid)typeof(T).GetProperty(PropertyName).GetValue(param);

            var realmId = context.HttpContext.User.Identity.IsAuthenticated
                ? new Guid(context.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value)
                : !string.IsNullOrEmpty(context.HttpContext.Request.Query["realmId"])
                    ? (await realmRepo.FindOne(x => x.RealmId == new Guid(context.HttpContext.Request.Query["realmId"]!) && x.DateInactive == null))?.RealmId
                    : (await realmRepo.FindOne(x => x.Name == "Default" && x.DateInactive == null))?.RealmId;

            bool condition = realmId.HasValue && (Relevant
                ? await repo.Any(x => x.ApplicationId == id && x.DateInactive == null && x.RealmId == realmId && x.Realm.DateInactive == null)
                : await repo.Any(x => x.ApplicationId == id && x.RealmId == realmId));

            if (!condition) context.Result = new StatusCodeResult(404);
            else await next();
        }
    }
}
