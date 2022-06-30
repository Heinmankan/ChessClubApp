using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class MemberDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = default;

        [JsonPropertyName(name: "name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("surname")]
        [Required]
        public string Surname { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("birthday")]
        [Required]
        public DateTime Birthday { get; set; } = default;

        [JsonPropertyName("current-rank")]
        public int CurrentRank { get; set; } = default;

        [JsonPropertyName("games-played")]
        public int GamesPlayed { get; set; } = default;
    }
}
