using ChessClub.Database.Models;

namespace ChessClub.Service
{
    public interface IChessClubService
    {
        IEnumerable<Member> GetMembers(int pageNumber = 1, int pageSize = 20);

        Member GetMemberById(Guid id);

        Guid AddMember(string name, string surname, string email, DateTime birthday);
        
        bool UpdateMember(Guid Id, string name = "", string surname = "", string email = "", DateTime birthday = default);
        
        bool DeleteMember(Guid id);

        Task<bool> AddResult(Guid player1, Guid player2, Guid? winner = null);
    }
}
