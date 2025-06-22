using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaGestionHotel.Domain.Entities;
namespace SistemaGestionHotel.Application.Interfaces;

public interface IPisoRepository
{
    Task<IEnumerable<Piso>> ObtenerTodosAsync();
    Task<Piso> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Piso piso);
    Task ActualizarAsync(Piso piso);
    Task EliminarAsync(int id);
}
