using Application.Registrations._Interfaces;
using Domain.Profile.Entities;

namespace Application.AppServices.V1._Interfaces;

public interface IProfileServiceV1 : IAppService
{
    Task CreateProfileAsync(Profile profile);
    Task<Profile> GetProfileInfoAsync(string userId);
    Task UpdateProfileAsync(Profile profile);
    Task DeleteProfileAsync(string userId);
}