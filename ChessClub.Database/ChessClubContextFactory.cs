using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace ChessClub.Database
{
    public class ChessClubContextFactory : IDesignTimeDbContextFactory<ChessClubContext>
    {
        public ChessClubContextFactory()
        { }

        public ChessClubContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ChessClubContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new ChessClubContext(optionsBuilder.Options, new NullLoggerFactory());
        }
    }
}
