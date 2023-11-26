using _CommonLibrary.Patterns.ResultPattern;

namespace _CommonLibrary.Patterns._Interfaces;

public interface IServiceResult
{
    bool IsSuccess { get; }
    string? Message { get; }
}