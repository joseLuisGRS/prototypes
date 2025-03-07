namespace Application.Commons;

/// <summary>
/// This class contains the properties for swagger documentation. 
/// </summary>
public class SwaggerSettings
{
    public const string SectionName = "DocSwagger";
    public Contact Contact { get; init; } = null!;
    public InfoApi InfoApi { get; init; } = null!;

}

public class Contact {
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Url { get; init; } = null!;
}

public class InfoApi
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
}