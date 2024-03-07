using Fanex.Aff12B.Ext.Api.Core.Models;

namespace Fanex.Aff12B.Ext.Api.Core.Services
{
    public interface IAuthenticateService
    {
        Tuple<bool, Token> IsAuthenticated(TokenRequest request);
    }
}
