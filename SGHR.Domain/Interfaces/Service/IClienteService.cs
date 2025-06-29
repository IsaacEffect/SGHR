using SGHR.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Interfaces.Service
{
    public interface IClienteService
    {
        Task<IEnumerable<ObtenerClienteDto>> ObtenerTodosAsync();
        Task<ObtenerClienteDto> ObtenerPorIdAsync(int id);
        Task InsertarAsync(InsertarClienteDto dto);
        Task ModificarAsync(ModificarClienteDto dto);
        Task EliminarAsync(int id);
    }
}
