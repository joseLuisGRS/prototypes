namespace Test.TestResources.Extensions;

public static class DbContextTest
{
    public static async Task<AppDbContext> InitializeTestDatabase(this AppDbContext context)
    {
        if (!context.Roles.Any())
        {
            var fakerRole = SeedData<Role>("Roles.json", @"TestResources\Seeds", true);
            fakerRole.ForEach(x => x.Id = 0);
            await context.Roles.AddRangeAsync(fakerRole);
        }

        await context.SaveChangesAsync();
        return context;
    }
}
