using SGHR.Web.ViewModel;
using SGHR.Web.ViewModel.ServicioCategoria;
using System.Text;
using System.Text.Json;
namespace SGHR.Web.Services
{
    public class ServicioCategoriaApiService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ApiResponse<string>> AsignarPrecioServicioCategoriaAsync(AsignarPrecioServicioCategoriaViewModel request)
        {
            try
            {
            
                var url = "/api/ServicioCategoria/AsignarPrecio";
                var response = await _httpClient.PostAsJsonAsync(url, request);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponseFromBackend = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

                    if (apiResponseFromBackend != null && apiResponseFromBackend.IsSuccess)
                    {
                        return apiResponseFromBackend;
                    }
                    else
                    {
                        var errorMessage = apiResponseFromBackend?.Message ?? "Respuesta exitosa de API, pero la operación no fue marcada como exitosa o el mensaje es nulo.";
                        return new ApiResponse<string> { IsSuccess = false, Message = errorMessage };
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorApiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(errorContent);
                        return new ApiResponse<string> { IsSuccess = false, Message = errorApiResponse?.Message ?? $"Error HTTP {response.StatusCode}: {errorContent}" };
                    }
                    catch (JsonException)
                    {
                        return new ApiResponse<string> { IsSuccess = false, Message = $"Error HTTP {response.StatusCode}: {errorContent}" };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, Message = $"Excepción al asignar precio: {ex.Message}" };
            }
        }
    
        public async Task<ApiResponse<string>> ActualizarPrecioServicioCategoriaAsync(ActualizarPrecioServicioCategoriaViewModel request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("/api/ServicioCategoria/ActualizarPrecio/" + request.IdServicio, request);
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<string> { IsSuccess = true, Data = message };
                }

                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse<string> { IsSuccess = false, Message = $"Error al actualizar precio: {error}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, Message = $"Excepción al actualizar precio: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<List<ServicioCategoriaViewModel>>> ObtenerPreciosServicioPorCategoriaAsync(int idCategoriaHabitacion)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/ServicioCategoria/ObtenerPreciosServicioPorCategoria/{idCategoriaHabitacion}");
                if (response.IsSuccessStatusCode)
                {
                    var precios = await response.Content.ReadFromJsonAsync<List<ServicioCategoriaViewModel>>();
                    return new ApiResponse<List<ServicioCategoriaViewModel>> { IsSuccess = true, Data = precios ?? [] };
                }

                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse<List<ServicioCategoriaViewModel>> { IsSuccess = false, Message = $"Error al obtener precios: {error}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ServicioCategoriaViewModel>> { IsSuccess = false, Message = $"Excepción: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<List<ServicioCategoriaViewModel>>> ObtenerPreciosCategoriaPorServicioAsync(int idServicio)
        {
            try
            {
                var url = $"/api/ServicioCategoria/ObtenerPreciosCategoriaPorServicio/{idServicio}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    // 1. Deserializa toda la ApiResponse que viene del backend
                    var apiResponseFromBackend = await response.Content.ReadFromJsonAsync<ApiResponse<List<ServicioCategoriaViewModel>>>();

                    // 2. Verifica si la deserialización fue exitosa y si hay datos
                    if (apiResponseFromBackend != null && apiResponseFromBackend.IsSuccess)
                    {
                        
                        return new ApiResponse<List<ServicioCategoriaViewModel>>
                        {
                            IsSuccess = true,
                            Data = apiResponseFromBackend.Data ?? new List<ServicioCategoriaViewModel>(), 
                            Message = apiResponseFromBackend.Message 
                        };
                    }
                    else
                    {
                        var errorMessage = apiResponseFromBackend?.Message ?? "Respuesta exitosa de API, pero sin datos o error interno reportado.";
                        return new ApiResponse<List<ServicioCategoriaViewModel>> { IsSuccess = false, Message = errorMessage };
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound) // Manejar 404 explícitamente si tu API lo retorna
                {
                    return new ApiResponse<List<ServicioCategoriaViewModel>> { IsSuccess = false, Message = "No se encontraron precios para el servicio." };
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ServicioCategoriaViewModel>> { IsSuccess = false, Message = $"Error al obtener precios por servicio: {error}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ServicioCategoriaViewModel>> { IsSuccess = false, Message = $"Excepción: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<ServicioCategoriaViewModel>> ObtenerPrecioServicioCategoriaEspecificoAsync(int idServicio, int idCategoriaHabitacion)
        {
            try
            {
                var url = $"/api/ServicioCategoria/ObtenerPrecioServicioCategoriaEspecifico?idServicio={idServicio}&idCategoriaHabitacion={idCategoriaHabitacion}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var precio = await response.Content.ReadFromJsonAsync<ServicioCategoriaViewModel>();
                    return new ApiResponse<ServicioCategoriaViewModel> { IsSuccess = true, Data = precio };
                }

                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse<ServicioCategoriaViewModel> { IsSuccess = false, Message = $"Error al obtener precio específico: {error}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ServicioCategoriaViewModel> { IsSuccess = false, Message = $"Excepción: {ex.Message}" };
            }
        }
    }
}