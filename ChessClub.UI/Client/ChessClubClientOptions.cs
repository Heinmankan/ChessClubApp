namespace ChessClub.UI.Client
{
    public class ChessClubClientOptions
    {
        public const string Position = "ChessClubClient";

        public string BaseAddress { get; set; } = string.Empty;

        public string GetMembersUri { get; set; } = string.Empty;

        public string GetMemberByIdUri { get; set; } = string.Empty;

        public string AddMemberUri { get; set; } = string.Empty;

        public string UpdateMemberUri { get; set; } = string.Empty;

        public string DeleteMemberUri { get; set; } = string.Empty;

        public string AddResultUri { get; set; } = string.Empty;

        public int TimeoutInSeconds { get; set; } = 30;
    }
}
