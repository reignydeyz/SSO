using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Domain.Management.Interfaces;
using System.Security.Claims;

namespace SSO.Filters
{
    public class GroupIdValidatorAttribute<T> : ActionFilterAttribute, IAsyncActionFilter where T : class
    {
        /// <summary>
        /// The name(parameter) used in the Action.
        /// </summary>
        public string ParameterName { get; set; } = "form";

        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; set; } = "GroupId";

        /// <summary>
        /// When set to true, validations will be applied, and errors relevant to the status may be triggered.
        /// Default is true.
        /// </summary>
        public bool Relevant { get; set; } = true;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var repo = context.HttpContext.RequestServices.GetService<IGroupRepository>();
            var param = context.ActionArguments[ParameterName] as T;
            var id = (Guid)typeof(T).GetProperty(PropertyName).GetValue(param);
            var realmId = new Guid(context.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.System).Value);

            bool condition = Relevant
                ? await repo.Any(x => x.GroupId == id && x.DateInactive == null && x.RealmId == realmId && x.Realm.DateInactive == null)
                : await repo.Any(x => x.GroupId == id && x.RealmId == realmId);

            if (!condition) context.Result = new StatusCodeResult(404);
            else await next();
        }
    }
}
