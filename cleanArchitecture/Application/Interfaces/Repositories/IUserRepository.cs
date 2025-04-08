namespace Application.Interfaces.Repositories;

/// <summary>
/// It is a contract that contains the business operations for the user repository.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Get a especified user by userName.
    /// </summary>
    /// <param name="userName">The userName property is the parameter for the search.</param>
    /// <returns>Returns the User entity.</returns>
    ValueTask<User> GetByUserNameAsync(string userName);
}
