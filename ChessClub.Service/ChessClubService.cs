using ChessClub.Database;
using ChessClub.Database.Models;
using ChessClub.Service.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClub.Service
{
    public class ChessClubService
    {
        private readonly ILogger<ChessClubService> _logger;
        private readonly ChessClubContext _chessClubContext;

        public ChessClubService(ILogger<ChessClubService> logger, ChessClubContext chessClubContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chessClubContext = chessClubContext ?? throw new ArgumentNullException(nameof(chessClubContext));
        }

        public Guid AddMember(string name, string surname, string email, DateTime birthday)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Invalid input:");
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentOutOfRangeException(nameof(surname), "Invalid input:");
            }

            if (!EmailHelper.IsValidEmail(email))
            {
                throw new ArgumentOutOfRangeException(nameof(email), "Invalid input:");
            }

            if (birthday > DateTime.Now.AddYears(-1))
            {
                throw new ArgumentOutOfRangeException(nameof(birthday), "Members should be at least 1 year old.");
            }

            var newMember = new Member
            {
                Name = name,
                Surname = surname,
                Email = email,
                Birthday = birthday.Date,
                CurrentRank = (_chessClubContext.Members.Max(m => (int?)m.CurrentRank) ?? 0) + 1
            };

            _chessClubContext.Members.Add(newMember);
            _chessClubContext.SaveChanges();

            return newMember.Id;
        }
    }
}
