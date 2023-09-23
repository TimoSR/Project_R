namespace Application.DataTransferObjects.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiryTime { get; set; }
}