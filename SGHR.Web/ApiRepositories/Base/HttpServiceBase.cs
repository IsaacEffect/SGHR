using System.Net;
using System.Text.Json;
using SGHR.Web.Models;

namespace SGHR.Web.ApiRepositories.Base
{
    public abstract class HttpServiceBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly JsonSerializerOptions _jsonOptions;

        public HttpServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        #region Métodos Genéricos HTTP
        protected async Task<ApiResponse<T>> GetAsync<T>(string endpoint) where T : class
        {
            try
           {
                var response = await _httpClient.GetAsync(endpoint);
                return await ProcessApiResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return CreateErrorResponse<T>($"Error de red: {ex.Message}");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>($"Error inesperado: {ex.Message}");
            }
        }
        protected async Task<ApiResponse<List<T>>> GetListAsync<T>(string endpoint) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                return await ProcessApiResponse<List<T>>(response);
            }
            catch (HttpRequestException ex)
            {
                return CreateErrorResponse<List<T>>($"Error de red: {ex.Message}");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<List<T>>($"Error inesperado: {ex.Message}");
            }
        }
        protected async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data) where T : class
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                return await ProcessApiResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return CreateErrorResponse<T>($"Error de red: {ex.Message}");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>($"Error inesperado: {ex.Message}");
            }
        }
        protected async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? data = null) 
        {
            try
            {
                HttpResponseMessage response = data != null
                    ? await _httpClient.PutAsJsonAsync(endpoint, data)
                    : await _httpClient.PutAsync(endpoint, null);

                return await ProcessApiResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return CreateErrorResponse<T>($"Error de red: {ex.Message}");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>($"Error inesperado: {ex.Message}");
            }
        }
        protected async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool> { IsSuccess = true, Data = true };
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return CreateDeleteErrorResponse(response.StatusCode, errorMessage);
            }
            catch (HttpRequestException ex)
            {
                return CreateErrorResponse<bool>($"Error de red: {ex.Message}");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<bool>($"Error inesperado: {ex.Message}");
            }
        }
        #endregion

        #region Procesamiento de Respuestas
        protected async Task<ApiResponse<T>> ProcessApiResponse<T>(HttpResponseMessage response) 
        {
            if (response.IsSuccessStatusCode)
            {
                return await ProcessSuccessResponse<T>(response);
            }
            else
            {
                return await ProcessErrorResponse<T>(response);
            }
        }
        protected async Task<ApiResponse<T>> ProcessSuccessResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = true,
                        Data = default,
                        Message = "Respuesta vacía con éxito"
                    };
                }
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
                if (apiResponse != null && (apiResponse.IsSuccess || apiResponse.Data != null))
                {
                    return apiResponse;
                }
            }
            catch (JsonException)
            {
                try
                {
                    if (typeof(T) == typeof(bool))
                    {
                        return new ApiResponse<T>
                        {
                            IsSuccess = true,
                            Data = (T)(object)true, 
                            Message = "Estado de servicio actualizado exitosamente."
                        };
                    }
                }
                catch (JsonException innerEx)
                {
                    return CreateErrorResponse<T>($"Error al deserializar respuesta directa: {innerEx.Message}");
                }
            }
            return CreateErrorResponse<T>("Respuesta del API no válida");
        }

        protected async Task<ApiResponse<T>> ProcessErrorResponse<T>(HttpResponseMessage response)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();

            return response.StatusCode switch
            {
                HttpStatusCode.NotFound => CreateErrorResponse<T>("Recurso no encontrado"),
                HttpStatusCode.BadRequest => CreateErrorResponse<T>($"Solicitud inválida: {errorMessage}"),
                HttpStatusCode.Unauthorized => CreateErrorResponse<T>("No autorizado"),
                HttpStatusCode.Forbidden => CreateErrorResponse<T>("Acceso prohibido"),
                HttpStatusCode.InternalServerError => CreateErrorResponse<T>($"Error interno del servidor: {errorMessage}"),
                _ => CreateErrorResponse<T>($"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase} - {errorMessage}")
            };
        }

        protected ApiResponse<bool> CreateDeleteErrorResponse(HttpStatusCode statusCode, string errorMessage)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => CreateErrorResponse<bool>("Recurso no encontrado"),
                _ => CreateErrorResponse<bool>($"Error al eliminar: {errorMessage}")
            };
        }

        protected ApiResponse<T> CreateErrorResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default
            };
        }
        #endregion
    }
}