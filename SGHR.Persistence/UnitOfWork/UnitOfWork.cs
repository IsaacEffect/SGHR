using SGHR.Domain.Interfaces;
using SGHR.Persistence.Context;
namespace SGHR.Persistence.UnitOfWork
{
    public class UnitOfWork(SGHRDbContext context) : IUnitOfWork
    {
        private readonly SGHRDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
