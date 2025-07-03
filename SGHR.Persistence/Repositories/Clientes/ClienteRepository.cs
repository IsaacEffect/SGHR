using SGHR.Domain.Entities.Users;
using SGHR.Persistence.Base;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Clientes;



namespace SGHR.Persistence.Repositories.Clientes
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(SGHRDbContext context, ISqlConnectionFactory sqlConnectionFactory)
            : base(context) { }

    }
}