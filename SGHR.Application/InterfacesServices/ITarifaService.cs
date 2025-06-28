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
        Task ActualizarTarifaAsync(ActualizarTarifaDto dto);
        Task DefinirTarifaAsync(DefinirTarifaBaseDto dto);
        Task DefinirTarifaPorTemporadaAsync(DefinirTarifaPorTemporadaDto dto);
    }
}
