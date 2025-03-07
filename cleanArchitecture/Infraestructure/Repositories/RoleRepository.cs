namespace Infrastructure.Repositories;

/// <summary>
/// The role class is used to perform corresponding operations on the database.
/// </summary>
public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    protected readonly AppDbContext _dbContext;
    public RoleRepository(AppDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async ValueTask<bool> ExistsByNameAsync(string name)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == name);
        if (role != null)
            return true;

        return false;
    }

}
