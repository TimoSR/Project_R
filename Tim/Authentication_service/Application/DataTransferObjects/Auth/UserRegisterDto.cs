using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Auth;

public class UserRegisterDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}