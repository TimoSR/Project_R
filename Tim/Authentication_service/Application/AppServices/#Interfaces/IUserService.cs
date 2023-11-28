using _SharedKernel.Patterns.RegistrationHooks.Services._Interfaces;
using _SharedKernel.Patterns.ResultPattern;
using Application.DTO.Auth;
using Application.DTO.UserManagement;

namespace Application.AppServices._Interfaces;

public interface IUserService : IAppService
{
    Task<ServiceResult> RegisterAsync(UserRegisterDto userDto);
    Task<ServiceResult<UserDto>> GetUserByIdAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}