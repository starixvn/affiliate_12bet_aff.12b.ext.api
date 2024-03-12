using Newtonsoft.Json;

namespace Fanex.Aff12B.Ext.Api.Core.Models
{
    public class TokenManagementOptions
    {
        public string Secret { get; set; } = string.Empty;

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessExpiration { get; set; }

        public int RefreshExpiration { get; set; }
    }
}
