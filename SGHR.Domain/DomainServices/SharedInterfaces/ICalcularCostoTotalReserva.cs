using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.DomainServices.SharedInterfaces
{
    public interface ICalcularCostoTotalReserva
    {
        public decimal CalcularTotalReserva(decimal tarifaCategoria, int dias, IEnumerable<decimal> serviciosAdicionales);
    }
}
