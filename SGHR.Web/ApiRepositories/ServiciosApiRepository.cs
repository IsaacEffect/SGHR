using SGHR.Application.DTOs.Servicios;
using SGHR.Web.ApiRepositories.Base;
using SGHR.Web.ApiRepositories.Interfaces.Servicios;
using SGHR.Web.ViewModel.Servicios;
using AutoMapper;
using SGHR.Web.Models;
namespace SGHR.Web.ApiRepositories
{
    public class ServiciosApiRepository(HttpClient httpClient, IMapper mapper) : HttpServiceBase(httpClient), IServiciosApiRepository
    {
        private const string _baseEndpoint = "api/Servicios";
        private readonly IMapper _mapper = mapper;

        public Task<ApiResponse<ServiciosViewModel>> AgregarServicioAsync(AgregarServicioRequest request)
        {
            var viewModel = _mapper.Map<ServiciosViewModel>(request);
            return PostAsync<ServiciosViewModel>($"{_baseEndpoint}/AgregarServicio", viewModel);
        }

        public Task<ApiResponse<bool>> ActualizarServicioAsync(int id, ActualizarServicioRequest model)
        {
            return PutAsync<bool>($"{_baseEndpoint}/ActualizarServicio/{id}", model);
        }

        public Task<ApiResponse<bool>> EliminarServicioAsync(int id)
        {
            return DeleteAsync($"{_baseEndpoint}/EliminarServicio/{id}");
        }

        public Task<ApiResponse<ServiciosViewModel>> ObtenerServicioPorIdAsync(int id)
        {
            return GetAsync<ServiciosViewModel>($"{_baseEndpoint}/ObtenerServicio/{id}");
        }

        public Task<ApiResponse<List<ServiciosViewModel>>> ObtenerTodosLosServiciosAsync()
        {
            return GetListAsync<ServiciosViewModel>($"{_baseEndpoint}/ObtenerTodosLosServicios");
        }

        public Task<ApiResponse<List<ServiciosViewModel>>> ObtenerServiciosActivosAsync()
        {
            return GetListAsync<ServiciosViewModel>($"{_baseEndpoint}/ObtenerServiciosActivos");
        }
        public Task<ApiResponse<bool>> ActivarServicioAsync(int id)
        {
            return PutAsync<bool>($"{_baseEndpoint}/ActivarServicio/{id}");
        }
        public Task<ApiResponse<bool>> DesactivarServicioAsync(int id)
        {
            return PutAsync<bool>($"{_baseEndpoint}/DesactivarServicio/{id}");
        }
    }
}