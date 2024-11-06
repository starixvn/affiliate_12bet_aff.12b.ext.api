
namespace Fanex.Aff12B.Common.Core.Models
{
    public class TokenManagementOptions
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessExpiration { get; set; }
    }
}
