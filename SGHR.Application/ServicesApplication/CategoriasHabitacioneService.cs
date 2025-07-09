using Microsoft.Extensions.Logging;
using SGHR.Application.DTOs;
using SGHR.Domain.InterfacesDomainServices;
using SGHR.Domain.InterfacesRepositories;
using SGHR.Domain.InterfacesServices;
using SGHR.Persistance.Models;

namespace SGHR.Application.ServicesApplication
{
    public sealed class CategoriasHabitacioneService : ICategoriasHabitacionesService
    {
        private readonly ICategoriasHabitacionRepository _categoriasHabitacionRepository;
        private readonly ILogger _logger;
        private readonly ICategoriasHabitacionesDomainService _categoriasHabitacionesDomain;
        public CategoriasHabitacioneService(ICategoriasHabitacionRepository categoriasHabitacionRepository,
                                            ILogger<CategoriasHabitacioneService> logger,
                                            ICategoriasHabitacionesDomainService categoriasHabitacionesDomainService)
        {
            _categoriasHabitacionRepository = categoriasHabitacionRepository;
            _logger = logger;
            _categoriasHabitacionesDomain = categoriasHabitacionesDomainService;
        }

        

        public async Task ActualizarCategoriaAsync(int? id, ActualizarCategoriaDto actualizarCategoriaDto)
        {
            try
            {
                var categoria = await _categoriasHabitacionRepository.ObtenerPorIdAsync(id);
                if (categoria == null)
                {
                    _logger.LogWarning($"No se encontró la categoría con ID {id}");
                    return;
                }

                categoria.Nombre = actualizarCategoriaDto.Nombre;
                categoria.Descripcion = actualizarCategoriaDto.Descripcion;
                categoria.Caracteristicas = actualizarCategoriaDto.Caracteristicas;
                categoria.Estado = actualizarCategoriaDto.Estado;

                await _categoriasHabitacionRepository.ActualizarCategoriaAsync(categoria);
                await _categoriasHabitacionRepository.GuardarCambiosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la categoría con ID {id}");
                throw;
            }
        }

        public async Task EliminarCategoriaAsync(int id)
        {
            try
            {
                var categoria = await _categoriasHabitacionRepository.ObtenerPorIdAsync(id);

                if (categoria == null)
                {
                    _logger.LogError($"No se encontró la categoría con ID {id} para eliminar.");
                    return;
                }

                await _categoriasHabitacionesDomain.NoEliminarCategoriasConHabitacionesAsignadas(categoria.IdCategoriaHabitacion);
                await _categoriasHabitacionRepository.EliminarCategoriaAsync(categoria);

                _logger.LogInformation($"Categoría con ID {id} eliminada correctamente.");
            }
            catch (Exception) 
            {
                _logger.LogError("No se pudo eliminar la categoria correctamente");
            }
            
        }

        public async Task<CategoriasHabitacion?> ObtenerPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando la búsqueda de la categoría con ID: {Id}", id);

                var categoria = await _categoriasHabitacionRepository.ObtenerPorIdAsync(id);

                if (categoria == null)
                {
                    _logger.LogError("No se encontró ninguna categoría con ID: {Id}", id);
                    return null;
                }

                _logger.LogInformation("Categoría con ID: {Id} encontrada correctamente", id);
                return categoria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar obtener la categoría con ID: {Id}", id);
                return null;
            }
        }

        public async Task<IEnumerable<ObtenerTodoCategoriaDto>> ObtenerTodasCategoriasAsync()
        {
            try
            {
                _logger.LogInformation("Se obtuvieron todas las categorias");
                var categorias = await _categoriasHabitacionRepository.ObtenerTodasCategoriasAsync();

                var resultado = categorias.Select(c => new ObtenerTodoCategoriaDto
                {
                    Id = c.IdCategoriaHabitacion,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    TarifaBase = c.TarifaBase ?? 0,
                    Caracteristicas = c.Caracteristicas,
                    Estado = c.Estado
                });

             
                return resultado;

            }
            catch(Exception) 
            {
                _logger.LogError("No se pudo devolver todas las categorias");
                return Enumerable.Empty<ObtenerTodoCategoriaDto>();
            }

        }

        public async Task<CategoriasHabitacion> CrearCategoriaAsync(CrearCategoriaDto crearCategoriaDto)
        {
            try
            {
                if (crearCategoriaDto == null)
                {
                    throw new ArgumentNullException(nameof(crearCategoriaDto));
                }

                if (await _categoriasHabitacionRepository.ExisteCategoriaPorNombre(crearCategoriaDto.Nombre))
                {
                    throw new Exception("Ya existe una categoria con ese nombre");
                }

                var categoria = new CategoriasHabitacion(
                    crearCategoriaDto.Nombre,
                    crearCategoriaDto.Descripcion,
                    crearCategoriaDto.Caracteristicas
                );

                await _categoriasHabitacionRepository.CrearCategoriaAsync(categoria);
                await _categoriasHabitacionRepository.GuardarCambiosAsync();

                _logger.LogInformation("Se creó la categoría con ID: {Id}", categoria.IdCategoriaHabitacion);
                return categoria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se guardó correctamente la categoría");
                throw; 
            }
        }

        public async Task GuardarCambiosAsync()
        {
            await _categoriasHabitacionRepository.GuardarCambiosAsync();
        }
    }
}
