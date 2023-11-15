using Application.Registrations._Interfaces;

namespace Application.AppServices.V1._Interfaces;

public interface ICharacterServiceV1 : IAppService
{
    Task CreateCharacter();
    Task ListCharacters();
    Task UpdateCharacter();
    Task DeleteCharacter();
}