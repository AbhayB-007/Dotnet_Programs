using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.Tasks;

public record TaskItemDto(
    Guid Id,
    string Title,
    string? Description,
    WorkStatus Status,
    Priority Priority,
    DateTime? DueDateUtc,
    Guid OwnerId,
    string? OwnerName,
    Guid? AssignedToUserId,
    string? AssignedToName,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);

public record CreateTaskRequest(
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDateUtc,
    Guid? AssignedToUserId);

public record UpdateTaskRequest(
    string Title,
    string? Description,
    WorkStatus Status,
    Priority Priority,
    DateTime? DueDateUtc,
    Guid? AssignedToUserId);

public record TaskQueryParams(
    int Page = 1,
    int PageSize = 10,
    WorkStatus? Status = null,
    Priority? Priority = null,
    string? Search = null);
