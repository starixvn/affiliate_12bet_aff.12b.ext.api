using Fanex.Aff12B.Ext.Api.Core.Constant;
using Fanex.Aff12B.Ext.Api.Core.Models;
using Fanex.Aff12B.Ext.Api.Core.Models.DTO;
using Fanex.Aff12B.Ext.Api.Core.Models.MetaModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Fanex.Aff12B.Ext.Api.Core.Services
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

            if (!_userManagementService.IsValidUser(realUserName, request.Password))
            {
                return Tuple.Create(false, token);
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, realUserName),
                new (CustomClaimType.Site, siteName),
                new (CustomClaimType.IsShowIPVN, request.IsShowIpvn.ToString(), ClaimValueTypes.Boolean)
             };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var encryptCredenction = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg,
                SecurityAlgorithms.Aes256CbcHmacSha512);
            var handler = new JsonWebTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenManagement.Issuer,
                Audience = _tokenManagement.Audience,
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_tokenManagement.AccessExpiration),
                SigningCredentials = credentials,
                EncryptingCredentials = encryptCredenction
            };

            token.AccessToken = handler.CreateToken(tokenDescriptor);
            token.TokenType = handler.TokenType.Name;

            return Tuple.Create(true, token);
        }
    }
}
