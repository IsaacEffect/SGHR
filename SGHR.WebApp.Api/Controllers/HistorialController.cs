using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Contracts.Service;
using SGHR.WebApp.Api.Extensions;

namespace SGHR.WebApp.Api.Controllers
{
    [Authorize(Roles = "Cliente")]
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
            var result = await _historialService.ObtenerHistorialAsync(
                clienteId, fechaInicio, fechaFin, estado, tipoHabitacion);

            return result.ToActionResult();
        }

        [HttpGet("detalle/{idReserva}/{clienteId}")]
        public async Task<IActionResult> GetDetalleReserva(
            [FromRoute] int idReserva,
            [FromRoute] int clienteId)
        {
            var result = await _historialService.ObtenerDetalleAsync(idReserva, clienteId);
            return result.ToActionResult();
        }
    }
}
