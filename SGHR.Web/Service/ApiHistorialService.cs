using SGHR.Web.Base.Helpers;
using SGHR.Web.Models;
using SGHR.Web.Service.Contracts;

namespace SGHR.Web.Service
{
    public class ApiHistorialService : IApiHistorialService
    {
        private readonly HttpClient _client;

        public ApiHistorialService(IHttpContextAccessor accessor)
        {
            _client = ApiHttpClientHelper.GetClientWithToken(accessor, "http://localhost:5095/api/");
        }

        public async Task<List<HistorialModel>?> FiltrarHistorialAsync(int clienteId, DateTime? fechaInicio, DateTime? fechaFin, string estado, string tipoHabitacion)
        {
            var url = $"Historial/filtrado?clienteId={clienteId}" +
                      $"{(fechaInicio.HasValue ? $"&fechaInicio={fechaInicio:yyyy-MM-dd}" : "")}" +
                      $"{(fechaFin.HasValue ? $"&fechaFin={fechaFin:yyyy-MM-dd}" : "")}" +
                      $"{(string.IsNullOrEmpty(estado) ? "" : $"&estado={estado}")}" +
                      $"{(string.IsNullOrEmpty(tipoHabitacion) ? "" : $"&tipoHabitacion={tipoHabitacion}")}";

            var response = await _client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<List<HistorialModel>>(json);
            return result.Success ? result.Data : null;
        }

        public async Task<HistorialModel?> ObtenerDetalleReservaAsync(int idReserva, int idCliente)
        {
            var response = await _client.GetAsync($"Historial/detalle/{idReserva}/{idCliente}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<HistorialModel>(json);
            return result.Success ? result.Data : null;
        }
    }
}
