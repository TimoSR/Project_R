using _SharedKernel.Patterns.ResultPattern;
using Application.AppServices._Interfaces;
using Application.Controllers.GraphQL.GraphQL._Interfaces;
using Application.DTO.UserManagement;

namespace Application.Controllers.GraphQL.GraphQL.ProductCollection;

public class ProductMutation : IMutation
{
    private readonly IUserService _userService;
    
    public ProductMutation(IUserService userService)
    {
        _userService = userService;
    }
    
    // Assuming UserRegisterDto and User are your domain models
    public async Task<ServiceResult> RegisterUser(UserRegisterDto newUserDto)
    {
        var result = await _userService.RegisterAsync(newUserDto);

        if (result.IsSuccess)
        {
            return result;
        }

        return result;
    }
}