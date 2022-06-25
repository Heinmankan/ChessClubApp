namespace ChessClub.API.Models
{
    public class UpdateMemberRequest : AddMemberRequest
    {
        public Guid Id { get; set; }
    }
}
