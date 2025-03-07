namespace Application.Interfaces.Repositories;

/// <summary>
/// It's a base contract that contains the basic operations of any repository.
/// Inherits from the "Base" class which is the base class for any entity.
/// </summary>
/// <typeparam name="T">T represents any class that can implement the IBaseRepository</typeparam>
public interface IBaseRepository<T> where T : Base
{
    /// <summary>
    /// Get all data from any entity.
    /// </summary>
    /// <returns>Returns a list of any entity</returns>
    ValueTask<IEnumerable<T>> GetAllAsync();
    /// <summary>
    /// Get the data of an entity by Id.
    /// </summary>
    /// <param name="id">The Id property is the parameter for the search</param>
    /// <returns>An entity with matching data</returns>
    ValueTask<T> GetByIdAsync(Int64 id);
    /// <summary>
    /// This method creates a new record in the db.
    /// </summary>
    /// <param name="entity">The entity parameter contains the data to be recorded in the db and specifies the entity that will be updated. </param>
    ValueTask AddAsync(T entity);
    /// <summary>
    /// This method update a existent record in the db.
    /// </summary>
    /// <param name="entity">The entity parameter contains the data to be updated in the db. </param>
    void UpdateAsync(T entity);
    /// <summary>
    /// This method update status a record in the db.
    /// </summary>
    /// <param name="entity">The entity parameter specifies the entity whose state will be updated. </param>
    void UpdateStatusAsync(T entity);
    /// <summary>
    /// This method deletes a record in the db.
    /// </summary>
    /// <param name="entity">The entity parameter specifies the entity to be deleted. </param>
    void DeleteAsync(T entity);
    /// <summary>
    /// This method saves changes to the database.
    /// </summary>
    /// <returns>Returns true when changes complete successfully or false otherwise. </returns>
    ValueTask<bool> SaveChangesAsync();
}
