namespace Application.Interfaces.Services;

/// <summary>
/// This interface is a functionality for log records.
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// This method is responsible for recording logs.
    /// </summary>
    /// <param name="eventLevel">Severity level in logs. </param>
    /// <param name="template">Template used for registration. </param>
    /// <param name="ex">Type of exception received. </param>
    /// <param name="logTypes">Log type used for registration. </param>
    /// <param name="arg">Object that specifies the details of the exception. </param>
    void LogWrite(LogLevels eventLevel,
        string template,
        Exception ex = null,
        LogTypes logTypes = LogTypes.File,
        params object[] arg);
}
