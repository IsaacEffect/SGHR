using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SGHR.Web.Base.Helpers
{
    public static class ApiHttpClientHelper
    {
        public static HttpClient GetClientWithToken(IHttpContextAccessor httpContextAccessor, string baseUrl)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            var token = httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public static StringContent CreateJsonContent<T>(T model)
        {
            var json = JsonSerializer.Serialize(model);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
