using Microsoft.Extensions.Logging;
using SGHR.Application.DTOsTarifa;
using SGHR.Application.InterfacesServices;
using SGHR.Domain.InterfacesRepositories;
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

        public TarifaService(ITarifaRepository tarifaRepository, ILogger<TarifaService> logger)
        {
            _tarifaRepository = tarifaRepository;
            _logger = logger;
        }

        public async Task ActualizarTarifaAsync(ActualizarTarifaDto dto)
        {
            try
            {
                var tarifa = new Tarifa
                {
                    IdCategoriaHabitacion = dto.IdCategoriaHabitacion,
                    TipoTemporada = dto.TipoTemporada,
                    Precio = dto.Precio,
                   
                };

                await _tarifaRepository.ActualizarTarifaAsync(tarifa);
                _logger.LogInformation($"Tarifa fue actualizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar tarifa.");
                throw;
            }
        }

        public async Task DefinirTarifaAsync(DefinirTarifaBaseDto dto)
        {
            try
            {
                var tarifa = new Tarifa
                {
                    IdCategoriaHabitacion = dto.IdCategoriaHabitacion,
                    Precio = dto.Precio,
                    
                };

                await _tarifaRepository.DefinirTarifaAsync(tarifa);
                _logger.LogInformation($"Tarifa base definida para categoría {dto.IdCategoriaHabitacion}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al definir tarifa base.");
                throw;
            }
        }

        public async Task DefinirTarifaPorTemporadaAsync(DefinirTarifaPorTemporadaDto dto)
        {
            try
            {
                var tarifa = new Tarifa
                {
                    IdCategoriaHabitacion = dto.IdCategoriaHabitacion,
                    TipoTemporada = dto.TipoTemporada,
                    Precio = dto.Precio,
                   
                };

                await _tarifaRepository.DefinirTarifaPorTemporadaAsync(tarifa);
                _logger.LogInformation($"Tarifa por temporada '{dto.TipoTemporada}' definida para categoría {dto.IdCategoriaHabitacion}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al definir tarifa por temporada.");
                throw;
            }
        }
    }
}
