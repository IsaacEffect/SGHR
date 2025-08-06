using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using SGHR.Web.Models;
using System.Text;
using SGHR.Web.Helpers.Abstraction;

namespace SGHR.Web.Controllers
{
    public class CategoriasHabitacionController : Controller
    {
        private readonly IHelper _helper;
        private List<CategoriaHabitacion>? categorias;
        

        public CategoriasHabitacionController(IHelper helper )
        { 
            _helper = helper;
        }

        public ActionResult Index()
        {
            
            return View();
        }

        public async Task<IActionResult> GetAllCategoriasHabitacion()
        {
            try
            {
                var response = await _helper.EjecutarHttpAsync("http://localhost:5217/api/",
                    cliente => cliente.GetAsync("CategoriasHabitacion/GetAllCategoriasHabitacion"));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                      

                        categorias = JsonSerializer.Deserialize<List<CategoriaHabitacion>>(responseString);

                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrece: {ex.StackTrace}");
                throw;
            }

            var model = categorias.Select(c => new CategoriasHabitacionModel
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
                var json = JsonSerializer.Serialize(categoria);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _helper.EjecutarHttpAsync("http://localhost:5217/api/", cliente => cliente.PostAsync("CategoriasHabitacion/CrearCategoriaHabitacion", content));

                if (response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", "Error al crear la categoria");
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
            
            var response = await _helper.EjecutarHttpAsync("http://localhost:5217/api/",
                      cliente => cliente.GetAsync("CategoriasHabitacion/GetAllCategoriasHabitacion"));

                 if (!response.IsSuccessStatusCode)
                 {
                      TempData["Error"] = "No se pudo cargar la categoría.";
                    return RedirectToAction("GetAllCategoriasHabitacion");
                 }

            var json = await response.Content.ReadAsStringAsync();
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

            var response = await _helper.EjecutarHttpAsync("http://localhost:5217/api/",
                    cliente => cliente.PutAsync($"CategoriasHabitacion/{categoria.Id}/ActualizarCategoria", contenido));
      

            if (response.IsSuccessStatusCode)
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
            var response = await _helper.EjecutarHttpAsync("http://localhost:5217/api/",
                    cliente => cliente.DeleteAsync($"CategoriasHabitacion/{id}/EliminarCategoria"));

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
