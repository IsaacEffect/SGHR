using SGHR.Web.ViewModel;
using SGHR.Web.ViewModel.Servicios;

namespace SGHR.Web.Services
{
    public class ServiciosApiService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Obtiene todos los servicios
        /// </summary>
        public async Task<ApiResponse<List<ServiciosViewModel>>> ObtenerTodosServiciosAsync()
        {
            try
            {
                var url = $"/api/Servicios/ObtenerTodosLosServicios";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ServiciosViewModel>>>();

                    return apiResponse ?? new ApiResponse<List<ServiciosViewModel>> { IsSuccess = false, Data = new List<ServiciosViewModel>() };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ServiciosViewModel>>
                    {
                        IsSuccess = false,
                        Message = $"Error al obtener todos los servicios: {response.ReasonPhrase} - {errorMessage}"
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<List<ServiciosViewModel>> { IsSuccess = false, Message = $"Error de red al obtener servicios {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ServiciosViewModel>> { IsSuccess = false, Message = $"Error al obtener servicios: {ex.Message}" };
            }
        }

        /// <summary>
        /// Obtiene un servicio específico por su ID
        /// </summary>
        public async Task<ApiResponse<ActualizarServiciosViewModel>> ObtenerServicioPorIdAsync(int id)
        {
            try
            {
                var url = $"/api/Servicios/ObtenerServicio/{id}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var servicio = await response.Content.ReadFromJsonAsync<ApiResponse<ActualizarServiciosViewModel>>();

                    if (servicio != null)
                    {
                        return servicio;
                    }
                else
                {
                    return new ApiResponse<ActualizarServiciosViewModel> { IsSuccess = false, Message = "La respuesta del API fue nula o inválida." };
                }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<ActualizarServiciosViewModel> { IsSuccess = false, Message = "Servicio no encontrado" };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<ActualizarServiciosViewModel> { IsSuccess = false, Message = $"Error al obtener el servicio: {response.ReasonPhrase} - {errorMessage}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ActualizarServiciosViewModel> { IsSuccess = false, Message = $"Error de red al obtener servicio: {ex.Message}" };
            }
        }

        /// <summary>
        /// Obtiene servicios por categoría
        /// </summary>
        public async Task<ApiResponse<List<ServiciosViewModel>>> ObtenerServiciosPorCategoriaAsync(int categoriaId)
        {
            try
            {
                var url = $"/api/Servicios/categoria/{categoriaId}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ServiciosViewModel>>>();
                    return apiResponse ?? new ApiResponse<List<ServiciosViewModel>> { IsSuccess = false, Data = new List<ServiciosViewModel>() };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ServiciosViewModel>>
                    {
                        IsSuccess = false,
                        Message = $"No se encontraron servicios en la categoría especificada: {errorMessage}"
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ServiciosViewModel>>
                    {
                        IsSuccess = false,
                        Message = $"Error al obtener servicios por categoría: {response.ReasonPhrase} - {errorMessage}"
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<List<ServiciosViewModel>> { IsSuccess = false, Message = $"Error de red al obtener servicios por categoría: {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ServiciosViewModel>> { IsSuccess = false, Message = $"Error al obtener servicios por categoría: {ex.Message}" };
            }
        }

        /// <summary>
        /// Crea un nuevo servicio a través de la API
        /// </summary>
        public async Task<ApiResponse<ServiciosViewModel>> CrearServicioAsync(CrearServiciosViewModel model)
        {
            try
            {
                var url = "/api/Servicios/AgregarServicio";
                var response = await _httpClient.PostAsJsonAsync(url, model);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ServiciosViewModel>>();
                    return apiResponse ?? new ApiResponse<ServiciosViewModel> { IsSuccess = true };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<ServiciosViewModel> { IsSuccess = false, Message = $"Error al crear el servicio: {response.ReasonPhrase} - {errorMessage}" };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<ServiciosViewModel> { IsSuccess = false, Message = $"Error de red al crear servicio: {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ServiciosViewModel> { IsSuccess = false, Message = $"Error al crear servicio: {ex.Message}" };
            }
        }

        /// <summary>
        /// Actualizar un servicio a través de la API
        /// </summary>
        public async Task<ApiResponse<object>> ActualizarServicioAsync(int id, ActualizarServiciosViewModel model)
        {
            try
            {
                var url = $"/api/Servicios/ActualizarServicio/{id}";
                var response = await _httpClient.PutAsJsonAsync(url, model);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        IsSuccess = true,
                        Data = true,
                        Message = "Servicio actualizado correctamente"
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Servicio no encontrado"
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = $"Error al actualizar el servicio: {response.ReasonPhrase} - {errorMessage}"
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Error de red al actualizar servicio: {httpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Error inesperado al actualizar servicio: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Elimina un servicio por su ID a través de la API
        /// </summary>
        public async Task<ApiResponse<bool>> EliminarServicioAsync(int id)
        {
            try
            {
                var url = $"/api/Servicios/EliminarServicio/{id}";
                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool> { IsSuccess = true, Data = true, Message = "Servicio eliminado correctamente" };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<bool> { IsSuccess = false, Message = "Servicio no encontrado" };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<bool> { IsSuccess = false, Message = $"Error al eliminar el servicio: {response.ReasonPhrase} - {errorMessage}" };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = $"Error de red al eliminar servicio: {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = $"Error al eliminar servicio: {ex.Message}" };
            }
        }

        /// Activa un servicio por su ID a través de la API
        /// </summary>
        public async Task<ApiResponse<object>> ActivarServicioAsync(int id)
        {
            try
            {
                var url = $"/api/Servicios/ActivarServicio/{id}";
                var response = await _httpClient.PutAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ServicioDto>>();
                    return new ApiResponse<object>
                    {
                        IsSuccess = apiResponse?.IsSuccess ?? true,
                        Message = apiResponse?.Message ?? "Servicio activado correctamente",
                        Data = apiResponse?.Data
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Servicio no encontrado"
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = $"Error al activar el servicio: {response.ReasonPhrase} - {errorMessage}"
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Error de red al activar servicio: {httpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Error inesperado al activar servicio: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Desactiva un servicio por su ID a través de la API
        /// </summary>
        public async Task<ApiResponse<object>> DesactivarServicioAsync(int id)
        {
            try
            {
                var url = $"/api/Servicios/DesactivarServicio/{id}";
                var response = await _httpClient.PutAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ServicioDto>>();
                    return new ApiResponse<object>
                    {
                        IsSuccess = apiResponse?.IsSuccess ?? true,
                        Message = apiResponse?.Message ?? "Servicio desactivado correctamente",
                        Data = apiResponse?.Data
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Servicio no encontrado"
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = $"Error al desactivar el servicio: {response.ReasonPhrase} - {errorMessage}"
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Error de red al desactivar servicio: {httpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Error inesperado al desactivar servicio: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Obtener servicios activos únicamente
        /// </summary>
        public async Task<ApiResponse<List<ServiciosViewModel>>> ObtenerServiciosActivosAsync()
        {
            try
            {
                var url = "/api/Servicios/activos";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var servicios = await response.Content.ReadFromJsonAsync<List<ServiciosViewModel>>();
                    return new ApiResponse<List<ServiciosViewModel>> { IsSuccess = true, Data = servicios };
                }

                return new ApiResponse<List<ServiciosViewModel>> { IsSuccess = false, Message = "No se encontraron servicios activos." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ServiciosViewModel>>
                {
                    IsSuccess = false,
                    Message = $"Error al buscar servicios activos: {ex.Message}"
                };
            }
        }
    }
}