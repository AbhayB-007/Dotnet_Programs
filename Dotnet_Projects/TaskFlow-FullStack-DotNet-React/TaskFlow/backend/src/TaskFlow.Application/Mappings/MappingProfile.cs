using AutoMapper;
using TaskFlow.Application.DTOs.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskItem, TaskItemDto>()
            .ForCtorParam(nameof(TaskItemDto.OwnerName), opt => opt.MapFrom(_ => (string?)null))
            .ForCtorParam(nameof(TaskItemDto.AssignedToName), opt => opt.MapFrom(_ => (string?)null));
    }
}
