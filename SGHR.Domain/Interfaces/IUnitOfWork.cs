using SGHR.Domain.Interfaces.Repository;

namespace SGHR.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository Clients { get; }
        Task<int> SaveChangesAsync();
    }
}
