using Bogus;
using ChessClub.Database;
using ChessClub.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ChessClub.Service.Tests
{
    [TestFixture]
    public partial class ChessClubServiceTests
    {
        private ChessClubContext? _chessClubContext;
        private ChessClubService? _chessClubService;


        [OneTimeSetUp]
        public void GlobalPrepare()
        {
            var builder = new DbContextOptionsBuilder<ChessClubContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            _chessClubContext = new ChessClubContext(builder.Options, new NullLoggerFactory());
            _chessClubService = new ChessClubService(NullLogger<ChessClubService>.Instance, _chessClubContext);

            SeedDatabase();
        }

        [Test]
        public void TestDatabaseHasData()
        {
            var memberCount = _chessClubContext?.Members.Count();

            Assert.NotNull(_chessClubContext);

            Assert.Greater(memberCount, 0);
        }

        private static Faker<MemberFakerModel> MemberFaker => new Faker<MemberFakerModel>()
                .RuleFor(m => m.Name, f => f.Name.FirstName())
                .RuleFor(m => m.Surname, f => f.Name.LastName())
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.Birthday, f => f.Date.PastOffset(60, DateTime.Now.AddYears(-18)).Date);

        private void SeedDatabase()
        {
            int currentRank = _chessClubContext?.Members?.Max(m => (int?)m.CurrentRank) ?? 1;

            var members = MemberFaker.Generate(10)
                .Select(m => new Member
                {
                    Name = m.Name,
                    Surname = m.Surname,
                    Email = m.Email,
                    Birthday = m.Birthday,
                    CurrentRank = currentRank++,
                    GamesPlayed = 0
                }).ToList();

            _chessClubContext?.Members.AddRange(members);
            _chessClubContext?.SaveChanges();
        }
    }
}
