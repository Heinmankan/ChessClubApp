namespace ChessClub.API.Models
{
    public class AddMemberRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime Birthday { get; set; } = default;
    }
}
