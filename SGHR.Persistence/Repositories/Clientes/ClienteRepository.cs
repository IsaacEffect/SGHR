using SGHR.Persistence.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGHR.Domain.Entities.Users;
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.Clientes
{
    public class ClienteRepository : IBaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository() { }

        public Task AddAsync(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Cliente> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cliente>> FindAsync(Expression<Func<Cliente, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cliente>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Cliente?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Cliente> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Cliente entity)
        {
            throw new NotImplementedException();
        }
    }
}
