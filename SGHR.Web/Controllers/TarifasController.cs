using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SGHR.Web.Controllers
{
    public class TarifasController : Controller
    {
        private readonly HttpClient _httpClient;
        private TarifaModel _modelo;

        public TarifasController(HttpClient httpClient) 
        { 
            _httpClient = httpClient;
        }
        // GET: TarifasController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DefinirTarifaBase(int id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5217/api/");

                var response = await client.GetAsync("CategoriasHabitacion/GetAllCategoriasHabitacion");

                if (!response.IsSuccessStatusCode)
                    return NotFound();
                
                    var responseString = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                responseString = responseString.Replace("\"estado\": true", "\"estado\":\"Activo\"");
                responseString = responseString.Replace("\"estado\": false", "\"estado\":\"Inactivo\"");

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

                var response = await _httpClient.PostAsync($"http://localhost:5217/api/Tarifa/DefinirTarifaBase/{model.CategoriaId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Error al actualizar la tarifa base.");
                    return View(model);
                }

                return RedirectToAction("GetAllCategoriasHabitacion", "CategoriasHabitacion");
            


        }

        // POST: TarifasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TarifasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TarifasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TarifasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TarifasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
