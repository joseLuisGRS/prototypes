namespace Application.Commons;

/// <summary>
/// This class contains the connection configuration properties. 
/// </summary>
public class ConnectionSettings
{
    public const string SectionName = "ConnectionStrings";
    public string DefaultConnection { get; init; } = "";
    public string DBHost { get; init; } = "";
    public string DBPort { get; init; } = "";
    public string DBInstance { get; init; } = "";
    public string DBName { get; init; } = "";
    public string DBUser { get; init; } = "";
    public string DBPassword { get; init; } = "";
}
