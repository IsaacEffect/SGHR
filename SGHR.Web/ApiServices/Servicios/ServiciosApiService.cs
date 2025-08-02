using AutoMapper;
using SGHR.Application.DTOs.Servicios;
using SGHR.Web.ApiRepositories;
using SGHR.Web.ApiRepositories.Interfaces.Servicios;
using SGHR.Web.ApiServices.Interfaces.Servicios;
using SGHR.Web.Models;
using SGHR.Web.ViewModel.ServicioCategoria;
using SGHR.Web.ViewModel.Servicios;

namespace SGHR.Web.ApiServices.Servicios
{
    public class ServiciosApiService(IMapper mapper, IServiciosApiRepository serviciosApiRepository, IServicioCategoriaApiRepository servicioCategoriaApiRepository) : IServicioApiService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IServiciosApiRepository _serviciosApiRepository = serviciosApiRepository;
        private readonly IServicioCategoriaApiRepository _servicioCategoriaApiRepository = servicioCategoriaApiRepository;

        public async Task<ApiResponse<bool>> ActivarServicioAsync(int id)
        {
            return await _serviciosApiRepository.ActivarServicioAsync(id);
        }
        public async Task<ApiResponse<List<ServiciosViewModel>>> ObtenerTodosLosServiciosAsync()
        {
            var response = await _serviciosApiRepository.ObtenerTodosLosServiciosAsync();

            var viewModels = _mapper.Map<List<ServiciosViewModel>>(response.Data);
            return ApiResponse<List<ServiciosViewModel>>.Success(viewModels, response.Message);
        }

        public async Task<ApiResponse<ServicioDto>> AgregarServicioAsync(CrearServiciosViewModel request)
        {
            var dto = _mapper.Map<AgregarServicioRequest>(request);
            var response = await _serviciosApiRepository.AgregarServicioAsync(dto);

            if (!response.IsSuccess)
                return ApiResponse<ServicioDto>.Fail(response.Message);

            var servicioDto = _mapper.Map<ServicioDto>(response.Data);
            return ApiResponse<ServicioDto>.Success(servicioDto, response.Message);
        }

        public async Task<ApiResponse<bool>> DesactivarServicioAsync(int id)
        {
            return await _serviciosApiRepository.DesactivarServicioAsync(id);
        }

        public async Task<ApiResponse<bool>> EditarServicioAsync(int id, ActualizarServicioRequest request)
        {
            var dto = _mapper.Map<ActualizarServicioRequest>(request);
            return await _serviciosApiRepository.ActualizarServicioAsync(id, dto);
        }

        public async Task<ApiResponse<bool>> EliminarServicioAsync(int id)
        {
            return await _serviciosApiRepository.EliminarServicioAsync(id);
        }

        public async Task<ApiResponse<ServicioConPreciosViewModel>> ObtenerServicioConPreciosAsync(int id)
        {
            var servicioResponse = await _serviciosApiRepository.ObtenerServicioPorIdAsync(id);
            if (!servicioResponse.IsSuccess || servicioResponse.Data == null)
                return ApiResponse<ServicioConPreciosViewModel>.Fail("Servicio no encontrado");

            var preciosResponse = await _servicioCategoriaApiRepository.ObtenerPreciosPorServicioAsync(id);
            var precios = preciosResponse.IsSuccess && preciosResponse.Data != null
                ? preciosResponse.Data
                : new List<ServicioCategoriaViewModel>();

            var viewModel = _mapper.Map<ServicioConPreciosViewModel>(servicioResponse.Data);
            viewModel.PreciosPorCategoria = precios;

            return ApiResponse<ServicioConPreciosViewModel>.Success(viewModel);
        }
        public async Task<ApiResponse<ServiciosViewModel>> ObtenerServicioPorIdAsync(int id)
        {
            var response = await _serviciosApiRepository.ObtenerServicioPorIdAsync(id);

            return response;
        }

        public async Task<ApiResponse<object>> AsignarActualizarPrecioAsync(AsignarPrecioServicioCategoriaViewModel model)
        {
            return await _servicioCategoriaApiRepository.AsignarActualizarPrecioAsync(model);
        }
    }
}
