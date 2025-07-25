using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.DTOs.Common;

namespace SGHR.WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController(IReservaApplicationService reservaApplicationService) : ControllerBase
    {
        private readonly IReservaApplicationService _reservaApplicationService = reservaApplicationService;

        /// <summary>
        /// Crea una nueva reserva
        /// </summary>
        [HttpPost("CrearReserva")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearReservaAsync([FromBody] CrearReservaRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<ReservaDto> { IsSuccess = false, Message = "Errores de validación: " + string.Join("; ", errors) });
            }

            var reservaDto = await _reservaApplicationService.CrearReservaAsync(request); 
            var apiResponse = new ApiResponse<ReservaDto>
            {
                IsSuccess = true,
                Message = "Reserva creada exitosamente.",
                Data = reservaDto
            };
            return CreatedAtAction(nameof(ObtenerReservaPorId), new { id = reservaDto.Id }, apiResponse);
        }

        /// <summary>
        ///  Actualiza una reserva existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ActualizarReserva(int id, [FromBody] ActualizarReservaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var reservaActualizada = await _reservaApplicationService.ActualizarReservaAsync(id, request);
                return Ok(reservaActualizada);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("no encontrada"))
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancela una reserva.
        /// </summary>
        [HttpPatch("{id}/cancelar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelarReserva([FromBody] CancelarReservaDto request)
        {
            try
            {
                var reservaCancelada = await _reservaApplicationService.CancelarReservaAsync(request); 
                return Ok(reservaCancelada);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("no encontrada"))
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene una reserva por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerReservaPorId(int id)
        {
            var reserva = await _reservaApplicationService.ObtenerReservaPorIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        /// <summary>
        /// Obtiene reservas por el id del cliente
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerReservasPorClienteId(int clienteId)
        {
            var reservas = await _reservaApplicationService.ObtenerReservasPorClienteIdAsync(clienteId);
            if (reservas == null || !reservas.Any())
            {
                return NotFound();
            }
            return Ok(reservas);
        }

        /// <summary>
        /// Obtiene reservas en un rango de fechas
        /// </summary>
        [HttpGet("rango")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ObtenerReservasEnRango([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            if (desde >= hasta)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            }
            var reservas = await _reservaApplicationService.ObtenerReservasEnRangoAsync(desde, hasta);
            if (reservas == null || !reservas.Any())
            {
                return NotFound("No se encontraron reservas en el rango de fechas especificado.");
            }
            return Ok(reservas);
        }

        /// <summary>
        /// Obtiene todas las reservas
        /// </summary>
        [HttpGet("todas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerTodasReservas([FromQuery] bool incluirRelaciones = false)
        {
            var reservas = await _reservaApplicationService.ObtenerTodasReservasAsync(incluirRelaciones);
            if (reservas == null || !reservas.Any())
            {
                return NotFound("No se encontraron reservas.");
            }
            return Ok(new ApiResponse<List<ReservaDto>>
            {
                IsSuccess = true,
                Message = "Reservas obtenidas",
                Data = reservas
            });
        }

    }
}
