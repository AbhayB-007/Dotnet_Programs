using System;
using System.Threading;
using System.Threading.Tasks;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Tasks;

namespace TaskFlow.Application.Interfaces;

public interface ITaskService
{
    Task<PagedResult<TaskItemDto>> GetTasksAsync(TaskQueryParams query, CancellationToken ct = default);
    Task<TaskItemDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TaskItemDto> CreateAsync(CreateTaskRequest request, CancellationToken ct = default);
    Task<TaskItemDto?> UpdateAsync(Guid id, UpdateTaskRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
