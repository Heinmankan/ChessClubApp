using ChessClub.API.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ChessClub.UI.Client
{
    public class ChessClubClient
    {
        private readonly ILogger<ChessClubClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly ChessClubClientOptions _options;

        public ChessClubClient(ILogger<ChessClubClient> logger, IHttpClientFactory httpClientFactory, IOptions<ChessClubClientOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClientFactory.CreateClient("ChessClubClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _logger.LogTrace("Started: {name}", nameof(ChessClubClient));
        }

        public async Task<GetMembersResponse?> GetMembersAsync(int pageNumer = 0, int pageSize = 0) =>
            await _httpClient.GetFromJsonAsync<GetMembersResponse>($"{_options.GetMembersUri}?pageNumer={pageNumer}&pageSize={pageSize}");

        public async Task<MemberDTO?> GetMemberByIdAsync(Guid id) =>
            await _httpClient.GetFromJsonAsync<MemberDTO>($"{_options.GetMemberByIdUri}/{id}");

        public async Task<AddMemberResponse?> AddMemberAsync(AddMemberRequest request) =>
            await PostAsync<AddMemberResponse, AddMemberRequest>(request, _options.AddMemberUri);

        public async Task<bool?> UpdateMemberAsync(UpdateMemberRequest request) =>
            await PostAsync<bool?, UpdateMemberRequest>(request, _options.UpdateMemberUri);

        public async Task<bool?> DeleteMemberAsync(DeleteMemberRequest request) =>
            await PostAsync<bool?, DeleteMemberRequest>(request, _options.DeleteMemberUri);

        public async Task<AddResultResponse?> AddResultAsync(AddResultRequest request) =>
            await PostAsync<AddResultResponse, AddResultRequest>(request, _options.AddResultUri);

        private async Task<TResponse?> PostAsync<TResponse, TRequest>(TRequest request, string uri)
        {
            try
            {
                _logger.LogTrace("Method called: {method}", nameof(PostAsync));

                var jsonData = JsonSerializer.Serialize(request);

                var requestData = new StringContent(
                    jsonData,
                    Encoding.UTF8,
                    Application.Json);

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, Application.Json)
                };

                var httpResponse = await _httpClient.SendAsync(httpRequest);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<TResponse>(responseContent);
                }

                _logger.LogError("Unexpected error ({uri}):\n{json}", uri, JsonSerializer.Serialize(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception: {message}", ex.Message);
            }

            return default;
        }
    }
}
