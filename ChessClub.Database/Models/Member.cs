namespace ChessClub.Database.Models
{
    public class Member
    {
        public Guid Id { get; set; } = default;

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public int CurrentRank { get; set; } = default;

        public int GamesPlayed { get; set; } = default;

        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
