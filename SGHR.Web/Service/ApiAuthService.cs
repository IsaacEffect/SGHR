using SGHR.Web.Base.Helpers;
using SGHR.Web.Models;
using SGHR.Web.Service.Contracts;
using System.Text;
using System.Text.Json;

namespace SGHR.Web.Service
{
    public class ApiAuthService : IApiAuthService
    {
        private readonly HttpClient _client;

        public ApiAuthService()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5095/api/") };
        }

        public async Task<string?> AutenticarAsync(AuthModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("Auth/login", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<string>(responseJson);

            return result.Success ? result.Data : null;
        }
    }
}
