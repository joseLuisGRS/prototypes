using System.Text.Json;
using static Utilities.Helpers.BaseHelper;

namespace Utilities.Helpers;

public static class Seed
{
    public static List<TEntity> SeedData<TEntity>(string fileName, string path, bool customPath = false)
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        if (customPath)
            currentDirectory = GetRootPath();

        string fullPath = Path.Combine(currentDirectory, path, fileName);

        using (StreamReader reader = new StreamReader(fullPath))
        {
            string json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<List<TEntity>>(json);
        }
    }

}
