using SGHR.Application.DTOs.Servicios;
using SGHR.Web.Models;
using SGHR.Web.ViewModel.ServicioCategoria;
using SGHR.Web.ViewModel.Servicios;
namespace SGHR.Web.ApiServices.Interfaces
{
    public interface IServicioApiService
    {
        Task<ApiResponse<ServicioDto>> AgregarServicioAsync(CrearServiciosViewModel request);
        Task<ApiResponse<object>> AsignarActualizarPrecioAsync(AsignarPrecioServicioCategoriaViewModel model);
        Task<ApiResponse<ServicioConPreciosViewModel>> ObtenerServicioConPreciosAsync(int id);
        Task<ApiResponse<List<ServiciosViewModel>>> ObtenerTodosLosServiciosAsync();
        Task<ApiResponse<ServiciosViewModel>> ObtenerServicioPorIdAsync(int id);
        Task<ApiResponse<bool>> ActivarServicioAsync(int id);
        Task<ApiResponse<bool>> DesactivarServicioAsync(int id);
        Task<ApiResponse<bool>> EliminarServicioAsync(int id);
        Task<ApiResponse<bool>> EditarServicioAsync(int id, ActualizarServicioRequest request);
    }
}   