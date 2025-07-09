using SGHR.Domain.DomainServices.SharedInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.DomainServices.SharedServices
{
    public sealed class CalcularCostoTotalReservaDomainService : ICalcularCostoTotalReserva
    {
        public CalcularCostoTotalReservaDomainService() { }

        public decimal CalcularTotalReserva(decimal tarifaCategoria, int dias, IEnumerable<decimal> serviciosAdicionales)
        {
            return (tarifaCategoria * dias) + serviciosAdicionales.Sum();
           
        }
    }
}
