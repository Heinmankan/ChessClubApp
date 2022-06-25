﻿using ChessClub.Database;
using ChessClub.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ChessClub.Service.Tests
{
    [TestFixture]
    public partial class ChessClubServiceTests
    {
        [Test]
        [TestCase("10000000-0000-0000-0000-000000000001", "20000000-0000-0000-0000-000000000002", "10000000-0000-0000-0000-000000000001", 1, 2)]
        [TestCase("10000000-0000-0000-0000-000000000001", "20000000-0000-0000-0000-000000000002", "20000000-0000-0000-0000-000000000002", 2, 1)]
        [TestCase("10000000-0000-0000-0000-000000000001", "20000000-0000-0000-0000-000000000002", null, 1, 2)]
        [TestCase("10000000-0000-0000-0000-000000000001", "30000000-0000-0000-0000-000000000003", "10000000-0000-0000-0000-000000000001", 1, 3)]
        [TestCase("10000000-0000-0000-0000-000000000001", "30000000-0000-0000-0000-000000000003", "30000000-0000-0000-0000-000000000003", 3, 2)]
        [TestCase("10000000-0000-0000-0000-000000000001", "30000000-0000-0000-0000-000000000003", null, 1, 2)]
        [TestCase("10000000-0000-0000-0000-000000000001", "40000000-0000-0000-0000-000000000004", "10000000-0000-0000-0000-000000000001", 1, 4)]
        [TestCase("10000000-0000-0000-0000-000000000001", "40000000-0000-0000-0000-000000000004", "40000000-0000-0000-0000-000000000004", 3, 2)]
        [TestCase("10000000-0000-0000-0000-000000000001", "40000000-0000-0000-0000-000000000004", null, 1, 3)]
        [TestCase("10000000-0000-0000-0000-000000000001", "50000000-0000-0000-0000-000000000005", "10000000-0000-0000-0000-000000000001", 1, 5)]
        [TestCase("10000000-0000-0000-0000-000000000001", "50000000-0000-0000-0000-000000000005", "50000000-0000-0000-0000-000000000005", 2, 3)]
        [TestCase("10000000-0000-0000-0000-000000000001", "50000000-0000-0000-0000-000000000005", null, 1, 4)]
        [TestCase("10000000-0000-0000-0000-000000000001", "60000000-0000-0000-0000-000000000006", "10000000-0000-0000-0000-000000000001", 1, 6)]
        [TestCase("10000000-0000-0000-0000-000000000001", "60000000-0000-0000-0000-000000000006", "60000000-0000-0000-0000-000000000006", 2, 3)]
        [TestCase("10000000-0000-0000-0000-000000000001", "60000000-0000-0000-0000-000000000006", null, 1, 5)]
        [TestCase("10000000-0000-0000-0000-000000000001", "70000000-0000-0000-0000-000000000007", "10000000-0000-0000-0000-000000000001", 1, 7)]
        [TestCase("10000000-0000-0000-0000-000000000001", "70000000-0000-0000-0000-000000000007", "70000000-0000-0000-0000-000000000007", 2, 4)]
        [TestCase("10000000-0000-0000-0000-000000000001", "70000000-0000-0000-0000-000000000007", null, 1, 6)]
        [TestCase("10000000-0000-0000-0000-000000000001", "80000000-0000-0000-0000-000000000008", "10000000-0000-0000-0000-000000000001", 1, 8)]
        [TestCase("10000000-0000-0000-0000-000000000001", "80000000-0000-0000-0000-000000000008", "80000000-0000-0000-0000-000000000008", 2, 4)]
        [TestCase("10000000-0000-0000-0000-000000000001", "80000000-0000-0000-0000-000000000008", null, 1, 7)]
        [TestCase("10000000-0000-0000-0000-000000000001", "90000000-0000-0000-0000-000000000009", "10000000-0000-0000-0000-000000000001", 1, 9)]
        [TestCase("10000000-0000-0000-0000-000000000001", "90000000-0000-0000-0000-000000000009", "90000000-0000-0000-0000-000000000009", 2, 5)]
        [TestCase("10000000-0000-0000-0000-000000000001", "90000000-0000-0000-0000-000000000009", null, 1, 8)]

        [TestCase("30000000-0000-0000-0000-000000000003", "40000000-0000-0000-0000-000000000004", "30000000-0000-0000-0000-000000000003", 3, 4)]
        [TestCase("30000000-0000-0000-0000-000000000003", "40000000-0000-0000-0000-000000000004", "40000000-0000-0000-0000-000000000004", 4, 3)]
        [TestCase("30000000-0000-0000-0000-000000000003", "40000000-0000-0000-0000-000000000004", null, 3, 4)]
        [TestCase("30000000-0000-0000-0000-000000000003", "50000000-0000-0000-0000-000000000005", "30000000-0000-0000-0000-000000000003", 3, 5)]
        [TestCase("30000000-0000-0000-0000-000000000003", "50000000-0000-0000-0000-000000000005", "50000000-0000-0000-0000-000000000005", 5, 4)]
        [TestCase("30000000-0000-0000-0000-000000000003", "50000000-0000-0000-0000-000000000005", null, 3, 4)]
        [TestCase("30000000-0000-0000-0000-000000000003", "60000000-0000-0000-0000-000000000006", "30000000-0000-0000-0000-000000000003", 3, 6)]
        [TestCase("30000000-0000-0000-0000-000000000003", "60000000-0000-0000-0000-000000000006", "60000000-0000-0000-0000-000000000006", 5, 4)]
        [TestCase("30000000-0000-0000-0000-000000000003", "60000000-0000-0000-0000-000000000006", null, 3, 5)]

        [TestCase("60000000-0000-0000-0000-000000000006", "90000000-0000-0000-0000-000000000009", "60000000-0000-0000-0000-000000000006", 6, 9)]
        [TestCase("60000000-0000-0000-0000-000000000006", "90000000-0000-0000-0000-000000000009", "90000000-0000-0000-0000-000000000009", 8, 7)]
        [TestCase("60000000-0000-0000-0000-000000000006", "90000000-0000-0000-0000-000000000009", null, 6, 8)]
        [TestCase("70000000-0000-0000-0000-000000000007", "90000000-0000-0000-0000-000000000009", "70000000-0000-0000-0000-000000000007", 7, 9)]
        [TestCase("70000000-0000-0000-0000-000000000007", "90000000-0000-0000-0000-000000000009", "90000000-0000-0000-0000-000000000009", 9, 8)]
        [TestCase("70000000-0000-0000-0000-000000000007", "90000000-0000-0000-0000-000000000009", null, 7, 8)]
        [TestCase("80000000-0000-0000-0000-000000000008", "90000000-0000-0000-0000-000000000009", "80000000-0000-0000-0000-000000000008", 8, 9)]
        [TestCase("80000000-0000-0000-0000-000000000008", "90000000-0000-0000-0000-000000000009", "90000000-0000-0000-0000-000000000009", 9, 8)]
        [TestCase("80000000-0000-0000-0000-000000000008", "90000000-0000-0000-0000-000000000009", null, 8, 9)]
        public async Task TestAddResult(Guid player1Id, Guid player2Id, Guid? winnerId, int player1NewRanking, int player2NewRanking)
        {
            var builder = new DbContextOptionsBuilder<ChessClubContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new ChessClubContext(builder.Options);
            var members = Enumerable.Range(1, 9)
                    .Select(i => new Member
                    {
                        Id = Guid.Parse($"{i}0000000-0000-0000-0000-00000000000{i}"),
                        Name = $"Name{i}",
                        Surname = $"Surname{i}",
                        Email = $"Name{i}@gmail.com",
                        GamesPlayed = 0,
                        CurrentRank = i
                    });
            context.Members.AddRange(members);
            var saveCount = await context.SaveChangesAsync();

            Assert.AreEqual(9, saveCount);

            var dbService = new ChessClubService(NullLogger<ChessClubService>.Instance, context);

            var result = await dbService.AddResult(player1Id, player2Id, winnerId);

            var player1 = context.Members.First(m => m.Id == player1Id);
            var player2 = context.Members.First(m => m.Id == player2Id);

            Assert.AreEqual(9, saveCount);

            Assert.AreEqual(player1NewRanking, player1.CurrentRank, "Player1 ranking not correct");
            Assert.AreEqual(player2NewRanking, player2.CurrentRank, "Player2 ranking not correct");
        }
    }
}