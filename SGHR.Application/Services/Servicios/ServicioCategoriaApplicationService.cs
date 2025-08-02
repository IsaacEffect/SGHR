using AutoMapper;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
namespace SGHR.Application.Services.Servicios
{
    public class ServicioCategoriaApplicationService
        (
        IServicioCategoriaRepository servicioCategoria,
        ICategoriaHabitacionRepository categoriaHabitacionRepository,
        IServicioRules servicioRules,
        IUnitOfWork unitOfWork,
        IMapper mapper
        
        ) : IServicioCategoriaApplicationService
    {
        private readonly IServicioCategoriaRepository _servicioCategoriaRepository = servicioCategoria;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository = categoriaHabitacionRepository;
        private readonly IServicioRules _servicioRules = servicioRules;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task AsignarActualizarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request)
        {
            _servicioRules.ValidarPrecioServicio(request.Precio);
            await _servicioRules.ValidarExistenciaSerivicioAsync(request.IdServicio);
            // await ValidarExistenciaCategoriaAsync(request.IdCategoriaHabitacion); // implementar validacion luego
 
            await _servicioCategoriaRepository.AgregarActualizarPrecioServicioCategoriaAsync(request.IdServicio, request.IdCategoriaHabitacion, request.Precio);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId)
        {
            _servicioRules.ValidarIdPositivo(servicioId);
            await _servicioRules.ValidarExistenciaSerivicioAsync(servicioId);
            var precios = await _servicioCategoriaRepository.ObtenerPreciosPorServicioAsync(servicioId);
            return _mapper.Map<List<ServicioCategoriaDto>>(precios);
        }
   
     
    }
}
