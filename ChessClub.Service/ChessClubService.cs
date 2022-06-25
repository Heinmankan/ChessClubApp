﻿using ChessClub.Database;
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
    public class ChessClubService : IChessClubService
    {
        private readonly ILogger<ChessClubService> _logger;
        private readonly ChessClubContext _chessClubContext;

        public ChessClubService(ILogger<ChessClubService> logger, ChessClubContext chessClubContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chessClubContext = chessClubContext ?? throw new ArgumentNullException(nameof(chessClubContext));
        }

        public IEnumerable<Member> GetMembers(int pageNumber = 1, int pageSize = 20)
        {
            return _chessClubContext.Members
                .OrderBy(m => m.CurrentRank)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
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

        public async Task<bool> AddResult(Guid player1, Guid player2, Guid? winner = null)
        {
            if (player1 == player2)
            {
                _logger.LogError("Cannot play against yourself. Id1 = {player1}, Id2 = {player2}", player1, player2);

                return false;
            }

            var members = _chessClubContext.Members.Where(m => m.Id == player1 || m.Id == player2).OrderBy(m => m.CurrentRank).ToList();

            if (members.Count != 2)
            {
                // Could not find a member... (specify which one)
                if (!members.Any(m => m.Id == player1))
                {
                    _logger.LogError("Unable to find member: {memberId}", player1);
                }
                else if (!members.Any(m => m.Id == player2))
                {
                    _logger.LogError("Unable to find member: {memberId}", player2);
                }

                return false;
            }

            if (winner is null)
            {
                // If it’s a draw, the lower-ranked player can gain one position, unless the two players are
                // adjacent. So if the players are ranked 10th and 11th, and it’s a draw, no change in
                // ranking takes place. But if the players are ranked 10th and 15th, and it’s a draw, the
                // player with rank 15 will move up to rank 14 and the player with rank 10 will stay the
                // same

                var higherPositionMember = members.First();
                var lowerPositionMember = members.Last();

                if (lowerPositionMember.CurrentRank - higherPositionMember.CurrentRank > 1)
                {
                    // Move lowerPositionMember Position up by 1
                    var swapMember = _chessClubContext.Members.First(m => m.CurrentRank == lowerPositionMember.CurrentRank - 1);
                    swapMember.CurrentRank += 1;
                    lowerPositionMember.CurrentRank -= 1;

                    _chessClubContext.Members.UpdateRange(new List<Member>() { swapMember, lowerPositionMember });
                    var count = await _chessClubContext.SaveChangesAsync();

                    if (count != 2)
                    {
                        _logger.LogError("Error saving draw result: {count}", count);

                        return false;
                    }
                }

                return true;
            }

            var winningMember = members.First(m => m.Id == winner);
            var winningPosition = winningMember.CurrentRank;

            var losingMember = members.First(m => m.Id != winner);
            var losingPosition = losingMember.CurrentRank;

            double positionDifference = (double)(winningPosition - losingPosition);

            // If the lower-ranked player beats a higher-ranked player, the higher-ranked player will
            // move one rank down, and the lower level player will move up by half the difference
            // between their original ranks.
            // For example, if players ranked 10th and 16th play and the lower-ranked player wins, the
            // first player will move to rank 11th and the other player will move to rank (16 - 10) / 2 = 3
            // placed up, to rank 13th

            if (positionDifference > 0) // Winning member has lower ranking than winning member
            {
                var lowerPosition = Math.Min(winningPosition, losingPosition);
                var higherPosition = Math.Max(winningPosition, losingPosition);

                var affectedMembers = _chessClubContext.Members
                    .Where(m => m.CurrentRank >= lowerPosition && m.CurrentRank <= higherPosition)
                    .ToList();

                // Move losing member down 1 position:
                // If the lower-ranked player beats a higher-ranked player, the higher-ranked player will move one rank down
                var memberToMoveDown = affectedMembers.First(m => m.CurrentRank == losingPosition);
                var memberToMoveUp = affectedMembers.First(m => m.CurrentRank == losingPosition + 1);

                memberToMoveDown.CurrentRank += 1;
                memberToMoveUp.CurrentRank -= 1;

                // Move the winning member up:
                // ..., and the lower level player will move up by half the difference between their original ranks.
                if (positionDifference > 1) // Move the winning member up 
                {
                    var winnerMoveUpPositions = (int)Math.Round(positionDifference / 2, 0, MidpointRounding.AwayFromZero);

                    var winnerNewPosition = winningPosition - winnerMoveUpPositions;

                    var membersToMoveUp = affectedMembers.Where(m => m.CurrentRank >= winnerNewPosition && m.CurrentRank < winningPosition).ToList();
                    var membersToUpdate = membersToMoveUp.Count;

                    winningMember.CurrentRank = winnerNewPosition;

                    membersToMoveUp.ForEach(m => m.CurrentRank++);

                    _chessClubContext.Members.UpdateRange(new List<Member>(membersToMoveUp));
                    _chessClubContext.Members.Update(winningMember);

                    var count = await _chessClubContext.SaveChangesAsync();

                    if (count != membersToUpdate + 1)
                    {
                        _logger.LogError("Error saving draw result: {count}", count);

                        return false;
                    }
                }
            }

            return true;
        }
    }
}
