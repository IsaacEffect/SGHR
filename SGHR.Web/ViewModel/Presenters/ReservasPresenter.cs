using SGHR.Web.ApiServices.Interfaces.Reservas;
using SGHR.Web.ViewModel.Presenters.Interfaces;
using SGHR.Web.ViewModel.Reservas;
using SGHR.Web.Models;

namespace SGHR.Web.ViewModel.Presenters
{
    public class ReservasPresenter(IReservasApiService reservasApiService) : IReservasPresenter
    {
        private readonly IReservasApiService _reservasApiService = reservasApiService;

        public async Task<ApiResponse<ReservasIndexViewModel>> ConstruirIndexViewModelAsync(DateTime? desde, DateTime? hasta)
        {
            var viewModel = new ReservasIndexViewModel();

            try
            {
                ApiResponse<List<ReservasViewModel>> resultado;

                if (desde is not null && hasta is not null)
                {
                    resultado = await _reservasApiService.ObtenerReservasEnRangoAsync(desde.Value, hasta.Value);
                    viewModel.FechaDesde = desde.Value.ToString("dd-MM-yyyy");
                    viewModel.FechaHasta = hasta.Value.ToString("dd-MM-yyyy");
                    viewModel.MostrandoFiltro = true;
                }
                else
                {
                    resultado = await _reservasApiService.ObtenerTodasReservasAsync();
                }

                if (!resultado.IsSuccess)
                {
                    viewModel.MensajeError = resultado.Message ?? "Error al cargar las reservas.";
                    return new ApiResponse<ReservasIndexViewModel>
                    {
                        IsSuccess = false,
                        Message = viewModel.MensajeError,
                        Data = viewModel
                    };

                }

                viewModel.Reservas = resultado.Data ?? [];
            }
            catch (Exception ex)
            {
                viewModel.MensajeError = $"Error inesperado: {ex.Message}";
            }

            return new ApiResponse<ReservasIndexViewModel>
            {
                IsSuccess = true,
                Data = viewModel    
            };

        }
    }
}