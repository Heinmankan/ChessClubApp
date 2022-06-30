using ChessClub.Database.Helpers;
using ChessClub.Service;
using Serilog;

namespace ChessClub.API
{
    public class Program
    {
        private static IConfiguration? _configuration;

        private static IConfiguration Configuration
        {
            get
            {
                _configuration ??= LoadConfiguration();
                return _configuration;
            }
        }

        private static string CurrentEnvironment =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;

        public static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .Enrich.FromLogContext()
              .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            // Add services to the container.
            builder.Services.RegisterDataService(builder.Configuration);
            builder.Services.AddTransient<IChessClubService, ChessClubService>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors(cors => cors
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                );

            app.Run();
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{CurrentEnvironment}.json", true);

            if (CurrentEnvironment.Equals(Environments.Development, StringComparison.InvariantCultureIgnoreCase))
            {
                builder.AddUserSecrets<Program>();
            }

            return builder.Build();
        }
    }
}