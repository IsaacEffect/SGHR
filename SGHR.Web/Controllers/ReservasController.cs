using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Services;
using SGHR.Web.ViewModel.Reservas;
namespace SGHR.Web.Controllers
{
    public class ReservasController(ReservasApiService reservasApiService) : Controller
    {
        private readonly ReservasApiService _reservasApiService = reservasApiService;



        // GET: /Reservas
        /// <summary>
        /// Muestra la lista de todas las reservas
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var apiResponse = await _reservasApiService.ObtenerTodasReservasAsync();

            if(apiResponse.IsSuccess && apiResponse.Data != null)
            {
                return View(apiResponse.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Error al cargar las  reservas");
                return View(new List<ReservasViewModel>()); 
            }
        }

        // GET: /Reservas/Create
        /// <summary>
        /// Muestra el formulario para crear una nueva reserva
        /// </summary>
        public ActionResult Create()
        {
            return View(new CrearReservaViewModel());
        }

        // POST: /Reservas/Create
        /// <summary>
        /// Procesa la creacion de una nueva reserva enviada desde el formulario
        /// </summary>
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearReservaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiResponse = await _reservasApiService.CrearReservaAsync(model);
            if (apiResponse.IsSuccess && apiResponse.Data != null)
            {
                TempData["SuccessMessage"] = "Reserva creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Error al crear la reserva");
                return View(model);
            }
        }

        // GET: /Reservas/Edit/5
        /// <summary>
        ///  Muestar el formulario para editar una reserva existente
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var apiResponse = await _reservasApiService.ObtenerReservaPorIdAsync(id);
            if(apiResponse.IsSuccess && apiResponse.Data != null)
            {
                var model = new ActualizarReservaViewModel
                {
                    IdCliente = apiResponse.Data.IdCliente.ToString(),
                    IdCategoriaHabitacion = apiResponse.Data.IdCategoriaHabitacion.ToString(),
                    FechaEntrada = apiResponse.Data.FechaEntrada,
                    FechaSalida = apiResponse.Data.FechaSalida,
                    Estado = apiResponse.Data.Estado,
                    NumeroHuespedes = apiResponse.Data.NumeroHuespedes
                };
                return View(model);
            }
            else if(!apiResponse.IsSuccess && apiResponse.Message.Contains("no encontrada"))
            {
                TempData["ErrorMessage"] = "Reserva no encontrada";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "Error al cargar la reserva para edicion";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Reservas/Edit/5
        /// <summary>
        ///  Procesa la edicion de una reserva existente enviada desde el formulario
        /// </summary>
        /// <param name="id">Id de la reserva a actualizar</param>
        /// <param name="model">Los datos actualizados</param>
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActualizarReservaViewModel model)
        {
            if(id<= 0)
            {
                return BadRequest("Id de reserva invalido");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiResponse = await _reservasApiService.ActualizarReservasAsync(id, model);
            if(apiResponse.IsSuccess && apiResponse.Data != null)
            {
                TempData["SuccessMessage"] = "Reserva actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            else if(!apiResponse.IsSuccess && apiResponse.Message.Contains("no encontrada"))
            {
                ModelState.AddModelError(string.Empty, "Reserva no encontrada: no se puede actualizar");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Error al actualizar la reserva");
                return View(model);
            }
        }

       
        // GET: Reserva/Delete/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            
            return View(new CancelarReservaViewModel());
        }
        
        // POST: Reserva/Delete/5
        /// <summary>
        /// Cancela una reserva existente
        /// </summary>
        /// <param name="id">El Id de la reserva a cancelar</param>
        /// <returns></returns>
        [HttpPut, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CancelarReservaViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _reservasApiService.CancelarReservaAsync(model);

            return View(response);

        }
    }
}
