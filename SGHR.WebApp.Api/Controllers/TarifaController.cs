using Microsoft.AspNetCore.Mvc;
using SGHR.Application.DTOsTarifa;
using SGHR.Application.InterfacesServices;
using SGHR.Application.ServicesApplication;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarifaController : ControllerBase
    {
        private readonly ITarifaService _service;
        public TarifaController(ITarifaService tarifaService)
        {
            _service = tarifaService;
        }

        // POST api/<TarifaController>
        [HttpPost("DefinirTarifaBase")]
        public async Task<IActionResult> DefinirTarifaBase(int id, [FromBody] DefinirTarifaBaseDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _service.DefinirTarifaAsync(id, dto);
            return Ok("Tarifa base definida correctamente.");
        }

        // PUT api/<TarifaController>/5
        [HttpPut("ActualizarTarifaBase")]
        public async Task<IActionResult> ActualizarTarifaBase(int id, [FromBody] ActualizarTarifaDto dto)
         {
                if (!ModelState.IsValid) return BadRequest();

                await _service.ActualizarTarifaAsync(id, dto);
                return Ok($"Tarifa con ID {id} actualizada correctamente.");
        }

       // DELETE api/<TarifaController>/5
       [HttpPost("DefinirTarifaPorTemporada")]
       public async Task<IActionResult> DefinirTarifaPorTemporada(int id, [FromBody] DefinirTarifaPorTemporadaDto dto)
       { 
                if (!ModelState.IsValid) return BadRequest(ModelState);

                await _service.DefinirTarifaPorTemporadaAsync(id, dto);
                return Ok($"Tarifa para la temporada '{dto.TipoTemporada}' definida correctamente.");
        
       }
    }
}
