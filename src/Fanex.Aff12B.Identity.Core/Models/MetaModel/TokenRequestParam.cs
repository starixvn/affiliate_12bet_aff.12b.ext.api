using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fanex.Aff12B.Identity.Core.Models.MetaModel
{
    public class TokenRequestParam
    {
        const int MaxLengthOfUserName = 50;
        const int MaxLengthOfPassword = 16;

        [MaxLength(MaxLengthOfUserName)]
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [MaxLength(MaxLengthOfPassword)]
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        public bool IsShowIpvn { get; set; } = true;
    }
}
