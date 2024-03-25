namespace Fanex.Aff12B.Ext.Api.Core.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AffFilterAttribute : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;

        private const string AffId = "affId";
        private const string AffAccessList = "AffAccessList";

        public AffFilterAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Key.Contains(AffId)).Value;
            var affAccessList = _configuration.GetSection(AffAccessList).Get<List<int>>();

            var isAccessed = affAccessList is null || (param != null && affAccessList.Exists(affId => affId == (int)param));
            if (!isAccessed)
            {
                context.Result = new BadRequestObjectResult("API access is not permitted!");
            }
        }
    }
}