using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using SGHR.Web.Models;
using System.Text;
using System.Xml.Serialization;

namespace SGHR.Web.Controllers
{
    public class CategoriasHabitacionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private List<CategoriaHabitacion>? categorias;
        private Object model;

        public CategoriasHabitacionController(HttpClient httpClient, ILogger<CategoriasHabitacionController> logger
            )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public ActionResult Index()
        {
            
            return View();
        }

        public async Task<IActionResult> GetAllCategoriasHabitacion()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5217/api/");

                    var response = await client.GetAsync("CategoriasHabitacion/GetAllCategoriasHabitacion");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        };

                        categorias = JsonSerializer.Deserialize<List<CategoriaHabitacion>>(responseString, options);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrece: {ex.StackTrace}");
                throw;
            }

            model = categorias.Select(c => new CategoriasHabitacionModel
            {
                Id = c.Id,
                nombre = c.Nombre,
                descripcion = c.Descripcion,
                caracteristicas = c.Caracteristicas,
                tarifaBase = c.TarifaBase,
                estado = c.Estado
            }).ToList();

            return View("GetAllCategoriasHabitacion", model);
        }

        public async Task<IActionResult> CrearCategoriaHabitacion(CategoriaHabitacion categoria)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5217/api/");

                    var json = JsonSerializer.Serialize(categoria);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("CategoriasHabitacion/CrearCategoriaHabitacion", content);

                    if (response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", "Error al crear la categoria");

                    }
                }
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", "Ocurrió un error al crear la categoría.");
            }

            return View("CrearCategoria");
        }


        [HttpGet]
        public async Task<IActionResult> EditarCategoria(int id)
        {
            var respuesta = await _httpClient.GetAsync($"http://localhost:5217/api/CategoriasHabitacion/GetAllCategoriasHabitacion");

            Console.WriteLine(respuesta);

            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo cargar la categoría.";
                return RedirectToAction("GetAllCategoriasHabitacion");
            }

            var json = await respuesta.Content.ReadAsStringAsync();
            var lista = JsonSerializer.Deserialize<List<CategoriaHabitacion>>(json);

            var categoria = lista.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            return View(categoria); 
        }

        [HttpPost]
        public async Task<IActionResult> EditarCategoria(CategoriaHabitacion categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var contenido = new StringContent(JsonSerializer.Serialize(categoria), Encoding.UTF8, "application/json");

            var respuesta = await _httpClient.PutAsync($"http://localhost:5217/api/CategoriasHabitacion/{categoria.Id}/ActualizarCategoria", contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] = "Categoría actualizada correctamente.";
                return RedirectToAction("GetAllCategoriasHabitacion");
            }

            TempData["Error"] = "Error al actualizar la categoría.";
            return View(categoria);
        }

          

        [HttpPost]
        public async Task<IActionResult> ConfirmadoEliminarCategoria(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5217/api/CategoriasHabitacion/{id}/EliminarCategoria");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Categoría eliminada correctamente.";
                return RedirectToAction("GetAllCategoriasHabitacion");
            }

            TempData["Error"] = "No se pudo eliminar la categoría.";
            return RedirectToAction("GetAllCategoriasHabitacion", new { id }); 

            
        }
    }
}
