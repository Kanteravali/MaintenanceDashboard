
using System.Text.Json;
using MaintenanceDashboard.Models;

namespace MaintenanceDashboard.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ApiResponseModel>> GetUsersAsync()
        {
            var response =
                await _httpClient.GetAsync("users");

            response.EnsureSuccessStatusCode();

            var json =
                await response.Content.ReadAsStringAsync();

            var users =
                JsonSerializer.Deserialize<List<ApiResponseModel>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return users ?? new List<ApiResponseModel>();
        }
    }
}
