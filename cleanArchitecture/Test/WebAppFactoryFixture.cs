using Test.TestResources.Extensions;

namespace Test;

/// <summary>
/// Use only with Testcontainers
/// </summary>
/// <typeparam name="TProgram"></typeparam>
public class TestContainerFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : Program
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(
                d => d.ServiceType.Name.Contains("DbContext") ||
                (d.ServiceType.Name.Contains("DbContextOptions") &&
                d.ImplementationType?.Name.Contains("SqlServer") == true)
            ).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });

            var sp = services.BuildServiceProvider(validateScopes: true);

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<TestContainerFactory<Program>>>();

                db.Database.EnsureCreated();
                try
                {
                    DbContextTest.InitializeTestDatabase(db).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error seeding database with test data.");
                }
            }
        });
    }

}