using Newtonsoft.Json;

namespace JWTManager.Models
{
    public class JwtPayload
    {
        [JsonProperty("uname")]
        public string Username { get; set; } = null!;
        [JsonProperty("exp")]
        public DateTime ExpiresAt { get; set; }
    }
}
