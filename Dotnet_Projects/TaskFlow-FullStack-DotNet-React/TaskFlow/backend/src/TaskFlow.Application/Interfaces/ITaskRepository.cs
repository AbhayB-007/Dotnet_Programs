using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

// Repository abstraction lives in Application; implementation lives in Infrastructure.
// This is what lets us swap EF Core for Dapper (or anything else) later without
// touching business logic — the classic "ports and adapters" idea from Clean Architecture.
public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(List<TaskItem> Items, int TotalCount)> GetPagedAsync(
        Guid? ownerId, int page, int pageSize, string? search,
        Domain.Enums.WorkStatus? status, Domain.Enums.Priority? priority, CancellationToken ct = default);
    Task AddAsync(TaskItem task, CancellationToken ct = default);
    void Update(TaskItem task);
    void Remove(TaskItem task);
    Task<bool> SaveChangesAsync(CancellationToken ct = default);
}
