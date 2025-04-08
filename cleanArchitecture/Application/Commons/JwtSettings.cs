namespace Application.Commons;

/// <summary>
/// This class contains the Jwt configuration properties. 
/// </summary>
public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secrets { get; init; } = null!;
    public int ExpireMinutes { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; }
    public int AdditionalExpireMinutes { get; init; }
}
