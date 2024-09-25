using AutoMapper;
using WebAPIProject_In_Dotnet_7.DTOs.Character;
using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Services;

public class CharacterService : ICharacterService
{
    private static readonly List<Character> Characters = new()
    {
        new(),
        new() { Id = 1, Name = "Sam" }
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
        try
        {
            var character = Characters.FirstOrDefault(x => x.Id == id);
            if (character is null)
                throw new Exception($"Character with Id '{id}' Not Found!");

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return Task.FromResult(serviceResponse);
    }

    public Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = Characters.FirstOrDefault(x => x.Id == updatedCharacter.Id);
            if (character != null)
            {
                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;
            }
            else
                throw new Exception($"Character with Id '{updatedCharacter.Id}' Not Found!");

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return Task.FromResult(serviceResponse);
    }
}