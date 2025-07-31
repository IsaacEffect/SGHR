using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Dtos;
using SGHR.Domain.Base;
using SGHR.Web.Base.Helpers;
using SGHR.Web.Base.Mappers;
using SGHR.Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace SGHR.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string apiBaseUrl = "http://localhost:5095/api/";

        public ClientesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObtenerIdClienteDesdeToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return 0;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var idClaim = jwtToken?.Claims.FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || c.Type.EndsWith("/nameidentifier"));
            return idClaim != null ? int.Parse(idClaim.Value) : 0;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Auth");

            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var response = await client.GetAsync("Clientes/GetAllClients");
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return View(new List<ClientesModel>());

            var result = JsonSerializer.Deserialize<ObtenerTodosClientesResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(result?.data ?? new List<ClientesModel>());
        }

        public async Task<IActionResult> PerfilCliente()
        {
            int id = ObtenerIdClienteDesdeToken();
            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var response = await client.GetAsync($"Clientes/GetClientById?id={id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ObtenerClienteResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result?.data == null)
                return NotFound();

            return View("PerfilCliente", result.data);
        }

        [HttpGet]
        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Registro(ClientesModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new
            {
                Nombre = model.nombre,
                Apellido = model.apellido,
                Email = model.email,
                Telefono = model.telefono,
                Direccion = model.direccion,
                ContrasenaHashed = model.contrasena,
            };

            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var content = ApiHttpClientHelper.CreateJsonContent(dto);
            var response = await client.PostAsync("Clientes/InsertClient", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonHelper.DeserializeOperationResult<object>(responseString);

            if (response.IsSuccessStatusCode && result.Success)
            {
                return RedirectToAction("Login", "Auth");
            }

            ModelState.AddModelError("", result.Message ?? "Error al registrar el cliente.");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarPerfil()
        {
            int id = ObtenerIdClienteDesdeToken();
            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var response = await client.GetAsync($"Clientes/GetClientById?id={id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ObtenerClienteResponse>(responseString);
            var modelo = ClienteMapper.AModeloActualizar(result?.data);
            return View("ActualizarPerfil", modelo);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPerfil(ActualizarClienteModel model)
        {
            if (!ModelState.IsValid)
                return View("ActualizarPerfil", model);

            var dto = new
            {
                Id = model.idCliente,
                Nombre = model.nombre,
                Apellido = model.apellido,
                Correo = model.email,
                Direccion = model.direccion,
                Telefono = model.telefono
            };

            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var content = ApiHttpClientHelper.CreateJsonContent(dto);
            var response = await client.PutAsync("Clientes/ModifyClient", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonHelper.DeserializeOperationResult<object>(responseString);

            if (response.IsSuccessStatusCode && result != null && result.Success)
                return RedirectToAction("PerfilCliente");

            ModelState.AddModelError("", result?.Message ?? "Error al actualizar el perfil.");

            return View("ActualizarPerfil", model);
        }

        [HttpGet]
        public async Task<IActionResult> EliminarCuenta()
        {
            int id = ObtenerIdClienteDesdeToken();
            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var response = await client.GetAsync($"Clientes/GetClientById?id={id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ObtenerClienteResponse>(responseString);

            return View("EliminarCuenta", result?.data);
        }

        [HttpPost, ActionName("EliminarCuenta")]
        public async Task<IActionResult> EliminarCuentaConfirmada()
        {
            int id = ObtenerIdClienteDesdeToken();
            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var response = await client.DeleteAsync($"Clientes/DeleteClient?id={id}");
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonHelper.DeserializeOperationResult(responseString);
            if (response.IsSuccessStatusCode && result != null && result.Success)
            {
                _httpContextAccessor.HttpContext?.Session.Clear();
                return RedirectToAction("Login", "Auth");
            }

            TempData["Error"] = result?.Message ?? "Error al eliminar la cuenta.";
            return RedirectToAction("EliminarCuenta");
        }

        [HttpGet]
        public IActionResult CambiarContrasena() => View();

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(CambiarContrasenaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int clienteId = ObtenerIdClienteDesdeToken();
            var dto = new
            {
                IdCliente = clienteId,
                ContrasenaActual = model.contrasenaActual,
                NuevaContrasena = model.nuevaContrasena
            };

            using var client = ApiHttpClientHelper.GetClientWithToken(_httpContextAccessor, apiBaseUrl);
            var content = ApiHttpClientHelper.CreateJsonContent(dto);
            var response = await client.PutAsync("Clientes/ChangePassword", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonHelper.DeserializeOperationResult<object>(responseString);
            if (response.IsSuccessStatusCode && result != null && result.Success)
                return RedirectToAction("PerfilCliente");

            ModelState.AddModelError("", result?.Message ?? "No se pudo cambiar la contraseña. Verifica tus datos.");
            return View(model);
        }

        public IActionResult MiPerfil() => RedirectToAction("PerfilCliente");
        public IActionResult OpcionesCliente() => View();
    }

}
