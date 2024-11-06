using Fanex.Aff12B.Common.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Fanex.Aff12B.Common.Core.Utilities
{
    public static class TokenUtils
    {
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            var token = configuration.GetSection("TokenManagement").Get<TokenManagementOptions>();

            return token != null ? new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                ValidIssuer = token.Issuer,
                ValidAudience = token.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                RoleClaimType = ClaimsIdentity.DefaultNameClaimType,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            } : new TokenValidationParameters();
        }
    }
}
