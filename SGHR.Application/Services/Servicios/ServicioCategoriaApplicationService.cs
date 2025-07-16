using AutoMapper;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
namespace SGHR.Application.Services.Servicios
{
    public class ServicioCategoriaApplicationService
        (
        IServicioCategoriaRepository servicioCategoria,
        IServicioRepository serviciosRepository,
        ICategoriaHabitacionRepository categoriaHabitacionRepository,
        IServicioRules servicioRules,
        IUnitOfWork unitOfWork,
        IMapper mapper
        
        ) : IServicioCategoriaApplicationService
    {
        private readonly IServicioCategoriaRepository _servicioCategoriaRepository = servicioCategoria;
        private readonly IServicioRepository _serviciosRepository = serviciosRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository = categoriaHabitacionRepository;
        private readonly IServicioRules _servicioRules = servicioRules;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


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
        public Task ActualizarPrecioServicioCategoriaAsync(int servicioId, int categoriaId, decimal precio)
        {
            _servicioRules.ValidarIdPositivo(servicioId);
            _servicioRules.ValidarIdPositivo(categoriaId);
            _servicioRules.ValidarPrecioServicio(precio);
            
            _ = _serviciosRepository.ObtenerPorIdAsync(servicioId)
                ?? throw new KeyNotFoundException($"Servicio con ID {servicioId} no encontrado.");
            _ = _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId)
                ?? throw new KeyNotFoundException($"Categoría de habitación con ID {categoriaId} no encontrada.");

            return _servicioCategoriaRepository.ActualizarPrecioServicioCategoriaAsync(servicioId, categoriaId, precio);
        }
        public async Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId)
        {
            _servicioRules.ValidarIdPositivo(servicioId);
            _servicioRules.ValidarIdPositivo(categoriaId);
            await _servicioRules.ValidarExistenciaSerivicioAsync(servicioId);
            _ = await _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId)
                ?? throw new KeyNotFoundException($"Categoría de habitación con ID {categoriaId} no encontrada.");

            await _servicioCategoriaRepository.EliminarPrecioServicioCategoriaAsync(servicioId, categoriaId);

        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId)
        {
            _servicioRules.ValidarIdPositivo(categoriaId);
            _ = await _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId)
                            ?? throw new KeyNotFoundException($"Categoría de habitación con el ID {categoriaId} no encontrada.");

            var precios = await _servicioCategoriaRepository.ObtenerPreciosPorCategoriaAsync(categoriaId);
            return _mapper.Map<List<ServicioCategoriaDto>>(precios);
        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId)
        {
            _servicioRules.ValidarIdPositivo(servicioId);
            await _servicioRules.ValidarExistenciaSerivicioAsync(servicioId);
            var precios = await _servicioCategoriaRepository.ObtenerPreciosPorServicioAsync(servicioId);
            return _mapper.Map<List<ServicioCategoriaDto>>(precios);
        }
        public async Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId)
        {
            _servicioRules.ValidarIdPositivo(servicioId);
            _servicioRules.ValidarIdPositivo(categoriaId);
            await _servicioRules.ValidarExistenciaSerivicioAsync(servicioId);
            _ = await _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId)
                    ?? throw new KeyNotFoundException($"Categoría de habitación con el ID {categoriaId} no encontrada.");
            var precio = await _servicioCategoriaRepository.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId);
            return _mapper.Map<ServicioCategoriaDto>(precio);
        }
     
    }
}
