using Fanex.Aff12B.Identity.Core.Models.DTO;
using Fanex.Aff12B.Identity.Core.Models.MetaModel;

namespace Fanex.Aff12B.Identity.Core.Services
{
    public interface IAuthenticateService
    {
        Tuple<bool, Token> IsAuthenticated(TokenRequestParam request);
    }
}
