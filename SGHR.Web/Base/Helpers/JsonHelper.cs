using System;
using System.Text.Json;
using SGHR.Domain.Base;

namespace SGHR.Web.Base.Helpers
{
    public static class JsonHelper
    {
        // OperationResult Generico
        public static OperationResult<T> DeserializeOperationResult<T>(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return OperationResult<T>.Fail("Respuesta vacía de la API.");

                var result = JsonSerializer.Deserialize<OperationResult<T>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? OperationResult<T>.Fail("No se pudo deserializar la respuesta.");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Fail($"Error al deserializar: {ex.Message}");
            }
        }

        // Para OperationResult sin T
        public static OperationResult DeserializeOperationResult(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return OperationResult.Fail("Respuesta vacía de la API.");

                var result = JsonSerializer.Deserialize<OperationResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? OperationResult.Fail("No se pudo deserializar la respuesta.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Error al deserializar: {ex.Message}");
            }
        }
    }
}