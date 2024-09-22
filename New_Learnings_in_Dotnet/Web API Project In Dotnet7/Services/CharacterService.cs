using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Services
{
    public class CharacterService : ICharacterService
    {
        private static readonly List<Character> Characters = new List<Character>
        {
            new Character (),
            new Character { Id = 1,  Name = "Sam" }
        };

        public Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<Character>>();
            Characters.Add(newCharacter);
            serviceResponse.Data = Characters;
            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<List<Character>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<Character>>
            {
                Data = Characters
            };
            return Task.FromResult(serviceResponse);
        }


        public Task<ServiceResponse<Character>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<Character>();
            var character = Characters.FirstOrDefault(x => x.Id == id);
            serviceResponse.Data = character;
            return Task.FromResult(serviceResponse);
        }
    }
}
