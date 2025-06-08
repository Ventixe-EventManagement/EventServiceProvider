namespace Business.Models;
public class EventResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; } = 200;
    public static EventResult CreateSuccess(int statusCode = 200) => new()
    {
        Success = true,
        StatusCode = statusCode
    };

    public static EventResult CreateFailure(string error, int statusCode = 400) => new()
    {
        Success = false,
        Error = error,
        StatusCode = statusCode
    };
}

public class EventResult<T> : EventResult
{
    public T? Result { get; set; }

    public static EventResult<T> CreateSuccess(T result, int statusCode = 200) => new()
    {
        Success = true,
        StatusCode = statusCode,
        Result = result
    };

    public new static EventResult<T> CreateFailure(string error, int statusCode = 400) => new()
    {
        Success = false,
        Error = error,
        StatusCode = statusCode
    };
}
