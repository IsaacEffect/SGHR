using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGHR.Domain.Entities.Users;
using SGHR.Persistence.Interfaces.Repositories.Base;

namespace SGHR.Persistence.Interfaces.Repositories.Clientes
{
    public interface IClienteRepository 
    {
        Task<Cliente?> GetByIdAsync(int id);
        
    }
}
