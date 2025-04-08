namespace Infrastructure.Repositories;

/// <summary>
/// The user class is used to perform corresponding operations on the database.
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    protected readonly AppDbContext _dbContext;
    public UserRepository(AppDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async ValueTask<User> GetByUserNameAsync(string userName)
    {
        return await _dbContext.Users.AsNoTracking().Include(e => e.Person).Include(e => e.Role)
            .Where(x => x.UserName.Equals(userName) && x.IsActive.Equals(Convert.ToBoolean(StatusType.Active))
                && x.IsDeleted.Equals(Convert.ToBoolean(StatusType.InActive)))
            .SingleOrDefaultAsync();
    }
}
