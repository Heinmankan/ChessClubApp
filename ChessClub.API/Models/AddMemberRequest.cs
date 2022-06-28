using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class AddMemberRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("surname")]
        public string Surname { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; } = default;
    }
}
