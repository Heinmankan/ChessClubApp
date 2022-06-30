using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class AddMemberRequest
    {
        [JsonPropertyName("name")]
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
    }
}
