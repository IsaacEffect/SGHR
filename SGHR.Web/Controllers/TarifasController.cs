using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Helpers.Abstraction;
using SGHR.Web.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SGHR.Web.Controllers
{
    public class TarifasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHelper _helper;
        private TarifaModel _modelo;

        public TarifasController(HttpClient httpClient, IHelper helper) 
        { 
            _httpClient = httpClient;
            _helper = helper;
        }
        // GET: TarifasController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DefinirTarifaBase(int id)
        {

            var response = await _helper.EjecutarHttpAsync("http://localhost:5217/api/",
                     cliente => cliente.GetAsync("CategoriasHabitacion/GetAllCategoriasHabitacion"));

            if (!response.IsSuccessStatusCode)
                    return NotFound();
                
                    var responseString = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };


                var Listacategorias = JsonSerializer.Deserialize<List<CategoriasHabitacionModel>>(responseString, options);

                    var categoria = Listacategorias.FirstOrDefault(c => c.Id == id);

                    if (categoria == null)
                        return NotFound();

                var modelo = new TarifaModel
                {
                    CategoriaId = categoria.Id,
                    TarifaBase = categoria.tarifaBase
                };


                return View("DefinirTarifaBase", modelo);
            
        }

        [HttpPost]
        public async Task<IActionResult> DefinirTarifaBase(TarifaModel model)
        {
                if (!ModelState.IsValid)
                    return View(model);

                var jsonContent = JsonSerializer.Serialize(new
                {
                    precio = model.TarifaBase
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _helper.EjecutarHttpAsync("http://localhost:5217/api/",
               cliente => cliente.PostAsync($"Tarifa/DefinirTarifaBase/{model.CategoriaId}", content));

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Error al actualizar la tarifa base.");
                    return View(model);
                }

                return RedirectToAction("GetAllCategoriasHabitacion", "CategoriasHabitacion");
            


        }

        
    }
}
