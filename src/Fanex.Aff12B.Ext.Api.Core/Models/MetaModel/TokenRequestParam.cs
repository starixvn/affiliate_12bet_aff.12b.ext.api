using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Aff12B.Ext.Api.Core.Models.MetaModel
{
    public class TokenRequestParam
    {
        const int MaxLengthOfUserName = 50;
        const int MaxLengthOfPassword = 16;

        [MaxLength(MaxLengthOfUserName)]
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [MaxLength(MaxLengthOfPassword)]
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        public bool IsShowIpvn { get; set; } = true;
    }
}
