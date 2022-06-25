using ChessClub.Database.Models;

namespace ChessClub.Service
{
    public interface IChessClubService
    {
        Guid AddMember(string name, string surname, string email, DateTime birthday);
        Task<bool> AddResult(Guid player1, Guid player2, Guid? winner = null);
        bool DeleteMember(Guid id);
        IEnumerable<Member> GetMembers(int pageNumber = 1, int pageSize = 20);
        bool UpdateMember(Guid Id, string name = "", string surname = "", string email = "", DateTime birthday = default);
    }
}
