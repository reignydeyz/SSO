using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Business.Groups;
using SSO.Domain.Management.Interfaces;

namespace SSO.Filters
{
    public class GroupIdValidatorAttribute : ActionFilterAttribute, IAsyncActionFilter
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
            var repo = context.HttpContext.RequestServices.GetService<IGroupRepository>();
            var form = context.ActionArguments[ParameterName] as GroupIdDto;

            bool condition = Relevant
                ? await repo.Any(x => x.GroupId == form!.GroupId && x.DateInactive == null)
                : await repo.Any(x => x.GroupId == form!.GroupId);

            if (!condition) context.Result = new StatusCodeResult(404);
            else await next();
        }
    }
}
