namespace Infrastructure.Repositories;

/// <summary>
/// It's a base class that contains the basic operations of any repository. It implements the BaseDisposable class that is used to release resources in memory.
/// Inherits from the "Base" class which is the base class for any entity.
/// </summary>
/// <typeparam name="T">T represents any class that can implement the BaseRepository class. </typeparam>
public class BaseRepository<T> : BaseDisposable, IBaseRepository<T> where T : Base
{

    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    protected override void DisposeManagedResource()
    {
        try
        {
            _dbContext.Dispose();
        }
        finally
        {
            base.DisposeManagedResource();
        }
    }

    public virtual async ValueTask<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async ValueTask<T> GetByIdAsync(Int64 id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async ValueTask AddAsync(T entity)
    {
        entity.CreationDate = DateTime.Now;
        entity.IsActive = Convert.ToBoolean(StatusType.Active);
        entity.IsDeleted = Convert.ToBoolean(StatusType.InActive);
        await _dbSet.AddAsync(entity);
    }

    public virtual void UpdateAsync(T entity)
    {
        entity.ModificationDate = DateTime.Now;
        _dbSet.Entry(entity).State = EntityState.Modified;
    }

    public virtual void UpdateStatusAsync(T entity)
    {
        if (entity.IsActive)
            entity.IsActive = Convert.ToBoolean(StatusType.InActive);
        else
            entity.IsActive = Convert.ToBoolean(StatusType.Active);

        entity.ModificationDate = DateTime.Now;
        _dbSet.Entry(entity).State = EntityState.Modified;
    }

    public virtual void DeleteAsync(T entity)
    {
        entity.ModificationDate = DateTime.Now;
        entity.IsActive = Convert.ToBoolean(StatusType.InActive);
        entity.IsDeleted = Convert.ToBoolean(StatusType.Active);
        _dbSet.Entry(entity).State = EntityState.Modified;
    }

    public virtual async ValueTask<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

}
