using Microsoft.AspNetCore.Mvc;
using SGHR.Web.ApiServices.Interfaces.Reservas;
using SGHR.Web.Validations;
using SGHR.Web.ViewModel.Reservas;
using SGHR.Web.ViewModel.Presenters.Interfaces;


namespace SGHR.Web.Controllers
{
    public class ReservasController(IReservasApiService reservasApiService, IReservasPresenter reservasPresenter) : Controller
    {
        private readonly IReservasApiService _reservasApiService = reservasApiService;
        private readonly IReservasPresenter _reservasPresenter = reservasPresenter;

        public async Task<IActionResult> Index(DateTime? desde = null, DateTime? hasta = null)
        {
            var response = await _reservasPresenter.ConstruirIndexViewModelAsync(desde, hasta);
            ControllerActionHelper.ProcesarApiResponseConMensajesDinamicos(this, response, desde, hasta);
            return View(response.Data);
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
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Reserva creada correctamente", $"Error al crear Reservas {apiResponse.Message}");
            return RedirectToAction(nameof(Index));
        }
        // GET: /Reservas/Edit/{id}
        /// <summary>
        ///  Muestar el formulario para editar una reserva existente
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var apiResponse = await _reservasApiService.ObtenerReservaPorIdAsync(id);
            if (apiResponse.Data is null)
                return RedirectToAction(nameof(Index));
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, $"Error al cargar la vista: {apiResponse.Message}");

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
        public async Task<IActionResult> Edit(ActualizarReservaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var apiResponse = await _reservasApiService.ActualizarReservaAsync(model.Id, model);
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Reserva actualizada con exito", $"Error al actualizar reserva {apiResponse.Message}");
            return RedirectToAction(nameof(Index));
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
            ControllerActionHelper.ProcesarApiResponse(this, response, "Reserva cancelada correctamente.", $"Error al cancelar la reserva {response.Message}");
            return RedirectToAction(nameof(Index));
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
            var response = await _reservasApiService.ObtenerReservasPorClienteIdAsync(id);
            ControllerActionHelper.ProcesarApiResponse(this, response, $"Reservas del cliente", $"Error al cargar las reservas del cliente: {response.Message}");

            return View("ListaPorCliente", response.Data);
        }
    }
}