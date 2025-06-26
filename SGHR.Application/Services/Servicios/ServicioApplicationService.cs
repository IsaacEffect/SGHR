using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.Services.Reservas;

using SGHR.Domain.Entities.Habitaciones;
using SGHR.Domain.Entities.Servicios;

using SGHR.Persistence.Interfaces.Repositories;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Servicios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGHR.Application.Services.Servicios
{

    public class ServicioApplicationService : IServicioApplicationService
    {
        private readonly IServicioRepository _serviciosRepository;
        private readonly IServicioCategoriaRepository _servicioCategoriaRepository;
        private readonly ICategoriaHabitacionRepository _categoriaHabitacionRepository; 
        public ServicioApplicationService(IServicioRepository serviciosRepository, IServicioCategoriaRepository servicioCategoriaRepository, ICategoriaHabitacionRepository categoriaHabitacionRepository)
        {
            _serviciosRepository = serviciosRepository;
            _servicioCategoriaRepository = servicioCategoriaRepository;
            _categoriaHabitacionRepository = categoriaHabitacionRepository;
            
        }

        public async Task<ServicioDto> CrearServicioAsync(CrearServicioRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
            {
                throw new ArgumentException("El nombre del servicio es requerido.", nameof(request.Nombre));
            }


            var nuevoServicio = new Domain.Entities.Servicios.Servicios(request.Nombre, request.Descripcion);
            await _serviciosRepository.AgregarServicioAsync(nuevoServicio);

            return new ServicioDto
            {
                IdServicio = nuevoServicio.Id,
                Nombre = nuevoServicio.Nombre,
                Descripcion = nuevoServicio.Descripcion,
                Activo = nuevoServicio.Activo
            };

        }
        public async Task ActualizarServicioAsync(ActualizarServicioRequest request)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(request.IdServicio);
            if (servicio == null)
            {
                throw new KeyNotFoundException($"Servicio con ID {request.IdServicio} no encontrado.");
            }
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ArgumentException("El nombre del servicio es requerido.", nameof(request.Nombre));

            servicio.Actualizar(request.Nombre, request.Descripcion);

            if (request.Activo)
            {
                servicio.Activar();
            }
            else
            {
                servicio.Desactivar();
            }
            await _serviciosRepository.ActualizarServicioAsync(servicio);
        }
        public async Task EliminarServicioAsync(int idServicio)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio) ?? throw new KeyNotFoundException($"Servicio con ID {idServicio} no encontrado.");
           
            await _serviciosRepository.EliminarServicioAsync(idServicio);
        }
        public async Task ActivarServicioAsync(int idServicio)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio) ?? throw new KeyNotFoundException($"Servicio con ID {idServicio} no encontrado.");
            servicio.Activar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
        }
        public async Task DesactivarServicioAsync(int idServicio)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio) ?? throw new KeyNotFoundException($"Servicio con ID {idServicio} no encontrado.");
            servicio.Desactivar();
            await _serviciosRepository.ActualizarServicioAsync(servicio);
        }

        public async Task<ServicioDto?> ObtenerServicioPorIdAsync(int idServicio)
        {
            var servicio = await _serviciosRepository.ObtenerPorIdAsync(idServicio);
            if (servicio == null)
            {
                return null; 
            }
            return new ServicioDto
            {
                IdServicio = servicio.Id,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Activo = servicio.Activo
            };
        }

        public async Task<List<ServicioDto>> ObtenerTodosLosServiciosAsync()
        {
            var servicios = await _serviciosRepository.ObtenerTodosAsync();
            return servicios.Select(s => new ServicioDto
            {
                IdServicio = s.Id,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                Activo = s.Activo
            }).ToList();
        }
        
        
        public async Task AsignarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaRequest request)
        {
            if(request.Precio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor que cero.", nameof(request.Precio));
            }
            var servicioExistente = await _serviciosRepository.ObtenerPorIdAsync(request.IdServicio) != null;
            if (!servicioExistente)
            {
                throw new KeyNotFoundException($"Servicio con ID {request.IdServicio} no encontrado.");
            }
            var categoriaExistente = await _categoriaHabitacionRepository.ObtenerPorIdAsync(request.IdCategoria) != null;
        }
        public async Task EliminarPrecioServicioCategoriaAsync(int servicioId, int categoriaId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosServicioPorCategoriaAsync(int categoriaId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<List<ServicioCategoriaDto>> ObtenerPreciosCategoriaPorServicioAsync(int servicioId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public async Task<ServicioCategoriaDto?> ObtenerPrecioServicioCategoriaEspecificoAsync(int servicioId, int categoriaId)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
    }
}
