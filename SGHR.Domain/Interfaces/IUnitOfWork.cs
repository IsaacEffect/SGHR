namespace SGHR.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int>CommitAsync();
    }
}
