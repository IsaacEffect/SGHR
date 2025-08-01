using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Base.Helpers;
using SGHR.Web.Base.Mappers;
using SGHR.Web.Models;
using SGHR.Web.Service.Contracts;

namespace SGHR.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IApiClienteService _clienteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientesController(IApiClienteService clienteService, IHttpContextAccessor accessor)
        {
            _clienteService = clienteService;
            _httpContextAccessor = accessor;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("JWToken") == null)
                return RedirectToAction("Login", "Auth");

            var clientes = await _clienteService.ObtenerTodosAsync();
            return View(clientes);
        }

        public async Task<IActionResult> PerfilCliente()
        {
            int id = TokenHelper.ObtenerIdClienteDesdeToken(HttpContext);
            var cliente = await _clienteService.ObtenerPorIdAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpGet]
        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Registro(ClientesModel model)
        {
            if (!ModelState.IsValid) return View(model);

            bool registrado = await _clienteService.InsertarAsync(model);
            if (registrado)
                return RedirectToAction("Login", "Auth");

            ModelState.AddModelError("", "Error al registrar el cliente.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarPerfil()
        {
            int id = TokenHelper.ObtenerIdClienteDesdeToken(HttpContext);
            var cliente = await _clienteService.ObtenerPorIdAsync(id);
            Console.WriteLine($"Cliente ID recibido del servicio: {cliente.idCliente}");

            if (cliente == null) return NotFound();

            var model = ClienteMapper.AModeloActualizar(cliente);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPerfil(ActualizarClienteModel model)
        {
            if (!ModelState.IsValid) return View(model);

            Console.WriteLine($"ID Cliente enviado al servicio: {model.idCliente}");

            var actualizado = await _clienteService.ActualizarAsync(model);
            if (actualizado)
                return RedirectToAction("PerfilCliente");

            ModelState.AddModelError("", "Error al actualizar el perfil.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EliminarCuenta()
        {
            int id = TokenHelper.ObtenerIdClienteDesdeToken(HttpContext);
            var cliente = await _clienteService.ObtenerPorIdAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("EliminarCuenta")]
        public async Task<IActionResult> EliminarCuentaConfirmada()
        {
            int id = TokenHelper.ObtenerIdClienteDesdeToken(HttpContext);
            bool eliminado = await _clienteService.EliminarAsync(id);
            if (eliminado)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Auth");
            }

            TempData["Error"] = "Error al eliminar la cuenta.";
            return RedirectToAction("EliminarCuenta");
        }

        [HttpGet]
        public IActionResult CambiarContrasena() => View();

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(CambiarContrasenaModel model)
        {
            if (!ModelState.IsValid) return View(model);

            int clienteId = TokenHelper.ObtenerIdClienteDesdeToken(HttpContext);
            bool cambioExitoso = await _clienteService.CambiarContrasenaAsync(clienteId, model.contrasenaActual, model.nuevaContrasena);

            if (cambioExitoso)
                return RedirectToAction("PerfilCliente");

            ModelState.AddModelError("", "No se pudo cambiar la contraseña. Verifica tus datos.");
            return View(model);
        }

        public IActionResult MiPerfil() => RedirectToAction("PerfilCliente");
        public IActionResult OpcionesCliente() => View();
    }
}
