using Microsoft.AspNetCore.Mvc;
using SGHR.Domain.Interfaces.Service;

namespace SGHR.WebApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialController : ControllerBase
    {
        private readonly IHistorialReservaService _historialService;

        public HistorialController(IHistorialReservaService historialService)
        {
            _historialService = historialService;
        }

        [HttpGet("filtrado")]
        public async Task<IActionResult> GetHistorialFiltrado(
            [FromQuery] int clienteId,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] string estado = null,
            [FromQuery] string tipoHabitacion = null)
        {
            try
            {
                var historial = await _historialService.ObtenerHistorialAsync(
                    clienteId,
                    fechaInicio,
                    fechaFin,
                    estado,
                    tipoHabitacion);

                if (historial == null || !historial.Any())
                    return NotFound("No se encontraron reservas con los filtros aplicados");

                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener historial: {ex.Message}");
            }
        }

        [HttpGet("detalle/{idReserva}/{clienteId}")]
        public async Task<IActionResult> GetDetalleReserva(
            [FromRoute] int idReserva,
            [FromRoute] int clienteId)
        {
            try
            {
                var detalle = await _historialService.ObtenerDetalleAsync(idReserva, clienteId);

                if (detalle == null)
                    return NotFound("Reserva no encontrada o no pertenece al cliente");

                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el detalle: {ex.Message}");
            }
        }
    }

}
