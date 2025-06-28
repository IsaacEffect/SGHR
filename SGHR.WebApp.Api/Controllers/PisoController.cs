using Microsoft.AspNetCore.Mvc;
using SGHR.Persistence.Domain;
using SGHR.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGHR.WebApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PisoController : ControllerBase
    {
        private readonly IPisoRepository _pisoRepository;

        public PisoController(IPisoRepository pisoRepository)
        {
            _pisoRepository = pisoRepository;
        }

        // GET: api/Piso
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pisos = await _pisoRepository.GetAllAsync();
            return Ok(pisos);
        }

        // GET: api/Piso/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var piso = await _pisoRepository.GetByIdAsync(id);
            if (piso == null)
                return NotFound();
            return Ok(piso);
        }

        // POST: api/Piso
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Piso piso)
        {
            if (piso == null)
                return BadRequest();

            await _pisoRepository.AddAsync(piso);
            return CreatedAtAction(nameof(Get), new { id = piso.Id }, piso);
        }

        // PUT: api/Piso/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Piso piso)
        {
            if (id != piso.Id)
                return BadRequest("El ID del piso no coincide.");

            var existente = await _pisoRepository.GetByIdAsync(id);
            if (existente == null)
                return NotFound();

            await _pisoRepository.UpdateAsync(piso);
            return NoContent();
        }

        // DELETE: api/Piso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var piso = await _pisoRepository.GetByIdAsync(id);
            if (piso == null)
                return NotFound();

            await _pisoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
