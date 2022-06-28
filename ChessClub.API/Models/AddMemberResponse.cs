using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class AddMemberResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
