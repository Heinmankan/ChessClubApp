using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class UpdateMemberRequest : AddMemberRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
