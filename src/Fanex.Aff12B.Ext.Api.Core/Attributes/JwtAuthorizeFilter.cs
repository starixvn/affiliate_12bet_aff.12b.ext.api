using Fanex.Aff12B.Ext.Api.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Fanex.Aff12B.Ext.Api.Core.Attributes
{
    internal class JwtAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public JwtAuthorizeFilter(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.Request.Query["access_token"].ToString();
            if (!string.IsNullOrEmpty(accessToken))
            {
                var handler = new JsonWebTokenHandler();
                var tokenValidationParameters = TokenUtils.GetTokenValidationParameters(_configuration);
                var result = handler.ValidateTokenAsync(accessToken, tokenValidationParameters).Result;

                if (result == null || !result.IsValid) {
                    context.Result = new UnauthorizedObjectResult("You don't have permission to access");
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("You don't have permission to access");
            }
        }
    }
}
