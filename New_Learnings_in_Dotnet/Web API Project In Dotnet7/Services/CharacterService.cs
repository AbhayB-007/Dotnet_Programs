using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Services
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character (),
            new Character { Id = 1,  Name = "Sam" }
        };

        public List<Character> AddCharacter(Character newCharacter)
        {
            characters.Add(newCharacter);
            return characters;
        }

        public List<Character> GetAllCharacters() => characters;


        public Character GetCharacterById(int id)
        {
            var character = characters.FirstOrDefault(x => x.Id == id);
            if (character is not null)
                return character;
            throw new Exception("Character not found");
            //return characters?.FirstOrDefault(x => x.Id == id);
        }
    }
}
