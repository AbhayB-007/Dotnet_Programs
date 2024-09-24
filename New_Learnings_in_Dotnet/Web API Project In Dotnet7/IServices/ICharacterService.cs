using WebAPIProject_In_Dotnet_7.DTOs.Character;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.IServices
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);

    }
}
