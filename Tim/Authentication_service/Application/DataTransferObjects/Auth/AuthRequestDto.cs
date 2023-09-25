namespace Application.DataTransferObjects.Auth;

public class AuthRequestDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}