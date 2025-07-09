using SGHR.Application.DTOsTarifa;
using SGHR.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.InterfacesServices
{
    public interface ITarifaService
    {
        Task ActualizarTarifaAsync(int id, ActualizarTarifaDto dto);
        Task DefinirTarifaAsync(int idCategoria, DefinirTarifaBaseDto dto);
        Task DefinirTarifaPorTemporadaAsync(int id, DefinirTarifaPorTemporadaDto dto);
    }
}
