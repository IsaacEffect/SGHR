using Microsoft.AspNetCore.Mvc;
using SGHR.Domain.Interfaces.Service;
using SGHR.Model.Dtos;

namespace SGHR.WebApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clientes = await _clienteService.ObtenerTodosAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener clientes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);
                if (cliente == null)
                    return NotFound("Cliente no encontrado.");

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener cliente: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertarClienteDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _clienteService.InsertarAsync(dto);
                return Ok("Cliente creado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear cliente: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ModificarClienteDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _clienteService.ModificarAsync(dto);
                return Ok("Cliente modificado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al modificar cliente: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.EliminarAsync(id);
                return Ok("Cliente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar cliente: {ex.Message}");
            }
        }
    }
}
