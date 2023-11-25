using _CommonLibrary.Patterns.RegistrationHooks.Services._Interfaces;
using _CommonLibrary.Patterns.ResultPattern;
using Application.DTO.Auth;

namespace Application.AppServices._Interfaces;

public interface IUserService : IAppService
{
    Task<ServiceResult> RegisterAsync(UserRegisterDto userDto);
    Task<bool> DeleteUserAsync(string userId);
}