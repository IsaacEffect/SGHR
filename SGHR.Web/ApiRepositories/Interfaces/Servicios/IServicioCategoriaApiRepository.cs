using SGHR.Web.Models;
using SGHR.Web.ViewModel.ServicioCategoria;

namespace SGHR.Web.ApiRepositories.Interfaces.Servicios
{
    public interface IServicioCategoriaApiRepository
    {
        Task<ApiResponse<object>> AsignarActualizarPrecioAsync(AsignarPrecioServicioCategoriaViewModel model);
        Task<ApiResponse<List<ServicioCategoriaViewModel>>> ObtenerPreciosPorServicioAsync(int idServicio);
    }
}