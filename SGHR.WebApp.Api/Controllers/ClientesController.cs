using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Contracts.Service;
using SGHR.Application.Dtos;
using SGHR.WebApp.Api.Extensions;

namespace SGHR.WebApp.Api.Controllers
{
    [Authorize(Roles = "Administrador,Cliente")]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("GetAllClients")]
        public async Task<IActionResult> Get()
        {
            var result = await _clienteService.ObtenerTodosAsync();
            return result.ToActionResult();
        }

        [HttpGet("GetClientById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _clienteService.ObtenerPorIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost("InsertClient")]
        public async Task<IActionResult> Post([FromBody] InsertarClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clienteService.InsertarAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("ModifyClient")]
        public async Task<IActionResult> Put([FromBody] ModificarClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clienteService.ModificarAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasenaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clienteService.CambiarContrasenaAsync(dto);
            return result.ToActionResult();
        }

        [HttpDelete("DeleteClient")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clienteService.EliminarAsync(id);
            return result.ToActionResult();
        }
    }
}
