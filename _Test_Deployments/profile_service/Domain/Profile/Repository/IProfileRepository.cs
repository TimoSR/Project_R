namespace Domain.Profile.Repository;

public interface IProfileRepository
{
    Task CreateProfileAsync(Entities.Profile profile);
    Task<Entities.Profile> GetProfileInfoAsync(string userId);
    Task<bool> UpdateProfileAsync(Entities.Profile profile);
    Task<bool> DeleteProfileAsync(string userId);
}