using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models;
using SGHR.Web.Base.Helpers;
using System.Text.Json;

namespace SGHR.Web.Controllers
{
    public class HistorialController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string apiBaseUrl = "http://localhost:5095/api/";

        public HistorialController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObtenerIdClienteDesdeToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return 0;

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
            var idClaim = jwtToken?.Claims.FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || c.Type.EndsWith("/nameidentifier"));
            return idClaim != null ? int.Parse(idClaim.Value) : 0;
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarHistorial(DateTime? fechaInicio, DateTime? fechaFin, string estado, string tipoHabitacion)
        {
            int clienteId = ObtenerIdClienteDesdeToken();

            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var url = $"Historial/filtrado?clienteId={clienteId}" +
                      $"{(fechaInicio.HasValue ? $"&fechaInicio={fechaInicio:yyyy-MM-dd}" : "")}" +
                      $"{(fechaFin.HasValue ? $"&fechaFin={fechaFin:yyyy-MM-dd}" : "")}" +
                      $"{(string.IsNullOrEmpty(estado) ? "" : $"&estado={estado}")}" +
                      $"{(string.IsNullOrEmpty(tipoHabitacion) ? "" : $"&tipoHabitacion={tipoHabitacion}")}";

            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.DeserializeOperationResult<List<HistorialModel>>(responseString);

            if (!response.IsSuccessStatusCode || !result.Success)
            {
                ViewBag.Error = result.Message ?? "No se pudo obtener el historial.";
                return View(new List<HistorialModel>());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> DetalleReserva(int id)
        {
            int clienteId = ObtenerIdClienteDesdeToken();

            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var response = await client.GetAsync($"Historial/detalle/{id}/{clienteId}");
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonHelper.DeserializeOperationResult<HistorialModel>(responseString);
            Console.WriteLine($"Reserva ID: {id}, Cliente ID desde token: {clienteId}");

            if (!response.IsSuccessStatusCode || !result.Success || result.Data == null)
            {
                var mensaje = result?.Message ?? "No se encontró la reserva o no tienes permiso para verla.";
                ViewBag.Error = mensaje;
                return View("ErrorReserva"); // Crea una vista personalizada para errores si no tienes una
            }


            return View(result.Data);
        }
    }
}
