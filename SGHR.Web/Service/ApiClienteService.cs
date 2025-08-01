using SGHR.Web.Base.Helpers;
using SGHR.Web.Models;
using SGHR.Web.Service.Contracts;

namespace SGHR.Web.Service
{
    public class ApiClienteService : IApiClienteService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;

        public ApiClienteService(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
            _client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, "http://localhost:5095/api/");
        }

        public async Task<List<ClientesModel>> ObtenerTodosAsync()
        {
            var response = await _client.GetAsync("Clientes/GetAllClients");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<List<ClientesModel>>(json);
            return result.Success ? result.Data : new List<ClientesModel>();
        }

        public async Task<ClientesModel?> ObtenerPorIdAsync(int id)
        {
            var response = await _client.GetAsync($"Clientes/GetClientById?id={id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<ClientesModel>(json);
            return result.Success ? result.Data : null;
        }

        public async Task<bool> InsertarAsync(ClientesModel model)
        {
            var dto = new
            {
                model.nombre,
                model.apellido,
                model.email,
                model.telefono,
                model.direccion,
                ContrasenaHashed = model.contrasena
            };
            var content = ApiHttpClientHelper.CreateJsonContent(dto);
            var response = await _client.PostAsync("Clientes/InsertClient", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<object>(json);
            return response.IsSuccessStatusCode && result.Success;
        }

        public async Task<bool> ActualizarAsync(ActualizarClienteModel model)
        {
            var dto = new
            {
                id = model.idCliente,
                nombre = model.nombre?.Trim(),
                apellido = model.apellido?.Trim(),
                correo = model.email?.Trim().Replace("\n", "").Replace("\r", ""),
                direccion = model.direccion?.Trim(),
                telefono = model.telefono?.Trim()
            };

            var content = ApiHttpClientHelper.CreateJsonContent(dto);
            var response = await _client.PutAsync("Clientes/ModifyClient", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<object>(json);
            return response.IsSuccessStatusCode && result.Success;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var response = await _client.DeleteAsync($"Clientes/DeleteClient?id={id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult(json);
            return response.IsSuccessStatusCode && result.Success;
        }

        public async Task<bool> CambiarContrasenaAsync(int idCliente, string actual, string nueva)
        {
            var dto = new { IdCliente = idCliente, ContrasenaActual = actual, NuevaContrasena = nueva };
            var content = ApiHttpClientHelper.CreateJsonContent(dto);
            var response = await _client.PutAsync("Clientes/ChangePassword", content);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<object>(json);
            return response.IsSuccessStatusCode && result.Success;
        }
    }

}
