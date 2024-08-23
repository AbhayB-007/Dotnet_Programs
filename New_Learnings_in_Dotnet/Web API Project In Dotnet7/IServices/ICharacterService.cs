using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.IServices
{
    public interface ICharacterService
    {
        List<Character> GetAllCharacters();
        Character GetCharacterById(int id);
        List<Character> AddCharacter(Character newCharacter);

    }
}
