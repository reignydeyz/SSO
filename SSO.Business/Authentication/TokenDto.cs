using System.Text.Json.Serialization;

namespace SSO.Business.Authentication
{
    public class TokenDto
    {
        public Guid Id { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires")]
        public DateTime Expires { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";
    }
}
