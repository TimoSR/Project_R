using _CommonLibrary.Patterns._Enums;
using _CommonLibrary.Patterns._Interfaces;

namespace _CommonLibrary.Patterns.ResultPattern;

public class ValidationResult : IServiceResult
{
    public bool IsSuccess { get; protected set; }
    public string? Message { get; protected set; }
    
    public static ValidationResult Success(string? message = null)
    {
        return new ValidationResult { IsSuccess = true, Message = message };
    }

    public static ValidationResult Failure(string message, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return new ValidationResult { IsSuccess = false, Message = message};
    }
}