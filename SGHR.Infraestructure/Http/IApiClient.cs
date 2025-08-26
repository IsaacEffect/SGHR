namespace SGHR.Infrastructure.Http;
public interface IApiClient
{
    Task<T?> GetAsync<T>(string uri);
    Task<TOut?> PostAsync<TIn, TOut>(string uri, TIn body);
    Task<bool> PutAsync<TIn>(string uri, TIn body);
    Task<bool> DeleteAsync(string uri);
}
