using Newtonsoft.Json;

namespace JWTManager.Models
{
    public class JwtHeader
    {
        [JsonProperty("alg")]
        public string Algorithm { get; set; } = null!;
        [JsonProperty("typ")]
        public string Type { get; set; } = null!;
    }
}
