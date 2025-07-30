using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.DTOs.Common;
namespace SGHR.WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioCategoriaController(IServicioCategoriaApplicationService servicioCategoriaApplicationService) : Controller
    {
        IServicioCategoriaApplicationService _servicioCategoriaApplicationService = servicioCategoriaApplicationService;



        /// <summary>
        /// Asignar y si existe actualiza un precio a un servicio por categoría
        /// </summary> 
        [HttpPost("AsignarPrecio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AsignarPrecioServicioCategoria([FromBody] AsignarPrecioServicioCategoriaRequest request)
        {
            if (request.IdServicio <= 0 || request.IdCategoriaHabitacion <= 0)
            {
                return BadRequest("Los IDs de servicio y categoria deben ser numeros positivos");
            }
            await _servicioCategoriaApplicationService.AsignarActualizarPrecioServicioCategoriaAsync(request);
            return Ok(new ApiResponse<ServicioDto>
            {
                IsSuccess = true,
                Message = "Precio Asignado exitosamente.",

            });
        }

        /// <summary>
        /// Obtener precios por servcio
        /// </summary>
        [HttpGet("ObtenerPreciosCategoriaPorServicio/{idServicio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPreciosCategoriaPorServicio(int idServicio)
        {
            var precios = await _servicioCategoriaApplicationService.ObtenerPreciosCategoriaPorServicioAsync(idServicio);
            return Ok(new ApiResponse<List<ServicioCategoriaDto>>
            {
                IsSuccess = true,
                Data = precios,
                Message = "Precios por servicio obtenidos correctamente"
            });
        }

        /// <summary>
        /// Obtener un precio específico de un servicio por categoría
        /// </summary>
        [HttpGet("ObtenerPrecioServicioCategoriaEspecifico")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPrecioServicioCategoriaEspecifico([FromQuery] int idServicio, [FromQuery] int idCategoriaHabitacion)
        {
            var precio = await _servicioCategoriaApplicationService.ObtenerPrecioServicioCategoriaEspecificoAsync(idServicio, idCategoriaHabitacion);
            return precio is null ? NotFound() : Ok(precio);
        }

    }
}
