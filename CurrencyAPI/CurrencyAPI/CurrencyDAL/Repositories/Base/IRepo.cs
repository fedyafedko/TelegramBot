using Microsoft.EntityFrameworkCore;

namespace CurrencyDAL.Repositories
{
    public interface IRepo<TEntity, TKey> : IDisposable
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        DbSet<TEntity> Table { get; }

        int Add(TEntity entity, bool persist = true);
        Task<int> AddAsync(TEntity entity, bool persist = true);

        TEntity Find(TKey key);
        Task<TEntity> FindAsync(TKey key);

        int Update(TEntity entity, bool persist = true);
        Task<int> UpdateAsync(TEntity entity, bool persist = true);

        int Delete(TEntity entity, bool persist = true);
        Task<int> DeleteAsync(TEntity entity, bool persist = true);
        IEnumerable<TEntity> GetAll();

        int SaveChages();
        Task<int> SaveChagesAsync();
    }
}
