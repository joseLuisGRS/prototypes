namespace Application.Interfaces.Repositories;

/// <summary>
/// It's the role interface used to perform the corresponding operations in the database. 
/// </summary>
public interface IRoleRepository : IBaseRepository<Role>
{
    /// <summary>
    /// This method validates the existence of a record by name.
    /// </summary>
    /// <param name="name">It's the parameter by which the search must be done. </param>
    /// <returns>Returns true when a record exists in the database or false otherwise. </returns>
    ValueTask<bool> ExistsByNameAsync(string name);
}
