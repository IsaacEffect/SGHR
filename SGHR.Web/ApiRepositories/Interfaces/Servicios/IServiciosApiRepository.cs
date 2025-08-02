using SGHR.Application.DTOs.Servicios;
using SGHR.Web.Models;
using SGHR.Web.ViewModel.Servicios;

namespace SGHR.Web.ApiRepositories.Interfaces.Servicios
{
    public interface IServiciosApiRepository
    {
        Task<ApiResponse<ServiciosViewModel>> AgregarServicioAsync(AgregarServicioRequest dto);
        Task<ApiResponse<bool>> ActualizarServicioAsync(int id, ActualizarServicioRequest dto);
        Task<ApiResponse<bool>> EliminarServicioAsync(int id);
        Task<ApiResponse<ServiciosViewModel>> ObtenerServicioPorIdAsync(int id);
        Task<ApiResponse<List<ServiciosViewModel>>> ObtenerTodosLosServiciosAsync();
        Task<ApiResponse<bool>> ActivarServicioAsync(int id);
        Task<ApiResponse<bool>> DesactivarServicioAsync(int id);
    }
}