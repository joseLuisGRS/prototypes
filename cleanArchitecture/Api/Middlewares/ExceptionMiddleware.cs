namespace Api.Middlewares;

/// <summary>
/// Custom middleware for handling exceptions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
/// </remarks>
/// <param name="next">The next middleware in the pipeline.</param>
/// <param name="env">The current hosting environment.</param>
/// <param name="loggerService">The logger service for logging errors.</param>
/// <param name="logger">The logger for logging errors.</param>
public class ExceptionMiddleware(RequestDelegate next,
    IHostEnvironment env,
    ILoggerService loggerService,
    ILogger<ExceptionMiddleware> logger)
{


    /// <summary>
    /// Processes the HTTP request.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the current request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (AccessViolationException avEx)
        {
            logger.LogError("A new violation exception has been thrown: {message}", avEx.Message);
            await HandlerExceptionAsync(httpContext, avEx);
        }
        catch (NotImplementedException imEx)
        {
            logger.LogError("Some method isn't implemented yet: {message}", imEx.Message);
            await HandlerExceptionAsync(httpContext, imEx);
        }
        catch (CustomException customEx)
        {
            logger.LogError("Something went wrong: {message}", customEx.Message);

            loggerService.LogWrite(
                LogLevels.Error,
                "Project {source}, Severity Type {severity} : StackTrace {customEx}",
                ex: customEx,
                logTypes: LogTypes.File,
                arg: new object[] { customEx.Sources.DescriptionAttr(),
                               customEx.Severity.DescriptionAttr(),
                               customEx.StackTrace });
            await HandlerExceptionAsync(httpContext, customEx, (HttpStatusCode)customEx.StatusCode);
        }
        catch (SqlException sqlEx)
        {
            logger.LogError("Something went wrong: {message}", sqlEx.Message);

            loggerService.LogWrite(
                LogLevels.Error,
                "Project {source}, Severity Type {severity} : StackTrace {customEx}",
                ex: sqlEx,
                logTypes: LogTypes.File,
                arg: new object[] { sqlEx,
                               sqlEx.InnerException,
                               sqlEx.Message });
            await HandlerExceptionAsync(httpContext, sqlEx);
        }
        catch (Exception ex)
        {
            logger.LogError("Something went wrong: {message}", ex.Message);
            await HandlerExceptionAsync(httpContext, ex);
        }
    }

    /// <summary>
    /// Handles exceptions and sends an appropriate response to the client.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the current request.</param>
    /// <param name="ex">The exception that occurred.</param>
    /// <param name="statusCode">Number StatusCode</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandlerExceptionAsync(HttpContext httpContext, Exception ex, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)statusCode;

        //if you want control of classification errors in the API response, 
        // do you only change or add an option on the ex switch
        var message = ex switch
        {
            CustomException => ex.Message,
            AccessViolationException => "Access violation error from the custom middleware",
            NotImplementedException => "Some method isn't implemented yet",
            _ => "Internal Server Error from the custom middleware, check the log to see more details"
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var properties = (new
        {
            Success = false,
            Message = env.IsDevelopment() ? ex.Message : message,
            Data = default(object)
        });
        var json = JsonSerializer.Serialize(properties, options);
        await httpContext.Response.WriteAsync(json);
    }
}
