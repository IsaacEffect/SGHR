using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SGHR.Application.DTOs.Servicios;
using SGHR.Web.ApiRepositories.Interfaces.Servicios;
using SGHR.Web.ApiServices.Interfaces.Servicios;
using SGHR.Web.Validations;
using SGHR.Web.ViewModel.ServicioCategoria;
using SGHR.Web.ViewModel.Servicios;

namespace SGHR.Web.Controllers
{
    public class ServiciosController(IServicioApiService servicioApiService) : Controller
    {
        private readonly IServicioApiService _servicioApiService = servicioApiService;
        // GET: /Servicios
        /// <summary>
        /// Muestra la lista de todos los servicios
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var apiResponse = await _servicioApiService.ObtenerTodosLosServiciosAsync();


            return View(apiResponse.Data); 
        }
       

        // GET: /Servicios/Create
        /// <summary>
        /// Muestra el formulario para crear un nuevo servicio
        /// </summary>
        public ActionResult Create()
        {
            return View(new CrearServiciosViewModel());
        }

        // POST: /Servicios/Create
        /// <summary>
        /// Procesa la creación de un nuevo servicio enviado desde el formulario
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearServiciosViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var apiResponse = await _servicioApiService.AgregarServicioAsync(request);
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Se agrego el servicio correctamente", $"Error al agregar el servicio: {apiResponse.Message}");
            return RedirectToAction(nameof(Index));

        }
        // GET: /Servicios/Edit/{id}
        /// <summary>
        /// Muestra el formulario para editar un servicio existente
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var apiResponse = await _servicioApiService.ObtenerServicioPorIdAsync(id);
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Se cargo el servicio para edicion correctamente", $"No se pudo cargar el servicio para edición: {apiResponse.Message}");
            
            return View(apiResponse.Data);
        }
        // POST: /Servicios/Edit/
        /// <summary>
        /// Procesa la edición de un servicio existente enviado desde el formulario
        /// </summary>
        /// <param name="id">Id del servicio a actualizar</param>
        /// <param name="model">Los datos actualizados</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActualizarServicioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var apiResponse = await _servicioApiService.EditarServicioAsync(id, request);
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Servicio editado correctamente", $"Error al editar el servicio {request.Nombre}: {apiResponse.Message}");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Servicios/ActivarServicio
        /// <summary>
        /// Activa un servicio existente
        /// </summary>
        /// <param name="id">El Id del servicio a activar</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivarServicio(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id de servicio inválido";
                return RedirectToAction(nameof(Index));
            }
            var apiResponse = await _servicioApiService.ActivarServicioAsync(id);
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Servicio activado Correctamente", $"Error al activar el servicio: {apiResponse.Message}");
            return RedirectToAction(nameof(Index)); ;

        }

        // POST: /Servicios/DesactivarServicio
        /// <summary>
        /// Desactiva un servicio existente
        /// </summary>
        /// <param name="id">El Id del servicio a desactivar</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DesactivarServicio(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id de servicio inválido";
                return RedirectToAction(nameof(Index));
            }

            var apiResponse = await _servicioApiService.DesactivarServicioAsync(id);
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Servicio desactivado Correctamente", $"Error al desactivar el servicio: {apiResponse.Message}");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Servicios/Delete
        /// <summary>
        /// Elimina un servicio existente
        /// </summary>
        /// <param name="id">El Id del servicio a eliminar</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apiResponse = await _servicioApiService.EliminarServicioAsync(id);
            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Se ha eliminado el servicio correctamente", $"Error al eliminar el servicio {apiResponse.Message}");
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Obtiene los precios y las categorias de un servicio especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPreciosPorServicio(int id)
        {
            var apiResponse = await _servicioApiService.ObtenerServicioConPreciosAsync(id);

            ControllerActionHelper.ProcesarApiResponse(this, apiResponse, "Precios obtenidos", $"Precios no obtenidos {apiResponse.Message}");
            return View("DetallesServicio", apiResponse.Data);
        }

      
        /// <summary>
        /// Muestra el formulario para asignar o editar un nuevo precio a un servicio específico.
        /// Solo requiere el ID del servicio. El ID de categoría y precio se ingresan manualmente.
        /// </summary>
        /// <param name="idServicio">El ID del servicio al que se asignará el precio.</param>
        [HttpGet]
        public async Task<IActionResult> AsignarPrecio(int idServicio)
        {
            var servicioResponse = await _servicioApiService.ObtenerServicioPorIdAsync(idServicio);
            if (servicioResponse?.Data == null)
            {
                TempData["ErrorMessage"] = "Servicio no encontrado para asignar precio.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new AsignarPrecioServicioCategoriaViewModel
            {
                IdServicio = idServicio,
                NombreServicio = servicioResponse.Data.Nombre
            };
            return View("AsignarPrecio", viewModel);
        }

        // POST: /Servicios/AsignarPrecio
        /// <summary>
        /// Asigna un precio a un servicio por categoría
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarEditarPrecio(AsignarPrecioServicioCategoriaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AsignarPrecio", model);
            }
            var response = await _servicioApiService.AsignarActualizarPrecioAsync(model);
            ControllerActionHelper.ProcesarApiResponse(this, response, "Precio asignado correctamente", $"Error al asignar precio");
            return RedirectToAction(nameof(Index));
        }
    }
}