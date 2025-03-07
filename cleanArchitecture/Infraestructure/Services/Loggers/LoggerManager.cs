namespace Infrastructure.Services.Loggers;

/// <summary>
/// Logger Manager service.
/// </summary> 
/// <param name="configuration"></param>
/// <param name="options"></param>
public class LoggerManager(IConfiguration configuration, IOptions<ConnectionSettings> options) : SerilogService(configuration, options), ILoggerService
{
    /// <summary>
    /// Logger method
    /// </summary>
    /// <param name="eventLevel"></param>
    /// <param name="template"></param>
    /// <param name="ex"></param>
    /// <param name="logTypes"></param>
    /// <param name="arg"></param>
    public void LogWrite(
        LogLevels eventLevel,
        string template,
        Exception ex = null,
        LogTypes logTypes = LogTypes.File,
        params object[] arg) => ServiceLogWrite(eventLevel, logTypes, template, ex, arg);
}
