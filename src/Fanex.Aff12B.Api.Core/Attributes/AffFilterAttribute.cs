namespace Fanex.Aff12B.Api.Core.Attributes
{
    using Fanex.Aff12B.Api.Core.Extension;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AffFilterAttribute(IConfiguration configuration, ILogger<AffFilterAttribute> logger) : ActionFilterAttribute
    {
        private const string AffId = "affId";
        private const string SuppotedAffList = "SuppotedAffList";
        private const string AccessedAffsClaimType = "AccessedAffs";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var affIdQuery = context.ActionArguments
                .SingleOrDefault(x => x.Key.Contains(AffId)).Value.ToString();
            var suppotedAffList = configuration.GetSection(SuppotedAffList).Get<string>()?.Split(",");
            var accessedAffsByClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == AccessedAffsClaimType)?.Value.Split(',');

            var isNull = affIdQuery is null || suppotedAffList is null || accessedAffsByClaim is null;

            if (isNull || !(Array.Exists(suppotedAffList, affId => affId.IsMatch(affIdQuery)) && Array.Exists(accessedAffsByClaim, affId => affId.IsMatch(affIdQuery))))
            {
                context.Result = new BadRequestObjectResult("The Affiliate is invalid or unsupported!");

                if (!isNull)
                {
                    string message = $"The user {context.HttpContext.User.Identity.Name} tried accessing an invalid affiliate (AffId: {affIdQuery})";
                    logger.LogCritical(message);
                }
            }
        }
    }
}