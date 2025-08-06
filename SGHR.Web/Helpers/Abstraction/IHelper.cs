namespace SGHR.Web.Helpers.Abstraction
{
    public interface IHelper
    {
        Task<HttpResponseMessage> EjecutarHttpAsync(string baseUrl, Func<HttpClient, Task<HttpResponseMessage>> accion );
    }
}
