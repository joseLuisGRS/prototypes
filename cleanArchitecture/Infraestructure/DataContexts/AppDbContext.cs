namespace Infrastructure.DataContexts;

/// <summary>
/// Contains the database models and accesses the connection settings.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// In this method the entities of the database are defined; if an alias is used, the name it has in the database is specified.
    /// </summary>
    /// <param name="modelBuilder">Privides a simple Api surface for configuring a Microsoft.EntityFramewwork. </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("person");
        modelBuilder.Entity<Role>().ToTable("role");
        modelBuilder.Entity<User>().ToTable("user");

        modelBuilder.Entity<User>()
            .HasOne(c => c.Person)
            .WithMany()
            .HasForeignKey(c => c.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(c => c.Role)
            .WithMany()
            .HasForeignKey(c => c.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
