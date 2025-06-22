using Microsoft.AspNetCore.Mvc;
using SGHR.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.InterfacesRepositories
{
    public interface ICategoriasHabitacionRepository
    {
       Task<IEnumerable<CategoriasHabitacion>> ObtenerTodasCategoriasAsync();
       Task CrearCategoriaAsync(CategoriasHabitacion categoriasHabitacion);
       Task ActualizarCategoriaAsync(CategoriasHabitacion categoriasHabitacion);
       void EliminarCategoriaAsync(CategoriasHabitacion categoriasHabitacion);
       Task GuardarCambiosAsync(); 
    }
}
