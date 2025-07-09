using Microsoft.EntityFrameworkCore;
using SGHR.Domain.DomainServices.SharedInterfaces;
using SGHR.Persistance.DBContext;


namespace SGHR.Infraestructure.Adapters
{
    public class ProveedorTarifaCategoriaAdapter : IProveedorTarifaCategoria
    {
        private readonly SGHRDbContext _context;
        public ProveedorTarifaCategoriaAdapter(SGHRDbContext SGHRDb) 
        { 
            _context = SGHRDb;
        }

        public async Task<decimal?> ObtenerTarifaBasePorCategoriaAsync(int idCategoria)
        {
            var tarifa = await _context.Tarifas
            .Where(t => t.IdCategoriaHabitacion == idCategoria && t.Estado)
            .OrderByDescending(t => t.FechaInicio)
            .FirstOrDefaultAsync();

            return tarifa?.Precio;
        }
    }
}
