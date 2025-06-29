using SGHR.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository Clients { get; }
        Task<int> SaveChangesAsync();
    }
}
