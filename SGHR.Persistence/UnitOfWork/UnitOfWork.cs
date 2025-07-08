using SGHR.Domain.Interfaces;
using SGHR.Persistence.Context;
namespace SGHR.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SGHRDbContext _context;
        public UnitOfWork(SGHRDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
