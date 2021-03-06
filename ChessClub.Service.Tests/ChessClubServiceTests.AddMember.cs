using ChessClub.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ChessClub.Service.Tests
{
    [TestFixture]
    public partial class ChessClubServiceTests
    {
        [Test]
        public void TestAddFirstMember()
        {
            var builder = new DbContextOptionsBuilder<ChessClubContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var chessClubContext = new ChessClubContext(builder.Options, new NullLoggerFactory());
            var chessClubService = new ChessClubService(NullLogger<ChessClubService>.Instance, chessClubContext);

            var testMember = MemberFaker.Generate(1).First();

            Assert.NotNull(chessClubContext);
            Assert.NotNull(chessClubService);

            var maxMemberRank = chessClubContext?.Members?.Max(m => (int?)m.CurrentRank) ?? 0;

            var result = chessClubService?.AddMember(testMember.Name, testMember.Surname, testMember.Email, testMember.Birthday);

            var newMember = chessClubContext?.Members.First(m => m.Id == result);

            Assert.IsNotNull(newMember);

            Assert.AreEqual(testMember.Name, newMember?.Name);
            Assert.AreEqual(testMember.Surname, newMember?.Surname);
            Assert.AreEqual(testMember.Email, newMember?.Email);
            Assert.AreEqual(testMember.Birthday, newMember?.Birthday);
            Assert.AreEqual(maxMemberRank + 1, newMember?.CurrentRank);
        }

        [Test]
        public void TestAddMember()
        {
            var testMember = MemberFaker.Generate(1).First();

            Assert.NotNull(_chessClubContext);
            Assert.NotNull(_chessClubService);

            var maxMemberRank = _chessClubContext?.Members?.Max(m => (int?)m.CurrentRank) ?? 0;

            var result = _chessClubService?.AddMember(testMember.Name, testMember.Surname, testMember.Email, testMember.Birthday);

            var newMember = _chessClubContext?.Members.First(m => m.Id == result);

            Assert.IsNotNull(newMember);

            Assert.AreEqual(testMember.Name, newMember?.Name);
            Assert.AreEqual(testMember.Surname, newMember?.Surname);
            Assert.AreEqual(testMember.Email, newMember?.Email);
            Assert.AreEqual(testMember.Birthday, newMember?.Birthday);
            Assert.AreEqual(maxMemberRank + 1, newMember?.CurrentRank);
        }

        [Test]
        public void TestAddMemberWithIncorrectName()
        {
            var testMember = MemberFaker.Generate(1).First();
            testMember.Name = "";

            Assert.NotNull(_chessClubContext);
            Assert.NotNull(_chessClubService);

            ArgumentOutOfRangeException? ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => { var result = _chessClubService?.AddMember(testMember.Name, testMember.Surname, testMember.Email, testMember.Birthday); });

            Assert.NotNull(ex);
            Assert.That(ex?.Message, Is.EqualTo("Invalid input: (Parameter 'name')"));
        }

        [Test]
        public void TestAddMemberWithIncorrectSurname()
        {
            var testMember = MemberFaker.Generate(1).First();
            testMember.Surname = "";

            Assert.NotNull(_chessClubContext);
            Assert.NotNull(_chessClubService);

            ArgumentOutOfRangeException? ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => { var result = _chessClubService?.AddMember(testMember.Name, testMember.Surname, testMember.Email, testMember.Birthday); });

            Assert.NotNull(ex);
            Assert.That(ex?.Message, Is.EqualTo("Invalid input: (Parameter 'surname')"));
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("a.")]
        [TestCase(".a  ")]
        public void TestAddMemberWithIncorrectEmail(string email)
        {
            var testMember = MemberFaker.Generate(1).First();
            testMember.Email = email;

            Assert.NotNull(_chessClubContext);
            Assert.NotNull(_chessClubService);

            ArgumentOutOfRangeException? ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => { var result = _chessClubService?.AddMember(testMember.Name, testMember.Surname, testMember.Email, testMember.Birthday); });

            Assert.NotNull(ex);
            Assert.That(ex?.Message, Is.EqualTo("Invalid input: (Parameter 'email')"));
        }

        [Test]
        public void TestAddMemberWithIncorrectBirthday()
        {
            var testMember = MemberFaker.Generate(1).First();
            testMember.Birthday = DateTime.Now.AddMonths(-11);

            Assert.NotNull(_chessClubContext);
            Assert.NotNull(_chessClubService);

            ArgumentOutOfRangeException? ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => { var result = _chessClubService?.AddMember(testMember.Name, testMember.Surname, testMember.Email, testMember.Birthday); });

            Assert.NotNull(ex);
            Assert.That(ex?.Message, Is.EqualTo("Members should be at least 1 year old. (Parameter 'birthday')"));
        }
    }
}
