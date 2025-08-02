using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs.Common;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.Interfaces.Reservas;
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
            try
            {
                var reservaActualizada = await _reservaApplicationService.ActualizarReservaAsync(id, request);
                return Ok(new ApiResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Reservas obtenidas",
                    Data = reservaActualizada
                });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("no encontrada"))
            {
                return NotFound(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = false
                }); ;
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = false
                }); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = false
                }); 
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Error interno del servidor: {ex.Message}",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Cancela una reserva.
        /// </summary>
        [HttpPut("cancelar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelarReserva(int id, [FromBody] CancelarReservaDto request)
        {
            try
            {
                
                var reservaCancelada = await _reservaApplicationService.CancelarReservaAsync(id, request); 
                return Ok(reservaCancelada);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("no encontrada"))
            {
                return NotFound(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                }); ;
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
            return Ok(new ApiResponse<ReservaDto>{
                IsSuccess = true,
                Data = reserva,
                Message = "Reserva obtenida con exito"
            });
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
            return Ok(new ApiResponse<List<ReservaDto>>
            {
                IsSuccess = true,
                Data = reservas,
                Message = $"Reservas del cliente {clienteId}"
            });
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
            return Ok(new ApiResponse<List<ReservaDto>>
            {
                IsSuccess = true,
                Message = "Reservas obtenidas",
                Data = reservas
            });
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
