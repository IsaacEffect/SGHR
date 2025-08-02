using AutoMapper;
using SGHR.Application.DTOs.Reservas;
using SGHR.Web.ApiRepositories.Interfaces.Reservas;
using SGHR.Web.ApiServices.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.ViewModel.Reservas;
namespace SGHR.Web.ApiServices.Reservas
{
    public class ReservasApiService(IReservasApiRepository reservasApiRepository,IMapper mapper) : IReservasApiService
    {
        private readonly IReservasApiRepository _reservasApiRepository = reservasApiRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<bool>> ActualizarReservaAsync(int id, ActualizarReservaViewModel request)
        {
            var dto = _mapper.Map<ActualizarReservaRequest>(request);
            return await _reservasApiRepository.ActualizarReservaAsync(id, dto);
        }
        public async Task<ApiResponse<bool>> CancelarReservaAsync(CancelarReservaViewModel request)
        {
            return await _reservasApiRepository.CancelarReservaAsync(request);

        }

        public async Task<ApiResponse<ReservasViewModel>> CrearReservaAsync(CrearReservaViewModel request)
        {
            if (request.FechaEntrada >= request.FechaSalida)
            {
                return ApiResponse<ReservasViewModel>.Fail("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            var response = await _reservasApiRepository.CrearReservaAsync(request);

            var viewModel = _mapper.Map<ReservasViewModel>(response.Data);
            return ApiResponse<ReservasViewModel>.Success(viewModel, "Reserva creada correctamente.");

        }

        public async Task<ApiResponse<ActualizarReservaViewModel>> ObtenerReservaPorIdAsync(int id)
        {
            var dto = await _reservasApiRepository.ObtenerReservaPorIdAsync(id);
            if (dto?.Data == null)
                return ApiResponse<ActualizarReservaViewModel>.Fail("Reserva no encontrada");

            var viewModel = _mapper.Map<ActualizarReservaViewModel>(dto.Data);   

            return ApiResponse<ActualizarReservaViewModel>.Success(viewModel);
        }

        public async Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasEnRangoAsync(DateTime desde, DateTime hasta)
        {
            if (desde >= hasta)
            {
                return ApiResponse<List<ReservasViewModel>>.Fail("La fecha desde debe ser anterior a la fecha hasta.");
            }
            var response = await _reservasApiRepository.ObtenerReservasEnRangoAsync(desde, hasta);
            var viewModels = _mapper.Map<List<ReservasViewModel>>(response.Data);
            return ApiResponse<List<ReservasViewModel>>.Success(viewModels);
        }

        public async Task<ApiResponse<List<ReservasViewModel>>> ObtenerTodasReservasAsync(bool incluirRelaciones = false)
        {
            var response = await _reservasApiRepository.ObtenerTodasReservasAsync(incluirRelaciones);
            var viewModels = _mapper.Map<List<ReservasViewModel>>(response.Data);
            return ApiResponse<List<ReservasViewModel>>.Success(viewModels);
        }

        public async Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasPorClienteIdAsync(int clienteId)
        {
            return await _reservasApiRepository.ObtenerReservasPorClienteIdAsync(clienteId);
        }

    }
}