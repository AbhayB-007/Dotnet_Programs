using AutoMapper;
using WebAPIProject_In_Dotnet_7.DTOs.Character;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}
