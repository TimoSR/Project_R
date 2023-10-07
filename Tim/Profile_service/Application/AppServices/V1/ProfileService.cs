using Application.AppServices.V1._Interfaces;
using Domain.Profile.Entities;
using Domain.Profile.Repository;

namespace Application.AppServices.V1;

public class ProfileService : IProfileServiceV1
{
    private readonly ILogger<ProfileService> _logger;
    private readonly IProfileRepository _profileRepository;  
    
    public ProfileService(
        ILogger<ProfileService> logger,
        IProfileRepository profileRepository)
    {
        _logger = logger;
        _profileRepository = profileRepository;
    }
    
    public Task CreateProfile(Profile profile)
    {
        return _profileRepository.CreateProfileAsync(profile);
    }

    public Task GetProfileInfo(string userId)
    {
        return _profileRepository.GetProfileInfoAsync(userId);
    }

    public Task UpdateProfile()
    {
        return _profileRepository.UpdateProfileAsync()
    }

    public Task DeleteProfile()
    {
        throw new NotImplementedException();
    }
}