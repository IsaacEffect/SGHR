using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Clientes;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Context;

namespace SGHR.WebApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clientes = await _unitOfWork.Clients.GetAllAsync();
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
                var cliente = await _unitOfWork.Clients.GetByIdAsync(id);
                if (cliente == null)
                    return NotFound("Cliente no encontrado.");

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener cliente: {ex.Message}");
            }
        }
    }
}
