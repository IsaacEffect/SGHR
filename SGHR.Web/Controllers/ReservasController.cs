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
        /// Muestra la lista de todas las reservas con opción de filtrar por rango de fechas
        /// </summary>
        public async Task<IActionResult> Index(DateTime? desde = null, DateTime? hasta = null)
        {
            try
            {
                if (desde.HasValue && hasta.HasValue)
                {
                    if (desde.Value >= hasta.Value)
                    {
                        TempData["ErrorMessage"] = "La fecha desde debe ser anterior a la fecha hasta.";
                        ViewBag.FechaDesde = desde.Value.ToString("yyyy-MM-dd");
                        ViewBag.FechaHasta = hasta.Value.ToString("yyyy-MM-dd");
                        return View(new List<ReservasViewModel>());
                    }

                    var rangeResponse = await _reservasApiService.ObtenerReservasPorRangoFechasAsync(desde.Value, hasta.Value);

                    if (rangeResponse.IsSuccess && rangeResponse.Data != null)
                    {
                        ViewBag.FechaDesde = desde.Value.ToString("yyyy-MM-dd");
                        ViewBag.FechaHasta = hasta.Value.ToString("yyyy-MM-dd");
                        ViewBag.MostrandoFiltro = true;
                        return View(rangeResponse.Data);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = rangeResponse.Message ?? "Error al buscar reservas en el rango especificado";
                        ViewBag.FechaDesde = desde.Value.ToString("yyyy-MM-dd");
                        ViewBag.FechaHasta = hasta.Value.ToString("yyyy-MM-dd");
                        ViewBag.MostrandoFiltro = true;
                        return View(new List<ReservasViewModel>());
                    }
                }

                var apiResponse = await _reservasApiService.ObtenerTodasReservasAsync();

                if (apiResponse.IsSuccess && apiResponse.Data != null)
                {
                    return View(apiResponse.Data);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Error al cargar las reservas");
                    return View(new List<ReservasViewModel>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error inesperado: {ex.Message}";
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
                TempData["ErrorMessage"] = $"Error al crear Reserva: {apiResponse.Message}";
                return View(model);
            }
        }
        // GET: /Reservas/Edit/{id}
        /// <summary>
        ///  Muestar el formulario para editar una reserva existente
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var apiResponse = await _reservasApiService.ObtenerReservaPorIdAsync(id);

            if (!apiResponse.IsSuccess || apiResponse.Data == null)
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "No se pudo cargar la reserva para edición";
                return RedirectToAction(nameof(Index));
            }

            return View(apiResponse.Data);
        }
        // POST: /Reservas/Edit/
        /// <summary>
        ///  Procesa la edicion de una reserva existente enviada desde el formulario
        /// </summary>
        /// <param name="id">Id de la reserva a actualizar</param>
        /// <param name="model">Los datos actualizados</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActualizarReservaViewModel model)
        {
            if (id <= 0)
            {
                return BadRequest("Id de reserva invalido");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var apiResponse = await _reservasApiService.ActualizarReservasAsync(id, model);
            if (apiResponse.IsSuccess && apiResponse.Data != null)
            {
                TempData["SuccessMessage"] = "Reserva actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            else if (!apiResponse.IsSuccess && apiResponse.Message.Contains("no encontrada"))
            {
                ModelState.AddModelError(string.Empty, "Reserva no encontrada: no se puede actualizar");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Error al actualizar la reserva";
                return View(model);
            }
        }

        // GET: Reserva/Cancel/{id}
        /// <summary>
        /// Muestra el Fromulario para cancelar una reserva existente
        /// </summary>
        [HttpGet]
        public IActionResult Cancel(int id)
        {
            return View(new CancelarReservaViewModel { Id = id });
        }
        // POST: Reserva/Cancel/
        /// <summary>
        /// Cancela una reserva existente
        /// </summary>
        /// <param name="id">El Id de la reserva a cancelar</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(CancelarReservaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var response = await _reservasApiService.CancelarReservaAsync(model);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Reserva cancelada correctamente.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", response.Message ?? "Error al cancelar la reserva");
            return View(model);
        }
        // GET: Reserva/cliente/{id}
        /// <summary>
        /// Obtiene una reserva por el id del cliente
        /// </summary>
        /// <param name="id">Id del Cliente</param>
        /// <returns>Una reserva del cliente</returns>
        [HttpGet]
        public async Task<IActionResult> GetByClientId(int id)
        {
            var result = await _reservasApiService.ObtenerReservasPorCliente(id);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound("No se encontro la reserva.");
            }
            return View("ListaPorCliente", result.Data);
        }
    }
}