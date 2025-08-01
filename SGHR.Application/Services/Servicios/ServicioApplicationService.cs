using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using ServiciosEntity = SGHR.Domain.Entities.Servicios.Servicios;
using SGHR.Domain.Interfaces;
using AutoMapper;

namespace SGHR.Application.Services.Servicios
{
    public class ServicioApplicationService(
        IServicioRepository serviciosRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IServicioRules servicioRules) : IServicioApplicationService
    {
        private readonly IServicioRepository _serviciosRepository = serviciosRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;  
        private readonly IServicioRules _servicioRules = servicioRules;

        public async Task<ServicioDto> AgregarServicioAsync(AgregarServicioRequest request)
        {
            await _servicioRules.ValidarDatosBasicosAsync(request.Nombre, request.Descripcion);
            var nuevoServicio = _mapper.Map<ServiciosEntity>(request);
            await _serviciosRepository.AgregarServicioAsync(nuevoServicio);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ServicioDto>(nuevoServicio);
        }
        public async Task<bool> ActualizarServicioAsync(int id, ActualizarServicioRequest request)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(id);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(id);
            await _servicioRules.ValidarDatosBasicosAsync(request.Nombre, request.Descripcion);

            if (request.Activo)
                servicio.Activar();
            else
                servicio.Desactivar();
            
            servicio.Actualizar
                (
                    request.Nombre,
                    request.Descripcion
                );
            
            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _unitOfWork.CommitAsync();


            return true;
        }

        public async Task EliminarServicioAsync(int idServicio)
        {
            _servicioRules.ValidarIdPositivo(idServicio);
            await _serviciosRepository.EliminarServicioAsync(idServicio);
            await _unitOfWork.CommitAsync();

        }
        public async Task ActivarServicioAsync(int idServicio)
        {
            _servicioRules.ValidarIdPositivo(idServicio);
            await _servicioRules.ValidarExistenciaSerivicioAsync(idServicio);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio);
            servicio.Activar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _unitOfWork.CommitAsync();

        }
        public async Task DesactivarServicioAsync(int idServicio)
        {
            _servicioRules.ValidarIdPositivo(idServicio);
            await _servicioRules.ValidarExistenciaSerivicioAsync(idServicio);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio);
           
            servicio.Desactivar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _unitOfWork.CommitAsync();

        }
        public async Task<ServicioDto?> ObtenerServicioPorIdAsync(int idServicio)
        {
            _servicioRules.ValidarIdPositivo(idServicio);
            await _servicioRules.ValidarExistenciaSerivicioAsync(idServicio);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio);
            return _mapper.Map<ServicioDto>(servicio);
        }
        // Hacer los unit tests de estas funciones 
        public async Task<List<ServicioDto>> ObtenerServiciosActivosAsync()
        {
            var servicios = await _serviciosRepository.ObtenerServiciosActivosAsync();
            return _mapper.Map<List<ServicioDto>>(servicios);
        }
        public async Task<List<ServicioDto>> ObtenerTodosLosServiciosAsync()
        {

            var servicios = await _serviciosRepository.ObtenerTodosLosServiciosAsync();
            return _mapper.Map<List<ServicioDto>>(servicios);

        }
       
    }
}