using SGHR.Application.DTOs;
using SGHR.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.InterfacesServices
{
    public interface ICategoriasHabitacionesService
    {
        Task<IEnumerable<ObtenerTodoCategoriaDto>> ObtenerTodasCategoriasAsync();
        Task<CategoriasHabitacion?> ObtenerPorIdAsync(int id);
        Task<CategoriasHabitacion> CrearCategoriaAsync(CrearCategoriaDto crearCategoriaDto);
        Task ActualizarCategoriaAsync(int? id, ActualizarCategoriaDto actualizarCategoriaDto);
        Task EliminarCategoriaAsync(int id);
        Task GuardarCambiosAsync();
    }
}
