using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace SGHR.Web.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5095/api/");

                    var content = new StringContent(
                        JsonSerializer.Serialize(model),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await client.PostAsync("Auth/Login", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var loginResult = JsonSerializer.Deserialize<Login>(responseString, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        // Obtiene el rol
                        var handler = new JwtSecurityTokenHandler();
                        var tokenData = handler.ReadJwtToken(loginResult.token);
                        var roles = tokenData.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

                        // Guarda el token en la sesion
                        HttpContext.Session.SetString("JWToken", loginResult.token);

                        // Redirige al area protegida
                        if (roles.Contains("Administrador"))
                            return RedirectToAction("Index", "Clientes");
                        else if (roles.Contains("Cliente"))
                            return RedirectToAction("OpcionesCliente", "Clientes");
                        else
                            return RedirectToAction("Login");

                        

                    }
                    else
                    {
                        TempData["Error"] = "Credenciales incorrectas.";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error inesperado: " + ex.Message;
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login");
        }
    }
}
