using ChessClub.Database;
using ChessClub.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClub.Service.Tests
{
    [TestFixture]
    public partial class ChessClubServiceTests
    {
        [Test]
        public void TestDeleteFirstMember()
        {
            var builder = new DbContextOptionsBuilder<ChessClubContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var chessClubContext = new ChessClubContext(builder.Options);
            var chessClubService = new ChessClubService(NullLogger<ChessClubService>.Instance, chessClubContext);

            int i = 1;

            var testMembers = MemberFaker.Generate(5).Select(m => new Member
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-00000000000{i}"),
                Name = m.Name,
                Surname = m.Surname,
                Email = m.Email,
                Birthday = m.Birthday,
                GamesPlayed = 0,
                CurrentRank = i++
            });

            Assert.NotNull(chessClubContext);
            Assert.NotNull(chessClubService);

            chessClubContext.Members.AddRange(testMembers);
            chessClubContext.SaveChanges();

            var memberToDelete = chessClubContext.Members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000001"));

            var result = chessClubService.DeleteMember(memberToDelete.Id);

            Assert.AreEqual(true, result);

            var memberCount = chessClubContext.Members.Count();

            Assert.AreEqual(4, memberCount);

            var members = chessClubContext.Members.OrderBy(m => m.Id).ToList();

            Assert.AreEqual(1, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000002")).CurrentRank);
            Assert.AreEqual(2, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000003")).CurrentRank);
            Assert.AreEqual(3, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")).CurrentRank);
            Assert.AreEqual(4, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000005")).CurrentRank);
        }



        [Test]
        public void TestDeleteMiddleMember()
        {
            var builder = new DbContextOptionsBuilder<ChessClubContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var chessClubContext = new ChessClubContext(builder.Options);
            var chessClubService = new ChessClubService(NullLogger<ChessClubService>.Instance, chessClubContext);

            int i = 1;

            var testMembers = MemberFaker.Generate(5).Select(m => new Member
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-00000000000{i}"),
                Name = m.Name,
                Surname = m.Surname,
                Email = m.Email,
                Birthday = m.Birthday,
                GamesPlayed = 0,
                CurrentRank = i++
            });

            Assert.NotNull(chessClubContext);
            Assert.NotNull(chessClubService);

            chessClubContext.Members.AddRange(testMembers);
            chessClubContext.SaveChanges();

            var memberToDelete = chessClubContext.Members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000003"));

            var result = chessClubService.DeleteMember(memberToDelete.Id);

            Assert.AreEqual(true, result);

            var memberCount = chessClubContext.Members.Count();

            Assert.AreEqual(4, memberCount);

            var members = chessClubContext.Members.OrderBy(m => m.Id).ToList();

            Assert.AreEqual(1, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")).CurrentRank);
            Assert.AreEqual(2, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000002")).CurrentRank);
            Assert.AreEqual(3, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")).CurrentRank);
            Assert.AreEqual(4, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000005")).CurrentRank);
        }

        [Test]
        public void TestDeleteLastMember()
        {
            var builder = new DbContextOptionsBuilder<ChessClubContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var chessClubContext = new ChessClubContext(builder.Options);
            var chessClubService = new ChessClubService(NullLogger<ChessClubService>.Instance, chessClubContext);

            int i = 1;

            var testMembers = MemberFaker.Generate(5).Select(m => new Member
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-00000000000{i}"),
                Name = m.Name,
                Surname = m.Surname,
                Email = m.Email,
                Birthday = m.Birthday,
                GamesPlayed = 0,
                CurrentRank = i++
            });

            Assert.NotNull(chessClubContext);
            Assert.NotNull(chessClubService);

            chessClubContext.Members.AddRange(testMembers);
            chessClubContext.SaveChanges();

            var memberToDelete = chessClubContext.Members.OrderBy(m => m.CurrentRank).Last();

            var result = chessClubService.DeleteMember(memberToDelete.Id);

            Assert.AreEqual(true, result);

            var memberCount = chessClubContext.Members.Count();

            Assert.AreEqual(4, memberCount);

            var members = chessClubContext.Members.OrderBy(m => m.Id).ToList();

            Assert.AreEqual(1, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")).CurrentRank);
            Assert.AreEqual(2, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000002")).CurrentRank);
            Assert.AreEqual(3, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000003")).CurrentRank);
            Assert.AreEqual(4, members.First(m => m.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")).CurrentRank);
        }

        [Test]
        public void TestDeleteMemberWithInvalidId()
        {
            var builder = new DbContextOptionsBuilder<ChessClubContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var chessClubContext = new ChessClubContext(builder.Options);
            var chessClubService = new ChessClubService(NullLogger<ChessClubService>.Instance, chessClubContext);

            int i = 1;

            var testMembers = MemberFaker.Generate(5).Select(m => new Member
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-00000000000{i}"),
                Name = m.Name,
                Surname = m.Surname,
                Email = m.Email,
                Birthday = m.Birthday,
                GamesPlayed = 0,
                CurrentRank = i++
            });

            Assert.NotNull(chessClubContext);
            Assert.NotNull(chessClubService);

            chessClubContext.Members.AddRange(testMembers);
            chessClubContext.SaveChanges();



            InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
                () => { var result = chessClubService.DeleteMember(Guid.Empty); });

            Assert.NotNull(ex);

            StringAssert.StartsWith(ex?.Message, "Sequence contains no elements");
        }
    }
}
