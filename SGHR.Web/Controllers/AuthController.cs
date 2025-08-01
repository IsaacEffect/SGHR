using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models;
using SGHR.Web.Service.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SGHR.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiAuthService _authService;

        public AuthController(IApiAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var token = await _authService.AutenticarAsync(model);

            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Credenciales incorrectas.";
                return View(model);
            }

            HttpContext.Session.SetString("JWToken", token);

            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(token);
            var roles = tokenData.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (roles.Contains("Administrador"))
                return RedirectToAction("Index", "Clientes");
            else if (roles.Contains("Cliente"))
                return RedirectToAction("OpcionesCliente", "Clientes");
            else
                return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login");
        }
    }
}
