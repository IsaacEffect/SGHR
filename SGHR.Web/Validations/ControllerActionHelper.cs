using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models;

namespace SGHR.Web.Validations
{
    public class ControllerActionHelper
    {
        public static void ProcesarApiResponse<T>(
        Controller controller,
        ApiResponse<T> response,
        string successMessage,
        string fallbackErrorMessage)
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

