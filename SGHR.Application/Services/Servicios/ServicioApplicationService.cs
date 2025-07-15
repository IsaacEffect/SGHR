using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using ServiciosEntity = SGHR.Domain.Entities.Servicios.Servicios;
using SGHR.Domain.Interfaces;
using AutoMapper;

namespace SGHR.Application.Services.Servicios
{
    public class ServicioApplicationService(
        IServicioRepository serviciosRepository,
        IServicioCategoriaRepository servicioCategoriaRepository,
        ICategoriaHabitacionRepository categoriaHabitacionRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IServicioRules servicioRules) : IServicioApplicationService
    {
        private readonly IServicioRepository _serviciosRepository = serviciosRepository;
        private readonly IServicioCategoriaRepository _servicioCategoriaRepository = servicioCategoriaRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository = categoriaHabitacionRepository;
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
        public async Task ActualizarServicioAsync(ActualizarServicioRequest request)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(request.IdServicio);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(request.IdServicio);
            await _servicioRules.ValidarDatosBasicosAsync(request.Nombre, request.Descripcion);

            servicio.Actualizar(request.Nombre, request.Descripcion);   
            if (request.Activo) servicio.Activar(); else servicio.Desactivar();

            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _unitOfWork.CommitAsync();
        }
        public async Task EliminarServicioAsync(int idServicio)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(idServicio);
            await _serviciosRepository.ObtenerPorIdAsync(idServicio);
            await _serviciosRepository.EliminarServicioAsync(idServicio);
            await _unitOfWork.CommitAsync();

        }
        public async Task ActivarServicioAsync(int idServicio)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(idServicio);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio);
            servicio.Activar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _unitOfWork.CommitAsync();

        }
        public async Task DesactivarServicioAsync(int idServicio)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(idServicio);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio);
           
            servicio.Desactivar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _unitOfWork.CommitAsync();

        }
        public async Task<ServicioDto?> ObtenerServicioPorIdAsync(int idServicio)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(idServicio);
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio);
            return _mapper.Map<ServicioDto>(servicio);
        }
        public async Task<List<ServicioDto>> ObtenerTodosLosServiciosAsync()
        {
            var servicios = await _serviciosRepository.ObtenerTodosLosServiciosAsync();
            return _mapper.Map<List<ServicioDto>>(servicios);
        }
        public async Task AsignarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request)
        {
            _servicioRules.ValidarPrecioServicio(request.Precio);
            await _serviciosRepository.ObtenerPorIdAsync(request.IdServicio);
            await _servicioRules.ValidarExistenciaSerivicioAsync(request.IdServicio);
            var categoriaExiste = await _categoriaHabitacionRepository.ObtenerPorIdAsync(request.IdCategoriaHabitacion) != null;
            
            if (!categoriaExiste) throw new KeyNotFoundException($"Categoría de habitación con ID {request.IdCategoriaHabitacion} no encontrada."); 
            await _servicioCategoriaRepository.AgregarPrecioServicioCategoriaAsync( 
                request.IdServicio,
                request.IdCategoriaHabitacion,
                request.Precio
            );
        }
        public async Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(servicioId);
            _ = await _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId) 
                ?? throw new KeyNotFoundException($"Categoría de habitación con ID {categoriaId} no encontrada."); 
            await _servicioCategoriaRepository.EliminarPrecioServicioCategoriaAsync(servicioId, categoriaId); 
          
        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId)
        {
            _ = await _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId)
                            ?? throw new KeyNotFoundException($"Categoría de habitación con el ID {categoriaId} no encontrada."); 

            var precios = await _servicioCategoriaRepository.ObtenerPreciosPorCategoriaAsync(categoriaId); 
            return _mapper.Map<List<ServicioCategoriaDto>>(precios);
        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId)
        {
            await _servicioRules.ValidarExistenciaSerivicioAsync(servicioId);
            var precios = await _servicioCategoriaRepository.ObtenerPreciosPorServicioAsync(servicioId); 
            return _mapper.Map<List<ServicioCategoriaDto>>(precios);
        }
        public async Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId)
        {
            _ = await _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId) 
                    ?? throw new KeyNotFoundException($"Categoría de habitación con el ID {categoriaId} no encontrada."); 
            var precio = await _servicioCategoriaRepository.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId); 
            return _mapper.Map<ServicioCategoriaDto>(precio);
        }
    }
}