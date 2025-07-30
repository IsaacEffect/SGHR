using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs.Common;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Domain.Entities.Servicios;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarSerivicioAsync([FromBody] AgregarServicioRequest request)
        {
            var servicio = await _servicioApplicationService.AgregarServicioAsync(request) ?? throw new InvalidOperationException("No se pudo agregar el servicio.");
            return Ok(new ApiResponse<ServicioDto>
            {
                IsSuccess = true,
                Message = "Servicio agregado exitosamente.",
                Data = servicio
            });
        }

        /// <summary>
        /// Actualizar un servicio existente
        /// </summary>
        [HttpPut("ActualizarServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] ActualizarServicioRequest request)
        {
            var nuevoServicio = await _servicioApplicationService.ActualizarServicioAsync(id, request);
            return Ok(new ApiResponse<bool>
            {
                IsSuccess = true,
                Message = "Servicio actualizado exitosamente.",
                Data = nuevoServicio
            });
        }

        /// <summary>
        /// Eliminar un servicio
        /// </summary>
        [HttpDelete("EliminarServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID del servicio debe ser un numero positivo");
            }
            await _servicioApplicationService.EliminarServicioAsync(id);
            return Ok(new ApiResponse<ServicioDto>
            {
                IsSuccess = true,
                Message = "Servicio cancelada exitosamente."
            });
        }

    

        /// <summary>
        /// Obtener todos los servicios activos
        /// </summary>
        [HttpGet("ObtenerServiciosActivos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerServiciosActivos()
        {
            var servicios = await _servicioApplicationService.ObtenerServiciosActivosAsync();
            return Ok(new ApiResponse<List<ServicioDto>>
            {
                IsSuccess = true,
                Message = "Servicio obtenido exitosamente.",
                Data = servicios
            });
        }

        ///<summary>
        /// Obtener servicio Por id
        /// </summary>
        [HttpGet("ObtenerServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerServicioPorIdAsync(int id)
        {
            var servicios = await _servicioApplicationService.ObtenerServicioPorIdAsync(id);
            if(servicios == null)
            {
                return NotFound();
            }
            return Ok(new ApiResponse<ServicioDto>
            {
                IsSuccess = true,
                Message = "Servicio obtenido correctamente",
                Data = servicios
            });
        }

        /// <summary>
        /// Obtener todos los servicios
        /// </summary>
        [HttpGet("ObtenerTodosLosServicios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                Message = "Servicio obtenido exitosamente.",
                Data = servicios
            });
        }

        /// <summary>
        /// Activar un servicio
        /// </summary>
        [HttpPut("ActivarServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivarServicio(int id)
        {
            await _servicioApplicationService.ActivarServicioAsync(id);
            return Ok(new ApiResponse<ServicioDto>
            {
                IsSuccess = true,
                Message = "Servicio Activado exitosamente."
            });
        }

        /// <summary>
        ///  Desactivar un servicio
        /// </summary>
        [HttpPut("DesactivarServicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DesactivarServicio(int id)
        {
            await _servicioApplicationService.DesactivarServicioAsync(id);
            return Ok(new ApiResponse<ServicioDto>
            {
                IsSuccess = true,
                Message = "Servicio desactivado exitosamente.",
               
            });
        }

    }
}
