using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Base.Helpers;
using SGHR.Web.Models;
using SGHR.Web.Service.Contracts;

namespace SGHR.Web.Controllers
{
    public class HistorialController : Controller
    {
        private readonly IApiHistorialService _historialService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HistorialController(IApiHistorialService historialService, IHttpContextAccessor accessor)
        {
            _historialService = historialService;
            _httpContextAccessor = accessor;
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarHistorial(DateTime? fechaInicio, DateTime? fechaFin, string estado, string tipoHabitacion)
        {
            int clienteId = TokenHelper.ObtenerIdClienteDesdeToken(HttpContext);

            var historial = await _historialService.FiltrarHistorialAsync(clienteId, fechaInicio, fechaFin, estado, tipoHabitacion);
            if (historial == null)
            {
                ViewBag.Error = "No se pudo obtener el historial.";
                return View(new List<HistorialModel>());
            }

            return View(historial);
        }

        [HttpGet]
        public async Task<IActionResult> DetalleReserva(int id)
        {
            int clienteId = TokenHelper.ObtenerIdClienteDesdeToken(HttpContext);
            var reserva = await _historialService.ObtenerDetalleReservaAsync(id, clienteId);

            if (reserva == null)
            {
                ViewBag.Error = "No se encontró la reserva o no tienes permiso para verla.";
                return View("ErrorReserva");
            }

            return View(reserva);
        }
    }
}