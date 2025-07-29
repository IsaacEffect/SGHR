using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Services;
using SGHR.Web.ViewModel.Servicios;
using System.Threading.Tasks;

namespace SGHR.Web.Controllers
{
    public class ServiciosController(ServiciosApiService serviciosApiService) : Controller
    {
        private readonly ServiciosApiService _serviciosApiService = serviciosApiService;

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
                return RedirectToAction(nameof(Index)); // Redirige al listado
            }

            return View(apiResponse.Data); // Solo muestra la vista si los datos están bien
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
            if (apiResponse.IsSuccess && apiResponse.Data != null)
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

        // POST: /Servicios/Delete/{id}
        /// <summary>
        /// Elimina un servicio directamente por su ID
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID de servicio inválido";
                return RedirectToAction(nameof(Index));
            }

            var apiResponse = await _serviciosApiService.EliminarServicioAsync(id);

            if (apiResponse.IsSuccess)
            {
                TempData["SuccessMessage"] = "Servicio eliminado exitosamente";
            }
            else
            {
                TempData["ErrorMessage"] = $"Error al eliminar servicio: {apiResponse.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Servicios/CambiarEstado/{id}
        /// <summary>
        /// Muestra el formulario para cambiar el estado de un servicio (activar/desactivar)
        /// </summary>
        //[HttpGet]
        //public async Task<IActionResult> CambiarEstado(int id)
        //{
        //    var apiResponse = await _serviciosApiService.ObtenerServicioPorIdAsync(id);

        //    if (!apiResponse.IsSuccess || apiResponse.Data == null)
        //    {
        //        TempData["ErrorMessage"] = apiResponse.Message ?? "No se pudo cargar el servicio";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var model = new CambiarEstadoServicioViewModel
        //    {
        //        Id = id,
        //        Activo = !apiResponse.Data.Activo // Cambiar al estado opuesto
        //    };

        //    return View(model);
        //}

        // POST: /Servicios/CambiarEstado/
        /// <summary>
        /// Cambia el estado de un servicio existente (activar/desactivar)
        /// </summary>
       // [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CambiarEstado(CambiarEstadoServicioViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var response = await _serviciosApiService.CambiarEstadoServicioAsync(model);
        //    if (response.IsSuccess)
        //    {
        //        var mensaje = model.Activo ? "Servicio activado correctamente" : "Servicio desactivado correctamente";
        //        TempData["SuccessMessage"] = mensaje;
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError("", response.Message ?? "Error al cambiar el estado del servicio");
        //    return View(model);
        //}

        // GET: /Servicios/Categoria/{categoriaId}
        /// <summary>
        /// Obtiene servicios filtrados por categoría
        /// </summary>
        /// <param name="categoriaId">Id de la categoría</param>
        /// <returns>Lista de servicios de la categoría</returns>
        [HttpGet]
        public async Task<IActionResult> PorCategoria(int categoriaId)
        {
            var result = await _serviciosApiService.ObtenerServiciosPorCategoriaAsync(categoriaId);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = "No se encontraron servicios en esta categoría.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoriaId = categoriaId;
            return View("ListaPorCategoria", result.Data);
        }

        // GET: /Servicios/Activos
        /// <summary>
        /// Muestra solo los servicios activos
        /// </summary>
        public async Task<IActionResult> Activos()
        {
            var result = await _serviciosApiService.ObtenerServiciosActivosAsync();
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = "No se encontraron servicios activos.";
                return View("Index", new List<ServiciosViewModel>());
            }

            ViewBag.SoloActivos = true;
            return View("Index", result.Data);
        }

        // GET: /Servicios/Buscar
        /// <summary>
        /// Muestra el formulario de búsqueda de servicios
        /// </summary>
        public IActionResult Buscar()
        {
            return View();
        }

        // POST: /Servicios/Buscar
        /// <summary>
        /// Realiza la búsqueda de servicios por término
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Buscar(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
            {
                ModelState.AddModelError("termino", "Debe ingresar un término de búsqueda");
                return View();
            }

            var result = await _serviciosApiService.BuscarServiciosAsync(termino);
            if (!result.IsSuccess || result.Data == null)
            {
                ViewBag.Mensaje = $"No se encontraron servicios que coincidan con '{termino}'";
                return View("ResultadosBusqueda", new List<ServiciosViewModel>());
            }

            ViewBag.TerminoBusqueda = termino;
            return View("ResultadosBusqueda", result.Data);
        }

        // GET: /Servicios/Details/{id}
        /// <summary>
        /// Muestra los detalles de un servicio específico
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var apiResponse = await _serviciosApiService.ObtenerServicioPorIdAsync(id);

            if (!apiResponse.IsSuccess || apiResponse.Data == null)
            {
                TempData["ErrorMessage"] = apiResponse.Message ?? "No se pudo cargar el servicio";
                return RedirectToAction(nameof(Index));
            }

            return View(apiResponse.Data);
        }
    }
}