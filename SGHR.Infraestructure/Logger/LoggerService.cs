using Microsoft.Extensions.Logging;
using SGHR.Infraestructure.Interface;

namespace SGHR.Infraestructure.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger _logger;
        void ILoggerService.LogError(string message, Exception? exception = null)
        {
            if (exception != null)
                _logger.LogError(exception, message);
            else
                _logger.LogError(message);
        }
        void ILoggerService.LogInfo(string message) => _logger.LogInformation(message);
        void ILoggerService.LogWarning(string message) => _logger.LogWarning(message);
    }
}
