using SGHR.Web.Models;

namespace SGHR.Web.Service.Contracts
{
    public interface IApiClienteService
    {
        Task<List<ClientesModel>> ObtenerTodosAsync();
        Task<ClientesModel?> ObtenerPorIdAsync(int id);
        Task<bool> InsertarAsync(ClientesModel model);
        Task<bool> ActualizarAsync(ActualizarClienteModel model);
        Task<bool> EliminarAsync(int id);
        Task<bool> CambiarContrasenaAsync(int idCliente, string actual, string nueva);
    }

}
