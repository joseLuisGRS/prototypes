using System.ComponentModel;

namespace Utilities.Helpers;

/// <summary>
/// This class has the function of handling exceptions.
/// </summary>
[Serializable]
public class CustomException : Exception
{
    public SourceType Sources
    {
        get;
        internal set;
    }

    public SeverityType Severity
    {
        get;
        internal set;
    }

    public EHttpStatusCode StatusCode
    {
        get;
        internal set;
    }

    public CustomException(
        SourceType source,
        SeverityType severity,
        EHttpStatusCode statusCode,
        string errorDescription) : base(errorDescription)
    {
        Sources = source;
        Severity = severity;
        StatusCode = statusCode;
    }

    public CustomException()
    {

    }
}

public enum SourceType
{
    [Description("Application")]
    Application,
    [Description("Infrastructure")]
    Infrastructure,
    [Description("Domain")]
    Domain,
    [Description("Utilities")]
    Utilities
}

public enum SeverityType
{
    [Description("Warning")]
    Warning,
    [Description("Error")]
    Error
}

public enum EHttpStatusCode
{
    OK = 200,
    Created = 201,
    NoContent = 204,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500
}