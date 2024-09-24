using AutoMapper;
using WebAPIProject_In_Dotnet_7.DTOs.Character;
using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Services
{
    public class CharacterService : ICharacterService
    {
        private static readonly List<Character> Characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Sam" }
        };

        private readonly IMapper _mapper;
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = Characters.Max(x => x.Id) + 1;
            Characters.Add(character);
            serviceResponse.Data = Characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToList();
            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = _mapper.Map<List<GetCharacterDto>>(Characters)
            };
            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = Characters.FirstOrDefault(x => x.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return Task.FromResult(serviceResponse);
        }
    }
}