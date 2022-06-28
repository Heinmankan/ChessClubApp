using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class MemberDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = default;

        [JsonPropertyName(name: "name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("surname")]
        public string Surname { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; } = default;

        [JsonPropertyName("current-rank")]
        public int CurrentRank { get; set; } = default;

        [JsonPropertyName("games-played")]
        public int GamesPlayed { get; set; } = default;
    }
}
