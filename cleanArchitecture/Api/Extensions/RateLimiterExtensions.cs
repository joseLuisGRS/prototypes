using Microsoft.AspNetCore.RateLimiting;

namespace Api.Extensions;

/// <summary>
/// extension methods for rate limiting.
/// </summary>
public static class RateLimiterExtensions
{
    const string fixedWindowPolicy = "fixedWindow";

    /// <summary>
    /// Adds rate limiting functionality to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration for rate limiting settings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRatelimiting(
        this IServiceCollection services, IConfiguration configuration)
    {
        RateLimitSettings rateLimitSettings = configuration.GetSection(RateLimitSettings.SectionName).Get<RateLimitSettings>();

        services.AddRateLimiter(configureOptions =>
        {
            configureOptions.AddFixedWindowLimiter(
                policyName: fixedWindowPolicy, fixedWindow =>
                {
                    fixedWindow.PermitLimit = rateLimitSettings.PermitLimit;
                    fixedWindow.Window = TimeSpan.FromSeconds(Convert.ToDouble(rateLimitSettings.Window));
                    fixedWindow.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    fixedWindow.QueueLimit = int.Parse(rateLimitSettings.QueueLimit);
                });
            configureOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }

}
