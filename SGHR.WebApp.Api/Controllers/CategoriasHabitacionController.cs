using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOs;
using SGHR.Domain.InterfacesServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasHabitacionController : ControllerBase
    {
        private readonly ICategoriasHabitacionesService _services;
        public CategoriasHabitacionController(ICategoriasHabitacionesService categoriasHabitacionesService) 
        { 
            _services = categoriasHabitacionesService;
        }
        // GET: api/<ValuesController>
        [HttpGet("GetAllCategoriasHabitacion")]
        public async Task<ActionResult<IEnumerable<ObtenerTodoCategoriaDto>>> Get()
        {
            var categoria = await _services.ObtenerTodasCategoriasAsync();

            if (categoria == null) { return NotFound("No se encontraron las categorias"); }

            return Ok(categoria);
        }

       

        // POST api/<ValuesController>
        [HttpPost("CrearCategoriaHabitacion")]
        public async Task<ActionResult<CrearCategoriaDto>> Post([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            await _services.CrearCategoriaAsync(crearCategoriaDto);
            return Ok("Categoria guardada correctamente");
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}/ActualizarCategoria")]
        public async Task<ActionResult<ActualizarCategoriaDto>> Put(int id, [FromBody] ActualizarCategoriaDto actualizarCategoriaDto)
        {
            var categoriaPorID = await _services.ObtenerPorIdAsync(id);

            if (categoriaPorID == null)
            {
                return NotFound(actualizarCategoriaDto);
            }

            await _services.ActualizarCategoriaAsync(id, actualizarCategoriaDto);
            return Ok("Se actualizo la categoria exitosamente");
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}/EliminarCategoria")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoriaID = await _services.ObtenerPorIdAsync(id);

            if(categoriaID is null)
            { 
                return NotFound();
            }

           await _services.EliminarCategoriaAsync(categoriaID.IdCategoriaHabitacion);
            return Ok("Categoria eliminada");
        }
    }
}
