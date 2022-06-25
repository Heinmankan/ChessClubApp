using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChessClub.Database.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChessClubContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
