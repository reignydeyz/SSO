using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Domain.Management.Interfaces;

namespace SSO.Filters
{
    public class RealmIdValidatorAttribute<T> : ActionFilterAttribute, IAsyncActionFilter where T : class
    {
        /// <summary>
        /// The name(parameter) used in the Action.
        /// </summary>
        public string ParameterName { get; set; } = "form";

        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; set; } = "RealmId";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var repo = context.HttpContext.RequestServices.GetService<IRealmUserRepository>();
            var param = context.ActionArguments[ParameterName] as T;
            var id = (Guid?)typeof(T).GetProperty(PropertyName).GetValue(param);

            var condition = !id.HasValue || await repo.Any(x => x.RealmId == id && x.Realm.DateInactive == null);

            if (!condition) context.Result = new StatusCodeResult(400);
            else await next();
        }
    }
}
