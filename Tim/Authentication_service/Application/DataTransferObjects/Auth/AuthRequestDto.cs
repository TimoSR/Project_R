namespace Application.DataTransferObjects.Auth;

public class AuthRequestDto
{
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}