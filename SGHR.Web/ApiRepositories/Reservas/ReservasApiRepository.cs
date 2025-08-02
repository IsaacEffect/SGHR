using SGHR.Web.ApiRepositories.Base;
using SGHR.Web.ApiRepositories.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.ViewModel.Reservas;
using SGHR.Application.DTOs.Reservas;
namespace SGHR.Web.ApiRepositories.Reservas
{
    public class ReservasApiRepository(HttpClient httpClient) : HttpServiceBase(httpClient), IReservasApiRepository
    {
        private const string _baseEndpoint = "api/Reservas";

        public Task<ApiResponse<ReservasViewModel>> CrearReservaAsync(CrearReservaViewModel model)
        {
            return PostAsync<ReservasViewModel>($"{_baseEndpoint}/CrearReserva", model);
        }

        public Task<ApiResponse<bool>> ActualizarReservaAsync(int id, ActualizarReservaRequest request)
        {
            return PutAsync<bool>($"{_baseEndpoint}/{id}", request);
        }

        public Task<ApiResponse<bool>> CancelarReservaAsync(CancelarReservaViewModel model)
        {
            return PutAsync<bool>($"{_baseEndpoint}/cancelar/{model.Id}", model);
        }

        public Task<ApiResponse<ReservaDto>> ObtenerReservaPorIdAsync(int id)
        {
            return GetAsync<ReservaDto>($"{_baseEndpoint}/{id}");
        }

        public Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasPorClienteIdAsync(int clienteId)
        {
            return GetListAsync<ReservasViewModel>($"{_baseEndpoint}/cliente/{clienteId}");
        }

        public Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta)
        {
            var endpoint = $"{_baseEndpoint}/rango?desde={desde:O}&hasta={hasta:O}";
            return GetListAsync<ReservasViewModel>(endpoint);
        }

        public Task<ApiResponse<List<ReservasViewModel>>> ObtenerTodasReservasAsync(bool incluirRelaciones)
        {
            var endpoint = $"{_baseEndpoint}/todas?incluirRelaciones={incluirRelaciones.ToString().ToLower()}";
            return GetListAsync<ReservasViewModel>(endpoint);
        }
    }
}