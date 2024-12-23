namespace SimCorp.Coding.Triangles;

public abstract class Result
{
    public bool IsSucceed { get; init; }

    public string? Error { get; init; }

    public static Result<T> Succeed<T>(T value)
    {
        return new Result<T>
        {
            Value     = value,
            IsSucceed = true,
            Error     = null
        };
    }

    public static Result<T> Failed<T>(string? error)
    {
        return new Result<T>()
        {
            Value     = default!,
            Error     = error,
            IsSucceed = false
        };
    }

    public static Result<T> Failed<T>(Exception exception)
    {
        return Failed<T>(exception.Message);
    }
}

public class Result<T> : Result
{
    public T? Value { get; init; }
}