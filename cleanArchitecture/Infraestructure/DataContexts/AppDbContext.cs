namespace Infrastructure.DataContexts;

/// <summary>
/// Contains the database models and accesses the connection settings.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// In this method the entities of the database are defined; if an alias is used, the name it has in the database is specified.
    /// </summary>
    /// <param name="modelBuilder">Privides a simple Api surface for configuring a Microsoft.EntityFramewwork. </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().ToTable("role");
    }
}
