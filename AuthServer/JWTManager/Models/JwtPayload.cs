using Newtonsoft.Json;

namespace JWTManager.Models
{
    public class JwtPayload
    {
        [JsonProperty("userid")]
        public int UserId { get; set; }

        [JsonProperty("uname")]
        public string Username { get; set; } = null!;

        [JsonProperty("fname")]
        public string FirstName { get; set; } = null!;

        [JsonProperty("lname")]
        public string LastName { get; set; } = null!;

        [JsonProperty("mail")]
        public string Email { get; set; } = null!; 
    }
}
