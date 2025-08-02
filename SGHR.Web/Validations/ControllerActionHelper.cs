using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models;

namespace SGHR.Web.Validations
{
    public class ControllerActionHelper
    {
        public static void ProcesarApiResponseConMensajesDinamicos<T>(
        Controller controller,
        ApiResponse<T> response,
        DateTime? desde,
        DateTime? hasta)
        {
            string successMessage;
            string errorMessage = response.Message ?? "Error desconocido";

            if (desde.HasValue && hasta.HasValue)
                successMessage = $"Mostrando las reservas desde {desde:dd-MM-yyyy}, hasta {hasta:dd-MM-yyyy}";
            else if (desde.HasValue)
                successMessage = $"Mostrando las reservas desde {desde:dd-MM-yyyy}";
            else if (hasta.HasValue)
                successMessage = $"Mostrando las reservas hasta {hasta:dd-MM-yyyy}";
            else
                successMessage = "Mostrando todas las reservas";

            ControllerActionHelper.ProcesarApiResponse(controller, response, errorMessage, successMessage);
        }
        public static void ProcesarApiResponse<T>(
        Controller controller,
        ApiResponse<T> response,
        string fallbackErrorMessage,
        string? successMessage = null)
        {
            if (response.IsSuccess)
            {
                controller.TempData["SuccessMessage"] = response.Message ?? successMessage;
            }
            else if (response.Message?.Contains("no encontrado") == true)
            {
                controller.TempData["ErrorMessage"] = "El recurso no existe o fue eliminado.";
            }
            else
            {
                controller.TempData["ErrorMessage"] = response.Message ?? fallbackErrorMessage;
            }
        }

       

    }
}

