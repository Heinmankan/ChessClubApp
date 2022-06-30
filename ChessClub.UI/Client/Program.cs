using ChessClub.UI.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

namespace ChessClub.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var http = new HttpClient()
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };

            builder.Services.AddScoped(sp => http);
            using var response = await http.GetAsync("appsettings.json");
            using var stream = await response.Content.ReadAsStreamAsync();
            builder.Configuration.AddJsonStream(stream);

            builder.Services.AddHttpClient("ChessClubClient", (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ChessClubClientOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseAddress);
                client.Timeout = new TimeSpan(0, 0, options.TimeoutInSeconds);
            });
            builder.Services.Configure<ChessClubClientOptions>(builder.Configuration.GetSection(ChessClubClientOptions.Position));
            builder.Services.AddScoped<ChessClubClient>();

            await builder.Build().RunAsync();
        }
    }
}
