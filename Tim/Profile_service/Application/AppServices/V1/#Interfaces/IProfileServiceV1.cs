using Application.Registrations._Interfaces;
using Domain.Profile.Entities;

namespace Application.AppServices.V1._Interfaces;

public interface IProfileServiceV1 : IAppService
{
    Task CreateProfile(Profile profile);
    Task GetProfileInfo(string userId);
    Task UpdateProfile();
    Task DeleteProfile();
}