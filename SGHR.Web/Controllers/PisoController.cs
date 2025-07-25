using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Interfaces;
using SGHR.Application.Dtos;

namespace SGHR.Web.Controllers
{
    public class PisoController : Controller
    {
        private readonly IPisoService _pisoService;

        public PisoController(IPisoService pisoService)
        {
            _pisoService = pisoService;
        }

        // Listar todos los pisos
        public async Task<IActionResult> Index()
        {
            var pisos = await _pisoService.GetAllAsync();
            return View(pisos);
        }

        // Mostrar formulario para crear
        public IActionResult Create()
        {
            return View();
        }

        // Guardar piso nuevo
        [HttpPost]
        public async Task<IActionResult> Create(PisoDto piso)
        {
            if (ModelState.IsValid)
            {
                await _pisoService.AddAsync(piso);
                return RedirectToAction(nameof(Index));
            }
            return View(piso);
        }

        // Mostrar formulario de edición
        public async Task<IActionResult> Edit(int id)
        {
            var piso = await _pisoService.GetByIdAsync(id);
            return View(piso);
        }

        // Guardar cambios de edición
        [HttpPost]
        public async Task<IActionResult> Edit(PisoDto piso)
        {
            if (ModelState.IsValid)
            {
                await _pisoService.UpdateAsync(piso);
                return RedirectToAction(nameof(Index));
            }
            return View(piso);
        }

        // Confirmación para eliminar
        public async Task<IActionResult> Delete(int id)
        {
            var piso = await _pisoService.GetByIdAsync(id);
            return View(piso);
        }

        // Acción final para eliminar
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pisoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
