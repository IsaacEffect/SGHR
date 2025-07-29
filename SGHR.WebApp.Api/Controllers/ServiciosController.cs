using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs.Common;
using SGHR.Application.DTOs.Reservas;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Domain.Entities.Reservas;

namespace SGHR.WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController(IServicioApplicationService servicioApplicationService) : ControllerBase
    {
        private readonly IServicioApplicationService _servicioApplicationService = servicioApplicationService;

        /// <summary>
        /// Agregar un nuevo servicio
        /// </summary>
        [HttpPost("AgregarServicio")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AgregarSerivicioAsync([FromBody] AgregarServicioRequest request)
        {
            var servicio = await _servicioApplicationService.AgregarServicioAsync(request) ?? throw new InvalidOperationException("No se pudo agregar el servicio.");
            return CreatedAtAction(nameof(ObtenerServicioPorId), new { id = servicio.IdServicio }, servicio);
        }

        /// <summary>
        /// Actualizar un servicio existente
        /// </summary>
        [HttpPut("ActualizarServicio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ActualizarServicio([FromBody] ActualizarServicioRequest request)
        {
            await _servicioApplicationService.ActualizarServicioAsync(request);
            return Ok("El servicio se actualizo correctamente");
        }

        /// <summary>
        /// Eliminar un servicio
        /// </summary>
        [HttpDelete("EliminarServicio/{id}")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID del servicio debe ser un numero positivo");
            }
            await _servicioApplicationService.EliminarServicioAsync(id);
            return Ok("El servicio se elimino correctamente");
        }

        /// <summary>
        /// Obtener un servicio por ID
        /// </summary>
        [HttpGet("ObtenerServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerServicioPorId(int id)
        {
            var servicio = await _servicioApplicationService.ObtenerServicioPorIdAsync(id);
            return servicio is null ? NotFound() : Ok(servicio);
        }

        /// <summary>
        /// Obtener todos los servicios activos
        /// </summary>
        [HttpGet("ObtenerServiciosActivos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerServiciosActivos()
        {
            var servicios = await _servicioApplicationService.ObtenerServiciosActivosAsync();
            return Ok(servicios);
        }

        /// <summary>
        /// Obtener todos los servicios
        /// </summary>
        [HttpGet("ObtenerTodosLosServicios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerTodosLosServicios()
        {
            var servicios = await _servicioApplicationService.ObtenerTodosLosServiciosAsync();
            if (servicios == null || !servicios.Any())
            {
                return NotFound("No se encontraron servicios.");
            }
            return Ok(new ApiResponse<List<ServicioDto>>
            {
                IsSuccess = true,
                Message = "Reserva creada exitosamente.",
                Data = servicios
            });
        }

        /// <summary>
        /// Activar un servicio
        /// </summary>
        [HttpPut("ActivarServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivarServicio(int id)
        {
            await _servicioApplicationService.ActivarServicioAsync(id);
            return Ok("Servicio Activado con exito.");
        }

        /// <summary>
        ///  Desactivar un servicio
        /// </summary>
        [HttpPut("DesactivarServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DesactivarServicio(int id)
        {
            await _servicioApplicationService.DesactivarServicioAsync(id);
            return Ok("Servicio Desactivado con exito.");
        }

    }
}
