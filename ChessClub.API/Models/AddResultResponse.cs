using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class AddResultResponse
    {
        [JsonPropertyName("is-success")]
        public bool IsSuccess { get; set; }
    }
}
