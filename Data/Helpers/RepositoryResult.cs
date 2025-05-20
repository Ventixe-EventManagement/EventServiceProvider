namespace Data.Helpers;

public class RepositoryResult<T>
{
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
    public T? Result { get; set; }

    public static RepositoryResult<T> Success(T result, int statusCode = 200) => new()
    {
        Succeeded = true,
        Result = result,
        StatusCode = statusCode
    };

    public static RepositoryResult<T> Failure(string error, int statusCode = 400) => new()
    {
        Succeeded = false,
        Error = error,
        StatusCode = statusCode
    };
}
