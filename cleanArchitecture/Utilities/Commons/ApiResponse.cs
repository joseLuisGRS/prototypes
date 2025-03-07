namespace Utilities.Commons;

/// <summary>
/// This a generic response that is usaded in the Response Api, it's can have any data
/// </summary>
/// <typeparam name="T">Represent any response class</typeparam>
/// <value name="Success">Returns true on successful completion or false otherwise </value>
/// <value name="Message">Returns a message based on the success value </value>
/// <value name="Data">Returns a generic data when is Success or empty data otherwise </value>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}
