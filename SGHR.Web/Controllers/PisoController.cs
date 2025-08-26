using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Dtos;
using SGHR.Application.Dtos;
using SGHR.Application.Interfaces;
using System.Threading.Tasks;

namespace SGHR.Web.Controllers
{
    public class PisoController : Controller
    {
        private readonly IPisoService _pisoService;

        public PisoController(IPisoService pisoService)
        {
            _pisoService = pisoService;
        }

        // GET: /Piso
        public async Task<IActionResult> Index()
        {
            var pisos = await _pisoService.GetAllAsync();
            return View(pisos);
        }

        // GET: /Piso/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var piso = await _pisoService.GetByIdAsync(id);
            if (piso == null) return NotFound();
            return View(piso);
        }

        // GET: /Piso/Create
        public IActionResult Create()
        {
            return View(new SavePisoDto());
        }

        // POST: /Piso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePisoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _pisoService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Piso/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _pisoService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new UpdatePisoDto
            {
                Id = dto.Id,
                NumeroPiso = dto.NumeroPiso,
                Descripcion = dto.Descripcion
            };

            return View(vm);
        }

        // POST: /Piso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePisoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _pisoService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Piso/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var piso = await _pisoService.GetByIdAsync(id);
            if (piso == null) return NotFound();
            return View(piso);
        }

        // POST: /Piso/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pisoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
