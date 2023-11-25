using _CommonLibrary.Patterns._Interfaces;
using _CommonLibrary.Patterns.ResultPattern._Enums;

namespace _CommonLibrary.Patterns.ResultPattern;

public class ServiceResult : IServiceResult
{
    public bool IsSuccess { get; protected set; }
    public string? Message { get; protected set; }
    public ServiceErrorType ErrorType { get; protected set; } = ServiceErrorType.None;

    // Factory methods with ErrorType
    public static ServiceResult Success(string? message)
    {
        return new ServiceResult { IsSuccess = true, Message = message };
    }

    public static ServiceResult Failure(string message, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return new ServiceResult { IsSuccess = false, Message = message, ErrorType = errorType };
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T Data { get; private set; }

    private ServiceResult(T data, string? message, bool isSuccess, ServiceErrorType errorType = ServiceErrorType.None): base()
    {
        Data = data;
        Message = message;
        IsSuccess = isSuccess;
        ErrorType = errorType;
    }

    public static ServiceResult<T> Success(T data, string? message)
    {
        return new ServiceResult<T>(data, message, true);
    }

    public new static ServiceResult<T?> Failure(string? message, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return new ServiceResult<T?>(default(T), message, false, errorType);
    }
}
