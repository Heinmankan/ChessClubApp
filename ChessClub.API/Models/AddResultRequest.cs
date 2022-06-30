using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class AddResultRequest
    {
        [JsonPropertyName("player1")]
        public Guid Player1 { get; set; }

        [JsonPropertyName("player2")]
        public Guid Player2 { get; set; }

        [JsonPropertyName("winner")]
        public Guid? Winner { get; set; } = null;
    }
}
