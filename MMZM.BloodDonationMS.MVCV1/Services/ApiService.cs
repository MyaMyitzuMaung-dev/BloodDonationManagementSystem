using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MMZM.BloodDonationMS.MVCV1.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public ApiService(HttpClient httpClient, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = _config["ApiSettings:BaseUrl"] ?? "";
            
            // For development, ignore SSL errors if needed (depends on environment)
        }

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        };

        private async Task PrepareClient()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
        {
            try 
            {
                await PrepareClient();
                var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                // Log exception (omitted for brevity)
            }
            return default;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            try 
            {
                await PrepareClient();
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", data);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
                }
                else 
                {
                    // If not 200, try to see if it's a model with error message
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                        return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                // Log ex
            }
            return default;
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            await PrepareClient();
            var json = JsonSerializer.Serialize(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}{endpoint}", data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return default;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            await PrepareClient();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}");
            return response.IsSuccessStatusCode;
        }
    }
}
