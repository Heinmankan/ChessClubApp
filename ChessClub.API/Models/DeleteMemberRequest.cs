using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class DeleteMemberRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
