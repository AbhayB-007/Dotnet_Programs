using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPIProject_In_Dotnet_7.Data;
using WebAPIProject_In_Dotnet_7.DTOs.Character;
using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Services;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// Get All Characters
    /// </summary>
    /// <returns></returns>
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
    {
        var dbCharacters = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters)
        };
        return serviceResponse;
    }

    /// <summary>
    /// Get specific id from the list
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters.FindAsync(id);
            if (character is null)
                throw new Exception($"Character with Id '{id}' Not Found!");

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        return serviceResponse;
    }

    /// <summary>
    /// Add newCharacter in the list
    /// </summary>
    /// <param name="newCharacter"></param>
    /// <returns></returns>
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        try
        {
            var character = _mapper.Map<Character>(newCharacter);
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToListAsync();
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    /// <summary>
    /// Update a specific id from the list
    /// </summary>
    /// <param name="updatedCharacter"></param>
    /// <returns></returns>
    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters.FindAsync(updatedCharacter.Id);
            if (character is null)
                throw new Exception($"Character with Id '{updatedCharacter.Id}' Not Found!");

            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.Strength;
            character.Defense = updatedCharacter.Defense;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Class = updatedCharacter.Class;

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

    /// <summary>
    /// Delete a specific id from the list
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        try
        {
            var character = await _context.Characters.FindAsync(id);
            if (character is null)
                throw new Exception($"Character with Id '{id}' Not Found!");

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Select(x => _mapper.Map<GetCharacterDto>(x)).ToListAsync();
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }
}