using Microsoft.AspNetCore.Mvc;
using SGHR.Domain.Base;

namespace SGHR.WebApp.Api.Extensions
{
    public static class OperationResultExtensions
    {
        public static IActionResult ToActionResult(this OperationResult result)
        {
            var response = new
            {
                success = result.Success,
                message = result.Message,
                data = (object)null
            };

            if (result.Success)
                return new OkObjectResult(response);

            return new BadRequestObjectResult(response);
        }

        public static IActionResult ToActionResult<T>(this OperationResult<T> result)
        {
            var response = new
            {
                success = result.Success,
                message = result.Message,
                data = result.Data
            };

            if (result.Success)
                return new OkObjectResult(response);

            if (!string.IsNullOrWhiteSpace(result.Message) &&
                result.Message.Contains("no encontrado", StringComparison.OrdinalIgnoreCase))
            {
                return new NotFoundObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }
    }
}
