using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context) => _context = context;

    public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<(List<TaskItem> Items, int TotalCount)> GetPagedAsync(
        Guid? ownerId, int page, int pageSize, string? search,
        WorkStatus? status, Priority? priority, CancellationToken ct = default)
    {
        var query = _context.TaskItems.AsQueryable();

        if (ownerId.HasValue)
            query = query.Where(t => t.OwnerId == ownerId || t.AssignedToUserId == ownerId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));

        if (status.HasValue)
            query = query.Where(t => t.Status == status);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(t => t.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task AddAsync(TaskItem task, CancellationToken ct = default)
    {
        _context.TaskItems.Add(task);
        return Task.CompletedTask;
    }

    public void Update(TaskItem task) => _context.TaskItems.Update(task);

    public void Remove(TaskItem task) => _context.TaskItems.Remove(task);

    public async Task<bool> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct) >= 0;
}
