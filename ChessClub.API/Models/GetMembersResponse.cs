using System.Text.Json.Serialization;

namespace ChessClub.API.Models
{
    public class GetMembersResponse
    {
        [JsonPropertyName("members")]
        public IEnumerable<MemberDTO> Members { get; set; } = new List<MemberDTO>();
    }
}
