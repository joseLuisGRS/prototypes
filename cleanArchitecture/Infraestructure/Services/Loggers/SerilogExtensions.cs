namespace Infrastructure.Services.Loggers;

/// <summary>
/// class to add LoggerConfiguration 
/// </summary>
public static class SerilogExtensions
{

    /// <summary>
    /// Console & Enrich configuration
    /// </summary>
    /// <param name="loggerConfiguration"></param>
    /// <param name="appName"></param>
    /// <returns></returns>
    public static LoggerConfiguration ConfigureBaseLogging(this LoggerConfiguration loggerConfiguration, string appName)
    {
        loggerConfiguration
            .MinimumLevel.Debug()
            .WriteTo.Async(a => a.Console(theme: AnsiConsoleTheme.Code))
            .Enrich.FromLogContext()
            .Enrich.WithProcessName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("ApplicationName", appName);

        return loggerConfiguration;
    }

}
