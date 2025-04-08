namespace Api.Middlewares;

/// <summary>
/// Custom middleware for handling authentication
/// </summary>
/// <param name="next">The next middleware in the pipeline.</param>
/// <param name="jwtTokenService">The jwtTokenService provides the methods used for operations in the middleware.</param>
/// <param name="logger">The logger for logging errors.</param>
public class TokenRevocationMiddleware(RequestDelegate next, IJwtTokenService jwtTokenService, ILogger<TokenRevocationMiddleware> logger)
{

    /// <summary>
    /// Processes the HTTP request and validates that it contains a valid authorization.
    /// </summary>
    /// <param name="context">The context for the current request.</param>
    /// <returns>Returns Status401Unauthorized when the request is invalid, otherwise it continues processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token) && await jwtTokenService.IsTokenRevokedAsync(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            context.Response.ContentType = "application/json";

            var errorResponse = ApiResponse<string>.ErrorResponse("Invalid access.");
            var json = JsonSerializer.Serialize(errorResponse);
            var bytes = Encoding.UTF8.GetBytes(json);

            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);

            logger.LogError("The token was revoked, it cannot be used: {token}", token);
            return;

        }

        await next(context);
    }

}
