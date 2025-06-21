using Microsoft.AspNetCore.Mvc;
using SGHR.Domain.Interfaces;

namespace SGHR.WebApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialController : ControllerBase
    {
        private readonly IHistorialReservaRepository _repo;

        public HistorialController(IHistorialReservaRepository repo)
        {
            _repo = repo;
        }

        //
        [HttpGet]
        public async Task<IActionResult> GetHistorial(
            [FromQuery] int clienteId,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string estado,
            [FromQuery] string tipoHabitacion)
        {
            try
            {
                var historial = await _repo.GetHistorialByClienteAsync(
                    clienteId, fechaInicio, fechaFin, estado, tipoHabitacion
                );

                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener historial: {ex.Message}");
            }
        }

        //
        [HttpGet("detalle/{idReserva}")]
        public async Task<IActionResult> GetDetalleReserva(int idReserva, [FromQuery] int clienteId)
        {
            try
            {
                var detalle = await _repo.GetDetalleReservaAsync(idReserva, clienteId);
                if (detalle == null)
                    return NotFound("Reserva no encontrada.");

                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el detalle: {ex.Message}");
            }
        }
    }

}
