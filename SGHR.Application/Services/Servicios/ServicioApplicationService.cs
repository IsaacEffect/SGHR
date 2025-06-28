using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using SGHR.Persistence.Context;
using ServiciosEntity = SGHR.Domain.Entities.Servicios.Servicios;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using AutoMapper;

namespace SGHR.Application.Services.Servicios
{
    public class ServicioApplicationService : IServicioApplicationService
    {
        private readonly IServicioRepository _serviciosRepository;
        private readonly IServicioCategoriaRepository _servicioCategoriaRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository;
        private readonly IMapper _mapper;
        private readonly SGHRDbContext _dbContext; 

        public ServicioApplicationService(
            IServicioRepository serviciosRepository,
            IServicioCategoriaRepository servicioCategoriaRepository,
            ICategoriaHabitacionRepository categoriaHabitacionRepository,
            IMapper mapper,
            SGHRDbContext dbContext)
        {
            _serviciosRepository = serviciosRepository;
            _servicioCategoriaRepository = servicioCategoriaRepository;
            _categoriaHabitacionRepository = categoriaHabitacionRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        
        public async Task<ServicioDto> AgregarServicioAsync(AgregarServicioRequest request)
        {
            var nuevoServicio = _mapper.Map<ServiciosEntity>(request);
            await _serviciosRepository.AgregarServicioAsync(nuevoServicio);
            await _dbContext.SaveChangesAsync(); 
            return _mapper.Map<ServicioDto>(nuevoServicio);
        }

        public async Task ActualizarServicioAsync(ActualizarServicioRequest request)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(request.IdServicio);
            if (servicio == null) throw new KeyNotFoundException($"Servicio con ID {request.IdServicio} no encontrado.");
            if (string.IsNullOrWhiteSpace(request.Nombre)) throw new ArgumentException("El nombre del servicio es requerido.", nameof(request.Nombre));

            servicio.Actualizar(request.Nombre, request.Descripcion);
            if (request.Activo) servicio.Activar(); else servicio.Desactivar();

            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _dbContext.SaveChangesAsync(); 
        }

        public async Task EliminarServicioAsync(int idServicio)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio)
                           ?? throw new KeyNotFoundException($"Servicio con ID {idServicio} no encontrado.");
            await _serviciosRepository.EliminarServicioAsync(idServicio);
            await _dbContext.SaveChangesAsync(); 
        }

        public async Task ActivarServicioAsync(int idServicio)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio)
                           ?? throw new KeyNotFoundException($"Servicio con ID {idServicio} no encontrado.");
            servicio.Activar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _dbContext.SaveChangesAsync(); 
        }

        public async Task DesactivarServicioAsync(int idServicio)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio)
                           ?? throw new KeyNotFoundException($"Servicio con ID {idServicio} no encontrado.");
            servicio.Desactivar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
            await _dbContext.SaveChangesAsync(); 
        }

        public async Task<ServicioDto?> ObtenerServicioPorIdAsync(int idServicio)
        {
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
            if (request.Precio <= 0) throw new ArgumentException("El precio debe ser mayor que cero.", nameof(request.Precio));

            var servicioExistente = await _serviciosRepository.ObtenerPorIdAsync(request.IdServicio) != null;
            if (!servicioExistente) throw new KeyNotFoundException($"Servicio con ID {request.IdServicio} no encontrado.");

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
            await _servicioCategoriaRepository.EliminarPrecioServicioCategoriaAsync(servicioId, categoriaId); 
          
        }

        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId)
        {
            var categoria = await _categoriaHabitacionRepository.ObtenerPorIdAsync(categoriaId)
                            ?? throw new KeyNotFoundException($"Categoría de habitación con el ID {categoriaId} no encontrada.");

            var precios = await _servicioCategoriaRepository.ObtenerPreciosPorCategoriaAsync(categoriaId); 
            return _mapper.Map<List<ServicioCategoriaDto>>(precios);
        }

        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(servicioId)
                           ?? throw new KeyNotFoundException($"Servicio con ID {servicioId} no encontrado.");

            var precios = await _servicioCategoriaRepository.ObtenerPreciosPorServicioAsync(servicioId); 
            return _mapper.Map<List<ServicioCategoriaDto>>(precios);
        }

        public async Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId)
        {
            var precio = await _servicioCategoriaRepository.ObtenerPrecioServicioCategoriaEspecificoAsync(servicioId, categoriaId); 
            return _mapper.Map<ServicioCategoriaDto>(precio);
        }
    }
}