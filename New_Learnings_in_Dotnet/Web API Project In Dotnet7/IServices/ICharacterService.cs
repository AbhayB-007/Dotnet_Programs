using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.IServices
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<Character>>> GetAllCharacters();
        Task<ServiceResponse<Character>> GetCharacterById(int id);
        Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter);

    }
}
