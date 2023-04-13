using CurrencyDAL.EF;
using Microsoft.EntityFrameworkCore;


namespace CurrencyDAL.Repositories;

class Repo<TEntity, TKey> : IRepo<TEntity, TKey>
    where TEntity : class
    where TKey : IEquatable<TKey>
{
    private bool _disposeContext;
    private bool _isDisposed;

    private ApplicationDbContext ApplicationDbContext { get; set; }
    public DbSet<TEntity> Table { get; }

    protected Repo(ApplicationDbContext applicationDbContext)
    {
        ApplicationDbContext = applicationDbContext;
        Table = ApplicationDbContext.Set<TEntity>();
        _disposeContext = false;
    }

    public virtual int Add(TEntity entity, bool persist = true)
    {
        Table.Add(entity);
        return persist ? SaveChages() : 0;
    }

    public virtual async Task<int> AddAsync(TEntity entity, bool persist = true)
    {
        await Table.AddAsync(entity);
        return persist ? await SaveChagesAsync() : 0;
    }

    public int Delete(TEntity entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? SaveChages() : 0;
    }

    public virtual async Task<int> DeleteAsync(TEntity entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? await SaveChagesAsync() : 0;
    }
    public TEntity Find(TKey key)
    {
        return Table.Find(key);
    }

    public async Task<TEntity> FindAsync(TKey key)
    {
        return await Table.FindAsync(key);
    }
    public IEnumerable<TEntity> GetAll()
    {
        return Table;
    }
    public int Update(TEntity entity, bool persist = true)
    {
        throw new NotImplementedException();
    }
    public int SaveChages()
    {
        try
        {
            return ApplicationDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred updating the database", ex);
        }

    }

    public Task<int> SaveChagesAsync()
    {
        try
        {
            return ApplicationDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred updating the database", ex);
        }
    }

    public async Task<int> UpdateAsync(TEntity entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? await SaveChagesAsync() : 0;
    }

    public void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;
        if (disposing && _disposeContext)
            ApplicationDbContext.Dispose();
        _isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
