namespace ChessClub.API.Models
{
    public class GetMemberResponse
    {
        public IEnumerable<MemberDTO> Members { get; set; } = new List<MemberDTO>();
    }
}
