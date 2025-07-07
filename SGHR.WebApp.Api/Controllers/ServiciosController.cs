using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs.Servicios;
using SGHR.Application.Interfaces.Servicios;


namespace SGHR.WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly IServicioApplicationService _servicioApplicationService;
        public ServiciosController(IServicioApplicationService servicioApplicationService)
        {
            _servicioApplicationService = servicioApplicationService;
        }

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
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] ActualizarServicioRequest request)
        {
            if (id != request.IdServicio)
            {
                return BadRequest("El ID del servicio no coincide con el ID proporcionado en la solicitud.");
            }
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
        /// Obtener todos los servicios
        /// </summary>
        [HttpGet("ObtenerTodosLosServicios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerTodosLosServicios()
        {
            var servicios = await _servicioApplicationService.ObtenerTodosLosServiciosAsync();
            return Ok(servicios);
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
            await _servicioApplicationService.AsignarPrecioServicioCategoriaAsync(request);
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
            await _servicioApplicationService.EliminarPrecioServicioCategoriaAsync(idServicio, idCategoriaHabitacion);
            return Ok("Se ha eliminado el precio correctamente");
        }

        /// <summary>
        /// Obtener los precios de un servicio por categoría
        /// </summary>
        [HttpGet("ObtenerPreciosServicioPorCategoria/{idCategoriaHabitacion}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPreciosServicioPorCategoria(int idCategoriaHabitacion)
        {
            var precios = await _servicioApplicationService.ObtenerPreciosServicioPorCategoriaAsync(idCategoriaHabitacion);
            return Ok(precios);
        }

        /// <summary>
        /// Obtener precios por servcio
        /// </summary>
        [HttpGet("ObtenerPreciosCategoriaPorServicio/{idServicio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPreciosCategoriaPorServicio(int idServicio)
        {
            var precios = await _servicioApplicationService.ObtenerPreciosCategoriaPorServicioAsync(idServicio);
            return Ok(precios);
        }
        /// <summary>
        /// Obtener un precio específico de un servicio por categoría
        /// </summary>
        [HttpGet("ObtenerPrecioServicioCategoriaEspecifico")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPrecioServicioCategoriaEspecifico([FromQuery]int idServicio, [FromQuery]int idCategoriaHabitacion)
        {
            var precio = await _servicioApplicationService.ObtenerPrecioServicioCategoriaEspecificoAsync(idServicio, idCategoriaHabitacion);
            return precio is null ? NotFound() : Ok(precio);
        }
    }
}
