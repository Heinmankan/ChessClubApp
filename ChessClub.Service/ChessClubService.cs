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
            if (!Validations.IsValidName(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Invalid input:");
            }

            if (!Validations.IsValidSurname(surname))
            {
                throw new ArgumentOutOfRangeException(nameof(surname), "Invalid input:");
            }

            if (!Validations.IsValidEmailAddress(email))
            {
                throw new ArgumentOutOfRangeException(nameof(email), "Invalid input:");
            }

            if (!Validations.IsValidBirthday(birthday))
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

        public bool UpdateMember(Guid Id, string name = "", string surname = "", string email = "", DateTime birthday = default)
        {
            var hasChanges = false;
            var memberToUpdate = _chessClubContext.Members.First(m => m.Id == Id);

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (!Validations.IsValidName(name))
                {
                    throw new ArgumentOutOfRangeException(nameof(name), "Invalid input:");
                }

                memberToUpdate.Name = name;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(surname))
            {
                if (!Validations.IsValidSurname(surname))
                {
                    throw new ArgumentOutOfRangeException(nameof(surname), "Invalid input:");
                }

                memberToUpdate.Surname = surname;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!Validations.IsValidEmailAddress(email))
                {
                    throw new ArgumentOutOfRangeException(nameof(email), "Invalid input:");
                }

                memberToUpdate.Email = email;
                hasChanges = true;
            }

            if (birthday != default)
            {
                if (!Validations.IsValidBirthday(birthday))
                {
                    throw new ArgumentOutOfRangeException(nameof(birthday), "Members should be at least 1 year old.");
                }

                memberToUpdate.Birthday = birthday;
                hasChanges = true;
            }

            if (hasChanges)
            {
                return _chessClubContext.SaveChanges() == 1;
            }

            return true; // This could also be an error if nothing was given to update. Decided to just return, no harm done.
        }

        public bool DeleteMember(Guid id)
        {
            var memberToDelete = _chessClubContext.Members.First(m => m.Id == id);

            _chessClubContext.Members.Remove(memberToDelete);

            var membersToUpdate = _chessClubContext.Members.Where(m => m.CurrentRank >= memberToDelete.CurrentRank).ToList();

            foreach (var member in membersToUpdate)
            {
                member.CurrentRank -= 1;
            }

            _chessClubContext.SaveChanges();

            return true;
        }
    }
}
