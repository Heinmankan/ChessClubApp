using ChessClub.Database.Configuration;
using ChessClub.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ChessClub.Database
{
    public class ChessClubContext : DbContext
    {
        public ChessClubContext([NotNull] DbContextOptions<ChessClubContext> options)
            : base(options)
        { }

        public DbSet<Member> Members => Set<Member>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
        }
    }
}
