using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Services;
using SGHR.Web.ViewModel.ServicioCategoria;
using SGHR.Web.ViewModel.Servicios;

namespace SGHR.Web.Controllers
{
    public class ServiciosController(ServiciosApiService serviciosApiService, ServicioCategoriaApiService servicioCategoriaApiService) : Controller
    {
        private readonly ServiciosApiService _serviciosApiService = serviciosApiService;
        private readonly ServicioCategoriaApiService _servicioCategoriaApiService = servicioCategoriaApiService;

        // GET: /Servicios
        /// <summary>
        /// Muestra la lista de todos los servicios
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var apiResponse = await _serviciosApiService.ObtenerTodosServiciosAsync();
            if (apiResponse.IsSuccess && apiResponse.Data != null)
            {
                return View(apiResponse.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Error al cargar los servicios");
                return View(new List<ServiciosViewModel>());
            }
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
        public async Task<IActionResult> Create(CrearServiciosViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiResponse = await _serviciosApiService.CrearServicioAsync(model);
            if (apiResponse.IsSuccess && apiResponse.Data != null)
            {
                TempData["SuccessMessage"] = "Servicio creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = $"Error al crear servicio: {apiResponse.Message}";
                return View(model);
            }
        }

        // GET: /Servicios/Edit/{id}
        /// <summary>
        /// Muestra el formulario para editar un servicio existente
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var apiResponse = await _serviciosApiService.ObtenerServicioPorIdAsync(id);

            if (!apiResponse.IsSuccess || apiResponse.Data == null)
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "No se pudo cargar el servicio para edición";
                return RedirectToAction(nameof(Index));
            }

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
        public async Task<IActionResult> Edit(int id, ActualizarServiciosViewModel model)
        {
            if (id <= 0)
            {
                return BadRequest("Id de servicio inválido");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiResponse = await _serviciosApiService.ActualizarServicioAsync(id, model);
            if (apiResponse.IsSuccess)
            {
                TempData["SuccessMessage"] = "Servicio actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            else if (!apiResponse.IsSuccess && apiResponse.Message.Contains("no encontrado"))
            {
                ModelState.AddModelError(string.Empty, "Servicio no encontrado: no se puede actualizar");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Error al actualizar el servicio";
                return View(model);
            }
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

            var apiResponse = await _serviciosApiService.ActivarServicioAsync(id);
            if (apiResponse.IsSuccess)
            {
                TempData["SuccessMessage"] = apiResponse.Message ?? "Servicio activado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "Error al activar el servicio";
            }

            return RedirectToAction(nameof(Index));
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

            var apiResponse = await _serviciosApiService.DesactivarServicioAsync(id);
            if (apiResponse.IsSuccess)
            {
                TempData["SuccessMessage"] = apiResponse.Message ?? "Servicio desactivado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "Error al desactivar el servicio";
            }

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
            var apiResponse = await _serviciosApiService.EliminarServicioAsync(id);
            if (apiResponse.IsSuccess)
            {
                TempData["SuccessMessage"] = "Servicio eliminado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "Error al eliminar el servicio";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Servicios/GetByCategoria/{id}
        /// <summary>
        /// Obtiene servicios por categoría
        /// </summary>
        /// <param name="id">Id de la Categoría</param>
        [HttpGet]
        public async Task<IActionResult> GetByCategoria(int id)
        {
            var result = await _serviciosApiService.ObtenerServiciosPorCategoriaAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound("No se encontraron servicios para esta categoría.");
            }
            return View("ListaPorCategoria", result.Data);
        }

        // GET: /Servicios/Activos
        /// <summary>
        /// Muestra solo los servicios activos
        /// </summary>
        public async Task<IActionResult> Activos()
        {
            var apiResponse = await _serviciosApiService.ObtenerServiciosActivosAsync();

            if (apiResponse.IsSuccess && apiResponse.Data != null)
            {
                return View("Index", apiResponse.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, apiResponse.Message ?? "Error al cargar los servicios activos");
                return View("Index", new List<ServiciosViewModel>());
            }
        }
        /// <summary>
        /// Obtiene los precios y las categorias de un servicio especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPreciosPorServicio(int id)
        {
            var servicioResponse = await _serviciosApiService.ObtenerServicioPorIdAsync(id);
            var preciosResponse = await _servicioCategoriaApiService.ObtenerPreciosCategoriaPorServicioAsync(id);

            if (servicioResponse?.Data == null)
            {
                TempData["ErrorMessage"] = "Servicio no encontrado";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ServicioConPreciosViewModel
            {
                IdServicio = servicioResponse.Data.IdServicio,
                Nombre = servicioResponse.Data.Nombre, 
                Descripcion = servicioResponse.Data.Descripcion,
                Activo = servicioResponse.Data.Activo,
                FechaCreacion = servicioResponse.Data.FechaCreacion, 
                FechaModificacion = servicioResponse.Data.FechaModificacion, 
                Eliminado = servicioResponse.Data.Eliminado,
                FechaEliminacion = servicioResponse.Data.FechaEliminacion,
                PreciosPorCategoria = preciosResponse.IsSuccess && preciosResponse.Data != null
                                ? preciosResponse.Data
                                : new List<ServicioCategoriaViewModel>()
            };
            return View("DetallesServicio", viewModel); 
        }

        // GET: /Servicios/GetPrecioEspecifico?idServicio=1&idCategoriaHabitacion=2
        /// <summary>
        /// Muestra el precio específico de un servicio por categoría
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPrecioEspecifico(int idServicio, int idCategoriaHabitacion)
        {
            var response = await _servicioCategoriaApiService.ObtenerPrecioServicioCategoriaEspecificoAsync(idServicio, idCategoriaHabitacion);
            if (!response.IsSuccess || response.Data == null)
            {
                return NotFound(response.Message ?? "Precio no encontrado");
            }

            return View("DetallePrecioEspecifico", response.Data);
        }
        /// <summary>
        /// Muestra el formulario para asignar un nuevo precio a un servicio específico.
        /// Solo requiere el ID del servicio. El ID de categoría y precio se ingresan manualmente.
        /// </summary>
        /// <param name="idServicio">El ID del servicio al que se asignará el precio.</param>
        [HttpGet]
        public async Task<IActionResult> AsignarPrecio(int idServicio)
        {
            var servicioResponse = await _serviciosApiService.ObtenerServicioPorIdAsync(idServicio);
            if (servicioResponse?.Data == null)
            {
                TempData["ErrorMessage"] = "Servicio no encontrado para asignar precio.";
                return RedirectToAction(nameof(Index)); 
            }

            var viewModel = new AsignarPrecioServicioCategoriaViewModel
            {
                IdServicio = idServicio,
                NombreServicio = servicioResponse?.Data?.Nombre       
            };
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", servicioResponse.Message ?? "Error desconocido al asignar precio.");
                return View("AsignarPrecio", viewModel); 
            }
            else 
            {
                return View("AsignarPrecio", viewModel); 
            }
        }

        // POST: /Servicios/AsignarPrecio
        /// <summary>
        /// Asigna un precio a un servicio por categoría
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarPrecio(AsignarPrecioServicioCategoriaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AsignarPrecio", model);
            }

            var response = await _servicioCategoriaApiService.AsignarPrecioServicioCategoriaAsync(model);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = response.Data ?? "Precio asignado correctamente";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["ErrorMessage"] = response.Message ?? "Error al asignar precio";
            return View("AsignarPrecio", model);
        }

        // POST: /Servicios/ActualizarPrecio
        /// <summary>
        /// Actualiza el precio de un servicio por categoría
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPrecio(ActualizarPrecioServicioCategoriaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditarPrecio", model);
            }

            var response = await _servicioCategoriaApiService.ActualizarPrecioServicioCategoriaAsync(model);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = response.Data ?? "Precio actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = response.Message ?? "Error al actualizar precio";
            return View("ActualizarPrecio", model);
        }

    }
}