namespace Fanex.Aff12B.Ext.Api.Core.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AffFilterAttribute(IConfiguration configuration) : ActionFilterAttribute
    {
        private const string AffId = "affId";
        private const string AffAccessList = "AffAccessList";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Key.Contains(AffId)).Value;
            var affAccessList = configuration.GetSection(AffAccessList).Get<List<int>>();

            var isAccessed = affAccessList is null || (param != null && affAccessList.Exists(affId => affId == (int)param));
            if (!isAccessed)
            {
                context.Result = new BadRequestObjectResult("The Affiliate is invalid or unsupport!");
            }
        }
    }
}