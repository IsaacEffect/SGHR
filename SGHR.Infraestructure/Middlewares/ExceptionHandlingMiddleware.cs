//using Microsoft.AspNetCore.Http;
//using SGHR.Infraestructure.Interface;


//namespace SGHR.Infraestructure.Middlewares
//{
//    public class ExceptionHandlingMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILoggerService _logger;

//        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerService logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (BusinessException ex)
//            {
//                _logger.LogWarning($"Error de negocio: {ex.Message}");
//                context.Response.StatusCode = 400;
//                await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Error inesperado", ex);
//                context.Response.StatusCode = 500;
//                await context.Response.WriteAsJsonAsync(new { Message = "Ocurrió un error inesperado." });
//            }
//        }
//    }

//}
