using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;
namespace SGHR.WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioCategoriaController(IServicioCategoriaApplicationService servicioCategoriaApplicationService) : Controller
    {
        IServicioCategoriaApplicationService _servicioCategoriaApplicationService = servicioCategoriaApplicationService;



        /// <summary>
        /// Asignar un precio a un servicio por categoría
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
            await _servicioCategoriaApplicationService.AsignarPrecioServicioCategoriaAsync(request);
            return Ok("Precio asignado correctamente");
        }
        /// <summary>
        /// Eliminar un precio asignado a un servicio por categoría
        /// </summary> 
        [HttpDelete("EliminarPrecio/{idServicio}/{idCategoriaHabitacion}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarPrecioServicioCategoria(int idServicio, int idCategoriaHabitacion)
        {
            await _servicioCategoriaApplicationService.EliminarPrecioServicioCategoriaAsync(idServicio, idCategoriaHabitacion);
            return Ok("Se ha eliminado el precio correctamente");
        }
        // Revisar porque si trae las columnas pero sin los datos, sin embargo en la db si se guardan los datos
        /// <summary>
        /// Obtener los precios de un servicio por categoría
        /// </summary>
        [HttpGet("ObtenerPreciosServicioPorCategoria/{idCategoriaHabitacion}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPreciosServicioPorCategoria(int idCategoriaHabitacion)
        {
            var precios = await _servicioCategoriaApplicationService.ObtenerPreciosServicioPorCategoriaAsync(idCategoriaHabitacion);
            return Ok(precios);
        }
        // Revisar 
        /// <summary>
        /// Obtener precios por servcio
        /// </summary>
        [HttpGet("ObtenerPreciosCategoriaPorServicio/{idServicio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPreciosCategoriaPorServicio(int idServicio)
        {
            var precios = await _servicioCategoriaApplicationService.ObtenerPreciosCategoriaPorServicioAsync(idServicio);
            return Ok(precios);
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
