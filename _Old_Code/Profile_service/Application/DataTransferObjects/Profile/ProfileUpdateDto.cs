namespace Application.DataTransferObjects.Profile;

public class ProfileUpdateDto
{
    public string DisplayName { get; set; }
    public DateTime? LastLoginDate { get; } = DateTime.UtcNow;
}