namespace Infrastructure.Services.Loggers;

/// <summary>
/// Provides methods used for logging.
/// </summary>
public class SerilogService
{
    private static string _customLoggingLevel { get; set; }

    /// <summary>
    /// Logger manager service
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="options"></param>
    public SerilogService(IConfiguration configuration, IOptions<ConnectionSettings> options)
    {
        _customLoggingLevel = configuration.GetSection("CustomLoggingLevel").Value;
    }

    /// <summary>
    /// Logger method
    /// </summary>
    /// <param name="eventLevel"></param>
    /// <param name="logTypes"></param>
    /// <param name="template"></param>
    /// <param name="ex"></param>
    /// <param name="arg"></param>
    public static void ServiceLogWrite(
        LogLevels eventLevel,
        LogTypes logTypes,
        string template,
        Exception ex = null,
        params object[] arg)
    {
        //Log in console to trace any Serilog issues
        Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));

        var levelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = GetLogEventLevel(_customLoggingLevel)
        };

        var logLevel = GetLogEventLevel(eventLevel.ToString());

        using (var log = GetLoggerConfiguration(levelSwitch, logTypes))
        {
            if (log.IsEnabled(logLevel))
            {
                if (ex is not null)
                    log.Write(logLevel, ex, template, arg);
                else
                    log.Write(logLevel, template, arg);
            }
            else
            {
                var result = $"Logger: eventLevel:{nameof(logLevel)} is not enabled";
                System.Diagnostics.Debug.WriteLine(result);
            }
        }
    }

    /// <summary>
    /// Get Logger Configuration
    /// </summary>
    /// <param name="levelSwitch">Dynamically controls logging level</param>
    /// <param name="logTypes">Enum LogTypes</param>
    /// <returns> Serilog.Core.Logger</returns>
    public static Logger GetLoggerConfiguration(LoggingLevelSwitch levelSwitch, LogTypes logTypes)
    {
        switch (logTypes)
        {
            case LogTypes.File:
                return new LoggerConfiguration()
                        .MinimumLevel.ControlledBy(levelSwitch)
                        .WriteTo.File($"{Directory.GetCurrentDirectory()}/Logs/log.txt",
                                        rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true,
                                        fileSizeLimitBytes: 1000000,
                                        shared: true)
                        .Enrich.FromLogContext()
                        .CreateLogger();
            default:
                return new LoggerConfiguration()
                       .MinimumLevel.ControlledBy(levelSwitch)
                       .WriteTo.File($"{Directory.GetCurrentDirectory()}/Logs/log.txt",
                                       rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true,
                                       fileSizeLimitBytes: 1000000,
                                       shared: true)
                       .Enrich.FromLogContext()
                       .CreateLogger();
        }
    }

    /// <summary>
    /// Gets correspondent Serilog Log Event Level
    /// </summary>
    /// <param name="logLevel">name of the log level</param>
    /// <returns>LogEventLevel</returns>
    public static LogEventLevel GetLogEventLevel(string logLevel)
    {
        var level = LogEventLevel.Debug;
        try
        {
            level = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), logLevel);
        }
        catch (Exception ex)
        {
            var result = $"LoggerManager.getLogEventLevel(): exception getting customlogging level, defaulted to Debug Level. logLevel:{logLevel}, exception:{ex}";
            System.Diagnostics.Debug.WriteLine(result);
        }
        return level;
    }

}
