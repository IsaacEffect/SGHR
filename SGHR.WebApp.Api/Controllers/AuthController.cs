using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SGHR.Application.Contracts.Service;
using SGHR.Application.Dtos;
using SGHR.Domain.Base;
using SGHR.WebApp.Api.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGHR.WebApp.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IClienteService clienteService, JwtSettings jwtSettings)
        {
            _clienteService = clienteService;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clienteService.ValidarCredencialesAsync(dto.Email, dto.Contrasena);
            if (!result.Success)
                return Unauthorized(result.Message);

            var token = GenerarToken(result.Data);

            return Ok(new OperationResult<string>
            {
                Success = true,
                Message = "Login exitoso",
                Data = token
            });

        }

        private string GenerarToken(ObtenerClienteDto cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Rol))
                throw new Exception("Rol no definido para el cliente.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim(ClaimTypes.Role, cliente.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
