using SGHR.Web.ApiServices.Base;
using SGHR.Web.ApiServices.Interfaces;
using SGHR.Web.ViewModel;
using SGHR.Web.ViewModel.Reservas;

namespace SGHR.Web.ApiServices
{
    public class ReservasApiService : HttpServiceBase, IReservasApiService
    {
        private const string _baseEndpoint = "api/Reservas";

        public ReservasApiService(HttpClient httpClient) : base(httpClient) { }

        public Task<ApiResponse<ReservasViewModel>> CrearReservaAsync(CrearReservaViewModel model)
        {
            return PostAsync<ReservasViewModel>($"{_baseEndpoint}/CrearReserva", model);
        }

        public Task<ApiResponse<bool>> ActualizarReservaAsync(int id, ActualizarReservaViewModel model)
        {
            return PutAsync<bool>($"{_baseEndpoint}/{id}", model);
        }

        public Task<ApiResponse<bool>> CancelarReservaAsync(CancelarReservaViewModel model)
        {
            return PutAsync<bool>($"{_baseEndpoint}/cancelar/{model.Id}", model);
        }

        public Task<ApiResponse<ReservasViewModel>> ObtenerReservaPorIdAsync(int id)
        {
            return GetAsync<ReservasViewModel>($"{_baseEndpoint}/{id}");
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