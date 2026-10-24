using AutoMapper;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Tasks;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

// Application-layer service: orchestrates repositories + mapping + authorization rules
// (who owns what) but has zero knowledge of EF Core, HTTP, or SQL Server.
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public TaskService(
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<PagedResult<TaskItemDto>> GetTasksAsync(TaskQueryParams query, CancellationToken ct = default)
    {
        // Admins see every task; normal users only ever see their own.
        Guid? ownerFilter = _currentUser.IsAdmin ? null : _currentUser.UserId;

        var (items, total) = await _taskRepository.GetPagedAsync(
            ownerFilter, query.Page, query.PageSize, query.Search, query.Status, query.Priority, ct);

        var dtos = await MapWithNamesAsync(items, ct);

        return new PagedResult<TaskItemDto>
        {
            Items = dtos,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, ct);
        if (task is null) return null;
        if (!CanAccess(task)) return null;

        var dtos = await MapWithNamesAsync(new List<TaskItem> { task }, ct);
        return dtos.First();
    }

    public async Task<TaskItemDto> CreateAsync(CreateTaskRequest request, CancellationToken ct = default)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDateUtc = request.DueDateUtc,
            OwnerId = _currentUser.UserId!.Value,
            // Only admins are allowed to assign tasks to someone else.
            AssignedToUserId = _currentUser.IsAdmin ? request.AssignedToUserId : null
        };

        await _taskRepository.AddAsync(task, ct);
        await _taskRepository.SaveChangesAsync(ct);

        var dtos = await MapWithNamesAsync(new List<TaskItem> { task }, ct);
        return dtos.First();
    }

    public async Task<TaskItemDto?> UpdateAsync(Guid id, UpdateTaskRequest request, CancellationToken ct = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, ct);
        if (task is null || !CanAccess(task)) return null;

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.DueDateUtc = request.DueDateUtc;
        task.UpdatedAtUtc = DateTime.UtcNow;

        if (_currentUser.IsAdmin)
            task.AssignedToUserId = request.AssignedToUserId;

        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync(ct);

        var dtos = await MapWithNamesAsync(new List<TaskItem> { task }, ct);
        return dtos.First();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, ct);
        if (task is null || !CanAccess(task)) return false;

        task.IsDeleted = true; // soft delete keeps an audit trail
        _taskRepository.Update(task);
        return await _taskRepository.SaveChangesAsync(ct);
    }

    private bool CanAccess(TaskItem task) =>
        _currentUser.IsAdmin || task.OwnerId == _currentUser.UserId;

    private async Task<List<TaskItemDto>> MapWithNamesAsync(List<TaskItem> tasks, CancellationToken ct)
    {
        var userIds = tasks.Select(t => t.OwnerId)
            .Concat(tasks.Where(t => t.AssignedToUserId.HasValue).Select(t => t.AssignedToUserId!.Value))
            .Distinct();

        var names = await _userRepository.GetNamesByIdsAsync(userIds, ct);

        return tasks.Select(t => new TaskItemDto(
            t.Id, t.Title, t.Description, t.Status, t.Priority, t.DueDateUtc,
            t.OwnerId, names.GetValueOrDefault(t.OwnerId),
            t.AssignedToUserId, t.AssignedToUserId.HasValue ? names.GetValueOrDefault(t.AssignedToUserId.Value) : null,
            t.CreatedAtUtc, t.UpdatedAtUtc
        )).ToList();
    }
}
