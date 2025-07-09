using SGHR.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.InterfacesRepositories
{
    public interface ITarifaRepository
    {
        Task DefinirTarifaAsync(Tarifa tarifa);
        Task DefinirTarifaPorTemporadaAsync(Tarifa tarifa);
        Task ActualizarTarifaAsync(Tarifa tarifa);
        Task<Tarifa?> ObtenerTarifaPorID(int id);
    }
}
