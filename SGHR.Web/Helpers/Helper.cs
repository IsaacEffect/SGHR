using SGHR.Web.Helpers.Abstraction;

namespace SGHR.Web.Helpers
{
    public class Helper : IHelper
    {
        public async Task<HttpResponseMessage> EjecutarHttpAsync(string baseUrl, Func<HttpClient, Task<HttpResponseMessage>> accion)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            return await accion(client);
        }
    }
}

