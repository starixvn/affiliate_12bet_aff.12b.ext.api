using Fanex.Aff12B.Common.Core.Helpers;
using Fanex.Aff12B.Common.Core.Models;
using Fanex.Aff12B.Identity.Core.Constants;
using Fanex.Aff12B.Identity.Core.Models.DTO;
using Fanex.Aff12B.Identity.Core.Models.MetaModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Fanex.Aff12B.Identity.Core.Services
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagementOptions _tokenManagement;

        public TokenAuthenticationService(IUserManagementService service,
            IOptions<TokenManagementOptions> tokenManagementAccessor)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagementAccessor.Value;
        }

        public Tuple<bool, Token> IsAuthenticated(TokenRequestParam request)
        {
            var token = new Token
            {
                ExpiresIn = _tokenManagement.AccessExpiration
            };
            var siteName = request.Username;
            var realUserName = request.Username;

            if (request.Username.Contains('@'))
            {
                var temp = request.Username.Split('@');
                siteName = temp[0];
                realUserName = temp[1];
            }
            var userInfo = _userManagementService.GetValidUser(realUserName, request.Password);
            if (userInfo is null)
            {
                return Tuple.Create(false, token);
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, realUserName),
                new (CustomClaimType.Site, siteName),
                new (CustomClaimType.Role, userInfo.Role),
                new (CustomClaimType.Scopes, userInfo.Scopes),
                new (CustomClaimType.IsShowIPVN, request.IsShowIpvn.ToString(), ClaimValueTypes.Boolean)
             };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var encryptCredentials = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg,
                SecurityAlgorithms.Aes256CbcHmacSha512);
            var handler = new JsonWebTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenManagement.Issuer,
                Audience = _tokenManagement.Audience,
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTimeHelper.UtcNow,
                NotBefore = DateTimeHelper.UtcNow,
                Expires = DateTimeHelper.UtcNow.AddMinutes(_tokenManagement.AccessExpiration),
                SigningCredentials = credentials,
                EncryptingCredentials = encryptCredentials
            };

            token.AccessToken = handler.CreateToken(tokenDescriptor);
            token.TokenType = handler.TokenType.Name;

            return Tuple.Create(true, token);
        }
    }
}
