using _CommonLibrary.Patterns._Interfaces;

namespace _CommonLibrary.Patterns.ResultPattern;

public class ServiceResult : IServiceResult
{
    public bool IsSuccess { get; protected set; }
    public string? Message { get; protected set; }
    
    /// <summary>
    /// Static factory methods are used for creating instances of ServiceResult;.
    /// This approach provides clear, meaningful construction logic (e.g., Success, Failure),
    /// ensures immutability by controlling instance initialization, and offers flexibility for
    /// future changes or extensions in object creation.
    /// </summary>

    public static ServiceResult Success(string? message)
    {
        return new ServiceResult { IsSuccess = true, Message = message };
    }

    public static ServiceResult Failure(string message)
    {
        return new ServiceResult { IsSuccess = false, Message = message };
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T Data { get; private set; }

    private ServiceResult(T data, string? message, bool isSuccess) : base()
    {
        Data = data;
        Message = message;
        IsSuccess = isSuccess;
    }

    public static ServiceResult<T> Success(T data, string? message)
    {
        return new ServiceResult<T>(data, message, true);
    }

    public new static ServiceResult<T?> Failure(string? message)
    {
        return new ServiceResult<T?>(default(T), message, false);
    }
}