namespace Fanex.Aff12B.Api.Core.Attributes
{
    using Fanex.Aff12B.Api.Core.Extension;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AffFilterAttribute(IConfiguration configuration) : ActionFilterAttribute
    {
        private const string AffId = "affId";
        private const string SuppotedAffList = "SuppotedAffList";
        private const string AccessedAffsClaimType = "AccessedAffs";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var affIdQuery = context.ActionArguments
                .SingleOrDefault(x => x.Key.Contains(AffId)).Value.ToString();
            var suppotedAffList = configuration.GetSection(SuppotedAffList).Get<List<string>>();
            var accessedAffsByClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == AccessedAffsClaimType)?.Value.Split(',');

            var isNull = affIdQuery is null || suppotedAffList is null || accessedAffsByClaim is null;
            
            if (isNull || !(suppotedAffList.Exists(affId => affId.IsMatch(affIdQuery)) && Array.Exists(accessedAffsByClaim, affId => affId.IsMatch(affIdQuery))))
            {
                context.Result = new BadRequestObjectResult("The Affiliate is invalid or unsupported!");
            }
        }
    }
}