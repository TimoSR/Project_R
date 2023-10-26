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
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
    }

    public async Task CreateProfileAsync(Profile profile)
    {
        if (profile == null)
        {
            _logger.LogError("Profile object cannot be null.");
            throw new ArgumentNullException(nameof(profile));
        }

        _logger.LogInformation("Attempting to create a new profile for user ID: {UserId}", profile.UserId);

        try
        {
            await _profileRepository.CreateProfileAsync(profile);
            _logger.LogInformation("Successfully created a new profile for user ID: {UserId}", profile.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to create a new profile: {ex.Message}");
            throw; // Rethrow the exception to the caller
        }
    }

    public async Task<Profile> GetProfileInfoAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError("UserId cannot be null or empty.");
            throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
        }

        _logger.LogInformation("Attempting to fetch the profile for user ID: {UserId}", userId);

        try
        {
            var profile = await _profileRepository.GetProfileInfoAsync(userId);
            _logger.LogInformation("Successfully fetched the profile for user ID: {UserId}", userId);
            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to fetch the profile: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateProfileAsync(Profile profile)
    {
        if (profile == null)
        {
            _logger.LogError("Profile object cannot be null.");
            throw new ArgumentNullException(nameof(profile));
        }

        _logger.LogInformation("Attempting to update the profile for user ID: {UserId}", profile.UserId);

        try
        {
            await _profileRepository.UpdateProfileAsync(profile);
            _logger.LogInformation("Successfully updated the profile for user ID: {UserId}", profile.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to update the profile: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteProfileAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError("UserId cannot be null or empty.");
            throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
        }

        _logger.LogInformation("Attempting to delete the profile for user ID: {UserId}", userId);

        try
        {
            await _profileRepository.DeleteProfileAsync(userId);
            _logger.LogInformation("Successfully deleted the profile for user ID: {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to delete the profile: {ex.Message}");
            throw;
        }
    }
}
