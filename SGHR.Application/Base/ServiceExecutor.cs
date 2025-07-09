using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;

namespace SGHR.Application.Base
{
    public static class ServiceExecutor
    {
        public static async Task<OperationResult<T>> ExecuteAsync<T>(
            ILogger logger,
            Func<Task<T>> action,
            string errorMessage,
            string context = null)
        {
            try
            {
                return OperationResult<T>.Ok(await action());
            }
            catch (Exception ex)
            {
                if (context != null)
                    logger.LogError(ex, $"{errorMessage} | Contexto: {context}");
                else
                    logger.LogError(ex, errorMessage);

                return OperationResult<T>.Fail(errorMessage);
            }
        }

        public static async Task<OperationResult> ExecuteAsync(
            ILogger logger,
            Func<Task> action,
            string errorMessage,
            string context = null)
        {
            try
            {
                await action();
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                if (context != null)
                    logger.LogError(ex, $"{errorMessage} | Contexto: {context}");
                else
                    logger.LogError(ex, errorMessage);

                return OperationResult.Fail(errorMessage);
            }
        }
    }
}
