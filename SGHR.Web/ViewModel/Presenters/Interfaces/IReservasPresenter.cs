using SGHR.Web.ViewModel.Reservas;
using SGHR.Web.Models;

namespace SGHR.Web.ViewModel.Presenters.Interfaces
{
    public interface IReservasPresenter
    {
        Task<ApiResponse<ReservasIndexViewModel>> ConstruirIndexViewModelAsync(DateTime? desde, DateTime? hasta);
    }
}
