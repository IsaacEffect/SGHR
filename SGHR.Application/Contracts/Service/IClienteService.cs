using SGHR.Application.Dtos;
using SGHR.Domain.Base;

namespace SGHR.Application.Contracts.Service
{
    public interface IClienteService
    {
        Task<OperationResult<IEnumerable<ObtenerClienteDto>>> ObtenerTodosAsync();
        Task<OperationResult<ObtenerClienteDto>> ObtenerPorIdAsync(int id);
        Task<OperationResult> InsertarAsync(InsertarClienteDto dto);
        Task<OperationResult> ModificarAsync(ModificarClienteDto dto);
        Task<OperationResult> CambiarContrasenaAsync(CambiarContrasenaDto dto);
        Task<OperationResult> EliminarAsync(int id);
        Task<OperationResult<ObtenerClienteDto>> ValidarCredencialesAsync(string email, string contrasena);

    }
}
