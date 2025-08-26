using System.Text;
using System.Text.Json;

namespace SGHR.Infrastructure.Http;
public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions Opt = new() { PropertyNameCaseInsensitive = true };
    public ApiClient(HttpClient http) => _http = http;

    public async Task<T?> GetAsync<T>(string uri)
    {
        using var res = await _http.GetAsync(uri);
        if (!res.IsSuccessStatusCode) return default;
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, Opt);
    }
    public async Task<TOut?> PostAsync<TIn, TOut>(string uri, TIn body)
    {
        var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        using var res = await _http.PostAsync(uri, bodyJson);
        if (!res.IsSuccessStatusCode) return default;
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TOut>(json, Opt);
    }
    public async Task<bool> PutAsync<TIn>(string uri, TIn body)
    {
        var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        using var res = await _http.PutAsync(uri, bodyJson);
        return res.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(string uri)
    {
        using var res = await _http.DeleteAsync(uri);
        return res.IsSuccessStatusCode;
    }
}
