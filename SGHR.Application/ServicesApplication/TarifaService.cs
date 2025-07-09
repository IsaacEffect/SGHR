using Microsoft.Extensions.Logging;
using SGHR.Application.DTOs;
using SGHR.Application.DTOsTarifa;
using SGHR.Application.InterfacesServices;
using SGHR.Domain.InterfacesRepositories;
using SGHR.Domain.InterfacesServices;
using SGHR.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.ServicesApplication
{
    public sealed class TarifaService : ITarifaService
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly ILogger<TarifaService> _logger;
        private readonly ICategoriasHabitacionesService _categoriasHabitacionesService;

        public TarifaService(ITarifaRepository tarifaRepository, 
                            ILogger<TarifaService> logger,
                            ICategoriasHabitacionesService categoriasHabitacionesService)
        {
            _tarifaRepository = tarifaRepository;
            _logger = logger;
            _categoriasHabitacionesService = categoriasHabitacionesService;
        }


        public async Task ActualizarTarifaAsync(int id, ActualizarTarifaDto dto)
        {
            try
            {
                var tarifa = await _tarifaRepository.ObtenerTarifaPorID(id);

                if (tarifa == null)
                {
                    _logger.LogError($"No se encontró la categoría con ID: {id}.");
                    throw new Exception("No se encontró la categoria.");
                }

                tarifa.Precio = dto.Precio;
                await _tarifaRepository.ActualizarTarifaAsync(tarifa);

                _logger.LogInformation($"Tarifa base actualizada correctamente para la Tarifa con ID:{id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la tarifa base de la Tarifa con ID:{id}.");
                throw;
            }
        }

        public async Task DefinirTarifaAsync(int idCategoria, DefinirTarifaBaseDto dto)
        {
            try
            {
                _logger.LogInformation("Iniciando proceso para definir tarifa a la categoría con ID {IdCategoria}", idCategoria);

                var categoria = await _categoriasHabitacionesService.ObtenerPorIdAsync(idCategoria);

                if (categoria == null)
                {
                    _logger.LogError("No se encontró la categoría con ID {IdCategoria} al intentar definir una tarifa", idCategoria);
                    throw new Exception("Categoría no encontrada");
                }

                var tarifa = new Tarifa
                {
                    IdCategoriaHabitacion = idCategoria,
                    Precio = dto.Precio,
                    Estado = true
                };

                await _tarifaRepository.DefinirTarifaAsync(tarifa);

                categoria.TarifaBase = dto.Precio;
                await _categoriasHabitacionesService.GuardarCambiosAsync();

                _logger.LogInformation("Tarifa definida exitosamente para la categoría con ID {IdCategoria}", idCategoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al definir tarifa para la categoría con ID {IdCategoria}", idCategoria);
                throw;
            }
        }

        public async Task DefinirTarifaPorTemporadaAsync(int id, DefinirTarifaPorTemporadaDto dto)
        {
            try
            {
                var tarifa = new Tarifa
                {
                    FechaFin = dto.FechaFin,
                    FechaInicio = dto.FechaInicio,
                    TipoTemporada = dto.TipoTemporada,
                    Precio = dto.Precio,
                    IdCategoriaHabitacion = id


                };

                await _tarifaRepository.DefinirTarifaPorTemporadaAsync(tarifa);
                _logger.LogInformation($"Tarifa por temporada '{dto.TipoTemporada}' definida para categoría {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al definir tarifa por temporada.");
                throw;
            }
        }
    }
}
