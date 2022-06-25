namespace ChessClub.API.Models
{
    public class MemberDTO
    {
        public Guid Id { get; set; } = default;

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime Birthday { get; set; } = default;

        public int CurrentRank { get; set; } = default;

        public int GamesPlayed { get; set; } = default;
    }
}
