using SGHR.Web.ViewModel;
using SGHR.Web.ViewModel.Reservas;
using SGHR.Application.DTOs;
namespace SGHR.Web.Services
{
    public class ReservasApiService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Obtiene todas las reservas
        /// </summary>
        public async Task<ApiResponse<List<ReservasViewModel>>> ObtenerTodasReservasAsync(bool incluirRelaciones = false)
        {
            try
            {
                var url = $"/api/Reservas/todas?incluirRelaciones={incluirRelaciones}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Leyendo el contenido como string");
                    Console.WriteLine(content);
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReservasViewModel>>>();
                    
                    return apiResponse ?? new ApiResponse<List<ReservasViewModel>> { IsSuccess = false, Data = new List<ReservasViewModel>()};
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ReservasViewModel>>
                    {
                        IsSuccess = false,
                        Message = $"Error al obtener todas las reservas: {response.ReasonPhrase} - {errorMessage}"
                    };
                }
            }
            catch(HttpRequestException httpEx)
            {
                return new ApiResponse<List<ReservasViewModel>> { IsSuccess = false, Message = $"Error de red al obtener reservas {httpEx.Message}" };
            }
            catch(Exception ex)
            {
                return new ApiResponse<List<ReservasViewModel>> { IsSuccess = false, Message = $"Error al obtener reservas: {ex.Message}" };
            }
        }

        /// <summary>
        /// Obtiene una reserva especifica por su ID
        /// </summary>
        public async Task<ApiResponse<ReservasViewModel>> ObtenerReservaPorIdAsync(int id)
        {
            try
            {
                var url = $"/api/Reservas/{id}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ReservasViewModel>>();
                    return apiResponse ?? new ApiResponse<ReservasViewModel> { IsSuccess = true };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<ReservasViewModel> {IsSuccess = false, Message = "Reserva no encontrada"};
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error al obtener la reserva: {response.ReasonPhrase} - {errorMessage}" };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error de red al obtener reserva: {httpEx.Message}" };
            }
            catch(Exception ex)
            {
                return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error de red al obtener reserva: {ex.Message}" };
            }
        }

        /// <summary>
        /// Obtiene reservas en un rango especifico de fechas
        /// </summary>
        public async Task<ApiResponse<List<ReservasViewModel>>> ObtenerReservasPorRangoFechasAsync(DateTime desde, DateTime hasta)
        {
            try
            {
                string formatoInicio = desde.ToString("yyyy-MM-ddTHH:mm:ss");
                string formatoSalida = hasta.ToString("yyyy-MM-ddTHH:mm:ss");
                var url = $"/api/Reservas/rango?desde={formatoInicio}&hasta={formatoSalida}"; // aqui ta mijo 
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<ReservasViewModel>>>(); 
                    return apiResponse ?? new ApiResponse<List<ReservasViewModel>> { IsSuccess = false, Data = new List<ReservasViewModel>() };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ReservasViewModel>> 
                    { 
                        IsSuccess = false, 
                        Message = $"No se encontraron reservas en el rango de fechas especificado: {errorMessage}" 
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ReservasViewModel>> 
                    { 
                        IsSuccess = false, 
                        Message = $"Error al obtener reservas por rango de fechas: {response.ReasonPhrase} - {errorMessage}" 
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<List<ReservasViewModel>> { IsSuccess = false, Message = $"Error de red al obtener reservas por rango de fechas: {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ReservasViewModel>> { IsSuccess = false, Message = $"Error al obtener reservas por rango de fechas: {ex.Message}" };
            }
        }

        /// <summary>
        /// Crea una nueva reserva a traves de la API
        /// </summary>
        public async Task<ApiResponse<ReservasViewModel>> CrearReservaAsync(CrearReservaViewModel model)
        {
            try
            {
                var url = "/api/Reservas/CrearReserva";
                var response = await _httpClient.PostAsJsonAsync(url, model);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ReservasViewModel>>();
                    return apiResponse ?? new ApiResponse<ReservasViewModel> { IsSuccess = true };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error al crear la reserva: {response.ReasonPhrase} - {errorMessage}" };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error de red al crear reserva: {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error al crear reserva: {ex.Message}" };
            }
        }

        ///<summary>
        /// Actualizar una reserva a traves de la API
        /// </summary>
        public async Task<ApiResponse<ReservasViewModel>> ActualizarReservasAsync(int id,ActualizarReservaViewModel actualizarReservaViewModel)
        {
            try
            {
                var url = $"/api/Reservas/{id}";
                var response = await _httpClient.PutAsJsonAsync(url, actualizarReservaViewModel);

                if(response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ReservasViewModel>>();
                    return apiResponse ?? new ApiResponse<ReservasViewModel> { IsSuccess = true };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = "Reserva no encontrada" };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<ReservasViewModel> 
                    { 
                        IsSuccess = false, 
                        Message = $"Error al actualizar la reserva: {response.ReasonPhrase} - {errorMessage}"
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error de red al crear reserva: {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ReservasViewModel> { IsSuccess = false, Message = $"Error al crear reserva: {ex.Message}" };
            }
        }

        /// <summary>
        /// Cancela una reserva por su ID a traves de la api
        /// </summary>
        public async Task<ApiResponse<bool>> CancelarReservaAsync(CancelarReservaViewModel model)
        {
            try
            {
                var url = $"/api/Reservas/{model.Id}/cancelar";
                var response = await _httpClient.PutAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool> { IsSuccess = true, Data = true };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ApiResponse<bool> { IsSuccess = false, Message = "Reserva no encontrada" };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<bool> { IsSuccess = false, Message = $"Error al cancelar la reserva: {response.ReasonPhrase} - {errorMessage}" };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = $"Error de red al cancelar reserva: {httpEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { IsSuccess = false, Message = $"Error al cancelar reserva: {ex.Message}" };
            }
        }
    }
}
