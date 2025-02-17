namespace Shared.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpsertRangeAsync(IEnumerable<T> entities);
    }
}
