using ChessClub.Database.Configuration;
using ChessClub.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ChessClub.Database
{
    public class ChessClubContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public ChessClubContext([NotNull] DbContextOptions<ChessClubContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public DbSet<Member> Members => Set<Member>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory)
                    .EnableSensitiveDataLogging();
            }
        }
    }
}
