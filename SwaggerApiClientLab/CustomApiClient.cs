using System.Net.Http;
using System.Threading.Tasks;

namespace CustomNamespace
{
    public class CustomApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CustomApiClient(string baseUrl, HttpClient httpClient)
        {
            _baseUrl = baseUrl;
            _httpClient = httpClient;
        }

        public async Task<string> GetUserAsync(int userId)
        {
            var response = await _httpClient.GetStringAsync($"{_baseUrl}/api/users/{userId}");
            return response;
        }
    }
}