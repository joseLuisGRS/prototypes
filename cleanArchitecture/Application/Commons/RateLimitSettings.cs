namespace Application.Commons;

public class RateLimitSettings
{
    public const string SectionName = "RateLimiting";
    public int PermitLimit { get; init; }
    public string Window { get; init; }
    public string QueueLimit { get; init; }
}
